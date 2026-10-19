
namespace Cau3_KeHoachDaNgoai
{
    partial class FormChuyenDi
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
            lblTieuDe = new Label();
            lblNgonNgu = new Label();
            cboNgonNgu = new ComboBox();

            grpThongTin = new GroupBox();

            lblTenChuyen = new Label();
            txtTenChuyen = new TextBox();

            lblDiaDiem = new Label();
            txtDiaDiem = new TextBox();

            lblNgayDi = new Label();
            dtpNgayDi = new DateTimePicker();

            lblSoNguoi = new Label();
            nudSoNguoi = new NumericUpDown();

            lblNganSach = new Label();
            nudNganSach = new NumericUpDown();

            lblPhuongTien = new Label();
            cboPhuongTien = new ComboBox();

            lblGhiChu = new Label();
            txtGhiChu = new TextBox();

            chkDaHoanThanh = new CheckBox();

            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();

            btnXemThoiTiet = new Button();
            prgThoiTiet = new ProgressBar();

            pnlThoiTiet = new Panel();
            lblTieuDeThoiTiet = new Label();
            lblBieuTuong = new Label();
            lblMoTa = new Label();
            lblNgayDuBao = new Label();
            lblNhietDo = new Label();
            lblXacSuatMua = new Label();
            lblLoiKhuyen = new Label();

            lblTimKiem = new Label();
            txtTimKiem = new TextBox();
            chkChuaHoanThanh = new CheckBox();

            lblTuNgay = new Label();
            dtpTuNgay = new DateTimePicker();

            lblDenNgay = new Label();
            dtpDenNgay = new DateTimePicker();

            btnTim = new Button();
            btnBoLoc = new Button();

            dgvChuyenDi = new DataGridView();

            colMaChuyen = new DataGridViewTextBoxColumn();
            colTenChuyen = new DataGridViewTextBoxColumn();
            colDiaDiem = new DataGridViewTextBoxColumn();
            colNgayDi = new DataGridViewTextBoxColumn();
            colSoNguoi = new DataGridViewTextBoxColumn();
            colNganSach = new DataGridViewTextBoxColumn();
            colPhuongTien = new DataGridViewTextBoxColumn();
            colDaHoanThanh = new DataGridViewCheckBoxColumn();

            lblTrangThai = new Label();

            grpThongTin.SuspendLayout();
            pnlThoiTiet.SuspendLayout();

