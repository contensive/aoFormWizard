using Contensive.Addon.aoFormWizard3.Controllers;
using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.Addon.aoFormWizard3.Models.Domain;
using Contensive.BaseClasses;
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
                using (var app = new ApplicationModel(cp)) {
                    var request = new RequestModel(cp);
                    string userErrorMessage = "";
                    if (!processView(app, request, ref userErrorMessage)) { return ""; }
                    return getView(app, request, userErrorMessage);
                }
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
        /// <param name="app"></param>
        /// <param name="cp"></param>
        /// <param name="request"></param>
        /// <param name="errorMessage"></param>
        public static bool processView(ApplicationModel app, RequestModel request, ref string errorMessage) {
            CPBaseClass cp = app.cp;
            try {
                //
                // -- form widget required, else redirect to form widget list
                if (request.formId <= 0) {
                    cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormPageListAddon.guidPortalFeature);
                    return false;
                }
                switch (request.button ?? "") {
                    case Constants.buttonCancel: {
                            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormPageListAddon.guidPortalFeature, $"&{Constants.rnFormId}={request.formId}");
                            return false;
                        }
                    case Constants.buttonSave: {
                            saveFormPage(cp, request);
                            return true;
                        }
                    case Constants.buttonOK: {
                            saveFormPage(cp, request);
                            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormPageListAddon.guidPortalFeature, $"&{Constants.rnFormId}={request.formId}");
                            return false;
                        }
                    case Constants.buttonDelete: {
                            foreach (var formQuestion in DbBaseModel.createList<FormQuestionModel>(cp, $"formid={request.formPageId}")) {
                                DbBaseModel.delete<FormQuestionModel>(cp, formQuestion.id);
                            }
                            DbBaseModel.delete<FormPageModel>(cp, request.formPageId);
                            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormPageListAddon.guidPortalFeature, $"&{Constants.rnFormId}={request.formId}");
                            return false;
                        }
                    default: {
                            return true;
                        }
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
        // 
        // =====================================================================================
        //
        public static string getView(ApplicationModel app, RequestModel request, string userErrorMessage) {
            CPBaseClass cp = app.cp;
            try {
                //
                // -- init builder
                var layoutBuilder = cp.AdminUI.CreateLayoutBuilderNameValue();
                //
                // -- init parent portal data
                FormModel form = DbBaseModel.create<FormModel>(cp, request.formId);
                if (form == null) { return cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormListAddon.guidPortalFeature); }
                FormPageModel formPage = DbBaseModel.create<FormPageModel>(cp, request.formPageId);
                // 
                // -- add rows
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Name";
                layoutBuilder.rowValue = cp.Html5.InputText(Constants.rnName, 255, formPage?.name ?? "", "form-control");
                layoutBuilder.rowHelp = "The name for this form page used by administrators to manage the form. The user will not see this name.";
                //
                // -- setup layout
                layoutBuilder.title = (formPage == null) ? "Add Form Page" : "Edit Form Page";
                layoutBuilder.portalSubNavTitle = (formPage == null ? "new page" : $"page: {formPage.name}") + $"<br>in form: '{form.name}'";
                layoutBuilder.description = "A form page is one page of questions a user see when submitting a form online. A form can have one or more form pages. Each form page contains one or more form questions.";
                layoutBuilder.callbackAddonGuid = guidAddon;
                layoutBuilder.failMessage = userErrorMessage;
                // 
                // -- add buttons
                layoutBuilder.addFormButton(Constants.buttonOK);
                layoutBuilder.addFormButton(Constants.buttonSave);
                layoutBuilder.addFormButton(Constants.buttonCancel);
                layoutBuilder.addFormButton(Constants.buttonDelete);
                // 
                // -- add hiddens
                //
                // -- set rqs for subnav links
                cp.Doc.AddRefreshQueryString(Constants.rnFormPageId, request.formPageId);
                cp.Doc.AddRefreshQueryString(Constants.rnFormId, request.formId);
                //
                return layoutBuilder.getHtml();
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
                    //
                    // -- important. this record becomes the current focus for the get method
                    request.formPageId = formPage.id;
                }
                formPage.name = request.name;
                formPage.formid = request.formId;
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
                formId = cp.Doc.GetInteger(Constants.rnFormId);
                formPageId = cp.Doc.GetInteger(Constants.rnFormPageId);
                //
                // -- individual fields for the record, request name and requestModel name match the field name (except id)
                name = cp.Doc.GetText("name");
            }
            private CPBaseClass cp;
            //
            public string button { get; }
            //
            public int formId { get; set; }
            //
            public int formPageId { get; set; }
            //
            public string name { get; set; }
        }
    }
}