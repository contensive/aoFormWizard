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
        // 
        public static bool processView(ApplicationModel app, RequestModel request, ref string userErrorMessage) {
            CPBaseClass cp = app.cp;
            try {
                // 
                // -- validate portal environment
                if (!cp.AdminUI.EndpointContainsPortal()) {
                    RedirectController.redirectToFormQuestionList(cp, request.formId, request.formPageId);
                    return false;
                }
                // 
                // -- cancel button
                if (request.button.Equals(Constants.buttonCancel)) {
                    RedirectController.redirectToFormPageEdit(cp, request.formId, request.formPageId);
                    return false;
                }
                //
                // -- form required
                if (request.formId <= 0) {
                    RedirectController.redirectToFormList(cp);
                    return false;
                }
                //
                // -- form page required
                if (request.formPageId <= 0) {
                    RedirectController.redirectToFormPageList(cp, request.formId);
                    return false;
                }
                //
                // -- add button
                if (request.button.Equals(Constants.ButtonAdd)) {
                    RedirectController.redirectToFormQuestionAdd(cp, request.formId, request.formPageId);
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
                FormModel form = DbBaseModel.create<FormModel>(cp, request.formId);
                if (form == null) {
                    RedirectController.redirectToFormList(cp);
                    return "";
                }
                FormPageModel formPage = DbBaseModel.create<FormPageModel>(cp, request.formPageId);
                if (formPage == null) {
                    RedirectController.redirectToFormPageList(cp, request.formId);
                    return "";
                }
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
                layoutBuilder.columnCaption = "Form";
                layoutBuilder.columnName = "Form";
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
                    string formQuestionEditLink = cp.AdminUI.GetPortalFeatureLink(Constants.guidPortalForms, FormQuestionEditAddon.guidPortalFeature) + $"&{Constants.rnFormId}={row.formId}&{Constants.rnFormPageId}={row.formPageId}&{Constants.rnFormQuestionId}={row.formQuestionId}";
                    layoutBuilder.setCell($"<a href=\"{formQuestionEditLink}\">{row.formQuestionName}</a>", row.formQuestionName);
                    //
                    // -- form page
                    string formPageEditLink = cp.AdminUI.GetPortalFeatureLink(Constants.guidPortalForms, FormQuestionEditAddon.guidPortalFeature) + $"&{Constants.rnFormId}={row.formId}&{Constants.rnFormPageId}={row.formPageId}";
                    layoutBuilder.setCell($"<a href=\"{formPageEditLink}\">{row.formPageName}</a>", row.formPageName);
                    // 
                    // -- form widget 
                    string formLink = cp.AdminUI.GetPortalFeatureLink(Constants.guidPortalForms, FormEditAddon.guidPortalFeature) + $"&{Constants.rnFormId}={row.formId}";
                    layoutBuilder.setCell($"<a href=\"{formLink}\">{row.formName}</a>", row.formName);
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
                layoutBuilder.portalSubNavTitle = $"page: {formPage.name}" + $"<br>in form: '{form.name}'";
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
                layoutBuilder.addFormHidden(Constants.rnFormId, request.formId);
                layoutBuilder.addFormHidden(Constants.rnFormPageId, request.formPageId);
                //
                // -- feature subnav link querystring - clicks must include these values
                cp.Doc.AddRefreshQueryString(Constants.rnFormId, request.formId);
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