namespace DoAn
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "",
            "Status: Online"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("");
            this.menutab = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnguihinh = new MaterialSkin.Controls.MaterialButton();
            this.txtchat = new System.Windows.Forms.TextBox();
            this.tabuser = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.khungchat = new System.Windows.Forms.RichTextBox();
            this.btngui = new MaterialSkin.Controls.MaterialButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnguihinhgroup = new MaterialSkin.Controls.MaterialButton();
            this.txttn = new System.Windows.Forms.TextBox();
            this.gui = new MaterialSkin.Controls.MaterialButton();
            this.btntaonhom = new MaterialSkin.Controls.MaterialButton();
            this.khungchatgr = new System.Windows.Forms.RichTextBox();
            this.listgroup = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.menutab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menutab
            // 
            this.menutab.Controls.Add(this.tabPage1);
            this.menutab.Controls.Add(this.tabPage2);
            this.menutab.Controls.Add(this.tabPage3);
            this.menutab.Depth = 0;
            this.menutab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menutab.ImageList = this.imageList1;
            this.menutab.ItemSize = new System.Drawing.Size(120, 35);
            this.menutab.Location = new System.Drawing.Point(3, 64);
            this.menutab.MouseState = MaterialSkin.MouseState.HOVER;
            this.menutab.Multiline = true;
            this.menutab.Name = "menutab";
            this.menutab.SelectedIndex = 0;
            this.menutab.Size = new System.Drawing.Size(921, 539);
            this.menutab.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.menutab.TabIndex = 2;
            this.menutab.Selected += new System.Windows.Forms.TabControlEventHandler(this.menutab_Selected);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.btnguihinh);
            this.tabPage1.Controls.Add(this.txtchat);
            this.tabPage1.Controls.Add(this.tabuser);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.btngui);
            this.tabPage1.ImageKey = "message.jpg";
            this.tabPage1.Location = new System.Drawing.Point(4, 39);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(913, 496);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Message";
            // 
            // btnguihinh
            // 
            this.btnguihinh.AutoSize = false;
            this.btnguihinh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnguihinh.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnguihinh.Depth = 0;
            this.btnguihinh.HighEmphasis = true;
            this.btnguihinh.Icon = null;
            this.btnguihinh.Location = new System.Drawing.Point(284, 413);
            this.btnguihinh.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnguihinh.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnguihinh.Name = "btnguihinh";
            this.btnguihinh.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnguihinh.Size = new System.Drawing.Size(127, 41);
            this.btnguihinh.TabIndex = 5;
            this.btnguihinh.Text = "Gửi hình";
            this.btnguihinh.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnguihinh.UseAccentColor = false;
            this.btnguihinh.UseVisualStyleBackColor = true;
            this.btnguihinh.Click += new System.EventHandler(this.btnguihinh_Click);
            // 
            // txtchat
            // 
            this.txtchat.Location = new System.Drawing.Point(281, 356);
            this.txtchat.Multiline = true;
            this.txtchat.Name = "txtchat";
            this.txtchat.Size = new System.Drawing.Size(364, 39);
            this.txtchat.TabIndex = 4;
            this.txtchat.TextChanged += new System.EventHandler(this.txtchat_TextChanged);
            // 
            // tabuser
            // 
            this.tabuser.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.tabuser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.tabuser.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tabuser.HideSelection = true;
            this.tabuser.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem3});
            this.tabuser.Location = new System.Drawing.Point(72, 18);
            this.tabuser.MultiSelect = false;
            this.tabuser.Name = "tabuser";
            this.tabuser.Size = new System.Drawing.Size(203, 328);
            this.tabuser.SmallImageList = this.imageList2;
            this.tabuser.TabIndex = 3;
            this.tabuser.UseCompatibleStateImageBehavior = false;
            this.tabuser.View = System.Windows.Forms.View.Details;
            this.tabuser.SelectedIndexChanged += new System.EventHandler(this.tabuser_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 200;
            // 
            // imageList2
            // 
            this.imageList2.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "usericon.png");
            this.imageList2.Images.SetKeyName(1, "online.png");
            this.imageList2.Images.SetKeyName(2, "offline.jpg");
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.khungchat);
            this.panel1.Location = new System.Drawing.Point(281, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(470, 328);
            this.panel1.TabIndex = 2;
            // 
            // khungchat
            // 
            this.khungchat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.khungchat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.khungchat.Location = new System.Drawing.Point(0, 0);
            this.khungchat.Name = "khungchat";
            this.khungchat.ReadOnly = true;
            this.khungchat.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.khungchat.Size = new System.Drawing.Size(468, 326);
            this.khungchat.TabIndex = 0;
            this.khungchat.Text = "";
            // 
            // btngui
            // 
            this.btngui.AutoSize = false;
            this.btngui.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btngui.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btngui.Depth = 0;
            this.btngui.HighEmphasis = true;
            this.btngui.Icon = null;
            this.btngui.Location = new System.Drawing.Point(652, 354);
            this.btngui.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btngui.MouseState = MaterialSkin.MouseState.HOVER;
            this.btngui.Name = "btngui";
            this.btngui.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btngui.Size = new System.Drawing.Size(99, 41);
            this.btngui.TabIndex = 1;
            this.btngui.Text = "Gửi";
            this.btngui.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btngui.UseAccentColor = false;
            this.btngui.UseVisualStyleBackColor = true;
            this.btngui.Click += new System.EventHandler(this.btngui_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.btnguihinhgroup);
            this.tabPage2.Controls.Add(this.txttn);
            this.tabPage2.Controls.Add(this.gui);
            this.tabPage2.Controls.Add(this.btntaonhom);
            this.tabPage2.Controls.Add(this.khungchatgr);
            this.tabPage2.Controls.Add(this.listgroup);
            this.tabPage2.ImageKey = "group.png";
            this.tabPage2.Location = new System.Drawing.Point(4, 39);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(913, 496);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Group";
            // 
            // btnguihinhgroup
            // 
            this.btnguihinhgroup.AutoSize = false;
            this.btnguihinhgroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnguihinhgroup.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnguihinhgroup.Depth = 0;
            this.btnguihinhgroup.HighEmphasis = true;
            this.btnguihinhgroup.Icon = null;
            this.btnguihinhgroup.Location = new System.Drawing.Point(357, 433);
            this.btnguihinhgroup.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnguihinhgroup.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnguihinhgroup.Name = "btnguihinhgroup";
            this.btnguihinhgroup.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnguihinhgroup.Size = new System.Drawing.Size(127, 41);
            this.btnguihinhgroup.TabIndex = 6;
            this.btnguihinhgroup.Text = "Gửi hình";
            this.btnguihinhgroup.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnguihinhgroup.UseAccentColor = false;
            this.btnguihinhgroup.UseVisualStyleBackColor = true;
            this.btnguihinhgroup.Click += new System.EventHandler(this.btnguihinhgroup_Click);
            // 
            // txttn
            // 
            this.txttn.Location = new System.Drawing.Point(357, 397);
            this.txttn.Name = "txttn";
            this.txttn.Size = new System.Drawing.Size(399, 27);
            this.txttn.TabIndex = 5;
            // 
            // gui
            // 
            this.gui.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gui.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.gui.Depth = 0;
            this.gui.HighEmphasis = true;
            this.gui.Icon = null;
            this.gui.Location = new System.Drawing.Point(763, 391);
            this.gui.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.gui.MouseState = MaterialSkin.MouseState.HOVER;
            this.gui.Name = "gui";
            this.gui.NoAccentTextColor = System.Drawing.Color.Empty;
            this.gui.Size = new System.Drawing.Size(64, 36);
            this.gui.TabIndex = 4;
            this.gui.Text = "Gui";
            this.gui.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.gui.UseAccentColor = false;
            this.gui.UseVisualStyleBackColor = true;
            this.gui.Click += new System.EventHandler(this.gui_Click);
            // 
            // btntaonhom
            // 
            this.btntaonhom.AutoSize = false;
            this.btntaonhom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btntaonhom.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btntaonhom.Depth = 0;
            this.btntaonhom.HighEmphasis = true;
            this.btntaonhom.Icon = null;
            this.btntaonhom.Location = new System.Drawing.Point(154, 391);
            this.btntaonhom.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btntaonhom.MouseState = MaterialSkin.MouseState.HOVER;
            this.btntaonhom.Name = "btntaonhom";
            this.btntaonhom.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btntaonhom.Size = new System.Drawing.Size(134, 38);
            this.btntaonhom.TabIndex = 2;
            this.btntaonhom.Text = "Tạo nhóm";
            this.btntaonhom.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btntaonhom.UseAccentColor = false;
            this.btntaonhom.UseVisualStyleBackColor = true;
            this.btntaonhom.Click += new System.EventHandler(this.btntaonhom_Click);
            // 
            // khungchatgr
            // 
            this.khungchatgr.Location = new System.Drawing.Point(357, 27);
            this.khungchatgr.Name = "khungchatgr";
            this.khungchatgr.ReadOnly = true;
            this.khungchatgr.Size = new System.Drawing.Size(470, 355);
            this.khungchatgr.TabIndex = 1;
            this.khungchatgr.Text = "";
            // 
            // listgroup
            // 
            this.listgroup.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.listgroup.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.listgroup.HideSelection = true;
            this.listgroup.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listgroup.Location = new System.Drawing.Point(85, 27);
            this.listgroup.MultiSelect = false;
            this.listgroup.Name = "listgroup";
            this.listgroup.Size = new System.Drawing.Size(266, 355);
            this.listgroup.SmallImageList = this.imageList3;
            this.listgroup.TabIndex = 3;
            this.listgroup.UseCompatibleStateImageBehavior = false;
            this.listgroup.View = System.Windows.Forms.View.Details;
            this.listgroup.SelectedIndexChanged += new System.EventHandler(this.listgroup_SelectedIndexChanged);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "";
            this.columnHeader2.Width = 200;
            // 
            // imageList3
            // 
            this.imageList3.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList3.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList3.ImageStream")));
            this.imageList3.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList3.Images.SetKeyName(0, "group.png");
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.White;
            this.tabPage3.ImageKey = "logout.jpg";
            this.tabPage3.Location = new System.Drawing.Point(4, 39);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(913, 496);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Logout";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.White;
            this.imageList1.Images.SetKeyName(0, "message.jpg");
            this.imageList1.Images.SetKeyName(1, "group.png");
            this.imageList1.Images.SetKeyName(2, "logout.jpg");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(927, 606);
            this.Controls.Add(this.menutab);
            this.DrawerShowIconsWhenHidden = true;
            this.DrawerTabControl = this.menutab;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menutab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialTabControl menutab;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private ImageList imageList1;
        private MaterialSkin.Controls.MaterialButton btngui;
        private Panel panel1;
        private RichTextBox khungchat;
        private ListView tabuser;
        private ColumnHeader columnHeader1;
        private ImageList imageList2;
        private TextBox txtchat;
        private MaterialSkin.Controls.MaterialButton btnguihinh;
        private MaterialSkin.Controls.MaterialButton btntaonhom;
        private RichTextBox khungchatgr;
        private ListView listgroup;
        private ColumnHeader columnHeader2;
        private MaterialSkin.Controls.MaterialButton gui;
        private TextBox txttn;
        private ImageList imageList3;
        private MaterialSkin.Controls.MaterialButton btnguihinhgroup;
    }
}