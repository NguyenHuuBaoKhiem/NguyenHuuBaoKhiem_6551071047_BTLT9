
using Cau2_QuanLyNhaTro.Models;
using Cau2_QuanLyNhaTro.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data;

namespace Cau2_QuanLyNhaTro
{
    public partial class FormKhachThue : Form
    {
        private int _maKhachDangChon = 0;
        private int _lanChonDong = 0;
        private int _lanTaiPhuong = 0;

        private bool _dangNapLuoi = false;
        private bool _dangCapNhatDiaChi = false;

        private CancellationTokenSource? _ctsPhuong;

        public FormKhachThue()
        {
            InitializeComponent();
            Load += FormKhachThue_Load;
        }

        // =========================
        // DỮ LIỆU HIỂN THỊ
        // =========================
        private class PhongItem
        {
            public int? MaPhong { get; set; }
            public string HienThi { get; set; } = "";
        }

        private class KhachRow
        {
            public int MaKhach { get; set; }
            public string HoTen { get; set; } = "";
            public string GioiTinh { get; set; } = "";
            public DateOnly NgaySinh { get; set; }
            public string SoDienThoai { get; set; } = "";

            public int? MaTinh { get; set; }
            public string TenTinh { get; set; } = "";

            public int? MaPhuongXa { get; set; }
            public string TenPhuongXa { get; set; } = "";

            public bool DaDangKyTamTru { get; set; }

            public int MaPhong { get; set; }
            public string TenPhong { get; set; } = "";
            public decimal GiaThue { get; set; }

            public string QueQuan =>
                $"{TenPhuongXa} - {TenTinh}";
        }

        // =========================
        // LOAD FORM
        // =========================
        private async void FormKhachThue_Load(
            object? sender, EventArgs e)
        {
            dtpNgaySinh.MaxDate = DateTime.Today;
            dtpNgaySinh.Value = DateTime.Today.AddYears(-20);

            cboTinh.Enabled = false;
            cboPhuong.Enabled = false;

            try
            {
                await NapBoLocAsync();
                await NapPhongNhapAsync();
                await NapDanhSachAsync();

                LamMoiNhapLieu();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khởi tạo dữ liệu nhà trọ");
                ThongBaoLoi("Không thể tải dữ liệu nhà trọ.");
            }

            await NapTinhThanhAsync();
        }

        // =========================
        // NẠP BỘ LỌC TỪ DATABASE
        // =========================
        private async Task NapBoLocAsync()
        {
            using var db = AppDb.Create();

            int? maPhongCu =
                (cboLocPhong.SelectedItem as PhongItem)?.MaPhong;

            string tinhCu =
                cboLocTinh.SelectedItem?.ToString() ?? "Tất cả";

            var phong = await db.Set<PhongTro>()
                .AsNoTracking()
                .OrderBy(x => x.TenPhong)
                .ToListAsync();

            var dsPhong = new List<PhongItem>
            {
                new PhongItem
                {
                    MaPhong = null,
                    HienThi = "Tất cả"
                }
            };

            dsPhong.AddRange(phong.Select(x => new PhongItem
            {
                MaPhong = x.MaPhong,
                HienThi = x.TenPhong
            }));

            cboLocPhong.DataSource = null;
            cboLocPhong.DisplayMember = "HienThi";
            cboLocPhong.ValueMember = "MaPhong";
            cboLocPhong.DataSource = dsPhong;

            cboLocPhong.SelectedItem =
                dsPhong.FirstOrDefault(x => x.MaPhong == maPhongCu)
                ?? dsPhong[0];

            var dsTinh = await db.Set<KhachThue>()
                .AsNoTracking()
                .Select(x => x.TenTinh)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();

            cboLocTinh.Items.Clear();
            cboLocTinh.Items.Add("Tất cả");

            foreach (string tinh in dsTinh)
                cboLocTinh.Items.Add(tinh);

            cboLocTinh.SelectedItem =
                cboLocTinh.Items.Contains(tinhCu)
                    ? tinhCu
                    : "Tất cả";
        }

