
using Cau3_KeHoachDaNgoai.Models;
using Cau3_KeHoachDaNgoai.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Cau3_KeHoachDaNgoai
{
    public partial class FormChuyenDi : Form
    {
        private int _maChuyenDangChon = 0;
        private int _yeuCauThoiTiet = 0;

        private bool _dangKhoiTao = true;
        private bool _dangNapLuoi = false;

        private List<ChuyenDi> _duLieuLoc = new();

        private DuBaoThoiTiet? _duBaoHienTai;

        private enum TrangThaiThoiTiet
        {
            BanDau,
            DangTai,
            ThanhCong,
            KhongCoDuBao,
            KhongTimThay,
            LoiMang
        }

        private TrangThaiThoiTiet _trangThaiThoiTiet =
            TrangThaiThoiTiet.BanDau;

        private static string T(string vi, string en) =>
            AppLanguage.T(vi, en);

        private class PhuongTienItem
        {
            public string GiaTri { get; set; } = "";
            public string HienThi { get; set; } = "";
        }

        private class ChuyenDiRow
        {
            public int MaChuyen { get; set; }
            public string TenChuyen { get; set; } = "";
            public string DiaDiem { get; set; } = "";
            public DateTime NgayDi { get; set; }
            public int SoNguoi { get; set; }
            public decimal NganSach { get; set; }
            public string PhuongTien { get; set; } = "";
            public string PhuongTienGoc { get; set; } = "";
            public bool DaHoanThanh { get; set; }
            public string GhiChu { get; set; } = "";
        }

        public FormChuyenDi()
        {
            InitializeComponent();
            Load += FormChuyenDi_Load;
        }

        // ============================
        // LOAD FORM
        // ============================
        private async void FormChuyenDi_Load(
            object? sender, EventArgs e)
        {
            cboNgonNgu.Items.AddRange(
                new object[] { "Tiếng Việt", "English" });

            cboNgonNgu.SelectedIndex =
                AppLanguage.IsEnglish ? 1 : 0;

            dtpNgayDi.Value = DateTime.Today.AddDays(1);
            dtpTuNgay.Value = DateTime.Today;
            dtpDenNgay.Value = DateTime.Today.AddDays(40);

            _dangKhoiTao = false;

            ApDungNgonNgu();

            try
            {
                await NapDanhSachAsync();
                LamMoiNhapLieu();
            }
            catch (Exception ex)
            {
                BaoLoiDatabase(ex);
            }
        }

        // ============================
        // DANH SÁCH PHƯƠNG TIỆN
        // ============================
        private void NapPhuongTien()
        {
            string? giaTriCu =
                (cboPhuongTien.SelectedItem as PhuongTienItem)?
                    .GiaTri;

            var danhSach = new List<PhuongTienItem>
            {
                new()
                {
                    GiaTri = "Xe máy",
                    HienThi = T("Xe máy", "Motorbike")
                },
                new()
                {
                    GiaTri = "Ô tô",
                    HienThi = T("Ô tô", "Car")
                },
                new()
                {
                    GiaTri = "Xe khách",
                    HienThi = T("Xe khách", "Bus")
                },
                new()
                {
                    GiaTri = "Tàu hỏa",
                    HienThi = T("Tàu hỏa", "Train")
                }
            };

            cboPhuongTien.DataSource = null;
            cboPhuongTien.DisplayMember = "HienThi";
            cboPhuongTien.ValueMember = "GiaTri";
            cboPhuongTien.DataSource = danhSach;

            if (giaTriCu != null)
            {
                cboPhuongTien.SelectedItem =
                    danhSach.FirstOrDefault(
                        x => x.GiaTri == giaTriCu);
            }
            else
            {
                cboPhuongTien.SelectedIndex = 0;
            }
        }

        private string DichPhuongTien(string giaTri)
        {
            return giaTri switch
            {
                "Xe máy" => T("Xe máy", "Motorbike"),
                "Ô tô" => T("Ô tô", "Car"),
                "Xe khách" => T("Xe khách", "Bus"),
                "Tàu hỏa" => T("Tàu hỏa", "Train"),
                _ => giaTri
            };
        }

        // ============================
        // NẠP DỮ LIỆU + LỌC
        // ============================
        private async Task NapDanhSachAsync()
        {
            if (dtpTuNgay.Value.Date > dtpDenNgay.Value.Date)
            {
                BaoLoi(
                    "Từ ngày phải nhỏ hơn hoặc bằng Đến ngày.",
                    "From date must not be after To date.");
                return;
            }

            try
            {
                using var db = AppDb.Create();

                IQueryable<ChuyenDi> query =
                    db.Set<ChuyenDi>().AsNoTracking();

                string tuKhoa = txtTimKiem.Text.Trim();

                if (!string.IsNullOrWhiteSpace(tuKhoa))
                {
                    query = query.Where(x =>
                        x.TenChuyen.Contains(tuKhoa) ||
                        x.DiaDiem.Contains(tuKhoa));
                }

                if (chkChuaHoanThanh.Checked)
                {
                    query = query.Where(x =>
                        !x.DaHoanThanh);
                }

                DateOnly tuNgay =
                    DateOnly.FromDateTime(
                        dtpTuNgay.Value.Date);

                DateOnly denNgay =
                    DateOnly.FromDateTime(
                        dtpDenNgay.Value.Date);

                query = query.Where(x =>
                    x.NgayDi >= tuNgay &&
                    x.NgayDi <= denNgay);

                _duLieuLoc = await query
                    .OrderBy(x => x.NgayDi)
                    .ThenBy(x => x.MaChuyen)
                    .ToListAsync();

                CapNhatLuoi();
            }
            catch (Exception ex)
            {
                BaoLoiDatabase(ex);
            }
        }

        // ============================
        // HIỂN THỊ DATAGRIDVIEW
        // ============================
        private void CapNhatLuoi()
        {
            _dangNapLuoi = true;

            try
            {
                var danhSach = _duLieuLoc.Select(x =>
                    new ChuyenDiRow
                    {
                        MaChuyen = x.MaChuyen,
                        TenChuyen = x.TenChuyen,
                        DiaDiem = x.DiaDiem,
                        NgayDi = x.NgayDi.ToDateTime(
                            TimeOnly.MinValue),
                        SoNguoi = x.SoNguoi,
                        NganSach = x.NganSach,
                        PhuongTien = DichPhuongTien(
                            x.PhuongTien),
                        PhuongTienGoc = x.PhuongTien,
                        DaHoanThanh = x.DaHoanThanh,
                        GhiChu = x.GhiChu ?? ""
                    }).ToList();

                dgvChuyenDi.DataSource = danhSach;
                dgvChuyenDi.ClearSelection();

                lblTrangThai.Text = AppLanguage.IsEnglish
                    ? $"Found {danhSach.Count} trips"
                    : $"Tìm thấy {danhSach.Count} chuyến đi";
            }
            finally
            {
                _dangNapLuoi = false;
            }
        }

        // ============================
        // KIỂM TRA DỮ LIỆU
        // ============================
        private bool KiemTraDuLieu(bool themMoi)
        {
            if (string.IsNullOrWhiteSpace(txtTenChuyen.Text))
            {
                BaoLoi(
                    "Tên chuyến đi không được để trống.",
                    "Trip name cannot be empty.");
                txtTenChuyen.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDiaDiem.Text))
            {
                BaoLoi(
                    "Địa điểm không được để trống.",
                    "Destination cannot be empty.");
                txtDiaDiem.Focus();
                return false;
            }

            if (themMoi && dtpNgayDi.Value.Date < DateTime.Today)
            {
                BaoLoi(
                    "Không được tạo chuyến đi trong quá khứ.",
                    "Cannot create a trip in the past.");
                return false;
            }

            if (nudSoNguoi.Value < 1)
            {
                BaoLoi(
                    "Số người phải lớn hơn hoặc bằng 1.",
                    "Number of people must be at least 1.");
                return false;
            }

            if (nudNganSach.Value < 0)
            {
                BaoLoi(
                    "Ngân sách không được âm.",
                    "Budget cannot be negative.");
                return false;
            }

            if (cboPhuongTien.SelectedItem is not PhuongTienItem)
            {
                BaoLoi(
                    "Vui lòng chọn phương tiện.",
                    "Please select transportation.");
                return false;
            }

            return true;
        }

        // ============================
        // GÁN THÔNG TIN CHUYẾN ĐI
        // ============================
        private void GanThongTin(ChuyenDi chuyen)
        {
            chuyen.TenChuyen = txtTenChuyen.Text.Trim();
            chuyen.DiaDiem = txtDiaDiem.Text.Trim();

            chuyen.NgayDi = DateOnly.FromDateTime(
                dtpNgayDi.Value.Date);

            chuyen.SoNguoi = (int)nudSoNguoi.Value;
            chuyen.NganSach = nudNganSach.Value;

            chuyen.PhuongTien =
                ((PhuongTienItem)cboPhuongTien.SelectedItem!)
                    .GiaTri;

            chuyen.DaHoanThanh = chkDaHoanThanh.Checked;

            chuyen.GhiChu =
                string.IsNullOrWhiteSpace(txtGhiChu.Text)
                    ? null
                    : txtGhiChu.Text.Trim();
        }

        // ============================
        // THÊM
        // ============================
        private async void btnThem_Click(
            object? sender, EventArgs e)
        {
            if (!KiemTraDuLieu(true))
                return;

            try
            {
                using var db = AppDb.Create();

                var chuyen = new ChuyenDi();

                GanThongTin(chuyen);

                db.Set<ChuyenDi>().Add(chuyen);

                await db.SaveChangesAsync();

                MessageBox.Show(
                    T("Thêm chuyến đi thành công!",
                      "Trip added successfully!"),
                    T("Thông báo", "Notice"));

                await NapDanhSachAsync();
                LamMoiNhapLieu();
            }
            catch (Exception ex)
            {
                BaoLoiDatabase(ex);
            }
        }

        // ============================
        // SỬA
        // ============================
        private async void btnSua_Click(
            object? sender, EventArgs e)
        {
            if (_maChuyenDangChon == 0)
            {
                BaoLoi(
                    "Vui lòng chọn chuyến đi cần sửa.",
                    "Please select a trip to edit.");
                return;
            }

            if (!KiemTraDuLieu(false))
                return;

            try
            {
                using var db = AppDb.Create();

                var chuyen = await db.Set<ChuyenDi>()
                    .FindAsync(_maChuyenDangChon);

                if (chuyen == null)
                {
                    BaoLoi(
                        "Không tìm thấy chuyến đi.",
                        "Trip not found.");
                    return;
                }

                GanThongTin(chuyen);

                await db.SaveChangesAsync();

                MessageBox.Show(
                    T("Cập nhật thành công!",
                      "Trip updated successfully!"),
                    T("Thông báo", "Notice"));

                await NapDanhSachAsync();
                LamMoiNhapLieu();
            }
            catch (Exception ex)
            {
                BaoLoiDatabase(ex);
            }
        }

        // ============================
        // XÓA
        // ============================
        private async void btnXoa_Click(
            object? sender, EventArgs e)
        {
            if (_maChuyenDangChon == 0)
            {
                BaoLoi(
                    "Vui lòng chọn chuyến đi cần xóa.",
                    "Please select a trip to delete.");
                return;
            }

            DialogResult result = MessageBox.Show(
                T("Bạn có chắc muốn xóa chuyến đi này?",
                  "Are you sure you want to delete this trip?"),
                T("Xác nhận xóa", "Confirm delete"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using var db = AppDb.Create();

                var chuyen = await db.Set<ChuyenDi>()
                    .FindAsync(_maChuyenDangChon);

                if (chuyen == null)
                {
                    BaoLoi(
                        "Không tìm thấy chuyến đi.",
                        "Trip not found.");
                    return;
                }

                db.Set<ChuyenDi>().Remove(chuyen);

                await db.SaveChangesAsync();

                MessageBox.Show(
                    T("Xóa chuyến đi thành công!",
                      "Trip deleted successfully!"),
                    T("Thông báo", "Notice"));

                await NapDanhSachAsync();
                LamMoiNhapLieu();
            }
            catch (Exception ex)
            {
                BaoLoiDatabase(ex);
            }
        }

        // ============================
        // LÀM MỚI
        // ============================
        private async void btnLamMoi_Click(
            object? sender, EventArgs e)
        {
            txtTimKiem.Clear();
            chkChuaHoanThanh.Checked = false;

            dtpTuNgay.Value = DateTime.Today;
            dtpDenNgay.Value = DateTime.Today.AddDays(40);

            LamMoiNhapLieu();
            await NapDanhSachAsync();
        }

        private void LamMoiNhapLieu()
        {
            _maChuyenDangChon = 0;

            txtTenChuyen.Clear();
            txtDiaDiem.Clear();
            txtGhiChu.Clear();

            dtpNgayDi.Value = DateTime.Today.AddDays(1);
            nudSoNguoi.Value = 1;
            nudNganSach.Value = 0;

            chkDaHoanThanh.Checked = false;

            if (cboPhuongTien.Items.Count > 0)
                cboPhuongTien.SelectedIndex = 0;

            dgvChuyenDi.ClearSelection();

            ++_yeuCauThoiTiet;
            btnXemThoiTiet.Enabled = true;
            prgThoiTiet.Visible = false;

            _duBaoHienTai = null;
            _trangThaiThoiTiet = TrangThaiThoiTiet.BanDau;
            HienThiThoiTiet();

            txtTenChuyen.Focus();
        }

        // ============================
        // TÌM KIẾM
        // ============================
        private async void btnTim_Click(
            object? sender, EventArgs e)
        {
            _maChuyenDangChon = 0;
            await NapDanhSachAsync();
        }

        private async void btnBoLoc_Click(
            object? sender, EventArgs e)
        {
            txtTimKiem.Clear();
            chkChuaHoanThanh.Checked = false;

            dtpTuNgay.Value = DateTime.Today;
            dtpDenNgay.Value = DateTime.Today.AddDays(40);

            _maChuyenDangChon = 0;

            await NapDanhSachAsync();
        }

        // ============================
        // CHỌN DÒNG
        // ============================
        private void dgvChuyenDi_SelectionChanged(
            object? sender, EventArgs e)
        {
            if (_dangNapLuoi ||
                dgvChuyenDi.SelectedRows.Count == 0)
                return;

            if (dgvChuyenDi.SelectedRows[0].DataBoundItem
                is not ChuyenDiRow chuyen)
                return;

            _maChuyenDangChon = chuyen.MaChuyen;

            txtTenChuyen.Text = chuyen.TenChuyen;
            txtDiaDiem.Text = chuyen.DiaDiem;
            txtGhiChu.Text = chuyen.GhiChu;

            dtpNgayDi.Value = chuyen.NgayDi;
            nudSoNguoi.Value = chuyen.SoNguoi;

            nudNganSach.Value = Math.Min(
                nudNganSach.Maximum,
                Math.Max(nudNganSach.Minimum, chuyen.NganSach));

            chkDaHoanThanh.Checked = chuyen.DaHoanThanh;

            foreach (PhuongTienItem item in cboPhuongTien.Items)
            {
                if (item.GiaTri == chuyen.PhuongTienGoc)
                {
                    cboPhuongTien.SelectedItem = item;
                    break;
                }
            }

            // Hủy hiển thị kết quả của dòng cũ
            ++_yeuCauThoiTiet;

            btnXemThoiTiet.Enabled = true;
            prgThoiTiet.Visible = false;

            _duBaoHienTai = null;
            _trangThaiThoiTiet = TrangThaiThoiTiet.BanDau;

            HienThiThoiTiet();
        }

        // ============================
        // XEM THỜI TIẾT
        // ============================
        private async void btnXemThoiTiet_Click(
            object? sender, EventArgs e)
        {
            if (dgvChuyenDi.SelectedRows.Count == 0 ||
                dgvChuyenDi.SelectedRows[0].DataBoundItem
                    is not ChuyenDiRow chuyen)
            {
                BaoLoi(
                    "Vui lòng chọn một chuyến đi trong danh sách.",
                    "Please select a trip from the list.");
                return;
            }

            int yeuCau = ++_yeuCauThoiTiet;

            btnXemThoiTiet.Enabled = false;
            prgThoiTiet.Visible = true;

            _trangThaiThoiTiet = TrangThaiThoiTiet.DangTai;
            _duBaoHienTai = null;

            HienThiThoiTiet();

            try
            {
                DateOnly ngayDi =
                    DateOnly.FromDateTime(chuyen.NgayDi);

                var ketQua =
                    await ThoiTietService.LayDuBaoAsync(
                        chuyen.DiaDiem, ngayDi);

                if (yeuCau != _yeuCauThoiTiet)
                    return;

                _duBaoHienTai = ketQua;

                _trangThaiThoiTiet = ketQua == null
                    ? TrangThaiThoiTiet.KhongCoDuBao
                    : TrangThaiThoiTiet.ThanhCong;
            }
            catch (KhongTimThayDiaDanhException)
            {
                if (yeuCau != _yeuCauThoiTiet)
                    return;

                _trangThaiThoiTiet =
                    TrangThaiThoiTiet.KhongTimThay;
            }
            catch (HttpRequestException)
            {
                if (yeuCau != _yeuCauThoiTiet)
                    return;

                _trangThaiThoiTiet =
                    TrangThaiThoiTiet.LoiMang;
            }
            catch (TaskCanceledException)
            {
                if (yeuCau != _yeuCauThoiTiet)
                    return;

                _trangThaiThoiTiet =
                    TrangThaiThoiTiet.LoiMang;
            }
            catch (Exception)
            {
                if (yeuCau != _yeuCauThoiTiet)
                    return;

                _trangThaiThoiTiet =
                    TrangThaiThoiTiet.LoiMang;
            }
            finally
            {
                if (yeuCau == _yeuCauThoiTiet)
                {
                    btnXemThoiTiet.Enabled = true;
                    prgThoiTiet.Visible = false;
                    HienThiThoiTiet();
                }
            }
        }

        // ============================
        // HIỂN THỊ KẾT QUẢ API
        // ============================
        private void HienThiThoiTiet()
        {
            pnlThoiTiet.BackColor = Color.White;
            lblLoiKhuyen.BackColor = Color.Transparent;

            lblTieuDeThoiTiet.Text =
                T("Dự báo thời tiết", "Weather forecast");

            lblBieuTuong.Text = "🌤️";
            lblNgayDuBao.Text = "";
            lblNhietDo.Text = "";
            lblXacSuatMua.Text = "";
            lblLoiKhuyen.Text = "";

            if (_trangThaiThoiTiet ==
                TrangThaiThoiTiet.BanDau)
            {
                lblMoTa.Text = T(
                    "Chọn chuyến để xem dự báo",
                    "Select a trip to see the forecast");
                return;
            }

            if (_trangThaiThoiTiet ==
                TrangThaiThoiTiet.DangTai)
            {
                lblMoTa.Text = T(
                    "Đang tải thời tiết...",
                    "Loading weather...");
                return;
            }

            if (_trangThaiThoiTiet ==
                TrangThaiThoiTiet.KhongCoDuBao)
            {
                lblMoTa.Text = T(
                    "Chưa có dữ liệu dự báo",
                    "Forecast is not available");

                lblLoiKhuyen.Text = T(
                    "Ngày đi nằm ngoài phạm vi dự báo 16 ngày.",
                    "Trip date is outside the 16-day forecast window.");
                return;
            }

            if (_trangThaiThoiTiet ==
                TrangThaiThoiTiet.KhongTimThay)
            {
                lblMoTa.Text = T(
                    "Không tìm thấy địa danh",
                    "Location not found");

                lblLoiKhuyen.Text = T(
                    "Hãy thử tên không dấu, ví dụ: Da Lat.",
                    "Try a name without accents, e.g. Da Lat.");
                return;
            }

            if (_trangThaiThoiTiet ==
                TrangThaiThoiTiet.LoiMang)
            {
                lblMoTa.Text = T(
                    "Không tải được thời tiết",
                    "Unable to load weather");

                lblLoiKhuyen.Text = T(
                    "Vui lòng kiểm tra mạng và thử lại.",
                    "Check your internet connection and try again.");
                return;
            }

            if (_duBaoHienTai == null)
                return;

            var duBao = _duBaoHienTai;

            lblBieuTuong.Text =
                ThoiTietService.LayBieuTuong(
                    duBao.WeatherCode);

            lblMoTa.Text =
                ThoiTietService.LayMoTa(
                    duBao.WeatherCode);

            lblNgayDuBao.Text =
                duBao.Ngay.ToDateTime(TimeOnly.MinValue)
                    .ToString("d", AppLanguage.Culture);

            string min = DinhDangNhietDo(duBao.NhietDoMin);
            string max = DinhDangNhietDo(duBao.NhietDoMax);

            lblNhietDo.Text = T(
                $"Nhiệt độ: {min} - {max}",
                $"Temperature: {min} - {max}");

            string mua = duBao.XacSuatMua.HasValue
                ? $"{duBao.XacSuatMua.Value}%"
                : "--";

            lblXacSuatMua.Text = T(
                $"Khả năng mưa: {mua}",
                $"Rain probability: {mua}");

            if (duBao.XacSuatMua >= 60)
            {
                pnlThoiTiet.BackColor =
                    Color.FromArgb(255, 230, 185);

                lblLoiKhuyen.BackColor =
                    Color.FromArgb(255, 203, 130);

                lblLoiKhuyen.Text = T(
                    "⚠ Nên mang áo mưa hoặc cân nhắc dời lịch.",
                    "⚠ Bring a raincoat or consider rescheduling.");
            }
            else
            {
                lblLoiKhuyen.Text = T(
                    "Hãy theo dõi dự báo trước ngày đi.",
                    "Check the forecast again before your trip.");
            }
        }

        private string DinhDangNhietDo(double? giaTri)
        {
            return giaTri.HasValue
                ? giaTri.Value.ToString(
                    "0.#", AppLanguage.Culture) + "°C"
                : "--";
        }

        // ============================
        // CHUYỂN NGÔN NGỮ
        // ============================
        private void cboNgonNgu_SelectedIndexChanged(
            object? sender, EventArgs e)
        {
            if (_dangKhoiTao)
                return;

            string language =
                cboNgonNgu.SelectedIndex == 1 ? "en" : "vi";

            try
            {
                AppLanguage.Save(language);
            }
            catch (Exception)
            {
                BaoLoi(
                    "Không lưu được lựa chọn ngôn ngữ.",
                    "Could not save language preference.");
            }

            ApDungNgonNgu();
        }

        private void ApDungNgonNgu()
        {
            Text = T(
                "Kế hoạch dã ngoại",
                "Picnic Planner");

            lblTieuDe.Text = T(
                "KẾ HOẠCH DÃ NGOẠI CUỐI TUẦN",
                "WEEKEND PICNIC PLANNER");

            lblNgonNgu.Text = T(
                "Ngôn ngữ", "Language");

            grpThongTin.Text = T(
                "Thông tin chuyến đi", "Trip information");

            lblTenChuyen.Text = T("Tên chuyến", "Trip name");
            lblDiaDiem.Text = T("Địa điểm", "Destination");
            lblNgayDi.Text = T("Ngày đi", "Trip date");
            lblSoNguoi.Text = T("Số người", "People");
            lblNganSach.Text = T("Ngân sách", "Budget");
            lblPhuongTien.Text = T("Phương tiện", "Transport");
            lblGhiChu.Text = T("Ghi chú", "Notes");

            chkDaHoanThanh.Text = T(
                "Đã hoàn thành", "Completed");

            btnThem.Text = T("Thêm", "Add");
            btnSua.Text = T("Sửa", "Edit");
            btnXoa.Text = T("Xóa", "Delete");
            btnLamMoi.Text = T("Làm mới", "Refresh");

            btnXemThoiTiet.Text = T(
                "Xem thời tiết", "View weather");

            lblTimKiem.Text = T("Tìm kiếm", "Search");

            chkChuaHoanThanh.Text = T(
                "Chỉ hiện chuyến chưa hoàn thành",
                "Incomplete trips only");

            lblTuNgay.Text = T("Từ", "From");
            lblDenNgay.Text = T("Đến", "To");

            btnTim.Text = T("Tìm", "Search");
            btnBoLoc.Text = T("Bỏ lọc", "Clear");

            colMaChuyen.HeaderText = T("Mã", "ID");
            colTenChuyen.HeaderText = T("Tên chuyến", "Trip name");
            colDiaDiem.HeaderText = T("Địa điểm", "Destination");
            colNgayDi.HeaderText = T("Ngày đi", "Trip date");
            colSoNguoi.HeaderText = T("Số người", "People");
            colNganSach.HeaderText = T("Ngân sách", "Budget");
            colPhuongTien.HeaderText = T("Phương tiện", "Transport");
            colDaHoanThanh.HeaderText = T("Hoàn thành", "Completed");

            dtpNgayDi.CustomFormat = AppLanguage.IsEnglish
                ? "MM/dd/yyyy"
                : "dd/MM/yyyy";

            dtpTuNgay.CustomFormat = dtpNgayDi.CustomFormat;
            dtpDenNgay.CustomFormat = dtpNgayDi.CustomFormat;

            colNgayDi.DefaultCellStyle.Format = "d";
            colNgayDi.DefaultCellStyle.FormatProvider =
                AppLanguage.Culture;

            colNganSach.DefaultCellStyle.Format = "N0";
            colNganSach.DefaultCellStyle.FormatProvider =
                AppLanguage.Culture;

            NapPhuongTien();
            CapNhatLuoi();
            HienThiThoiTiet();
        }

        // ============================
        // THÔNG BÁO LỖI
        // ============================
        private void BaoLoi(string vi, string en)
        {
            MessageBox.Show(
                T(vi, en),
                T("Thông báo", "Notice"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void BaoLoiDatabase(Exception ex)
        {
            MessageBox.Show(
                T("Không thể truy cập CSDL:\n",
                  "Database operation failed:\n") +
                ex.Message,
                T("Lỗi", "Error"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
