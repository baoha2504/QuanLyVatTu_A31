namespace QuanLyVatTu.GUI.Share
{
    partial class frm_XemAnh
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_XemAnh));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnXoayTrai = new DevComponents.DotNetBar.ButtonX();
            this.img_XoayTrai = new System.Windows.Forms.Panel();
            this.btnXoayPhai = new DevComponents.DotNetBar.ButtonX();
            this.imgXoayPhai = new System.Windows.Forms.Panel();
            this.btnDoiXung = new DevComponents.DotNetBar.ButtonX();
            this.img_DoiXung = new System.Windows.Forms.Panel();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.panel1.SuspendLayout();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.btnXoayTrai.SuspendLayout();
            this.btnXoayPhai.SuspendLayout();
            this.btnDoiXung.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelEx1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1382, 1053);
            this.panel1.TabIndex = 0;
            // 
            // panelEx1
            // 
            this.panelEx1.AutoScroll = true;
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.pictureBox);
            this.panelEx1.Controls.Add(this.btnXoayTrai);
            this.panelEx1.Controls.Add(this.btnXoayPhai);
            this.panelEx1.Controls.Add(this.btnDoiXung);
            this.panelEx1.Controls.Add(this.panelEx2);
            this.panelEx1.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(1382, 1053);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 18;
            // 
            // pictureBox
            // 
            this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox.Location = new System.Drawing.Point(0, 53);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(1382, 1000);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // btnXoayTrai
            // 
            this.btnXoayTrai.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnXoayTrai.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnXoayTrai.Controls.Add(this.img_XoayTrai);
            this.btnXoayTrai.Location = new System.Drawing.Point(422, 8);
            this.btnXoayTrai.Name = "btnXoayTrai";
            this.btnXoayTrai.Size = new System.Drawing.Size(120, 35);
            this.btnXoayTrai.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnXoayTrai.TabIndex = 5;
            this.btnXoayTrai.Click += new System.EventHandler(this.btnXoayTrai_Click);
            // 
            // img_XoayTrai
            // 
            this.img_XoayTrai.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("img_XoayTrai.BackgroundImage")));
            this.img_XoayTrai.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.img_XoayTrai.Location = new System.Drawing.Point(47, 4);
            this.img_XoayTrai.Name = "img_XoayTrai";
            this.img_XoayTrai.Size = new System.Drawing.Size(27, 27);
            this.img_XoayTrai.TabIndex = 0;
            this.img_XoayTrai.Click += new System.EventHandler(this.img_XoayTrai_Click);
            // 
            // btnXoayPhai
            // 
            this.btnXoayPhai.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnXoayPhai.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnXoayPhai.Controls.Add(this.imgXoayPhai);
            this.btnXoayPhai.Location = new System.Drawing.Point(833, 8);
            this.btnXoayPhai.Name = "btnXoayPhai";
            this.btnXoayPhai.Size = new System.Drawing.Size(120, 35);
            this.btnXoayPhai.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnXoayPhai.TabIndex = 4;
            this.btnXoayPhai.Click += new System.EventHandler(this.btnXoayPhai_Click);
            // 
            // imgXoayPhai
            // 
            this.imgXoayPhai.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("imgXoayPhai.BackgroundImage")));
            this.imgXoayPhai.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.imgXoayPhai.Location = new System.Drawing.Point(45, 4);
            this.imgXoayPhai.Name = "imgXoayPhai";
            this.imgXoayPhai.Size = new System.Drawing.Size(27, 27);
            this.imgXoayPhai.TabIndex = 0;
            this.imgXoayPhai.Click += new System.EventHandler(this.imgXoayPhai_Click);
            // 
            // btnDoiXung
            // 
            this.btnDoiXung.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDoiXung.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDoiXung.Controls.Add(this.img_DoiXung);
            this.btnDoiXung.Location = new System.Drawing.Point(627, 8);
            this.btnDoiXung.Name = "btnDoiXung";
            this.btnDoiXung.Size = new System.Drawing.Size(120, 35);
            this.btnDoiXung.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnDoiXung.TabIndex = 3;
            this.btnDoiXung.Click += new System.EventHandler(this.btnDoiXung_Click);
            // 
            // img_DoiXung
            // 
            this.img_DoiXung.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("img_DoiXung.BackgroundImage")));
            this.img_DoiXung.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.img_DoiXung.Location = new System.Drawing.Point(46, 4);
            this.img_DoiXung.Name = "img_DoiXung";
            this.img_DoiXung.Size = new System.Drawing.Size(27, 27);
            this.img_DoiXung.TabIndex = 0;
            this.img_DoiXung.Click += new System.EventHandler(this.img_DoiXung_Click);
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx2.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx2.Location = new System.Drawing.Point(0, 0);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(1382, 50);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 6;
            // 
            // frm_XemAnh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1382, 1053);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_XemAnh";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frm_XemAnh";
            this.panel1.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.btnXoayTrai.ResumeLayout(false);
            this.btnXoayPhai.ResumeLayout(false);
            this.btnDoiXung.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.PictureBox pictureBox;
        private DevComponents.DotNetBar.ButtonX btnXoayTrai;
        private DevComponents.DotNetBar.ButtonX btnXoayPhai;
        private DevComponents.DotNetBar.ButtonX btnDoiXung;
        private System.Windows.Forms.Panel img_XoayTrai;
        private System.Windows.Forms.Panel imgXoayPhai;
        private System.Windows.Forms.Panel img_DoiXung;
        private DevComponents.DotNetBar.PanelEx panelEx2;
    }
}