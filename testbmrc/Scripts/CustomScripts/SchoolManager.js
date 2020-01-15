var SchoolManager = {
    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {

        if (AppUtil.GetIdValue("SchoolName") === '') {
            AppUtil.ShowSuccess("Please write down School name.");
            return false;
        }
        if (AppUtil.GetIdValue("Email") === '') {
            AppUtil.ShowSuccess("Please write Email.");
            return false;
        }
        if (AppUtil.GetIdValue("Phone") === '') {
            AppUtil.ShowSuccess("Please write Phone");
            return false;
        }
        if (AppUtil.GetIdValue("CourseStructure") === '') {
            AppUtil.ShowSuccess("Please Select Course Structure.");
            return false;
        }
        if (AppUtil.GetIdValue("ApplicationPA") === '') {
            AppUtil.ShowSuccess("Please insert ApplicationPA.");
            return false;
        }
        if (AppUtil.GetIdValue("AcceptedPA") === '') {
            AppUtil.ShowSuccess("Please insert AcceptedPA.");
            return false;
        }
        if (AppUtil.GetIdValue("SuccessRatio") === '') {
            AppUtil.ShowSuccess("Please insert success ratio.");
            return false;
        }
        if (AppUtil.GetIdValue("IntercalatedBSc") === '') {
            AppUtil.ShowSuccess("Please Select Intercalated BSc.");
            return false;
        }
        if (AppUtil.GetIdValue("GCSESubjectsRequired") === '') {
            AppUtil.ShowSuccess("Please  insert GCSE Subjects Required.");
            return false;
        }
        if (AppUtil.GetIdValue("A_LevelSubjectsRequired") === '') {
            AppUtil.ShowSuccess("Please insert A-Level Subjects Required.");
            return false;
        }
        if (AppUtil.GetIdValue("A_LevelGradesRequired") === '') {
            AppUtil.ShowSuccess("Please A Level Grades Required.");
            return false;
        }
        if (AppUtil.GetIdValue("InterviewStyle") === '') {
            AppUtil.ShowSuccess("Please Select Interview Style.");
            return false;
        }
        if (AppUtil.GetIdValue("Fees_EUStudents") === '') {
            AppUtil.ShowSuccess("Please Insert EU Student fees.");
            return false;
        }
        if (AppUtil.GetIdValue("Fees_NonEUStudents") === '') {
            AppUtil.ShowSuccess("Please Insert EU Student fees.");
            return false;
        }

        return true;
    },
    SearchValidation: function () {
        if (AppUtil.GetIdValue("SchoolDetails") === '' && AppUtil.GetIdValue("StartDateID") === '' && AppUtil.GetIdValue("EndDateID") === '') {
            AppUtil.ShowSuccess("Please Select Some Criteria for Search.");
            return false;
        }
        return true;

    },
    SearchValidationForSchoolDetailsStaff: function () {
        if (AppUtil.GetIdValue("StartDateID") === '' && AppUtil.GetIdValue("EndDateID") === '') {
            AppUtil.ShowSuccess("Please Select Some Criteria for Search.");
            return false;
        }
        return true;

    },

    InsertSchool: function () {

        var url = "/School/AddOrUpdateSchoolDetails";
        var SchoolID = AppUtil.GetIdValue("SchoolID");
        var SchoolName = AppUtil.GetIdValue("SchoolName");
        var Email = AppUtil.GetIdValue("Email");
        var Phone = AppUtil.GetIdValue("Phone");
        var CourseStructure = AppUtil.GetIdValue("CourseStructure");
        var ApplicationPA = AppUtil.GetIdValue("ApplicationPA");
        var AcceptedPA = AppUtil.GetIdValue("AcceptedPA");
        var SuccessRatio = AppUtil.GetIdValue("SuccessRatio");
        var IntercalatedBSc = AppUtil.GetIdValue("IntercalatedBSc");
        var GCSESubjectsRequired = AppUtil.GetIdValue("GCSESubjectsRequired");
        var A_LevelSubjectsRequired = AppUtil.GetIdValue("A_LevelSubjectsRequired");
        var A_LevelGradesRequired = AppUtil.GetIdValue("A_LevelGradesRequired");
        var ScottishHighersSubjectsRequired = AppUtil.GetIdValue("ScottishHighersSubjectsRequired");
        var ScottishHigherGradesRequired = AppUtil.GetIdValue("ScottishHigherGradesRequired");
        var ScottishAdvancedHighersSubjectsRequired = AppUtil.GetIdValue("ScottishAdvancedHighersSubjectsRequired");
        var ScottishAdvancedHighersGradesRequired = AppUtil.GetIdValue("ScottishAdvancedHighersGradesRequired");
        var IBSubjectsRequired = AppUtil.GetIdValue("IBSubjectsRequired");
        var IBGradesRequired = AppUtil.GetIdValue("IBGradesRequired");
        var HowisUCATused = AppUtil.GetIdValue("HowisUCATused");
        var HowisBMATUsed = AppUtil.GetIdValue("HowisBMATUsed");
        var InterviewStyle = AppUtil.GetIdValue("InterviewStyle");
        var Fees_EUStudents = AppUtil.GetIdValue("Fees_EUStudents");
        var Fees_NonEUStudents = AppUtil.GetIdValue("Fees_NonEUStudents");

        var UCATRequired = $("#UCATRequired").is(":checked") ? true : false;
        var BMATRequired = $("#BMATRequired").is(":checked") ? true : false;
        var IsGraduateEntryAvailable = $("#IsGraduateEntryAvailable").is(":checked") ? true : false;
        var FoundationOrAccessCoursesAvailable = $("#FoundationOrAccessCoursesAvailable").is(":checked") ? true : false;

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var Header = {};
        Header['__RequestVerificationToken'] = AntiForgeryToken;
        var SchoolDetails = {
            SchoolID: SchoolID, SchoolName: SchoolName, Email: Email,
            Phone: Phone, CourseStructure: CourseStructure, ApplicationPA: ApplicationPA,
            AcceptedPA: AcceptedPA, SuccessRatio: SuccessRatio, IntercalatedBSc: IntercalatedBSc,
            A_LevelSubjectsRequired: A_LevelSubjectsRequired, A_LevelGradesRequired: A_LevelGradesRequired, A_LevelGradesRequired: A_LevelGradesRequired,
            ScottishHighersSubjectsRequired: ScottishHighersSubjectsRequired, ScottishHigherGradesRequired: ScottishHigherGradesRequired, ScottishAdvancedHighersSubjectsRequired: ScottishAdvancedHighersSubjectsRequired,
            ScottishAdvancedHighersGradesRequired: ScottishAdvancedHighersGradesRequired, IBSubjectsRequired: IBSubjectsRequired, IBGradesRequired: IBGradesRequired,
            HowisUCATused: HowisUCATused, HowisBMATUsed: HowisBMATUsed,
            InterviewStyle: InterviewStyle, Fees_EUStudents: Fees_EUStudents, Fees_NonEUStudents: Fees_NonEUStudents,
            GCSESubjectsRequired: GCSESubjectsRequired, FoundationOrAccessCoursesAvailable: FoundationOrAccessCoursesAvailable, UCATRequired: UCATRequired,
            BMATRequired: BMATRequired, IsGraduateEntryAvailable: IsGraduateEntryAvailable,
        };

        var data = JSON.stringify({ SchoolDetails: SchoolDetails });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, Header, SchoolManager.InsertSchoolSuccess, SchoolManager.InsertSchoolFail);

    },
    InsertSchoolSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            window.location.href = "/School/SchoolDetails/";
            SchoolManager.clearForSaveSchoolDetailsInformation();
        }
        if (data.success === false) {
            AppUtil.ShowSuccess("Saved Failed.");
        }

    },
    InsertSchoolFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
    },

    DeleteSchool: function (_SchoolID) {

        var url = "/School/SchoolDelete/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ SchoolID: _SchoolID });
        data = SchoolManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, SchoolManager.DeleteSchoolSuccess, SchoolManager.DeleteSchoolFailed);
    },
    DeleteSchoolSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Successfully Deleted!");
            table.draw();
        }
        if (data.success === false) {
            AppUtil.ShowSuccess("Failed to delete!");
        }
        $("#mdlSchoolDelete").modal("hide");

    },
    DeleteSchoolFailed: function (data) {
        AppUtil.ShowSuccess("Error! Contact with Administator")
    },
    
    ShowDetailsByID: function (_SchoolID) {

        var url = "/School/ShowSchoolInformationByID/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ SchoolID: _SchoolID });
        data = SchoolManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, SchoolManager.ShowDetailsByIDSuccess, SchoolManager.ShowDetailsByIDFailed);
    },
    ShowDetailsByIDSuccess: function (data) {
        $("#SchoolName").val(data.SchoolDetails.SchoolName);
        $("#Email").val(data.SchoolDetails.Email);
        $("#Phone").val(data.SchoolDetails.Phone);
        $("#CourseStructure").val(data.SchoolDetails.CourseStructure);
        $("#ApplicationPA").val(data.SchoolDetails.ApplicationPA);
        $("#AcceptedPA").val(data.SchoolDetails.AcceptedPA);
        $("#SuccessRatio").val(data.SchoolDetails.SuccessRatio);
        $("#IntercalatedBSc").val(data.SchoolDetails.IntercalatedBSc);
        $("#GCSESubjectsRequired").val(data.SchoolDetails.GCSESubjectsRequired);
        $("#A_LevelSubjectsRequired").val(data.SchoolDetails.A_LevelSubjectsRequired);
        $("#A_LevelGradesRequired").val(data.SchoolDetails.A_LevelGradesRequired);
        $("#ScottishHighersSubjectsRequired").val(data.SchoolDetails.ScottishHighersSubjectsRequired);
        $("#ScottishHigherGradesRequired").val(data.SchoolDetails.ScottishHigherGradesRequired);
        $("#ScottishAdvancedHighersSubjectsRequired").val(data.SchoolDetails.ScottishAdvancedHighersSubjectsRequired);
        $("#ScottishAdvancedHighersGradesRequired").val(data.SchoolDetails.ScottishAdvancedHighersGradesRequired);
        $("#IBSubjectsRequired").val(data.SchoolDetails.IBSubjectsRequired);
        $("#IBGradesRequired").val(data.SchoolDetails.IBGradesRequired);
        $("#HowisUCATused").val(data.SchoolDetails.HowisUCATused);
        $("#HowisBMATUsed").val(data.SchoolDetails.HowisBMATUsed);
        $("#InterviewStyle").val(data.SchoolDetails.InterviewStyle);
        $("#Fees_EUStudents").val(data.SchoolDetails.Fees_EUStudents);
        $("#Fees_NonEUStudents").val(data.SchoolDetails.Fees_NonEUStudents);

        if (data.SchoolDetails.BMATRequired === true) {
            $("#BMATRequired").prop("checked", true);
        }
        if (data.SchoolDetails.UCATRequired === true) {
            $("#UCATRequired").prop("checked", true);
        }
        if (data.SchoolDetails.FoundationOrAccessCoursesAvailable === true) {
            $("#FoundationOrAccessCoursesAvailable").prop("checked", true);
        }
        if (data.SchoolDetails.IsGraduateEntryAvailable === true) {
            $("#GraduateEntryAvailable").prop("checked", true);
        }
        $("#mdlSchoolDetails").modal("show");

    },
    ShowDetailsByIDFailed: function (data) {
        AppUtil.ShowSuccess("Error! Contact with Administator")
    },

    clearForSaveSchoolDetailsInformation: function () {
        $("#SchoolID").val("");
        $("#SchoolName").val("");
        $("#Email").val("");
        $("#Phone").val("");
        $("#CourseStructure").val("");
        $("#ApplicationPA").val("");
        $("#AcceptedPA").val("");
        $("#SuccessRatio").val("");
        $("#IntercalatedBSc").val("");
        $("#GCSESubjectsRequired").val("");
        $("#A_LevelSubjectsRequired").val("");
        $("#A_LevelGradesRequired").val("");
        $("#ScottishHighersSubjectsRequired").val("");
        $("#ScottishHigherGradesRequired").val("");
        $("#ScottishAdvancedHighersSubjectsRequired").val("");
        $("#ScottishAdvancedHighersGradesRequired").val("");
        $("#IBSubjectsRequired").val("");
        $("#IBGradesRequired").val("");
        $("#HowisUCATused").val("");
        $("#HoisBMATUsed").val("");
        $("#InterviewStyle").val("");
        $("#Fees_EUStudents").val("");
        $("#Fees_NonEUStudents").val("");
        $("#UCATRequired").prop("checked", false);
        $("#BMATRequired").prop("checked", false);
        $("#IsGraduateEntryAvailable").prop("checked", false);
        $("#FoundationOrAccessCoursesAvailable").prop("checked", false);
    },
}