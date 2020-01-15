using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project;
using Project.Models;
using testbmrc.ViewModels;
using static Project.JSON_Antiforgery_Token_Validation;

namespace testbmrc.Controllers
{
    //[Authorize]
    //[AjaxAuthorize]
    public class CountryController : Controller
    {
        private DataContext db = new DataContext();

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Country)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllCountryAJAXData()
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
                var Country = db.Country.Where(x => x.Status == AppUtils.TableStatusIsActive).AsEnumerable();
                int ifSearch = 0;
                List<CountryViewModel> data =
                    Country.Any() ? Country.Skip(startRec).Take(pageSize).AsEnumerable()
                        .Select(
                            s => new CountryViewModel
                            {
                                CountryID = s.CountryID,
                                CountryName = s.CountryName,
                                UpdateCountry = Project.AppUtils.HasAccessInTheList(AppUtils.Update_Country) ? true : false,
                            })
                        .ToList() : new List<CountryViewModel>();
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (data.Any()) ? data.Where(p => p.CountryID.ToString().ToLower().Contains(search.ToLower()) || p.CountryName.ToString().ToLower().Contains(search.ToLower())).Count() : 0;


                    data = data.Where(p => p.CountryID.ToString().ToLower().Contains(search.ToLower()) || p.CountryName.ToString().ToLower().Contains(search.ToLower())
                    ).ToList();
                }

                data = this.SortByColumnWithOrder(order, orderDir, data);

                int totalRecords = Country.AsEnumerable().Count();
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : Country.AsEnumerable().Count();

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

        private List<CountryViewModel> SortByColumnWithOrder(string order, string orderDir, List<CountryViewModel> data)
        {
            List<CountryViewModel> lst = new List<CountryViewModel>();
            try
            {
                switch (order)
                {

                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CountryID).ToList() : data.OrderBy(p => p.CountryID).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CountryName).ToList() : data.OrderBy(p => p.CountryName).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CountryID).ToList() : data.OrderBy(p => p.CountryID).ToList();
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
        public ActionResult InsertCountryFromPopUp(Country Country)
        {
            try
            {
                Country.CreateBy = AppUtils.GetLoginUserID();
                Country.CreateDate = AppUtils.GetDateTimeNow();
                Country.Status = AppUtils.TableStatusIsActive;
                db.Country.Add(Country);
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
        public ActionResult GetCountryDetailsByID(int CountryID)
        {
            var CountryInfo = db.Country.Where(s => s.CountryID == CountryID).Select(s => new { CountryID = s.CountryID, CountryName = s.CountryName }).FirstOrDefault();

            var JSON = Json(new { CountryInfo = CountryInfo }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateCountry(Country Country)
        {
            try
            {
                Country dbCountry = new Country();
                dbCountry = db.Country.Find(Country.CountryID);
                dbCountry.CountryName = Country.CountryName;
                dbCountry.UpdateBy = AppUtils.GetLoginUserID();
                dbCountry.UpdateDate = AppUtils.GetDateTimeNow();
                db.Entry(dbCountry).State = System.Data.Entity.EntityState.Modified;
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
        public ActionResult DeleteCountry(int CountryID)
        {
            Country Country = new Country();
            Country = db.Country.Find(CountryID);
            Country.DeleteBy = AppUtils.GetLoginUserID();
            Country.DeleteDate = AppUtils.GetDateTimeNow();
            Country.Status = AppUtils.TableStatusIsDelete;


            db.Entry(Country).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var JSON = Json(new { success = true }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

    }
}