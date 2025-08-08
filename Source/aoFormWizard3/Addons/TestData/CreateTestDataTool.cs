using Contensive.Addon.aoFormWizard3.Addons.WidgetDashboardWidgets;
using Contensive.Addon.aoFormWizard3.Controllers;
using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.Addon.aoFormWizard3.Models.Domain;
using Contensive.Addon.aoFormWizard3.Views;
using Contensive.BaseClasses;
using Contensive.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contensive.Addon.aoFormWizard3.Addons {
    public class CreateTestDataTool : AddonBaseClass {
        //
        public const string guidPortalFeature = "";
        public const string guidAddon = "";
        //
        public const int indent = 20; // px indent for each line of output
        //
        // -- digits 1   Form Widget Name
        // -- digits 2   number of Form Pages in the form widget
        // -- difits 3-4 number of Form Questions on each form page, 0-99
        // -- digits 5-7 number of responses to each for widget, 0-999
        //
        public string[] DataGuids = [
            "{A1050100-0000-0000-0000-00000000000}",
            "{B3109990-0000-0000-0000-00000000000}",
            "{C3109990-0000-0000-0000-00000000000}",
            "{D3109990-0000-0000-0000-00000000000}",
            "{E3109990-0000-0000-0000-00000000000}",
            "{F3109990-0000-0000-0000-00000000000}",
            ];
        // 
        // =====================================================================================
        //
        public override object Execute(CPBaseClass cp) {
            try {
                // 
                // -- authenticate/authorize
                if (!cp.User.IsAdmin) { return SecurityController.getNotAuthorizedHtmlResponse(cp); }
                // 
                // -- only run in test environment
                if (cp.ServerConfig.productionEnvironment) { return "<p>This tool is disabled because the server is a production server</p>"; }
                // 
                // -- Go To Forms Manager
                if (cp.Doc.GetText("button").Equals("Go To Forms Manager")) { return cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormWidgetListAddon.guidPortalFeature); }
                // 
                // -- cancel
                if (cp.Doc.GetText("button").Equals(Constants.ButtonCancel)) { return cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, ""); }
                // 
                using (var app = new ApplicationModel(cp)) {
                    var layout = app.cp.AdminUI.CreateLayoutBuilder();
                    //
                    if (app.cp.Doc.GetText("button") == "Create Test Data") {
                        layout.body += createTestData(app);
                    }
                    layout.title = "Create Forms Manager Test Data Tool";
                    layout.description = "Create or Repair Forms Manager Test dataset.";
                    //
                    layout.addFormButton("Cancel");
                    layout.addFormButton("Go To Forms Manager");
                    layout.addFormButton("Create Test Data");
                    return layout.getHtml();
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
        //
        // =====================================================================================
        //
        public string createTestData(ApplicationModel app) {
            int margin = 10;
            string body = "";
            //
            // -- create data
            foreach (string dataGuid in DataGuids) {
                body += createFormWidgets(app, margin, dataGuid);
                //body += createFormResponses(app, margin, dataGuid);
            }
            return body;
        }
        //
        // =====================================================================================
        //
        public string createFormWidgets(ApplicationModel app, int margin, string dataGuid) {
            CPBaseClass cp = app.cp;
            //
            string nameSuffix = dataGuid.Substring(1, 1);
            //
            string result = "";
            FormWidgetModel formWidget = DbBaseModel.create<FormWidgetModel>(app.cp, dataGuid);
            if (formWidget == null) {
                formWidget = DbBaseModel.addDefault<FormWidgetModel>(app.cp);
                formWidget.ccguid = dataGuid;
            }
            //
            app.cp.Db.ExecuteNonQuery($"delete from ccFormResponse where formwidget={formWidget.id}");
            //
            formWidget.name = $"form {nameSuffix}";
            result += $"<div style=\"margin-left:{margin}px\">Form Widget: {formWidget.name}</div>";
            //
            formWidget.modifiedDate = DateTime.Now;
            //
            formWidget.save(app.cp);
            //
            // -- form pages, digit 5
            for (int index = 0; index < app.cp.Utils.EncodeInteger(dataGuid.Substring(2, 1)); index++) {
                int formPageId = 0;
                result += createFormPages(app, margin + indent, dataGuid, formWidget, ref formPageId, index, nameSuffix);
            }
            //
            return result;
        }
        //
        // =====================================================================================
        //
        public string createFormPages(ApplicationModel app, int margin, string dataGuid, FormWidgetModel formWidget, ref int formPageId, int index, string nameSuffix) {
            string result = "";
            var formPage = DbBaseModel.create<FormPageModel>(app.cp, $"{formWidget.ccguid}-{index}");
            if (formPage == null) {
                formPage = DbBaseModel.addDefault<FormPageModel>(app.cp);
                formPage.ccguid = $"{formWidget.ccguid}-{index}";
            }
            formPage.name = $"formPage-{index}-{nameSuffix}";
            formPage.formsetid = formWidget.id;
            //
            //
            formPage.save(app.cp);
            formPageId = formPage.id;
            //
            result += $"<div style=\"margin-left:{margin}px\">form-page: {formPage.name}</div>";
            //
            // -- form page questions, digit 6-8
            for (int i = 0; i < app.cp.Utils.EncodeInteger(dataGuid.Substring(3, 2)); i++) {
                int formQuestionId = 0;
                result += createFormQuestions(app, margin + indent, dataGuid, formWidget, formPage, ref formQuestionId, i, nameSuffix);
            }
            //
            return result;
        }
        //
        // =====================================================================================
        //
        public string createFormQuestions(ApplicationModel app, int margin, string dataGuid, FormWidgetModel formWidget, FormPageModel formPage, ref int formQuestionId, int index, string nameSuffix) {
            string result = "";
            var formQuestion = DbBaseModel.create<FormQuestionModel>(app.cp, $"{formWidget.ccguid}-{index}");
            if (formQuestion == null) {
                formQuestion = DbBaseModel.addDefault<FormQuestionModel>(app.cp);
                formQuestion.ccguid = $"{formWidget.ccguid}-{index}";
            }
            formQuestion.name = $"formQuestion-{index}-{nameSuffix}";
            formQuestion.formid = formPage.id;
            //
            //
            formQuestion.save(app.cp);
            formQuestionId = formQuestion.id;
            //
            result += $"<div style=\"margin-left:{margin}px\">form-page: {formQuestion.name}</div>";
            //
            return result;
        }






        ////
        //// =====================================================================================
        ////
        //public string createFormResponses(ApplicationModel app, int margin, string meetingGuid) {
        //    StringBuilder result = new StringBuilder();
        //    FormWidgetModel meeting = DbBaseModel.create<FormWidgetModel>(app.cp, meetingGuid);
        //    //
        //    if (meeting == null) {
        //        return "<div style=\"margin-left:" + margin + "px\">Can not add attendees because meeting not valid</div>";
        //    }
        //    //
        //    if (meeting.dateend < DateTime.Now) {
        //        return "<div style=\"margin-left:" + margin + "px\">Meeting registration date past</div>";
        //    }
        //    List<MeetingAttendeeTypeModel> MeetingAttendeeTypeList = DbBaseModel.createList<MeetingAttendeeTypeModel>(app.cp, $"meetingid={meeting.id}");
        //    for (int regIndex = 0; regIndex < app.cp.Utils.EncodeInteger(meetingGuid.Substring(6, 3)); regIndex++) {
        //        //
        //        // -- the registrant user
        //        createRegistration(app, margin, result, meeting, regIndex, MeetingAttendeeTypeList);
        //    }
        //    return result.ToString();
        //}
    }
}
