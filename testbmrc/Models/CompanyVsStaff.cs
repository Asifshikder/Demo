using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class CompanyVsStaff
    {
        [Key]
        public int CompanyVsStaffID { get; set; }
        public int CompanyID { get; set; }
        public virtual Company Company { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CompanyStaffImage { get; set; }
        public byte[] CompanyStaffImageByte { get; set; }
        public int? RoleID { get; set; }
        public virtual Role Role { get; set; }
        public int? UserRightPermissionID { get; set; }
        public virtual UserRightPermission UserRightPermission { get; set; }
        public int Status { get; set; }
        public bool IsItSuperAdmin { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
