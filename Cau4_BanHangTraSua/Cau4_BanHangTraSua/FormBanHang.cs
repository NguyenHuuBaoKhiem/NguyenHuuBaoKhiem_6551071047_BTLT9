
using System.ComponentModel;
using Cau4_BanHangTraSua.Models;
using Cau4_BanHangTraSua.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Cau4_BanHangTraSua;

public partial class FormBanHang : Form
{
    private readonly ComboBox cboDanhMuc = new();
    private readonly ListBox lstMon = new();
    private readonly NumericUpDown nudSoLy = new();

    private readonly DataGridView dgvGioHang = new();
    private readonly DataGridView dgvHoaDon = new();
    private readonly DataGridView dgvChiTiet = new();

    private readonly DateTimePicker dtpNgayHoaDon = new();

    private readonly Label lblTongTien = new();
    private readonly Label lblTaiKhoan = new();
    private readonly Label lblThanhToan = new();
    private readonly Label lblNoiDung = new();
    private readonly Label lblDoanhThu = new();

    private readonly PictureBox picQR = new();

    private readonly Button btnChuyenKhoan = new();
    private readonly Button btnDaNhanTien = new();
    private readonly Button btnTienMat = new();

    private readonly BindingList<GioHangDong> _gioHang = new();
    private readonly BindingSource _nguonGio = new();

    private bool _dangNapDanhMuc = false;
    private bool _dangNapHoaDon = false;
    private bool _dangLuu = false;
    private bool _dangTaiQr = false;

    private int _phienBanGio = 0;
    private int _yeuCauHoaDon = 0;

    private string? _noiDungChuyenKhoan;
    private decimal _soTienChuyenKhoan;

    // ==========================
    // CLASS HIỂN THỊ
    // ==========================
    private class DanhMucItem
    {
        public int MaDM { get; set; }
        public string TenDM { get; set; } = "";
    }

    private class MonItem
    {
        public int MaMon { get; set; }
        public string TenMon { get; set; } = "";
        public decimal GiaBan { get; set; }

        public string HienThi =>
            $"{TenMon} - {GiaBan:N0} đ";
    }

    private class GioHangDong
    {
        public int MaMon { get; set; }
        public string TenMon { get; set; } = "";
        public int SoLy { get; set; }
        public decimal DonGia { get; set; }

        public decimal ThanhTien =>
            SoLy * DonGia;
    }

    private class HoaDonRow
    {
        public int MaHD { get; set; }
        public DateTime NgayLap { get; set; }
        public string HinhThucTT { get; set; } = "";
        public decimal TongTien { get; set; }
        public bool DaThanhToan { get; set; }
    }

    private class ChiTietRow
    {
        public string TenMon { get; set; } = "";
        public int SoLy { get; set; }
        public decimal DonGia { get; set; }

        public decimal ThanhTien =>
            SoLy * DonGia;
    }

    private decimal TongTien =>
        _gioHang.Sum(x => x.ThanhTien);

    // ==========================
    // KHỞI TẠO FORM
    // ==========================
    public FormBanHang()
    {
        InitializeComponent();

        Text = "Bán hàng quán trà sữa";
        ClientSize = new Size(1195, 855);
        MinimumSize = new Size(1211, 894);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.WhiteSmoke;
        Font = new Font("Segoe UI", 10);

        KhoiTaoGiaoDien();

        _nguonGio.DataSource = _gioHang;
        dgvGioHang.DataSource = _nguonGio;

        Load += async (_, _) =>
        {
            await NapDanhMucAsync();
            await NapMonTheoDanhMucAsync();
            await NapHoaDonAsync();
        };
    }

    // ==========================
    // HÀM TẠO CONTROL
    // ==========================
    private static Label TaoNhan(
        string text,
        int x,
        int y,
        int width = 180)
    {
        return new Label
        {
            Text = text,
            Location = new Point(x, y),
            Size = new Size(width, 26),
            TextAlign = ContentAlignment.MiddleLeft
        };
    }

    private static Button TaoNut(
        string text,
        int x,
        int y,
        int width = 110)
    {
        return new Button
        {
            Text = text,
            Location = new Point(x, y),
            Size = new Size(width, 34)
        };
    }

