namespace Cau1_SoChiTieuCaNhan
{
    partial class Form1
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
            grpThongTinKhoanChi = new GroupBox();
            lblNgayChi = new Label();
            dtpNgayChi = new DateTimePicker();
            lblNoiDung = new Label();
            txtNoiDung = new TextBox();
            lblLoaiChi = new Label();
            cboLoaiChi = new ComboBox();
            lblSoTien = new Label();
            nudSoTien = new NumericUpDown();
            lblGhiChu = new Label();
            txtGhiChu = new TextBox();

            grpTimKiem = new GroupBox();
            lblTimNoiDung = new Label();
            txtTimNoiDung = new TextBox();
            lblLocLoaiChi = new Label();
            cboLocLoaiChi = new ComboBox();
            lblTuNgay = new Label();
            dtpTuNgay = new DateTimePicker();
            lblDenNgay = new Label();
            dtpDenNgay = new DateTimePicker();
            btnTim = new Button();

            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();

            dgvChiTieu = new DataGridView();

            colMaCT = new DataGridViewTextBoxColumn();
            colNgayChi = new DataGridViewTextBoxColumn();
            colNoiDung = new DataGridViewTextBoxColumn();
            colLoaiChi = new DataGridViewTextBoxColumn();
            colSoTien = new DataGridViewTextBoxColumn();
            colGhiChu = new DataGridViewTextBoxColumn();

            lblTongChi = new Label();

            grpThongTinKhoanChi.SuspendLayout();
            grpTimKiem.SuspendLayout();

            ((System.ComponentModel.ISupportInitialize)nudSoTien)
                .BeginInit();

            ((System.ComponentModel.ISupportInitialize)dgvChiTieu)
                .BeginInit();

            SuspendLayout();

            // =========================
            // GROUP THÔNG TIN
            // =========================
            grpThongTinKhoanChi.Controls.Add(lblNgayChi);
            grpThongTinKhoanChi.Controls.Add(dtpNgayChi);
            grpThongTinKhoanChi.Controls.Add(lblNoiDung);
            grpThongTinKhoanChi.Controls.Add(txtNoiDung);
            grpThongTinKhoanChi.Controls.Add(lblLoaiChi);
            grpThongTinKhoanChi.Controls.Add(cboLoaiChi);
            grpThongTinKhoanChi.Controls.Add(lblSoTien);
            grpThongTinKhoanChi.Controls.Add(nudSoTien);
            grpThongTinKhoanChi.Controls.Add(lblGhiChu);
            grpThongTinKhoanChi.Controls.Add(txtGhiChu);

            grpThongTinKhoanChi.Location =
                new Point(18, 15);

            grpThongTinKhoanChi.Size =
                new Size(570, 190);

            grpThongTinKhoanChi.Text =
                "Thông tin khoản chi";

            // Ngày chi
            lblNgayChi.AutoSize = true;
            lblNgayChi.Location =
                new Point(20, 32);

            lblNgayChi.Text =
                "Ngày chi";

            dtpNgayChi.CustomFormat =
                "dd/MM/yyyy";

            dtpNgayChi.Format =
                DateTimePickerFormat.Custom;

            dtpNgayChi.Location =
                new Point(105, 28);

            dtpNgayChi.Size =
                new Size(150, 23);

            // Nội dung
            lblNoiDung.AutoSize = true;
            lblNoiDung.Location =
                new Point(20, 68);

            lblNoiDung.Text =
                "Nội dung";

            txtNoiDung.Location =
                new Point(105, 64);

            txtNoiDung.Size =
                new Size(215, 23);

            // Loại chi
            lblLoaiChi.AutoSize = true;
            lblLoaiChi.Location =
                new Point(20, 104);

            lblLoaiChi.Text =
                "Loại chi";

            cboLoaiChi.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cboLoaiChi.Location =
                new Point(105, 100);

            cboLoaiChi.Size =
                new Size(150, 23);

            // Số tiền
            lblSoTien.AutoSize = true;
            lblSoTien.Location =
                new Point(345, 104);

            lblSoTien.Text =
                "Số tiền";

            nudSoTien.Location =
                new Point(405, 100);

            nudSoTien.Maximum =
                1000000000;

            nudSoTien.Increment =
                1000;

            nudSoTien.ThousandsSeparator =
                true;

            nudSoTien.Size =
                new Size(145, 23);

            // Ghi chú
            lblGhiChu.AutoSize = true;
            lblGhiChu.Location =
                new Point(20, 140);

            lblGhiChu.Text =
                "Ghi chú";

            txtGhiChu.Location =
                new Point(105, 136);

