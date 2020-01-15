var CallCategoryManager = {

    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },




    Validation: function () {

        if (AppUtil.GetIdValue("UpdateCallCategoryName") === '') {
            AppUtil.ShowSuccess("Please insert category name.");
            return false;
        }
        return true;
    },
    CreateValidation: function () {

        if (AppUtil.GetIdValue("CreateCallCategoryName") === '') {
            AppUtil.ShowSuccess("Please Insert Call Category Name.");
            return false;
        }
        return true;
    },

    InsertCallCategoryFromPopUp: function () {

        var url = "/CallCategory/InsertCallCategoryFromPopUp/";
        var CallCategoryName = AppUtil.GetIdValue("CreateCallCategoryName");

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var Header = {};
        Header['__RequestVerificationToken'] = AntiForgeryToken;


        var CallCategory = { CallCategoryName: CallCategoryName};
        var data = JSON.stringify({ CallCategory: CallCategory });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, Header, CallCategoryManager.InsertCallCategoryFromPopUpSuccess, CallCategoryManager.InsertCallCategoryFromPopUpFail);

    },
    InsertCallCategoryFromPopUpSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            
        }
        if (data.success === false) {
            AppUtil.ShowSuccess("Saved Failed.");
        }
        CallCategoryManager.clearForSaveInformation();
        $("#mdlCallCategoryInsert").modal("hide");
        table.draw();
    },
    InsertCallCategoryFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },

    EditCallCategoryGet: function (_CallCategoryID) {

        var url = "/CallCategory/GetCallCategoryDetailsByID/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var Header = {};
        Header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ CallCategoryID: _CallCategoryID });
        datas = CallCategoryManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, CallCategoryManager.EditCallCategoryGetsuccess, CallCategoryManager.EditCallCategoryGetFail);

    },
    EditCallCategoryGetsuccess: function (data) {
        $("#UpdateCallCategoryName").val(data.CallCategoryInfo.CallCategoryName);
        $("#mdlCallCategoryUpdate").modal("show");

    },
    EditCallCategoryGetFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");

    },


    EditCallCategoryFromPopUp: function (_CallCategoryID) {
        var url = "/CallCategory/UpdateCallCategory/";
        var CallCategoryID = _CallCategoryID;
        var CallCategoryName = AppUtil.GetIdValue("UpdateCallCategoryName");
        var CallCategory = { CallCategoryID: CallCategoryID, CallCategoryName: CallCategoryName};

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var Header = {};
        Header['__RequestVerificationToken'] = AntiForgeryToken;

        data = JSON.stringify({ CallCategory: CallCategory });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, Header, CallCategoryManager.EditCallCategoryFromPopUpSuccess, CallCategoryManager.EditCallCategoryFromPopUpFail);


    },
    EditCallCategoryFromPopUpSuccess: function (data) {
        
        if (data.success === true) {

            //$("#tblCallCategory tbody>tr").each(function () {

            //    var CallCategoryID = $(this).find("td:eq(0) input").val();
            //    var index = $(this).index();
            //    $("#tblCallCategory tbody>tr:eq(" + index + ")").find("td:eq(1)").text(CallCategoryInfo.CallCategoryName);
            //    $("#tblCallCategory tbody>tr:eq(" + index + ")").find("td:eq(2)").text(CallCategoryType);

            //});

            AppUtil.ShowSuccess("Successfully Edited");
        }
        else {
            AppUtil.ShowError("Save failed");
        }
        CallCategoryManager.clearForUpdateInformation();
        $("#mdlCallCategoryUpdate").modal("hide");
        table.draw();
    },
    EditCallCategoryFromPopUpFail: function (data) {
        alert("Error Occured. Contact With Admninstrator. ");
    },

    DeleteCallCategory: function (_CallCategoryID) {

        var url = "/CallCategory/DeleteCallCategory/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var Header = {};
        Header['__RequestVerificationToken'] = AntiForgeryToken;
        var datas = ({ CallCategoryID: _CallCategoryID });
        datas = CallCategoryManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, CallCategoryManager.DeleteCallCategorysuccess, CallCategoryManager.DeleteCallCategoryFail);

    },
    DeleteCallCategorysuccess: function (data) {
        if (data.DeleteSuccess == true) {
            AppUtil.ShowSuccess("Successfully Deleted!");
            
        }
        $("#mdlCallCategoryDelete").modal("hide");
        table.draw();
    },
    DeleteCallCategoryFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },



    clearForSaveInformation: function () {
        $("#CreateCallCategoryName").val("");
    },
    clearForUpdateInformation: function () {
        $("#UpdateCallCategoryName").val("");
    },
}