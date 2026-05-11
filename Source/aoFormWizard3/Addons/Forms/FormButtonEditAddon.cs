using Contensive.FormWidget;
using Contensive.FormWidget.Addons;
using Contensive.FormWidget.Controllers;
using Contensive.FormWidget.Models.Db;
using Contensive.FormWidget.Models.Domain;
using Contensive.BaseClasses;
using Contensive.Models.Db;
using System;

namespace Contensive.FormWidget.Addons {
    /// <summary>
    /// Form Buttons Edit Feature - allows editing button text and options
    /// </summary>
    public class FormButtonEditAddon : AddonBaseClass {
        //
        public const string guidPortalFeature = "{C3D4E5F6-A7B8-4C9D-0E1F-2A3B4C5D6E7F}";
        public const string guidAddon = "{D4E5F6A7-B8C9-4D0E-1F2A-3B4C5D6E7F80}";
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
                string userErrorMessage = "";
                var request = new RequestModel(cp);
                using (var app = new ApplicationModel(cp)) {
                    if (!processView(app, request, ref userErrorMessage)) { return ""; }
                    return getView(app, request);
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
        public static bool processView(ApplicationModel app, RequestModel request, ref string userErrorMessage) {
            CPBaseClass cp = app.cp;
            try {
                //
                // -- validate portal environment
                if (!cp.AdminUI.EndpointContainsPortal()) {
                    RedirectController.redirectToFormButtonEdit(cp, request.formId);
                    return false;
                }
                //
                // -- form required
                if (request.formId <= 0) {
                    RedirectController.redirectToFormList(cp);
                    return false;
                }
                //
                // -- cancel button
                if (request.button.Equals(Constants.buttonCancel)) {
                    RedirectController.redirectToFormEdit(cp, request.formId);
                    return false;
                }
                //
                // -- save button
                if (request.button.Equals(Constants.buttonSave)) {
                    saveForm(cp, request);
                    return true;
                }
                //
                // -- ok button
                if (request.button.Equals(Constants.buttonOK)) {
                    saveForm(cp, request);
                    RedirectController.redirectToFormEdit(cp, request.formId);
                    return false;
                }
                return true;
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
                //
                // -- init builder
                var layoutBuilder = cp.AdminUI.CreateLayoutBuilderNameValue();
                //
                // -- init parent portal data
                FormModel form = DbBaseModel.create<FormModel>(cp, request.formId);
                if (form == null) {
                    RedirectController.redirectToFormList(cp);
                    return "";
                }
                //
                // -- add rows
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Add Reset Button";
                layoutBuilder.rowValue = cp.Html5.CheckBox("addResetButton", form.addResetButton);
                layoutBuilder.rowHelp = "If checked, a reset button is added to all pages of this form to clear the input and take the user back to the first page.";
                //
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Reset Button Text";
                layoutBuilder.rowValue = cp.Html5.InputText("resetButtonName", 255, form.resetButtonName ?? "", "form-control");
                layoutBuilder.rowHelp = "If a reset button is included, this text that appears on the Reset button.";
                //
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Back Button Text";
                layoutBuilder.rowValue = cp.Html5.InputText("backButtonName", 255, form.backButtonName ?? "", "form-control");
                layoutBuilder.rowHelp = "If the form includes multiple pages, this is the text that appears on the back button used to move backward between pages.";
                //
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Continue Button Text";
                layoutBuilder.rowValue = cp.Html5.InputText("continueButtonName", 255, form.continueButtonName ?? "", "form-control");
                layoutBuilder.rowHelp = "If the form includes multiple pages, this is the text that appears on the continue button used to move forward between pages.";
                //
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Submit Button Text";
                layoutBuilder.rowValue = cp.Html5.InputText("submitButtonName", 255, form.submitButtonName ?? "", "form-control");
                layoutBuilder.rowHelp = "The text that appears on the submit button on the last page of the form.";
                //
                // -- setup layout
                layoutBuilder.title = "Form Buttons";
                layoutBuilder.portalSubNavTitleList.Add($"form: '{form.name}'");
                layoutBuilder.description = "Configure the button text for this form. These settings control the text displayed on each button the user sees when filling out the form.";
                layoutBuilder.callbackAddonGuid = guidAddon;
                //
                // -- add buttons
                layoutBuilder.addFormButton(Constants.buttonOK);
                layoutBuilder.addFormButton(Constants.buttonSave);
                layoutBuilder.addFormButton(Constants.buttonCancel);
                //
                // -- set rqs for subnav links
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
        private static void saveForm(CPBaseClass cp, RequestModel request) {
            try {
                var form = DbBaseModel.create<FormModel>(cp, request.formId);
                if (form is null) { return; }
                form.addResetButton = request.addResetButton;
                form.resetButtonName = request.resetButtonName;
                form.backButtonName = request.backButtonName;
                form.continueButtonName = request.continueButtonName;
                form.submitButtonName = request.submitButtonName;
                form.save(cp);
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
                // -- initialize properties
                button = cp.Doc.GetText(Constants.rnButton);
                formId = cp.Doc.GetInteger(Constants.rnFormId);
                //
                // -- individual fields for the record
                addResetButton = cp.Doc.GetBoolean("addResetButton");
                resetButtonName = cp.Doc.GetText("resetButtonName");
                backButtonName = cp.Doc.GetText("backButtonName");
                continueButtonName = cp.Doc.GetText("continueButtonName");
                submitButtonName = cp.Doc.GetText("submitButtonName");
            }
            private CPBaseClass cp;
            //
            public string button { get; }
            //
            public int formId { get; set; }
            //
            public bool addResetButton { get; set; }
            //
            public string resetButtonName { get; set; }
            //
            public string backButtonName { get; set; }
            //
            public string continueButtonName { get; set; }
            //
            public string submitButtonName { get; set; }
        }
    }
}