        // =========================
        // NẠP PHÒNG HỢP LỆ
        // =========================
        private async Task NapPhongNhapAsync(
            int maPhongHienTai = 0,
            int lanChon = -1)
        {
            using var db = AppDb.Create();

            var ds = await db.Set<PhongTro>()
                .AsNoTracking()
                .Where(x =>
                    x.TinhTrang == "Trống" ||
                    x.MaPhong == maPhongHienTai)
                .OrderBy(x => x.TenPhong)
                .ToListAsync();

            if (lanChon >= 0 && lanChon != _lanChonDong)
                return;

            var phongItems = ds.Select(x => new PhongItem
            {
                MaPhong = x.MaPhong,
                HienThi = $"{x.TenPhong} - {x.GiaThue:N0} đ"
            }).ToList();

            cboPhong.DataSource = null;
            cboPhong.DisplayMember = "HienThi";
            cboPhong.ValueMember = "MaPhong";
            cboPhong.DataSource = phongItems;

            if (maPhongHienTai > 0)
            {
                cboPhong.SelectedItem = phongItems
                    .FirstOrDefault(x =>
                        x.MaPhong == maPhongHienTai);
            }
            else
            {
                cboPhong.SelectedIndex = -1;
            }
        }

        // =========================
        // NẠP DANH SÁCH KHÁCH
        // =========================
        private async Task NapDanhSachAsync()
        {
            try
            {
                using var db = AppDb.Create();

                string tuKhoa = txtTimKiem.Text.Trim();

                int? maPhong =
                    (cboLocPhong.SelectedItem as PhongItem)?.MaPhong;

                string locTinh =
                    cboLocTinh.SelectedItem?.ToString() ?? "Tất cả";

                IQueryable<KhachThue> khach =
                    db.Set<KhachThue>().AsNoTracking();

                if (!string.IsNullOrWhiteSpace(tuKhoa))
                {
                    khach = khach.Where(x =>
                        x.HoTen.Contains(tuKhoa) ||
                        x.SoDienThoai.Contains(tuKhoa));
                }

                if (maPhong.HasValue)
                {
                    int ma = maPhong.Value;
                    khach = khach.Where(x => x.MaPhong == ma);
                }

                if (locTinh != "Tất cả")
                {
                    khach = khach.Where(x =>
                        x.TenTinh == locTinh);
                }

                var query =
                    from k in khach
                    join p in db.Set<PhongTro>().AsNoTracking()
                        on k.MaPhong equals p.MaPhong
                    orderby k.MaKhach
                    select new
                    {
                        k.MaKhach,
                        k.HoTen,
                        k.GioiTinh,
                        k.NgaySinh,
                        k.SoDienThoai,
                        k.MaTinh,
                        k.TenTinh,
                        k.MaPhuongXa,
                        k.TenPhuongXa,
                        k.DaDangKyTamTru,
                        k.MaPhong,
                        p.TenPhong,
                        p.GiaThue
                    };

                var duLieu = await query.ToListAsync();

                var ketQua = duLieu.Select(x => new KhachRow
                {
                    MaKhach = x.MaKhach,
                    HoTen = x.HoTen,
                    GioiTinh = x.GioiTinh,
                    NgaySinh = x.NgaySinh,
                    SoDienThoai = x.SoDienThoai,
                    MaTinh = x.MaTinh,
                    TenTinh = x.TenTinh,
                    MaPhuongXa = x.MaPhuongXa,
                    TenPhuongXa = x.TenPhuongXa,
                    DaDangKyTamTru = x.DaDangKyTamTru,
                    MaPhong = x.MaPhong,
                    TenPhong = x.TenPhong,
                    GiaThue = x.GiaThue
                }).ToList();

                _dangNapLuoi = true;
                try
                {
                    dgvKhachThue.DataSource = ketQua;
                    dgvKhachThue.ClearSelection();
                }
                finally
                {
                    _dangNapLuoi = false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi truy vấn khách thuê");
                ThongBaoLoi(
                    "Không thể tải danh sách khách thuê.");
            }
        }

        // =========================
        // LẤY TỈNH/THÀNH TỪ API
        // =========================
        private async Task NapTinhThanhAsync()
        {
            _dangCapNhatDiaChi = true;
            cboTinh.Enabled = false;
            cboTinh.DropDownStyle = ComboBoxStyle.DropDown;
            cboTinh.Text = "Đang tải...";
            lblTrangThai.Text = "Đang tải tỉnh/thành...";

            try
            {
                var ds = await HanhChinhApi.LayTinhThanhAsync();

                if (ds.Count == 0)
                    throw new Exception("API trả về danh sách trống.");

                cboTinh.DataSource = null;
                cboTinh.DisplayMember = "Name";
                cboTinh.ValueMember = "Code";
                cboTinh.DataSource = ds.OrderBy(x => x.Name).ToList();
                cboTinh.DropDownStyle = ComboBoxStyle.DropDownList;
                cboTinh.SelectedIndex = -1;

                lblTrangThai.Text =
                    $"Đã tải {ds.Count} tỉnh/thành từ provinces.open-api.vn";
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không tải được danh sách tỉnh/thành");

                cboTinh.DataSource = null;
                cboTinh.Items.Clear();
                cboTinh.DropDownStyle = ComboBoxStyle.DropDown;
                cboTinh.Text = "";

                lblTrangThai.Text =
                    "Không có mạng - cho phép nhập địa chỉ thủ công";

                ThongBaoLoi(
                    "Không tải được danh sách địa chỉ, vui lòng kiểm tra mạng. Bạn có thể nhập tay.");
            }
            finally
            {
                cboTinh.Enabled = true;
                _dangCapNhatDiaChi = false;
            }
        }

        // =========================
        // CHỌN TỈNH → TẢI PHƯỜNG/XÃ
        // =========================
        private async void cboTinh_SelectedIndexChanged(
            object? sender, EventArgs e)
        {
            if (_dangCapNhatDiaChi || !IsHandleCreated)
                return;

            await NapPhuongXaAsync();
        }

        private async Task NapPhuongXaAsync()
        {
            _ctsPhuong?.Cancel();
            _ctsPhuong?.Dispose();

            int lanTai = ++_lanTaiPhuong;

            _dangCapNhatDiaChi = true;

            cboPhuong.DataSource = null;
            cboPhuong.Items.Clear();
            cboPhuong.DropDownStyle = ComboBoxStyle.DropDown;
            cboPhuong.Text = "";
            cboPhuong.Enabled = true;

            _dangCapNhatDiaChi = false;

            if (cboTinh.SelectedItem is not DonViHanhChinh tinh ||
                cboTinh.Text != tinh.Name)
            {
                return;
            }

            var cts = new CancellationTokenSource();
            _ctsPhuong = cts;

            cboPhuong.Enabled = false;
            cboPhuong.Text = "Đang tải...";

            try
            {
                var ds = await HanhChinhApi.LayPhuongXaAsync(
                    tinh.Code, cts.Token);

                if (lanTai != _lanTaiPhuong || IsDisposed)
                    return;

                if (ds.Count == 0)
                    throw new Exception("Danh sách phường/xã trống.");

                cboPhuong.DisplayMember = "Name";
                cboPhuong.ValueMember = "Code";
                cboPhuong.DataSource =
                    ds.OrderBy(x => x.Name).ToList();

                cboPhuong.DropDownStyle =
                    ComboBoxStyle.DropDownList;

                cboPhuong.SelectedIndex = -1;

                lblTrangThai.Text =
                    $"Đã tải {ds.Count} phường/xã của {tinh.Name}";
            }
            catch (OperationCanceledException)
                when (cts.IsCancellationRequested)
            {
                // Người dùng chọn tỉnh khác.
            }
            catch (Exception ex)
            {
                if (lanTai != _lanTaiPhuong || IsDisposed)
                    return;

                Log.Warning(ex,
                    "Lỗi API phường/xã: MaTinh={MaTinh}",
                    tinh.Code);

                ChoNhapPhuongThuCong();

                ThongBaoLoi(
                    "Không tải được danh sách địa chỉ, vui lòng kiểm tra mạng. Bạn có thể nhập tay Phường/Xã.");
            }
            finally
            {
                if (lanTai == _lanTaiPhuong && !IsDisposed)
                    cboPhuong.Enabled = true;
            }
        }

        private void ChoNhapPhuongThuCong()
        {
            cboPhuong.DataSource = null;
            cboPhuong.Items.Clear();
            cboPhuong.DropDownStyle = ComboBoxStyle.DropDown;
            cboPhuong.Text = "";
            cboPhuong.Enabled = true;

            lblTrangThai.Text =
                "API lỗi - cho phép nhập Phường/Xã thủ công";
        }

        // =========================
        // VALIDATE
        // =========================
        private bool KiemTraDuLieu()
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                ThongBaoLoi("Họ tên không được để trống.");
                txtHoTen.Focus();
                return false;
            }

