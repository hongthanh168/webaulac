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
    
    public partial class HRM_PLAN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HRM_PLAN()
        {
            this.HRM_DETAILPLAN = new HashSet<HRM_DETAILPLAN>();
        }
    
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public Nullable<System.DateTime> PlanDate { get; set; }
        public string Note { get; set; }
        public Nullable<bool> IsLock { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRM_DETAILPLAN> HRM_DETAILPLAN { get; set; }
    }
}
