
using Cau4_BanHangTraSua.Models;
using Microsoft.EntityFrameworkCore;

namespace Cau4_BanHangTraSua;

public partial class FormThucDon : Form
{
    private readonly TextBox txtTenMon = new();
    private readonly TextBox txtTimKiem = new();

    private readonly ComboBox cboDanhMuc = new();
    private readonly ComboBox cboLocDanhMuc = new();

    private readonly NumericUpDown nudGiaBan = new();
    private readonly CheckBox chkDangBan = new();

    private readonly DataGridView dgvMonUong = new();
    private readonly Label lblTrangThai = new();

    private int _maMonDangChon = 0;
    private bool _dangNapLuoi = false;

    private class DanhMucItem
    {
        public int? MaDM { get; set; }
        public string TenDM { get; set; } = "";
    }

    private class MonRow
    {
        public int MaMon { get; set; }
        public string TenMon { get; set; } = "";
        public int MaDM { get; set; }
        public string TenDM { get; set; } = "";
        public decimal GiaBan { get; set; }
        public bool DangBan { get; set; }
    }

    public FormThucDon()
    {
        InitializeComponent();

        Text = "Quản lý thực đơn - Quán trà sữa";
        ClientSize = new Size(1040, 650);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.WhiteSmoke;
        Font = new Font("Segoe UI", 10);

        KhoiTaoGiaoDien();

        Load += async (_, _) =>
        {
            await NapDanhMucAsync();
            await NapMonAsync();
            LamMoiNhapLieu();
        };
    }

    // ==========================
    // TẠO LABEL
    // ==========================
    private Label TaoNhan(
        string text, int x, int y, int width = 130)
    {
        return new Label
        {
            Text = text,
            Location = new Point(x, y),
            Size = new Size(width, 28),
            TextAlign = ContentAlignment.MiddleLeft
        };
    }

    // ==========================
    // TẠO BUTTON
    // ==========================
    private Button TaoNut(
        string text,
        int x,
        int y,
        EventHandler click,
        int width = 110)
    {
        var btn = new Button
        {
            Text = text,
            Location = new Point(x, y),
            Size = new Size(width, 35)
        };

        btn.Click += click;
        Controls.Add(btn);

        return btn;
    }

    // ==========================
    // TẠO CỘT DATAGRIDVIEW
    // ==========================
    private void ThemCot(
        string property,
        string header,
        int width,
        string format = "")
    {
        var col = new DataGridViewTextBoxColumn
        {
            DataPropertyName = property,
            HeaderText = header,
            Name = property,
            Width = width
        };

        if (!string.IsNullOrEmpty(format))
            col.DefaultCellStyle.Format = format;

        dgvMonUong.Columns.Add(col);
    }

