var AdminManager = {


    addRequestVerificationToken: function (data) {
        debugger;
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    Validation: function () {

        if (AppUtil.GetIdValue("FirstName") === '') {
            AppUtil.ShowSuccess("Please Insert Admin Name.");
            return false;
        }
        if (AppUtil.GetIdValue("LoginName") === '') {
            AppUtil.ShowSuccess("Please Insert Admin Login Name.");
            return false;
        }
        if (AppUtil.GetIdValue("Password") === '') {
            AppUtil.ShowSuccess("Please Insert Password.");
            return false;
        }
        if (AppUtil.GetIdValue("Phone") === '') {
            AppUtil.ShowSuccess("Please Insert Phone Number.");
            return false;
        }

        if (AppUtil.GetIdValue("Address") === '') {
            AppUtil.ShowSuccess("Please Insert Address.");
            return false;
        }

        if (AppUtil.GetIdValue("Email") === '') {
            AppUtil.ShowSuccess("Please Insert Email.");
            return false;
        }

        if (AppUtil.GetIdValue("DepartmentID") === '') {
            AppUtil.ShowSuccess("Please Insert DepartmentID.");
            return false;
        }

        if (AppUtil.GetIdValue("RoleID") === '') {
            AppUtil.ShowSuccess("Please Insert RoleID.");
            return false;
        }

        if (AppUtil.GetIdValue("AdminStatus") === '') {
            AppUtil.ShowSuccess("Please Insert Status.");
            return false;
        }
        return true;
    },

    InsertAdminFromPopUp: function (Admin) {
        debugger;
        //AppUtil.ShowWaitingDialog();

        var url = "/Admin/InsertAdmin/";
        var AdminID = AppUtil.GetIdValue("AdminID");
        var FirstName = AppUtil.GetIdValue("FirstName");
        var LastName = AppUtil.GetIdValue("LastName");
        var LoginName = AppUtil.GetIdValue("LoginName");
        var Password = AppUtil.GetIdValue("Password");
        var Phone = AppUtil.GetIdValue("Phone");
        var Address = AppUtil.GetIdValue("Address");
        var Email = AppUtil.GetIdValue("Email");
        var DepartmentID = AppUtil.GetIdValue("DepartmentID");
        var RoleID = AppUtil.GetIdValue("RoleID");
        //var AdminStatus = AppUtil.GetIdValue("AdminStatus");
        var AdminStatus = AppUtil.GetIdValue("AdminStatusID");

        // setTimeout(function () {
        var Admin = { AdminID: AdminID, FirstName: FirstName, LastName: LastName, LoginName: LoginName, Password: Password, Phone: Phone, Address: Address, Email: Email, DepartmentID: DepartmentID, RoleID: RoleID, AdminStatus: AdminStatus };

        // var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        debugger;
        var datas = JSON.stringify({ Admin: Admin });
        AppUtil.MakeAjaxCall(url, "POST", datas, AdminManager.AdminManagerFromPopUpSuccess, AdminManager.AdminManagerFromPopUpFail);
        //    }, 500);


    },
    AdminManagerFromPopUpSuccess: function (data) {
        debugger;
        //AppUtil.HideWaitingDialog();
        if (data.SuccessInsert === true) {
            debugger;
            AppUtil.ShowSuccess("Successfully Insert Admin.");
            window.location.href = "/Admin/Index";
            //AppUtil.ShowSuccess("Successfully Insert ");
        }
        if (data.SuccessInsert === false) {

            if (data.LoginNameExist === true) {
                AppUtil.ShowError("Sorry Login Already Exist. Choose Another One.");
            }
            else {
                alert("Employe Insert fail ");
            }
            debugger;
            //AppUtil.ShowError("Insert fail");
        }
    },
    AdminManagerFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    ShowAdminDetailsByIDForUpdate: function (AdminID) {
        debugger;
        //var url = '@Url.Action("GetPackageDetailsByID", "Package")';
        debugger;
        //AppUtil.ShowWaitingDialog();
        //  setTimeout(function () {
        debugger;
        var url = "/Admin/GetAdminDetailsByID/";
        // var data = { AdminID: AdminID };
        var data = ({ AdminID: AdminID });
        //  data = PackageManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AdminManager.ShowAdminDetailsByIDForUpdateSuccess, AdminManager.ShowAdminDetailsByIDForUpdateError);

        //   }, 500);

    },
    ShowAdminDetailsByIDForUpdateSuccess: function (data) {
        // console.log(data);
        debugger;
        var AdminJSONParse = (data.AdminDetails);

        $("#NewFirstName").val(AdminJSONParse.FirstName);
        $("#NewLastName").val(AdminJSONParse.LastName);
        $("#NewLoginName").val(AdminJSONParse.LoginName);
        $("#NewPassword").val(AdminJSONParse.Password);
        $("#NewPhone").val(AdminJSONParse.Phone);
        $("#NewAddress").val(AdminJSONParse.Address);
        $("#NewEmail").val(AdminJSONParse.Email);
        $("#NewDepartmentID").val(AdminJSONParse.DepartmentID[0]);
        $("#NewRoleID").val(AdminJSONParse.RoleID);
        //$("#NewAdminStatus").val(AdminJSONParse.AdminStatus);
        $("#NewAdminStatusID").val(AdminJSONParse.AdminStatus);


        $("#mdlAdminUpdate").modal("show");
    },
    ShowAdminDetailsByIDForUpdateError: function () {
        console.log(data);
        alert("error");
    },

    AdminUpdateValidation: function () {
        if (AppUtil.GetIdValue("NewFirstName") === '') {
            AppUtil.ShowSuccess("Please Insert Admin First Name.");
            return false;
        }
        if (AppUtil.GetIdValue("NewLoginName") === '') {
            AppUtil.ShowSuccess("Please Insert Admin Login Name.");
            return false;
        }
        if (AppUtil.GetIdValue("NewPassword") === '') {
            AppUtil.ShowSuccess("Please Insert Password.");
            return false;
        }
        if (AppUtil.GetIdValue("NewPhone") === '') {
            AppUtil.ShowSuccess("Please Insert Phone Number.");
            return false;
        }

        if (AppUtil.GetIdValue("NewAddress") === '') {
            AppUtil.ShowSuccess("Please Insert Address.");
            return false;
        }

        if (AppUtil.GetIdValue("NewEmail") === '') {
            AppUtil.ShowSuccess("Please Insert Email.");
            return false;
        }

        if (AppUtil.GetIdValue("NewDepartmentID") === '') {
            AppUtil.ShowSuccess("Please Insert DepartmentID.");
            return false;
        }

        if (AppUtil.GetIdValue("NewRoleID") === '') {
            AppUtil.ShowSuccess("Please Insert RoleID.");
            return false;
        }

        if (AppUtil.GetIdValue("NewAdminStatus") === '') {
            AppUtil.ShowSuccess("Please Insert Status.");
            return false;
        }
        return true;
    },

    UpdateAdminInformation: function () {
        debugger;
        var AdminID = _AdminID;

        var NewFirstName = $("#NewFirstName").val();
        var NewLastName = $("#NewLastName").val();
        var NewLoginName = $("#NewLoginName").val();
        var Password = $("#NewPassword").val();
        var NewPhone = $("#NewPhone").val();
        var NewAddress = $("#NewAddress").val();
        var NewEmail = $("#NewEmail").val();
        var NewDepartmentID = $("#NewDepartmentID").val();
        var NewRoleID = $("#NewRoleID").val();
        //var NewAdminStatus = $("#NewAdminStatus").val();
        var NewAdminStatus = $("#NewAdminStatusID").val();
        var url = "/Admin/UpdateAdminInformation/";
        debugger;
        var AdminInfo = ({ AdminID: AdminID, FirstName: NewFirstName, LastName: NewLastName, LoginName: NewLoginName,Password:Password, Phone: NewPhone, Address: NewAddress, Email: NewEmail, DepartmentID: NewDepartmentID, RoleID: NewRoleID, AdminStatus: NewAdminStatus });
        var data = JSON.stringify({ Admin: AdminInfo });
        AppUtil.MakeAjaxCall(url, "POST", data, AdminManager.UpdateAdminSuccess, AdminManager.UpdateAdminError);
    },
    UpdateAdminSuccess: function (data) {
        debugger;
        if (data.UpdateSuccess === false) {
            if (data.LoginNameExist === true) {
                AppUtil.ShowError("Sorry Login Already Exist. Choose Another One.");
            }
            else {
                AppUtil.ShowError("Update Fail. Contact With Administrator.");
            }
        }

        if (data.UpdateSuccess === true) {
            var AdminInformation = (data.Admins);

            $("#tblAdmin tbody>tr").each(function () {
                debugger;
                var AdminID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (AdminInformation.AdminID == AdminID) {
                    debugger;
                    var status = "";
                    if (AdminInformation.AdminStatus == 1)
                    {
                        status = "Active";
                    }
                    if (AdminInformation.AdminStatus == 2)
                    {
                        status = "Lock";
                    }
                    $('#tblAdmin tbody>tr:eq(' + index + ')').find("td:eq(1)").text(AdminInformation.Name);
                    $('#tblAdmin tbody>tr:eq(' + index + ')').find("td:eq(2)").text(AdminInformation.LoginName);
                    $('#tblAdmin tbody>tr:eq(' + index + ')').find("td:eq(3)").text(AdminInformation.Phone);
                    $('#tblAdmin tbody>tr:eq(' + index + ')').find("td:eq(4)").text(AdminInformation.Address);
                    $('#tblAdmin tbody>tr:eq(' + index + ')').find("td:eq(5)").text(AdminInformation.Email);
                    $('#tblAdmin tbody>tr:eq(' + index + ')').find("td:eq(6)").text(AdminInformation.DepartmentName);
                    $('#tblAdmin tbody>tr:eq(' + index + ')').find("td:eq(7)").text(AdminInformation.RoleName);
                    $('#tblAdmin tbody>tr:eq(' + index + ')').find("td:eq(8)").text(status);
                    //$('#tblExpense tbody>tr:eq(' + index + ')').find("td:eq(6)").text(AppUtil.ParseDateTime(PackageInformation[0].Date));
                }
            });

        }
        $("#mdlAdminUpdate").modal('hide');
        console.log(data);
    },
    UpdateAdminError: function (data) {
        console.log(data);
    },

    
    PrintAdminList: function () {
        debugger;
        var url = "/Excel/CreateReportForAdmin";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        debugger;
        var data = ({});
        data = AdminManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AdminManager.PrintAdminListSuccess, AdminManager.PrintAdminListFail);
    },
    PrintAdminListSuccess: function (data) {
        debugger;
        console.log(data);
        var response = (data);
        window.location = '/Excel/Download?fileGuid=' + response.FileGuid
            + '&filename=' + response.FileName;

        //if (data.Success === true) {
        //    AppUtil.ShowSuccess("Admin List Print Successfully.");
        //}
        //if (data.Success === false) {
        //    AppUtil.ShowSuccess("Failed To Print Admin List.");
        //}
    },
    PrintAdminListFail: function (data) {
        debugger;
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },
}