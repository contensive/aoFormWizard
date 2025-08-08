using Contensive.Addon.aoFormWizard3.Controllers;
using Contensive.Addon.aoFormWizard3.Models.Domain;
using Contensive.BaseClasses;
using Contensive.BaseClasses.LayoutBuilder;
using Contensive.Models.Db;
using System;
using static Contensive.BaseClasses.LayoutBuilder.LayoutBuilderBaseClass;

namespace Contensive.Addon.aoFormWizard3.Addons.WidgetDashboardWidgets {
    //
    // ========================================================================================
    /// <summary>
    /// Meetings
    /// </summary>
    /// <remarks></remarks>
    public class FormResponseListAddon : AddonBaseClass {
        //
        public const string guidPortalFeature = "{CBBB730E-1390-471B-A311-F49E6C12E35A}";
        public const string guidAddon = "{909559CA-59D3-4A1B-9915-2D69297AFE2D}";
        public const string viewName = "FormResponseList";
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
                // 
                // -- cancel
                var request = new RequestModel(cp);
                if (request.button.Equals(Constants.ButtonCancel)) { return cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, ""); }
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
                LayoutBuilderListBaseClass layoutBuilder = cp.AdminUI.CreateLayoutBuilderList();
                // 
                // -- setup column headers
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "&nbsp;";
                layoutBuilder.columnName = "";
                layoutBuilder.columnCaptionClass = AfwStyles.afwTextAlignCenter + AfwStyles.afwWidth20px;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignCenter;
                //
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "&nbsp;";
                layoutBuilder.columnName = "";
                layoutBuilder.columnCaptionClass = AfwStyles.afwTextAlignCenter + AfwStyles.afwWidth20px;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignCenter;
                // 
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "Responses";
                layoutBuilder.columnName = "Responses";
                layoutBuilder.columnCaptionClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignLeft;
                // 
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "Form";
                layoutBuilder.columnName = "Form";
                layoutBuilder.columnCaptionClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignLeft;
                // 
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "Submitter";
                layoutBuilder.columnName = "memberId";
                layoutBuilder.columnCaptionClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignLeft;
                // 
                // -- add filters
                request.formWidgetId = layoutBuilder.getFilterInteger(Constants.rnFormWidgetId, viewName);
                layoutBuilder.addFilterSelectContent("Form", Constants.rnFormWidgetId, request.formWidgetId, "form widgets", "", "Any Form");
                //
                // todo: tjis hsould be a list of users that have filled out forms
                //
                request.responseUserId = layoutBuilder.getFilterInteger(Constants.rnResponseUserId, viewName);
                layoutBuilder.addFilterSelectContent("User", Constants.rnResponseUserId, request.responseUserId, "people", "", "Any User");
                //
                FormResponseListDataModel data = new(cp, request, layoutBuilder.sqlOrderBy, layoutBuilder.sqlSearchTerm, layoutBuilder.paginationPageNumber, layoutBuilder.paginationPageSize);
                //
                layoutBuilder.recordCount = data.rowCount;
                //
                int rowPtr = 0;
                int rowPtrStart = layoutBuilder.paginationPageSize * (layoutBuilder.paginationPageNumber - 1);
                int peopleCid = cp.Content.GetID("people");
                foreach (var row in data.rowData) {
                    layoutBuilder.addRow();
                    //
                    layoutBuilder.setCell((rowPtrStart + rowPtr + 1).ToString());
                    //
                    layoutBuilder.setCell($"<input type=checkbox name=\"row{rowPtr}\" value=\"{row.formWidgetId}\">");
                    //
                    // -- form response
                    string formResponseCountLink = cp.AdminUI.GetPortalFeatureLink(Constants.guidPortalForms, FormResponseDetailsAddon.guidPortalFeature) + $"&{Constants.rnFormResponseId}={row.formResponseId}";
                    layoutBuilder.setCell($"<a href=\"{formResponseCountLink}\">{row.formResponseName}</a>");
                    // 
                    // -- form widget
                    string formLink = cp.AdminUI.GetPortalFeatureLink(Constants.guidPortalForms, FormWidgetEditAddon.guidPortalFeature) + $"&{Constants.rnFormWidgetId}={row.formWidgetId}";
                    layoutBuilder.setCell($"<a href=\"{formLink}\">{row.formWidgetName}</a>");
                    //
                    // -- submitter
                    layoutBuilder.setCell(row.submitterName);
                    //
                    rowPtr += 1;
                }
                //
                // -- build page
                layoutBuilder.title = "Form Response List";
                layoutBuilder.description = "Forms are created by dropping the Form Widget on a page or by creating a form here, and adding Form-Pages, and Form-Questions to the form. Each time a user submits the form online it creates a Form Response.";
                layoutBuilder.callbackAddonGuid = guidAddon;
                layoutBuilder.paginationRecordAlias = "forms";
                layoutBuilder.failMessage = errorMessage;
                // 
                // -- add buttons
                //layoutBuilder.addFormButton(Constants.ButtonConfirmRegistration);
                //layoutBuilder.addFormButton(Constants.ButtonCancelRegistration);
                layoutBuilder.addFormButton(Constants.ButtonRefresh);
                layoutBuilder.addFormButton(Constants.ButtonCancel);
                // 
                // -- add hiddens
                layoutBuilder.addFormHidden("rowCnt", rowPtr);
                //
                // -- feature subnav link querystring - clicks must include these values
                cp.Doc.AddRefreshQueryString(Constants.rnFormWidgetId, request.formWidgetId);
                cp.Doc.AddRefreshQueryString(Constants.rnFormPageId, request.formPageId);   
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
                formWidgetId = cp.Doc.GetInteger(Constants.rnFormWidgetId);
                formPageId = cp.Doc.GetInteger(Constants.rnFormPageId);
                responseUserId = cp.Doc.GetInteger(Constants.rnResponseUserId);
            }
            //
            public string button { get; }
            //
            public int formWidgetId { get; set; }
            //
            public int formPageId { get; set; }
            //
            public int responseUserId { get; set; }
        }
    }
}