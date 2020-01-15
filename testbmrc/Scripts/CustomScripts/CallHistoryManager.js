var CallHistoryManager = {
    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {

        if (AppUtil.GetIdValue("CallerName") === '') {
            AppUtil.ShowSuccess("Please write down caller name.");
            return false;
        }
        if (AppUtil.GetIdValue("CallerPhone") === '') {
            AppUtil.ShowSuccess("Please write caller phone.");
            return false;
        }
        if (AppUtil.GetIdValue("CallerEmail") === '') {
            AppUtil.ShowSuccess("Please write caller email");
            return false;
        }
        if (AppUtil.GetIdValue("CountryID") === '') {
            AppUtil.ShowSuccess("Please Select Country.");
            return false;
        }
        if (AppUtil.GetIdValue("CallerAddress") === '') {
            AppUtil.ShowSuccess("Please insert Address.");
            return false;
        }
        if (AppUtil.GetIdValue("CompanyID") === '') {
            AppUtil.ShowSuccess("Please Select Company.");
            return false;
        }
        if (AppUtil.GetIdValue("CompanyStaffID") === '') {
            AppUtil.ShowSuccess("Please Select Company Staff.");
            return false;
        }
        if (AppUtil.GetIdValue("Subect") === '') {
            AppUtil.ShowSuccess("Please Wirte Subject.");
            return false;
        }
        if (AppUtil.GetIdValue("CallCategoryID") === '') {
            AppUtil.ShowSuccess("Please Select Call Category.");
            return false;
        }
        if (AppUtil.GetIdValue("Description") === '') {
            AppUtil.ShowSuccess("Please Write Description.");
            return false;
        }

        return true;
    },
    SearchValidation: function () {
        if (AppUtil.GetIdValue("Company") === '' && AppUtil.GetIdValue("StartDateID") === '' && AppUtil.GetIdValue("EndDateID") === '') {
            AppUtil.ShowSuccess("Please Select Some Criteria for Search.");
            return false;
        }
        return true;

    },
    SearchValidationForCompanyStaff: function () {
        if (AppUtil.GetIdValue("StartDateID") === '' && AppUtil.GetIdValue("EndDateID") === '') {
            AppUtil.ShowSuccess("Please Select Some Criteria for Search.");
            return false;
        }
        return true;

    },


    InsertCallHistory: function () {

        var url = "/CallHistory/CallHistoryDetailsInsert";
        var CallHistoryID = AppUtil.GetIdValue("CallHistoryID");
        var CallerName = AppUtil.GetIdValue("CallerName");
        var CallerPhone = AppUtil.GetIdValue("CallerPhone");
        var CallerEmail = AppUtil.GetIdValue("CallerEmail");
        var CountryID = AppUtil.GetIdValue("CountryID");
        var CallerAddress = AppUtil.GetIdValue("CallerAddress");
        var CompanyID = AppUtil.GetIdValue("CompanyID");
        var CompanyVsStaffID = AppUtil.GetIdValue("CompanyStaffID");
        var Subject = AppUtil.GetIdValue("Subect");
        var CallCategoryID = AppUtil.GetIdValue("CallCategoryID");
        var Description = AppUtil.GetIdValue("Description");

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var Header = {};
        Header['__RequestVerificationToken'] = AntiForgeryToken;
        var CallHistoryDetails = { CallHistoryID: CallHistoryID, CallerName: CallerName, CallerPhone: CallerPhone, CallerEmail: CallerEmail, CountryID: CountryID, CallerAddress: CallerAddress, CompanyID: CompanyID, CompanyVsStaffID: CompanyVsStaffID, Subject: Subject, CallCategoryID: CallCategoryID, Description: Description };

        var data = JSON.stringify({ CallHistoryDetails: CallHistoryDetails });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, Header, CallHistoryManager.InsertCallHistorySuccess, CallHistoryManager.InsertCallHistoryFail);

    },
    InsertCallHistorySuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            window.location.href = "/CallHistory/Index/";
            CallHistoryManager.clearForSaveCompanyInformation();
        }
        if (data.success === false) {
            AppUtil.ShowSuccess("Saved Failed.");
        }

    },
    InsertCallHistoryFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
    },

    GetEmployeeDetailsByID: function (_EmpID) {

        var url = "/CallHistory/GetEmployeeDetailsByID/";
        var data = ({ EmpID: _EmpID });
        data = CallHistoryManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, CallHistoryManager.GetEmployeeDetailsByIDSuccess, CallHistoryManager.GetEmployeeDetailsByIDFailed);
    },
    GetEmployeeDetailsByIDSuccess: function (data) {
        if (data.success === true) {
            $('#MdlEmpDetails').html(
                '<div class="modal-header">' +
                '<button type="button" id="detailsMdlClose" class="close" data-dismiss="modal">&times;</button>' +
                '<h4 class="modal-title">' + data.employee.FirstName + " " + data.employee.LastName + '</h4>' +
                '</div>' +
                ' <div class="modal-body panel-body">' +
                '<div class="col-md-6">' +
                '<div class="img-bordered">'+
                '<img src=' + data.employee.EmployeeOwnImageBytesPaths + ' height="100" width="auto"/>' +
                '</div>' +
                '</div>' +
                '<div class="col-md-6">' +
                '<h3 >'+
                '<b>' + data.employee.FirstName + " " + data.employee.LastName + '</b>'+
                '</h3> ' +
                '<h4 >' + data.employee.Email+'</h4>' +
                '<h2>'+"Total:"+' ' + data.callCount + '</h2>' +
                '<p>'+"Call recieved"+'</p>'+
                '</div>' +
                '</div>'
            );
            $("#mdlEmployeeDetails").modal("show");
        }
    },
    GetEmployeeDetailsByIDFailed: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
    },



    DeleteCallHistory: function (_CallHistoryID) {

        var url = "/CallHistory/CallHistoryDelete/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ CallHistoryID: _CallHistoryID });
        data = CallHistoryManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, CallHistoryManager.DeleteCallHistorySuccess, CallHistoryManager.DeleteCallHistoryFailed);
    },
    DeleteCallHistorySuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Successfully Deleted!");
            table.draw();
        }
        if (data.success === false) {
            AppUtil.ShowSuccess("Failed to delete!");
        }
        $("#mdlCallHistoryDelete").modal("hide");

    },
    DeleteCallHistoryFailed: function (data) {
        AppUtil.ShowSuccess("Error! Contact with Administator")
    },


    clearForSaveCompanyInformation: function () {
        $("#CallerName").val("");
        $("#CallerPhone").val("");
        $("#CallerEmail").val("");
        $("#CallerAddress").val("");
        $("#CountryID").val("");
        $("#CompanyID").val("");
        $("#CompanyStaffID").val("");
        $("#Subect").val("");
        $("#CallCategoryID").val("");
        $("#Description").val("");
    },
}
