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
    /// Form Features Edit Feature - allows editing Track Submission by User and Include Bot Detection
    /// </summary>
    public class FormFeatureEditAddon : AddonBaseClass {
        //
        public const string guidPortalFeature = "{A7B8C9D0-E1F2-4A3B-4C5D-6E7F80911223}";
        public const string guidAddon = "{B8C9D0E1-F2A3-4B4C-5D6E-7F8091122334}";
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
                    RedirectController.redirectToFormFeatureEdit(cp, request.formId);
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
                layoutBuilder.rowName = "Track Submission by User";
                layoutBuilder.rowValue = cp.Html5.CheckBox("useUserProperty", form.useUserProperty);
                layoutBuilder.rowHelp = "When checked, the submission will be tracked for each user instead of each visit. This means if a logged-in user saves a submission, they can return later and continue. If unchecked an abandoned submission cannot be completed in the future.";
                //
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Include Bot Detection";
                layoutBuilder.rowValue = cp.Html5.CheckBox("allowRecaptcha", form.allowRecaptcha);
                layoutBuilder.rowHelp = "Include a recaptcha bot blocker (ask if the user is a human) on the first page of the form.";
                //
                // -- setup layout
                layoutBuilder.title = "Form Features";
                layoutBuilder.portalSubNavTitleList.Add($"form: '{form.name}'");
                layoutBuilder.description = "Configure additional features for this form including user tracking and bot detection.";
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
                form.useUserProperty = request.useUserProperty;
                form.allowRecaptcha = request.allowRecaptcha;
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
                useUserProperty = cp.Doc.GetBoolean("useUserProperty");
                allowRecaptcha = cp.Doc.GetBoolean("allowRecaptcha");
            }
            private CPBaseClass cp;
            //
            public string button { get; }
            //
            public int formId { get; set; }
            //
            public bool useUserProperty { get; set; }
            //
            public bool allowRecaptcha { get; set; }
        }
    }
}
