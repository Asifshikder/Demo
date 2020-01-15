using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;
namespace Project.Models
{
    public class UserRightPermission
    {
        [Key]
        public int UserRightPermissionID { get; set; }
        public string UserRightPermissionName { get; set; }
        public string UserRightPermissionDescription { get; set; }
        public string UserRightPermissionDetails { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}