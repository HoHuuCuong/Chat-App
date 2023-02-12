using MaterialSkin;
using MaterialSkin.Controls;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using Application = System.Windows.Forms.Application;

namespace DoAn
{
    public partial class Form1 : MaterialForm
    {
        readonly MaterialSkin.MaterialSkinManager manager;
        private Thread trd;
        private Thread mainformThread;
        Boolean login = true;
        public Form1()
        {
            InitializeComponent();
            //
            manager = MaterialSkin.MaterialSkinManager.Instance;
            manager.EnforceBackcolorOnAllComponents = true;
            manager.AddFormToManage(this);
            manager.Theme = MaterialSkin.MaterialSkinManager.Themes.LIGHT;
            manager.ColorScheme = new MaterialSkin.ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.LightBlue200, TextShade.WHITE);

            Graphics g = this.CreateGraphics();
            Double startingPoint = (this.Width / 2) - (g.MeasureString(this.Text.Trim(), this.Font).Width / 2);
            Double widthOfASpace = g.MeasureString(" ", this.Font).Width;
            String tmp = " ";
            Double tmpWidth = 0;

            while ((tmpWidth + widthOfASpace) < startingPoint)
            {
                tmp += " ";
                tmpWidth += widthOfASpace;
            }

            this.Text = tmp + this.Text.Trim();


        }

        private void CloseThisForm()
        {
            this.BeginInvoke(new MethodInvoker(() =>
            {
                this.Close();
            }));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(0, 0);
        }

        private void sendJson(object obj)
        {
            string jsonStringgui = JsonSerializer.Serialize(obj);
            //client.Send(jsonUtf8Bytes, jsonUtf8Bytes.Length, SocketFlags.None);
            Shared.strwrite.WriteLine(jsonStringgui);
            Shared.strwrite.Flush();
        }
        private void ConnectToServer()
        {
            try
            {
                Shared.server = new TcpClient();
                Shared.server.Connect(Shared.iep);
                Shared.strwrite = new StreamWriter(Shared.server.GetStream());
                Shared.strread = new StreamReader(Shared.server.GetStream());
                trd = new Thread(new ThreadStart(() => ThreadTask(Shared.strread, Shared.strwrite)));
                trd.IsBackground = true;
                trd.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Server chua hoat dong hoac loi mang!");
            }
        }

        private void ThreadTask(StreamReader sr, StreamWriter sw)
        {
            if (login == true)
            {
                GOI.LOGIN login = new GOI.LOGIN(txtusername.Text.Trim(), txtpassword.Text.Trim());

                string jsonString = JsonSerializer.Serialize(login);

                GOI.THUONG goidangnhap = new GOI.THUONG("dangnhap", jsonString);
                sendJson(goidangnhap);
                jsonString = sr.ReadLine();
                jsonString.Replace("\\u0022", "\"");
                GOI.THUONG? goinhan = JsonSerializer.Deserialize<GOI.THUONG>(jsonString);
                try
                {
                    if (goinhan != null && goinhan.kind == "dangnhap")
                    {
                        if (goinhan.content == "OK")
                        {
                            MessageBox.Show("Dang nhap thanh cong!");
                            Shared.taikhoan = txtusername.Text.Trim();
                            Shared.matkhau = txtpassword.Text.Trim();
                            mainformThread = new Thread(openMainForm);
                            CloseThisForm();
                            mainformThread.SetApartmentState(ApartmentState.STA);
                            mainformThread.Start();
                        }
                        else
                        {
                            MessageBox.Show("Dang nhap that bai!");
                        }
                    }
                    //client.Disconnect(true);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                GOI.DANGKI dangki = new GOI.DANGKI(txtusername.Text.Trim(), txtpassword.Text.Trim());

                string jsonString = JsonSerializer.Serialize(dangki);

                GOI.THUONG goidangki = new GOI.THUONG("dangki", jsonString);
                sendJson(goidangki);


                jsonString = sr.ReadLine();
                jsonString.Replace("\\u0022", "\"");
                GOI.THUONG? goinhan = JsonSerializer.Deserialize<GOI.THUONG>(jsonString);
                try
                {
                    if (goinhan != null && goinhan.kind == "dangki")
                    {
                        if (goinhan.content == "OK")
                        {
                            MessageBox.Show("Dang ki thanh cong!");
                            txtusername.Text = "";
                            txtpassword.Text = "";
                        }
                        else if (goinhan.content == "already_exist")
                        {
                            MessageBox.Show("Tai khoan nay da ton tai!");
                        }
                        else
                        {
                            MessageBox.Show("Dang ki that bai!");
                        }
                    }
                    //client.Disconnect(true);
                }
                catch (Exception)
                {

                }

            }

        }

        private void openMainForm(object obj)
        {
            Application.Run(new MainForm(Shared.server));
        }

        private void btndangnhap_Click(object sender, EventArgs e)
        {
            login = true;
            ConnectToServer();
        }

        private void btn_dangki_Click(object sender, EventArgs e)
        {
            login = false;
            ConnectToServer();
        }
    }
}