using Newtonsoft.Json;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testbmrc.ViewModels;
using static Project.JSON_Antiforgery_Token_Validation;

namespace Project.Controllers
{
    [Authorize]
    [AjaxAuthorize]
    public class CompanyVsSatffController : Controller
    {
        private DataContext db = new DataContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDetailsByID(int CompanyVsStaffID)
        {
            CompanyVsStaff companyVsStaff = new CompanyVsStaff();
            companyVsStaff = db.CompanyVsStaff.Find(CompanyVsStaffID);

            var JSON = Json(new { companyVsStaff = companyVsStaff, success = true }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpGet]
        public ActionResult CompanyStaffDetails()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetStaffListForSpecificCompanyByID()
        {
            JsonResult result = new JsonResult();
            try
            {
                List<CompanyVsStaffViewModel> data = new List<CompanyVsStaffViewModel>();
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                int totalRecords = 0;
                var firstPartOfQuery = db.CompanyVsStaff.Where(a => a.Status != AppUtils.TableStatusIsDelete).AsQueryable();
                int StaffID = AppUtils.GetLoginUserID();
                int LoginStaffCompanyID = db.CompanyVsStaff.Where(s => s.CompanyVsStaffID == StaffID).FirstOrDefault().CompanyID;

                int ifSearch = 0;
                firstPartOfQuery = firstPartOfQuery.Where(s => s.CompanyID == LoginStaffCompanyID).AsQueryable();



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
                                CompanyStaffImage = s.CompanyStaffImage
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

        private List<CompanyVsStaffViewModel> SortByColumnWithOrder(string order, string orderDir, List<CompanyVsStaffViewModel> data)
        {
            List<CompanyVsStaffViewModel> lst = new List<CompanyVsStaffViewModel>();
            try
            {
                switch (order)
                {

                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyVsStaffID).ToList() : data.OrderBy(p => p.CompanyVsStaffID).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Email).ToList() : data.OrderBy(p => p.Email).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Phone).ToList() : data.OrderBy(p => p.Phone).ToList();
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

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult InsertCompanyVsStaffFromPopUp(FormCollection form, HttpPostedFileBase StaffImage)
        {
            CompanyVsStaff client_Staff_info = JsonConvert.DeserializeObject<CompanyVsStaff>(form["CompanyVsStaffDetails"]);
            CompanyVsStaff CompanyStaffReturn = new CompanyVsStaff();

            try
            {
                if (client_Staff_info.IsItSuperAdmin)
                {
                    client_Staff_info.RoleID = AppUtils.CompanyAdminRole;
                    client_Staff_info.UserRightPermissionID = AppUtils.UserRightPermissionIDIsCompanyAdmin;
                }
                else
                {
                    client_Staff_info.RoleID = AppUtils.CompanyStaffRole;
                    client_Staff_info.UserRightPermissionID = AppUtils.UserRightPermissionIDIsCompanyStaff;
                }
                client_Staff_info.Status = client_Staff_info.Status;
                client_Staff_info.CreateBy = AppUtils.GetLoginUserID();
                //Company_info.CreateBy = AppUtils.GetLoginEmployeeName();
                client_Staff_info.CreateDate = AppUtils.GetDateTimeNow();
                CompanyStaffReturn = db.CompanyVsStaff.Add(client_Staff_info);
                db.SaveChanges();
                if (StaffImage != null)
                {
                    SaveImageInFolderAndAddInformationInCompanyTable(ref client_Staff_info, "LOGO", StaffImage);
                }
                if (CompanyStaffReturn.CompanyID > 0)
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
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateCompanyVsStaffFromPopUp(FormCollection form, HttpPostedFileBase StaffImageUpdate)
        {

            CompanyVsStaff client_Staff_details = JsonConvert.DeserializeObject<CompanyVsStaff>(form["Staff_details"]);
            CompanyVsStaff CompanyStaff_DB = db.CompanyVsStaff.Where(s => s.CompanyVsStaffID == client_Staff_details.CompanyVsStaffID).FirstOrDefault();
            try
            {

                AddGivenImageInCurrentRow(ref CompanyStaff_DB, client_Staff_details, "LOGO", StaffImageUpdate, client_Staff_details.CompanyStaffImage);


                if (CompanyStaff_DB.CompanyID > 0)
                {
                    SetValuesFromClientModelToDBModel(ref CompanyStaff_DB, ref client_Staff_details);
                    if (client_Staff_details.IsItSuperAdmin)
                    {
                        CompanyStaff_DB.RoleID = AppUtils.CompanyAdminRole;
                        CompanyStaff_DB.UserRightPermissionID = AppUtils.UserRightPermissionIDIsCompanyAdmin;
                    }
                    else
                    {
                        CompanyStaff_DB.RoleID = AppUtils.CompanyStaffRole;
                        CompanyStaff_DB.UserRightPermissionID = AppUtils.UserRightPermissionIDIsCompanyStaff;
                    }


                    db.Entry(CompanyStaff_DB).State = System.Data.Entity.EntityState.Modified;
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

        private void SetValuesFromClientModelToDBModel(ref CompanyVsStaff CompanyStaff_DB, ref CompanyVsStaff client_Staff_details)
        {
            CompanyStaff_DB.FirstName = client_Staff_details.FirstName;
            CompanyStaff_DB.Address = client_Staff_details.Address;
            CompanyStaff_DB.LastName = client_Staff_details.LastName;
            CompanyStaff_DB.Password = client_Staff_details.Password;
            CompanyStaff_DB.LoginName = client_Staff_details.LoginName;
            CompanyStaff_DB.CompanyID = CompanyStaff_DB.CompanyID;
            CompanyStaff_DB.Email = client_Staff_details.Email;
            CompanyStaff_DB.Phone = client_Staff_details.Phone;
            CompanyStaff_DB.RoleID = CompanyStaff_DB.RoleID;
            CompanyStaff_DB.Status = client_Staff_details.Status;
            CompanyStaff_DB.UserRightPermissionID = client_Staff_details.UserRightPermissionID;
            CompanyStaff_DB.IsItSuperAdmin = client_Staff_details.IsItSuperAdmin;
            CompanyStaff_DB.UpdateBy = AppUtils.GetLoginUserID();
            //Company_DB.UpdateBy = AppUtils.GetLoginEmployeeName();
            CompanyStaff_DB.UpdateDate = AppUtils.GetDateTimeNow();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyVsSatffDelete(int CompanyVsStaffID)
        {
            try
            {
                var StaffInfo = db.CompanyVsStaff.Where(s => s.CompanyVsStaffID == CompanyVsStaffID).FirstOrDefault();
                //company.DeleteBy = AppUtils.GetLoginEmployeeName();
                StaffInfo.DeleteDate = AppUtils.GetDateTimeNow();
                StaffInfo.Status = AppUtils.TableStatusIsDelete;
                db.Entry(StaffInfo).State = System.Data.Entity.EntityState.Modified;
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

        private void AddGivenImageInCurrentRow(ref CompanyVsStaff Staff_DB, CompanyVsStaff Staff_details, string type, HttpPostedFileBase image, string imagePath)
        {
            if (type == "LOGO")
            {
                if (image != null && imagePath != null)
                {
                    RemoveOldImageAndThenSaveImageDuringClientUpdate(ref Staff_DB, Staff_details, "LOGO", image);
                }
                else if (!string.IsNullOrEmpty(imagePath))
                {
                    Staff_DB.CompanyStaffImage = Staff_details.CompanyStaffImage;
                }
                else
                {
                    RemoveImageFromServerFolder(type, Staff_DB);
                    Staff_DB.CompanyStaffImage = null;
                }
            }
        }

        private void RemoveOldImageAndThenSaveImageDuringClientUpdate(ref CompanyVsStaff Staff_DB, CompanyVsStaff Staff_details, string type, HttpPostedFileBase image)
        {
            RemoveImageFromServerFolder(type, Staff_DB);
            byte[] imagebyte = null;

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(image.FileName);
            string extension = Path.GetExtension(image.FileName);
            var fileName = Staff_DB.CompanyVsStaffID + "_" + type + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/images/CompanyVsStaffImage"), fileName);
            image.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(image.InputStream);
            imagebyte = reader.ReadBytes(image.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            if (type == "LOGO")
            {
                Staff_DB.CompanyStaffImage = "/images/CompanyVsStaffImage/" + fileName;

            }
        }

        private void RemoveImageFromServerFolder(string type, CompanyVsStaff Staff_DB)
        {
            string removeImageName = "";
            if (type == "LOGO")
            {
                removeImageName = !string.IsNullOrEmpty(Staff_DB.CompanyStaffImage) ? Staff_DB.CompanyStaffImage.Split('/')[3] : "";

            }

            var filePath = Server.MapPath("~/images/CompanyVsStaffImage/" + removeImageName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        private void SaveImageInFolderAndAddInformationInCompanyTable(ref CompanyVsStaff companyStaff_info, string WhichPic, HttpPostedFileBase image)
        {
            if (!IsValidContentType(image.ContentType))
            {
                ViewBag.Error = "Only PNG image are allowed";
            }

            byte[] imagebyte = null;

            string fileNameWithExtension = Path.GetFileName(image.FileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(image.FileName);
            string extension = Path.GetExtension(image.FileName);
            var fileName = companyStaff_info.CompanyVsStaffID + "_" + WhichPic + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/images/CompanyVsStaffImage"), fileName);
            image.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(image.InputStream);
            imagebyte = reader.ReadBytes(image.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            companyStaff_info.CompanyStaffImageByte = imagebyte;
            companyStaff_info.CompanyStaffImage = "/images/CompanyVsStaffImage/" + fileName;


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