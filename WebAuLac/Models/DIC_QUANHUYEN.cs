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
    
    public partial class DIC_QUANHUYEN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DIC_QUANHUYEN()
        {
            this.DIC_PHUONGXA = new HashSet<DIC_PHUONGXA>();
        }
    
        public int QuanHuyenID { get; set; }
        public string QuanHuyenName { get; set; }
        public string QuanHuyenCode { get; set; }
        public Nullable<int> TinhThanhPhoID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DIC_PHUONGXA> DIC_PHUONGXA { get; set; }
        public virtual DIC_TINHTHANHPHO DIC_TINHTHANHPHO { get; set; }
    }
}