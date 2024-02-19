using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebAuLac.Models
{
    [MetadataType(typeof(HRM_ROLEMetadata))]
    public partial class HRM_ROLE { }

    [MetadataType(typeof(LichKiemTraTauMetadata))]
    public partial class LichKiemTraTau { }

    [MetadataType(typeof(HRM_EMPLOYEEMetadata))]
    public partial class HRM_EMPLOYEE
    {
        private AuLacEntities tempdb = new AuLacEntities();
        public string HoTen
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }
        public string DiaChiFull
        {
            get
            {
                string sDiaChi = this.ContactAddress;
                string quanHuyen = "";
                string tinhthanh = "";
                DIC_QUANHUYEN qh = tempdb.DIC_QUANHUYEN.Find(this.ContactAddress_Huyen);
                if (qh != null)
                {
                    quanHuyen = qh.QuanHuyenName;
                }
                DIC_TINHTHANHPHO tt = tempdb.DIC_TINHTHANHPHO.Find(this.ContactAddress_Tinh);
                if (tt != null)
                {
                    tinhthanh = tt.TinhThanhPhoName;
                }
                if (quanHuyen != "")
                {
                    sDiaChi += ", " + quanHuyen;
                }
                if (tinhthanh != "")
                {
                    sDiaChi += ", " + tinhthanh;
                }
                return sDiaChi;
            }
        }
    }

    [MetadataType(typeof(sp_T_LayDanhSachNhanVien_ResultMetadata))]
    public partial class sp_T_LayDanhSachNhanVien_Result
    {
        public string HoTen
        {
            get
            {
                return this.FirstName.Trim() + " " + this.LastName.Trim();
            }
        }
    }

    [MetadataType(typeof(viewHRM_EMPLOYMENTHISTORYMetadata))]
    public partial class viewHRM_EMPLOYMENTHISTORY
    {
        public string HoTen
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }
    }
    [MetadataType(typeof(HRM_PLANMetadata))]
    public partial class HRM_PLAN
    {
    }

    [MetadataType(typeof(sp_LayDSThuyenVien_ResultMetadata))]
    public partial class sp_LayDSThuyenVien_Result { }

    [MetadataType(typeof(sp_LayKeHoachDieuDong_ResultMetadata))]
    public partial class sp_LayKeHoachDieuDong_Result { }

    [MetadataType(typeof(sp_Luong_4_ResultMetadata))]
    public partial class sp_Luong_4_Result { }

    //[MetadataType(typeof(sp_LayDSThuyenVien_ResultMetadata))]
    //public partial class sp_LayDSThuyenVienDieuDong_Result { }

    [MetadataType(typeof(tbl_khoadaotaoMetadata))]
    public partial class tbl_khoadaotao {
        public string ThoiGian
        {
            get
            {
                string sKq = "";
                if (this.ngaybatdau.HasValue)
                {
                    sKq = this.ngaybatdau.Value.ToString("dd/MM/yyyy") + " - ";
                }
                if (this.NgayKetThuc.HasValue)
                {
                    sKq = sKq + this.NgayKetThuc.Value.ToString("dd/MM/yyyy");
                }
                return sKq;
            }
        }
    }

    [MetadataType(typeof(tbl_ctdaotaoMetadata))]
    public partial class tbl_ctdaotao { }

    [MetadataType(typeof(tbl_cosodaotaoMetadata))]
    public partial class tbl_cosodaotao { }

    [MetadataType(typeof(sp_T_LayDanhSachHocTheoThoiGian_ResultMetadata))]
    public partial class sp_T_LayDanhSachHocTheoThoiGian_Result
    {
        public string HoTen
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }
        public string ThoiGian
        {
            get
            {
                string sKq = "";
                if (this.ngaybatdau.HasValue)
                {
                    sKq = this.ngaybatdau.Value.ToString("dd/MM/yyyy") + " - ";
                }
                if (this.NgayKetThuc.HasValue)
                {
                    sKq = sKq + this.NgayKetThuc.Value.ToString("dd/MM/yyyy");
                }
                return sKq;
            }
        }
    }
    [MetadataType(typeof(sp_T_ThongKeDaoTaoCaNhan_ResultMetadata))]
    public partial class sp_T_ThongKeDaoTaoCaNhan_Result {
        public string ThoiGian
        {
            get
            {
                string sKq = "";
                if (this.ngaybatdau.HasValue)
                {
                    sKq = this.ngaybatdau.Value.ToString("dd/MM/yyyy") + " - ";
                }
                if (this.NgayKetThuc.HasValue)
                {
                    sKq = sKq + this.NgayKetThuc.Value.ToString("dd/MM/yyyy");
                }
                return sKq;
            }
        }
    }

    [MetadataType(typeof(tbl_QuanHeMetadata))]
    public partial class tbl_QuanHe { }

    [MetadataType(typeof(DIC_POSITIONMetadata))]
    public partial class DIC_POSITION { }

    [MetadataType(typeof(DIC_SCHOOLMetadata))]
    public partial class DIC_SCHOOL { }

    [MetadataType(typeof(DIC_EDUCATIONMetadata))]
    public partial class DIC_EDUCATION { }

    [MetadataType(typeof(DIC_CONTRACTTYPEMetadata))]
    public partial class DIC_CONTRACTTYPE { }

    [MetadataType(typeof(DIC_RELIGIONMetadata))]
    public partial class DIC_RELIGION { }

    [MetadataType(typeof(DIC_NATIONALITYMetadata))]
    public partial class DIC_NATIONALITY { }

    [MetadataType(typeof(DIC_RELATIVEMetadata))]
    public partial class DIC_RELATIVE { }

    [MetadataType(typeof(sp_LayDSKhongDatPassport_ResultMetadata))]
    public partial class sp_LayDSKhongDatPassport_Result { }

    [MetadataType(typeof(LoaiKiemTraMetadata))]
    public partial class LoaiKiemTra{ }

    [MetadataType(typeof(LichKiemTraMetadata))]
    public partial class LichKiemTra { }

    [MetadataType(typeof(DIC_DEPARTMENTMetadata))]
    public partial class DIC_DEPARTMENT{ }

    [MetadataType(typeof(HRM_SALARYMetadata))]
    public partial class HRM_SALARY { }

    [MetadataType(typeof(viewLuongThangMetadata))]
    public partial class viewLuongThang { }

    [MetadataType(typeof(HRM_DETAILSALARYMetadata))]
    public partial class HRM_DETAILSALARY { }
}