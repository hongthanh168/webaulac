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
    
    public partial class DIC_CONTRACTTYPE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DIC_CONTRACTTYPE()
        {
            this.HRM_CONTRACTHISTORY = new HashSet<HRM_CONTRACTHISTORY>();
        }
    
        public int ContractTypeID { get; set; }
        public string ContractTypeName { get; set; }
        public Nullable<int> GroupContractTypeID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRM_CONTRACTHISTORY> HRM_CONTRACTHISTORY { get; set; }
    }
}
