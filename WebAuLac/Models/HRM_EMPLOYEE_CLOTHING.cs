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
    
    public partial class HRM_EMPLOYEE_CLOTHING
    {
        public int EmployeeClothingID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> ClothingID { get; set; }
        public string Size { get; set; }
    
        public virtual DIC_CLOTHING DIC_CLOTHING { get; set; }
        public virtual HRM_EMPLOYEE HRM_EMPLOYEE { get; set; }
    }
}