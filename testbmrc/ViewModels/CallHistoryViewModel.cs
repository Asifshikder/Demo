using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testbmrc.ViewModels
{
    public class CallHistoryViewModel
    {
        public int CallHistoryID { get; set; }
        public string CallerName { get; set; }
        public string CallerPhone { get; set; }
        public string CallerEmail { get; set; }
        public string CallerAddress { get; set; }
        public string CountryID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string CompanyID { get; set; }
        public string CompanyVsStaffID { get; set; }
        public DateTime CallTime { get; set; }
        public string Description { get; set; }
        public int CallCategoryID { get; set; }
        public string Subject { get; set; }
        public bool UpdateCallHistory { get; set; }
       
    }
}