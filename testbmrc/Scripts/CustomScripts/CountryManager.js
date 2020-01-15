var CountryManager = {

    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },




    Validation: function () {

        if (AppUtil.GetIdValue("UpdateCountryName") === '') {
            AppUtil.ShowSuccess("Please insert category name.");
            return false;
        }
        return true;
    },
    CreateValidation: function () {

        if (AppUtil.GetIdValue("CreateCountryName") === '') {
            AppUtil.ShowSuccess("Please Insert Call Category Name.");
            return false;
        }
        return true;
    },

    InsertCountryFromPopUp: function () {

        var url = "/Country/InsertCountryFromPopUp/";
        var CountryName = AppUtil.GetIdValue("CreateCountryName");

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var Header = {};
        Header['__RequestVerificationToken'] = AntiForgeryToken;


        var Country = { CountryName: CountryName };
        var data = JSON.stringify({ Country: Country });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, Header, CountryManager.InsertCountryFromPopUpSuccess, CountryManager.InsertCountryFromPopUpFail);

    },
    InsertCountryFromPopUpSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Saved Successfully.");

        }
        if (data.success === false) {
            AppUtil.ShowSuccess("Saved Failed.");
        }
        CountryManager.clearForSaveInformation();
        $("#mdlCountryInsert").modal("hide");
        table.draw();
    },
    InsertCountryFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },

    EditCountryGet: function (_CountryID) {

        var url = "/Country/GetCountryDetailsByID/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var Header = {};
        Header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ CountryID: _CountryID });
        datas = CountryManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, CountryManager.EditCountryGetsuccess, CountryManager.EditCountryGetFail);

    },
    EditCountryGetsuccess: function (data) {
        $("#UpdateCountryName").val(data.CountryInfo.CountryName);
        $("#mdlCountryUpdate").modal("show");

    },
    EditCountryGetFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");

    },


    EditCountryFromPopUp: function (_CountryID) {
        var url = "/Country/UpdateCountry/";
        var CountryID = _CountryID;
        var CountryName = AppUtil.GetIdValue("UpdateCountryName");
        var Country = { CountryID: CountryID, CountryName: CountryName };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var Header = {};
        Header['__RequestVerificationToken'] = AntiForgeryToken;

        data = JSON.stringify({ Country: Country });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, Header, CountryManager.EditCountryFromPopUpSuccess, CountryManager.EditCountryFromPopUpFail);


    },
    EditCountryFromPopUpSuccess: function (data) {

        if (data.success === true) {

            AppUtil.ShowSuccess("Successfully Edited");
        }
        else {
            AppUtil.ShowError("Save failed");
        }
        CountryManager.clearForUpdateInformation();
        $("#mdlCountryUpdate").modal("hide");
        table.draw();
    },
    EditCountryFromPopUpFail: function (data) {
        alert("Error Occured. Contact With Admninstrator. ");
    },

    DeleteCountry: function (_CountryID) {

        var url = "/Country/DeleteCountry/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var Header = {};
        Header['__RequestVerificationToken'] = AntiForgeryToken;
        var datas = ({ CountryID: _CountryID });
        datas = CountryManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, CountryManager.DeleteCountrysuccess, CountryManager.DeleteCountryFail);

    },
    DeleteCountrysuccess: function (data) {
        if (data.DeleteSuccess == true) {
            AppUtil.ShowSuccess("Successfully Deleted!");

        }
        $("#mdlCountryDelete").modal("hide");
        table.draw();
    },
    DeleteCountryFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },



    clearForSaveInformation: function () {
        $("#CreateCountryName").val("");
    },
    clearForUpdateInformation: function () {
        $("#UpdateCountryName").val("");
    },
}