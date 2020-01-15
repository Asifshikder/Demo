using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.ViewModels;
using static Project.JSON_Antiforgery_Token_Validation;

namespace Project.Models
{
    [Authorize]
    [AjaxAuthorize]
    public class CallCategoryController : Controller
    {
        private DataContext db = new DataContext();

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_CallCategory)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllCallCategoryAJAXData()
        {
            JsonResult result = new JsonResult();
            try
            {
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                var CallCategory = db.CallCategory.Where(x => x.Status == AppUtils.TableStatusIsActive).AsEnumerable();
                int ifSearch = 0;
                List<CallCategoryViewModel> data =
                    CallCategory.Any() ? CallCategory.Skip(startRec).Take(pageSize).AsEnumerable()
                        .Select(
                            s => new CallCategoryViewModel
                            {
                                CallCategoryID = s.CallCategoryID,
                                CallCategoryName = s.CallCategoryName,
                                CallCategoryUpdate = Project.AppUtils.HasAccessInTheList(AppUtils.Update_CallCategory) ? true : false
                            })
                        .ToList() : new List<CallCategoryViewModel>();
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (data.Any()) ? data.Where(p => p.CallCategoryID.ToString().ToLower().Contains(search.ToLower()) || p.CallCategoryName.ToString().ToLower().Contains(search.ToLower())).Count() : 0;


                    data = data.Where(p => p.CallCategoryID.ToString().ToLower().Contains(search.ToLower()) || p.CallCategoryName.ToString().ToLower().Contains(search.ToLower())
                    ).ToList();
                }

                data = this.SortByColumnWithOrder(order, orderDir, data);

                int totalRecords = CallCategory.AsEnumerable().Count();
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : CallCategory.AsEnumerable().Count();

                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return result;
        }


        private List<CallCategoryViewModel> SortByColumnWithOrder(string order, string orderDir, List<CallCategoryViewModel> data)
        {
            List<CallCategoryViewModel> lst = new List<CallCategoryViewModel>();
            try
            {
                switch (order)
                {

                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CallCategoryID).ToList() : data.OrderBy(p => p.CallCategoryID).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CallCategoryName).ToList() : data.OrderBy(p => p.CallCategoryName).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CallCategoryID).ToList() : data.OrderBy(p => p.CallCategoryID).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return lst;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult InsertCallCategoryFromPopUp(CallCategory CallCategory)
        {
            try
            {
                CallCategory.CreateBy = AppUtils.GetLoginUserID();
                CallCategory.CreateDate = AppUtils.GetDateTimeNow();
                CallCategory.Status = AppUtils.TableStatusIsActive;
                db.CallCategory.Add(CallCategory);
                db.SaveChanges();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCallCategoryDetailsByID(int CallCategoryID)
        {
            var CallCategoryInfo = db.CallCategory.Where(s => s.CallCategoryID == CallCategoryID).Select(s => new { CallCategoryID = s.CallCategoryID, CallCategoryName = s.CallCategoryName}).FirstOrDefault();
             
            var JSON = Json(new { CallCategoryInfo = CallCategoryInfo }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateCallCategory(CallCategory CallCategory)
        {
            try
            {
                CallCategory dbCallCategory = new CallCategory();
                dbCallCategory = db.CallCategory.Find(CallCategory.CallCategoryID);
                dbCallCategory.CallCategoryName = CallCategory.CallCategoryName;
                dbCallCategory.UpdateBy = AppUtils.GetLoginUserID();
                dbCallCategory.UpdateDate = AppUtils.GetDateTimeNow();
                db.Entry(dbCallCategory).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                var JSON = Json(new { success = true }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);

            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCallCategory(int CallCategoryID)
        {
            CallCategory callCategory = new CallCategory();
            callCategory = db.CallCategory.Find(CallCategoryID);
            callCategory.DeleteBy = AppUtils.GetLoginUserID();
            callCategory.DeleteDate = AppUtils.GetDateTimeNow();
            callCategory.Status = AppUtils.TableStatusIsDelete;


            db.Entry(callCategory).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var JSON = Json(new { success = true }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

    }
}