            txtGhiChu.Multiline = true;
            txtGhiChu.ScrollBars =
                ScrollBars.Vertical;

            txtGhiChu.Size =
                new Size(445, 40);

            // =========================
            // GROUP TÌM KIẾM
            // =========================
            grpTimKiem.Controls.Add(lblTimNoiDung);
            grpTimKiem.Controls.Add(txtTimNoiDung);
            grpTimKiem.Controls.Add(lblLocLoaiChi);
            grpTimKiem.Controls.Add(cboLocLoaiChi);
            grpTimKiem.Controls.Add(lblTuNgay);
            grpTimKiem.Controls.Add(dtpTuNgay);
            grpTimKiem.Controls.Add(lblDenNgay);
            grpTimKiem.Controls.Add(dtpDenNgay);
            grpTimKiem.Controls.Add(btnTim);

            grpTimKiem.Location =
                new Point(605, 15);

            grpTimKiem.Size =
                new Size(380, 190);

            grpTimKiem.Text =
                "Tìm kiếm";

            // Tìm nội dung
            lblTimNoiDung.AutoSize = true;
            lblTimNoiDung.Location =
                new Point(18, 31);

            lblTimNoiDung.Text =
                "Textbox";

            txtTimNoiDung.Location =
                new Point(85, 27);

            txtTimNoiDung.PlaceholderText =
                "Nhập nội dung...";

            txtTimNoiDung.Size =
                new Size(275, 23);

            // Lọc loại
            lblLocLoaiChi.AutoSize = true;
            lblLocLoaiChi.Location =
                new Point(18, 67);

            lblLocLoaiChi.Text =
                "Loại chi";

            cboLocLoaiChi.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cboLocLoaiChi.Location =
                new Point(85, 63);

            cboLocLoaiChi.Size =
                new Size(275, 23);

            // Từ ngày
            lblTuNgay.AutoSize = true;
            lblTuNgay.Location =
                new Point(18, 103);

            lblTuNgay.Text =
                "Từ ngày";

            dtpTuNgay.CustomFormat =
                "dd/MM/yyyy";

            dtpTuNgay.Format =
                DateTimePickerFormat.Custom;

            dtpTuNgay.Location =
                new Point(85, 99);

            dtpTuNgay.Size =
                new Size(150, 23);

            // Đến ngày
            lblDenNgay.AutoSize = true;
            lblDenNgay.Location =
                new Point(18, 139);

            lblDenNgay.Text =
                "Đến ngày";

            dtpDenNgay.CustomFormat =
                "dd/MM/yyyy";

            dtpDenNgay.Format =
                DateTimePickerFormat.Custom;

            dtpDenNgay.Location =
                new Point(85, 135);

            dtpDenNgay.Size =
                new Size(150, 23);

            // Tìm
            btnTim.Location =
                new Point(260, 132);

            btnTim.Size =
                new Size(100, 30);

            btnTim.Text =
                "Tìm";

            btnTim.Click +=
                btnTim_Click;

            // =========================
            // CRUD BUTTON
            // =========================
            btnThem.Location =
                new Point(60, 220);

            btnThem.Size =
                new Size(90, 32);

            btnThem.Text =
                "Thêm";

            btnThem.Click +=
                btnThem_Click;

            btnSua.Location =
                new Point(165, 220);

            btnSua.Size =
                new Size(90, 32);

            btnSua.Text =
                "Sửa";

            btnSua.Click +=
                btnSua_Click;

            btnXoa.Location =
                new Point(270, 220);

            btnXoa.Size =
                new Size(90, 32);

            btnXoa.Text =
                "Xóa";

            btnXoa.Click +=
                btnXoa_Click;

            btnLamMoi.Location =
                new Point(375, 220);

            btnLamMoi.Size =
                new Size(90, 32);

            btnLamMoi.Text =
                "Làm mới";

            btnLamMoi.Click +=
                btnLamMoi_Click;

            // =========================
            // DATAGRIDVIEW
            // =========================
            dgvChiTieu.AllowUserToAddRows =
                false;

            dgvChiTieu.AllowUserToDeleteRows =
                false;

            dgvChiTieu.AutoGenerateColumns =
                false;

            dgvChiTieu.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvChiTieu.BackgroundColor =
                Color.White;

            dgvChiTieu.Columns.AddRange(
                colMaCT,
                colNgayChi,
                colNoiDung,
                colLoaiChi,
                colSoTien,
                colGhiChu
            );

            dgvChiTieu.Location =
                new Point(18, 270);

            dgvChiTieu.MultiSelect =
                false;

            dgvChiTieu.ReadOnly =
                true;

