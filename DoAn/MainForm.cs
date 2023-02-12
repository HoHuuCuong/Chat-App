using DoAn.DTO;
using GOI;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DoAn
{
    public partial class MainForm : MaterialForm
    {
        readonly MaterialSkin.MaterialSkinManager manager;
        private TcpClient client;
        private StreamReader sr;
        private StreamWriter sw;
        public Thread trdClient;
        private Thread trdtaonhom;
        private IPEndPoint iep;
        //private bool thoat;
        private string usrname, pwd, receiv;
        private string firstitemstr;
        private List<int> listvitrihinh = new List<int>();
        private List<int> listvitrihinhgroup = new List<int>();
        private static int vitri = 0;
        private static int vitrihinhgr = 0;
        private string tennhomhientai = "";
        public MainForm(TcpClient client)
        {
            InitializeComponent();
            
            this.usrname = Shared.taikhoan;
            this.pwd = Shared.matkhau;
            this.client = client;
            //this.thoat = false;
            Shared.MainFormthoat = false;
            sr = new StreamReader(client.GetStream());
            sw = new StreamWriter(client.GetStream());
            NhanTinNhan();
           
            //CODE UI//
            manager = MaterialSkin.MaterialSkinManager.Instance;
            manager.EnforceBackcolorOnAllComponents = false;
            manager.AddFormToManage(this);
            manager.Theme = MaterialSkin.MaterialSkinManager.Themes.LIGHT;
            //manager.ColorScheme = new ColorScheme(
            //Primary.DeepOrange300, Primary.BlueGrey900,
            //Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
            manager.ColorScheme = new MaterialSkin.ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.LightBlue200, TextShade.WHITE);
            
        }

        private void drawitem(object sender, DrawItemEventArgs e)
        {
            var g = e.Graphics;
            var text = this.menutab.TabPages[e.Index].Text;
            var sizeText = g.MeasureString(text, this.menutab.Font);

            var x = e.Bounds.Left + 3;
            var y = e.Bounds.Top + (e.Bounds.Height - sizeText.Height) / 2;

            g.DrawString(text, this.menutab.Font, Brushes.Black, x, y);
        }

        public void NhanTinNhan()
        {
            trdClient = new Thread(new ThreadStart(() => ThreadTask(sr, sw)));
            trdClient.IsBackground = true;
            trdClient.Start();
        }

        private void AppendTextBox(string value, string type)
        {
            khungchat.BeginInvoke(new MethodInvoker(() =>
            {
                if(type == "gui")
                {
                    khungchat.AppendText(Environment.NewLine + value);
                    khungchat.SelectionAlignment = HorizontalAlignment.Right;
                    int endofrtb = khungchat.TextLength;
                    int beginoftxt = endofrtb - value.Length;
                    khungchat.Select(beginoftxt, endofrtb);
                    khungchat.SelectionColor = Color.Blue;
                }
                else
                {
                    khungchat.AppendText(Environment.NewLine + value);
                    khungchat.SelectionAlignment = HorizontalAlignment.Left;
                    int endofrtb = khungchat.TextLength;
                    int beginoftxt = endofrtb - value.Length;
                    khungchat.Select(beginoftxt, endofrtb);
                    khungchat.SelectionColor = Color.Red;
                }
            }));
        }
        private void AppendTextBoxgr(GOI.TINNHANGR tinnhangr, string type)
        {
            
            
            if (tennhomhientai.Equals(tinnhangr.usernameReceiver))
            {
                khungchatgr.BeginInvoke(new MethodInvoker(() =>
                {
                    if (type == "gui")
                    {
                        khungchatgr.AppendText(Environment.NewLine + tinnhangr.content);
                        khungchatgr.SelectionAlignment = HorizontalAlignment.Right;
                        int endofrtb = khungchatgr.TextLength;
                        int beginoftxt = endofrtb - tinnhangr.content.Length;
                        khungchatgr.Select(beginoftxt, endofrtb);
                        khungchatgr.SelectionColor = Color.Blue;
                    }
                    else
                    {
                        string value = tinnhangr.usernameSender + ": " + tinnhangr.content;
                        khungchatgr.AppendText(Environment.NewLine + value);
                        khungchatgr.SelectionAlignment = HorizontalAlignment.Left;
                        int endofrtb = khungchatgr.TextLength;
                        int beginoftxt = endofrtb - value.Length;
                        khungchatgr.Select(beginoftxt, endofrtb);
                        khungchatgr.SelectionColor = Color.Red;
                    }
                }));
            }
        }

        private void AppendImageToTextBoxLeft(byte[] manghinh)
        {
            khungchat.BeginInvoke(new MethodInvoker(() =>
            {
                MemoryStream memoryStream = new MemoryStream(manghinh);
                Image hinh = Image.FromStream(memoryStream);
                Clipboard.SetImage(hinh);
                khungchat.ReadOnly = false;
                //khungchat.AppendText(Environment.NewLine + " ");
                if (khungchat.Lines.Length != 0)
                {
                    khungchat.SelectionStart = listvitrihinh[vitri];
                    vitri += 1;
                }
                khungchat.Paste();
                khungchat.SelectionAlignment = HorizontalAlignment.Left;
                khungchat.ReadOnly = true;
            }));
        }

        private void SenderAppendImageToTextBoxLeft(byte[] manghinh)
        {
            khungchat.BeginInvoke(new MethodInvoker(() =>
            {
                MemoryStream memoryStream = new MemoryStream(manghinh);
                Image hinh = Image.FromStream(memoryStream);
                Clipboard.SetImage(hinh);
                khungchat.ReadOnly = false;
                khungchat.AppendText(Environment.NewLine + " ");
                if (khungchat.Lines.Length != 0)
                {
                    khungchat.SelectionStart = khungchat.GetFirstCharIndexOfCurrentLine();
                    vitri += 1;
                }
                khungchat.Paste();
                khungchat.SelectionAlignment = HorizontalAlignment.Left;
                khungchat.AppendText(Environment.NewLine);
                khungchat.ReadOnly = true;
            }));
        }

        private void SenderAppendImageToTextBoxGroupLeft(byte[] manghinh)
        {
            khungchatgr.BeginInvoke(new MethodInvoker(() =>
            {
                MemoryStream memoryStream = new MemoryStream(manghinh);
                Image hinh = Image.FromStream(memoryStream);
                Clipboard.SetImage(hinh);
                khungchatgr.ReadOnly = false;
                khungchatgr.AppendText(Environment.NewLine + " ");
                if (khungchatgr.Lines.Length != 0)
                {
                    khungchatgr.SelectionStart = khungchatgr.GetFirstCharIndexOfCurrentLine();
                    vitrihinhgr += 1;
                }
                khungchatgr.Paste();
                khungchatgr.SelectionAlignment = HorizontalAlignment.Left;
                khungchatgr.AppendText(Environment.NewLine);
                khungchatgr.ReadOnly = true;
            }));
        }

        private void AppendImageToTextBoxRight(byte[] manghinh)
        {
            khungchat.BeginInvoke(new MethodInvoker(() =>
            {
                MemoryStream memoryStream = new MemoryStream(manghinh);
                Image hinh = Image.FromStream(memoryStream);
                Clipboard.SetImage(hinh);
                khungchat.ReadOnly = false;
                //khungchat.AppendText(Environment.NewLine + " ");
                if (khungchat.Lines.Length != 0)
                {
                    khungchat.SelectionStart = listvitrihinh[vitri];
                    vitri += 1;
                }
                khungchat.Paste();
                khungchat.SelectionAlignment = HorizontalAlignment.Right;
                khungchat.ReadOnly = true;
            }));
        }

        private void AppendImageToTextBoxGroupLeft(byte[] manghinh)
        {
            khungchatgr.BeginInvoke(new MethodInvoker(() =>
            {
                MemoryStream memoryStream = new MemoryStream(manghinh);
                Image hinh = Image.FromStream(memoryStream);
                Clipboard.SetImage(hinh);
                khungchatgr.ReadOnly = false;
                //khungchat.AppendText(Environment.NewLine + " ");
                if (khungchatgr.Lines.Length != 0)
                {
                    khungchatgr.SelectionStart = listvitrihinhgroup[vitrihinhgr];
                    vitrihinhgr += 1;
                }
                khungchatgr.Paste();
                khungchatgr.SelectionAlignment = HorizontalAlignment.Left;
                khungchatgr.ReadOnly = true;
            }));
        }

        private void AppendImageToTextBoxGroupRight(byte[] manghinh)
        {
            khungchatgr.BeginInvoke(new MethodInvoker(() =>
            {
                MemoryStream memoryStream = new MemoryStream(manghinh);
                Image hinh = Image.FromStream(memoryStream);
                Clipboard.SetImage(hinh);
                khungchatgr.ReadOnly = false;
                //khungchat.AppendText(Environment.NewLine + " ");
                if (khungchatgr.Lines.Length != 0)
                {
                    khungchatgr.SelectionStart = listvitrihinhgroup[vitrihinhgr];
                    vitrihinhgr += 1;
                }
                khungchatgr.Paste();
                khungchatgr.SelectionAlignment = HorizontalAlignment.Right;
                khungchatgr.ReadOnly = true;
            }));
        }

        private bool IsValidPath(string path, bool exactPath = true)
        {
            bool isValid = true;

            try
            {
                string fullPath = Path.GetFullPath(path);

                if (exactPath)
                {
                    string root = Path.GetPathRoot(path);
                    isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
                }
                else
                {
                    isValid = Path.IsPathRooted(path);
                }
            }
            catch (Exception ex)
            {
                isValid = false;
            }

            return isValid;
        }

       

        private void ShowAllMes(List<KeyValuePair<string, string>> dic)
        {
            khungchat.BeginInvoke(new MethodInvoker(() =>
            {
                khungchat.Clear();
                var list = dic.ToList();
                listvitrihinh.Clear();
                vitri = 0;
                foreach(var item in list)
                {
                    string doituong = item.Key.Substring(0, item.Key.IndexOf("@"));
                    if(doituong == "sender")
                    {
                        try
                        {
                            if (IsValidPath(item.Value))
                            {   
                                khungchat.AppendText(Environment.NewLine + " ");
                                listvitrihinh.Add(khungchat.SelectionStart + khungchat.SelectionLength);

                                GOI.LAYHINH userlayhinh = new GOI.LAYHINH(usrname, item.Value, "sender");
                                string userlayhinhstr = JsonSerializer.Serialize(userlayhinh);
                                GOI.THUONG goilayhinh = new GOI.THUONG("userlayhinh", userlayhinhstr);
                                sendJson(goilayhinh);

                                khungchat.AppendText(Environment.NewLine);
                            }
                            else
                            {
                                khungchat.AppendText(Environment.NewLine + item.Value);
                                //int temp = khungchat.GetFirstCharIndexOfCurrentLine();
                                khungchat.SelectionAlignment = HorizontalAlignment.Right;
                                int endofrtb = khungchat.TextLength;
                                int beginoftxt = endofrtb - item.Value.Length;
                                khungchat.Select(beginoftxt, endofrtb);
                                khungchat.SelectionColor = Color.Blue;
                                khungchat.AppendText(Environment.NewLine + " ");
                                khungchat.AppendText(Environment.NewLine);
                            }
                        }
                        catch(Exception ex)
                        {

                        }
                        
                    }
                    else
                    {
                        try
                        {
                            if (IsValidPath(item.Value))
                            {
                                khungchat.AppendText(Environment.NewLine + " ");
                                //int t = khungchat.GetFirstCharIndexOfCurrentLine();
                                //listvitrihinh.Add(khungchat.GetFirstCharIndexOfCurrentLine());
                                listvitrihinh.Add(khungchat.SelectionStart + khungchat.SelectionLength);
                                //test commit
                                GOI.LAYHINH userlayhinh = new GOI.LAYHINH(usrname, item.Value, "receiver");
                                string userlayhinhstr = JsonSerializer.Serialize(userlayhinh);
                                GOI.THUONG goilayhinh = new GOI.THUONG("userlayhinh", userlayhinhstr);
                                sendJson(goilayhinh);

                                khungchat.AppendText(Environment.NewLine);
                            }
                            else
                            {
                                khungchat.AppendText(Environment.NewLine + item.Value);
                                //int temp = khungchat.GetFirstCharIndexOfCurrentLine();
                                khungchat.SelectionAlignment = HorizontalAlignment.Left;
                                int endofrtb = khungchat.TextLength;
                                int beginoftxt = endofrtb - item.Value.Length;
                                khungchat.Select(beginoftxt, endofrtb);
                                khungchat.SelectionColor = Color.Red;
                                khungchat.AppendText(Environment.NewLine + " ");
                                khungchat.AppendText(Environment.NewLine);
                            }
                        }
                        catch(Exception ex)
                        {

                        }
                        
                    }
                }
                 //KeyValuePair<string, string> temp = new KeyValuePair<string, string>(list[i].Key, list[i].Value);
                //foreach (var item in dic)
                //{
                //    if (item.Key == usrname)
                //    {
                //        List<string> lsmessage = dic[item.Key];
                //        foreach (var mes in lsmessage)
                //        {
                //            khungchat.AppendText(Environment.NewLine + mes);
                //            khungchat.SelectionAlignment = HorizontalAlignment.Right;
                //        }
                //    }
                //    else
                //    {
                //        List<string> lsmessage = dic[item.Key];
                //        foreach (var mes in lsmessage)
                //        {
                //            khungchat.AppendText(Environment.NewLine + mes);
                //            khungchat.SelectionAlignment = HorizontalAlignment.Left;
                //        }
                //    }
                //}
            }));
        }
        private void ShowAllMesgr(List<KeyValuePair<string, string>> dic)
        {
            khungchatgr.BeginInvoke(new MethodInvoker(() =>
            {
                khungchatgr.Clear();
                var list = dic.ToList();
                listvitrihinhgroup.Clear();
                vitrihinhgr = 0;
                foreach (var item in list)
                {
                    string[] mang = item.Key.Split("@");
                    if (mang[0] == "sender")
                    {
                        try
                        {
                            if (IsValidPath(item.Value))
                            {
                                khungchatgr.AppendText(Environment.NewLine + " ");
                                listvitrihinhgroup.Add(khungchatgr.SelectionStart + khungchatgr.SelectionLength);

                                GOI.LAYHINH userlayhinh = new GOI.LAYHINH(usrname, item.Value, "sender");
                                string userlayhinhstr = JsonSerializer.Serialize(userlayhinh);
                                GOI.THUONG goilayhinh = new GOI.THUONG("userlayhinhgroup", userlayhinhstr);
                                sendJson(goilayhinh);

                                khungchatgr.AppendText(Environment.NewLine);
                            }
                            else
                            {
                               
                                khungchatgr.AppendText(Environment.NewLine + item.Value);
                                khungchatgr.SelectionAlignment = HorizontalAlignment.Right;
                                int endofrtb = khungchatgr.TextLength;
                                int beginoftxt = endofrtb - item.Value.Length;
                                khungchatgr.Select(beginoftxt, endofrtb);
                                khungchatgr.SelectionColor = Color.Blue;
                                khungchatgr.AppendText(Environment.NewLine + " ");
                                khungchatgr.AppendText(Environment.NewLine);

                            }
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    else
                    {
                        try
                        {
                            if (IsValidPath(item.Value))
                            {
                                khungchatgr.AppendText(Environment.NewLine + " ");
                                listvitrihinhgroup.Add(khungchatgr.SelectionStart + khungchatgr.SelectionLength);

                                GOI.LAYHINH userlayhinh = new GOI.LAYHINH(usrname, item.Value, "receiver");
                                string userlayhinhstr = JsonSerializer.Serialize(userlayhinh);
                                GOI.THUONG goilayhinh = new GOI.THUONG("userlayhinhgroup", userlayhinhstr);
                                sendJson(goilayhinh);

                                khungchatgr.AppendText(Environment.NewLine);
                            }
                            else
                            {
                                string val = mang[2] + ": " + item.Value;
                                khungchatgr.AppendText(Environment.NewLine + val);
                                khungchatgr.SelectionAlignment = HorizontalAlignment.Left;
                                int endofrtb = khungchatgr.TextLength;
                                int beginoftxt = endofrtb - val.Length;
                                khungchatgr.Select(beginoftxt, endofrtb);
                                khungchatgr.SelectionColor = Color.Red;
                                khungchatgr.AppendText(Environment.NewLine + " ");
                                khungchatgr.AppendText(Environment.NewLine);
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                }
               
            }));
        }
        private void AppendTabUser(List<User> ls)
        {
            tabuser.BeginInvoke(new MethodInvoker(() =>
            {
                tabuser.Items.Clear();
                foreach (var u in ls)
                {
                    if(u.name != usrname)
                    {
                        if (u.trangthai.Equals("online"))
                        {
                            tabuser.Items.Add(u.ToString(), 1);
                        }
                        else
                        {
                            tabuser.Items.Add(u.ToString(), 2);
                        }
                    }
                }
                tabuser.Items[0].Selected = true;
                tabuser.Items[0].Focused = true;
            }));
            
        }

        private void AppendTabGroup(List<Group> ls)
        {
            if(ls.Count != 0)
            {
                listgroup.BeginInvoke(new MethodInvoker(() =>
                {
                    listgroup.Items.Clear();
                    foreach (var g in ls)
                    {
                        listgroup.Items.Add(g.ToString(), 0);
                    }
                    listgroup.Items[0].Selected = true;
                    listgroup.Items[0].Focused = true;
                }));
            }
        }

        private void btngui_Click(object sender, EventArgs e)
        {
            if(txtchat.Text.Length != 0)
            {
                string itemstr = tabuser.SelectedItems[0].Text;
                string receiver = itemstr.Substring(itemstr.IndexOf(" ") + 1);
                GOI.TINNHAN mes = new GOI.TINNHAN(usrname, receiver, txtchat.Text);
                string jsonString = JsonSerializer.Serialize(mes);
                GOI.THUONG common = new GOI.THUONG("tinnhan", jsonString);
                sendJson(common);
                AppendTextBox(txtchat.Text, "gui");
            }
            txtchat.Clear();
        }

        private void sendJson(object obj)
        {
            string jsonStringgui = JsonSerializer.Serialize(obj);
            //client.Send(jsonUtf8Bytes, jsonUtf8Bytes.Length, SocketFlags.None);
            sw.WriteLine(jsonStringgui);
            sw.Flush();
        }

        private void openLoginForm()
        {
            Application.Run(new Form1());
        }

        

        private void btnguihinh_Click(object sender, EventArgs e)
        {
            //MO DIALOG CHON HINH//
            OpenFileDialog layhinh = new OpenFileDialog();
            layhinh.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            layhinh.Multiselect = false;
            string path = "";
            string tenhinh = "";
            object clipboarddata = Clipboard.GetDataObject;
            khungchat.AppendText(Environment.NewLine + " ");

            //CLEAR LIST VI TRI HINH //
            listvitrihinh.Clear();
            vitri = 0;
            if (layhinh.ShowDialog() == DialogResult.OK)
            {
                path = layhinh.FileName;
                string imagepath = path.ToString();
                imagepath = imagepath.Substring(imagepath.LastIndexOf("\\"));
                tenhinh = imagepath.Remove(0, 1);

                Image hinh = Image.FromFile(path);
                Image hinhresize = (Image)ResizeImage(hinh, 150, 150);
                Clipboard.SetImage(hinhresize);
                khungchat.ReadOnly = false;
                khungchat.AppendText(Environment.NewLine+" ");
                if (khungchat.Lines.Length != 0)
                {
                    
                    khungchat.SelectionStart = khungchat.GetFirstCharIndexOfCurrentLine();
                }
                khungchat.Paste();
                khungchat.SelectionAlignment = HorizontalAlignment.Right;
                khungchat.ReadOnly = true;
                
                Clipboard.SetDataObject(clipboarddata);

                //GUI HINH QUA TCPCLIENT//
                byte[] bStream = ImageToByte(hinhresize);
                string itemstr = tabuser.SelectedItems[0].Text;
                string receiver = itemstr.Substring(itemstr.IndexOf(" ") + 1);
                GOI.GUIHINH guihinh = new GOI.GUIHINH(usrname, receiver, bStream, tenhinh);
                string guihinhstr = JsonSerializer.Serialize(guihinh);
                GOI.THUONG goi = new GOI.THUONG("guihinhchoclient", guihinhstr);
                sendJson(goi);
            }
        }

        private static byte[] ImageToByte(Image iImage)
        {
            MemoryStream mMemoryStream = new MemoryStream();
            iImage.Save(mMemoryStream, ImageFormat.Png);
            return mMemoryStream.ToArray();
        }
        public Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void tabuser_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //var arr = tabuser.SelectedItems;
                //foreach(var item in arr)
                //{
                //    var templiv = (ListViewItem)item;
                //    string txt = templiv.SubItems[0].Text;
                //    GOI.GETMES getmes = new GOI.GETMES(usrname, receiv);
                //    string getmesstr = JsonSerializer.Serialize<GOI.GETMES>(getmes);
                //    GOI.THUONG goi = new GOI.THUONG("getallmes", getmesstr);
                //    sendJson(goi);
                //    break;
                //}
                if (tabuser.SelectedIndices.Count > 0)
                {
                    string itemstr = tabuser.Items[tabuser.SelectedIndices[0]].Text;
                    receiv = itemstr.Substring(itemstr.IndexOf(" ") + 1);
                    GOI.GETMES getmes = new GOI.GETMES(usrname, receiv);
                    string getmesstr = JsonSerializer.Serialize<GOI.GETMES>(getmes);
                    GOI.THUONG goi = new GOI.THUONG("getallmes", getmesstr);
                    sendJson(goi);
                }
                //if (tabuser.FocusedItem == null) return;
                //int p = tabuser.FocusedItem.Index;

            }
            catch (Exception ex)
            {

            }
        }

        private void btntaonhom_Click(object sender, EventArgs e)
        {

            //trdtaonhom = new Thread(openFormTaoNhom);
            //thoat = true;
            //this.Close();
            //trdtaonhom.SetApartmentState(ApartmentState.STA);
            //trdtaonhom.Start();

            //this.Close();
            Shared.MainFormthoat = true;
            trdtaonhom = new Thread(new ThreadStart(ThreadTaoNhom));
            trdtaonhom.Start();
        }

        private void ThreadTaoNhom()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => openFormTaoNhom()));
                return;
            }
            openFormTaoNhom();

        }


        private void openFormTaoNhom()
        {
            //Application.Run(new TaoNhom(usrname));
            var formtaonhom = new TaoNhom(usrname, this);
            formtaonhom.ShowDialog();
            //Shared.MainFormthoat = false;
        }

        private void txtchat_TextChanged(object sender, EventArgs e)
        {

        }

        private void listgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                if (listgroup.SelectedIndices.Count > 0)
                {
                    tennhomhientai = listgroup.Items[listgroup.SelectedIndices[0]].Text;
                    receiv = tennhomhientai.Substring(tennhomhientai.IndexOf(" ") + 1);
                    GOI.GETMES getmes = new GOI.GETMES(usrname, receiv);
                    string getmesstr = JsonSerializer.Serialize<GOI.GETMES>(getmes);
                    GOI.THUONG goi = new GOI.THUONG("getallmesgr", getmesstr);
                    sendJson(goi);
                }
                //if (tabuser.FocusedItem == null) return;
                //int p = tabuser.FocusedItem.Index;

            }
            catch (Exception ex)
            {

            }
        }

        private void gui_Click(object sender, EventArgs e)
        {
            if (txttn.Text.Length != 0)
            {
                tennhomhientai = listgroup.SelectedItems[0].Text;
                string receiver = tennhomhientai.Substring(tennhomhientai.IndexOf(" ") + 1);
                GOI.TINNHANGR mes = new GOI.TINNHANGR(usrname, receiver, txttn.Text);
                string jsonString = JsonSerializer.Serialize(mes);
                GOI.THUONG common = new GOI.THUONG("tinnhangr", jsonString);
                sendJson(common);
                AppendTextBoxgr(mes, "gui");
            }
            txttn.Clear();
        }


        private void btnguihinhgroup_Click(object sender, EventArgs e)
        {
            //MO DIALOG CHON HINH//
            OpenFileDialog layhinh = new OpenFileDialog();
            layhinh.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            layhinh.Multiselect = false;
            string path = "";
            string tenhinh = "";
            object clipboarddata = Clipboard.GetDataObject;
            khungchatgr.AppendText(Environment.NewLine + " ");

            //CLEAR LIST VI TRI HINH //
            listvitrihinhgroup.Clear();
            vitrihinhgr = 0;
            if (layhinh.ShowDialog() == DialogResult.OK)
            {
                path = layhinh.FileName;
                string imagepath = path.ToString();
                imagepath = imagepath.Substring(imagepath.LastIndexOf("\\"));
                tenhinh = imagepath.Remove(0, 1);

                Image hinh = Image.FromFile(path);
                Image hinhresize = (Image)ResizeImage(hinh, 150, 150);
                Clipboard.SetImage(hinhresize);
                khungchatgr.ReadOnly = false;
                khungchatgr.AppendText(Environment.NewLine + " ");
                if (khungchatgr.Lines.Length != 0)
                {

                    khungchatgr.SelectionStart = khungchatgr.GetFirstCharIndexOfCurrentLine();
                }
                khungchatgr.Paste();
                khungchatgr.SelectionAlignment = HorizontalAlignment.Right;
                khungchatgr.ReadOnly = true;

                Clipboard.SetDataObject(clipboarddata);

                //GUI HINH QUA TCPCLIENT//
                byte[] bStream = ImageToByte(hinhresize);
                string itemstr = listgroup.SelectedItems[0].Text;
                string receivergr = itemstr.Substring(itemstr.IndexOf(" ") + 1);
                GOI.GUIHINH guihinh = new GOI.GUIHINH(usrname, receivergr, bStream, tenhinh);
                string guihinhstr = JsonSerializer.Serialize(guihinh);
                GOI.THUONG goi = new GOI.THUONG("guihinhchogroup", guihinhstr);
                sendJson(goi);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            GOI.THUONG logout = new GOI.THUONG("logout", usrname);
            sendJson(logout);
        }

        private void menutab_Selected(object sender, TabControlEventArgs e)
        {

            try
            {
                if (e.TabPageIndex == 2)
                {
                    DialogResult dr = MessageBox.Show("Ban co chac chan muon dang xuat?",
                      "Dang xuat", MessageBoxButtons.YesNo);
                    if(dr == DialogResult.Yes)
                    {
                        GOI.THUONG logout = new GOI.THUONG("logout", usrname);
                        sendJson(logout);
                        this.Close();
                        Shared.taikhoan = "";
                        Shared.matkhau = "";
                        Thread loginformThread = new Thread(openLoginForm);
                        loginformThread.SetApartmentState(ApartmentState.STA);
                        loginformThread.Start();
                    }
                }
                if (e.TabPageIndex == 1)
                {
                        GOI.THUONG laygroup = new GOI.THUONG("getallgroup", usrname);
                        sendJson(laygroup);
                    //this.Close();
                    //Shared.taikhoan = "";
                    //Shared.matkhau = "";
                }
                if(e.TabPageIndex == 0)
                {
                    GOI.THUONG layuser = new GOI.THUONG("getonlineusers", usrname);
                    sendJson(layuser);
                }
            }
            catch(Exception ex)
            {
                
            }
        }

        private void ThreadTask(StreamReader sr, StreamWriter sw)
        {
            Shared.MainFormthoat = false;
            string jsonString = "";
            THUONG layuser = new THUONG("getonlineusers", usrname);
            sendJson(layuser);

            try
            {
                while (!Shared.MainFormthoat)
                {

                    jsonString = sr.ReadLine();
                    if (jsonString != null)
                    {
				        THUONG goi = JsonSerializer.Deserialize<GOI.THUONG>(jsonString);
				        if(goi != null)
                        {
                            switch (goi.kind)
                            {
                                case "tinnhan":
                                    TINNHAN? mes = JsonSerializer.Deserialize<GOI.TINNHAN>(goi.content);
                                    AppendTextBox(mes.content, "nhan");
                                    break;
                                case "tinnhangr":
                                    TINNHANGR? mesgr = JsonSerializer.Deserialize<GOI.TINNHANGR>(goi.content);
                                    AppendTextBoxgr(mesgr, "nhan");
                                    break;
                                case "getonlineusers":
                                    if (goi.content != null)
                                    {
                                        List<User> ls = JsonSerializer.Deserialize<List<User>>(goi.content);
                                        AppendTabUser(ls);
                                    }
                                    break;
                                case "getallgroup":
                                    if (goi.content != null)
                                    {
                                        List<Group> ls = JsonSerializer.Deserialize<List<Group>>(goi.content);
                                        AppendTabGroup(ls);
                                    }
                                    break;
                                case "getallmes":
                                    List<KeyValuePair<string, string>> dicmes = JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(goi.content);
                                    ShowAllMes(dicmes);
                                    break;
                                case "getallmesgr":
                                    List<KeyValuePair<string, string>> dicmesgr = JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(goi.content);
                                    ShowAllMesgr(dicmesgr);
                                    break;
                                case "guihinhchoclient":
                                    if (goi.content != null)
                                    {
                                        GOI.GUIHINH guihinh = JsonSerializer.Deserialize<GOI.GUIHINH>(goi.content);
                                        SenderAppendImageToTextBoxLeft(guihinh.manghinh);
                                    }
                                    break;
                                case "guihinhchogroup":
                                    if (goi.content != null)
                                    {
                                        GOI.GUIHINH guihinh = JsonSerializer.Deserialize<GOI.GUIHINH>(goi.content);
                                        SenderAppendImageToTextBoxGroupLeft(guihinh.manghinh);
                                    }
                                    break;
                                case "trahinhtusv":
                                    {
                                        if (goi.content != null)
                                        {

                                            GOI.TRAHINH guihinhtuSV = JsonSerializer.Deserialize<GOI.TRAHINH>(goi.content);

                                            if (guihinhtuSV.type == "sender")
                                            {
                                                AppendImageToTextBoxRight(guihinhtuSV.manghinh);
                                            }
                                            else
                                            {
                                                AppendImageToTextBoxLeft(guihinhtuSV.manghinh);
                                            }
                                        }
                                    }
                                    break;
                                case "trahinhtusvgroup":
                                    {
                                        if (goi.content != null)
                                        {

                                            GOI.TRAHINH guihinhtuSV = JsonSerializer.Deserialize<GOI.TRAHINH>(goi.content);

                                            if (guihinhtuSV.type == "sender")
                                            {
                                                AppendImageToTextBoxGroupRight(guihinhtuSV.manghinh);
                                            }
                                            else
                                            {
                                                AppendImageToTextBoxGroupLeft(guihinhtuSV.manghinh);
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        
                    }
                    
                }
            }
            catch(Exception ex)
            {
               // MessageBox.Show("Loi mang!");
            }
        }

       
    }
}
