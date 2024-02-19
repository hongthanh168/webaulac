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
    
    public partial class HRM_DETAILPLAN
    {
        public int DetailPlanID { get; set; }
        public Nullable<int> PlanID { get; set; }
        public Nullable<int> CrewOffID { get; set; }
        public Nullable<int> CrewOffDepartmentID { get; set; }
        public Nullable<int> OffPositionID { get; set; }
        public Nullable<int> OffPluralityID { get; set; }
        public string CrewOffPosition { get; set; }
        public Nullable<System.DateTime> DateOff { get; set; }
        public string TimeOff { get; set; }
        public Nullable<int> CrewOffHistoryID { get; set; }
        public Nullable<int> CrewOnID { get; set; }
        public Nullable<int> OnPositionID { get; set; }
        public Nullable<int> OnPluralityID { get; set; }
        public string CrewOnPosition { get; set; }
        public Nullable<System.DateTime> DateOn { get; set; }
        public string TimeOn { get; set; }
        public Nullable<int> CrewOnHistoryID { get; set; }
        public Nullable<int> EducationID { get; set; }
        public string CrewOnHistory { get; set; }
        public string Note { get; set; }
        public Nullable<bool> DaDuyet { get; set; }
        public Nullable<int> OffInternshipPosition { get; set; }
        public Nullable<int> OffInternshipPlurality { get; set; }
        public Nullable<int> OnInternshipPosition { get; set; }
        public Nullable<int> OnInternshipPlurality { get; set; }
    
        public virtual DIC_DEPARTMENT DIC_DEPARTMENT { get; set; }
        public virtual DIC_EDUCATION DIC_EDUCATION { get; set; }
        public virtual HRM_PLAN HRM_PLAN { get; set; }
    }
}