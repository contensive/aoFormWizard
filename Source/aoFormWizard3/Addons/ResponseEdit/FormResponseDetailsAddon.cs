using Contensive.Addon.aoFormWizard3.Controllers;
using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.Addon.aoFormWizard3.Models.Domain;
using Contensive.BaseClasses;
using Contensive.BaseClasses.LayoutBuilder;
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
                // -- cancel
                var request = new RequestModel(cp);
                if (request.button.Equals(Constants.buttonCancel)) { return cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormResponseListAddon.guidPortalFeature); }
                // 
                using (var app = new ApplicationModel(cp)) {
                    string userErrorMessage = "";
                    processView(app, request, ref userErrorMessage);
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
        public static void processView(ApplicationModel app, RequestModel request, ref string errorMessage) {
            CPBaseClass cp = app.cp;
            try {
                //if (request.button == Constants.ButtonConfirmRegistration) {
                //    for (var rowPtr = 0; rowPtr < cp.Doc.GetInteger("rowCnt"); rowPtr++) {
                //        // 
                //        // -- confirm registration
                //        if (cp.Doc.GetBoolean("row" + rowPtr)) {
                //            int registrationId = cp.Doc.GetInteger("row" + rowPtr);
                //            if (registrationId > 0) {
                //                MeetingRegistrationModel registration = DbBaseModel.create<MeetingRegistrationModel>(cp, registrationId);
                //                if (registration == null) {
                //                    errorMessage = "registration not found";
                //                    return;
                //                }
                //                OrderModel order = DbBaseModel.create<OrderModel>(cp, registration.orderid);
                //                if (order == null) {
                //                    errorMessage = "order not found";
                //                    return;
                //                }
                //                // -- cancel the registration
                //                MeetingRegistrationModel.confirmRegistration(cp, registration, order);
                //            }
                //        }
                //    }
                //}
                //if (request.button == Constants.ButtonCancelRegistration) {
                //    for (var rowPtr = 0; rowPtr < cp.Doc.GetInteger("rowCnt"); rowPtr++) {
                //        // 
                //        // -- cancel registration
                //        if (cp.Doc.GetBoolean("row" + rowPtr)) {
                //            int registrationId = cp.Doc.GetInteger("row" + rowPtr);
                //            if (registrationId > 0) {
                //                MeetingRegistrationModel registration = DbBaseModel.create<MeetingRegistrationModel>(cp, registrationId);
                //                if (registration == null) {
                //                    errorMessage = "registration not found";
                //                    return;
                //                }
                //                // -- cancel the registration
                //                MeetingRegistrationModel.cancelRegistration(cp, registrationId);
                //            }
                //        }
                //    }
                //}
                return;
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
                string mockWidgetGuid = $"Form-Portal-App-Score-Widget-For-Response-{response.id}";
                ApplicationScoreWidgetsModel appScoreWidget = DbBaseModel.create<ApplicationScoreWidgetsModel>(cp, mockWidgetGuid);
                if(appScoreWidget is null) {
                    appScoreWidget = DbBaseModel.addDefault<ApplicationScoreWidgetsModel>(cp);
                    appScoreWidget.name = mockWidgetGuid;
                    appScoreWidget.ccguid = mockWidgetGuid;
                    appScoreWidget.formid = response.formId;
                    appScoreWidget.save(cp);
                }
                //
                // -- call the remote method that returns the html for the submission scoring widget (response scoring widget, application scoring widget)
                cp.Doc.SetProperty("scoreWidgetId", appScoreWidget.id);
                cp.Doc.SetProperty("submissionId", request.formResponseId);
                string submissionScoringWidgetDataJson = cp.Addon.ExecuteByUniqueName("GetSubmissionScoringData");
                submissionScoringWidgetDataModel submissionScoringWidgetData = cp.JSON.Deserialize<submissionScoringWidgetDataModel>(submissionScoringWidgetDataJson);
                //
                // -- get just the application preview
                HtmlDocument doc = new();
                doc.LoadHtml(submissionScoringWidgetData.html);
                HtmlNode targetDiv = doc.GetElementbyId("js-response-preview");
                if (targetDiv != null) {
                    layoutBuilder.body = targetDiv.OuterHtml;
                    //
                    // -- remove the scoring tools at the bottom of the page
                    doc.LoadHtml(targetDiv.OuterHtml);
                    HtmlNode scoringToolsDiv = doc.GetElementbyId("js-score-widget-tools");
                    if (scoringToolsDiv != null) {
                        scoringToolsDiv.Remove(); // Removes it from the DOM
                    }
                    layoutBuilder.body = doc.DocumentNode.OuterHtml; 
                }
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