            string sdt = txtSDT.Text.Trim();

            if (sdt.Length < 9 ||
                sdt.Length > 11 ||
                !sdt.All(char.IsDigit))
            {
                ThongBaoLoi(
                    "Số điện thoại phải có 9-11 chữ số.");
                txtSDT.Focus();
                return false;
            }

            if (dtpNgaySinh.Value.Date > DateTime.Today)
            {
                ThongBaoLoi("Ngày sinh không hợp lệ.");
                return false;
            }

            if (cboPhong.SelectedItem is not PhongItem phong ||
                !phong.MaPhong.HasValue)
            {
                ThongBaoLoi("Vui lòng chọn phòng hợp lệ.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(cboTinh.Text) ||
                cboTinh.Text == "Đang tải...")
            {
                ThongBaoLoi("Vui lòng chọn hoặc nhập Tỉnh/Thành.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(cboPhuong.Text) ||
                cboPhuong.Text == "Đang tải...")
            {
                ThongBaoLoi("Vui lòng chọn hoặc nhập Phường/Xã.");
                return false;
            }

            if (cboTinh.Text.Trim().Length > 100 ||
                cboPhuong.Text.Trim().Length > 100)
            {
                ThongBaoLoi("Tên địa chỉ không được quá 100 ký tự.");
                return false;
            }

            return true;
        }

        // =========================
        // LẤY MÃ/TÊN ĐỊA CHỈ
        // =========================
        private int? LayMaTinh()
        {
            if (cboTinh.SelectedItem is DonViHanhChinh tinh &&
                cboTinh.Text == tinh.Name)
                return tinh.Code;

            return null;
        }

        private int? LayMaPhuong()
        {
            if (cboPhuong.SelectedItem is DonViHanhChinh phuong &&
                cboPhuong.Text == phuong.Name)
                return phuong.Code;

            return null;
        }

        private void GanThongTinKhach(KhachThue khach)
        {
            khach.HoTen = txtHoTen.Text.Trim();
            khach.GioiTinh = radNam.Checked ? "Nam" : "Nữ";
            khach.NgaySinh =
                DateOnly.FromDateTime(dtpNgaySinh.Value.Date);

            khach.SoDienThoai = txtSDT.Text.Trim();

            khach.MaTinh = LayMaTinh();
            khach.TenTinh = cboTinh.Text.Trim();

            khach.MaPhuongXa = LayMaPhuong();
            khach.TenPhuongXa = cboPhuong.Text.Trim();

            khach.DaDangKyTamTru = chkTamTru.Checked;
            khach.MaPhong =
                ((PhongItem)cboPhong.SelectedItem!).MaPhong!.Value;
        }

        // =========================
        // THÊM KHÁCH
        // =========================
        private async void btnThem_Click(
            object? sender, EventArgs e)
        {
            if (!KiemTraDuLieu())
                return;

            try
            {
                using var db = AppDb.Create();

                await using var tx = await db.Database
                    .BeginTransactionAsync(IsolationLevel.Serializable);

                int maPhong =
                    ((PhongItem)cboPhong.SelectedItem!).MaPhong!.Value;

                var phong = await db.Set<PhongTro>()
                    .FirstOrDefaultAsync(x => x.MaPhong == maPhong);

                if (phong == null || phong.TinhTrang != "Trống")
                {
                    ThongBaoLoi("Phòng này không còn trống.");
                    return;
                }

                var khach = new KhachThue();
                GanThongTinKhach(khach);

                db.Set<KhachThue>().Add(khach);
                phong.TinhTrang = "Đang thuê";

                await db.SaveChangesAsync();
                await tx.CommitAsync();

                Log.Information(
                    "Thêm khách MaKhach={MaKhach}, MaPhong={MaPhong}",
                    khach.MaKhach, maPhong);

                MessageBox.Show("Thêm khách thuê thành công!");

                await CapNhatGiaoDienAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi thêm khách thuê");
                ThongBaoLoi("Không thể thêm khách thuê.");
            }
        }

        // =========================
        // SỬA KHÁCH
        // =========================
        private async void btnSua_Click(
            object? sender, EventArgs e)
        {
            if (_maKhachDangChon == 0)
            {
                ThongBaoLoi("Vui lòng chọn khách cần sửa.");
                return;
            }

            if (!KiemTraDuLieu())
                return;

            try
            {
                using var db = AppDb.Create();

                await using var tx = await db.Database
                    .BeginTransactionAsync(IsolationLevel.Serializable);

                var khach = await db.Set<KhachThue>()
                    .FindAsync(_maKhachDangChon);

                if (khach == null)
                {
                    ThongBaoLoi("Không tìm thấy khách thuê.");
                    return;
                }

                int phongCu = khach.MaPhong;
                int phongMoi =
                    ((PhongItem)cboPhong.SelectedItem!).MaPhong!.Value;

                if (phongCu != phongMoi)
                {
                    var phong = await db.Set<PhongTro>()
                        .FirstOrDefaultAsync(x =>
                            x.MaPhong == phongMoi);

                    if (phong == null || phong.TinhTrang != "Trống")
                    {
                        ThongBaoLoi("Phòng mới không còn trống.");
                        return;
                    }

                    phong.TinhTrang = "Đang thuê";
                }

                GanThongTinKhach(khach);
                await db.SaveChangesAsync();

                if (phongCu != phongMoi)
                {
                    bool conKhach = await db.Set<KhachThue>()
                        .AnyAsync(x => x.MaPhong == phongCu);

                    if (!conKhach)
                    {
                        var phong = await db.Set<PhongTro>()
                            .FindAsync(phongCu);

                        if (phong != null)
                            phong.TinhTrang = "Trống";
                    }

                    await db.SaveChangesAsync();
                }

                await tx.CommitAsync();

                Log.Information(
                    "Sửa khách MaKhach={MaKhach}",
                    khach.MaKhach);

                MessageBox.Show("Cập nhật khách thuê thành công!");

                await CapNhatGiaoDienAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex,
                    "Lỗi sửa khách MaKhach={MaKhach}",
                    _maKhachDangChon);

                ThongBaoLoi("Không thể sửa khách thuê.");
            }
        }

