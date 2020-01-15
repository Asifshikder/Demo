
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Project
{
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {

        private DataContext db = new DataContext();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {


            //IEnumerable<Transaction> lstTransaction = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).AsEnumerable();

            //System.Web.HttpContext.Current.Session["AllClientInThisMonth"] = db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (Key, g) => g.OrderByDescending(e => e.LineStatusChangeDate).FirstOrDefault()).Count();
            //System.Web.HttpContext.Current.Session["ActiveClientInThisMonth"] = lstTransaction.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.AmountCountDate).FirstOrDefault()).Where(s => s.LineStatusID == AppUtils.LineIsActive).Count();
            //System.Web.HttpContext.Current.Session["LockClientInThisMonth"] = (int)System.Web.HttpContext.Current.Session["AllClientInThisMonth"] - (int)System.Web.HttpContext.Current.Session["ActiveClientInThisMonth"];//db.ClientLineStatus.ToList().GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(e => e.LineStatusChangeDate).FirstOrDefault()).Where(s => s.LineStatusID == AppUtils.LineIsLock).Count(); ;

            

            ClaimsIdentity claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;


            HttpContext ctx = HttpContext.Current;

            if (HttpContext.Current.Session["role_id"] == null || claimsIdentity.IsAuthenticated != true)
            {
                filterContext.Result = new RedirectResult("~/Account/LoginByClient");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}