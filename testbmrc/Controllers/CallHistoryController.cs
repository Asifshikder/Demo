using Project;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testbmrc.ViewModels;
using static Project.JSON_Antiforgery_Token_Validation;

namespace testbmrc.Controllers
{
    [Authorize]
    [AjaxAuthorize]
    public class CallHistoryController : Controller
    {
        private DataContext db = new DataContext();

        [HttpGet]
       [UserRIghtCheck(ControllerValue = AppUtils.View_CallHistory)]
        public ActionResult CallHistoryDetails()
        {
            ViewBag.Country = new SelectList(db.Country.Where(s => s.Status == AppUtils.TableStatusIsActive).Select(s => new { CountryID = s.CountryID, CountryName = s.CountryName }).ToList(), "CountryID", "CountryName");
            ViewBag.Company = new SelectList(db.Company.Where(s => s.Status == AppUtils.TableStatusIsActive).Select(s => new { CompanyID = s.CompanyID, CompanyName = s.CompanyName }).ToList(), "CompanyID", "CompanyName");
            ViewBag.CallCategory = new SelectList(db.CallCategory.Where(s => s.Status == AppUtils.TableStatusIsActive).Select(s => new { CallCategoryID = s.CallCategoryID, CallCategoryName = s.CallCategoryName }).ToList(), "CallCategoryID", "CallCategoryName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllCallHistoryAjaxData()
        {
            JsonResult result = new JsonResult();
            try
            {
                int CompanyFromDDL = 0;
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                var CompanyIDs = Request.Form.Get("CompanyIDs");
                var StartDateID = Request.Form.Get("StartDateID");
                var EndDateID = Request.Form.Get("EndDateID");
                DateTime? startDate = new DateTime?();
                DateTime? endDate = new DateTime?();
                int totalRecords = 0;
                if (!string.IsNullOrEmpty(CompanyIDs))
                {
                    CompanyFromDDL = int.Parse(CompanyIDs);
                }
                if (!string.IsNullOrEmpty(StartDateID))
                {
                    startDate = Convert.ToDateTime(StartDateID);
                }
                if (!string.IsNullOrEmpty(EndDateID))
                {
                    endDate = Convert.ToDateTime(EndDateID);
                }


                var firstPartOfQuery = db.CallHistory.Where(a => a.Status == AppUtils.TableStatusIsActive).AsQueryable();

                int ifSearch = 0;
                List<CallHistoryViewModel> data = new List<CallHistoryViewModel>();
                firstPartOfQuery =
                     (StartDateID != "" && EndDateID != "" && !string.IsNullOrEmpty(CompanyIDs)) ? firstPartOfQuery.Where(s => s.CallTime >= startDate && s.CallTime <= endDate && s.CompanyID == CompanyFromDDL).AsQueryable()
                         : (StartDateID != "" && EndDateID != "" && string.IsNullOrEmpty(CompanyIDs)) ? firstPartOfQuery.Where(s => s.CallTime >= startDate && s.CallTime <= endDate).AsQueryable()
                             : (StartDateID != "" && EndDateID == "" && !string.IsNullOrEmpty(CompanyIDs)) ? firstPartOfQuery.Where(s => s.CallTime >= startDate && s.CompanyID == CompanyFromDDL).AsQueryable()
                                 : (StartDateID != "" && EndDateID == "" && string.IsNullOrEmpty(CompanyIDs)) ? firstPartOfQuery.Where(s => s.CallTime >= startDate).AsQueryable()
                                  : (StartDateID == "" && EndDateID == "" && !string.IsNullOrEmpty(CompanyIDs)) ? firstPartOfQuery.Where(s => s.CompanyID == CompanyFromDDL).AsQueryable()
                                         : (StartDateID == "" && EndDateID != "" && string.IsNullOrEmpty(CompanyIDs)) ? firstPartOfQuery.Where(s => s.CallTime <= endDate).AsQueryable()
                                         : firstPartOfQuery.AsQueryable();


                var secondPartOfQuery = firstPartOfQuery.AsEnumerable();
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p => p.CallerName.ToString().ToLower().Contains(search.ToLower()) ||
                                                              p.CallerPhone.ToString().ToLower().Contains(search.ToLower()) ||
                                                              p.Subject.ToString().ToLower().Contains(search.ToLower())).Count() : 0;

                    secondPartOfQuery = secondPartOfQuery.Where(p => p.CallerName.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.CallerPhone.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.Subject.ToString().ToLower().Contains(search.ToLower())).AsEnumerable();
                }
                if (secondPartOfQuery.Count() > 0)
                {
                    totalRecords = secondPartOfQuery.AsEnumerable().Count();
                    data = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(

                            s => new CallHistoryViewModel
                            {
                                CallHistoryID = s.CallHistoryID,
                                CallerName = s.CallerName,
                                Subject = s.Subject,
                                CompanyID = db.Company.Find(s.CompanyID).CompanyName,
                                CallerPhone = s.CallerPhone,
                                CountryID = db.Country.Find(s.CountryID).CountryName,
                                CompanyVsStaffID = GetStaffName(s.CompanyVsStaffID),
                                UpdateCallHistory = AppUtils.HasAccessInTheList(AppUtils.Update_CallHistory) ? true : false,
                            })
                        .ToList();

                }

                data = this.SortByColumnWithOrder(order, orderDir, data);
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : firstPartOfQuery.AsEnumerable().Count();

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

        private string GetStaffName(int companyVsStaffID)
        {
            var companyVsStaff = db.CompanyVsStaff.Find(companyVsStaffID);
            return companyVsStaff.FirstName + " " + companyVsStaff.LastName;
        }
        
        private string GetEmpName(int EmployeeID)
        {
            var employee = db.Employee.Find(EmployeeID);
            return employee.FirstName + " " + employee.LastName;
        }

        private List<CallHistoryViewModel> SortByColumnWithOrder(string order, string orderDir, List<CallHistoryViewModel> data)
        {
            List<CallHistoryViewModel> lst = new List<CallHistoryViewModel>();
            try
            {
                switch (order)
                {

                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyVsStaffID).ToList() : data.OrderBy(p => p.CompanyVsStaffID).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CallerName).ToList() : data.OrderBy(p => p.CallerName).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CallerPhone).ToList() : data.OrderBy(p => p.CallerPhone).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyVsStaffID).ToList() : data.OrderBy(p => p.CompanyVsStaffID).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyID).ToList() : data.OrderBy(p => p.CompanyID).ToList();
                        break;

                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyVsStaffID).ToList() : data.OrderBy(p => p.CompanyVsStaffID).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {

                Console.Write(ex);
            }
            return lst;
        }
        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_CallHistoryForCompanyStaff)]
        public ActionResult CallHistoryForCompanyStaff()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllCallHistoryForCompanyStaffAjaxData()
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
                var StartDateID = Request.Form.Get("StartDateID");
                var EndDateID = Request.Form.Get("EndDateID");
                DateTime? startDate = new DateTime?();
                DateTime? endDate = new DateTime?();
                int totalRecords = 0;
                if (!string.IsNullOrEmpty(StartDateID))
                {
                    startDate = Convert.ToDateTime(StartDateID);
                }
                if (!string.IsNullOrEmpty(EndDateID))
                {
                    endDate = Convert.ToDateTime(EndDateID);
                }

                var firstPartOfQuery = db.CallHistory.Where(a => a.Status == AppUtils.TableStatusIsActive && a.CompanyID == 4).AsQueryable();

                int ifSearch = 0;
                int loginUserID = AppUtils.GetLoginUserID();
                CompanyVsStaff companyVsStaff = db.CompanyVsStaff.Where(x => x.CompanyVsStaffID == loginUserID).FirstOrDefault();
                List<CallHistoryViewModel> data = new List<CallHistoryViewModel>();

                DateTime dt = AppUtils.GetDateTimeNow();

                if (!companyVsStaff.IsItSuperAdmin)
                {
                    firstPartOfQuery = firstPartOfQuery.Where(x => x.CompanyVsStaffID == companyVsStaff.CompanyVsStaffID).AsQueryable();
                    var date = dt.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);
                    firstPartOfQuery =
                         (StartDateID != "" && EndDateID != "") ? firstPartOfQuery.Where(s => s.CallTime >= startDate && s.CallTime <= endDate).AsQueryable()
                                 : (StartDateID != "" && EndDateID == "") ? firstPartOfQuery.Where(s => s.CallTime >= startDate).AsQueryable()
                                           : (StartDateID == "" && EndDateID != "") ? firstPartOfQuery.Where(s => s.CallTime <= endDate).AsQueryable()
                                             : firstPartOfQuery.Where(x => x.CallTime > dt && x.CallTime <= date).AsQueryable();
                }
                else
                {
                    var date = dt.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);
                    firstPartOfQuery =
                         (StartDateID != "" && EndDateID != "") ? firstPartOfQuery.Where(s => s.CallTime >= startDate && s.CallTime <= endDate).AsQueryable()
                                 : (StartDateID != "" && EndDateID == "") ? firstPartOfQuery.Where(s => s.CallTime >= startDate).AsQueryable()
                                           : (StartDateID == "" && EndDateID != "") ? firstPartOfQuery.Where(s => s.CallTime <= endDate).AsQueryable()
                                             : firstPartOfQuery.Where(x => x.CallTime > dt && x.CallTime <= date).AsQueryable();
                }
                var secondPartOfQuery = firstPartOfQuery.AsEnumerable();
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p => p.CallerName.ToString().ToLower().Contains(search.ToLower()) ||
                                                              p.CallerPhone.ToString().ToLower().Contains(search.ToLower()) ||
                                                              p.Subject.ToString().ToLower().Contains(search.ToLower())).Count() : 0;

                    secondPartOfQuery = secondPartOfQuery.Where(p => p.CallerName.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.CallerPhone.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.Subject.ToString().ToLower().Contains(search.ToLower())).AsEnumerable();
                }
                if (secondPartOfQuery.Count() > 0)
                {
                    totalRecords = secondPartOfQuery.AsEnumerable().Count();
                    data = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(

                            s => new CallHistoryViewModel
                            {
                                CallHistoryID = s.CallHistoryID,
                                CallerName = s.CallerName,
                                Subject = s.Subject,
                                Description = s.Description,
                                CallerPhone = s.CallerPhone,
                                EmployeeID = s.EmployeeID,
                                EmployeeName = GetEmpName(s.EmployeeID),
                                CountryID = db.Country.Find(s.CountryID).CountryName,
                                CompanyVsStaffID = GetStaffName(s.CompanyVsStaffID),
                                UpdateCallHistory = AppUtils.HasAccessInTheList(AppUtils.Update_CallHistory) ? true : false,
                            })
                        .ToList();

                }

                data = this.SortByColumnWithOrderForCompanyStaff(order, orderDir, data);
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : firstPartOfQuery.AsEnumerable().Count();

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


        private List<CallHistoryViewModel> SortByColumnWithOrderForCompanyStaff(string order, string orderDir, List<CallHistoryViewModel> data)
        {
            List<CallHistoryViewModel> lst = new List<CallHistoryViewModel>();
            try
            {
                switch (order)
                {

                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyVsStaffID).ToList() : data.OrderBy(p => p.CompanyVsStaffID).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CallerName).ToList() : data.OrderBy(p => p.CallerName).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CallerPhone).ToList() : data.OrderBy(p => p.CallerPhone).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyVsStaffID).ToList() : data.OrderBy(p => p.CompanyVsStaffID).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyVsStaffID).ToList() : data.OrderBy(p => p.CompanyVsStaffID).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {

                Console.Write(ex);
            }
            return lst;
        }
        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Update_CallHistory)]
        public ActionResult AddOrUpdateCallHistory(int? id)
        {
            if (id != null)
            {
                CallHistory callHistory = db.CallHistory.Find(id.Value);
                ViewBag.Country = new SelectList(db.Country.Where(s => s.Status == AppUtils.TableStatusIsActive).Select(s => new { CountryID = s.CountryID, CountryName = s.CountryName }).ToList(), "CountryID", "CountryName", callHistory.CountryID);
                ViewBag.Company = new SelectList(db.Company.Where(s => s.Status == AppUtils.TableStatusIsActive).Select(s => new { CompanyID = s.CompanyID, CompanyName = s.CompanyName }).ToList(), "CompanyID", "CompanyName", callHistory.CompanyID);
                ViewBag.CallCategory = new SelectList(db.CallCategory.Where(s => s.Status == AppUtils.TableStatusIsActive).Select(s => new { CallCategoryID = s.CallCategoryID, CallCategoryName = s.CallCategoryName }).ToList(), "CallCategoryID", "CallCategoryName", callHistory.CallCategoryID);
                ViewBag.StaffListt = new SelectList(db.CompanyVsStaff.Where(x => x.CompanyID == callHistory.CompanyID && x.Status == AppUtils.TableStatusIsActive).Select(s => new { CompanyVsStaffID = s.CompanyVsStaffID, StaffName = s.FirstName + " " + s.LastName }).ToList(), "CompanyVsStaffID", "StaffName", callHistory.CompanyVsStaffID);

                return View(callHistory);
            }
            else
            {
                ViewBag.Country = new SelectList(db.Country.Where(s => s.Status == AppUtils.TableStatusIsActive).Select(s => new { CountryID = s.CountryID, CountryName = s.CountryName }).ToList(), "CountryID", "CountryName");
                ViewBag.Company = new SelectList(db.Company.Where(s => s.Status == AppUtils.TableStatusIsActive).Select(s => new { CompanyID = s.CompanyID, CompanyName = s.CompanyName }).ToList(), "CompanyID", "CompanyName");
                ViewBag.CallCategory = new SelectList(db.CallCategory.Where(s => s.Status == AppUtils.TableStatusIsActive).Select(s => new { CallCategoryID = s.CallCategoryID, CallCategoryName = s.CallCategoryName }).ToList(), "CallCategoryID", "CallCategoryName");

                return View(new CallHistory());
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CallHistoryDelete(int CallHistoryID)
        {
            CallHistory callHistory = new CallHistory();
            callHistory = db.CallHistory.Find(CallHistoryID);
            callHistory.DeleteBy = AppUtils.GetLoginUserID();
            callHistory.DeleteDate = AppUtils.GetDateTimeNow();
            callHistory.Status = AppUtils.TableStatusIsDelete;


            db.Entry(callHistory).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var JSON = Json(new { success = true }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }


        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult CallHistoryDetailsInsert(CallHistory CallHistoryDetails)
        {
            if (CallHistoryDetails.CallHistoryID != 0)
            {
                try
                {
                    CallHistory dbCallHistory = db.CallHistory.Find(CallHistoryDetails.CallHistoryID);
                    SetValuesFromDbModelToClientModel(ref CallHistoryDetails, ref dbCallHistory);

                    db.Entry(dbCallHistory).CurrentValues.SetValues(CallHistoryDetails);
                    db.SaveChanges();

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                try
                {
                    SetValuesIntoDbModel(ref CallHistoryDetails);
                    db.CallHistory.Add(CallHistoryDetails);
                    db.SaveChanges();

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }

        }

        private void SetValuesIntoDbModel(ref CallHistory CallHistoryDetails)
        {
            CallHistoryDetails.CreateBy = AppUtils.GetLoginUserID();
            CallHistoryDetails.EmployeeID = AppUtils.GetLoginUserID();
            CallHistoryDetails.CreateDate = AppUtils.GetDateTimeNow();
            CallHistoryDetails.CallTime = AppUtils.GetDateTimeNow();
            CallHistoryDetails.Status = AppUtils.TableStatusIsActive;
        }

        private void SetValuesFromDbModelToClientModel(ref CallHistory CallHistoryDetails, ref CallHistory dbCallHistory)
        {
            CallHistoryDetails.CreateBy = dbCallHistory.CreateBy;
            CallHistoryDetails.CreateDate = dbCallHistory.CreateDate;
            CallHistoryDetails.CallTime = dbCallHistory.CallTime;
            CallHistoryDetails.EmployeeID = dbCallHistory.EmployeeID;
            CallHistoryDetails.Status = dbCallHistory.Status;
            CallHistoryDetails.UpdateBy = AppUtils.GetLoginUserID();
            CallHistoryDetails.UpdateDate = AppUtils.GetDateTimeNow();
        }

        [HttpPost]
        public ActionResult GetStaffList(int CompanyID)
        {
            ViewBag.StaffListt = new SelectList(db.CompanyVsStaff.Where(x => x.CompanyID == CompanyID && x.Status == AppUtils.TableStatusIsActive).Select(s => new { CompanyVsStaffID = s.CompanyVsStaffID, StaffName = s.FirstName + " " + s.LastName }).ToList(), "CompanyVsStaffID", "StaffName");
            return PartialView("GetStaffList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetEmployeeDetailsByID(int EmpID)
        {
            var callCount = db.CallHistory.Where(s => s.EmployeeID == EmpID && s.Status==AppUtils.TableStatusIsActive).Count();
            Employee employee = db.Employee.Where(s => s.EmployeeID == EmpID).FirstOrDefault();

            var JSON = Json(new { callCount = callCount, employee= employee,success = true }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

    }
}