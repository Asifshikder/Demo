using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class AccessList
    {
        [Key]
        public int AccessListID { get; set; }
        public string AccessName { get; set; }
        public int AccessValue { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int Status { get; set; }
        public bool IsGranted { get; set; }
    }
}