            ((System.ComponentModel.ISupportInitialize)nudSoNguoi).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudNganSach).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvChuyenDi).BeginInit();

            SuspendLayout();

            // Tiêu đề
            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new Font(
                "Segoe UI", 14F, FontStyle.Bold);

            lblTieuDe.Location = new Point(20, 15);
            lblTieuDe.Text = "KẾ HOẠCH DÃ NGOẠI CUỐI TUẦN";

            // Ngôn ngữ
            lblNgonNgu.AutoSize = true;
            lblNgonNgu.Location = new Point(860, 24);
            lblNgonNgu.Text = "Ngôn ngữ";

            cboNgonNgu.Location = new Point(940, 20);
            cboNgonNgu.Size = new Size(155, 23);
            cboNgonNgu.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cboNgonNgu.SelectedIndexChanged +=
                cboNgonNgu_SelectedIndexChanged;

            // Group thông tin
            grpThongTin.Location = new Point(20, 57);
            grpThongTin.Size = new Size(550, 275);
            grpThongTin.Text = "Thông tin chuyến đi";

            grpThongTin.Controls.AddRange(new Control[]
            {
                lblTenChuyen, txtTenChuyen,
                lblDiaDiem, txtDiaDiem,
                lblNgayDi, dtpNgayDi,
                lblSoNguoi, nudSoNguoi,
                lblNganSach, nudNganSach,
                lblPhuongTien, cboPhuongTien,
                lblGhiChu, txtGhiChu,
                chkDaHoanThanh
            });

            // Tên chuyến
            lblTenChuyen.AutoSize = true;
            lblTenChuyen.Location = new Point(15, 33);
            lblTenChuyen.Text = "Tên chuyến";

            txtTenChuyen.Location = new Point(125, 29);
            txtTenChuyen.Size = new Size(285, 23);
            txtTenChuyen.MaxLength = 100;

            // Địa điểm
            lblDiaDiem.AutoSize = true;
            lblDiaDiem.Location = new Point(15, 68);
            lblDiaDiem.Text = "Địa điểm";

            txtDiaDiem.Location = new Point(125, 64);
            txtDiaDiem.Size = new Size(285, 23);
            txtDiaDiem.MaxLength = 100;

            // Ngày đi
            lblNgayDi.AutoSize = true;
            lblNgayDi.Location = new Point(15, 103);
            lblNgayDi.Text = "Ngày đi";

            dtpNgayDi.Location = new Point(125, 99);
            dtpNgayDi.Size = new Size(175, 23);
            dtpNgayDi.Format = DateTimePickerFormat.Custom;
            dtpNgayDi.CustomFormat = "dd/MM/yyyy";

            // Số người
            lblSoNguoi.AutoSize = true;
            lblSoNguoi.Location = new Point(15, 138);
            lblSoNguoi.Text = "Số người";

            nudSoNguoi.Location = new Point(125, 134);
            nudSoNguoi.Size = new Size(175, 23);
            nudSoNguoi.Minimum = 1;
            nudSoNguoi.Maximum = 1000;
            nudSoNguoi.Value = 1;

            // Ngân sách
            lblNganSach.AutoSize = true;
            lblNganSach.Location = new Point(15, 173);
            lblNganSach.Text = "Ngân sách";

            nudNganSach.Location = new Point(125, 169);
            nudNganSach.Size = new Size(210, 23);
            nudNganSach.Maximum = 1000000000000M;
            nudNganSach.Increment = 100000;
            nudNganSach.ThousandsSeparator = true;

            // Phương tiện
            lblPhuongTien.AutoSize = true;
            lblPhuongTien.Location = new Point(15, 208);
            lblPhuongTien.Text = "Phương tiện";

            cboPhuongTien.Location = new Point(125, 204);
            cboPhuongTien.Size = new Size(210, 23);
            cboPhuongTien.DropDownStyle =
                ComboBoxStyle.DropDownList;

            // Ghi chú
            lblGhiChu.AutoSize = true;
            lblGhiChu.Location = new Point(15, 243);
            lblGhiChu.Text = "Ghi chú";

            txtGhiChu.Location = new Point(125, 239);
            txtGhiChu.Size = new Size(285, 25);
            txtGhiChu.MaxLength = 200;

            // Hoàn thành
            chkDaHoanThanh.AutoSize = true;
            chkDaHoanThanh.Location = new Point(420, 205);
            chkDaHoanThanh.Text = "Đã hoàn thành";

            // CRUD
            btnThem.Location = new Point(20, 345);
            btnThem.Size = new Size(95, 33);
            btnThem.Text = "Thêm";
            btnThem.Click += btnThem_Click;

            btnSua.Location = new Point(128, 345);
            btnSua.Size = new Size(95, 33);
            btnSua.Text = "Sửa";
            btnSua.Click += btnSua_Click;

            btnXoa.Location = new Point(236, 345);
            btnXoa.Size = new Size(95, 33);
            btnXoa.Text = "Xóa";
            btnXoa.Click += btnXoa_Click;

            btnLamMoi.Location = new Point(344, 345);
            btnLamMoi.Size = new Size(110, 33);
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;

            // Xem thời tiết
            btnXemThoiTiet.Location = new Point(590, 58);
            btnXemThoiTiet.Size = new Size(155, 34);
            btnXemThoiTiet.Text = "Xem thời tiết";
            btnXemThoiTiet.Click += btnXemThoiTiet_Click;

            prgThoiTiet.Location = new Point(765, 67);
            prgThoiTiet.Size = new Size(310, 17);
            prgThoiTiet.Style = ProgressBarStyle.Marquee;
            prgThoiTiet.MarqueeAnimationSpeed = 30;
            prgThoiTiet.Visible = false;

            // Panel kết quả thời tiết
            pnlThoiTiet.Location = new Point(590, 105);
            pnlThoiTiet.Size = new Size(505, 227);
            pnlThoiTiet.BorderStyle = BorderStyle.FixedSingle;
            pnlThoiTiet.BackColor = Color.White;

            pnlThoiTiet.Controls.AddRange(new Control[]
            {
                lblTieuDeThoiTiet,
                lblBieuTuong,
                lblMoTa,
                lblNgayDuBao,
                lblNhietDo,
                lblXacSuatMua,
                lblLoiKhuyen
            });

            lblTieuDeThoiTiet.AutoSize = true;
            lblTieuDeThoiTiet.Font =
                new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTieuDeThoiTiet.Location = new Point(15, 12);
            lblTieuDeThoiTiet.Text = "Dự báo thời tiết";

            lblBieuTuong.AutoSize = false;
            lblBieuTuong.Location = new Point(20, 54);
            lblBieuTuong.Size = new Size(100, 85);
            lblBieuTuong.Font =
                new Font("Segoe UI Emoji", 42F);
            lblBieuTuong.Text = "🌤️";
            lblBieuTuong.TextAlign =
                ContentAlignment.MiddleCenter;

            lblMoTa.AutoSize = false;
            lblMoTa.Font =
                new Font("Segoe UI", 11F, FontStyle.Bold);
            lblMoTa.Location = new Point(140, 57);
            lblMoTa.Size = new Size(345, 25);
            lblMoTa.Text = "Chọn chuyến để xem dự báo";

            lblNgayDuBao.AutoSize = false;
            lblNgayDuBao.Location = new Point(140, 87);
            lblNgayDuBao.Size = new Size(340, 22);

            lblNhietDo.AutoSize = false;
            lblNhietDo.Location = new Point(140, 114);
            lblNhietDo.Size = new Size(340, 22);

            lblXacSuatMua.AutoSize = false;
            lblXacSuatMua.Location = new Point(140, 141);
            lblXacSuatMua.Size = new Size(340, 22);

            lblLoiKhuyen.AutoSize = false;
            lblLoiKhuyen.Location = new Point(18, 177);
            lblLoiKhuyen.Size = new Size(467, 36);
            lblLoiKhuyen.TextAlign =
                ContentAlignment.MiddleLeft;

            // Tìm kiếm
            lblTimKiem.AutoSize = true;
            lblTimKiem.Location = new Point(20, 405);
            lblTimKiem.Text = "Tìm kiếm";

            txtTimKiem.Location = new Point(95, 401);
            txtTimKiem.Size = new Size(248, 23);

            chkChuaHoanThanh.AutoSize = true;
            chkChuaHoanThanh.Location = new Point(355, 403);
            chkChuaHoanThanh.Text = "Chỉ hiện chuyến chưa hoàn thành";

            lblTuNgay.AutoSize = true;
            lblTuNgay.Location = new Point(585, 405);
            lblTuNgay.Text = "Từ";

            dtpTuNgay.Location = new Point(612, 401);
            dtpTuNgay.Size = new Size(110, 23);
            dtpTuNgay.Format = DateTimePickerFormat.Custom;
            dtpTuNgay.CustomFormat = "dd/MM/yyyy";

            lblDenNgay.AutoSize = true;
            lblDenNgay.Location = new Point(732, 405);
            lblDenNgay.Text = "Đến";

            dtpDenNgay.Location = new Point(764, 401);
            dtpDenNgay.Size = new Size(110, 23);
            dtpDenNgay.Format = DateTimePickerFormat.Custom;
            dtpDenNgay.CustomFormat = "dd/MM/yyyy";

            btnTim.Location = new Point(888, 396);
            btnTim.Size = new Size(85, 33);
            btnTim.Text = "Tìm";
            btnTim.Click += btnTim_Click;

            btnBoLoc.Location = new Point(985, 396);
            btnBoLoc.Size = new Size(110, 33);
            btnBoLoc.Text = "Bỏ lọc";
            btnBoLoc.Click += btnBoLoc_Click;

            // DataGridView
            dgvChuyenDi.Location = new Point(20, 445);
            dgvChuyenDi.Size = new Size(1075, 230);
            dgvChuyenDi.BackgroundColor = Color.White;

            dgvChuyenDi.AllowUserToAddRows = false;
            dgvChuyenDi.AllowUserToDeleteRows = false;
            dgvChuyenDi.AutoGenerateColumns = false;
            dgvChuyenDi.ReadOnly = true;
            dgvChuyenDi.MultiSelect = false;

            dgvChuyenDi.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvChuyenDi.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvChuyenDi.Columns.AddRange(
                colMaChuyen,
                colTenChuyen,
                colDiaDiem,
                colNgayDi,
                colSoNguoi,
                colNganSach,
                colPhuongTien,
                colDaHoanThanh);

            dgvChuyenDi.SelectionChanged +=
                dgvChuyenDi_SelectionChanged;

            colMaChuyen.Name = "colMaChuyen";
            colMaChuyen.HeaderText = "Mã";
            colMaChuyen.DataPropertyName = "MaChuyen";
            colMaChuyen.FillWeight = 40;

            colTenChuyen.Name = "colTenChuyen";
            colTenChuyen.HeaderText = "Tên chuyến";
            colTenChuyen.DataPropertyName = "TenChuyen";
            colTenChuyen.FillWeight = 135;

            colDiaDiem.Name = "colDiaDiem";
            colDiaDiem.HeaderText = "Địa điểm";
            colDiaDiem.DataPropertyName = "DiaDiem";
            colDiaDiem.FillWeight = 90;

            colNgayDi.Name = "colNgayDi";
            colNgayDi.HeaderText = "Ngày đi";
            colNgayDi.DataPropertyName = "NgayDi";
            colNgayDi.FillWeight = 90;

            colSoNguoi.Name = "colSoNguoi";
            colSoNguoi.HeaderText = "Số người";
            colSoNguoi.DataPropertyName = "SoNguoi";
            colSoNguoi.FillWeight = 65;

            colNganSach.Name = "colNganSach";
            colNganSach.HeaderText = "Ngân sách";
            colNganSach.DataPropertyName = "NganSach";
            colNganSach.FillWeight = 95;

            colPhuongTien.Name = "colPhuongTien";
            colPhuongTien.HeaderText = "Phương tiện";
            colPhuongTien.DataPropertyName = "PhuongTien";
            colPhuongTien.FillWeight = 85;

            colDaHoanThanh.Name = "colDaHoanThanh";
            colDaHoanThanh.HeaderText = "Hoàn thành";
            colDaHoanThanh.DataPropertyName = "DaHoanThanh";
            colDaHoanThanh.FillWeight = 70;

            lblTrangThai.AutoSize = true;
            lblTrangThai.Location = new Point(20, 684);
            lblTrangThai.Text = "Tìm thấy 0 chuyến đi";

            // Form
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1115, 713);
            MinimumSize = new Size(1131, 752);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kế hoạch dã ngoại";

            Controls.AddRange(new Control[]
            {
                lblTieuDe, lblNgonNgu, cboNgonNgu,
                grpThongTin,
                btnThem, btnSua, btnXoa, btnLamMoi,
                btnXemThoiTiet, prgThoiTiet, pnlThoiTiet,
                lblTimKiem, txtTimKiem, chkChuaHoanThanh,
                lblTuNgay, dtpTuNgay,
                lblDenNgay, dtpDenNgay,
                btnTim, btnBoLoc,
                dgvChuyenDi, lblTrangThai
            });

            grpThongTin.ResumeLayout(false);
            grpThongTin.PerformLayout();
            pnlThoiTiet.ResumeLayout(false);

            ((System.ComponentModel.ISupportInitialize)nudSoNguoi).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudNganSach).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvChuyenDi).EndInit();

            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTieuDe;
        private Label lblNgonNgu;
        private ComboBox cboNgonNgu;

        private GroupBox grpThongTin;
        private Label lblTenChuyen;
        private TextBox txtTenChuyen;
        private Label lblDiaDiem;
        private TextBox txtDiaDiem;
        private Label lblNgayDi;
        private DateTimePicker dtpNgayDi;
        private Label lblSoNguoi;
        private NumericUpDown nudSoNguoi;
        private Label lblNganSach;
        private NumericUpDown nudNganSach;
        private Label lblPhuongTien;
        private ComboBox cboPhuongTien;
        private Label lblGhiChu;
        private TextBox txtGhiChu;
        private CheckBox chkDaHoanThanh;

        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private Button btnXemThoiTiet;
        private ProgressBar prgThoiTiet;

        private Panel pnlThoiTiet;
        private Label lblTieuDeThoiTiet;
        private Label lblBieuTuong;
        private Label lblMoTa;
        private Label lblNgayDuBao;
        private Label lblNhietDo;
        private Label lblXacSuatMua;
        private Label lblLoiKhuyen;

        private Label lblTimKiem;
        private TextBox txtTimKiem;
        private CheckBox chkChuaHoanThanh;
        private Label lblTuNgay;
        private DateTimePicker dtpTuNgay;
        private Label lblDenNgay;
        private DateTimePicker dtpDenNgay;
        private Button btnTim;
        private Button btnBoLoc;

        private DataGridView dgvChuyenDi;
        private DataGridViewTextBoxColumn colMaChuyen;
        private DataGridViewTextBoxColumn colTenChuyen;
        private DataGridViewTextBoxColumn colDiaDiem;
        private DataGridViewTextBoxColumn colNgayDi;
        private DataGridViewTextBoxColumn colSoNguoi;
        private DataGridViewTextBoxColumn colNganSach;
        private DataGridViewTextBoxColumn colPhuongTien;
        private DataGridViewCheckBoxColumn colDaHoanThanh;

        private Label lblTrangThai;
    }
}
