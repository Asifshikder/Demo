using Project;
using Project.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Project
{
    internal class Http403Result : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            // Set the response code to 403.
            context.HttpContext.Response.StatusCode = 403;
        }
    }
    public class UserRIghtCheck : ActionFilterAttribute
    {
        private DataContext db = new DataContext();

        public string ControllerValue;
        //public UserRIghtCheck(string s)
        //{
        //    ControllerValue = s;
        //}


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {


            if (HttpContext.Current.Session["LoginEmpName"] == "ReallyUnknownPerson")
            {
                return;
            }
            else
            {
                var loginID = AppUtils.GetLoginUserID();
                if (AppUtils.GetLoginRoleID() != AppUtils.AdminRole || AppUtils.GetLoginRoleID() != AppUtils.SuperAdminRole || AppUtils.GetLoginRoleID() != AppUtils.SuperTalentUserRole)
                {
                    HttpContext.Current.Session["CurrentUserRightPermission"] = db.Employee.Where(s => s.EmployeeID == loginID/*AppUtils.LoginUserID*/).Select(s => s.UserRightPermissionID).FirstOrDefault().Value;
                }
                else
                {
                    HttpContext.Current.Session["CurrentUserRightPermission"] = db.CompanyVsStaff.Where(s => s.CompanyVsStaffID == loginID/*AppUtils.LoginUserID*/).Select(s => s.UserRightPermissionID).FirstOrDefault().Value;
                }
                int CurrentUserRightPermission = (int)HttpContext.Current.Session["CurrentUserRightPermission"];

                UserRightPermission userRightPermission = db.UserRightPermission.Where(s => s.UserRightPermissionID == CurrentUserRightPermission).FirstOrDefault();
                if (!string.IsNullOrEmpty(userRightPermission.UserRightPermissionDetails))
                {

                    List<string> lstAcessList = db.UserRightPermission.Where(s => s.UserRightPermissionID == CurrentUserRightPermission).Select(s => s.UserRightPermissionDetails).FirstOrDefault().Split(',').ToList();
                    HttpContext.Current.Session["lstAccessList"] = lstAcessList.Count() > 0 ? lstAcessList.ToList() : new List<string>(); 

                    AppUtils.GetTempNotUpdateEmployee = ConfigurationManager.AppSettings["EmployeeList"].Split(',').ToList();

                    ClaimsIdentity claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;

                    if (lstAcessList.Count() < 1)
                    {
                        filterContext.Result = new Http403Result();
                        //filterContext.Result = new RedirectResult("~/Account/LoginByClient");
                        return;
                    }
                    else
                    {

                        if (!AppUtils.HasAccessInTheList(ControllerValue))
                        {
                            filterContext.Result = new Http403Result();

                            //throw new UnauthorizedAccessException();
                            //   throw new HttpException((int)System.Net.HttpStatusCode.Forbidden, "Forbidden");


                            //return Content(HttpStatusCode.Forbidden, "RFID is disabled for this site.");
                            //HttpContext.Current.Session["role_id"] = null;
                            //claimsIdentity = null;
                            //    filterContext.Result = new RedirectResult("~/Account/LoginByClient");
                            //    return;
                        }
                    }
                }

                else
                {
                    filterContext.Result = new Http403Result();
                }
            }


        }
    }
}