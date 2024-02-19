using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebAuLac.Models
{
    public partial class HRM_ROLEMetadata
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[StringLength(maximumLength: 100, ErrorMessage = "Chiều dài không quá 100 ký tự")]
        public string RoleID { get; set; }


        [Display(Name = "Tên nhóm quyền")]
        //[Required(ErrorMessage = "Thông tin bắt buộc phải nhập")]
        //[StringLength(maximumLength: 100, ErrorMessage = "Chiều dài không quá 100 ký tự")]
        public string RoleName { get; set; }

    }
    public partial class LichKiemTraTauMetadata
    {
        public int IDLichKiemTraTau { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> NgayKiemTra { get; set; }
        public Nullable<int> IDLoaiKiemTraTau { get; set; }
    }
    public partial class viewHRM_EMPLOYMENTHISTORYMetadata
    {
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        [Display(Name ="Họ lót")]
        public string FirstName { get; set; }
        [Display(Name ="Tên")]
        public string LastName { get; set; }

        [Display(Name ="Ngày sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> BirthDay { get; set; }
        public string BirthPlace { get; set; }
        public int EmploymentHistoryID { get; set; }
        public string DecisionNo { get; set; }
        [Display(Name = "Ngày QĐ lên tàu/dự trữ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DecisionDate { get; set; }

        public string ContentDecision { get; set; }
        public Nullable<int> CategoryDecisionID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<int> PositionID { get; set; }
        public string PositionName { get; set; }
        public Nullable<bool> InternshipPosition { get; set; }
        public Nullable<int> PluralityID { get; set; }
        public string PluralityName { get; set; }
        public Nullable<bool> IntershipPlurality { get; set; }
        public string EducationName { get; set; }
        public string EducationDescription { get; set; }
        public Nullable<int> EducationID { get; set; }
        public string ChucVu { get; set; }
        [Display(Name ="Địa chỉ")]
        public string ContactAddress { get; set; }
    }

    public class HRM_EMPLOYEEMetadata
    {
        public int EmployeeID { get; set; }

        [Display(Name = "Mã nhân viên")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Mã thẻ")]
        public string CardNo { get; set; }

        [Display(Name = "Họ")]
        [Required(ErrorMessage = "Họ bắt buộc phải nhập")]
        public string FirstName { get; set; }

        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Tên bắt buộc phải nhập")]
        public string LastName { get; set; }

        [Display(Name = "Bí danh")]
        public string Alias { get; set; }

        [UIHint("GioiTinh")]
        [Display(Name = "Giới tính")]
        public Nullable<bool> Sex { get; set; }

        [UIHint("MarriageStatus")]
        [Display(Name = "Tình trạng hôn nhân")]
        public string MarriageStatus { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> BirthDay { get; set; }

        [Display(Name = "Nơi sinh")]
        public string BirthPlace { get; set; }

        [Display(Name = "Thường trú")]
        public string MainAddress { get; set; }

        [Display(Name = "Địa chỉ")]
        public string ContactAddress { get; set; }

        [Display(Name = "Địa chỉ tiếng Anh")]
        public string DiaChiTiengAnh { get; set; }

        [Display(Name = "Di động")]
        public string CellPhone { get; set; }

        [Display(Name = "Điện thoại")]
        public string HomePhone { get; set; }


        public string Email { get; set; }
        [Display(Name = "Mã số BHXH")]
        public string Skype { get; set; }
        public string Yahoo { get; set; }
        public string Facebook { get; set; }

        [Display(Name = "CMND")]
        public string IDCard { get; set; }

        [Display(Name = "Ngày cấp")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> IDCardDate { get; set; }

        [Display(Name = "Nơi cấp")]
        public string IDCardPlace { get; set; }

        [Display(Name = "Mã số thuế")]
        public string TaxNo { get; set; }

        [Display(Name = "Số tài khoản")]
        public string BankCode { get; set; }

        [Display(Name = "Tại ngân hàng")]
        public string BankName { get; set; }

        [Display(Name = "Số thẻ bảo hiểm")]
        public string InsuranceCode { get; set; }

        [Display(Name = "Ngày thẻ bảo hiểm")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> InsuranceDate { get; set; }

        [Display(Name = "Hình ảnh")]
        public byte[] Photo { get; set; }

        [Display(Name = "Trình độ")]
        public Nullable<int> EducationID { get; set; }

        [Display(Name = "Học vị")]
        public Nullable<int> DegreeID { get; set; }

        [Display(Name = "Dân tộc")]
        public Nullable<int> EthnicID { get; set; }

        [Display(Name = "Tôn giáo")]
        public Nullable<int> ReligionID { get; set; }

        [Display(Name = "Quốc tịch")]
        public Nullable<int> NationalityID { get; set; }

        [Display(Name = "Chức vụ")]
        public Nullable<int> Department_PositionID { get; set; }

        [Display(Name = "Tình trạng")]
        public Nullable<int> StatusID { get; set; }

        [UIHint("SSDD")]
        [Display(Name = "Sẵn sàng điều động")]
        public Nullable<Boolean> SSDD { get; set; }

        [Display(Name = "Ngày SSĐĐ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> NgaySSDD { get; set; }

        [Display(Name = "Chiều cao")]
        public string Height { get; set; }
        [Display(Name = "Cân nặng")]
        public string Weight { get; set; }
        [Display(Name = "Nhóm máu")]
        public string BloodType { get; set; }
    }
    public class HRM_PLANMetadata
    {
        public int PlanID { get; set; }

        [Display(Name ="Tên bản kế hoạch")]
        [Required]
        public string PlanName { get; set; }
        [Display(Name = "Ngày lập")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public Nullable<System.DateTime> PlanDate { get; set; }
    }
    public class sp_LayDSThuyenVien_ResultMetadata
    {
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> BirthDay { get; set; }

        public string BirthPlace { get; set; }
        public int EmploymentHistoryID { get; set; }
        public string DecisionNo { get; set; }


        public Nullable<System.DateTime> DecisionDate { get; set; }
        public string ContentDecision { get; set; }
        public Nullable<int> CategoryDecisionID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<int> PositionID { get; set; }
        public string PositionName { get; set; }
        public Nullable<int> InternshipPosition { get; set; }
        public Nullable<int> PluralityID { get; set; }
        public string PluralityName { get; set; }
        public Nullable<int> IntershipPlurality { get; set; }
        public string EducationName { get; set; }
        public string EducationDescription { get; set; }
        public Nullable<int> EducationID { get; set; }
        public string ChucVu { get; set; }
        public string ContactAddress { get; set; }
        public string DepartmentDescription { get; set; }
        public string IntershipPositionName { get; set; }
        public string IntershipPluralityName { get; set; }
        public Nullable<int> PerPosition { get; set; }
        public Nullable<int> PerPlurality { get; set; }
        public Nullable<int> SalaryPositionID { get; set; }
        public Nullable<int> SalaryPluralityID { get; set; }

        [Display(Name = "Ngày hiệu lực")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> EffectiveDate { get; set; }

        [Display(Name = "Ngày lên tàu")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> NgayXuongTau { get; set; }

        public Nullable<int> SoNgay { get; set; }
        public string TGDT { get; set; }
        public string QTDT { get; set; }
    }
    public class sp_LayKeHoachDieuDong_ResultMetadata
    {
        public int DetailPlanID { get; set; }
        public Nullable<int> PlanID { get; set; }
        public Nullable<int> CrewOffID { get; set; }
        public Nullable<int> CrewOffDepartmentID { get; set; }
        public Nullable<int> OffPositionID { get; set; }
        public Nullable<bool> OffInternshipPosition { get; set; }
        public Nullable<int> OffPluralityID { get; set; }
        public Nullable<bool> OffInternshipPlurality { get; set; }
        public string CrewOffPosition { get; set; }

        [Display(Name = "Ngày lên tàu")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DateOff { get; set; }

        public string TimeOff { get; set; }
        public Nullable<int> CrewOffHistoryID { get; set; }
        public Nullable<int> CrewOnID { get; set; }
        public Nullable<int> OnPositionID { get; set; }
        public Nullable<bool> OnInternshipPosition { get; set; }
        public Nullable<int> OnPluralityID { get; set; }
        public Nullable<bool> OnInternshipPlurality { get; set; }
        public string CrewOnPosition { get; set; }
        public Nullable<System.DateTime> DateOn { get; set; }
        public string TimeOn { get; set; }
        public Nullable<int> CrewOnHistoryID { get; set; }
        public Nullable<int> EducationID { get; set; }
        public string CrewOnHistory { get; set; }
        public string Note { get; set; }
        public string HoTen_CrewOff { get; set; }
        public string HoTen_CrewOn { get; set; }
        public string DepartmentName { get; set; }
    }
    public class tbl_khoadaotaoMetadata
    {
        public int id_khoadaotao { get; set; }
        public Nullable<int> id_cosodaotao { get; set; }
        [Display(Name = "Ngày bắt đầu")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ngaybatdau { get; set; }

        [Display(Name = "Ngày kết thúc")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> NgayKetThuc { get; set; }

        [Display(Name ="Học phí")]
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode =false)]
        [RegularExpression("([0-9]+)")]
        [Required(ErrorMessage = "Chỉ được nhập số")]
        public Nullable<decimal> hocphi { get; set; }

        [Display(Name ="Địa điểm")]
        public string diadiem { get; set; }

        [Display(Name ="Tên lớp")]
        public string tenkhoadaotao { get; set; }

        [Display(Name = "Môn học")]
        public string MonHoc { get; set; }

        [Display(Name = "Cấp độ")]
        public string CapDo { get; set; }

        [Display(Name = "Giấy chứng nhận")]
        public string GiayChungNhan { get; set; }

        [Display(Name = "Theo yêu cầu")]
        public string TheoYeuCau { get; set; }

        [Display(Name = "Khóa")]
        public string KhoaDaoTao { get; set; }

    }
    public class tbl_ctdaotaoMetadata
    {
        public int id_ctdaotao { get; set; }

        [Display(Name ="Tên lớp")]
        public Nullable<int> id_khoadaotao { get; set; }

        [Display(Name ="Tên nhân viên")]
        public Nullable<int> EmployeeID { get; set; }

        [Display(Name ="Kết quả")]
        public string ketqua { get; set; }

        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }
    }
    public class tbl_cosodaotaoMetadata
    {
        [Display(Name = "Tên cơ sở")]
        public string tencoso { get; set; }
    }
    public class sp_T_LayDanhSachHocTheoThoiGian_ResultMetadata
    {

        public int EmployeeID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EmployeeCode { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> BirthDay { get; set; }

        public string ChucDanh { get; set; }

        [Display(Name = "Theo yêu cầu")]
        public string TheoYeuCau { get; set; }

        public string ketqua { get; set; }
        public string tenkhoadaotao { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> ngaybatdau { get; set; }

        public string diadiem { get; set; }

        [Display(Name = "Học phí")]
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public Nullable<decimal> hocphi { get; set; }

        public string tencoso { get; set; }
    }
    public class sp_T_ThongKeDaoTaoCaNhan_ResultMetadata
    {
        public int EmployeeID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EmployeeCode { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> BirthDay { get; set; }

        public string ChucDanh { get; set; }

        [Display(Name = "Theo yêu cầu")]
        public String TheoYeuCau { get; set; }

        public string ketqua { get; set; }
        public string tenkhoadaotao { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> ngaybatdau { get; set; }

        public string diadiem { get; set; }

        [Display(Name = "Học phí")]
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public Nullable<decimal> hocphi { get; set; }

        public string tencoso { get; set; }
    }
    public class sp_T_LayDanhSachNhanVien_ResultMetadata
    {
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> BirthDay { get; set; }
        public string BirthPlace { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<int> PositionID { get; set; }
        public string EducationName { get; set; }
        public string PositionName { get; set; }
        public string ChucVu { get; set; }
        public string ContactAddress { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> KhuVucID { get; set; }
        public string TenKhuVuc { get; set; }
        public string Note { get; set; }
        public Nullable<int> QuanHeID { get; set; }
        public string LoaiQuanHe { get; set; }
        public string TenMoiQuanHe { get; set; }
    }
    public class tbl_QuanHeMetadata
    {
        public int QuanHeID { get; set; }
        [Display(Name = "Tên mối quan hệ")]
        [Required]
        public string HoTen { get; set; }
    }
    public class DIC_POSITIONMetadata
    {
        public int PositionID { get; set; }
        [Display(Name = "Tên chức danh")]
        [Required]
        public string PositionName { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        public Nullable<int> GroupPositionID { get; set; }      
    }
    public class DIC_SCHOOLMetadata
    {
        public int SchoolID { get; set; }
        [Display(Name = "Tên trường")]
        [Required]
        public string SchoolName { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }        
    }
    public class DIC_CONTRACTTYPEMetadata
    {
        public int ContractTypeID { get; set; }

        [Display(Name ="Loại hợp đồng")]
        [Required]
        public string ContractTypeName { get; set; }
    }
    public class DIC_NATIONALITYMetadata
    {
       
        public int NationalityID { get; set; }

        [Display(Name = "Quốc tịch")]
        [Required]
        public string NationalityName { get; set; }

        [Display(Name ="Mô tả")]
        public string Description { get; set; }

    }
    public class DIC_RELATIVEMetadata
    {
        public int RelativeID { get; set; }

        [Display(Name ="Quan hệ")]
        [Required]
        public string RelativeName { get; set; }

        [Display(Name ="Mô tả")]
        public string Description { get; set; }

    }
    public class DIC_RELIGIONMetadata
    {
        public int ReligionID { get; set; }

        [Display(Name ="Tôn giáo")]
        [Required]
        public string ReligionName { get; set; }

        [Display(Name ="Mô tả")]
        public string Description { get; set; }
    }
    public class DIC_EDUCATIONMetadata
    {
       
        public int EducationID { get; set; }

        [Display(Name ="Trình độ")]
        [Required]
        public string EducationName { get; set; }

        [Display(Name ="Mô tả")]
        public string Description { get; set; }

    }
    public class sp_LayDSKhongDatPassport_ResultMetadata
    {
        public int EmployeeID { get; set; }
        [Display(Name = "Họ")]
        public string FirstName { get; set; }

        [Display(Name = "Tên")]
        public string LastName { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> BirthDay { get; set; }

        [Display(Name ="Nơi sinh")]
        public string BirthPlace { get; set; }

        [Display(Name ="Chức vụ")]
        public string ChucVu { get; set; }

        [Display(Name ="Tàu")]
        public string DepartmentName { get; set; }

        [Display(Name ="Ngày xuống tàu")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> NgayXuongTau { get; set; }

        public Nullable<int> DegreeID { get; set; }
        public string DegreeNo { get; set; }
        public string DegreeName { get; set; }
        public string DegreeDate { get; set; }

        [Display(Name = "Ngày hết hạn")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string ExpirationDate { get; set; }

        public Nullable<bool> TH { get; set; }
    }

    public class LoaiKiemTraMetadata
    {
        public int LoaiKiemTraID { get; set; }

        [Display(Name = "Loại kiểm tra")]
        [Required(ErrorMessage = "Phải nhập tên")]
        public string TenLoaiKiemTra { get; set; }

        [Display(Name = "Viết tắt")]
        [Required(ErrorMessage = "Phải nhập chữ tắt")]
        public string VietTat { get; set; }
    }

    public class LichKiemTraMetadata
    {
        public int LichKiemTraID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public Nullable<int> LoaiKiemTraID { get; set; }

        [Display(Name = "Tháng")]
        [Range(1, 12,ErrorMessage ="Chỉ được nhập từ 1-12")]
        [Required(ErrorMessage = "Phải nhập tháng")]
        public Nullable<int> Thang { get; set; }

        [Display(Name = "Năm")]
        [Range(2018, int.MaxValue, ErrorMessage ="Năm nhập từ 2018 trở đi")]
        [Required(ErrorMessage = "Phải nhập năm")]
        public Nullable<int> Nam { get; set; }

        [Display(Name = "Ngày")]
        [Range(1, 31, ErrorMessage ="Nhập ngày trong tháng, từ 1-31")]
        public Nullable<int> Ngay { get; set; }

    }
    public class DIC_DEPARTMENTMetadata
    {
        public int DepartmentID { get; set; }

        [Display(Name = "Tên phòng ban")]
        [Required(ErrorMessage = "Tên phòng ban bắt buộc phải nhập")]
        [StringLength(maximumLength: 50, ErrorMessage = "Chiều dài không quá 50 ký tự")]
        [Remote("IsExistsChild", "DIC_DEPARTMENT", AdditionalFields = ("ParentID"), ErrorMessage = "Đã tồn tại phòng ban trực thuộc có tên như vậy.")]
        public string DepartmentName { get; set; }

        [Display(Name = "Điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Biên chế")]
        public Nullable<int> Quantity { get; set; }

        [Display(Name = "SL thực tế")]
        public Nullable<int> FactQuantity { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        public Nullable<bool> IsLast { get; set; }
        public Nullable<int> ParentID { get; set; }

        [Display(Name = "Loại tàu")]
        public Nullable<int> TypeOfVessel { get; set; }
        [Display(Name = "Dung tích")]
        public string Gross { get; set; }
        [Display(Name = "Công suất")]
        public string Power { get; set; }
        [Display(Name = "Số IMO")]
        public string IMO { get; set; }
        [Display(Name = "Chiều dài lớn nhất")]
        public string Length { get; set; }
        [Display(Name = "Chiều rộng")]
        public string Breadth { get; set; }
        [Display(Name = "Mớn nước")]
        public string Draft { get; set; }
        [Display(Name = "Trọng tải toàn phần")]
        public string DeadWeight { get; set; }
        [Display(Name = "Dung tích thực dụng")]
        public string Net { get; set; }
        [Display(Name = "Năm đóng")]
        public Nullable<int> YearOfBuilding { get; set; }
        [Display(Name = "Nơi đóng")]
        public string PlaceOfBuiding { get; set; }
        [Display(Name = "Cảng đăng ký")]
        public string PortOfRegistry { get; set; }
        [Display(Name = "Tổ chức đăng kiểm")]
        public string ClassificationAgency { get; set; }
    }

    public class HRM_SALARYMetadata
    {
        public int HRMSalaryID { get; set; }

        [Display(Name = "Tên bảng lương")]
        public string HRMSalaryName { get; set; }

        [Display(Name = "Năm")]
        [RegularExpression("([0-9]+)")]
        [Required(ErrorMessage = "Chỉ được nhập số")]
        public Nullable<int> Years { get; set; }

        [Display(Name = "Tháng")]
        [RegularExpression("([0-9]+)")]
        [Required(ErrorMessage = "Chỉ được nhập số")]
        public Nullable<int> Months { get; set; }

        [Display(Name = "%BHXH")]
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = false)]
        [RegularExpression("([0-9]+)")]
        [Required(ErrorMessage = "Chỉ được nhập số")]
        public Nullable<double> PerBHXH { get; set; }

        [Display(Name = "%BHYT")]
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = false)]
        [RegularExpression("([0-9]+)")]
        [Required(ErrorMessage = "Chỉ được nhập số")]
        public Nullable<double> PerBHYT { get; set; }

        [Display(Name = "%BHTN")]
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = false)]
        [RegularExpression("([0-9]+)")]
        [Required(ErrorMessage = "Chỉ được nhập số")]
        public Nullable<double> PerBHTN { get; set; }

        [Display(Name = "%Công đoàn")]
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = false)]
        [RegularExpression("([0-9]+)")]
        [Required(ErrorMessage = "Chỉ được nhập số")]
        public Nullable<double> PerCongDoan { get; set; }

        [Display(Name = "Tổng ngày công")]
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = false)]
        [RegularExpression("([0-9]+)")]
        [Required(ErrorMessage = "Chỉ được nhập số")]
        public Nullable<int> TotalDay { get; set; }

        [Display(Name = "Hệ số")]
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = false)]
        [RegularExpression("([0-9]+)")]
        [Required(ErrorMessage = "Chỉ được nhập số")]
        public Nullable<int> HeSo { get; set; }
    }
    public class viewLuongThangMetadata
    {
        public int HRMSalaryID { get; set; }
        public string HRMSalaryName { get; set; }
        public Nullable<int> Years { get; set; }
        public Nullable<int> Months { get; set; }
        public Nullable<double> PerBHXH { get; set; }
        public Nullable<double> PerBHYT { get; set; }
        public Nullable<double> PerBHTN { get; set; }
        public Nullable<double> PerCongDoan { get; set; }
        public Nullable<int> TotalDay { get; set; }
        //public Nullable<int> HeSo { get; set; }
        public int EmployeeID { get; set; }
        public Nullable<int> EmploymentHistoryID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        //public Nullable<int> PositionID { get; set; }
        //public string Position { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public Nullable<double> Salary { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public Nullable<double> AllowanceSalary { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public Nullable<double> Bonus { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public Nullable<double> AllowanceBonus { get; set; }

        public Nullable<int> NumOfDay { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public Nullable<double> TruKhac { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public Nullable<int> PhuThuoc { get; set; }
        public string GhiChu { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BankCode { get; set; }
        //public string BankName { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public Nullable<int> TongThuNhap { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public Nullable<double> BHXH { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public Nullable<double> BHYT { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public Nullable<double> CongDoan { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public Nullable<double> BHTN { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public Nullable<int> ThueTNCN { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public Nullable<double> ConLai { get; set; }
        public int HRMDetailSalaryID { get; set; }
    }
    public class HRM_DETAILSALARYMetadata
    {
        //public int HRMDetailSalaryID { get; set; }
        //public int HRMSalaryID { get; set; }
        //public int EmployeeID { get; set; }
        //public Nullable<int> EmploymentHistoryID { get; set; }
        //public Nullable<int> DepartmentID { get; set; }
        //public Nullable<int> PositionID { get; set; }
        //public string Position { get; set; }

        //[Display(Name = "LươngCB")]
        //[DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Chỉ được nhập số")]
        //public Nullable<double> Salary { get; set; }

        //[Display(Name = "Lương PC")]
        //[DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Chỉ được nhập số")]
        //public Nullable<double> AllowanceSalary { get; set; }

        //[Display(Name = "Lương thỏa thuận")]
        //[DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Chỉ được nhập số")]
        //public Nullable<double> Bonus { get; set; }

        //[Display(Name = "PC thỏa thuận")]
        //[DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Chỉ được nhập số")]
        //public Nullable<double> AllowanceBonus { get; set; }

        //[Display(Name = "Ngày làm việc")]
        //[DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Chỉ được nhập số")]
        //public Nullable<int> NumOfDay { get; set; }

        //[Display(Name = "Trừ khác")]
        //[DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Chỉ được nhập số")]
        //public Nullable<double> TruKhac { get; set; }

        //[Display(Name = "Số người phụ thuộc")]
        //[DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Chỉ được nhập số")]
        //public Nullable<int> PhuThuoc { get; set; }

        //[Display(Name = "Ghi chú")]
        //public string GhiChu { get; set; }
    }

    public class sp_Luong_4_ResultMetadata
    {
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; } 
                

        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public Nullable<float> Salary { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public Nullable<float> AllowanceSalary { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public Nullable<float> Bonus { get; set; }

        public int NumOfDay { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public Nullable<float> TongThuNhap { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public Nullable<float> BHXH { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public Nullable<float> BHYT { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public Nullable<float> CongDoan { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public Nullable<float> BHTN { get; set; }

        public int PhuThuoc { get; set; }
        public float ThueTNCN { get; set; }
        public int TruKhac { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public Nullable<float> ConLai { get; set; }
    }
}