            dgvChiTieu.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvChiTieu.Size =
                new Size(967, 330);

            dgvChiTieu.SelectionChanged +=
                dgvChiTieu_SelectionChanged;

            // Mã
            colMaCT.DataPropertyName =
                "MaCt";

            colMaCT.HeaderText =
                "Mã";

            colMaCT.Name =
                "colMaCT";

            colMaCT.FillWeight =
                55;

            // Ngày chi
            colNgayChi.DataPropertyName =
                "NgayChi";

            colNgayChi.HeaderText =
                "Ngày chi";

            colNgayChi.Name =
                "colNgayChi";

            colNgayChi.FillWeight =
                85;

            // Nội dung
            colNoiDung.DataPropertyName =
                "NoiDung";

            colNoiDung.HeaderText =
                "Nội dung";

            colNoiDung.Name =
                "colNoiDung";

            colNoiDung.FillWeight =
                140;

            // Loại chi
            colLoaiChi.DataPropertyName =
                "LoaiChi";

            colLoaiChi.HeaderText =
                "Loại chi";

            colLoaiChi.Name =
                "colLoaiChi";

            colLoaiChi.FillWeight =
                90;

            // Số tiền
            colSoTien.DataPropertyName =
                "SoTien";

            colSoTien.HeaderText =
                "Số tiền";

            colSoTien.Name =
                "colSoTien";

            colSoTien.DefaultCellStyle.Format =
                "N0";

            colSoTien.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight;

            // Ghi chú
            colGhiChu.DataPropertyName =
                "GhiChu";

            colGhiChu.HeaderText =
                "Ghi chú";

            colGhiChu.Name =
                "colGhiChu";

            colGhiChu.FillWeight =
                130;

            // =========================
            // TỔNG CHI
            // =========================
            lblTongChi.AutoSize = true;

            lblTongChi.Font =
                new Font(
                    "Segoe UI",
                    11F,
                    FontStyle.Bold);

            lblTongChi.ForeColor =
                Color.Firebrick;

            lblTongChi.Location =
                new Point(760, 615);

            lblTongChi.Text =
                "Tổng chi: 0 đ";

            // =========================
            // FORM
            // =========================
            AutoScaleDimensions =
                new SizeF(7F, 15F);

            AutoScaleMode =
                AutoScaleMode.Font;

            ClientSize =
                new Size(1005, 655);

            Controls.Add(
                grpThongTinKhoanChi);

            Controls.Add(
                grpTimKiem);

            Controls.Add(
                btnThem);

            Controls.Add(
                btnSua);

            Controls.Add(
                btnXoa);

            Controls.Add(
                btnLamMoi);

            Controls.Add(
                dgvChiTieu);

            Controls.Add(
                lblTongChi);

            StartPosition =
                FormStartPosition.CenterScreen;

            Text =
                "Sổ chi tiêu cá nhân";

            grpThongTinKhoanChi.ResumeLayout(false);
            grpThongTinKhoanChi.PerformLayout();

            grpTimKiem.ResumeLayout(false);
            grpTimKiem.PerformLayout();

            ((System.ComponentModel.ISupportInitialize)nudSoTien)
                .EndInit();

            ((System.ComponentModel.ISupportInitialize)dgvChiTieu)
                .EndInit();

            ResumeLayout(false);
            PerformLayout();
        }

        private GroupBox grpThongTinKhoanChi;

        private Label lblNgayChi;
        private DateTimePicker dtpNgayChi;

        private Label lblNoiDung;
        private TextBox txtNoiDung;

        private Label lblLoaiChi;
        private ComboBox cboLoaiChi;

        private Label lblSoTien;
        private NumericUpDown nudSoTien;

        private Label lblGhiChu;
        private TextBox txtGhiChu;

        private GroupBox grpTimKiem;

        private Label lblTimNoiDung;
        private TextBox txtTimNoiDung;

        private Label lblLocLoaiChi;
        private ComboBox cboLocLoaiChi;

        private Label lblTuNgay;
        private DateTimePicker dtpTuNgay;

        private Label lblDenNgay;
        private DateTimePicker dtpDenNgay;

        private Button btnTim;

        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;

        private DataGridView dgvChiTieu;

        private DataGridViewTextBoxColumn colMaCT;
        private DataGridViewTextBoxColumn colNgayChi;
        private DataGridViewTextBoxColumn colNoiDung;
        private DataGridViewTextBoxColumn colLoaiChi;
        private DataGridViewTextBoxColumn colSoTien;
        private DataGridViewTextBoxColumn colGhiChu;

        private Label lblTongChi;
    }
}