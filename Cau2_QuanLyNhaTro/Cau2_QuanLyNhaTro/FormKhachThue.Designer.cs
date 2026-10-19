
namespace Cau2_QuanLyNhaTro
{
    partial class FormKhachThue
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            grpThongTin = new GroupBox();
            lblHoTen = new Label();
            txtHoTen = new TextBox();
            grpGioiTinh = new GroupBox();
            radNam = new RadioButton();
            radNu = new RadioButton();
            lblNgaySinh = new Label();
            dtpNgaySinh = new DateTimePicker();
            lblSDT = new Label();
            txtSDT = new TextBox();
            lblPhong = new Label();
            cboPhong = new ComboBox();
            lblTinh = new Label();
            cboTinh = new ComboBox();
            lblPhuong = new Label();
            cboPhuong = new ComboBox();
            chkTamTru = new CheckBox();

            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();

            lblTimKiem = new Label();
            txtTimKiem = new TextBox();
            lblLocPhong = new Label();
            cboLocPhong = new ComboBox();
            lblLocTinh = new Label();
            cboLocTinh = new ComboBox();
            btnTim = new Button();
            btnBoLoc = new Button();

            dgvKhachThue = new DataGridView();
            colHoTen = new DataGridViewTextBoxColumn();
            colSDT = new DataGridViewTextBoxColumn();
            colPhong = new DataGridViewTextBoxColumn();
            colGiaThue = new DataGridViewTextBoxColumn();
            colQueQuan = new DataGridViewTextBoxColumn();
            colTamTru = new DataGridViewCheckBoxColumn();

            lblTrangThai = new Label();
            btnGayLoiThu = new Button();

