using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ViewModels
{
    public class CompanyViewModel
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string CountryName { get; set; }
        public string Details { get; set; }
        public string CompanyLogo { get; set; }
        public string Tag { get; set; }
        public bool UpdateCompany { get; set; }
    }
}