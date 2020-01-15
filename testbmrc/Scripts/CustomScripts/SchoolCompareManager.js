var SchoolCompareManager = {
    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },
    SearchValidation: function () {
        if (AppUtil.GetIdValue("School1") === '' && AppUtil.GetIdValue("School2") === '' && AppUtil.GetIdValue("School3") === '' && AppUtil.GetIdValue("School4") === '') {
            AppUtil.ShowSuccess("Please Select Some Criteria for Search.");
            return false;
        }
        return true;

    },

    SchoolComparision: function () {

        var url = "/School/SchoolCmparisionSuccess/";
        var School1 = AppUtil.GetIdValue("School1");
        var School2 = AppUtil.GetIdValue("School2");
        var School3 = AppUtil.GetIdValue("School3");
        var School4 = AppUtil.GetIdValue("School4");

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var Header = {};
        Header['__RequestVerificationToken'] = AntiForgeryToken;


        var SchoolIDS = { School1: School1, School2: School2, School3: School3, School4: School4 };
        var data = JSON.stringify({ SchoolIDS: SchoolIDS });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, Header, SchoolCompareManager.SchoolComparisionSuccess, SchoolCompareManager.SchoolComparisionFail);

    },
    SchoolComparisionSuccess: function (data) {
        $('#divSchoolCompareResult').html(data);
    },
    SchoolComparisionFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },


}