using Contensive.Addon.aoFormWizard3.Controllers;
using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.Addon.aoFormWizard3.Models.Domain;
using Contensive.Addon.aoFormWizard3.Models.View;
using Contensive.Addon.aoFormWizard3.Views;
using Contensive.BaseClasses;
using Contensive.BaseClasses.LayoutBuilder;
using Contensive.DesignBlockBase.Controllers;
using Contensive.DesignBlockBase.Models.View;
using Contensive.Models.Db;
using HtmlAgilityPack;
using System;
using System.Runtime.InteropServices;
using static Contensive.BaseClasses.LayoutBuilder.LayoutBuilderBaseClass;

namespace Contensive.Addon.aoFormWizard3.Addons.WidgetDashboardWidgets {
    //
    // ========================================================================================
    /// <summary>
    /// Meetings
    /// </summary>
    /// <remarks></remarks>
    public class FormResponseDetailsAddon : AddonBaseClass {
        //
        public const string guidPortalFeature = "{10F67098-2D21-4F04-B196-EDBDEF52AFC2}";
        public const string guidAddon = "{1B1C0DEC-334F-4A1A-BC03-444093BEB924}";
        public const string viewName = "FormResponseDetailsAddon";
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
                var request = new RequestModel(cp);
                using (var app = new ApplicationModel(cp)) {
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
        // ========================================================================================
        // 
        public static bool processView(ApplicationModel app, RequestModel request, ref string userErrorMessage) {
            CPBaseClass cp = app.cp;
            try {
                // 
                // -- cancel
                if (request.button.Equals(Constants.buttonCancel)) {
                    cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormResponseListAddon.guidPortalFeature);
                    return false;
                }
                return true;
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// Display a list of registrations -- REFACTOR. This is called from both the meetingList and the search.
        /// the registrationList view should call a getModel. That getModel can be shared with others.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="request"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static string getView(ApplicationModel app, RequestModel request, string errorMessage) {
            CPBaseClass cp = app.cp;
            try {
                //
                // -- init layoutbuilder
                LayoutBuilderBaseClass layoutBuilder = cp.AdminUI.CreateLayoutBuilder();
                //
                // -- build body from app scoring widget
                FormResponseModel response = DbBaseModel.create<FormResponseModel>(cp, request.formResponseId);
                if (response is null) { return "The selected response is invalid."; }
                //
                var form = DbBaseModel.create<FormModel>(cp, response.formId);
                if (response is null) { return "The selected form is invalid."; }
                PersonModel submittedBy = DbBaseModel.create<PersonModel>(cp, response.memberId);
                //
                // -- Render the form with mulitpage-preview true and editing false
                var formWidget = new FormWidgetModel();
                var previewData = DesignBlockViewBaseModel.create<FormWidgetViewModel>(cp, formWidget);
                previewData.id = 0;
                previewData.instanceId = "";
                previewData = FormWidgetViewModel.createFromResponse(cp, previewData, true, false, form, response);
                var previewLayout = cp.Layout.GetLayout(Constants.guidLayoutFormWizard, Constants.nameLayoutFormWizard, Constants.pathFilenameLayoutFormWizard);
                string renderedForm = cp.Mustache.Render(previewLayout, previewData);
                //
                LayoutBuilderNameValueBaseClass contentLB = cp.AdminUI.CreateLayoutBuilderNameValue();
                //
                contentLB.addRow();
                contentLB.rowName = "Submission";
                contentLB.rowValue = response.name;
                //
                contentLB.addRow();
                contentLB.rowName = "Submitted";
                contentLB.rowValue = (response?.dateSubmitted == null || response.dateSubmitted == DateTime.MinValue) ? "(not submitted)" : ((DateTime)response.dateSubmitted).ToString("g");
                //
                contentLB.addColumn();
                contentLB.rowName = "User";
                contentLB.rowValue = (string.IsNullOrEmpty(submittedBy?.name) ? "unknown" : submittedBy.name) + (string.IsNullOrEmpty(submittedBy?.email) ? "" : $", email {submittedBy.email}");
                //
                contentLB.addRow();
                contentLB.rowName = "";
                contentLB.rowValue = renderedForm;
                //
                layoutBuilder.body = contentLB.getHtml();
                //
                // -- build page
                layoutBuilder.title = "Form Response Details";
                layoutBuilder.description = "";
                layoutBuilder.callbackAddonGuid = guidAddon;
                layoutBuilder.failMessage = errorMessage;
                // 
                // -- add buttons
                layoutBuilder.addFormButton(Constants.ButtonRefresh);
                layoutBuilder.addFormButton(Constants.buttonCancel);
                //
                // -- feature subnav link querystring - clicks must include these values
                cp.Doc.AddRefreshQueryString(Constants.rnFormResponseId, request.formResponseId);
                cp.Doc.AddRefreshQueryString(Constants.rnResponseUserId, request.responseUserId);
                //
                return layoutBuilder.getHtml();
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
        public class RequestModel {
            //
            public RequestModel(CPBaseClass cp) {
                string removeFilter = cp.Doc.GetText(Constants.rnRemoveFilter);
                if (!string.IsNullOrEmpty(removeFilter)) { cp.Doc.SetProperty(removeFilter, ""); }
                //
                button = cp.Doc.GetText(Constants.rnButton);
                formResponseId = cp.Doc.GetInteger(Constants.rnFormResponseId);
                responseUserId = cp.Doc.GetInteger(Constants.rnResponseUserId);
            }
            //
            public string button { get; }
            //
            public int formResponseId { get; set; }
            //
            public int responseUserId { get; set; }
        }
    }
}