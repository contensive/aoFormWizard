using Contensive.Addon.aoFormWizard3.Controllers;
using Contensive.Addon.aoFormWizard3.Models.Db;
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
    public class FormQuestionListAddon : AddonBaseClass {
        //
        public const string guidPortalFeature = "{540D0034-E405-4EC9-B80D-038056053923}";
        public const string guidAddon = "{1373B564-3722-478E-B693-E007B2547B3F}";
        public const string viewName = "formQuestionList";
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
                if (request.button.Equals(Constants.ButtonCancel)) { return cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormPageEditAddon.guidPortalFeature) + $"&{Constants.rnFormPageId}={request.formPageId}"; }
                //
                // -- form widget and page required, else redirect to form widget list
                if (request.formId <= 0) { return cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormListAddon.guidPortalFeature); }
                if (request.formPageId <= 0) { return cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormListAddon.guidPortalFeature); }
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
        /// <param name="userErrorMessage"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static string getView(ApplicationModel app, RequestModel request, string userErrorMessage) {
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
                layoutBuilder.columnCaptionClass =  AfwStyles.afwWidth50px + AfwStyles.afwTextAlignCenter;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignCenter;
                layoutBuilder.columnVisible = true;
                layoutBuilder.columnDownloadable = false;
                //
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "&nbsp;";
                layoutBuilder.columnName = "";
                layoutBuilder.columnCaptionClass = AfwStyles.afwWidth50px + AfwStyles.afwTextAlignCenter;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignCenter;
                layoutBuilder.columnVisible = true;
                layoutBuilder.columnDownloadable = false;
                // 
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "Form Question";
                layoutBuilder.columnName = "FormQuestion";
                layoutBuilder.columnCaptionClass = AfwStyles.afwWidth400px + AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnVisible = true;
                layoutBuilder.columnDownloadable = true;
                // 
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "Form Page";
                layoutBuilder.columnName = "FormPage";
                layoutBuilder.columnCaptionClass = AfwStyles.afwWidth400px + AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnVisible = true;
                layoutBuilder.columnDownloadable = true;
                // 
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "Form Widget";
                layoutBuilder.columnName = "FormWidget";
                layoutBuilder.columnCaptionClass = AfwStyles.afwWidth400px + AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnVisible = true;
                layoutBuilder.columnDownloadable = true;
                // 
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "Sort Order";
                layoutBuilder.columnName = "SortOrder";
                layoutBuilder.columnCaptionClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnCellClass = AfwStyles.afwWidth100px + AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnVisible = true;
                layoutBuilder.columnDownloadable = true;
                // 
                // -- add filters
                //
                // -- get data
                FormQuestionListDataModel data = new(cp, request, layoutBuilder.sqlOrderBy, layoutBuilder.sqlSearchTerm, layoutBuilder.paginationPageNumber, layoutBuilder.paginationPageSize);
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
                    layoutBuilder.setCell($"<input type=checkbox name=\"row{rowPtr}\" value=\"{row.formPageId}\">");
                    //
                    // -- form question
                    string formQuestionEditLink = cp.AdminUI.GetPortalFeatureLink(Constants.guidPortalForms, "FormQuestionEditAddon.guidPortalFeature") + $"&{Constants.rnFormId}={row.formId}&{Constants.rnFormPageId}={row.formPageId}&{Constants.rnFormQuestionId}={row.formQuestionId}";
                    layoutBuilder.setCell($"<a href=\"{formQuestionEditLink}\">{row.formQuestionName}</a>", row.formQuestionName);
                    //
                    // -- form page
                    string formPageEditLink = cp.AdminUI.GetPortalFeatureLink(Constants.guidPortalForms, FormPageEditAddon.guidPortalFeature) + $"&{Constants.rnFormId}={row.formId}&{Constants.rnFormPageId}={row.formPageId}";
                    layoutBuilder.setCell($"<a href=\"{formPageEditLink}\">{row.formPageName}</a>", row.formPageName);
                    // 
                    // -- form widget 
                    string formWidgetLink = cp.AdminUI.GetPortalFeatureLink(Constants.guidPortalForms, FormEditAddon.guidPortalFeature) + $"&{Constants.rnFormId}={row.formId}";
                    layoutBuilder.setCell($"<a href=\"{formWidgetLink}\">{row.formWidgetName}</a>", row.formWidgetName);
                    //
                    // -- sort order
                    layoutBuilder.setCell(row.formQuestionSortOrder);
                    //
                    rowPtr += 1;
                }
                //
                // -- build page
                layoutBuilder.title = "Form Questions";
                layoutBuilder.description = "Forms are created by dropping the Form Widget on a page or by creating a form here, and adding Form-Pages, and Form-Questions to the form. Each time a user submits the form online it creates a Form Response.";
                layoutBuilder.callbackAddonGuid = guidAddon;
                layoutBuilder.paginationRecordAlias = "questions";
                layoutBuilder.portalSubNavTitle = DbBaseModel.getRecordName<FormPageModel>(cp, request.formPageId);
                layoutBuilder.failMessage = userErrorMessage;
                layoutBuilder.allowDownloadButton = true;
                // 
                // -- add buttons
                layoutBuilder.addFormButton(Constants.ButtonRefresh);
                layoutBuilder.addFormButton(Constants.ButtonCancel);
                // 
                // -- add hiddens
                layoutBuilder.addFormHidden("rowCnt", rowPtr);
                //
                // -- feature subnav link querystring - clicks must include these values
                cp.Doc.AddRefreshQueryString(Constants.rnFormId, request.formId);
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
                formId = cp.Doc.GetInteger(Constants.rnFormId);
                formPageId = cp.Doc.GetInteger(Constants.rnFormPageId);
            }
            //
            public string button { get; }
            //
            public int formId { get; set; }
            //
            public int formPageId { get; set; }
        }
    }
}