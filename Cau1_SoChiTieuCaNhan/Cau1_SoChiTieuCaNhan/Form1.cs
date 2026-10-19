using Cau1_SoChiTieuCaNhan.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Cau1_SoChiTieuCaNhan
{
    public partial class Form1 : Form
    {
        private int _maCtDangChon = 0;

        public Form1()
        {
            InitializeComponent();

            Load += Form1_Load;
        }

        private async void Form1_Load(
            object? sender,
            EventArgs e)
        {
            cboLoaiChi.Items.AddRange(
                new object[]
                {
                    "Ăn uống",
                    "Đi lại",
                    "Học tập",
                    "Giải trí",
                    "Khác"
                });

            cboLoaiChi.SelectedIndex = 0;

            cboLocLoaiChi.Items.AddRange(
                new object[]
                {
                    "Tất cả",
                    "Ăn uống",
                    "Đi lại",
                    "Học tập",
                    "Giải trí",
                    "Khác"
                });

            cboLocLoaiChi.SelectedIndex = 0;

            dtpNgayChi.Value =
                DateTime.Today;

            dtpTuNgay.Value =
                new DateTime(
                    DateTime.Today.Year,
                    DateTime.Today.Month,
                    1);

            dtpDenNgay.Value =
                DateTime.Today;

            await LoadDataAsync();

            LamMoiNhapLieu();
        }

        // ==========================
        // LOAD + FILTER
        // ==========================
        private async Task LoadDataAsync()
        {
            try
            {
                using QL_ChiTieuContext context =
                    AppServices.CreateDbContext();

                IQueryable<ChiTieu> query =
                    context.Set<ChiTieu>()
                        .AsNoTracking();

                string noiDung =
                    txtTimNoiDung.Text.Trim();

                if (!string.IsNullOrWhiteSpace(
                        noiDung))
                {
                    query =
                        query.Where(x =>
                            x.NoiDung.Contains(
                                noiDung));
                }

                string loaiChi =
                    cboLocLoaiChi
                        .SelectedItem?
                        .ToString()
                    ?? "Tất cả";

                if (loaiChi != "Tất cả")
                {
                    query =
                        query.Where(x =>
                            x.LoaiChi == loaiChi);
                }

                DateOnly tuNgay =
                    DateOnly.FromDateTime(
                        dtpTuNgay.Value.Date);

                DateOnly denNgay =
                    DateOnly.FromDateTime(
                        dtpDenNgay.Value.Date);

                query =
                    query.Where(x =>
                        x.NgayChi >= tuNgay
                        &&
                        x.NgayChi <= denNgay);

                List<ChiTieu> ds =
                    await query
                        .OrderByDescending(
                            x => x.NgayChi)
                        .ThenByDescending(
                            x => x.MaCt)
                        .ToListAsync();

                dgvChiTieu.DataSource =
                    ds;

                dgvChiTieu.ClearSelection();

                decimal tongChi =
                    ds.Sum(x => x.SoTien);

                lblTongChi.Text =
                    $"Tổng chi: {tongChi:N0} đ";
            }
            catch (Exception ex)
            {
                Log.Error(
                    ex,
                    "Lỗi tải dữ liệu chi tiêu");

                MessageBox.Show(
                    "Không thể tải dữ liệu. "
                    + "Vui lòng kiểm tra kết nối CSDL.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================
        // VALIDATE
        // ==========================
        private bool KiemTraDuLieu()
        {
            if (string.IsNullOrWhiteSpace(
                    txtNoiDung.Text))
            {
                MessageBox.Show(
                    "Nội dung không được để trống!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtNoiDung.Focus();

                return false;
            }

            if (nudSoTien.Value <= 0)
            {
                MessageBox.Show(
                    "Số tiền phải lớn hơn 0!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                nudSoTien.Focus();

                return false;
            }

            if (dtpNgayChi.Value.Date >
                DateTime.Today)
            {
                MessageBox.Show(
                    "Ngày chi không được lớn hơn hôm nay!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }

        // ==========================
        // THÊM
        // ==========================
        private async void btnThem_Click(
            object sender,
            EventArgs e)
        {
            if (!KiemTraDuLieu())
                return;

            try
            {
                using QL_ChiTieuContext context =
                    AppServices.CreateDbContext();

                ChiTieu chiTieu =
                    new ChiTieu
                    {
                        NgayChi =
                            DateOnly.FromDateTime(
                                dtpNgayChi.Value),

                        NoiDung =
                            txtNoiDung.Text.Trim(),

                        LoaiChi =
                            cboLoaiChi
                                .SelectedItem!
                                .ToString()!,

                        SoTien =
                            nudSoTien.Value,

                        GhiChu =
                            string.IsNullOrWhiteSpace(
                                txtGhiChu.Text)
                            ? null
                            : txtGhiChu.Text.Trim()
                    };

                context.Set<ChiTieu>()
                    .Add(chiTieu);

                await context
                    .SaveChangesAsync();

                Log.Information(
                    "Thêm chi tiêu MaCT={MaCT}",
                    chiTieu.MaCt);

                MessageBox.Show(
                    "Thêm khoản chi thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                await LoadDataAsync();

                LamMoiNhapLieu();
            }
            catch (Exception ex)
            {
                Log.Error(
                    ex,
                    "Lỗi thêm khoản chi");

                MessageBox.Show(
                    "Không thể thêm khoản chi. "
                    + "Vui lòng thử lại.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================
        // SỬA
        // ==========================
        private async void btnSua_Click(
            object sender,
            EventArgs e)
        {
            if (_maCtDangChon == 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn khoản chi cần sửa!");

                return;
            }

            if (!KiemTraDuLieu())
                return;

            try
            {
                using QL_ChiTieuContext context =
                    AppServices.CreateDbContext();

                ChiTieu? chiTieu =
                    await context
                        .Set<ChiTieu>()
                        .FindAsync(
                            _maCtDangChon);

                if (chiTieu == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy khoản chi!");

                    return;
                }

                chiTieu.NgayChi =
                    DateOnly.FromDateTime(
                        dtpNgayChi.Value);

                chiTieu.NoiDung =
                    txtNoiDung.Text.Trim();

                chiTieu.LoaiChi =
                    cboLoaiChi
                        .SelectedItem!
                        .ToString()!;

                chiTieu.SoTien =
                    nudSoTien.Value;

                chiTieu.GhiChu =
                    string.IsNullOrWhiteSpace(
                        txtGhiChu.Text)
                    ? null
                    : txtGhiChu.Text.Trim();

                await context
                    .SaveChangesAsync();

                Log.Information(
                    "Sửa chi tiêu MaCT={MaCT}",
                    chiTieu.MaCt);

                MessageBox.Show(
                    "Cập nhật khoản chi thành công!");

                await LoadDataAsync();

                LamMoiNhapLieu();
            }
            catch (Exception ex)
            {
                Log.Error(
                    ex,
                    "Lỗi sửa MaCT={MaCT}",
                    _maCtDangChon);

                MessageBox.Show(
                    "Không thể cập nhật khoản chi.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================
        // XÓA
        // ==========================
        private async void btnXoa_Click(
            object sender,
            EventArgs e)
        {
            if (_maCtDangChon == 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn khoản chi cần xóa!");

                return;
            }

            DialogResult result =
                MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa khoản chi này?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using QL_ChiTieuContext context =
                    AppServices.CreateDbContext();

                ChiTieu? chiTieu =
                    await context
                        .Set<ChiTieu>()
                        .FindAsync(
                            _maCtDangChon);

                if (chiTieu == null)
                    return;

                int maCt =
                    chiTieu.MaCt;

                context.Set<ChiTieu>()
                    .Remove(chiTieu);

                await context
                    .SaveChangesAsync();

                Log.Information(
                    "Xóa chi tiêu MaCT={MaCT}",
                    maCt);

                MessageBox.Show(
                    "Xóa khoản chi thành công!");

                await LoadDataAsync();

                LamMoiNhapLieu();
            }
            catch (Exception ex)
            {
                Log.Error(
                    ex,
                    "Lỗi xóa MaCT={MaCT}",
                    _maCtDangChon);

                MessageBox.Show(
                    "Không thể xóa khoản chi.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================
        // TÌM KIẾM
        // ==========================
        private async void btnTim_Click(
            object sender,
            EventArgs e)
        {
            if (dtpTuNgay.Value.Date >
                dtpDenNgay.Value.Date)
            {
                MessageBox.Show(
                    "Từ ngày phải nhỏ hơn hoặc bằng Đến ngày!");

                return;
            }

            await LoadDataAsync();
        }

        // ==========================
        // LÀM MỚI
        // ==========================
        private async void btnLamMoi_Click(
            object sender,
            EventArgs e)
        {
            txtTimNoiDung.Clear();

            cboLocLoaiChi.SelectedIndex =
                0;

            dtpTuNgay.Value =
                new DateTime(
                    DateTime.Today.Year,
                    DateTime.Today.Month,
                    1);

            dtpDenNgay.Value =
                DateTime.Today;

            LamMoiNhapLieu();

            await LoadDataAsync();
        }

        // ==========================
        // CHỌN DÒNG
        // ==========================
        private void dgvChiTieu_SelectionChanged(
            object sender,
            EventArgs e)
        {
            if (dgvChiTieu.SelectedRows.Count == 0)
                return;

            if (dgvChiTieu.SelectedRows[0]
                    .DataBoundItem
                is not ChiTieu chiTieu)
            {
                return;
            }

            _maCtDangChon =
                chiTieu.MaCt;

            dtpNgayChi.Value =
                chiTieu.NgayChi
                    .ToDateTime(
                        TimeOnly.MinValue);

            txtNoiDung.Text =
                chiTieu.NoiDung;

            cboLoaiChi.SelectedItem =
                chiTieu.LoaiChi;

            nudSoTien.Value =
                chiTieu.SoTien;

            txtGhiChu.Text =
                chiTieu.GhiChu ?? "";
        }

        // ==========================
        // RESET
        // ==========================
        private void LamMoiNhapLieu()
        {
            _maCtDangChon = 0;

            dtpNgayChi.Value =
                DateTime.Today;

            txtNoiDung.Clear();

            if (cboLoaiChi.Items.Count > 0)
                cboLoaiChi.SelectedIndex = 0;

            nudSoTien.Value = 0;

            txtGhiChu.Clear();

            dgvChiTieu.ClearSelection();

            txtNoiDung.Focus();
        }
    }
}