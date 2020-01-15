using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base("VirtualEnterpiseConString")
        {
        }

        public DbSet<AccessList> AccessList { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<CallCategory> CallCategory { get; set; }
        public DbSet<CallHistory> CallHistory { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<CompanyVsStaff> CompanyVsStaff { get; set; }
        
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRightPermission> UserRightPermission { get; set; }
        public DbSet<School> School { get; set; }
    }
}