    // ==========================
    // THIẾT KẾ GIAO DIỆN
    // ==========================
    private void KhoiTaoGiaoDien()
    {
        var title = TaoNhan(
            "QUẢN LÝ THỰC ĐƠN",
            22, 16, 450);

        title.Font = new Font(
            "Segoe UI", 15, FontStyle.Bold);

        Controls.Add(title);

        // Thông tin món
        var group = new GroupBox
        {
            Text = "Thông tin món",
            Location = new Point(20, 55),
            Size = new Size(1000, 185)
        };

        Controls.Add(group);

        // Tên món
        group.Controls.Add(
            TaoNhan("Tên món", 18, 35));

        txtTenMon.Location = new Point(145, 33);
        txtTenMon.Size = new Size(320, 25);
        txtTenMon.MaxLength = 100;

        group.Controls.Add(txtTenMon);

        // Danh mục
        group.Controls.Add(
            TaoNhan("Danh mục", 18, 78));

        cboDanhMuc.Location = new Point(145, 76);
        cboDanhMuc.Size = new Size(320, 25);
        cboDanhMuc.DropDownStyle =
            ComboBoxStyle.DropDownList;

        group.Controls.Add(cboDanhMuc);

        // Giá bán
        group.Controls.Add(
            TaoNhan("Giá bán", 525, 35));

        nudGiaBan.Location = new Point(645, 33);
        nudGiaBan.Size = new Size(290, 25);
        nudGiaBan.Minimum = 0;
        nudGiaBan.Maximum = 1000000000;
        nudGiaBan.Increment = 1000;
        nudGiaBan.ThousandsSeparator = true;

        group.Controls.Add(nudGiaBan);

        // Đang bán
        chkDangBan.Text = "Đang bán";
        chkDangBan.Checked = true;
        chkDangBan.Location = new Point(645, 77);
        chkDangBan.AutoSize = true;

        group.Controls.Add(chkDangBan);

        // CRUD
        TaoNut(
            "Thêm", 25, 255,
            async (_, _) => await ThemMonAsync());

        TaoNut(
            "Sửa", 145, 255,
            async (_, _) => await SuaMonAsync());

        TaoNut(
            "Xóa", 265, 255,
            async (_, _) => await XoaMonAsync());

        TaoNut(
            "Làm mới", 385, 255,
            async (_, _) =>
            {
                LamMoiNhapLieu();
                await NapMonAsync();
            });

        // Tìm kiếm
        Controls.Add(
            TaoNhan("Tìm món", 25, 313, 80));

        txtTimKiem.Location = new Point(110, 313);
        txtTimKiem.Size = new Size(270, 25);

        Controls.Add(txtTimKiem);

        // Lọc danh mục
        Controls.Add(
            TaoNhan("Danh mục", 400, 313, 100));

        cboLocDanhMuc.Location = new Point(500, 313);
        cboLocDanhMuc.Size = new Size(230, 25);
        cboLocDanhMuc.DropDownStyle =
            ComboBoxStyle.DropDownList;

        Controls.Add(cboLocDanhMuc);

        TaoNut(
            "Tìm kiếm", 755, 309,
            async (_, _) => await NapMonAsync());

        TaoNut(
            "Bỏ lọc", 875, 309,
            async (_, _) =>
            {
                txtTimKiem.Clear();

                if (cboLocDanhMuc.Items.Count > 0)
                    cboLocDanhMuc.SelectedIndex = 0;

                await NapMonAsync();
            });

        // DataGridView
        dgvMonUong.Location = new Point(20, 362);
        dgvMonUong.Size = new Size(1000, 242);
        dgvMonUong.BackgroundColor = Color.White;
        dgvMonUong.ReadOnly = true;
        dgvMonUong.AllowUserToAddRows = false;
        dgvMonUong.AllowUserToDeleteRows = false;
        dgvMonUong.AutoGenerateColumns = false;
        dgvMonUong.MultiSelect = false;

        dgvMonUong.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;

        ThemCot("MaMon", "Mã món", 100);
        ThemCot("TenMon", "Tên món", 310);
        ThemCot("TenDM", "Danh mục", 220);
        ThemCot("GiaBan", "Giá bán", 190, "N0");

        dgvMonUong.Columns.Add(
            new DataGridViewCheckBoxColumn
            {
                Name = "DangBan",
                HeaderText = "Đang bán",
                DataPropertyName = "DangBan",
                Width = 130
            });

        dgvMonUong.SelectionChanged += ChonDong;

        Controls.Add(dgvMonUong);

        // Trạng thái
        lblTrangThai.Location = new Point(22, 613);
        lblTrangThai.Size = new Size(500, 25);
        lblTrangThai.Text = "Tìm thấy 0 món";

        Controls.Add(lblTrangThai);
    }

    // ==========================
    // NẠP DANH MỤC
    // ==========================
    private async Task NapDanhMucAsync()
    {
        try
        {
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
            cboDanhMuc.DataSource = ds.ToList();

            var dsLoc = new List<DanhMucItem>
            {
                new DanhMucItem
                {
                    MaDM = null,
                    TenDM = "Tất cả"
                }
            };

            dsLoc.AddRange(ds);

            cboLocDanhMuc.DataSource = null;
            cboLocDanhMuc.DisplayMember = "TenDM";
            cboLocDanhMuc.ValueMember = "MaDM";
            cboLocDanhMuc.DataSource = dsLoc;

            cboLocDanhMuc.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            BaoLoi(ex);
        }
    }

    // ==========================
    // NẠP DANH SÁCH MÓN
    // ==========================
    private async Task NapMonAsync()
    {
        try
        {
            using var db = AppDb.Create();

            string ten = txtTimKiem.Text.Trim();

            int? maDM =
                (cboLocDanhMuc.SelectedItem
                    as DanhMucItem)?.MaDM;

            var query =
                from mon in db.Set<MonUong>().AsNoTracking()
                join dm in db.Set<DanhMucMon>().AsNoTracking()
                    on mon.MaDm equals dm.MaDm
                select new MonRow
                {
                    MaMon = mon.MaMon,
                    TenMon = mon.TenMon,
                    GiaBan = mon.GiaBan,
                    DangBan = mon.DangBan,
                    MaDM = dm.MaDm,
                    TenDM = dm.TenDm
                };

            if (!string.IsNullOrWhiteSpace(ten))
            {
                query = query.Where(x =>
                    x.TenMon.Contains(ten));
            }

            if (maDM.HasValue)
            {
                query = query.Where(x =>
                    x.MaDM == maDM.Value);
            }

            var ds = await query
                .OrderBy(x => x.MaMon)
                .ToListAsync();

            _dangNapLuoi = true;

            try
            {
                dgvMonUong.DataSource = ds;
                dgvMonUong.ClearSelection();

                lblTrangThai.Text =
                    $"Tìm thấy {ds.Count} món";
            }
            finally
            {
                _dangNapLuoi = false;
            }
        }
        catch (Exception ex)
        {
            BaoLoi(ex);
        }
    }

