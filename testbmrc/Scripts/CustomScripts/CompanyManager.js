var CompanyManager = {
    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {

        if (AppUtil.GetIdValue("CreateCompanyName") === '') {
            AppUtil.ShowSuccess("Please write down company name.");
            return false;
        }
        if (AppUtil.GetIdValue("CreatePostCode") === '') {
            AppUtil.ShowSuccess("Please write down company Post Code.");
            return false;
        }
        if (AppUtil.GetIdValue("CreateCity") === '') {
            AppUtil.ShowSuccess("Please Insert City");
            return false;
        }
        if (AppUtil.GetIdValue("CreateCountryID") === '') {
            AppUtil.ShowSuccess("Please Select Country.");
            return false;
        }
        if (AppUtil.GetIdValue("CreateAddress") === '') {
            AppUtil.ShowSuccess("Please insert Address.");
            return false;
        }
        if (AppUtil.GetIdValue("CreateDetails") === '') {
            AppUtil.ShowSuccess("Please Write Down Company Details.");
            return false;
        }
        if (AppUtil.GetIdValue("CompanyCreateImage") === '') {
            AppUtil.ShowSuccess("Please Upload an image.");
            return false;
        }

        return true;
    },
    UpdateValidation: function () {

        if (AppUtil.GetIdValue("UpdateCompanyName") === '') {
            AppUtil.ShowSuccess("Please write down company name.");
            return false;
        }
        if (AppUtil.GetIdValue("UpdatePostCode") === '') {
            AppUtil.ShowSuccess("Please write down Post Code.");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateAddress") === '') {
            AppUtil.ShowSuccess("Please write down company address");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateCity") === '') {
            AppUtil.ShowSuccess("Please insert city.");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateCountryID") === '') {
            AppUtil.ShowSuccess("Please select country.");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateDetails") === '') {
            AppUtil.ShowSuccess("Please write company details.");
            return false;
        }
        

        return true;
    },


    InsertCompanyFromPopUp: function () {

        var url = "/Company/InsertCompanyFromPopUp";
        var CompanyName = AppUtil.GetIdValue("CreateCompanyName");
        var PostCode = AppUtil.GetIdValue("CreatePostCode");
        var City = AppUtil.GetIdValue("CreateCity");
        var CountryID = AppUtil.GetIdValue("CreateCountryID");
        var Details = AppUtil.GetIdValue("CreateDetails");
        var Tag = AppUtil.GetIdValue("CreateTag");
        var Address = AppUtil.GetIdValue("CreateAddress");

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;


        var CompanyInfo = {
            CompanyName: CompanyName, PostCode: PostCode, City: City, CountryID: CountryID, Details: Details, Tag: Tag, Address: Address
        };

        var formData = new FormData();
        formData.append('CompanyImage', $('#CompanyCreateImage')[0].files[0]);
        formData.append('CompanyDetails', JSON.stringify(CompanyInfo));

        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, CompanyManager.InsertCompanyFromPopUpSuccess, CompanyManager.InsertCompanyFromPopUpFail);

    },
    InsertCompanyFromPopUpSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            CompanyManager.clearForSaveCompanyInformation();
            table.draw();
            $("#mdlCompanyInsert").modal('hide');
        }
        if (data.success === false) {

            if (data.AlreadyInsert === true) {
                AppUtil.ShowSuccess("Company already exist. Choose different Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }

    },
    InsertCompanyFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
    },



    GetCompanyDetailsByID: function (_CompanyID) {

        var url = "/Company/GetDetailsByID/";
        var data = ({ CompanyID: _CompanyID });
        data = CompanyManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, CompanyManager.GetCompanyDetailsByIDSuccess, CompanyManager.GetCompanyDetailsByIDFailed);
    },
    GetCompanyDetailsByIDSuccess: function (data) {
        if (data.success === true) {
            $("#UpdateCompanyName").val(data.company.CompanyName);
            $("#UpdatePostCode").val(data.company.PostCode);
            $("#UpdateAddress").val(data.company.Address);
            $("#UpdateCity").val(data.company.City);
            $("#UpdateCountryID").val(data.company.CountryID);
            $("#UpdateDetails").val(data.company.Details);
            $("#UpdateTag").val(data.company.Tag);
            $("#CompanyUpdateImagePath").val(data.company.CompanyLogo);

            var a = '' + data.company.CompanyLogo + '#' + new Date().getTime();

            $("#PreviewCompanyUpdateImage").hide(0).attr('src', '' + data.company.CompanyLogo + '#' + new Date().getTime()).show(0);
            $("#PreviewCompanyUpdateImage").prop("src", data.company.CompanyLogo);

            $("#mdlCompanyUpdate").modal("show");
        }
    },
    GetCompanyDetailsByIDFailed: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
    },




    UpdateCompanyInformation: function (_CompanyID) {

        var url = "/Company/UpdateCompanyFromPopUp";

        var CompanyID = _CompanyID;
        var CompanyName = AppUtil.GetIdValue("UpdateCompanyName");
        var PostCode = AppUtil.GetIdValue("UpdatePostCode");
        var City = AppUtil.GetIdValue("UpdateCity");
        var CountryID = AppUtil.GetIdValue("UpdateCountryID");
        var Details = AppUtil.GetIdValue("UpdateDetails");
        var Tag = AppUtil.GetIdValue("CreateTag");
        var Address = AppUtil.GetIdValue("UpdateAddress");
        var CompanyLogo = $("#CompanyUpdateImagePath").val();

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var CompanyInfo = {
            CompanyID: CompanyID, CompanyName: CompanyName, PostCode: PostCode, City: City, CountryID: CountryID, Details: Details, Tag: Tag, Address: Address, CompanyLogo: CompanyLogo
        };

        var formData = new FormData();
        formData.append('CompanyImageUpdate', $('#CompanyUpdateImage')[0].files[0]);
        formData.append('Company_details', JSON.stringify(CompanyInfo));
        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, CompanyManager.UpdateCompanyFromPopUpSuccess, CompanyManager.UpdateCompanyFromPopUpFail);


    },
    UpdateCompanyFromPopUpSuccess: function (data) {

        if (data.success === true) {
            AppUtil.ShowSuccess("Successfully Updated. ");
            table.draw();
            $("#mdlCompanyUpdate").modal('hide');
        }
        if (data.success === false) {
            if (data.AlreadyInsert === true) {
                AppUtil.ShowSuccess("Company already exist. Choose different Name.");
            } else {
                AppUtil.ShowSuccess("Update Failed.");
            }
        }
    },
    UpdateCompanyFromPopUpFail: function () {
        AppUtil.ShowSuccess("Error! Contact with Administator")
    },



    DeleteCompanyFromPopUp: function (_CompanyID) {

        var url = "/Company/CompanyDelete/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ CompanyID: _CompanyID });
        data = CompanyManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, CompanyManager.DeleteCompanyFromPopUpSuccess, CompanyManager.DeleteCompanyFromPopUpFailed);
    },
    DeleteCompanyFromPopUpSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Successfully Deleted!");
            table.draw();
        }
        if (data.success === false) {
            AppUtil.ShowSuccess("Failed to delete!");
        }
        $("#mdlCompanyDelete").modal("hide");

    },
    DeleteCompanyFromPopUpFailed: function (data) {
        AppUtil.ShowSuccess("Error! Contact with Administator")
    },


    GetCompanyVsStaffByID: function (_CompanyID) {

        var url = "/Company/GetStaffListForSpecificCompanyByID/";
        var data = ({ CompanyID: _CompanyID });
        data = CompanyManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, CompanyManager.GetGetCompanyVsStaffByIDSuccess, CompanyManager.GetGetCompanyVsStaffByIDFailed);
    },
    GetGetCompanyVsStaffByIDSuccess: function (data) {
        if (data.success === true) {
           
            $("#mdlCompanyVsStaff").modal("show");
        }
    },
    GetGetCompanyVsStaffByIDFailed: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
    },


    clearForSaveCompanyInformation: function () {
        $("#CreateCompanyName").val("");
        $("#CreatePostCode").val("");
        $("#CreateCity").val("");
        $("#CreateCountryID").val("");
        $("#CreateAddress").val("");
        $("#CreateDetails").val("");
        $("#CreateTag").val("");
        $("#PreviewCompanyCreateImage").attr("src", "");
        $("#CompanyCreateImage").wrap('<form>').closest('form').get(0).reset();
        $("#CompanyCreateImage").unwrap();
    },
    clearForUpdateCompanyInformation: function () {
        $("#UpdateCompanyName").val("");
        $("#UpdatePostCode").val("");
        $("#UpdateAddress").val("");
        $("#UpdateCity").val("");
        $("#UpdateCountryID").val("");
        $("#UpdateDetails").val("");
        $("#UpdateTag").val("");
        $("#PreviewCompanyUpdateImage").attr("src", "");
        $("#CompanyUpdateImage").wrap('<form>').closest('form').get(0).reset();
        $("#CompanyUpdateImage").unwrap();
    },
}