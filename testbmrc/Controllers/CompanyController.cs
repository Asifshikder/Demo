using Project;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.ViewModels;
using static Project.JSON_Antiforgery_Token_Validation;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using testbmrc.ViewModels;

namespace Project.Controllers
{
    [Authorize]
    [AjaxAuthorize]
    public class CompanyController : Controller
    {
        private DataContext db = new DataContext();
        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Company)]
        public ActionResult Index()
        {
            List<SelectListItem> StatusType = new List<SelectListItem>();
            StatusType.Add(new SelectListItem() { Text = "Active", Value = "1" });
            StatusType.Add(new SelectListItem() { Text = "Lock", Value = "2" });
            ViewBag.StatusType = new SelectList(StatusType, "Value", "Text");
            ViewBag.Country = new SelectList(db.Country.Where(s => s.Status == AppUtils.TableStatusIsActive).Select(s => new { CountryID = s.CountryID, CountryName = s.CountryName }).ToList(), "CountryID", "CountryName");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllCompanyAjaxData()
        {
            JsonResult result = new JsonResult();
            try
            {
                int CountryFromDDL = 0;
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                var CountryID = Request.Form.Get("CountryID");
                int totalRecords = 0;
                if (!string.IsNullOrEmpty(CountryID))
                {
                    CountryFromDDL = int.Parse(CountryID);
                }


                var firstPartOfQuery = db.Company.Where(a => a.Status == AppUtils.TableStatusIsActive).AsQueryable();

                int ifSearch = 0;
                List<CompanyViewModel> data = new List<CompanyViewModel>();
                firstPartOfQuery =
                     (!string.IsNullOrEmpty(CountryID)) ? firstPartOfQuery.Where(s => s.CountryID == CountryFromDDL).AsQueryable()
                                         : firstPartOfQuery.AsQueryable();



                var secondPartOfQuery = firstPartOfQuery.AsEnumerable();
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p => p.CompanyName.ToString().ToLower().Contains(search.ToLower()) || p.Details.ToString().ToLower().Contains(search.ToLower()) ||
                                                              p.Country.CountryName.ToString().ToLower().Contains(search.ToLower())).Count() : 0;

                    secondPartOfQuery = secondPartOfQuery.Where(p => p.CompanyName.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.Details.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.Country.CountryName.ToString().ToLower().Contains(search.ToLower())).AsEnumerable();
                }
                if (secondPartOfQuery.Count() > 0)
                {
                    totalRecords = secondPartOfQuery.AsEnumerable().Count();
                    data = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(

                            s => new CompanyViewModel
                            {
                                CompanyID = s.CompanyID,
                                CompanyName = s.CompanyName,
                                Details = s.Details,
                                CountryName = db.Country.Find(s.CountryID).CountryName,
                                CompanyLogo = s.CompanyLogo,
                                UpdateCompany = AppUtils.HasAccessInTheList(AppUtils.Update_Company) ? true : false,
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

        private List<CompanyViewModel> SortByColumnWithOrder(string order, string orderDir, List<CompanyViewModel> data)
        {
            List<CompanyViewModel> lst = new List<CompanyViewModel>();
            try
            {
                switch (order)
                {

                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyID).ToList() : data.OrderBy(p => p.CompanyID).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyName).ToList() : data.OrderBy(p => p.CompanyName).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Details).ToList() : data.OrderBy(p => p.Details).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CountryName).ToList() : data.OrderBy(p => p.CountryName).ToList();
                        break;

                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyID).ToList() : data.OrderBy(p => p.CompanyID).ToList();
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
        public ActionResult InsertCompanyFromPopUp(FormCollection form, HttpPostedFileBase CompanyImage)
        {

            Company client_Company_info = JsonConvert.DeserializeObject<Company>(form["CompanyDetails"]);
            Company db_Company_Check = db.Company.Where(s => s.CompanyName.ToLower() == client_Company_info.CompanyName.ToLower().Trim() && s.Status != AppUtils.TableStatusIsDelete).FirstOrDefault();
            if (db_Company_Check != null)
            {
                return Json(new { success = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Company CompanyReturn = new Company();

            try
            {

                client_Company_info.Status = AppUtils.TableStatusIsActive;
                client_Company_info.CreateBy = AppUtils.GetLoginUserID();
                //Company_info.CreateBy = AppUtils.GetLoginEmployeeName();
                client_Company_info.CreateDate = AppUtils.GetDateTimeNow();
                CompanyReturn = db.Company.Add(client_Company_info);
                db.SaveChanges();
                SaveImageInFolderAndAddInformationInCompanyTable(ref client_Company_info, "LOGO", CompanyImage);
                if (CompanyReturn.CompanyID > 0)
                {
                    db.SaveChanges();
                    return Json(new { success = true, }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDetailsByID(int CompanyID)
        {
            Company company = new Company();
            company = db.Company.Find(CompanyID);

            var JSON = Json(new { company = company, success = true }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateCompanyFromPopUp(FormCollection form, HttpPostedFileBase CompanyImageUpdate)
        {

            Company Company_details = JsonConvert.DeserializeObject<Company>(form["Company_details"]);
            Company Company_CheckExistOrNot = db.Company.Where(s => s.CompanyID != Company_details.CompanyID && s.CompanyName.ToLower().Trim() == Company_details.CompanyName.ToLower().Trim()).FirstOrDefault();

            if (Company_CheckExistOrNot != null)
            {
                return Json(new { success = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }


            Company Company_DB = db.Company.Where(s => s.CompanyID == Company_details.CompanyID).FirstOrDefault();
            try
            {

                AddGivenImageInCurrentRow(ref Company_DB, Company_details, "LOGO", CompanyImageUpdate, Company_details.CompanyLogo);


                if (Company_DB.CompanyID > 0)
                {
                    SetValuesFromClientModelToDbModel(ref Company_DB, ref Company_details);

                    db.Entry(Company_DB).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        private void SetValuesFromClientModelToDbModel(ref Company Company_DB, ref Company Company_details)
        {
            Company_DB.CompanyName = Company_details.CompanyName;
            Company_DB.Address = Company_details.Address;
            Company_DB.City = Company_details.City;
            Company_DB.PostCode = Company_details.PostCode;
            Company_DB.CountryID = Company_details.CountryID;
            Company_DB.Details = Company_details.Details;
            Company_DB.UpdateBy = AppUtils.GetLoginUserID();
            Company_DB.UpdateDate = AppUtils.GetDateTimeNow();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyDelete(int CompanyID)
        {
            try
            {
                var company = db.Company.Where(s => s.CompanyID == CompanyID).FirstOrDefault();
                company.DeleteBy = AppUtils.GetLoginUserID();
                company.DeleteDate = AppUtils.GetDateTimeNow();
                company.Status = AppUtils.TableStatusIsDelete;
                db.Entry(company).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                var JSON = Json(new { success = true }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch (Exception ex)
            {
                var JSON = Json(new { success = false }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetStaffListForSpecificCompanyByID()
        {
            JsonResult result = new JsonResult();
            try
            {
                List<CompanyVsStaffViewModel> data = new List<CompanyVsStaffViewModel>();
                int CompanyFromDDL = 0;
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                var CompanyID = Request.Form.Get("CompanyID");
                int totalRecords = 0;
                if (!string.IsNullOrEmpty(CompanyID))
                {
                    CompanyFromDDL = int.Parse(CompanyID);
                }

                if (CompanyFromDDL == 0)
                {
                    result = this.Json(new
                    {
                        draw = Convert.ToInt32(draw),
                        recordsTotal = 0,
                        recordsFiltered = 0,
                        data = data
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var firstPartOfQuery = db.CompanyVsStaff.Where(a => a.Status != AppUtils.TableStatusIsDelete).AsQueryable();

                    int ifSearch = 0;
                    firstPartOfQuery =
                         (!string.IsNullOrEmpty(CompanyID)) ? firstPartOfQuery.Where(s => s.CompanyID == CompanyFromDDL).AsQueryable()
                                             : firstPartOfQuery.AsQueryable();



                    var secondPartOfQuery = firstPartOfQuery.AsEnumerable();
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {

                        ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p => p.FirstName.ToString().ToLower().Contains(search.ToLower()) || p.LastName.ToString().ToLower().Contains(search.ToLower())
                        || p.Email.ToString().ToLower().Contains(search.ToLower()) || p.Phone.ToString().ToLower().Contains(search.ToLower())).Count() : 0;

                        secondPartOfQuery = secondPartOfQuery.Where(p => p.FirstName.ToString().ToLower().Contains(search.ToLower())
                                                                         || p.LastName.ToString().ToLower().Contains(search.ToLower())
                                                                         || p.Email.ToString().ToLower().Contains(search.ToLower())
                                                                         || p.Phone.ToString().ToLower().Contains(search.ToLower())).AsEnumerable();
                    }
                    if (secondPartOfQuery.Count() > 0)
                    {
                        totalRecords = secondPartOfQuery.AsEnumerable().Count();
                        data = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(

                                s => new CompanyVsStaffViewModel
                                {
                                    CompanyVsStaffID = s.CompanyVsStaffID,
                                    FirstName = s.FirstName,
                                    LastName = s.LastName,
                                    Email = s.Email,
                                    Phone = s.Phone,
                                    Status = s.Status,
                                    CompanyStaffImage = s.CompanyStaffImage,
                                    UpdateCompanyVsStaff = AppUtils.HasAccessInTheList(AppUtils.Update_CompanyVsStaff) ? true : false,
                                })
                            .ToList();

                    }

                    int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : firstPartOfQuery.AsEnumerable().Count();

                    result = this.Json(new
                    {
                        draw = Convert.ToInt32(draw),
                        recordsTotal = totalRecords,
                        recordsFiltered = recFilter,
                        data = data
                    }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return result;
        }

        private void AddGivenImageInCurrentRow(ref Company company_DB, Company company_details, string type, HttpPostedFileBase image, string imagePath)
        {
            if (type == "LOGO")
            {
                if (image != null && imagePath != null)
                {
                    RemoveOldImageAndThenSaveImageDuringClientUpdate(ref company_DB, company_details, "LOGO", image);
                }
                else if (!string.IsNullOrEmpty(imagePath))
                {
                    company_DB.CompanyLogo = company_details.CompanyLogo;
                }
                else
                {
                    RemoveImageFromServerFolder(type, company_DB);
                    company_DB.CompanyLogoByte = null;
                }
            }
        }

        private void RemoveOldImageAndThenSaveImageDuringClientUpdate(ref Company company_DB, Company company_details, string type, HttpPostedFileBase image)
        {
            RemoveImageFromServerFolder(type, company_DB);
            byte[] imagebyte = null;

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(image.FileName);
            string extension = Path.GetExtension(image.FileName);
            var fileName = company_DB.CompanyID + "_" + type + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/images/CompanyImage"), fileName);
            image.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(image.InputStream);
            imagebyte = reader.ReadBytes(image.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            if (type == "LOGO")
            {
                company_DB.CompanyLogo = "/images/CompanyImage/" + fileName;

            }
        }

        private void RemoveImageFromServerFolder(string type, Company company_DB)
        {
            string removeImageName = "";
            if (type == "LOGO")
            {
                removeImageName = !string.IsNullOrEmpty(company_DB.CompanyLogo) ? company_DB.CompanyLogo.Split('/')[3] : "";

            }

            var filePath = Server.MapPath("~/images/CompanyImage/" + removeImageName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        private void SaveImageInFolderAndAddInformationInCompanyTable(ref Company company_info, string WhichPic, HttpPostedFileBase image)
        {
            if (!IsValidContentType(image.ContentType))
            {
                ViewBag.Error = "Only PNG image are allowed";
            }

            byte[] imagebyte = null;

            string fileNameWithExtension = Path.GetFileName(image.FileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(image.FileName);
            string extension = Path.GetExtension(image.FileName);
            var fileName = company_info.CompanyID + "_" + WhichPic + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/images/CompanyImage"), fileName);
            image.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(image.InputStream);
            imagebyte = reader.ReadBytes(image.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            company_info.CompanyLogoByte = imagebyte;
            company_info.CompanyLogo = "/images/CompanyImage/" + fileName;


        }
        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }

        private byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
        private bool IsValidContentType(string contentType)
        {
            return contentType.Equals("image/jpeg");
        }

    }
}