    private static void ThemCot(
        DataGridView dgv,
        string ten,
        string tieuDe,
        int width,
        string format = "")
    {
        var col = new DataGridViewTextBoxColumn
        {
            Name = ten,
            DataPropertyName = ten,
            HeaderText = tieuDe,
            Width = width
        };

        if (!string.IsNullOrEmpty(format))
            col.DefaultCellStyle.Format = format;

        dgv.Columns.Add(col);
    }

    private static void CauHinhLuoi(DataGridView dgv)
    {
        dgv.BackgroundColor = Color.White;
        dgv.ReadOnly = true;
        dgv.MultiSelect = false;
        dgv.AutoGenerateColumns = false;
        dgv.AllowUserToAddRows = false;
        dgv.AllowUserToDeleteRows = false;
        dgv.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;
    }

    // ==========================
    // GIAO DIỆN
    // ==========================
    private void KhoiTaoGiaoDien()
    {

        var title = new Label
        {
            Text = "BÁN HÀNG QUÁN TRÀ SỮA",
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            Location = new Point(24, 8),
            Size = new Size(700, 45),
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Color.FromArgb(30, 45, 65)
        };

        Controls.Add(title);


        // Quản lý thực đơn
        var btnThucDon = TaoNut(
            "Quản lý thực đơn",
            988, 12, 180);

        btnThucDon.Click += async (_, _) =>
        {
            using var form = new FormThucDon();
            form.ShowDialog(this);

            await NapDanhMucAsync();
            await NapMonTheoDanhMucAsync();
        };

        Controls.Add(btnThucDon);

        // ==========================
        // CHỌN MÓN
        // ==========================
        var grpChonMon = new GroupBox
        {
            Text = "Chọn món",
            Location = new Point(20, 57),
            Size = new Size(255, 445)
        };

        Controls.Add(grpChonMon);

        grpChonMon.Controls.Add(
            TaoNhan("Danh mục", 15, 28));

        cboDanhMuc.Location = new Point(15, 58);
        cboDanhMuc.Size = new Size(225, 25);
        cboDanhMuc.DropDownStyle =
            ComboBoxStyle.DropDownList;

        cboDanhMuc.SelectedIndexChanged += async (_, _) =>
        {
            if (!_dangNapDanhMuc)
                await NapMonTheoDanhMucAsync();
        };

        grpChonMon.Controls.Add(cboDanhMuc);

        lstMon.Location = new Point(15, 103);
        lstMon.Size = new Size(225, 242);
        lstMon.IntegralHeight = false;

        grpChonMon.Controls.Add(lstMon);

        grpChonMon.Controls.Add(
            TaoNhan("Số ly", 15, 358, 70));

        nudSoLy.Location = new Point(90, 357);
        nudSoLy.Size = new Size(150, 25);
        nudSoLy.Minimum = 1;
        nudSoLy.Maximum = 1000;
        nudSoLy.Value = 1;

        grpChonMon.Controls.Add(nudSoLy);

        var btnThemGio = TaoNut(
            "Thêm vào đơn", 15, 395, 225);

        btnThemGio.Click += (_, _) => ThemVaoGio();

        grpChonMon.Controls.Add(btnThemGio);

        // ==========================
        // GIỎ HÀNG
        // ==========================
        var grpGio = new GroupBox
        {
            Text = "Đơn hiện tại",
            Location = new Point(287, 57),
            Size = new Size(490, 445)
        };

        Controls.Add(grpGio);

        dgvGioHang.Location = new Point(15, 33);
        dgvGioHang.Size = new Size(460, 295);

        CauHinhLuoi(dgvGioHang);

        ThemCot(
            dgvGioHang,
            "TenMon", "Tên món", 155);

        ThemCot(
            dgvGioHang,
            "SoLy", "SL", 45);

        ThemCot(
            dgvGioHang,
            "DonGia", "Đơn giá", 95, "N0");

        ThemCot(
            dgvGioHang,
            "ThanhTien", "Thành tiền", 125, "N0");

        grpGio.Controls.Add(dgvGioHang);

        var btnXoaDong = TaoNut(
            "Xóa dòng", 15, 340, 120);

        btnXoaDong.Click += (_, _) => XoaDong();

        grpGio.Controls.Add(btnXoaDong);

        lblTongTien.Text = "Tổng tiền: 0 đ";

        lblTongTien.Font = new Font(
            "Segoe UI", 14, FontStyle.Bold);

        lblTongTien.ForeColor = Color.DarkRed;
        lblTongTien.Location = new Point(15, 393);
        lblTongTien.Size = new Size(445, 32);
        lblTongTien.TextAlign =
            ContentAlignment.MiddleRight;

        grpGio.Controls.Add(lblTongTien);

        // ==========================
        // THANH TOÁN
        // ==========================
        var grpThanhToan = new GroupBox
        {
            Text = "Thanh toán",
            Location = new Point(789, 57),
            Size = new Size(380, 445)
        };

        Controls.Add(grpThanhToan);

        btnChuyenKhoan.Text =
            "Thanh toán chuyển khoản";

        btnChuyenKhoan.Location =
            new Point(12, 28);

        btnChuyenKhoan.Size =
            new Size(210, 33);

        btnChuyenKhoan.Click += async (_, _) =>
            await TaoQRAsync();

        grpThanhToan.Controls.Add(btnChuyenKhoan);

        btnTienMat.Text = "Tiền mặt";
        btnTienMat.Location = new Point(235, 28);
        btnTienMat.Size = new Size(130, 33);

        btnTienMat.Click += async (_, _) =>
            await ThanhToanTienMatAsync();

        grpThanhToan.Controls.Add(btnTienMat);

        // PictureBox QR
        picQR.Location = new Point(90, 68);
        picQR.Size = new Size(200, 215);
        picQR.SizeMode = PictureBoxSizeMode.Zoom;
        picQR.BackColor = Color.White;
        picQR.BorderStyle = BorderStyle.FixedSingle;

        grpThanhToan.Controls.Add(picQR);

        // Thông tin ngân hàng
        lblTaiKhoan.Location = new Point(12, 293);
        lblTaiKhoan.Size = new Size(355, 42);
        lblTaiKhoan.Text = "Chưa chọn thanh toán";

        grpThanhToan.Controls.Add(lblTaiKhoan);

        // Số tiền
        lblThanhToan.Location =
            new Point(12, 339);

        lblThanhToan.Size =
            new Size(355, 26);

        grpThanhToan.Controls.Add(lblThanhToan);

        // Nội dung chuyển khoản
        lblNoiDung.Location =
            new Point(12, 365);

        lblNoiDung.Size =
            new Size(355, 25);

        grpThanhToan.Controls.Add(lblNoiDung);

        // Đã nhận tiền
        btnDaNhanTien.Text = "Đã nhận tiền";
        btnDaNhanTien.BackColor = Color.LightGreen;
        btnDaNhanTien.Location = new Point(95, 397);
        btnDaNhanTien.Size = new Size(185, 35);
        btnDaNhanTien.Enabled = false;

        btnDaNhanTien.Click += async (_, _) =>
            await XacNhanChuyenKhoanAsync();

        grpThanhToan.Controls.Add(btnDaNhanTien);

        // ==========================
        // DANH SÁCH HÓA ĐƠN
        // ==========================
        var lblLichSu = TaoNhan(
            "DANH SÁCH HÓA ĐƠN",
            20, 522, 350);

        lblLichSu.Font = new Font(
            "Segoe UI", 12, FontStyle.Bold);

        Controls.Add(lblLichSu);

        Controls.Add(
            TaoNhan("Ngày", 380, 524, 55));

        dtpNgayHoaDon.Location = new Point(435, 523);
        dtpNgayHoaDon.Size = new Size(133, 25);
        dtpNgayHoaDon.Format =
            DateTimePickerFormat.Custom;

        dtpNgayHoaDon.CustomFormat = "dd/MM/yyyy";

        dtpNgayHoaDon.ValueChanged += async (_, _) =>
            await NapHoaDonAsync();

        Controls.Add(dtpNgayHoaDon);

        var lblChiTiet = TaoNhan(
            "CHI TIẾT HÓA ĐƠN",
            588, 522, 310);

        lblChiTiet.Font = new Font(
            "Segoe UI", 12, FontStyle.Bold);

        Controls.Add(lblChiTiet);

        // Lưới hóa đơn
        dgvHoaDon.Location = new Point(20, 561);
        dgvHoaDon.Size = new Size(550, 226);

        CauHinhLuoi(dgvHoaDon);

        ThemCot(
            dgvHoaDon,
            "MaHD", "Mã HD", 70);

        ThemCot(
            dgvHoaDon,
            "NgayLap", "Giờ lập",
            150, "dd/MM/yyyy HH:mm");

        ThemCot(
            dgvHoaDon,
            "HinhThucTT", "Hình thức", 155);

        ThemCot(
            dgvHoaDon,
            "TongTien", "Tổng tiền",
            130, "N0");

        dgvHoaDon.SelectionChanged += async (_, _) =>
            await ChonHoaDonAsync();

        Controls.Add(dgvHoaDon);

        // Lưới chi tiết hóa đơn
        dgvChiTiet.Location = new Point(588, 561);
        dgvChiTiet.Size = new Size(581, 226);

        CauHinhLuoi(dgvChiTiet);

        ThemCot(
            dgvChiTiet,
            "TenMon", "Tên món", 200);

        ThemCot(
            dgvChiTiet,
            "SoLy", "SL", 65);

        ThemCot(
            dgvChiTiet,
            "DonGia", "Đơn giá", 125, "N0");

        ThemCot(
            dgvChiTiet,
            "ThanhTien", "Thành tiền", 145, "N0");

        Controls.Add(dgvChiTiet);

        // Doanh thu
        lblDoanhThu.Text =
            "Doanh thu ngày: 0 đ";

        lblDoanhThu.Font = new Font(
            "Segoe UI", 12, FontStyle.Bold);

        lblDoanhThu.ForeColor = Color.DarkGreen;
        lblDoanhThu.Location = new Point(20, 803);
        lblDoanhThu.Size = new Size(600, 30);

        Controls.Add(lblDoanhThu);
    }

