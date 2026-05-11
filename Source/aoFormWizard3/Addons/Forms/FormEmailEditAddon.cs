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
    /// Form Emails Edit Feature - allows editing Notification and Auto-Responder System Emails
    /// </summary>
    public class FormEmailEditAddon : AddonBaseClass {
        //
        public const string guidPortalFeature = "{A1B2C3D4-E5F6-4A7B-8C9D-0E1F2A3B4C5D}";
        public const string guidAddon = "{B2C3D4E5-F6A7-4B8C-9D0E-1F2A3B4C5D6E}";
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
                    RedirectController.redirectToFormEmailEdit(cp, request.formId);
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
                layoutBuilder.rowName = "Notification System Email";
                layoutBuilder.rowValue = cp.AdminUI.GetLookupContentEditor("notificationemailid", "System Email", form.notificationemailid, "", false, false);
                layoutBuilder.rowHelp = "Optional. When the user completes this form set, this system email will be sent to Site Managers with all the values entered from all forms in the form set.";
                //
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Auto-Responder System Email";
                layoutBuilder.rowValue = cp.AdminUI.GetLookupContentEditor("responseemailid", "System Email", form.responseemailid, "", false, false);
                layoutBuilder.rowHelp = "Optional. When the user completes this form set, this system email will be sent to the user.";
                //
                // -- setup layout
                layoutBuilder.title = "Form Emails";
                layoutBuilder.portalSubNavTitleList.Add($"form: '{form.name}'");
                layoutBuilder.description = "Configure the email notifications for this form. The Notification System Email is sent to administrators when a form is submitted. The Auto-Responder System Email is sent to the user who submitted the form.";
                layoutBuilder.callbackAddonGuid = guidAddon;
                //
                // -- add buttons
                layoutBuilder.addFormButton(Constants.buttonOK);
                layoutBuilder.addFormButton(Constants.buttonSave);
                layoutBuilder.addFormButton(Constants.buttonCancel);
                //
                // -- add hiddens
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
                form.notificationemailid = request.notificationemailid;
                form.responseemailid = request.responseemailid;
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
                // -- initialize properties (cannot use default constructor)
                button = cp.Doc.GetText(Constants.rnButton);
                formId = cp.Doc.GetInteger(Constants.rnFormId);
                //
                // -- individual fields for the record
                notificationemailid = cp.Doc.GetInteger("notificationemailid");
                responseemailid = cp.Doc.GetInteger("responseemailid");
            }
            private CPBaseClass cp;
            //
            public string button { get; }
            //
            public int formId { get; set; }
            //
            public int notificationemailid { get; set; }
            //
            public int responseemailid { get; set; }
        }
    }
}
