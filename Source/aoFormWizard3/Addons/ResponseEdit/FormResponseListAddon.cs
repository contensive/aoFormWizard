using Contensive.FormWidget.Controllers;
using Contensive.FormWidget.Models.Db;
using Contensive.FormWidget.Models.Domain;
using Contensive.BaseClasses;
using Contensive.BaseClasses.LayoutBuilder;
using Contensive.Models.Db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using static Contensive.BaseClasses.LayoutBuilder.LayoutBuilderBaseClass;

namespace Contensive.FormWidget.Addons {
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
        public static bool processView(ApplicationModel app, RequestModel request, ref string errorMessage) {
            CPBaseClass cp = app.cp;
            try {
                // 
                // -- validate portal environment
                if (!cp.AdminUI.EndpointContainsPortal()) { 
                    RedirectController.redirectToFormResponseList(cp, request.formId);
                    return false;
                }
                // 
                // -- cancel
                if (request.button.Equals(Constants.buttonCancel)) {
                    RedirectController.redirectToFormPortal(cp);
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
                LayoutBuilderListBaseClass layoutBuilder = cp.AdminUI.CreateLayoutBuilderList();
                // 
                // -- setup column headers
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "&nbsp;";
                layoutBuilder.columnName = "";
                layoutBuilder.columnCaptionClass = AfwStyles.afwTextAlignCenter + AfwStyles.afwWidth20px;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignCenter;
                layoutBuilder.columnVisible = true;
                layoutBuilder.columnDownloadable = false;
                //
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "&nbsp;";
                layoutBuilder.columnName = "";
                layoutBuilder.columnCaptionClass = AfwStyles.afwTextAlignCenter + AfwStyles.afwWidth20px;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignCenter;
                layoutBuilder.columnVisible = true;
                layoutBuilder.columnDownloadable = false;
                // 
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "View";
                layoutBuilder.columnName = "View";
                layoutBuilder.columnCaptionClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnVisible = true;
                layoutBuilder.columnDownloadable = false;
                // 
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "Form";
                layoutBuilder.columnName = "Form";
                layoutBuilder.columnCaptionClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnVisible = true;
                layoutBuilder.columnDownloadable = true;
                // 
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "Started";
                layoutBuilder.columnName = "Started";
                layoutBuilder.columnCaptionClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnVisible = true;
                layoutBuilder.columnDownloadable = false;
                // 
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "Submitted";
                layoutBuilder.columnName = "Submitted";
                layoutBuilder.columnCaptionClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnVisible = true;
                layoutBuilder.columnDownloadable = false;
                // 
                layoutBuilder.addColumn();
                layoutBuilder.columnCaption = "Submitter";
                layoutBuilder.columnName = "memberId";
                layoutBuilder.columnCaptionClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnCellClass = AfwStyles.afwTextAlignLeft;
                layoutBuilder.columnVisible = true;
                layoutBuilder.columnDownloadable = true;
                // 
                // -- add filters
                request.formId = layoutBuilder.getFilterInteger(Constants.rnFormId, viewName);
                if(request.formId <= 0) { request.formId = FormModel.getLastestForm(cp); }
                layoutBuilder.addFilterSelectContent("Form", Constants.rnFormId, request.formId, "forms", "", "Any Form");
                //
                request.onlySubmitted = layoutBuilder.getFilterBoolean(Constants.rnOnlySubmitted, viewName);
                layoutBuilder.addFilterCheckbox("Only Submitted", Constants.rnOnlySubmitted, "1", request.onlySubmitted);
                //
                FormResponseListDataModel data = new(cp, request, layoutBuilder.sqlOrderBy, layoutBuilder.sqlSearchTerm, layoutBuilder.paginationPageNumber, layoutBuilder.paginationPageSize);
                //
                // special case filter, add users in the current response select
                request.responseUserId = layoutBuilder.getFilterInteger(Constants.rnResponseUserId, viewName);
                List<NameValueSelected> userOptionList = [];
                userOptionList.Add(new NameValueSelected("Any User", "0", false));
                foreach ( var row in data.rowData) {
                    //
                    // -- add the submitter to the userOptionList if not already there
                    bool found = userOptionList.Exists(x => x.value.Equals(row.submitterId.ToString()));
                    if (!found) {
                        userOptionList.Add(new NameValueSelected(row.submitterName, row.submitterId.ToString(), (row.submitterId == request.responseUserId)));
                    }
                }
                layoutBuilder.addFilterSelect("User", Constants.rnResponseUserId, userOptionList);
                //
                // -- create headers from form response data
                // -- create questionIdList, a list of question ids in the order they appear in the form
                List<questionPageClass> questionIndexList = [];
                var pageList = DbBaseModel.createList<FormPageModel>(cp, $"(formId={request.formId})", "sortOrder");
                foreach (var page in pageList) {
                    var questionList = DbBaseModel.createList<FormQuestionModel>(cp, $"(formid={page.id})", "sortOrder");
                    foreach (var question in questionList) {
                        //
                        questionIndexList.Add(new questionPageClass {
                            questionId = question.id,
                            pageId = page.id
                        });
                        // 
                        layoutBuilder.addColumn();
                        layoutBuilder.columnCaption = question.caption;
                        layoutBuilder.columnName = question.name;
                        layoutBuilder.columnCaptionClass = AfwStyles.afwTextAlignLeft;
                        layoutBuilder.columnCellClass = AfwStyles.afwTextAlignLeft;
                        layoutBuilder.columnVisible = false;
                        layoutBuilder.columnDownloadable = true;
                    }
                }
                //
                layoutBuilder.recordCount = data.rowCount;
                //
                // -- output data
                //
                int rowPtr = 0;
                int rowPtrStart = layoutBuilder.paginationPageSize * (layoutBuilder.paginationPageNumber - 1);
                int peopleCid = cp.Content.GetID("people");
                foreach (var row in data.rowData) {
                    layoutBuilder.addRow();
                    //
                    // -- create columns for record data
                    //
                    layoutBuilder.setCell((rowPtrStart + rowPtr + 1).ToString());
                    //
                    layoutBuilder.setCell($"<input type=checkbox name=\"row{rowPtr}\" value=\"{row.formId}\">");
                    //
                    // -- view
                    string formResponseCountLink = cp.AdminUI.GetPortalFeatureLink(Constants.guidPortalForms, FormResponseDetailsAddon.guidPortalFeature) + $"&{Constants.rnFormResponseId}={row.formResponseId}";
                    layoutBuilder.setCell($"<a href=\"{formResponseCountLink}\">view</a>", "");
                    // 
                    // -- form
                    string formLink = cp.AdminUI.GetPortalFeatureLink(Constants.guidPortalForms, FormEditAddon.guidPortalFeature) + $"&{Constants.rnFormId}={row.formId}";
                    layoutBuilder.setCell($"<a href=\"{formLink}\">{row.formName}</a>", row.formName);
                    //
                    // -- started
                    layoutBuilder.setCell(row.started?.ToString("G") ?? "");
                    //
                    // -- submitted
                    layoutBuilder.setCell(row.submitted?.ToString("G") ?? "");
                    //
                    // -- submitter
                    layoutBuilder.setCell(row.submitterName);
                    //
                    // -- create columns for submitted data
                    //
                    var formResponseDataJson = row.formResponseData;
                    if (string.IsNullOrEmpty(formResponseDataJson)) { continue; }
                    FormResponseDataModel formResponseData = cp.JSON.Deserialize<FormResponseDataModel>(formResponseDataJson);
                    foreach (var questionIndex in questionIndexList) {
                        //
                        // -- find the answer
                        string cellText = "";
                        if (formResponseData.pageDict.ContainsKey(questionIndex.pageId)) {
                            if (formResponseData.pageDict[questionIndex.pageId].questionDict.ContainsKey(questionIndex.questionId)) {
                                var answer = formResponseData.pageDict[questionIndex.pageId].questionDict[questionIndex.questionId];
                                if (answer.choiceAnswerDict.Count > 0) {
                                    //
                                    // -- multiple choice answer
                                    StringBuilder sb = new StringBuilder();
                                    foreach (var choice in answer.choiceAnswerDict) {
                                        if(choice.Value) {
                                            //
                                            // -- only append if the choice is selected
                                            if (sb.Length > 0) { sb.Append(", "); }
                                            sb.Append(choice.Key);
                                        }
                                    }
                                    cellText = sb.ToString();
                                } else if (!string.IsNullOrEmpty(answer.textAnswer)) {
                                    //
                                    // -- single text answer
                                    cellText = answer.textAnswer;
                                }
                            }
                        }
                        layoutBuilder.setCell(cellText);
                    }
                    //
                    rowPtr += 1;
                }
                //
                // -- build page
                layoutBuilder.title = "Form Response List";
                layoutBuilder.description = @"
                    This is a list of all responses to a form. A form must be selected. If no form is selected, the most recent form is used.
                    Forms are created by dropping the Form Widget on a page or by creating a form here, and adding Form-Pages, and Form-Questions to the form. 
                    Each time a user submits the form online it creates a Form Response.";
                layoutBuilder.callbackAddonGuid = guidAddon;
                layoutBuilder.paginationRecordAlias = "forms";
                layoutBuilder.failMessage = errorMessage;
                layoutBuilder.allowDownloadButton = true;
                // 
                // -- add buttons
                //layoutBuilder.addFormButton(Constants.ButtonConfirmRegistration);
                //layoutBuilder.addFormButton(Constants.ButtonCancelRegistration);
                layoutBuilder.addFormButton(Constants.ButtonRefresh);
                layoutBuilder.addFormButton(Constants.buttonCancel);
                // 
                // -- add hiddens
                layoutBuilder.addFormHidden("rowCnt", rowPtr);
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
                responseUserId = cp.Doc.GetInteger(Constants.rnResponseUserId);
            }
            //
            public string button { get; }
            //
            public int formId { get; set; }
            //
            public int formPageId { get; set; }
            //
            public int responseUserId { get; set; }
            //
            public bool onlySubmitted { get; set; }
        }
        /// <summary>
        /// questionids and their pageids
        /// </summary>
        public class questionPageClass {
            public int questionId { get; set; }
            public int pageId { get; set; }
        }
    }
}