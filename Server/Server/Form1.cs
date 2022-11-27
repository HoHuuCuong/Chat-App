using System.Net.Sockets;
using System.Net;
using MaterialSkin.Controls;
using MaterialSkin;
using Microsoft.VisualBasic.Logging;
using System.Text.Json;
using System.Windows.Forms;
using GOI;
using DoAn.DAL;
using DoAn.DTO;
using Server.DAL;
using System.Drawing.Imaging;
using System.IO;

namespace Server
{
    public partial class Form1 : MaterialForm
    {
        readonly MaterialSkin.MaterialSkinManager manager;
        IPEndPoint iep;
        TcpListener server;
        Dictionary<string, string> DS;
        Dictionary<string, List<string>> DSNhom;
        Dictionary<string, TcpClient> DSClient;

        bool active = false;
        //*DAL*//
        private UserDAL userdal;
        private MessageDAL mesdal;
        public Form1()
        {
            InitializeComponent();
            userdal = new UserDAL();
            mesdal = new MessageDAL();
            DS = new Dictionary<string, string>();
            DSClient = new Dictionary<string, TcpClient>();
            DSNhom = new Dictionary<string, List<string>>();
            //CODE UI//
            manager = MaterialSkin.MaterialSkinManager.Instance;
            manager.EnforceBackcolorOnAllComponents = true;
            manager.AddFormToManage(this);
            manager.Theme = MaterialSkin.MaterialSkinManager.Themes.LIGHT;
            manager.ColorScheme = new MaterialSkin.ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.LightBlue200, TextShade.WHITE);
        }

        private string getthisPCip()
        {
            string hostName = Dns.GetHostName();
            IPAddress[] IPlist = Dns.GetHostByName(hostName).AddressList;
            foreach(var ip in IPlist)
            {
                if (ip.ToString().Contains("192.168.")) return ip.ToString();
            }
            return "";
        }
        private void btnstart_Click(object sender, EventArgs e)
        {
            active = true;
            iep = new IPEndPoint(IPAddress.Parse(getthisPCip()), 2008);
            server = new TcpListener(iep);
            server.Start();
            //Console.WriteLine("Cho  ket  noi  tu  client");
            txtstatus.Text += "Cho  ket  noi  tu  client" + Environment.NewLine;


            Thread trd = new Thread(new ThreadStart(this.ThreadTask));
            trd.IsBackground = true;
            trd.Start();
        }

        private void AppendTextBox(string s)
        {
            txtstatus.BeginInvoke(new MethodInvoker(() =>
            {
                txtstatus.Text = txtstatus.Text + Environment.NewLine + s;
            }));
        }

        private static byte[] ImageToByte(Image iImage)
        {
            MemoryStream mMemoryStream = new MemoryStream();
            iImage.Save(mMemoryStream, ImageFormat.Png);
            return mMemoryStream.ToArray();
        }

        private void ThreadTask()
        {
            while (active)
            {
                try
                {
                    TcpClient client = server.AcceptTcpClient();
                    AppendTextBox("Da co 1 client ket noi");
                    var t = new Thread(() => ThreadClient(client));
                    t.Start();
                }
                catch (Exception)
                {
                    active = false;
                }

            }
        }

        private void sendJson(TcpClient client, object obj)
        {
            string jsonStringgui = JsonSerializer.Serialize(obj);
            StreamWriter sw = new StreamWriter(client.GetStream());
            //client.Send(jsonUtf8Bytes, jsonUtf8Bytes.Length, SocketFlags.None);
            sw.WriteLine(jsonStringgui);
            sw.Flush();
        }