            grpThongTin.SuspendLayout();
            grpGioiTinh.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvKhachThue).BeginInit();
            SuspendLayout();

            // Thông tin khách thuê
            grpThongTin.Location = new Point(18, 18);
            grpThongTin.Size = new Size(445, 510);
            grpThongTin.Text = "Thông tin khách thuê";

            grpThongTin.Controls.AddRange(new Control[]
            {
                lblHoTen, txtHoTen,
                grpGioiTinh,
                lblNgaySinh, dtpNgaySinh,
                lblSDT, txtSDT,
                lblPhong, cboPhong,
                lblTinh, cboTinh,
                lblPhuong, cboPhuong,
                chkTamTru
            });

            // Họ tên
            lblHoTen.AutoSize = true;
            lblHoTen.Location = new Point(20, 37);
            lblHoTen.Text = "Họ tên";

            txtHoTen.Location = new Point(125, 33);
            txtHoTen.Size = new Size(290, 23);
            txtHoTen.MaxLength = 100;

            // Giới tính
            grpGioiTinh.Location = new Point(125, 73);
            grpGioiTinh.Size = new Size(290, 69);
            grpGioiTinh.Text = "Giới tính";

            grpGioiTinh.Controls.Add(radNam);
            grpGioiTinh.Controls.Add(radNu);

            radNam.Location = new Point(25, 29);
            radNam.Size = new Size(90, 23);
            radNam.Text = "Nam";
            radNam.Checked = true;

            radNu.Location = new Point(150, 29);
            radNu.Size = new Size(80, 23);
            radNu.Text = "Nữ";

            // Ngày sinh
            lblNgaySinh.AutoSize = true;
            lblNgaySinh.Location = new Point(20, 167);
            lblNgaySinh.Text = "Ngày sinh";

            dtpNgaySinh.Location = new Point(125, 163);
            dtpNgaySinh.Size = new Size(290, 23);
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.CustomFormat = "dd/MM/yyyy";

            // Số điện thoại
            lblSDT.AutoSize = true;
            lblSDT.Location = new Point(20, 213);
            lblSDT.Text = "Số điện thoại";

            txtSDT.Location = new Point(125, 209);
            txtSDT.Size = new Size(290, 23);
            txtSDT.MaxLength = 15;

            // Phòng
            lblPhong.AutoSize = true;
            lblPhong.Location = new Point(20, 259);
            lblPhong.Text = "Phòng";

            cboPhong.Location = new Point(125, 255);
            cboPhong.Size = new Size(290, 23);
            cboPhong.DropDownStyle = ComboBoxStyle.DropDownList;

            // Tỉnh/Thành
            lblTinh.AutoSize = true;
            lblTinh.Location = new Point(20, 305);
            lblTinh.Text = "Tỉnh/Thành";

            cboTinh.Location = new Point(125, 301);
            cboTinh.Size = new Size(290, 23);
            cboTinh.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTinh.SelectedIndexChanged += cboTinh_SelectedIndexChanged;

            // Phường/Xã
            lblPhuong.AutoSize = true;
            lblPhuong.Location = new Point(20, 351);
            lblPhuong.Text = "Phường/Xã";

            cboPhuong.Location = new Point(125, 347);
            cboPhuong.Size = new Size(290, 23);
            cboPhuong.DropDownStyle = ComboBoxStyle.DropDownList;

            // Tạm trú
            chkTamTru.AutoSize = true;
            chkTamTru.Location = new Point(22, 410);
            chkTamTru.Text = "Đã đăng ký tạm trú";

            // Buttons
            btnThem.Location = new Point(22, 544);
            btnThem.Size = new Size(95, 37);
            btnThem.Text = "Thêm";
            btnThem.Click += btnThem_Click;

            btnSua.Location = new Point(132, 544);
            btnSua.Size = new Size(95, 37);
            btnSua.Text = "Sửa";
            btnSua.Click += btnSua_Click;

            btnXoa.Location = new Point(242, 544);
            btnXoa.Size = new Size(95, 37);
            btnXoa.Text = "Xóa";
            btnXoa.Click += btnXoa_Click;

            btnLamMoi.Location = new Point(352, 544);
            btnLamMoi.Size = new Size(105, 37);
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;

            // Tìm kiếm
            lblTimKiem.AutoSize = true;
            lblTimKiem.Location = new Point(488, 24);
            lblTimKiem.Text = "Tìm theo tên hoặc SĐT";

            txtTimKiem.Location = new Point(488, 48);
            txtTimKiem.Size = new Size(310, 23);
            txtTimKiem.PlaceholderText = "Nhập tên hoặc số điện thoại...";

            lblLocPhong.AutoSize = true;
            lblLocPhong.Location = new Point(816, 24);
            lblLocPhong.Text = "Phòng";

            cboLocPhong.Location = new Point(816, 48);
            cboLocPhong.Size = new Size(175, 23);
            cboLocPhong.DropDownStyle = ComboBoxStyle.DropDownList;

            btnTim.Location = new Point(1010, 44);
            btnTim.Size = new Size(100, 31);
            btnTim.Text = "Tìm";
            btnTim.Click += btnTim_Click;

            // Lọc tỉnh
            lblLocTinh.AutoSize = true;
            lblLocTinh.Location = new Point(488, 88);
            lblLocTinh.Text = "Lọc tỉnh";

            cboLocTinh.Location = new Point(552, 84);
            cboLocTinh.Size = new Size(275, 23);
            cboLocTinh.DropDownStyle = ComboBoxStyle.DropDownList;

            btnBoLoc.Location = new Point(1010, 81);
            btnBoLoc.Size = new Size(100, 31);
            btnBoLoc.Text = "Bỏ lọc";
            btnBoLoc.Click += btnBoLoc_Click;

            // DataGridView
            dgvKhachThue.Location = new Point(488, 130);
            dgvKhachThue.Size = new Size(742, 485);

            dgvKhachThue.AllowUserToAddRows = false;
            dgvKhachThue.AllowUserToDeleteRows = false;
            dgvKhachThue.AutoGenerateColumns = false;
            dgvKhachThue.ReadOnly = true;
            dgvKhachThue.MultiSelect = false;
            dgvKhachThue.BackgroundColor = Color.White;

            dgvKhachThue.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvKhachThue.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvKhachThue.RowHeadersWidth = 25;

            dgvKhachThue.Columns.AddRange(
                colHoTen, colSDT, colPhong,
                colGiaThue, colQueQuan, colTamTru);

            dgvKhachThue.SelectionChanged +=
                dgvKhachThue_SelectionChanged;

            // Các cột
            colHoTen.Name = "colHoTen";
            colHoTen.HeaderText = "Họ tên";
            colHoTen.DataPropertyName = "HoTen";
            colHoTen.FillWeight = 120;

            colSDT.Name = "colSDT";
            colSDT.HeaderText = "SĐT";
            colSDT.DataPropertyName = "SoDienThoai";
            colSDT.FillWeight = 85;

            colPhong.Name = "colPhong";
            colPhong.HeaderText = "Phòng";
            colPhong.DataPropertyName = "TenPhong";
            colPhong.FillWeight = 65;

            colGiaThue.Name = "colGiaThue";
            colGiaThue.HeaderText = "Giá thuê";
            colGiaThue.DataPropertyName = "GiaThue";
            colGiaThue.DefaultCellStyle.Format = "N0";
            colGiaThue.FillWeight = 100;

            colQueQuan.Name = "colQueQuan";
            colQueQuan.HeaderText = "Quê quán";
            colQueQuan.DataPropertyName = "QueQuan";
            colQueQuan.FillWeight = 165;

            colTamTru.Name = "colTamTru";
            colTamTru.HeaderText = "Tạm trú";
            colTamTru.DataPropertyName = "DaDangKyTamTru";
            colTamTru.FillWeight = 65;
            colTamTru.TrueValue = true;
            colTamTru.FalseValue = false;

            // Trạng thái tải API
            lblTrangThai.AutoSize = false;
            lblTrangThai.Location = new Point(22, 645);
            lblTrangThai.Size = new Size(1160, 28);
            lblTrangThai.Text = "Chưa tải danh sách tỉnh/thành";

            // Nút gây lỗi thử (ẩn)
            btnGayLoiThu.Location = new Point(22, 595);
            btnGayLoiThu.Size = new Size(135, 32);
            btnGayLoiThu.Text = "Gây lỗi thử";
            btnGayLoiThu.Visible = false;
            btnGayLoiThu.Click += btnGayLoiThu_Click;

            // Form
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1250, 695);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý nhà trọ - Khách thuê";
            KeyPreview = true;
            KeyDown += FormKhachThue_KeyDown;

            Controls.Add(grpThongTin);
            Controls.Add(btnThem);
            Controls.Add(btnSua);
            Controls.Add(btnXoa);
            Controls.Add(btnLamMoi);
            Controls.Add(lblTimKiem);
            Controls.Add(txtTimKiem);
            Controls.Add(lblLocPhong);
            Controls.Add(cboLocPhong);
            Controls.Add(lblLocTinh);
            Controls.Add(cboLocTinh);
            Controls.Add(btnTim);
            Controls.Add(btnBoLoc);
            Controls.Add(dgvKhachThue);
            Controls.Add(lblTrangThai);
            Controls.Add(btnGayLoiThu);

            grpGioiTinh.ResumeLayout(false);
            grpThongTin.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvKhachThue).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private GroupBox grpThongTin;
        private Label lblHoTen;
        private TextBox txtHoTen;
        private GroupBox grpGioiTinh;
        private RadioButton radNam;
        private RadioButton radNu;
        private Label lblNgaySinh;
        private DateTimePicker dtpNgaySinh;
        private Label lblSDT;
        private TextBox txtSDT;
        private Label lblPhong;
        private ComboBox cboPhong;
        private Label lblTinh;
        private ComboBox cboTinh;
        private Label lblPhuong;
        private ComboBox cboPhuong;
        private CheckBox chkTamTru;

        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;

        private Label lblTimKiem;
        private TextBox txtTimKiem;
        private Label lblLocPhong;
        private ComboBox cboLocPhong;
        private Label lblLocTinh;
        private ComboBox cboLocTinh;
        private Button btnTim;
        private Button btnBoLoc;

        private DataGridView dgvKhachThue;
        private DataGridViewTextBoxColumn colHoTen;
        private DataGridViewTextBoxColumn colSDT;
        private DataGridViewTextBoxColumn colPhong;
        private DataGridViewTextBoxColumn colGiaThue;
        private DataGridViewTextBoxColumn colQueQuan;
        private DataGridViewCheckBoxColumn colTamTru;

        private Label lblTrangThai;
        private Button btnGayLoiThu;
    }
}
