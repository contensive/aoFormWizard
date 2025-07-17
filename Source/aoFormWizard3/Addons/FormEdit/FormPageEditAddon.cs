using Contensive.Addon.aoFormWizard3.Controllers;
using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.Addon.aoFormWizard3.Models.Domain;
using Contensive.BaseClasses;
using Contensive.BaseClasses.LayoutBuilder;
using Contensive.Models.Db;
using System;

namespace Contensive.Addon.aoFormWizard3.Addons.WidgetDashboardWidgets {
    //
    // ========================================================================================
    /// <summary>
    /// Meetings
    /// </summary>
    /// <remarks></remarks>
    public class FormPageEditAddon : AddonBaseClass {
        //
        public const string guidPortalFeature = "{0358AA13-2C29-4F82-B2DF-78D89E7DAE6A}";
        public const string guidAddon = "{4C25B351-C03B-4235-B3CA-094CDDC70430}";
        // 
        // =====================================================================================
        /// <summary>
        /// Addon interface
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public override object Execute(CPBaseClass cp) {
            try {
                // 
                // -- authenticate/authorize
                if (!cp.User.IsAdmin) { return SecurityController.getNotAuthorizedHtmlResponse(cp); }
                // 
                // -- validate portal environment
                if (!cp.AdminUI.EndpointContainsPortal()) { return cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, guidPortalFeature, ""); }
                // 
                string errorMessage = "";
                var request = new RequestModel(cp);
                using (var app = new ApplicationModel(cp)) {
                    processView(app, request, ref errorMessage);
                    return getView(app, request);
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
        // 
        // =====================================================================================
        //
        public static string getView(ApplicationModel app, RequestModel request) {
            CPBaseClass cp = app.cp;
            try {
                var layoutBuilder = cp.AdminUI.CreateLayoutBuilderNameValue();
                //
                FormPageModel record = DbBaseModel.create<FormPageModel>(cp, request.formPageId);
                //
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Name";
                layoutBuilder.rowValue = cp.Html5.InputText(Constants.rnName, 255, record?.name ?? "", "form-control");
                layoutBuilder.rowHelp = "The name for this form page used by administrators to manage the form. The user will not see this name.";
                //
                layoutBuilder.title = (record == null) ? "Add Form Page" : "Edit Form Page";
                layoutBuilder.portalSubNavTitle = (record == null) ? "Add Form Page" : record.name;
                layoutBuilder.description = "A form page is one page of questions a user see when submitting a form online. A form can have one or more form pages. Each form page contains one or more form questions.";
                layoutBuilder.callbackAddonGuid = guidAddon;
                // 
                // -- add buttons
                layoutBuilder.addFormButton(Constants.buttonOK);
                layoutBuilder.addFormButton(Constants.buttonSave);
                layoutBuilder.addFormButton(Constants.ButtonCancel);
                // 
                // -- add hiddens
                //
                // -- feature subnav link querystring - clicks must include these values
                cp.Doc.AddRefreshQueryString(Constants.rnFormWidgetId, request.formWidgetId);
                //
                return layoutBuilder.getHtml();
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// Process the view
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="request"></param>
        /// <param name="errorMessage"></param>
        public static void processView(ApplicationModel app, RequestModel request, ref string errorMessage) {
            CPBaseClass cp = app.cp;
            try {
                switch (request.button ?? "") {
                    case Constants.buttonSave: {
                            saveFormPage(cp, request);
                            return;
                        }
                    case Constants.buttonOK: {
                            saveFormPage(cp, request);
                            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormWidgetListAddon.guidPortalFeature, "");
                            return;
                        }
                    case Constants.buttonDelete: {
                            foreach (var formQuestion in DbBaseModel.createList<FormQuestionModel>(cp, $"formid={request.formPageId}")) {
                                DbBaseModel.delete<FormQuestionModel>(cp, formQuestion.id);
                            }
                            DbBaseModel.delete<FormPageModel>(cp, request.formPageId);
                            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormWidgetListAddon.guidPortalFeature, "");
                            return;
                        }
                    case Constants.ButtonCancel: {
                            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormWidgetListAddon.guidPortalFeature, "");
                            return;
                        }
                    default: {
                            return;
                        }
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
        // 
        // ====================================================================================================
        // 
        private static void saveFormPage(CPBaseClass cp, RequestModel request) {
            try {
                var formPage = DbBaseModel.create<FormPageModel>(cp, request.formPageId);
                if (formPage is null) {
                    formPage = DbBaseModel.addDefault<FormPageModel>(cp);
                }
                formPage.name = request.name;
                formPage.save(cp);
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
        }
        // 
        // ====================================================================================================
        //
        public class RequestModel {
            //
            public RequestModel(CPBaseClass cp) {
                this.cp = cp;
                //
                // -- implement the remove-filter feature
                string removeFilter = cp.Doc.GetText(Constants.rnRemoveFilter);
                if (!string.IsNullOrEmpty(removeFilter)) { cp.Doc.SetProperty(removeFilter, ""); }
                //
                // -- initialize properties (cannot use default constructor)
                button = cp.Doc.GetText(Constants.rnButton);
                formWidgetId = cp.Doc.GetInteger(Constants.rnFormWidgetId);
                formPageId = cp.Doc.GetInteger(Constants.rnFormPageId);
                //
                // -- individual fields for the record, request name and requestModel name match the field name (except id)
                name = cp.Doc.GetText("name");
            }
            private CPBaseClass cp;
            //
            public string button { get; }
            //
            public int formWidgetId { get; set; }
            //
            public int formPageId { get; set; }
            //
            public string name { get; set; }
        }
    }
}