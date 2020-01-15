using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class CallHistory
    {
        [Key]
        public int CallHistoryID { get; set; }
        public string CallerName { get; set; }
        public string CallerPhone { get; set; }
        public string CallerEmail { get; set; }
        public string CallerAddress { get; set; }
        public int CountryID { get; set; }
        //public virtual Country Country { get; set; }
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
        public int CompanyID { get; set; }
        //public virtual Company Company { get; set; }
        public int CompanyVsStaffID { get; set; }
        public virtual CompanyVsStaff CompanyVsStaff { get; set; }
        public DateTime CallTime { get; set; }
        public string Description { get; set; }
        public int CallCategoryID { get; set; }
        public virtual CallCategory CallCategory { get; set; }
        public string Subject { get; set; }
        public int Status { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
