using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebAuLac.Models
{
    public class HRM_EMPLOYMENTHISTORYMetadata
    {
        public int EmploymentHistoryID { get; set; }
        [Display(Name = "Số QĐ")]
        public string DecisionNo { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày QĐ")]
        public Nullable<System.DateTime> DecisionDate { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày HL")]
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        [Display(Name = "Loại QĐ")]
        public Nullable<int> CategoryDecisionID { get; set; }

        //[Display(Name = "Ngạch Bậc lương")]
        //public Nullable<int> SalaryStepID { get; set; }
        //[Display(Name = "Nhân viên")]
        //public Nullable<int> EmployeeID { get; set; }

        //[Display(Name = "Hệ số lương")]
        //[DisplayFormat(DataFormatString = "{0:#,#}")]
        //[RegularExpression("([0-9]+)")]
        //[Required(ErrorMessage = "Chỉ được nhập số")]
        //public Nullable<double> Coefficient { get; set; }

        //[Display(Name = "Tiền lương")]
        //[DisplayFormat(DataFormatString = "{0:#,#}")]
        //[RegularExpression("([0-9]+)")]
        //[Required(ErrorMessage = "Chỉ được nhập số")]
        //public Nullable<double> Salary { get; set; }

        //[Display(Name = "Hệ số bảo hiểm")]
        //[DisplayFormat(DataFormatString = "{0:#,#}")]
        //[RegularExpression("([0-9]+)")]
        //[Required(ErrorMessage = "Chỉ được nhập số")]
        //public Nullable<double> CoefficientIns { get; set; }

        //[Display(Name = "Tiền lương bảo hiểm")]
        //[DisplayFormat(DataFormatString = "{0:#,#}")]
        //[RegularExpression("([0-9]+)")]
        //[Required(ErrorMessage = "Chỉ được nhập số")]
        //public Nullable<double> SalaryIns { get; set; }

        //[Display(Name = "Bảo hiểm xã hội")]
        //[DisplayFormat(DataFormatString = "{0:#,#}")]
        //[RegularExpression("([0-9]+)")]
        //[Required(ErrorMessage = "Chỉ được nhập số")]
        //public Nullable<double> BHXH { get; set; }

        //[Display(Name = "Bảo hiểm y tế")]
        //[DisplayFormat(DataFormatString = "{0:#,#}")]
        //[RegularExpression("([0-9]+)")]
        //[Required(ErrorMessage = "Chỉ được nhập số")]
        //public Nullable<double> BHYT { get; set; }

        //[Display(Name = "Bảo hiểm thất nghiệp")]
        //[DisplayFormat(DataFormatString = "{0:#,#}")]
        //[RegularExpression("([0-9]+)")]
        //[Required(ErrorMessage = "Chỉ được nhập số")]
        //public Nullable<double> BHTN { get; set; }
        [Display(Name = "Nội dung")]
        public string ContentDecision { get; set; }
        [Display(Name = "Đơn vị tàu")]
        public Nullable<int> DepartmentID { get; set; }
        [Display(Name = "Chức danh")]
        public Nullable<int> PositionID { get; set; }
        [Display(Name = "Thực tập chức vụ")]
        public Nullable<bool> InternshipPosition { get; set; }
        [Display(Name = "Chức vụ kiêm nhiệm")]
        public Nullable<int> PluralityID { get; set; }
        [Display(Name = "Thực tập chức vụ kiêm nhiệm")]
        public Nullable<bool> IntershipPlurality { get; set; }
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
        [Display(Name = "%LươngCD")]
        public Nullable<int> PerPosition { get; set; }
        [Display(Name = "%LươngKN")]
        public Nullable<int> PerPlurality { get; set; }
        [Display(Name = "Lương CD")]
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        [RegularExpression("([0-9]+)")]
        public Nullable<int> SalaryPositionID { get; set; }
        [Display(Name = "Lương KN")]
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        [RegularExpression("([0-9]+)")]
        public Nullable<int> SalaryPluralityID { get; set; }
        [Display(Name = "Phòng")]
        public string DepartmentName { get; set; }
        public Nullable<int> LoaiTauID { get; set; }
        [Display(Name = "Dung tích")]
        public string Gross { get; set; }
        [Display(Name = "Công suất")]
        public string Power { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày XT")]
        public Nullable<System.DateTime> NgayXuongTau { get; set; }
        [Display(Name = "Xác nhận")]
        [UIHint("MyBoolean")]
        public Nullable<bool> XacNhan { get; set; }
    }
    public class HRM_EMPLOYEE_DEGREEMetadata
    {
        public int EmployeeDegreeID { get; set; }

        [Display(Name = "Bằng cấp cho")]
        public int EmployeeID { get; set; }

        [Display(Name = "Loại bằng")]
        public int DegreeID { get; set; }

        [Display(Name = "Tên trường")]
        public int SchoolID { get; set; }

        [Display(Name = "Bằng số")]
        public string DegreeNo { get; set; }
        [Display(Name = "Tiêu chuẩn")]
        public string Qualification { get; set; }

        [Display(Name = "Ngày cấp")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DegreeDate { get; set; }

        [Display(Name = "Thời hạn")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }
        [Display(Name = "Bằng cấp sai?")]
        public Nullable<bool> IsBC { get; set; }
    }
    public class HRM_EMPLOYEE_RELATIVEMetadata
    {
        public int PersonID { get; set; }

        public Nullable<int> EmployeeID { get; set; }
        [Display(Name = "Họ tên")]
        public string PersonName { get; set; }
        [Display(Name = "Mối quan hệ")]
        public Nullable<int> RelativeID { get; set; }

        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày sinh")]
        public string Birthday { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Nơi ở")]
        public string Address { get; set; }
        [RegularExpression("([0-9]+)")]
        [Display(Name = "Số ĐT")]
        public string Phone { get; set; }
        [Display(Name = "MST")]
        public string IncomeTaxCode { get; set; }
        [Display(Name = "Nghề nghiệp")]
        public string Job { get; set; }
        [Display(Name = "Địa chỉ nơi làm việc")]
        public string CompanyAddress { get; set; }

    }

    public partial class ChungChiTauMetadata
    {
        public int ChungChiID { get; set; }
        [Display(Name = "Tên loại hồ sơ")]
        public string TenChungChi { get; set; }
        [Display(Name = "Số ngày cảnh báo")] // đổi từ ngày sang tháng
        public Nullable<int> SoNgayCanhBao { get; set; }
    }
    public partial class HoSoCongTyMetadata
    {
        public int HoSoCongTyID { get; set; }
        [Display(Name ="Tên hồ sơ")]
        public string TenHoSo { get; set; }

        [Display(Name = "Số hồ sơ")]
        public string SoHoSo { get; set; }

        [Display(Name = "Nơi cấp")]
        public string NoiCap { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày Hồ sơ")]
        [Required(ErrorMessage ="Ngày hồ sơ bắt buộc phải có")]
        public Nullable<System.DateTime> NgayHoSo { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày Hết hạn")]
        [Required(ErrorMessage = "Ngày hết hạn bắt buộc phải có")]
        public Nullable<System.DateTime> NgayHetHan { get; set; }

        [Display(Name ="File đính kèm")]
        public string FileDinhKem { get; set; }

        [Display(Name = "Số ngày cảnh báo")]
        public Nullable<int> SoNgayCanhBao { get; set; }
    }
    public partial class HoSoTauMetadata
    {
        public int HoSoTauID { get; set; }

        public Nullable<int> DepartmentID { get; set; }
        public Nullable<int> ChungChiID { get; set; }

        [Display(Name = "Số hồ sơ")]
        public string SoHoSo { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày cấp")]
        [Required(ErrorMessage = "Ngày cấp bắt buộc phải có")]
        public Nullable<System.DateTime> NgayCap { get; set; }
        
        [Display(Name = "Nơi cấp")]
        public string NoiCap { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày Hết hạn")]
        [Required(ErrorMessage = "Ngày hết hạn bắt buộc phải có")]
        public Nullable<System.DateTime> NgayHetHan { get; set; }

        [Display(Name = "File đính kèm")]
        public string FileDinhKem { get; set; }

    }
    public partial class sp_LayDSChungChiTau_ResultMetadata
    {
        public int HoSoTauID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<int> ChungChiID { get; set; }
        [Display(Name = "Tên hồ sơ")]
        public string TenChungChi { get; set; }
        [Display(Name = "Số ngày cảnh báo")]
        public Nullable<int> SoNgayCanhBao { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày cấp")]
        public Nullable<System.DateTime> NgayCap { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày Hết hạn")]
        public Nullable<System.DateTime> NgayHetHan { get; set; }
        public string FileDinhKem { get; set; }
    }
    public class HRM_CONTRACTHISTORYMetadata
    {

        public int ContractHistoryID { get; set; }
        [Display(Name = "Loại HĐ")]
        public Nullable<int> ContractTypeID { get; set; }
        [Display(Name = "Số HĐ")]
        public string ContractNo { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        [Display(Name = "Ngày ký HĐ")]
        public Nullable<System.DateTime> ContractDate { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày hiệu lực")]
        public Nullable<System.DateTime> EffctiveDate { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày hết hạn")]
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public int EmployeeID { get; set; }


    }
    public class HRM_EMPLOYEE_RETRIBUTIONMetadata
    {
        public int EmployeeRetributionID { get; set; }

        public Nullable<int> EmployeeID { get; set; }
        [Display(Name = "Loại khen thưởng")]
        public Nullable<int> TypeOfRetributionID { get; set; }
        [Display(Name = "Số QĐ")]
        public string RetributionNo { get; set; }
        [Display(Name = "Ngày khen thưởng")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> RetributionDate { get; set; }
        [Display(Name = "Lý do khen thưởng")]
        public string Reason { get; set; }


    }
    public class HRM_EMPLOYEE_DISCIPLINEMetadata
    {
        public int EmployeeDisciplineID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        [Display(Name = "Loại kỷ luật")]
        public Nullable<int> TypeOfDisciplineID { get; set; }
        [Display(Name = "Số QĐ")]
        public string DisciplineNo { get; set; }
        [Display(Name = "Ngày kỷ luật")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DisciplineDate { get; set; }
        [Display(Name = "Lý do kỷ luật")]

        public string Reason { get; set; }


    }
    public partial class sp_BangTheoDoiGiayToTau_ResultMetadata
    {
        public int ChungChiID { get; set; }
        public string TenChungChi { get; set; }
        public Nullable<int> SoNgayCanhBao { get; set; }
        public Nullable<int> STT { get; set; }
        public string Thang1 { get; set; }
        public string Thang2 { get; set; }
        public string Thang3 { get; set; }
        public string Thang4 { get; set; }
        public string Thang5 { get; set; }
        public string Thang6 { get; set; }
        public string Thang7 { get; set; }
        public string Thang8 { get; set; }
        public string Thang9 { get; set; }
        public string Thang10 { get; set; }
        public string Thang11 { get; set; }
        public string Thang12 { get; set; }
        public string Thang13 { get; set; }
        public string Thang14 { get; set; }
        public string Thang15 { get; set; }
        [Display(Name = "Ngày báo cáo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> NgayBaoCao { get; set; }
    }
    public partial class HeDaoTaoMetadata
    {
  
        public int HeDaoTaoID { get; set; }
        [Display(Name = "Hệ đào tạo")]
        public string TenHeDaoTao { get; set; }

    }
    public partial class TrinhDoAnhVanMetadata
    {
       
        public int TrinhDoAnhVanID { get; set; }
        [Display(Name = "Trình độ Anh Văn")]
        public string TenTrinhDoAnhVan { get; set; }
 
    }

    public partial class TrinhDoViTinhMetadata
    {
        
        public int TrinhDoViTinhID { get; set; }
        [Display(Name = "Trình độ Vi tính")]
        public string TenTrinhDoViTinh { get; set; }
 
    }
}