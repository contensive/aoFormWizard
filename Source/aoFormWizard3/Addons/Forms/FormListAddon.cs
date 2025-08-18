using Contensive.FormWidget.Controllers;
using Contensive.FormWidget.Models.Db;
using Contensive.FormWidget.Models.Domain;
using Contensive.BaseClasses;
using Contensive.BaseClasses.LayoutBuilder;
using Contensive.Models.Db;
using System;
using static Contensive.BaseClasses.LayoutBuilder.LayoutBuilderBaseClass;

namespace Contensive.FormWidget.Addons {
    //
    // ========================================================================================
    /// <summary>
    /// Meetings
    /// </summary>
    /// <remarks></remarks>
    public class FormListAddon : AddonBaseClass {
        //
        public const string guidPortalFeature = "{F043F3E1-1F5D-4D6E-A2D9-45124EE66D72}";
        public const string guidAddon = "{F2E3D417-8C2B-49FF-A568-57A04414A8A1}";
        public const string viewName = "FormList";
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
        // ========================================================================================
        /// <summary>
        /// return true to display this form, false to skip the form and return
        /// </summary>
        /// <param name="app"></param>
        /// <param name="request"></param>
        /// <param name="userErrorMessage"></param>
        /// <returns></returns>
        public static bool processView(ApplicationModel app, RequestModel request, ref string userErrorMessage) {
            CPBaseClass cp = app.cp;
            try {
                // 
                // -- validate portal environment
                if (!cp.AdminUI.EndpointContainsPortal()) {
                    RedirectController.redirectToFormList(cp);
                    return false;
                }
                // 
                // -- cancel button
                if (request.button.Equals(Constants.buttonCancel)) {
                    RedirectController.redirectToFormList(cp);
                    return false;
                }
                // 
                // -- add button
                if (request.button.Equals(Constants.ButtonAdd)) {
                    RedirectController.redirectToFormAdd(cp);   
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
                // -- init parent portal data
                // 
                // -- setup column headers
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "&nbsp;";
                layoutBuilder.columnName = "";
                layoutBuilder.columnCaptionClass = AfwStyles.afwWidth50px + AfwStyles.afwTextAlignCenter;
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
                layoutBuilder.columnCaption = "Form";
                layoutBuilder.columnName = "Form";
                layoutBuilder.columnCaptionClass = AfwStyles.afwWidth400px + AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnVisible = true;
                layoutBuilder.columnDownloadable = true;
                // 
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "Responses";
                layoutBuilder.columnName = "Responses";
                layoutBuilder.columnCaptionClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnVisible = true;
                layoutBuilder.columnDownloadable = true;
                //
                // -- add column for elipsis menu
                //
                // -- build data
                FormListDataModel data = new(cp, request, layoutBuilder.sqlOrderBy, layoutBuilder.sqlSearchTerm, layoutBuilder.paginationPageNumber, layoutBuilder.paginationPageSize);
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
                    layoutBuilder.setCell($"<input type=checkbox name=\"row{rowPtr}\" value=\"{row.formId}\">");
                    // 
                    string formLink = cp.AdminUI.GetPortalFeatureLink(Constants.guidPortalForms, FormEditAddon.guidPortalFeature) + $"&{Constants.rnFormId}={row.formId}";
                    layoutBuilder.setCell($"<a href=\"{formLink}\">{(string.IsNullOrEmpty(row.formName) ? "(Unnamed Form)" : row.formName)}</a>", row.formName);
                    //
                    string formResponseCountLink = cp.AdminUI.GetPortalFeatureLink(Constants.guidPortalForms, FormResponseListAddon.guidPortalFeature) + $"&{Constants.rnFormId}={row.formId}";
                    layoutBuilder.setCell($"<a href=\"{formResponseCountLink}\">{row.formResponseCount}</a>", row.formResponseCount.ToString());
                    //
                    // -- elipsis menu
                    //
                    rowPtr += 1;
                }
                //
                // -- setup layout
                layoutBuilder.title = "Form List";
                layoutBuilder.description = "Forms are created by dropping the Form Widget on a page or by creating a form here, and adding Form-Pages, and Form-Questions to the form. Each time a user submits the form online it creates a Form Response.";
                layoutBuilder.callbackAddonGuid = guidAddon;
                layoutBuilder.paginationRecordAlias = "forms";
                layoutBuilder.failMessage = userErrorMessage;
                layoutBuilder.allowDownloadButton = true;
                // 
                // -- add buttons
                layoutBuilder.addFormButton(Constants.ButtonAdd);
                layoutBuilder.addFormButton(Constants.ButtonRefresh);
                layoutBuilder.addFormButton(Constants.buttonCancel);
                // 
                // -- add hiddens
                layoutBuilder.addFormHidden("rowCnt", rowPtr);
                //
                // -- feature subnav link querystring - clicks must include these values
                //cp.Doc.AddRefreshQueryString(Constants.rnMeetingId, meetingId);
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
                sampleId = cp.Doc.GetInteger(Constants.rnSampleId);
            }
            //
            public string button { get; }
            //
            public int sampleId { get; set; }
        }
    }
}