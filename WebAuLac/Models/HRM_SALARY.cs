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
    
    public partial class HRM_SALARY
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
        public Nullable<int> HeSo { get; set; }
    }
}