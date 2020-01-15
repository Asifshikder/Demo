
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using Project.Models;
using System.ComponentModel;

namespace Project
{
    public static class AppUtils
    {
        protected internal enum PersonStatus
        {
            [Description("Active")]
            Active = 1,
            [Description("Lock")]
            Lock = 2
        }
        protected internal enum CourseStructure
        {
            [Description("Traditional")]
            Traditional = 1,
            [Description("Integrated")]
            Integrated = 2,
            [Description("PBL")]
            PBL = 3,
            [Description("CBL")]
            CBL = 4
        }
        protected internal enum IntercalatedBSc
        {
            [Description("Compusary")]
            Compusary = 1,
            [Description("Selective")]
            Selective = 2,
            [Description("None")]
            None = 2
        }
        protected internal enum InterViewStyle
        {
            [Description("Traditional")]
            Traditional = 1,
            [Description("MMI")]
            MMI = 2
        }
        public static List<string> GetTempNotUpdateEmployee { get; set; }

        public static DateTime dateTimeNow = DateTime.Now;


        public static bool HasAccessInTheList(string PID)
        {
            List<string> lstAccessList = HttpContext.Current.Session["lstAccessList"] != null ? HttpContext.Current.Session["lstAccessList"] as List<string> : new List<string>();
            if (lstAccessList.Contains(PID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //public static SMSReturnDetails SendSMS(string Sender, string ID, string Pass, string destination, string message)
        //{
        //    string html = string.Empty;
        //    string url = @"http://sms.walletmix.biz/sms-api/apiAccess?username=" + ID + "&password=" + Pass + "&type=0&destination=" + destination + "&source=" + Sender + "&message=" + message + "";

        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //    request.AutomaticDecompression = DecompressionMethods.GZip;

        //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //    using (Stream stream = response.GetResponseStream())
        //    using (StreamReader reader = new StreamReader(stream))
        //    {
        //        html = reader.ReadToEnd();
        //    }
        //    SMSReturnDetails SMSReturnDetails = JsonConvert.DeserializeObject<SMSReturnDetails>(html);

        //    return SMSReturnDetails;
        //}



        public static int GetLoginUserID()
        {
            //int LoggedInUserID = (int)HttpContext.Current.Session["LoggedUserID"];
            return 1 ;
        }
        public static int GetLoginRoleID()
        {
            int LoggedInUserID = (int)HttpContext.Current.Session["role_id"];
            return LoggedInUserID;
        }
        public static int GetLoginUserRightPermissionID()
        {
            int UserRightPermission = (int)HttpContext.Current.Session["CurrentUserRightPermission"];
            return UserRightPermission;
        }

        public static bool IsGranted(string buttonVal)
        {
            int countButtonNames = AppUtils.lstAccessList.Where(s => s == buttonVal).Count();
            if (countButtonNames > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static List<string> lstAccessList = new List<string>();

        private static DataContext db = new DataContext();

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        //public class SessionKeyConstants
        //{
        //    public const string LOGGEDIN_USER_KEY = "__loggedinuser";
        //    public const string CURRENT_FORM_KEY = "__loggedinuser";
        //    public const string PUBLIC_KEY = "__publicKey";
        //}

        //public static ClientDetails LoggedinUser
        //{
        //    get
        //    {
        //        ClientDetails loggedinUser = null;
        //        if (HttpContext.Current.Session[SessionKeyConstants.LOGGEDIN_USER_KEY] != null)
        //        {
        //            loggedinUser = HttpContext.Current.Session[SessionKeyConstants.LOGGEDIN_USER_KEY] as Users;
        //        }

        //        return loggedinUser;
        //    }
        //    set { HttpContext.Current.Session[SessionKeyConstants.LOGGEDIN_USER_KEY] = value; }
        //}
         
        public static int GetLoginUserRole()
        {
            int LoggedInUserID = (int)HttpContext.Current.Session["role_id"];
            return LoggedInUserID;
        }
        public static string GetLoginEmployeeName()
        {
            return db.Employee.Where(s => s.EmployeeID == AppUtils.LoginUserID).FirstOrDefault().FirstName;
        }

        public static DateTime ThisMonthStartDate()
        {
            DateTime firstDateOfThisMonth = new DateTime(dateTimeNow.Year, dateTimeNow.Month, 01);
            return firstDateOfThisMonth;
        }

        public static DateTime ThisMonthLastDate()
        {
            DateTime lastDateOfThisMonth = new DateTime(dateTimeNow.Year, dateTimeNow.Month,
                DateTime.DaysInMonth(dateTimeNow.Year, dateTimeNow.Month));
            return lastDateOfThisMonth;
        }

        public static DateTime GetDateTimeNow()
        {
            return dateTimeNow
            //return dateTimeNow.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute)
            //    .AddSeconds(DateTime.Now.Second).AddMilliseconds(DateTime.Now.Millisecond);
            ;
        }


        public static DateTime GetLastDayWithHrMinSecMsByMyDate(DateTime dt)
        {
            return dt.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);
        }


        public static int SuperTalentUserRole = 1;
        public static int SuperAdminRole = 2;
        public static int AdminRole = 3;
        public static int EmployeeRole = 4;
        public static int CompanyStaffRole = 7;
        public static int CompanyAdminRole = 6;

        public static bool SMSOptionEnable = false;

        public const int TableStatusIsActive = 1;
        public const int TableStatusIsUpdate = 2;
        public const int TableStatusIsDelete = 3;

        public const int PamentIsOccouringFromAccountsPage = 1;
        public const int PamentIsOccouringFromUnpaidPage = 2;
        public const int PamentIsOccouringFromNewClientSignUpPage = 3;
        public const int PamentIsOccouringFromAdvancePaymentAccountPage = 4;
        public const int PamentIsOccouringFromAdvancePaymentUnpaidPage = 5;

        public static int RunningYear = dateTimeNow.Year;
        public static int RunningMonth = dateTimeNow.Month;

        public static int LoginUserID { get; set; }
        public static int LoginUserType { get; set; }


        public static int SMSGlobalStatusIsTrue = 1;

        public static int SendSMSStatusTrue = 1;
        public static int SendSMSStatusFalse = 0;

        public static int PaymentIsNotPaid = 0;
        public static int PaymentIsPaid = 1;

        //public static int UserRightPermissionIDIsSuperUser = 1;
        public static int UserRightPermissionIDIsSuperTallentUser = 1;
        public static int UserRightPermissionIDIsCompanyAdmin = 5;
        public static int UserRightPermissionIDIsCompanyStaff = 6;
        //public static int UserRightPermissionIDIsAdminUser = 2;

        public static int CurrentUserRightPermission;

        public static int AdminStatusIsActive = 1;
        public static int AdminStatusIsLock = 2;
        public static int AdminStatusIsDelete = 3;



        public static int PaymentByHandCash = 1;
        public static int PaymentByPaymentGateWay = 2;

        public static string Bill_Pay_Code = "0000";
        public static string Member_Locked_This_Month = "0001";
        public static string Member_Active_This_Month = "0002";
        public static string SMS_Member_Complain_Open = "0003";
        public static string SMS_User_Complain_Open = "0004";
        public static string SMS_Member_Complain_Close = "0005";
        public static string SMS_User_Complain_Close = "0006";
        public static string New_Client_Request = "0007";
        public static string New_Client_Signup = "0008";
        public static string Member_Locked_Next_Month = "0009";
        public static string Member_Active_Next_Month = "0010";
        public static string Package_Change_This_Month = "0011";


        public static string ReturnMessageStatusCodeIsSuccess = "1000";
        public static string ReturnMessageStatusCodeIsFail = "";


        public static string[] lstSMSReleated = { "88", "89", "90" };

        public const string Add_Role = "1";
        public const string View_Role_List = "2";
        public const string Update_Role = "3";
        public const string Delete_Role = "4";

        public const string Set_User_Right = "5";
        public const string Assign_Admin_User_Right = "6";
        
        public const string Add_Admin = "7";
        public const string View_Admin_List = "8";
        public const string Update_Admin = "9";
        public const string Delete_Admin = "10";


        public const string View_Country = "23";
        public const string Create_Country = "24";
        public const string Update_Country = "25";
        public const string Delete_Country = "26";


        public const string View_Company = "7";
        public const string Create_Company = "8";
        public const string Update_Company = "9";
        public const string Delete_Company = "10";

        public const string View_CallCategory = "19";
        public const string Create_CallCategory = "20";
        public const string Update_CallCategory = "21";
        public const string Delete_CallCategory = "22";

        public const string View_CallHistory = "15";
        public const string View_CallHistoryForCompanyStaff = "27";
        public const string Create_CallHistory = "16";
        public const string Update_CallHistory = "17";
        public const string Delete_CallHistory = "18";

        public const string View_CompanyVsStaff = "11";
        public const string Create_CompanyVsSatff = "12";
        public const string Update_CompanyVsStaff = "13";
        public const string Delete_CompanyVsStaff = "14";

        public const string View_School = "11";
        public const string Add_School = "12";
        public const string Update_School = "13";
        public const string Delete_School = "14";

    }
}