        private void ThreadClient(TcpClient client)
        {
            StreamReader sr = new StreamReader(client.GetStream());
            StreamWriter sw = new StreamWriter(client.GetStream());
            string jsonString = sr.ReadLine();

            GOI.THUONG? com = JsonSerializer.Deserialize<GOI.THUONG>(jsonString);
            if (com != null)
            {
                if (com.content != null)
                {
                    switch (com.kind)
                    {
                        case "dangnhap":
                            {
                                LOGIN? login = JsonSerializer.Deserialize<LOGIN>(com.content);
                                if(login != null && login.username != null && login.pass != null)
                                {
                                    if(userdal.checklogin(login.username, login.pass))
                                    {
                                        int uid = userdal.getUserId(login.username);
                                        if (userdal.UpdateTrangThai(uid,"online"))
                                        {
                                            com = new THUONG("dangnhap", "OK");
                                            sendJson(client, com);
                                            DSClient.Remove(login.username);
                                            DSClient.Add(login.username, client);
                                        }
                                        else
                                        {
                                            com = new THUONG("dangnhap", "UPDATEERROR");
                                            sendJson(client, com);
                                        }
                                    }
                                    else
                                    {
                                        com = new THUONG("dangnhap", "CANCEL");
                                        sendJson(client, com);
                                        return;
                                    }
                                }
                            }
                            break;
                        case "dangki":
                            {
                                LOGIN? login = JsonSerializer.Deserialize<LOGIN>(com.content);
                                if (login != null && login.username != null && !DS.Keys.Contains(login.username))
                                {
                                    DS.Add(login.username, login.pass);
                                    com = new THUONG("dangki", "OK");
                                    sendJson(client, com);
                                }
                                else
                                {
                                    com = new THUONG("dangki", "CANCEL");
                                    sendJson(client, com);
                                    return;
                                }
                            }
                            break;
                        
                    }

                }
                else
                {
                    com = new THUONG("error", "CANCEL");
                    sendJson(client, com);
                    return;
                }
            }
            try
            {
                bool tieptuc = true;
                while (tieptuc)
                {

                    string s = sr.ReadLine();

                    com = JsonSerializer.Deserialize<GOI.THUONG>(s);

                    if (com != null && com.content != null) 
                    {
                        switch (com.kind)
                        {
                            case "getonlineusers":
                                {
                                    List<User> ls = userdal.getOnlineUsers(com.content);
                                    string lsuserstring = JsonSerializer.Serialize<List<User>>(ls);
                                    com = new THUONG("getonlineusers", lsuserstring);
                                    sendJson(client, com);
                                }
                                break;
                            case "tinnhan":
                                {
                                    GOI.TINNHAN mes = JsonSerializer.Deserialize<GOI.TINNHAN>(com.content);
                                    if(mes != null && mes.usernameReceiver != null)
                                    {
                                        if (DSClient.Keys.Contains(mes.usernameReceiver))
                                        {
                                            AppendTextBox(mes.usernameSender + " gui toi " + mes.usernameReceiver + " noi dung: " + mes.content + Environment.NewLine);
                                            mesdal.LuuTinNhan(mes.usernameSender, mes.usernameReceiver, mes.content,"");
                                            TcpClient friend = DSClient[mes.usernameReceiver];
                                            StreamWriter swtam = new StreamWriter(friend.GetStream());
                                            swtam.WriteLine(s);
                                            swtam.Flush();
                                            
                                        }
                                        else
                                        {
                                            AppendTextBox(mes.usernameReceiver + " dang offline, " + mes.usernameSender + " gui toi " + mes.usernameReceiver + " noi dung: " + mes.content + Environment.NewLine);
                                            mesdal.LuuTinNhan(mes.usernameSender, mes.usernameReceiver, mes.content, "");
                                        }
                                    }
                                }
                                break;
                            case "getallmes":
                                {
                                    GOI.GETMES getmes = JsonSerializer.Deserialize<GOI.GETMES>(com.content);
                                    List<KeyValuePair<string, string>> dicmes = new List<KeyValuePair<string, string>>();
                                    dicmes = mesdal.GetMes(getmes.sender, getmes.receiver);
                                    string dicmesstr = JsonSerializer.Serialize<List<KeyValuePair<string, string>>>(dicmes);
                                    GOI.THUONG goi = new GOI.THUONG("getallmes", dicmesstr);
                                    sendJson(client, goi);
                                }
                                break;
                            case "logout":
                                {
                                    int uid = userdal.getUserId(com.content);
                                    if (userdal.UpdateTrangThai(uid, "offline"))
                                    {
                                        DSClient[com.content].Close();
                                        DSClient.Remove(com.content);
                                        AppendTextBox("User " + com.content + " vua dang xuat");
                                        tieptuc = false;
                                    }
                                }
                                break;
                            case "guihinhchoclient":
                                {
                                    if(com.content != null)
                                    {
                                        GOI.GUIHINH guihinh = JsonSerializer.Deserialize<GOI.GUIHINH>(com.content);
                                        MemoryStream memoryStream = new MemoryStream(guihinh.manghinh);
                                        Image hinh = Image.FromStream(memoryStream);
                                        try
                                        {
                                            if (hinh != null)
                                            {
                                                DirectoryInfo drinfo = Directory.CreateDirectory("../../../Hinh/" + guihinh.usernameSender + "_" + guihinh.usernameReceiver);
                                                hinh.Save(drinfo.FullName + "\\" + guihinh.tenhinh, ImageFormat.Png);
                                                if (DSClient.Keys.Contains(guihinh.usernameReceiver))
                                                {
                                                    if (mesdal.LuuTinNhan(guihinh.usernameSender, guihinh.usernameReceiver, "", drinfo.FullName + "\\" + guihinh.tenhinh))
                                                    {
                                                        TcpClient friend = DSClient[guihinh.usernameReceiver];
                                                        StreamWriter swtam = new StreamWriter(friend.GetStream());
                                                        swtam.WriteLine(s);
                                                        swtam.Flush();
                                                    }
                                                }
                                                else
                                                {
                                                    mesdal.LuuTinNhan(guihinh.usernameSender, guihinh.usernameReceiver, "", drinfo.FullName + "\\" + guihinh.tenhinh);
                                                }
                                            }
                                        }
                                        catch(Exception ex)
                                        {
                                            
                                        }
                                    }
                                }
                                break;

                            case "userlayhinh":
                                {
                                    GOI.LAYHINH userlayhinh = JsonSerializer.Deserialize<GOI.LAYHINH>(com.content);

                                        string duongdanfolder = userlayhinh.path.Substring(0,userlayhinh.path.LastIndexOf("\\"));
                                        string tenhinh = userlayhinh.path.Substring(userlayhinh.path.LastIndexOf("\\") + 1);
                                        var file = Directory.GetFiles(duongdanfolder,tenhinh);
                                        Image hinh = Image.FromFile(file[0]);
                                        byte[] bytehinh = ImageToByte(hinh);
                                        GOI.TRAHINH guihinh = new GOI.TRAHINH(bytehinh, userlayhinh.type);
                                        string guihinhstr = JsonSerializer.Serialize(guihinh);
                                        GOI.THUONG goi = new GOI.THUONG("trahinhtusv", guihinhstr);

                                        string jsonStringgui = JsonSerializer.Serialize(goi);
                                        TcpClient friend = DSClient[userlayhinh.useryeucau];
                                        StreamWriter swtam = new StreamWriter(friend.GetStream());
                                        swtam.WriteLine(jsonStringgui);
                                        swtam.Flush();
                                }
                                break;
                        }
                    }
                }
                //client.Shutdown(SocketShutdown.Both);
                sr.Close();
                sw.Close();
                //client.Close();
            }
            catch (Exception ex)
            {
                
                //sr.Close();
                //sw.Close();
                //client.Close();
            }
        }
    }
}