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
    
    public partial class DIC_SALARY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DIC_SALARY()
        {
            this.DIC_SALARY_STEP = new HashSet<DIC_SALARY_STEP>();
            this.HRM_EMPLOYMENTHISTORY = new HashSet<HRM_EMPLOYMENTHISTORY>();
            this.HRM_EMPLOYMENTHISTORY1 = new HashSet<HRM_EMPLOYMENTHISTORY>();
            this.HRM_QTCT = new HashSet<HRM_QTCT>();
            this.HRM_QTCT1 = new HashSet<HRM_QTCT>();
        }
    
        public int SalaryID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public Nullable<int> PositionID { get; set; }
        public Nullable<int> RankID { get; set; }
        public Nullable<double> Salary { get; set; }
        public Nullable<double> AllowanceSalary { get; set; }
        public Nullable<double> Bonus { get; set; }
        public Nullable<double> AllowanceBonus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DIC_SALARY_STEP> DIC_SALARY_STEP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRM_EMPLOYMENTHISTORY> HRM_EMPLOYMENTHISTORY { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRM_EMPLOYMENTHISTORY> HRM_EMPLOYMENTHISTORY1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRM_QTCT> HRM_QTCT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRM_QTCT> HRM_QTCT1 { get; set; }
    }
}
