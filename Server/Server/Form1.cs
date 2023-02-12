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
using Server.DTO;

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
        Dictionary<string, TcpClient> DSClientGR;

        bool active = false;
        //*DAL*//
        private UserDAL userdal;
        private MessageDAL mesdal;
        private GroupDAL groupdal;
        public Form1()
        {
            InitializeComponent();
            userdal = new UserDAL();
            mesdal = new MessageDAL();
            groupdal = new GroupDAL();
            DS = new Dictionary<string, string>();
            DSClient = new Dictionary<string, TcpClient>();
            DSNhom = new Dictionary<string, List<string>>();
            DSClientGR = new Dictionary<string, TcpClient>();
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
                                if (login != null && login.username != null && login.pass != null)
                                {
                                    if (userdal.checklogin(login.username, login.pass))
                                    {
                                        int uid = userdal.getUserId(login.username);
                                        if (userdal.UpdateTrangThai(uid, "online"))
                                        {
                                            com = new THUONG("dangnhap", "OK");
                                            sendJson(client, com);
                                            DSClient.Remove(login.username);
                                            DSClient.Add(login.username, client);
                                            AppendTextBox("Da co 1 client ket noi");
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
                                DANGKI? dangki = JsonSerializer.Deserialize<DANGKI>(com.content);
                                if (dangki != null && dangki.username != "" && dangki.pass != "")
                                {

                                    if (userdal.checkdangki(dangki.username))
                                    {  //   DS.Add(login.username, login.pass);
                                        if (userdal.ThemTaiKhoan(dangki.username, dangki.pass))
                                        {
                                            com = new THUONG("dangki", "OK");
                                            sendJson(client, com);
                                            AppendTextBox("Da co 1 client dang ki thanh cong");
                                        }
                                        else
                                        {
                                            com = new THUONG("dangki", "CANCEL");
                                            sendJson(client, com);
                                        }
                                    }
                                    else
                                    {
                                        com = new THUONG("dangki", "already_exist");
                                        sendJson(client, com);

                                    }

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
                            case "getallgroup":
                                {
                                    int userid = userdal.getUserId(com.content);
                                    List<Group> ls = groupdal.Getallgroup(userid);
                                    string lsgroupstring = JsonSerializer.Serialize<List<Group>>(ls);
                                    com = new THUONG("getallgroup", lsgroupstring);
                                    sendJson(client, com);
                                }
                                break;
                            case "tinnhan":
                                {
                                    GOI.TINNHAN mes = JsonSerializer.Deserialize<GOI.TINNHAN>(com.content);
                                    if (mes != null && mes.usernameReceiver != null)
                                    {
                                        if (DSClient.Keys.Contains(mes.usernameReceiver))
                                        {
                                            AppendTextBox(mes.usernameSender + " gui toi " + mes.usernameReceiver + " noi dung: " + mes.content + Environment.NewLine);
                                            mesdal.LuuTinNhan(mes.usernameSender, mes.usernameReceiver, mes.content, "");
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
                            case "tinnhangr":
                                {
                                    GOI.TINNHANGR mes = JsonSerializer.Deserialize<GOI.TINNHANGR>(com.content);
                                    if (mes != null && mes.usernameReceiver != null)
                                    {
                                       
                                        int grid = groupdal.layidnhom(mes.usernameReceiver);
                                        //DSNhom.Add(mes.usernameReceiver,groupdal.Getalluseringroup(grid));
                                        List<string> useringr = groupdal.Getalluseringroup(grid);
                                        if (useringr.Contains(mes.usernameSender))
                                        {
                                                AppendTextBox(mes.usernameSender + " send to group " + mes.usernameReceiver + " content: " + mes.content + Environment.NewLine);
                                                mesdal.LuuTinNhanGR(mes.usernameSender, mes.usernameReceiver, mes.content, "", grid);
                                                foreach (string user in useringr)
                                                {
                                                    if (DSClient.Keys.Contains(user) && user!= mes.usernameSender)
                                                    {
                                                            TcpClient friend = DSClient[user];
                                                            StreamWriter swtam = new StreamWriter(friend.GetStream());
                                                            swtam.WriteLine(s);
                                                            swtam.Flush();
                                                    }
                                                }
                                               
                                         }
                                         else
                                         {
                                                AppendTextBox(mes.usernameReceiver + " dang offline, " + mes.usernameSender + " gui toi " + mes.usernameReceiver + " noi dung: " + mes.content + Environment.NewLine);
                                                mesdal.LuuTinNhanGR(mes.usernameSender, mes.usernameReceiver, mes.content, "",grid);
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
                            case "getallmesgr":
                                {
                                    GOI.GETMES getmes = JsonSerializer.Deserialize<GOI.GETMES>(com.content);
                                    List<KeyValuePair<string, string>> dicmes = new List<KeyValuePair<string, string>>();
                                    dicmes = mesdal.GetMesgr(getmes.sender, getmes.receiver);
                                    string dicmesstr = JsonSerializer.Serialize<List<KeyValuePair<string, string>>>(dicmes);
                                    GOI.THUONG goi = new GOI.THUONG("getallmesgr", dicmesstr);
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
                                    if (com.content != null)
                                    {
                                        GOI.GUIHINH guihinh = JsonSerializer.Deserialize<GOI.GUIHINH>(com.content);
                                        MemoryStream memoryStream = new MemoryStream(guihinh.manghinh);
                                        Image hinh = Image.FromStream(memoryStream);
                                        try
                                        {
                                            if (hinh != null)
                                            {
                                                string pathxuoi = "../../../Hinh/" + guihinh.usernameSender + "_" + guihinh.usernameReceiver;
                                                string pathnguoc = "../../../Hinh/" + guihinh.usernameReceiver + "_" + guihinh.usernameSender;
                                                DirectoryInfo drinfo = Directory.CreateDirectory("../../../Hinh/" + guihinh.usernameSender + "_" + guihinh.usernameReceiver);
                                                //if (!Directory.Exists(pathxuoi) == true && !Directory.Exists(pathnguoc) == true)
                                                //{
                                                //    drinfo = Directory.CreateDirectory("../../../Hinh/" + guihinh.usernameSender + "_" + guihinh.usernameReceiver);
                                                //}
                                                //if (!Directory.Exists(pathxuoi) == true && !Directory.Exists(pathnguoc) == false)
                                                //{
                                                //    drinfo = Directory.CreateDirectory("../../../Hinh/" + guihinh.usernameReceiver + "_" + guihinh.usernameSender);
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
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }
                                break;

                            case "guihinhchogroup":
                                {
                                    if (com.content != null)
                                    {
                                        GOI.GUIHINH guihinh = JsonSerializer.Deserialize<GOI.GUIHINH>(com.content);
                                        MemoryStream memoryStream = new MemoryStream(guihinh.manghinh);
                                        Image hinh = Image.FromStream(memoryStream);
                                        try
                                        {
                                            if (hinh != null)
                                            {
                                                string pathxuoi = "../../../Hinh/GROUP/"+ guihinh.usernameReceiver + "/" + guihinh.usernameSender;
                                                //string pathnguoc = "../../../Hinh/GROUP" + guihinh.usernameReceiver + "_" + guihinh.usernameSender;
                                                DirectoryInfo drinfo = Directory.CreateDirectory(pathxuoi);
                                                //if (!Directory.Exists(pathxuoi) == true && !Directory.Exists(pathnguoc) == true)
                                                //{
                                                //    drinfo = Directory.CreateDirectory("../../../Hinh/" + guihinh.usernameSender + "_" + guihinh.usernameReceiver);
                                                //}
                                                //if (!Directory.Exists(pathxuoi) == true && !Directory.Exists(pathnguoc) == false)
                                                //{
                                                //    drinfo = Directory.CreateDirectory("../../../Hinh/" + guihinh.usernameReceiver + "_" + guihinh.usernameSender);
                                                hinh.Save(drinfo.FullName + "\\" + guihinh.tenhinh, ImageFormat.Png);
                                                int idnhom = groupdal.layidnhom(guihinh.usernameReceiver);
                                                List<string> useringr = groupdal.Getalluseringroup(idnhom);
                                                if (useringr.Contains(guihinh.usernameSender))
                                                {
                                                    if (mesdal.LuuTinNhanGR(guihinh.usernameSender, guihinh.usernameReceiver, "", drinfo.FullName + "\\" + guihinh.tenhinh, idnhom))
                                                    {
                                                        foreach (string user in useringr)
                                                        {
                                                            if (DSClient.Keys.Contains(user))
                                                            {
                                                                if (!user.Equals(guihinh.usernameSender))
                                                                {
                                                                    TcpClient friend = DSClient[user];
                                                                    StreamWriter swtam = new StreamWriter(friend.GetStream());
                                                                    swtam.WriteLine(s);
                                                                    swtam.Flush();
                                                                }
                                                            }
                                                            
                                                        }
                                                    }
                                                }
                                                
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }
                                break;

                            case "userlayhinh":
                                {
                                    GOI.LAYHINH userlayhinh = JsonSerializer.Deserialize<GOI.LAYHINH>(com.content);

                                    string duongdanfolder = userlayhinh.path.Substring(0, userlayhinh.path.LastIndexOf("\\"));
                                    string tenhinh = userlayhinh.path.Substring(userlayhinh.path.LastIndexOf("\\") + 1);
                                    var file = Directory.GetFiles(duongdanfolder, tenhinh);
                                    if (file.Length != 0)
                                    {
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
                                    else
                                    {
                                        byte[] bytehinh = null;
                                        GOI.TRAHINH guihinh = new GOI.TRAHINH(bytehinh, userlayhinh.type);
                                        string guihinhstr = JsonSerializer.Serialize(guihinh);
                                        GOI.THUONG goi = new GOI.THUONG("trahinhtusv", guihinhstr);

                                        string jsonStringgui = JsonSerializer.Serialize(goi);
                                        TcpClient friend = DSClient[userlayhinh.useryeucau];
                                        StreamWriter swtam = new StreamWriter(friend.GetStream());
                                        swtam.WriteLine(jsonStringgui);
                                        swtam.Flush();
                                    }
                                }
                                break;

                            case "userlayhinhgroup":
                                {
                                    GOI.LAYHINH userlayhinh = JsonSerializer.Deserialize<GOI.LAYHINH>(com.content);

                                    string duongdanfolder = userlayhinh.path.Substring(0, userlayhinh.path.LastIndexOf("\\"));
                                    string tenhinh = userlayhinh.path.Substring(userlayhinh.path.LastIndexOf("\\") + 1);
                                    var file = Directory.GetFiles(duongdanfolder, tenhinh);
                                    if (file.Length != 0)
                                    {
                                        Image hinh = Image.FromFile(file[0]);
                                        byte[] bytehinh = ImageToByte(hinh);
                                        GOI.TRAHINH guihinh = new GOI.TRAHINH(bytehinh, userlayhinh.type);
                                        string guihinhstr = JsonSerializer.Serialize(guihinh);
                                        GOI.THUONG goi = new GOI.THUONG("trahinhtusvgroup", guihinhstr);

                                        string jsonStringgui = JsonSerializer.Serialize(goi);
                                        TcpClient friend = DSClient[userlayhinh.useryeucau];
                                        StreamWriter swtam = new StreamWriter(friend.GetStream());
                                        swtam.WriteLine(jsonStringgui);
                                        swtam.Flush();
                                    }
                                    else
                                    {
                                        byte[] bytehinh = null;
                                        GOI.TRAHINH guihinh = new GOI.TRAHINH(bytehinh, userlayhinh.type);
                                        string guihinhstr = JsonSerializer.Serialize(guihinh);
                                        GOI.THUONG goi = new GOI.THUONG("trahinhtusvgroup", guihinhstr);

                                        string jsonStringgui = JsonSerializer.Serialize(goi);
                                        TcpClient friend = DSClient[userlayhinh.useryeucau];
                                        StreamWriter swtam = new StreamWriter(friend.GetStream());
                                        swtam.WriteLine(jsonStringgui);
                                        swtam.Flush();
                                    }
                                }
                                break;

                            case "searchuser":
                                {
                                    GOI.SEARCHUSER searchuser = JsonSerializer.Deserialize<GOI.SEARCHUSER>(com.content);
                                    List<User> listkq = userdal.SearchUser(searchuser.username, searchuser.nguoitimkiem);
                                    string listkqstr = JsonSerializer.Serialize<List<User>>(listkq);
                                    GOI.THUONG goi = new GOI.THUONG("searchuser", listkqstr);
                                    sendJson(client, goi);
                                }
                                break;

                            case "taonhom":
                                {
                                    GOI.TAONHOM taonhom = JsonSerializer.Deserialize<GOI.TAONHOM>(com.content);

                                    GOI.THANHVIENNHOM tvnhom = JsonSerializer.Deserialize<GOI.THANHVIENNHOM>(taonhom.jsonstrthanhvien);
                                    List<User> thanhvien = JsonSerializer.Deserialize<List<User>>(tvnhom.strthanhviennhom);
                                    if (!groupdal.KiemTraTrungTenNhom(taonhom.tennhom))
                                    {
                                        if (groupdal.TaoNhom(taonhom.nguoitao, taonhom.tennhom, thanhvien))
                                        {
                                            GOI.THUONG goi = new GOI.THUONG("taonhom", "SUCCESS");
                                            string jsonStringgui = JsonSerializer.Serialize<GOI.THUONG>(goi);
                                            StreamWriter swtam = new StreamWriter(client.GetStream());
                                            swtam.WriteLine(jsonStringgui);
                                            swtam.Flush();
                                            //sendJson(client, goi);
                                        }
                                        else
                                        {
                                            GOI.THUONG goi = new GOI.THUONG("taonhom", "FAILED");
                                            string jsonStringgui = JsonSerializer.Serialize<GOI.THUONG>(goi);
                                            StreamWriter swtam = new StreamWriter(client.GetStream());
                                            swtam.WriteLine(jsonStringgui);
                                            swtam.Flush();
                                            //sendJson(client, goi);
                                        }
                                    }
                                    else
                                    {
                                        GOI.THUONG goi = new GOI.THUONG("taonhom", "TRUNGTENNHOM");
                                        string jsonStringgui = JsonSerializer.Serialize<GOI.THUONG>(goi);
                                        StreamWriter swtam = new StreamWriter(client.GetStream());
                                        swtam.WriteLine(jsonStringgui);
                                        swtam.Flush();
                                        //sendJson(client, goi);
                                    }
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