    // ==========================
    // NẠP DANH MỤC
    // ==========================
    private async Task NapDanhMucAsync()
    {
        try
        {
            _dangNapDanhMuc = true;

            using var db = AppDb.Create();

            var ds = await db.Set<DanhMucMon>()
                .AsNoTracking()
                .OrderBy(x => x.MaDm)
                .Select(x => new DanhMucItem
                {
                    MaDM = x.MaDm,
                    TenDM = x.TenDm
                })
                .ToListAsync();

            cboDanhMuc.DataSource = null;
            cboDanhMuc.DisplayMember = "TenDM";
            cboDanhMuc.ValueMember = "MaDM";
            cboDanhMuc.DataSource = ds;
        }
        catch (Exception ex)
        {
            BaoLoi(
                ex, "Không tải được danh mục.");
        }
        finally
        {
            _dangNapDanhMuc = false;
        }
    }

    // ==========================
    // NẠP MÓN ĐANG BÁN
    // ==========================
    private async Task NapMonTheoDanhMucAsync()
    {
        if (cboDanhMuc.SelectedItem
            is not DanhMucItem dm)
            return;

        try
        {
            using var db = AppDb.Create();

            var ds = await db.Set<MonUong>()
                .AsNoTracking()
                .Where(x =>
                    x.MaDm == dm.MaDM &&
                    x.DangBan)
                .OrderBy(x => x.TenMon)
                .Select(x => new MonItem
                {
                    MaMon = x.MaMon,
                    TenMon = x.TenMon,
                    GiaBan = x.GiaBan
                })
                .ToListAsync();

            lstMon.DataSource = null;
            lstMon.DisplayMember = "HienThi";
            lstMon.ValueMember = "MaMon";
            lstMon.DataSource = ds;
        }
        catch (Exception ex)
        {
            BaoLoi(
                ex, "Không tải được danh sách món.");
        }
    }

