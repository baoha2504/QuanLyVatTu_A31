namespace QuanLyVatTu.GUI.User
{
    partial class frmUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUser));
            this.mainContainer = new DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer();
            this.accordionControl1 = new DevExpress.XtraBars.Navigation.AccordionControl();
            this.PHUONGANVATTU = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.btnLapPhuongAnVatTu = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.btnDanhSachPhuongAnVatTu = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement3 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.QUANLYVATTU = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.btnDanhSachVatTu = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.btnDanhMucVatTu = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.btnLocDanhSachVatTu = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.btnImportDanhSachVatTu = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement8 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.TROGIUP = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.btnDoiMatKhau = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.btnDangXuat = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement11 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.fluentDesignFormControl1 = new DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.lblTieuDe1 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem3 = new DevExpress.XtraBars.BarStaticItem();
            this.lblTieuDe2 = new DevExpress.XtraBars.BarStaticItem();
            this.lblNguoiDung = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem4 = new DevExpress.XtraBars.BarStaticItem();
            this.fluentFormDefaultManager1 = new DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentDesignFormControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentFormDefaultManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // mainContainer
            // 
            this.mainContainer.Appearance.BackColor = System.Drawing.Color.White;
            this.mainContainer.Appearance.Options.UseBackColor = true;
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(398, 39);
            this.mainContainer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.Size = new System.Drawing.Size(1000, 710);
            this.mainContainer.TabIndex = 0;
            // 
            // accordionControl1
            // 
            this.accordionControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.accordionControl1.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.PHUONGANVATTU,
            this.QUANLYVATTU,
            this.TROGIUP});
            this.accordionControl1.Location = new System.Drawing.Point(0, 39);
            this.accordionControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.accordionControl1.Name = "accordionControl1";
            this.accordionControl1.ScrollBarMode = DevExpress.XtraBars.Navigation.ScrollBarMode.Touch;
            this.accordionControl1.Size = new System.Drawing.Size(398, 710);
            this.accordionControl1.TabIndex = 1;
            this.accordionControl1.ViewType = DevExpress.XtraBars.Navigation.AccordionControlViewType.HamburgerMenu;
            // 
            // PHUONGANVATTU
            // 
            this.PHUONGANVATTU.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.btnLapPhuongAnVatTu,
            this.btnDanhSachPhuongAnVatTu,
            this.accordionControlElement3});
            this.PHUONGANVATTU.Expanded = true;
            this.PHUONGANVATTU.Name = "PHUONGANVATTU";
            this.PHUONGANVATTU.Text = "PHƯƠNG ÁN VẬT TƯ";
            // 
            // btnLapPhuongAnVatTu
            // 
            this.btnLapPhuongAnVatTu.Name = "btnLapPhuongAnVatTu";
            this.btnLapPhuongAnVatTu.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.btnLapPhuongAnVatTu.Text = "Lập phương án vật tư";
            this.btnLapPhuongAnVatTu.Click += new System.EventHandler(this.btnLapPhuongAnVatTu_Click);
            // 
            // btnDanhSachPhuongAnVatTu
            // 
            this.btnDanhSachPhuongAnVatTu.Name = "btnDanhSachPhuongAnVatTu";
            this.btnDanhSachPhuongAnVatTu.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.btnDanhSachPhuongAnVatTu.Text = "Danh sách phương án vật tư";
            this.btnDanhSachPhuongAnVatTu.Click += new System.EventHandler(this.btnDanhSachPhuongAnVatTu_Click);
            // 
            // accordionControlElement3
            // 
            this.accordionControlElement3.Name = "accordionControlElement3";
            this.accordionControlElement3.Visible = false;
            // 
            // QUANLYVATTU
            // 
            this.QUANLYVATTU.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.btnDanhSachVatTu,
            this.btnDanhMucVatTu,
            this.btnLocDanhSachVatTu,
            this.btnImportDanhSachVatTu,
            this.accordionControlElement8});
            this.QUANLYVATTU.Expanded = true;
            this.QUANLYVATTU.Name = "QUANLYVATTU";
            this.QUANLYVATTU.Text = "QUẢN LÝ VẬT TƯ";
            // 
            // btnDanhSachVatTu
            // 
            this.btnDanhSachVatTu.Name = "btnDanhSachVatTu";
            this.btnDanhSachVatTu.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.btnDanhSachVatTu.Text = "Danh sách vật tư";
            this.btnDanhSachVatTu.Click += new System.EventHandler(this.btnDanhSachVatTu_Click);
            // 
            // btnDanhMucVatTu
            // 
            this.btnDanhMucVatTu.Name = "btnDanhMucVatTu";
            this.btnDanhMucVatTu.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.btnDanhMucVatTu.Text = "Danh mục vật tư";
            this.btnDanhMucVatTu.Click += new System.EventHandler(this.btnDanhMucVatTu_Click);
            // 
            // btnLocDanhSachVatTu
            // 
            this.btnLocDanhSachVatTu.Name = "btnLocDanhSachVatTu";
            this.btnLocDanhSachVatTu.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.btnLocDanhSachVatTu.Text = "Lọc danh sách vật tư";
            this.btnLocDanhSachVatTu.Click += new System.EventHandler(this.btnLocDanhSachVatTu_Click);
            // 
            // btnImportDanhSachVatTu
            // 
            this.btnImportDanhSachVatTu.Name = "btnImportDanhSachVatTu";
            this.btnImportDanhSachVatTu.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.btnImportDanhSachVatTu.Text = "Import danh sách vật tư";
            this.btnImportDanhSachVatTu.Click += new System.EventHandler(this.btnImportDanhSachVatTu_Click);
            // 
            // accordionControlElement8
            // 
            this.accordionControlElement8.Name = "accordionControlElement8";
            this.accordionControlElement8.Visible = false;
            // 
            // TROGIUP
            // 
            this.TROGIUP.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.btnDoiMatKhau,
            this.btnDangXuat,
            this.accordionControlElement11});
            this.TROGIUP.Expanded = true;
            this.TROGIUP.Name = "TROGIUP";
            this.TROGIUP.Text = "TRỢ GIÚP";
            // 
            // btnDoiMatKhau
            // 
            this.btnDoiMatKhau.Name = "btnDoiMatKhau";
            this.btnDoiMatKhau.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.btnDoiMatKhau.Text = "Đổi mật khẩu";
            this.btnDoiMatKhau.Click += new System.EventHandler(this.btnDoiMatKhau_Click);
            // 
            // btnDangXuat
            // 
            this.btnDangXuat.Name = "btnDangXuat";
            this.btnDangXuat.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.btnDangXuat.Text = "Đăng xuất";
            this.btnDangXuat.Click += new System.EventHandler(this.btnDangXuat_Click);
            // 
            // accordionControlElement11
            // 
            this.accordionControlElement11.Name = "accordionControlElement11";
            this.accordionControlElement11.Visible = false;
            // 
            // fluentDesignFormControl1
            // 
            this.fluentDesignFormControl1.FluentDesignForm = this;
            this.fluentDesignFormControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barStaticItem1,
            this.lblTieuDe1,
            this.barStaticItem3,
            this.lblTieuDe2,
            this.lblNguoiDung,
            this.barStaticItem4});
            this.fluentDesignFormControl1.Location = new System.Drawing.Point(0, 0);
            this.fluentDesignFormControl1.Manager = this.fluentFormDefaultManager1;
            this.fluentDesignFormControl1.Name = "fluentDesignFormControl1";
            this.fluentDesignFormControl1.Size = new System.Drawing.Size(1398, 39);
            this.fluentDesignFormControl1.TabIndex = 2;
            this.fluentDesignFormControl1.TabStop = false;
            this.fluentDesignFormControl1.TitleItemLinks.Add(this.barButtonItem1);
            this.fluentDesignFormControl1.TitleItemLinks.Add(this.barStaticItem1);
            this.fluentDesignFormControl1.TitleItemLinks.Add(this.lblTieuDe1);
            this.fluentDesignFormControl1.TitleItemLinks.Add(this.barStaticItem3);
            this.fluentDesignFormControl1.TitleItemLinks.Add(this.lblTieuDe2);
            this.fluentDesignFormControl1.TitleItemLinks.Add(this.lblNguoiDung);
            this.fluentDesignFormControl1.TitleItemLinks.Add(this.barStaticItem4);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.ImageOptions.Image")));
            this.barButtonItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.ImageOptions.LargeImage")));
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = ">";
            this.barStaticItem1.Id = 1;
            this.barStaticItem1.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.barStaticItem1.ItemAppearance.Normal.Options.UseFont = true;
            this.barStaticItem1.Name = "barStaticItem1";
            // 
            // lblTieuDe1
            // 
            this.lblTieuDe1.Caption = "Tiêu đề 1";
            this.lblTieuDe1.Id = 2;
            this.lblTieuDe1.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblTieuDe1.ItemAppearance.Normal.Options.UseFont = true;
            this.lblTieuDe1.Name = "lblTieuDe1";
            // 
            // barStaticItem3
            // 
            this.barStaticItem3.Caption = ">";
            this.barStaticItem3.Id = 3;
            this.barStaticItem3.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.barStaticItem3.ItemAppearance.Normal.Options.UseFont = true;
            this.barStaticItem3.Name = "barStaticItem3";
            // 
            // lblTieuDe2
            // 
            this.lblTieuDe2.Caption = "Tiêu đề 2";
            this.lblTieuDe2.Id = 4;
            this.lblTieuDe2.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblTieuDe2.ItemAppearance.Normal.Options.UseFont = true;
            this.lblTieuDe2.Name = "lblTieuDe2";
            // 
            // lblNguoiDung
            // 
            this.lblNguoiDung.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.lblNguoiDung.Caption = "Họ tên";
            this.lblNguoiDung.Id = 5;
            this.lblNguoiDung.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNguoiDung.ItemAppearance.Normal.ForeColor = System.Drawing.Color.Red;
            this.lblNguoiDung.ItemAppearance.Normal.Options.UseFont = true;
            this.lblNguoiDung.ItemAppearance.Normal.Options.UseForeColor = true;
            this.lblNguoiDung.Name = "lblNguoiDung";
            // 
            // barStaticItem4
            // 
            this.barStaticItem4.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItem4.Caption = "Xin chào:";
            this.barStaticItem4.Id = 6;
            this.barStaticItem4.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.barStaticItem4.ItemAppearance.Normal.Options.UseFont = true;
            this.barStaticItem4.Name = "barStaticItem4";
            // 
            // fluentFormDefaultManager1
            // 
            this.fluentFormDefaultManager1.Form = this;
            this.fluentFormDefaultManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barStaticItem1,
            this.lblTieuDe1,
            this.barStaticItem3,
            this.lblTieuDe2,
            this.lblNguoiDung,
            this.barStaticItem4});
            this.fluentFormDefaultManager1.MaxItemId = 7;
            // 
            // frmUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1398, 749);
            this.ControlContainer = this.mainContainer;
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.accordionControl1);
            this.Controls.Add(this.fluentDesignFormControl1);
            this.FluentDesignFormControl = this.fluentDesignFormControl1;
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("frmUser.IconOptions.Image")));
            this.Name = "frmUser";
            this.NavigationControl = this.accordionControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PHẦN MỀM QUẢN LÝ VẬT TƯ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentDesignFormControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentFormDefaultManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer mainContainer;
        private DevExpress.XtraBars.Navigation.AccordionControl accordionControl1;
        private DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl fluentDesignFormControl1;
        private DevExpress.XtraBars.Navigation.AccordionControlElement PHUONGANVATTU;
        private DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager fluentFormDefaultManager1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarStaticItem lblTieuDe1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem3;
        private DevExpress.XtraBars.BarStaticItem lblTieuDe2;
        private DevExpress.XtraBars.BarStaticItem barStaticItem4;
        public DevExpress.XtraBars.BarStaticItem lblNguoiDung;
        private DevExpress.XtraBars.Navigation.AccordionControlElement QUANLYVATTU;
        private DevExpress.XtraBars.Navigation.AccordionControlElement TROGIUP;
        private DevExpress.XtraBars.Navigation.AccordionControlElement btnLapPhuongAnVatTu;
        private DevExpress.XtraBars.Navigation.AccordionControlElement btnDanhSachPhuongAnVatTu;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement3;
        private DevExpress.XtraBars.Navigation.AccordionControlElement btnDanhSachVatTu;
        private DevExpress.XtraBars.Navigation.AccordionControlElement btnDanhMucVatTu;
        private DevExpress.XtraBars.Navigation.AccordionControlElement btnLocDanhSachVatTu;
        private DevExpress.XtraBars.Navigation.AccordionControlElement btnImportDanhSachVatTu;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement8;
        private DevExpress.XtraBars.Navigation.AccordionControlElement btnDoiMatKhau;
        private DevExpress.XtraBars.Navigation.AccordionControlElement btnDangXuat;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement11;
    }
}