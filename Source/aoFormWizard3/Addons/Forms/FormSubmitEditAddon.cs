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
    /// Form Submit Edit Feature - allows editing Thank You Page and Join Group on Completion
    /// </summary>
    public class FormSubmitEditAddon : AddonBaseClass {
        //
        public const string guidPortalFeature = "{E5F6A7B8-C9D0-4E1F-2A3B-4C5D6E7F8091}";
        public const string guidAddon = "{F6A7B8C9-D0E1-4F2A-3B4C-5D6E7F809112}";
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
                    RedirectController.redirectToFormSubmitEdit(cp, request.formId);
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
                layoutBuilder.rowName = "Thank You Page";
                layoutBuilder.rowValue = cp.Html5.InputHtml("thankyoucopy", 300, form.thankyoucopy ?? "");
                layoutBuilder.rowHelp = "When the user completes the form set, they will be shown this copy.";
                //
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Join Group on Completion";
                layoutBuilder.rowValue = cp.AdminUI.GetLookupContentEditor("joingroupid", "Groups", form.joingroupid, "", false, false);
                layoutBuilder.rowHelp = "Optional. When the user completes this form set, they will be added to this group.";
                //
                // -- setup layout
                layoutBuilder.title = "Form Submit";
                layoutBuilder.portalSubNavTitleList.Add($"form: '{form.name}'");
                layoutBuilder.description = "Configure what happens when a user submits this form. Set the thank you message and optionally add the user to a group.";
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
                form.thankyoucopy = request.thankyoucopy;
                form.joingroupid = request.joingroupid;
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
                thankyoucopy = cp.Doc.GetText("thankyoucopy");
                joingroupid = cp.Doc.GetInteger("joingroupid");
            }
            private CPBaseClass cp;
            //
            public string button { get; }
            //
            public int formId { get; set; }
            //
            public string thankyoucopy { get; set; }
            //
            public int joingroupid { get; set; }
        }
    }
}