    // ==========================
    // THÊM MÓN VÀO GIỎ
    // ==========================
    private void ThemVaoGio()
    {
        if (_dangLuu)
            return;

        if (lstMon.SelectedItem is not MonItem mon)
        {
            MessageBox.Show(
                "Vui lòng chọn món.");
            return;
        }

        int soLy = (int)nudSoLy.Value;

        var dong = _gioHang.FirstOrDefault(
            x => x.MaMon == mon.MaMon);

        if (dong != null)
        {
            dong.SoLy += soLy;
        }
        else
        {
            _gioHang.Add(new GioHangDong
            {
                MaMon = mon.MaMon,
                TenMon = mon.TenMon,
                DonGia = mon.GiaBan,
                SoLy = soLy
            });
        }

        _nguonGio.ResetBindings(false);
        CapNhatTongTien();
    }

    // ==========================
    // XÓA MÓN KHỎI GIỎ
    // ==========================
    private void XoaDong()
    {
        if (_dangLuu)
            return;

        if (dgvGioHang.CurrentRow?.DataBoundItem
            is not GioHangDong dong)
        {
            MessageBox.Show(
                "Vui lòng chọn món cần xóa.");
            return;
        }

        _gioHang.Remove(dong);

        _nguonGio.ResetBindings(false);

        CapNhatTongTien();
    }

