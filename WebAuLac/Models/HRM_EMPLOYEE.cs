//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAuLac.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HRM_EMPLOYEE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HRM_EMPLOYEE()
        {
            this.HRM_ATTLOG = new HashSet<HRM_ATTLOG>();
            this.HRM_CLOTHING_DETAILS = new HashSet<HRM_CLOTHING_DETAILS>();
            this.HRM_CONTRACTHISTORY = new HashSet<HRM_CONTRACTHISTORY>();
            this.HRM_EMPLOYEE_ACCIDENT = new HashSet<HRM_EMPLOYEE_ACCIDENT>();
            this.HRM_EMPLOYEE_CLOTHING = new HashSet<HRM_EMPLOYEE_CLOTHING>();
            this.HRM_EMPLOYEE_DEGREE = new HashSet<HRM_EMPLOYEE_DEGREE>();
            this.HRM_EMPLOYEE_DISCIPLINE = new HashSet<HRM_EMPLOYEE_DISCIPLINE>();
            this.HRM_EMPLOYEE_RELATIVE = new HashSet<HRM_EMPLOYEE_RELATIVE>();
            this.HRM_EMPLOYEE_RETRIBUTION = new HashSet<HRM_EMPLOYEE_RETRIBUTION>();
            this.HRM_EMPLOYMENTHISTORY = new HashSet<HRM_EMPLOYMENTHISTORY>();
            this.HRM_QTCT = new HashSet<HRM_QTCT>();
            this.HRM_TIMEKEEPER = new HashSet<HRM_TIMEKEEPER>();
            this.tbl_ctdaotao = new HashSet<tbl_ctdaotao>();
        }
    
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string CardNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Alias { get; set; }
        public Nullable<bool> Sex { get; set; }
        public Nullable<System.DateTime> BirthDay { get; set; }
        public string BirthPlace { get; set; }
        public string MainAddress { get; set; }
        public string ContactAddress { get; set; }
        public string CellPhone { get; set; }
        public string HomePhone { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
        public string Yahoo { get; set; }
        public string Facebook { get; set; }
        public string IDCard { get; set; }
        public Nullable<System.DateTime> IDCardDate { get; set; }
        public string IDCardPlace { get; set; }
        public string TaxNo { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string InsuranceCode { get; set; }
        public Nullable<System.DateTime> InsuranceDate { get; set; }
        public byte[] Photo { get; set; }
        public Nullable<int> EducationID { get; set; }
        public Nullable<int> DegreeID { get; set; }
        public Nullable<int> EthnicID { get; set; }
        public Nullable<int> ReligionID { get; set; }
        public Nullable<int> NationalityID { get; set; }
        public Nullable<int> Department_PositionID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string Origin { get; set; }
        public Nullable<int> SchoolID { get; set; }
        public string Qualification { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string BloodType { get; set; }
        public Nullable<bool> MarriageStatus { get; set; }
        public Nullable<int> KhuVucID { get; set; }
        public string Note { get; set; }
        public Nullable<int> QuanHeID { get; set; }
        public string LoaiQuanHe { get; set; }
        public string DiaChiTiengAnh { get; set; }
        public Nullable<bool> SSDD { get; set; }
        public string GhiChuSSDD { get; set; }
        public Nullable<int> SoNguoiPhuThuoc { get; set; }
        public Nullable<int> HeDaoTaoID { get; set; }
        public Nullable<int> TrinhDoAnhVanID { get; set; }
        public Nullable<int> TrinhDoViTinhID { get; set; }
        public string ThoiGianTotNghiep { get; set; }
        public string SDTNguoiThan { get; set; }
        public Nullable<int> MainAddress_Xa { get; set; }
        public Nullable<int> MainAddress_Huyen { get; set; }
        public Nullable<int> MainAddress_Tinh { get; set; }
        public Nullable<int> ContactAddress_Xa { get; set; }
        public Nullable<int> ContactAddress_Huyen { get; set; }
        public Nullable<int> ContactAddress_Tinh { get; set; }
        public Nullable<int> Origin_Xa { get; set; }
        public Nullable<int> Origin_Huyen { get; set; }
        public Nullable<int> Origin_Tinh { get; set; }
        public Nullable<System.DateTime> NgaySSDD { get; set; }
    
        public virtual DIC_DEPARTMENT_POSITION DIC_DEPARTMENT_POSITION { get; set; }
        public virtual DIC_EDUCATION DIC_EDUCATION { get; set; }
        public virtual DIC_ETHNIC DIC_ETHNIC { get; set; }
        public virtual DIC_KHUVUC DIC_KHUVUC { get; set; }
        public virtual DIC_NATIONALITY DIC_NATIONALITY { get; set; }
        public virtual DIC_RELIGION DIC_RELIGION { get; set; }
        public virtual DIC_SCHOOL DIC_SCHOOL { get; set; }
        public virtual DIC_STATUS DIC_STATUS { get; set; }
        public virtual HeDaoTao HeDaoTao { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRM_ATTLOG> HRM_ATTLOG { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRM_CLOTHING_DETAILS> HRM_CLOTHING_DETAILS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRM_CONTRACTHISTORY> HRM_CONTRACTHISTORY { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRM_EMPLOYEE_ACCIDENT> HRM_EMPLOYEE_ACCIDENT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRM_EMPLOYEE_CLOTHING> HRM_EMPLOYEE_CLOTHING { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRM_EMPLOYEE_DEGREE> HRM_EMPLOYEE_DEGREE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRM_EMPLOYEE_DISCIPLINE> HRM_EMPLOYEE_DISCIPLINE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRM_EMPLOYEE_RELATIVE> HRM_EMPLOYEE_RELATIVE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRM_EMPLOYEE_RETRIBUTION> HRM_EMPLOYEE_RETRIBUTION { get; set; }
        public virtual tbl_QuanHe tbl_QuanHe { get; set; }
        public virtual TrinhDoAnhVan TrinhDoAnhVan { get; set; }
        public virtual TrinhDoViTinh TrinhDoViTinh { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRM_EMPLOYMENTHISTORY> HRM_EMPLOYMENTHISTORY { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRM_QTCT> HRM_QTCT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRM_TIMEKEEPER> HRM_TIMEKEEPER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ctdaotao> tbl_ctdaotao { get; set; }
    }
}