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
    
    public partial class DIC_GROUPPOSITION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DIC_GROUPPOSITION()
        {
            this.DIC_POSITION = new HashSet<DIC_POSITION>();
        }
    
        public int GroupPositionID { get; set; }
        public string GroupPositionName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DIC_POSITION> DIC_POSITION { get; set; }
    }
}
