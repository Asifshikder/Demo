var CompanyVsStaffManager = {
    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {

        if (AppUtil.GetIdValue("InsertFirstName") === '') {
            AppUtil.ShowSuccess("Please write down first name.");
            return false;
        }
        if (AppUtil.GetIdValue("InsertLastName") === '') {
            AppUtil.ShowSuccess("Please write down last name.");
            return false;
        }
        if (AppUtil.GetIdValue("InsertLoginName") === '') {
            AppUtil.ShowSuccess("Please insert login name");
            return false;
        }
        if (AppUtil.GetIdValue("InsertPassword") === '') {
            AppUtil.ShowSuccess("Please insert password.");
            return false;
        }
        if (AppUtil.GetIdValue("InsertPhone") === '') {
            AppUtil.ShowSuccess("Please insert phone.");
            return false;
        }
        if (AppUtil.GetIdValue("InsertEmail") === '') {
            AppUtil.ShowSuccess("Please Write Down email address.");
            return false;
        }
        if (AppUtil.GetIdValue("StatusInsert") === '') {
            AppUtil.ShowSuccess("Please Select Staff Status.");
            return false;
        }
        if (AppUtil.GetIdValue("InsertAddress") === '') {
            AppUtil.ShowSuccess("Please insert Address.");
            return false;
        }
        if (AppUtil.GetIdValue("RoleID") === '') {
            AppUtil.ShowSuccess("Please select role.");
            return false;
        }
        if (AppUtil.GetIdValue("UserRightPermissionID") === '') {
            AppUtil.ShowSuccess("Please select user permission.");
            return false;
        }

        return true;
    },
    UpdateValidation: function () {

        if (AppUtil.GetIdValue("UpdateFirstName") === '') {
            AppUtil.ShowSuccess("Please write down first name.");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateLastName") === '') {
            AppUtil.ShowSuccess("Please write down last name.");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateLoginName") === '') {
            AppUtil.ShowSuccess("Please insert login name");
            return false;
        }
        if (AppUtil.GetIdValue("UpdatePassword") === '') {
            AppUtil.ShowSuccess("Please insert password.");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateStaffPhone") === '') {
            AppUtil.ShowSuccess("Please insert phone.");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateStaffEmail") === '') {
            AppUtil.ShowSuccess("Please Write Down email address.");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateStaffAddress") === '') {
            AppUtil.ShowSuccess("Please insert Address.");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateStatus") === '') {
            AppUtil.ShowSuccess("Please Select Staff Status.");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateRoleID") === '') {
            AppUtil.ShowSuccess("Please select role.");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateUserRightPermissionID") === '') {
            AppUtil.ShowSuccess("Please select user permission.");
            return false;
        }
        return true;
    },


    InsertCompanyStaffFromPopUp: function (_CompanyID) {

        var url = "/CompanyVsSatff/InsertCompanyVsStaffFromPopUp";
        var CompanyID = _CompanyID;
        var LoginName = AppUtil.GetIdValue("InsertLoginName");
        var Password = AppUtil.GetIdValue("InsertPassword");
        var FirstName = AppUtil.GetIdValue("InsertFirstName");
        var LastName = AppUtil.GetIdValue("InsertLastName");
        var Phone = AppUtil.GetIdValue("InsertPhone");
        var Email = AppUtil.GetIdValue("InsertEmail");
        var Address = AppUtil.GetIdValue("InsertAddress");
        var Status = AppUtil.GetIdValue("StatusInsert");
        //var RoleID = AppUtil.GetIdValue("RoleID");
        //var UserRightPermissionID = AppUtil.GetIdValue("UserRightPermissionID");
        var IsItSuperAdmin = $("#IsSuperUser").is(":checked") ? true : false;

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;


        var StaffInfo = {
            CompanyID: CompanyID, LoginName: LoginName, Password: Password, Status: Status, FirstName: FirstName, LastName: LastName, Phone: Phone, Email: Email,/* RoleID: RoleID, UserRightPermissionID: UserRightPermissionID,*/ IsItSuperAdmin: IsItSuperAdmin, Address: Address
        };

        var formData = new FormData();
        formData.append('StaffImage', $('#StaffImageInsert')[0].files[0]);
        formData.append('CompanyVsStaffDetails', JSON.stringify(StaffInfo));

        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, CompanyVsStaffManager.InsertCompanyStaffFromPopUpSuccess, CompanyVsStaffManager.InsertCompanyStaffFromPopUpFail);

    },
    InsertCompanyStaffFromPopUpSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Successfully inserted staff for this company.");
        }
        if (data.success === false) {
                AppUtil.ShowSuccess(" Failed to save staff.");
        }
        CompanyVsStaffManager.clearForSaveCompanyVsStaffInformation();
        $("#mdlCompanyStaffInsert").modal("hide");

        tblStaff.draw();
    },
    InsertCompanyStaffFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
    },



    GetCompanyVsStaffDetailsByID: function (_CompanyVsStaffID) {

        var url = "/CompanyVsSatff/GetDetailsByID/";
        var data = ({ CompanyVsStaffID: _CompanyVsStaffID });
        data = CompanyVsStaffManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, CompanyVsStaffManager.GetCompanyVsStaffDetailsByIDSuccess, CompanyVsStaffManager.GetCompanyVsStaffDetailsByIDFailed);
    },
    GetCompanyVsStaffDetailsByIDSuccess: function (data) {
        if (data.success === true) {
            $("#UpdateFirstName").val(data.companyVsStaff.FirstName);
            $("#UpdateLastName").val(data.companyVsStaff.LastName);
            $("#UpdateLoginName").val(data.companyVsStaff.LoginName);
            $("#UpdatePassword").val(data.companyVsStaff.Password);
            $("#UpdateStaffPhone").val(data.companyVsStaff.Phone);
            $("#UpdateStaffAddress").val(data.companyVsStaff.Address);
            $("#UpdateStaffEmail").val(data.companyVsStaff.Email);
            $("#UpdateStatus").val(data.companyVsStaff.Status);
            //$("#UpdateRoleID").val(data.companyVsStaff.RoleID);
            //$("#UpdateUserRightPermissionID").val(data.companyVsStaff.UserRightPermissionID);
            //$("#IsSuperUserUpdate").val();
            if (data.companyVsStaff.IsItSuperAdmin === true) {
                $("#IsSuperUserUpdate").prop("checked", true);
            }
            $("#StaffImageUpdatePath").val(data.companyVsStaff.CompanyStaffImage);

            var a = '' + data.companyVsStaff.CompanyStaffImage + '#' + new Date().getTime();

            $("#PreviewStaffImageUpdate").hide(0).attr('src', '' + data.companyVsStaff.CompanyStaffImage + '#' + new Date().getTime()).show(0);
            $("#PreviewCompanyUpdateImage").prop("src", data.companyVsStaff.CompanyStaffImage);

            $("#mdlCompanyStaffUpdate").modal("show");
        }
    },
    GetCompanyVsStaffDetailsByIDFailed: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
    },


    UpdateCompanyVsStaffInformation: function (_CompanyVsStaffID) {

        var url = "/CompanyVsSatff/UpdateCompanyVsStaffFromPopUp";

        var CompanyVsStaffID= _CompanyVsStaffID;
        var LoginName = AppUtil.GetIdValue("UpdateLoginName");
        var Password = AppUtil.GetIdValue("UpdatePassword");
        var FirstName = AppUtil.GetIdValue("UpdateFirstName");
        var LastName = AppUtil.GetIdValue("UpdateLastName");
        var Phone = AppUtil.GetIdValue("UpdateStaffPhone");
        var Email = AppUtil.GetIdValue("UpdateStaffEmail");
        var Address = AppUtil.GetIdValue("UpdateStaffAddress");
        var Status = AppUtil.GetIdValue("UpdateStatus");
        //var RoleID = AppUtil.GetIdValue("UpdateRoleID");
        var CompanyStaffImage = $("#StaffImageUpdatePath").val();
        //var UserRightPermissionID = AppUtil.GetIdValue("UpdateUserRightPermissionID");
        var IsItSuperAdmin = $("#IsSuperUserUpdate").is(":checked") ? true : false;

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;


        var StaffInfo = {
            CompanyVsStaffID: CompanyVsStaffID, CompanyStaffImage: CompanyStaffImage, LoginName: LoginName, Status: Status, Password: Password, FirstName: FirstName, LastName: LastName, Phone: Phone, Email: Email,/* RoleID: RoleID, UserRightPermissionID: UserRightPermissionID,*/ IsItSuperAdmin: IsItSuperAdmin, Address: Address
        };
        var formData = new FormData();
        formData.append('StaffImageUpdate', $('#StaffImageUpdate')[0].files[0]);
        formData.append('Staff_details', JSON.stringify(StaffInfo));
        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, CompanyVsStaffManager.UpdateCompanyVsStaffFromPopUpSuccess, CompanyVsStaffManager.UpdateCompanyVsStaffFromPopUpFail);


    },
    UpdateCompanyVsStaffFromPopUpSuccess: function (data) {

        if (data.success === true) {
            AppUtil.ShowSuccess("Successfully Updated. ");
            tblStaff.draw();
        }
        if (data.success === false) { 
                AppUtil.ShowSuccess("Update Failed.");
        }
        CompanyVsStaffManager.clearForUpdateCompanyVsStaffInformation();
        $("#mdlCompanyStaffUpdate").modal("hide");
    },
    UpdateCompanyVsStaffFromPopUpFail: function () {
        AppUtil.ShowSuccess("Error! Contact with Administator")
    },



    DeleteCompanyVsStaffFromPopUp: function (_CompanyVsStaffID) {

        var url = "/CompanyVsSatff/CompanyVsSatffDelete/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ CompanyVsStaffID: _CompanyVsStaffID });
        data = CompanyVsStaffManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, CompanyVsStaffManager.DeleteCompanyVsStaffFromPopUpSuccess, CompanyVsStaffManager.DeleteCompanyVsStaffFromPopUpFailed);
    },
    DeleteCompanyVsStaffFromPopUpSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Successfully Deleted!");
            tblStaff.draw();
        }
        if (data.success === false) {
            AppUtil.ShowSuccess("Failed to delete!");
        }
        $("#mdlCompanyStaffDelete").modal("hide");

    },
    DeleteCompanyVsStaffFromPopUpFailed: function (data) {
        AppUtil.ShowSuccess("Error! Contact with Administator")
    },


    clearForSaveCompanyVsStaffInformation: function () {
        $("#InsertFirstName").val("");
        $("#InsertLastName").val("");
        $("#InsertLoginName").val("");
        $("#InsertPassword").val("");
        $("#InsertPhone").val("");
        $("#CreateDetails").val("");
        $("#InsertEmail").val("");
        $("#InsertAddress").val("");
        $("#StatusInsert").val("");
        //$("#RoleID").val("");
        //$("#UserRightPermissionID").val("");
        $("#IsSuperUser").prop("checked", false);
        $("#PreviewStaffImageInsert").attr("src", "");
        $("#StaffImageInsert").wrap('<form>').closest('form').get(0).reset();
        $("#StaffImageInsert").unwrap();
    },
    clearForUpdateCompanyVsStaffInformation: function () {
        $("#UpdateFirstName").val("");
        $("#UpdateLastName").val("");
        $("#UpdateLoginName").val("");
        $("#UpdatePassword").val("");
        $("#UpdateStaffPhone").val("");
        $("#UpdateStaffAddress").val("");
        $("#UpdateStaffEmail").val("");
        $("#UpdateStatus").val("");
        //$("#UpdateRoleID").val("");
        $("#UpdateStaffAddress").val("");
        //$("#UpdateUserRightPermissionID").val("");
        $("#IsSuperUserUpdate").prop("checked", false);
        $("#PreviewStaffImageUpdate").attr("src", "");
        $("#StaffImageUpdate").wrap('<form>').closest('form').get(0).reset();
        $("#StaffImageUpdate").unwrap();
    },
}