        // =========================
        // XÓA KHÁCH
        // =========================
        private async void btnXoa_Click(
            object? sender, EventArgs e)
        {
            if (_maKhachDangChon == 0)
            {
                ThongBaoLoi("Vui lòng chọn khách cần xóa.");
                return;
            }

            if (MessageBox.Show(
                "Bạn có chắc chắn muốn xóa khách thuê này?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using var db = AppDb.Create();

                await using var tx = await db.Database
                    .BeginTransactionAsync(IsolationLevel.Serializable);

                var khach = await db.Set<KhachThue>()
                    .FindAsync(_maKhachDangChon);

                if (khach == null)
                {
                    ThongBaoLoi("Không tìm thấy khách thuê.");
                    return;
                }

                int maKhach = khach.MaKhach;
                int maPhong = khach.MaPhong;

                db.Set<KhachThue>().Remove(khach);
                await db.SaveChangesAsync();

                bool conKhach = await db.Set<KhachThue>()
                    .AnyAsync(x => x.MaPhong == maPhong);

                if (!conKhach)
                {
                    var phong = await db.Set<PhongTro>()
                        .FindAsync(maPhong);

                    if (phong != null)
                    {
                        phong.TinhTrang = "Trống";
                        await db.SaveChangesAsync();
                    }
                }

                await tx.CommitAsync();

                Log.Information(
                    "Xóa khách MaKhach={MaKhach}",
                    maKhach);

                MessageBox.Show("Xóa khách thuê thành công!");

                await CapNhatGiaoDienAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex,
                    "Lỗi xóa khách MaKhach={MaKhach}",
                    _maKhachDangChon);

                ThongBaoLoi("Không thể xóa khách thuê.");
            }
        }