    // ==========================
    // TÍNH TỔNG TIỀN
    // ==========================
    private void CapNhatTongTien()
    {
        _phienBanGio++;

        lblTongTien.Text =
            $"Tổng tiền: {TongTien:N0} đ";

        XoaThongTinQR();
    }

    // ==========================
    // XÓA QR CŨ
    // ==========================
    private void XoaThongTinQR()
    {
        var anhCu = picQR.Image;

        picQR.Image = null;
        anhCu?.Dispose();

        _noiDungChuyenKhoan = null;
        _soTienChuyenKhoan = 0;

        lblTaiKhoan.Text =
            "Chưa chọn thanh toán";

        lblThanhToan.Text = "";
        lblNoiDung.Text = "";

        btnDaNhanTien.Enabled = false;
    }

    // ==========================
    // TẠO MÃ VIETQR
    // ==========================
    private async Task TaoQRAsync()
    {
        if (_dangLuu || _dangTaiQr)
            return;

        if (_gioHang.Count == 0)
        {
            MessageBox.Show(
                "Giỏ hàng đang trống.");
            return;
        }

        try
        {
            // Đọc lại JSON để nhận cấu hình mới
            AppConfig.Load();
        }
        catch (Exception ex)
        {
            BaoLoi(
                ex, "Không đọc được cấu hình thanh toán.");
            return;
        }

        var bank = AppConfig.Settings.Payment;

        if (string.IsNullOrWhiteSpace(bank.BankId) ||
            string.IsNullOrWhiteSpace(bank.AccountNo) ||
            string.IsNullOrWhiteSpace(bank.AccountName) ||
            bank.AccountNo.All(x => x == '0') ||
            bank.AccountNo == "YOUR_ACCOUNT_NO")
        {
            MessageBox.Show(
                "Bạn đang sử dụng tài khoản demo.\n" +
                "Vui lòng cấu hình tài khoản nhận tiền " +
                "trong appsettings.json.");
            return;
        }

        _noiDungChuyenKhoan ??=
            VietQrService.TaoNoiDungChuyenKhoan();

        _soTienChuyenKhoan = TongTien;

        int phienBan = _phienBanGio;
        string noiDung = _noiDungChuyenKhoan;

        // Thông tin chuyển khoản dạng chữ
        lblTaiKhoan.Text =
            $"{bank.ShopName}\n" +
            $"{bank.BankId} | {bank.AccountNo} | {bank.AccountName}";

        lblThanhToan.Text =
            $"Số tiền: {TongTien:N0} đ";

        lblNoiDung.Text =
            $"Nội dung: {noiDung}";

        btnDaNhanTien.Enabled = true;
        btnChuyenKhoan.Enabled = false;
        btnChuyenKhoan.Text = "Đang tải QR...";

        _dangTaiQr = true;

        try
        {
            string url = VietQrService.TaoDuongDan(
                bank,
                TongTien,
                noiDung);

            byte[] bytes =
                await VietQrService.TaiAnhAsync(url);

            if (phienBan != _phienBanGio)
                return;

            using var stream = new MemoryStream(bytes);
            using var image = Image.FromStream(stream);

            Image anhMoi = new Bitmap(image);

            var anhCu = picQR.Image;
            picQR.Image = anhMoi;

            anhCu?.Dispose();

            Log.Information(
                "Tạo QR: MaGiaoDich={MaGiaoDich}, SoTien={SoTien}",
                noiDung,
                _soTienChuyenKhoan);
        }
        catch (Exception ex)
        {
            if (phienBan != _phienBanGio)
                return;

            Log.Error(
                ex,
                "Không tải được QR, MaGiaoDich={MaGiaoDich}",
                noiDung);

            var anhCu = picQR.Image;
            picQR.Image = null;
            anhCu?.Dispose();

            MessageBox.Show(
                "Không tải được ảnh QR.\n" +
                "Bạn vẫn có thể chuyển khoản thủ công " +
                "theo thông tin đang hiển thị " +
                "hoặc chuyển sang thanh toán tiền mặt.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        finally
        {
            _dangTaiQr = false;

            btnChuyenKhoan.Enabled = !_dangLuu;
            btnChuyenKhoan.Text =
                "Thanh toán chuyển khoản";
        }
    }

    // ==========================
    // XÁC NHẬN ĐÃ NHẬN TIỀN
    // ==========================
    private async Task XacNhanChuyenKhoanAsync()
    {
        if (_dangLuu)
            return;

        if (string.IsNullOrWhiteSpace(
            _noiDungChuyenKhoan))
        {
            MessageBox.Show(
                "Hãy tạo thông tin chuyển khoản trước.");
            return;
        }

        if (_soTienChuyenKhoan != TongTien)
        {
            MessageBox.Show(
                "Giỏ hàng đã thay đổi.\n" +
                "Vui lòng tạo lại mã QR.");
            return;
        }

        DialogResult result = MessageBox.Show(
            "Bạn đã kiểm tra tài khoản ngân hàng " +
            "và chắc chắn ĐÃ NHẬN TIỀN?",
            "Xác nhận thanh toán",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result != DialogResult.Yes)
            return;

        await LuuHoaDonAsync("Chuyển khoản");
    }

    // ==========================
    // THANH TOÁN TIỀN MẶT
    // ==========================
    private async Task ThanhToanTienMatAsync()
    {
        if (_dangLuu)
            return;

        if (_gioHang.Count == 0)
        {
            MessageBox.Show(
                "Giỏ hàng đang trống.");
            return;
        }

        DialogResult result = MessageBox.Show(
            $"Đã nhận {TongTien:N0} đ tiền mặt?",
            "Xác nhận thanh toán",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result != DialogResult.Yes)
            return;

        await LuuHoaDonAsync("Tiền mặt");
    }

    // ==========================
    // LƯU HÓA ĐƠN + CHI TIẾT
    // ==========================
    private async Task LuuHoaDonAsync(string hinhThuc)
    {
        if (_dangLuu || _gioHang.Count == 0)
            return;

        _dangLuu = true;

        btnTienMat.Enabled = false;
        btnDaNhanTien.Enabled = false;
        btnChuyenKhoan.Enabled = false;

        try
        {
            using var db = AppDb.Create();

            var maMon = _gioHang
                .Select(x => x.MaMon)
                .ToList();

            int soMonHopLe = await db.Set<MonUong>()
                .AsNoTracking()
                .CountAsync(x =>
                    maMon.Contains(x.MaMon) &&
                    x.DangBan);

            if (soMonHopLe != _gioHang.Count)
            {
                MessageBox.Show(
                    "Có món đã ngừng bán hoặc bị xóa.\n" +
                    "Vui lòng kiểm tra lại giỏ hàng.");
                return;
            }

            // Tạo hóa đơn
            var hoaDon = new HoaDon
            {
                NgayLap = DateTime.Now,
                TongTien = TongTien,
                HinhThucTt = hinhThuc,
                DaThanhToan = true
            };

            // Thêm tất cả chi tiết hóa đơn
            foreach (var dong in _gioHang)
            {
                hoaDon.ChiTietHoaDons.Add(
                    new ChiTietHoaDon
                    {
                        MaMon = dong.MaMon,
                        SoLy = dong.SoLy,
                        DonGia = dong.DonGia
                    });
            }

            db.Set<HoaDon>().Add(hoaDon);

            // Chỉ gọi một lần SaveChanges
            await db.SaveChangesAsync();

            Log.Information(
                "Thanh toán MaHD={MaHD}, HinhThuc={HinhThuc}, SoTien={SoTien}",
                hoaDon.MaHd,
                hinhThuc,
                hoaDon.TongTien);

            MessageBox.Show(
                "Thanh toán thành công!\n" +
                $"Mã hóa đơn: {hoaDon.MaHd}\n" +
                $"Tổng tiền: {hoaDon.TongTien:N0} đ",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // Xóa giỏ sau khi lưu thành công
            _gioHang.Clear();
            _nguonGio.ResetBindings(false);

            CapNhatTongTien();

            dtpNgayHoaDon.Value = DateTime.Today;
            await NapHoaDonAsync();
        }
        catch (Exception ex)
        {
            Log.Error(
                ex, "Không thể lưu hóa đơn");

            BaoLoi(
                ex, "Lưu hóa đơn thất bại.");
        }
        finally
        {
            _dangLuu = false;

            btnTienMat.Enabled = true;
            btnChuyenKhoan.Enabled = true;

            btnDaNhanTien.Enabled =
                _gioHang.Count > 0 &&
                !string.IsNullOrWhiteSpace(
                    _noiDungChuyenKhoan);
        }
    }

    // ==========================
    // NẠP HÓA ĐƠN THEO NGÀY
    // ==========================
    private async Task NapHoaDonAsync()
    {
        int yeuCau = ++_yeuCauHoaDon;

        try
        {
            using var db = AppDb.Create();

            DateTime tuNgay =
                dtpNgayHoaDon.Value.Date;

            DateTime denNgay =
                tuNgay.AddDays(1);

            var ds = await db.Set<HoaDon>()
                .AsNoTracking()
                .Where(x =>
                    x.NgayLap >= tuNgay &&
                    x.NgayLap < denNgay)
                .OrderByDescending(x => x.MaHd)
                .Select(x => new HoaDonRow
                {
                    MaHD = x.MaHd,
                    NgayLap = x.NgayLap,
                    HinhThucTT = x.HinhThucTt,
                    TongTien = x.TongTien,
                    DaThanhToan = x.DaThanhToan
                })
                .ToListAsync();

            if (yeuCau != _yeuCauHoaDon)
                return;

            _dangNapHoaDon = true;

            try
            {
                dgvHoaDon.DataSource = ds;
                dgvHoaDon.ClearSelection();
                dgvChiTiet.DataSource = null;
            }
            finally
            {
                _dangNapHoaDon = false;
            }

            decimal doanhThu = ds
                .Where(x => x.DaThanhToan)
                .Sum(x => x.TongTien);

            lblDoanhThu.Text =
                $"Doanh thu ngày: {doanhThu:N0} đ";
        }
        catch (Exception ex)
        {
            if (yeuCau == _yeuCauHoaDon)
            {
                BaoLoi(
                    ex,
                    "Không tải được danh sách hóa đơn.");
            }
        }
    }

    // ==========================
    // XEM CHI TIẾT HÓA ĐƠN
    // ==========================
    private async Task ChonHoaDonAsync()
    {
        if (_dangNapHoaDon ||
            dgvHoaDon.SelectedRows.Count == 0)
            return;

        if (dgvHoaDon.SelectedRows[0].DataBoundItem
            is not HoaDonRow hoaDon)
            return;

        try
        {
            using var db = AppDb.Create();

            var query =
                from ct in db.Set<ChiTietHoaDon>()
                    .AsNoTracking()
                join mon in db.Set<MonUong>()
                    .AsNoTracking()
                    on ct.MaMon equals mon.MaMon
                where ct.MaHd == hoaDon.MaHD
                select new ChiTietRow
                {
                    TenMon = mon.TenMon,
                    SoLy = ct.SoLy,
                    DonGia = ct.DonGia
                };

            var ds = await query.ToListAsync();

            if (dgvHoaDon.SelectedRows.Count == 0)
                return;

            if (dgvHoaDon.SelectedRows[0].DataBoundItem
                is HoaDonRow dangChon &&
                dangChon.MaHD == hoaDon.MaHD)
            {
                dgvChiTiet.DataSource = ds;
            }
        }
        catch (Exception ex)
        {
            BaoLoi(
                ex,
                "Không tải được chi tiết hóa đơn.");
        }
    }

    // ==========================
    // THÔNG BÁO LỖI
    // ==========================
    private static void BaoLoi(
        Exception ex,
        string noiDung)
    {
        MessageBox.Show(
            noiDung + "\n" + ex.Message,
            "Lỗi",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    // ==========================
    // GIẢI PHÓNG ẢNH QR
    // ==========================
    protected override void OnFormClosed(
        FormClosedEventArgs e)
    {
        var anhCu = picQR.Image;
        picQR.Image = null;
        anhCu?.Dispose();

        _nguonGio.Dispose();

        base.OnFormClosed(e);
    }
}
