using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testbmrc.ViewModels
{
    public class CompanyVsStaffViewModel
    {
        public int CompanyVsStaffID { get; set; }
        public int CompanyID { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CompanyStaffImage { get; set; }
        public int RoleID { get; set; }
        public int UserRightPermissionID { get; set; }
        public int Status { get; set; }
        public bool IsItSuperAdmin { get; set; }
        public bool UpdateCompanyVsStaff { get; set; }
    }
}