        // =========================
        // TÌM KIẾM
        // =========================
        private async void btnTim_Click(
            object? sender, EventArgs e)
        {
            await NapDanhSachAsync();
        }

        private async void btnBoLoc_Click(
            object? sender, EventArgs e)
        {
            txtTimKiem.Clear();

            if (cboLocPhong.Items.Count > 0)
                cboLocPhong.SelectedIndex = 0;

            if (cboLocTinh.Items.Count > 0)
                cboLocTinh.SelectedIndex = 0;

            await NapDanhSachAsync();
        }

        // =========================
        // LÀM MỚI
        // =========================
        private async void btnLamMoi_Click(
            object? sender, EventArgs e)
        {
            try
            {
                LamMoiNhapLieu();
                await NapPhongNhapAsync();
                await NapDanhSachAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi làm mới");
                ThongBaoLoi("Không thể làm mới.");
            }
        }

        private async Task CapNhatGiaoDienAsync()
        {
            try
            {
                LamMoiNhapLieu();

                await NapBoLocAsync();
                await NapPhongNhapAsync();
                await NapDanhSachAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi cập nhật giao diện");
                ThongBaoLoi("Dữ liệu đã lưu nhưng chưa thể tải lại.");
            }
        }

        // =========================
        // CHỌN DÒNG
        // =========================
        private async void dgvKhachThue_SelectionChanged(
            object? sender, EventArgs e)
        {
            if (_dangNapLuoi ||
                dgvKhachThue.SelectedRows.Count == 0)
            {
                return;
            }