    // ==========================
    // CHỌN DÒNG
    // ==========================
    private void ChonDong(object? sender, EventArgs e)
    {
        if (_dangNapLuoi ||
            dgvMonUong.SelectedRows.Count == 0)
            return;

        if (dgvMonUong.SelectedRows[0].DataBoundItem
            is not MonRow mon)
            return;

        _maMonDangChon = mon.MaMon;

        txtTenMon.Text = mon.TenMon;
        nudGiaBan.Value = mon.GiaBan;
        chkDangBan.Checked = mon.DangBan;

        foreach (DanhMucItem dm in cboDanhMuc.Items)
        {
            if (dm.MaDM == mon.MaDM)
            {
                cboDanhMuc.SelectedItem = dm;
                break;
            }
        }
    }

    // ==========================
    // VALIDATE
    // ==========================
    private bool KiemTra()
    {
        if (string.IsNullOrWhiteSpace(txtTenMon.Text))
        {
            MessageBox.Show(
                "Tên món không được để trống.");

            txtTenMon.Focus();
            return false;
        }

        if (nudGiaBan.Value <= 0)
        {
            MessageBox.Show(
                "Giá bán phải lớn hơn 0.");

            nudGiaBan.Focus();
            return false;
        }

        if (cboDanhMuc.SelectedItem is not DanhMucItem)
        {
            MessageBox.Show(
                "Vui lòng chọn danh mục.");
            return false;
        }

        return true;
    }

    // ==========================
    // GÁN DỮ LIỆU
    // ==========================
    private void GanDuLieu(MonUong mon)
    {
        mon.TenMon = txtTenMon.Text.Trim();
        mon.GiaBan = nudGiaBan.Value;
        mon.DangBan = chkDangBan.Checked;

        mon.MaDm =
            ((DanhMucItem)cboDanhMuc.SelectedItem!)
                .MaDM!.Value;
    }

    // ==========================
    // THÊM
    // ==========================
    private async Task ThemMonAsync()
    {
        if (!KiemTra())
            return;

        try
        {
            using var db = AppDb.Create();

            var mon = new MonUong();
            GanDuLieu(mon);

            db.Set<MonUong>().Add(mon);

            await db.SaveChangesAsync();

            MessageBox.Show(
                "Thêm món thành công!");

            LamMoiNhapLieu();
            await NapMonAsync();
        }
        catch (Exception ex)
        {
            BaoLoi(ex);
        }
    }

    // ==========================
    // SỬA
    // ==========================
    private async Task SuaMonAsync()
    {
        if (_maMonDangChon == 0)
        {
            MessageBox.Show(
                "Vui lòng chọn món cần sửa.");
            return;
        }

        if (!KiemTra())
            return;

        try
        {
            using var db = AppDb.Create();

            var mon = await db.Set<MonUong>()
                .FindAsync(_maMonDangChon);

            if (mon == null)
            {
                MessageBox.Show(
                    "Không tìm thấy món.");
                return;
            }

            GanDuLieu(mon);

            await db.SaveChangesAsync();

            MessageBox.Show(
                "Sửa món thành công!");

            LamMoiNhapLieu();
            await NapMonAsync();
        }
        catch (Exception ex)
        {
            BaoLoi(ex);
        }
    }

    // ==========================
    // XÓA
    // ==========================
    private async Task XoaMonAsync()
    {
        if (_maMonDangChon == 0)
        {
            MessageBox.Show(
                "Vui lòng chọn món cần xóa.");
            return;
        }

        DialogResult result = MessageBox.Show(
            "Bạn có chắc chắn muốn xóa món này?",
            "Xác nhận xóa",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result != DialogResult.Yes)
            return;

        try
        {
            using var db = AppDb.Create();

            var mon = await db.Set<MonUong>()
                .FindAsync(_maMonDangChon);

            if (mon == null)
            {
                MessageBox.Show(
                    "Không tìm thấy món.");
                return;
            }

            // Không xóa món đã có trong hóa đơn
            bool daBan = await db.Set<ChiTietHoaDon>()
                .AnyAsync(x => x.MaMon == mon.MaMon);

            if (daBan)
            {
                MessageBox.Show(
                    "Món đã xuất hiện trong hóa đơn.\n" +
                    "Hãy bỏ tích Đang bán thay vì xóa.");
                return;
            }

            db.Set<MonUong>().Remove(mon);

            await db.SaveChangesAsync();

            MessageBox.Show(
                "Xóa món thành công!");

            LamMoiNhapLieu();
            await NapMonAsync();
        }
        catch (Exception ex)
        {
            BaoLoi(ex);
        }
    }

    // ==========================
    // LÀM MỚI
    // ==========================
    private void LamMoiNhapLieu()
    {
        _maMonDangChon = 0;

        txtTenMon.Clear();
        nudGiaBan.Value = 0;
        chkDangBan.Checked = true;

        if (cboDanhMuc.Items.Count > 0)
            cboDanhMuc.SelectedIndex = 0;

        dgvMonUong.ClearSelection();
        txtTenMon.Focus();
    }

    // ==========================
    // THÔNG BÁO LỖI
    // ==========================
    private void BaoLoi(Exception ex)
    {
        MessageBox.Show(
            "Không thể thực hiện thao tác:\n" +
            ex.Message,
            "Lỗi",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }
}