            if (dgvKhachThue.SelectedRows[0].DataBoundItem
                is not KhachRow khach)
            {
                return;
            }

            int lanChon = ++_lanChonDong;

            _maKhachDangChon = khach.MaKhach;

            txtHoTen.Text = khach.HoTen;
            txtSDT.Text = khach.SoDienThoai;

            radNam.Checked = khach.GioiTinh == "Nam";
            radNu.Checked = khach.GioiTinh == "Nữ";

            dtpNgaySinh.Value =
                khach.NgaySinh.ToDateTime(TimeOnly.MinValue);

            chkTamTru.Checked = khach.DaDangKyTamTru;

            try
            {
                await NapPhongNhapAsync(khach.MaPhong, lanChon);

                if (lanChon != _lanChonDong)
                    return;

                await GanDiaChiKhachAsync(khach, lanChon);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi chọn khách thuê");
                ThongBaoLoi("Không thể hiển thị đầy đủ thông tin khách.");
            }
        }

        // =========================
        // ĐỔ ĐỊA CHỈ LÊN CONTROL
        // =========================
        private async Task GanDiaChiKhachAsync(
            KhachRow khach, int lanChon)
        {
            _dangCapNhatDiaChi = true;

            try
            {
                DonViHanhChinh? tinh = cboTinh.Items
                    .OfType<DonViHanhChinh>()
                    .FirstOrDefault(x =>
                        (khach.MaTinh.HasValue &&
                         x.Code == khach.MaTinh.Value) ||
                        x.Name == khach.TenTinh);

                if (tinh != null)
                {
                    cboTinh.SelectedItem = tinh;
                }
                else
                {
                    cboTinh.SelectedIndex = -1;
                    cboTinh.DropDownStyle = ComboBoxStyle.DropDown;
                    cboTinh.Text = khach.TenTinh;
                }
            }
            finally
            {
                _dangCapNhatDiaChi = false;
            }

            await NapPhuongXaAsync();

            if (lanChon != _lanChonDong || IsDisposed)
                return;

            DonViHanhChinh? phuong = cboPhuong.Items
                .OfType<DonViHanhChinh>()
                .FirstOrDefault(x =>
                    (khach.MaPhuongXa.HasValue &&
                     x.Code == khach.MaPhuongXa.Value) ||
                    x.Name == khach.TenPhuongXa);

            if (phuong != null)
            {
                cboPhuong.SelectedItem = phuong;
            }
            else
            {
                cboPhuong.SelectedIndex = -1;
                cboPhuong.DropDownStyle = ComboBoxStyle.DropDown;
                cboPhuong.Text = khach.TenPhuongXa;
            }
        }

        // =========================
        // RESET CONTROL
        // =========================
        private void LamMoiNhapLieu()
        {
            ++_lanChonDong;
            ++_lanTaiPhuong;

            _ctsPhuong?.Cancel();

            _maKhachDangChon = 0;
            _dangCapNhatDiaChi = true;

            txtHoTen.Clear();
            txtSDT.Clear();

            radNam.Checked = true;
            dtpNgaySinh.Value = DateTime.Today.AddYears(-20);
            chkTamTru.Checked = false;

            cboPhong.SelectedIndex = -1;
            cboTinh.SelectedIndex = -1;

            cboPhuong.DataSource = null;
            cboPhuong.Items.Clear();
            cboPhuong.DropDownStyle = ComboBoxStyle.DropDown;
            cboPhuong.Text = "";

            if (dgvKhachThue.IsHandleCreated)
                dgvKhachThue.ClearSelection();

            _dangCapNhatDiaChi = false;
            txtHoTen.Focus();
        }

        // =========================
        // KIỂM TRA GLOBAL EXCEPTION
        // Ctrl + Shift + F12 để hiện nút
        // =========================
        private void FormKhachThue_KeyDown(
            object? sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && e.KeyCode == Keys.F12)
            {
                btnGayLoiThu.Visible = !btnGayLoiThu.Visible;
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void btnGayLoiThu_Click(
            object? sender, EventArgs e)
        {
            throw new InvalidOperationException(
                "Đây là lỗi thử để kiểm tra xử lý lỗi toàn cục.");
        }

        // =========================
        // THÔNG BÁO
        // =========================
        private void ThongBaoLoi(string noiDung)
        {
            MessageBox.Show(
                noiDung,
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        protected override void OnFormClosed(
            FormClosedEventArgs e)
        {
            _ctsPhuong?.Cancel();
            _ctsPhuong?.Dispose();

            base.OnFormClosed(e);
        }
    }
}
