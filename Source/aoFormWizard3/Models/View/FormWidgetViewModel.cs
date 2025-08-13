using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.BaseClasses;
using Contensive.BaseClasses.LayoutBuilder;
using Contensive.DesignBlockBase.Models.View;
using Contensive.Models.Db;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Contensive.Addon.aoFormWizard3.Models.View {
    /// <summary>
    /// Construct the view to be displayed (the current form)
    /// </summary>
    public class FormWidgetViewModel : DesignBlockViewBaseModel {
        //
        /// <summary>
        /// the id of the formwidget record for this formwidget. The formwidget describes which form is displayed for a widget.
        /// </summary>
        public int id { get; set; }
        //
        /// <summary>
        /// display the page saying the form is not ready (not configured, non-admin user)
        /// </summary>
        public bool isNotAvailableView { get; set; }
        //
        /// <summary>
        /// display if the page is not ready and the user is admin -- select form or create form
        /// </summary>
        public bool isSelectFormView { get; set; }
        //
        /// <summary>
        /// true if the form is configured and can be displayed
        /// </summary>
        public bool isDisplayFormView {
            get {
                return !isNotAvailableView && !isSelectFormView;
            }
        }
        //
        /// <summary>
        /// The edit link for the form (within the widget, contains form-pages)
        /// </summary>
        public string formEditLink { get; set; }
        //
        /// <summary>
        /// class added to the form edit wrapper
        /// </summary>
        public string formEditWrapperClass { get; set; }
        //
        /// <summary>
        /// display the form to the user to submit
        /// </summary>
        public bool isFormView { get; set; }
        //
        /// <summary>
        /// display the thank you page
        /// </summary>
        public bool isThankYouView { get; set; }
        //
        /// <summary>
        /// when true, the form displays all the pages one after the other with no navigation buttons
        /// if true, the form is displayed as a preview, used for editing the form and for response review.
        /// Hide buttons and navigation.
        /// change all inputs to read only.
        /// </summary>
        public bool isMultipagePreviewMode { get; set; }
        //
        /// <summary>
        /// for the SelectFormView, if true there are form options to select
        /// </summary>
        public bool hasSelectOptions {
            get {
                return selectOptions.Count > 0;
            }
        }
        //
        /// <summary>
        /// list of forms that can be attached to this widget
        /// </summary>
        public List<NameValueSelected> selectOptions {
            get {
                return _selectOptions ??= [];
            }
        }
        private List<NameValueSelected> _selectOptions;
        //
        /// <summary>
        /// A short string that is unique to this form.
        /// </summary>
        public string formHtmlId { get; set; }//
        //
        /// <summary>
        /// The 0-based index to the current page. Saved in the page for processing. Continue moves to the next page.
        /// </summary>
        public int srcPageId { get; set; }
        //
        /// <summary>
        /// has to be added to the form so it can be rendered in the callback
        /// </summary>
        public string instanceId { get; set; }
        //
        public bool allowRecaptcha { get; set; }
        //
        public string recaptchaHTML { get; set; }
        //
        /// <summary>
        /// the description for the entire form, not just each page
        /// </summary>
        public string ThankYouCopy { get; set; }
        //
        //public List<FieldViewModel> formQuestionList { get; set; } = [];
        //
        public string fieldAddLink { get; set; }
        //
        //public string previousButton { get; set; }
        ////
        //public string resetButton { get; set; }
        ////
        //public string submitButton { get; set; }
        ////
        //public string saveButton { get; set; }
        ////
        //public string continueButton { get; set; }
        //
        public string formPageAddLink { get; set; }
        //
        public bool isEditing { get; set; }
        //
        public List<FormPageModel> pageList { get; set; }
        //
        public List<FormPageViewModel> formPageList { get; set; }
        // 
        public class PageQuestionViewModel {
            public string inputtype { get; set; }
            public string caption { get; set; }
            public string headline { get; set; }
            public string fielddescription { get; set; }
            public bool @required { get; set; }
            public string name { get; set; }
            public string currentValue { get; set; }
            public string currentFileUrl { get; set; }
            public bool isCheckbox { get; set; }
            public bool isRadio { get; set; }
            public bool isSelect { get; set; }
            public bool isTextArea { get; set; }
            public bool isFile { get; set; }
            public bool isDefault { get; set; }
            public int id { get; set; }
            public List<OptionClass> optionList { get; set; } = new List<OptionClass>();
            public string formQuestionEditWrapperClass { get; set; }
            public string formQuestionEditLink { get; set; }
            public bool invalidAnswer { get; set; }
            public bool isReadOnly { get; set; }
        }
        // 
        public class OptionClass {
            public string optionName { get; set; }
            public int optionPtr { get; set; }
            public bool isSelected { get; set; }
            public bool isChecked { get; set; }
        }
        //
        /// <summary>
        /// When in edit mode, this is the list of all pages so the user can edit any page
        /// </summary>
        public class FormPageViewModel {
            public string pageDescription { get; set; }
            public List<PageQuestionViewModel> pageQuestionList { get; set; } = new List<PageQuestionViewModel>();
            public string formQuestionAddLink { get; set; }
            public string previousButton { get; set; }
            public string resetButton { get; set; }
            public string submitButton { get; set; }
            public string saveButton { get; set; }
            public string continueButton { get; set; }
            public string formPageEditWrapperClass { get; set; }
            public string formPageEditLink { get; set; }
            public string formAddLink { get; set; }
            public bool isEditing { get; set; }
            public bool isThankYouPage { get; set; }
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// method compatible with design-block pattern, called by the public widgets.
        /// if editing, use multipageMode and previewMode.
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="formWidget"></param>
        /// <returns></returns>
        public static FormWidgetViewModel create(CPBaseClass cp, FormWidgetModel formWidget) {
            try {
                bool isMultipagePreviewMode = cp.Doc.GetBoolean("isMultipagePreviewMode");
                bool isEditing = cp.Doc.GetBoolean("isEditing");
                int formResponseId = cp.Doc.GetInteger("formResponseId");
                //
                cp.Log.Debug($"aoFormWizard.FormSetViewModel.create() start, isMultipagePreviewMode [{isEditing}], isMultipagePreviewMode [{isEditing}], formResponseId [{formResponseId}]");
                //
                string button = cp.Doc.GetText("button");
                if (!isMultipagePreviewMode && cp.User.IsAdmin) {
                    //
                    // -- process special cases. handle create form
                    if (button.Equals("Create Form")) {
                        formWidget.formId = (DbBaseModel.addDefault<FormModel>(cp)).id;
                        formWidget.save(cp);
                    }
                    //
                    // -- handle select form
                    if (button.Equals("Select Form")) {
                        formWidget.formId = cp.Doc.GetInteger("setFormWizardFormId");
                        if (formWidget.formId == 0) {
                            var result = new FormWidgetViewModel {
                                isSelectFormView = true,
                                isEditing = isEditing,
                                _selectOptions = []
                            };
                            populateFormSelections(cp, result);
                            return result;
                        }
                        formWidget.save(cp);
                    }
                }
                //
                //
                FormModel form = DbBaseModel.create<FormModel>(cp, formWidget.formId);
                if (form is null) {
                    //
                    // -- no form is selected
                    if (cp.User.IsAdmin) {
                        var result = new FormWidgetViewModel {
                            isSelectFormView = true,
                            isEditing = isEditing,
                            _selectOptions = []
                        };
                        populateFormSelections(cp, result);
                        return result;
                    }
                    return new FormWidgetViewModel() {
                        isNotAvailableView = true
                    };
                }
                //
                // -- determine the submission to display
                FormResponseModel formResponse = null;
                if (formResponseId > 0) {
                    //
                    // -- open the requested response (for preview mode)
                    formResponse = DbBaseModel.create<FormResponseModel>(cp, formResponseId);
                } else {
                    //
                    // -- select the the latest response for this form for this user/visit
                    string userFormResponseSql = form.useUserProperty ? $"memberid = {cp.User.Id}" : $"visitid={cp.Visit.Id}";
                    formResponse = DbBaseModel.createFirstOfList<FormResponseModel>(cp, $"(formId={form.id})and({userFormResponseSql})", "id desc");
                }
                //if (formResponse == null || formResponse.formId != form.id) {
                //    //
                //    // -- if the response is not for this form, that is an error. Block
                //    return new FormWidgetViewModel() {
                //        isNotAvailableView = true
                //    };
                //}
                // 
                // -- process form request
                if (!isMultipagePreviewMode) {
                    processRequest(cp, form.id, ref formResponse);
                }
                var resultViewData = create<FormWidgetViewModel>(cp, formWidget);
                resultViewData.id = formWidget.id;
                resultViewData.instanceId = formWidget.ccguid;
                //
                // -- create the view model from the response data
                return createFromResponse(cp, resultViewData, isMultipagePreviewMode,  isEditing, form, formResponse);
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
        //
        /// <summary>
        /// create the view model from the response data.
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="resultViewData"></param>
        /// <param name="isMultipagePreviewMode"></param>
        /// <param name="isEditing"></param>
        /// <param name="form"></param>
        /// <param name="formResponse"></param>
        /// <returns></returns>
        public static FormWidgetViewModel createFromResponse(CPBaseClass cp, FormWidgetViewModel resultViewData, bool isMultipagePreviewMode, bool isEditing, FormModel form, FormResponseModel formResponse) {
            // 
            // -- begin create output data
            resultViewData.formHtmlId = string.IsNullOrEmpty(("formHtmlId")) ? cp.Utils.GetRandomString(4) : ("formHtmlId");
            resultViewData.allowRecaptcha = false;
            resultViewData.recaptchaHTML = "";
            resultViewData.isEditing = isEditing;
            resultViewData.formPageList = [];
            resultViewData.formEditWrapperClass = resultViewData.isEditing ? "ccEditWrapper" : "";
            resultViewData.formEditLink = resultViewData.isEditing ? cp.Content.GetEditLink(FormModel.tableMetadata.contentName, form.id.ToString(), false, "", true) : "";
            resultViewData.isMultipagePreviewMode = isMultipagePreviewMode;
            //
            // -- validate the savedAnswers object
            FormResponseDataModel savedAnswers = string.IsNullOrEmpty(formResponse?.formResponseData) ? new() : cp.JSON.Deserialize<FormResponseDataModel>(formResponse.formResponseData);
            savedAnswers.pageDict ??= [];
            savedAnswers.activity ??= [];
            //
            if (!isMultipagePreviewMode &&savedAnswers.isComplete) {
                //
                // -- form complete, show thank you and exit
                resultViewData.ThankYouCopy = form.thankyoucopy;
                resultViewData.isThankYouView = true;
                return resultViewData;
            }
            //
            // -- the output is the normal user output
            resultViewData.isFormView = true;
            // -- if editing, preview multipage mode
            //
            // -- validate the current page
            resultViewData.pageList = FormPageModel.getPageList(cp, form.id);
            if (resultViewData.pageList.Count == 0) {
                if (!cp.User.IsAdmin) {
                    //
                    // -- no pages in the form, not admin
                    return new FormWidgetViewModel() {
                        isNotAvailableView = true
                    };
                }
                //
                // -- no pages, admin, add a form-page and reload formlist
                var formPage = DbBaseModel.addDefault<FormPageModel>(cp);
                formPage.formid = form.id;
                formPage.name = $"Initial Page for {form.name}";
                formPage.save(cp);
                //
                resultViewData.pageList = FormPageModel.getPageList(cp, form.id);
            }
            if (resultViewData.pageList.Count > 0) {
                var currentpage = resultViewData.pageList.Find((x) => x.id == savedAnswers.currentPageid);
                if (currentpage is null) {
                    savedAnswers.currentPageid = resultViewData.pageList.First().id;
                }
            }
            resultViewData.srcPageId = savedAnswers.currentPageid;
            //
            if (resultViewData.pageList.Count <= 0) { return resultViewData; }
            // 
            // -- output one page with page one header
            foreach (FormPageModel page in resultViewData.pageList) {
                var pageQuestionList = new List<PageQuestionViewModel>();
                //
                //-- skip to the current page
                if (!resultViewData.isMultipagePreviewMode && page.id != savedAnswers.currentPageid) {
                    //
                    // -- not multipagemode, and this not the current page
                    continue;
                }
                //
                // -- recapcha
                if (form.allowRecaptcha && !savedAnswers.recaptchaSuccess && !resultViewData.isMultipagePreviewMode) {
                    //
                    cp.Log.Debug($"aoFormWizard.FormSetViewModel.create(), add recaptcha display");
                    // 
                    resultViewData.allowRecaptcha = form.allowRecaptcha;
                    //const string recaptchaDisplayAddonGuid = "{E9E51C6E-9152-4284-A44F-D3ABC423AB90}";
                    //formViewData.recaptchaHTML = cp.Addon.Execute(recaptchaDisplayAddonGuid);
                    resultViewData.recaptchaHTML = cp.Addon.Execute(Constants.guidAddonRecaptchav2);
                    if (cp.UserError.OK()) {
                        savedAnswers.recaptchaSuccess = true;
                    }
                    //
                    cp.Log.Debug($"aoFormWizard.FormSetViewModel.create(), return recaptchaHTML [{cp.Utils.EncodeHTML(resultViewData.recaptchaHTML)}]");
                    //
                }
                //
                FormResponseDataPageModel savedAnswers_Page = savedAnswers.pageDict.TryGetValue(page.id, out var savedAnswers_Page_Result) ? savedAnswers_Page_Result : new FormResponseDataPageModel();
                savedAnswers_Page.questionDict ??= [];
                //
                int questionPtr = 0;
                var questionList = FormQuestionModel.getQuestionList(cp, page.id);
                foreach (var question in questionList) {
                    FormResponseDataPageQuestionModel savedAnswers_Page_Question = savedAnswers_Page.questionDict.TryGetValue(question.id, out var savedAnswers_Page_Question_Result) ? savedAnswers_Page_Question_Result : new FormResponseDataPageQuestionModel();
                    savedAnswers_Page_Question.choiceAnswerDict ??= [];
                    //
                    var optionList = new List<OptionClass>();
                    int optionPtr = 1;
                    foreach (var questionOptionName in question.optionList.Split(',')) {
                        if (string.IsNullOrEmpty(questionOptionName)) { continue; }
                        //
                        optionList.Add(new OptionClass() {
                            optionName = questionOptionName,
                            optionPtr = optionPtr,
                            isSelected = savedAnswers_Page_Question.choiceAnswerDict.ContainsKey(questionOptionName) ? savedAnswers_Page_Question.choiceAnswerDict[questionOptionName] : false,
                            isChecked = savedAnswers_Page_Question.choiceAnswerDict.ContainsKey(questionOptionName) ? savedAnswers_Page_Question.choiceAnswerDict[questionOptionName] : false
                        });
                        optionPtr += 1;
                    }
                    string fieldEditLink = cp.Content.GetEditLink(FormQuestionModel.tableMetadata.contentName, question.id.ToString(), false, "Edit Question", resultViewData.isEditing);
                    //
                    cp.Utils.AppendLog($"form-widget, FormViewModel.create, page [{page.id}], question [{question.id}], fieldEditLink [{fieldEditLink}]");
                    //
                    switch (question.inputTypeId) {
                        case (int)FormQuestionModel.inputTypeEnum.radio: {
                                string caption = question.caption;
                                if (string.IsNullOrEmpty(caption)) {
                                    caption = question.name;
                                }
                                pageQuestionList.Add(new PageQuestionViewModel() {
                                    caption = caption,
                                    name = question.name,
                                    currentValue = "",
                                    inputtype = FormQuestionModel.getInputTypeName(question.inputTypeId),
                                    @required = question.@required,
                                    headline = question.headline,
                                    fielddescription = question.description,
                                    isCheckbox = false,
                                    isDefault = false,
                                    isTextArea = false,
                                    isRadio = true,
                                    isSelect = false,
                                    isFile = false,
                                    id = question.id,
                                    optionList = optionList,
                                    formQuestionEditLink = fieldEditLink,
                                    formQuestionEditWrapperClass = resultViewData.isEditing ? "ccEditWrapper" : "",
                                    invalidAnswer = savedAnswers_Page_Question.invalidAnswer,
                                    isReadOnly = isMultipagePreviewMode
                                });
                                break;
                            }
                        case (int)FormQuestionModel.inputTypeEnum.select: {
                                string caption = question.caption;
                                if (string.IsNullOrEmpty(caption)) {
                                    caption = question.name;
                                }
                                pageQuestionList.Add(new PageQuestionViewModel() {
                                    caption = caption,
                                    name = question.name,
                                    currentValue = "",
                                    inputtype = FormQuestionModel.getInputTypeName(question.inputTypeId),
                                    @required = question.@required,
                                    headline = question.headline,
                                    fielddescription = question.description,
                                    isCheckbox = false,
                                    isDefault = false,
                                    isTextArea = false,
                                    isRadio = false,
                                    isSelect = true,
                                    isFile = false,
                                    id = question.id,
                                    optionList = optionList,
                                    formQuestionEditLink = fieldEditLink,
                                    formQuestionEditWrapperClass = resultViewData.isEditing ? "ccEditWrapper" : "",
                                    invalidAnswer = savedAnswers_Page_Question.invalidAnswer,
                                    isReadOnly = isMultipagePreviewMode
                                });
                                break;
                            }
                        case (int)FormQuestionModel.inputTypeEnum.checkbox: {
                                string caption = question.caption;
                                if (string.IsNullOrEmpty(caption)) {
                                    caption = question.name;
                                }
                                pageQuestionList.Add(new PageQuestionViewModel() {
                                    caption = caption,
                                    name = question.name,
                                    currentValue = "",
                                    inputtype = FormQuestionModel.getInputTypeName(question.inputTypeId),
                                    @required = question.@required,
                                    headline = question.headline,
                                    fielddescription = question.description,
                                    isCheckbox = true,
                                    isDefault = false,
                                    isTextArea = false,
                                    isRadio = false,
                                    isSelect = false,
                                    isFile = false,
                                    id = question.id,
                                    optionList = optionList,
                                    formQuestionEditLink = fieldEditLink,
                                    formQuestionEditWrapperClass = resultViewData.isEditing ? "ccEditWrapper" : "",
                                    invalidAnswer = savedAnswers_Page_Question.invalidAnswer,
                                    isReadOnly = isMultipagePreviewMode
                                });
                                break;
                            }
                        case (int)FormQuestionModel.inputTypeEnum.textarea: {
                                string caption = question.caption;
                                if (string.IsNullOrEmpty(caption)) {
                                    caption = question.name;
                                }
                                pageQuestionList.Add(new PageQuestionViewModel() {
                                    caption = caption,
                                    name = question.name,
                                    currentValue = savedAnswers_Page_Question.textAnswer,
                                    inputtype = FormQuestionModel.getInputTypeName(question.inputTypeId),
                                    @required = question.@required,
                                    headline = question.headline,
                                    fielddescription = question.description,
                                    isCheckbox = false,
                                    isDefault = false,
                                    isTextArea = true,
                                    isRadio = false,
                                    isSelect = false,
                                    isFile = false,
                                    id = question.id,
                                    optionList = optionList,
                                    formQuestionEditLink = fieldEditLink,
                                    formQuestionEditWrapperClass = resultViewData.isEditing ? "ccEditWrapper" : "",
                                    invalidAnswer = savedAnswers_Page_Question.invalidAnswer,
                                    isReadOnly = isMultipagePreviewMode
                                });
                                break;
                            }

                        case (int)FormQuestionModel.inputTypeEnum.file: {
                                string caption = question.caption;
                                if (string.IsNullOrEmpty(caption)) {
                                    caption = question.name;
                                }
                                pageQuestionList.Add(new PageQuestionViewModel() {
                                    caption = caption,
                                    name = question.name,
                                    currentFileUrl = string.IsNullOrEmpty(savedAnswers_Page_Question.textAnswer) ? "" : cp.Http.CdnFilePathPrefixAbsolute + savedAnswers_Page_Question.textAnswer,
                                    currentValue = Path.GetFileName(savedAnswers_Page_Question.textAnswer),
                                    inputtype = FormQuestionModel.getInputTypeName(question.inputTypeId),
                                    @required = question.@required,
                                    headline = question.headline,
                                    fielddescription = question.description,
                                    isCheckbox = false,
                                    isDefault = false,
                                    isTextArea = false,
                                    isRadio = false,
                                    isSelect = false,
                                    isFile = true,
                                    id = question.id,
                                    optionList = optionList,
                                    formQuestionEditLink = fieldEditLink,
                                    formQuestionEditWrapperClass = resultViewData.isEditing ? "ccEditWrapper" : "",
                                    invalidAnswer = savedAnswers_Page_Question.invalidAnswer,
                                    isReadOnly = isMultipagePreviewMode
                                });
                                break;
                            }

                        default: {
                                string caption = question.caption;
                                if (string.IsNullOrEmpty(caption)) {
                                    caption = question.name;
                                }
                                pageQuestionList.Add(new PageQuestionViewModel() {
                                    caption = caption,
                                    name = question.name,
                                    currentValue = savedAnswers_Page_Question.textAnswer,
                                    inputtype = FormQuestionModel.getInputTypeName(question.inputTypeId),
                                    @required = question.@required,
                                    headline = question.headline,
                                    fielddescription = question.description,
                                    isCheckbox = false,
                                    isDefault = true,
                                    isTextArea = false,
                                    isRadio = false,
                                    isSelect = false,
                                    id = question.id,
                                    optionList = optionList,
                                    formQuestionEditLink = fieldEditLink,
                                    formQuestionEditWrapperClass = resultViewData.isEditing ? "ccEditWrapper" : "",
                                    invalidAnswer = savedAnswers_Page_Question.invalidAnswer,
                                    isReadOnly = isMultipagePreviewMode
                                });
                                break;
                            }
                    }
                    questionPtr += 1;
                }
                resultViewData.formPageList.Add(new FormPageViewModel {
                    pageDescription = page.description,
                    pageQuestionList = pageQuestionList,
                    formPageEditWrapperClass = isEditing ? "ccEditWrapper" : "",
                    formPageEditLink = cp.Content.GetEditLink(FormPageModel.tableMetadata.contentName, page.id.ToString(), false, "", resultViewData.isEditing),
                    formQuestionAddLink = cp.Content.GetAddLink(FormQuestionModel.tableMetadata.contentName, "formid=" + page.id, false, resultViewData.isEditing),
                    formAddLink = cp.Content.GetAddLink(FormPageModel.tableMetadata.contentName, "formid=" + form.id, false, resultViewData.isEditing),
                    previousButton = page == resultViewData.pageList.First() ? "" : string.IsNullOrEmpty(form.backButtonName) ? "Previous" : form.backButtonName,
                    resetButton = !form.addResetButton ? "" : string.IsNullOrEmpty(form.resetButtonName) ? "Reset" : form.resetButtonName,
                    submitButton = page != resultViewData.pageList.Last() ? "" : string.IsNullOrEmpty(form.submitButtonName) ? "Submit" : form.submitButtonName,
                    continueButton = page == resultViewData.pageList.Last() ? "" : string.IsNullOrEmpty(form.continueButtonName) ? "Continue" : form.continueButtonName,
                    saveButton = !form.useUserProperty ? "" : string.IsNullOrEmpty(form.saveButtonName) ? "Save" : form.saveButtonName
                });
                if (!resultViewData.isMultipagePreviewMode) {
                    //
                    // -- if not multipage mode, this is the one page to display. Add it to the list and exit
                    break;
                }
            }
            resultViewData.formPageAddLink = cp.Content.GetAddLink(FormPageModel.tableMetadata.contentName, "formid=" + form.id, false, resultViewData.isEditing);
            return resultViewData;
        }

        private static void populateFormSelections(CPBaseClass cp, FormWidgetViewModel asdf) {
            using (DataTable dt = cp.Db.ExecuteQuery("select id, name from ccForms order by name")) {
                foreach (DataRow row in dt.Rows) {
                    asdf._selectOptions.Add(new NameValueSelected(cp.Utils.EncodeText(row["name"]), cp.Utils.EncodeInteger(row["id"]).ToString(), false));
                }
            }
        }
        //
        /// <summary>
        /// this create method is used for the application scoring widget to see another users form response
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="widget"></param>
        /// <param name="form"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static FormWidgetViewModel createForScoringWidget(CPBaseClass cp, ApplicationScoreWidgetsModel widget, FormModel form, int userId) {
            try {
                //
                cp.Log.Debug($"aoFormWizard.FormSetViewModel.create() start");
                // 
                // -- process form request, the request includes the srcPageId that needs to be processed
                string userFormResponseSql = (userId > 0) ? $"memberid = {userId}" : form.useUserProperty ? $"memberid = {cp.User.Id}" : $"visitid={cp.Visit.Id}";
                FormResponseModel userFormResponse = DbBaseModel.createFirstOfList<FormResponseModel>(cp, userFormResponseSql, "id desc");
                //
                // -- process the request
                processRequest(cp, form.id, ref userFormResponse);
                //
                // -- validate the savedAnswers object
                FormResponseDataModel savedAnswers = string.IsNullOrEmpty(userFormResponse?.formResponseData) ? new() : cp.JSON.Deserialize<FormResponseDataModel>(userFormResponse.formResponseData);
                savedAnswers.pageDict ??= [];
                savedAnswers.activity ??= [];
                //
                // -- validate the current page
                var pageList = FormPageModel.getPageList(cp, form.id);
                var currentpage = pageList.Find((x) => x.id == savedAnswers.currentPageid);
                if (currentpage is null) {
                    savedAnswers.currentPageid = pageList.First().id;
                }
                // 
                // -- begin create output data
                var formWidgetViewData = create<FormWidgetViewModel>(cp, widget);
                formWidgetViewData.id = form.id;
                formWidgetViewData.instanceId = form.ccguid;
                formWidgetViewData.formHtmlId = string.IsNullOrEmpty(("formHtmlId")) ? cp.Utils.GetRandomString(4) : ("formHtmlId");
                formWidgetViewData.srcPageId = savedAnswers.currentPageid;
                formWidgetViewData.allowRecaptcha = false;
                formWidgetViewData.recaptchaHTML = "";
                formWidgetViewData.isEditing = cp.User.IsEditing();
                formWidgetViewData.pageList = pageList;
                formWidgetViewData.formPageList = [];

                if (pageList.Count <= 0) { return formWidgetViewData; }
                //
                var pageQuestionList = new List<FormWidgetViewModel.PageQuestionViewModel>();
                // 
                // -- output one page with page one header
                foreach (FormPageModel page in pageList) {
                    //
                    //-- skip to the current page
                    if (page.id != savedAnswers.currentPageid && !formWidgetViewData.isEditing) { continue; }
                    //
                    // -- recapcha
                    if (form.allowRecaptcha && !formWidgetViewData.isEditing && !savedAnswers.recaptchaSuccess) {
                        //
                        cp.Log.Debug($"aoFormWizard.FormSetViewModel.create(), add recaptcha");
                        // 
                        formWidgetViewData.allowRecaptcha = form.allowRecaptcha;
                        //const string recaptchaDisplayAddonGuid = "{E9E51C6E-9152-4284-A44F-D3ABC423AB90}";
                        //formViewData.recaptchaHTML = cp.Addon.Execute(recaptchaDisplayAddonGuid);
                        formWidgetViewData.recaptchaHTML = cp.Addon.Execute(Constants.guidAddonRecaptchav2);
                        if (cp.UserError.OK()) {
                            savedAnswers.recaptchaSuccess = true;
                        }
                        //
                        cp.Log.Debug($"aoFormWizard.FormSetViewModel.create(), return recaptchaHTML [{cp.Utils.EncodeHTML(formWidgetViewData.recaptchaHTML)}]");
                        //
                    }
                    //
                    FormResponseDataPageModel savedAnswers_Page = savedAnswers.pageDict.TryGetValue(page.id, out var savedAnswers_Page_Result) ? savedAnswers_Page_Result : new FormResponseDataPageModel();
                    savedAnswers_Page.questionDict ??= [];
                    //
                    int questionPtr = 0;
                    var questionList = FormQuestionModel.getQuestionList(cp, page.id);
                    foreach (var question in questionList) {
                        FormResponseDataPageQuestionModel savedAnswers_Page_Question = savedAnswers_Page.questionDict.TryGetValue(question.id, out var savedAnswers_Page_Question_Result) ? savedAnswers_Page_Question_Result : new FormResponseDataPageQuestionModel();
                        savedAnswers_Page_Question.choiceAnswerDict ??= [];
                        //
                        var optionList = new List<OptionClass>();
                        int optionPtr = 1;
                        foreach (var questionOptionName in question.optionList.Split(',')) {
                            if (string.IsNullOrEmpty(questionOptionName)) { continue; }
                            //
                            optionList.Add(new OptionClass() {
                                optionName = questionOptionName,
                                optionPtr = optionPtr,
                                isSelected = savedAnswers_Page_Question.choiceAnswerDict.ContainsKey(questionOptionName) ? savedAnswers_Page_Question.choiceAnswerDict[questionOptionName] : false
                            });
                            optionPtr += 1;
                        }
                        string fieldEditLink = cp.Content.GetEditLink(FormQuestionModel.tableMetadata.contentName, question.id.ToString(), false, "Edit Question", formWidgetViewData.isEditing);
                        //
                        cp.Utils.AppendLog($"form-widget, FormViewModel.create, page [{page.id}], question [{question.id}], fieldEditLink [{fieldEditLink}]");
                        //
                        switch (question.inputTypeId) {
                            case (int)FormQuestionModel.inputTypeEnum.radio: {
                                    string caption = question.caption;
                                    if (string.IsNullOrEmpty(caption)) {
                                        caption = question.name;
                                    }
                                    pageQuestionList.Add(new PageQuestionViewModel() {
                                        caption = caption,
                                        name = question.name,
                                        currentValue = "",
                                        inputtype = FormQuestionModel.getInputTypeName(question.inputTypeId),
                                        @required = question.@required,
                                        headline = question.headline,
                                        fielddescription = question.description,
                                        isCheckbox = false,
                                        isDefault = false,
                                        isTextArea = false,
                                        isRadio = true,
                                        isSelect = false,
                                        isFile = false,
                                        id = question.id,
                                        optionList = optionList,
                                        formQuestionEditLink = fieldEditLink,
                                        formQuestionEditWrapperClass = formWidgetViewData.isEditing ? "ccEditWrapper" : "",
                                        invalidAnswer = savedAnswers_Page_Question.invalidAnswer
                                    });
                                    break;
                                }
                            case (int)FormQuestionModel.inputTypeEnum.select: {
                                    string caption = question.caption;
                                    if (string.IsNullOrEmpty(caption)) {
                                        caption = question.name;
                                    }
                                    pageQuestionList.Add(new PageQuestionViewModel() {
                                        caption = caption,
                                        name = question.name,
                                        currentValue = "",
                                        inputtype = FormQuestionModel.getInputTypeName(question.inputTypeId),
                                        @required = question.@required,
                                        headline = question.headline,
                                        fielddescription = question.description,
                                        isCheckbox = false,
                                        isDefault = false,
                                        isTextArea = false,
                                        isRadio = false,
                                        isSelect = true,
                                        isFile = false,
                                        id = question.id,
                                        optionList = optionList,
                                        formQuestionEditLink = fieldEditLink,
                                        formQuestionEditWrapperClass = formWidgetViewData.isEditing ? "ccEditWrapper" : "",
                                        invalidAnswer = savedAnswers_Page_Question.invalidAnswer
                                    });
                                    break;
                                }
                            case (int)FormQuestionModel.inputTypeEnum.checkbox: {
                                    string caption = question.caption;
                                    if (string.IsNullOrEmpty(caption)) {
                                        caption = question.name;
                                    }
                                    pageQuestionList.Add(new PageQuestionViewModel() {
                                        caption = caption,
                                        name = question.name,
                                        currentValue = "",
                                        inputtype = FormQuestionModel.getInputTypeName(question.inputTypeId),
                                        @required = question.@required,
                                        headline = question.headline,
                                        fielddescription = question.description,
                                        isCheckbox = true,
                                        isDefault = false,
                                        isTextArea = false,
                                        isRadio = false,
                                        isSelect = false,
                                        isFile = false,
                                        id = question.id,
                                        optionList = optionList,
                                        formQuestionEditLink = fieldEditLink,
                                        formQuestionEditWrapperClass = formWidgetViewData.isEditing ? "ccEditWrapper" : "",
                                        invalidAnswer = savedAnswers_Page_Question.invalidAnswer
                                    });
                                    break;
                                }
                            case (int)FormQuestionModel.inputTypeEnum.textarea: {
                                    string caption = question.caption;
                                    if (string.IsNullOrEmpty(caption)) {
                                        caption = question.name;
                                    }
                                    pageQuestionList.Add(new PageQuestionViewModel() {
                                        caption = caption,
                                        name = question.name,
                                        currentValue = savedAnswers_Page_Question.textAnswer,
                                        inputtype = FormQuestionModel.getInputTypeName(question.inputTypeId),
                                        @required = question.@required,
                                        headline = question.headline,
                                        fielddescription = question.description,
                                        isCheckbox = false,
                                        isDefault = false,
                                        isTextArea = true,
                                        isRadio = false,
                                        isSelect = false,
                                        isFile = false,
                                        id = question.id,
                                        optionList = optionList,
                                        formQuestionEditLink = fieldEditLink,
                                        formQuestionEditWrapperClass = formWidgetViewData.isEditing ? "ccEditWrapper" : "",
                                        invalidAnswer = savedAnswers_Page_Question.invalidAnswer
                                    });
                                    break;
                                }

                            case (int)FormQuestionModel.inputTypeEnum.file: {
                                    string caption = question.caption;
                                    if (string.IsNullOrEmpty(caption)) {
                                        caption = question.name;
                                    }
                                    pageQuestionList.Add(new PageQuestionViewModel() {
                                        caption = caption,
                                        name = question.name,
                                        currentFileUrl = string.IsNullOrEmpty(savedAnswers_Page_Question.textAnswer) ? "" : cp.Http.CdnFilePathPrefixAbsolute + savedAnswers_Page_Question.textAnswer,
                                        currentValue = Path.GetFileName(savedAnswers_Page_Question.textAnswer),
                                        inputtype = FormQuestionModel.getInputTypeName(question.inputTypeId),
                                        @required = question.@required,
                                        headline = question.headline,
                                        fielddescription = question.description,
                                        isCheckbox = false,
                                        isDefault = false,
                                        isTextArea = false,
                                        isRadio = false,
                                        isSelect = false,
                                        isFile = true,
                                        id = question.id,
                                        optionList = optionList,
                                        formQuestionEditLink = fieldEditLink,
                                        formQuestionEditWrapperClass = formWidgetViewData.isEditing ? "ccEditWrapper" : "",
                                        invalidAnswer = savedAnswers_Page_Question.invalidAnswer
                                    });
                                    break;
                                }

                            default: {
                                    string caption = question.caption;
                                    if (string.IsNullOrEmpty(caption)) {
                                        caption = question.name;
                                    }
                                    pageQuestionList.Add(new PageQuestionViewModel() {
                                        caption = caption,
                                        name = question.name,
                                        currentValue = savedAnswers_Page_Question.textAnswer,
                                        inputtype = FormQuestionModel.getInputTypeName(question.inputTypeId),
                                        @required = question.@required,
                                        headline = question.headline,
                                        fielddescription = question.description,
                                        isCheckbox = false,
                                        isDefault = true,
                                        isTextArea = false,
                                        isRadio = false,
                                        isSelect = false,
                                        id = question.id,
                                        optionList = optionList,
                                        formQuestionEditLink = fieldEditLink,
                                        formQuestionEditWrapperClass = formWidgetViewData.isEditing ? "ccEditWrapper" : "",
                                        invalidAnswer = savedAnswers_Page_Question.invalidAnswer
                                    });
                                    break;
                                }
                        }
                        questionPtr += 1;
                    }
                    //formViewData.fieldAddLink = cp.Content.GetAddLink(FormQuestionModel.tableMetadata.contentName, "formid=" + page.id, false, formViewData.isEditing);
                    //
                    // -- the rendering of this form page is complete. If not editing, exit with just one page
                    if (!formWidgetViewData.isEditing) {
                        break;
                    } else {
                        var currentEditingPage = new FormPageViewModel {
                            pageDescription = page.description,
                            pageQuestionList = pageQuestionList,
                            formPageEditWrapperClass = cp.User.IsEditing() ? " ccEditWrapper" : "",
                            formPageEditLink = cp.Content.GetEditLink(FormPageModel.tableMetadata.contentName, page.id.ToString(), false, "", formWidgetViewData.isEditing),
                            formQuestionAddLink = cp.Content.GetAddLink(FormQuestionModel.tableMetadata.contentName, "formid=" + page.id, false, formWidgetViewData.isEditing),
                            formAddLink = cp.Content.GetAddLink(FormPageModel.tableMetadata.contentName, "formid=" + form.id, false, formWidgetViewData.isEditing),
                            previousButton = page == pageList.First() ? "" : string.IsNullOrEmpty(form.backButtonName) ? "Previous" : form.backButtonName,
                            resetButton = !form.addResetButton ? "" : string.IsNullOrEmpty(form.resetButtonName) ? "Reset" : form.resetButtonName,
                            submitButton = page != pageList.Last() ? "" : string.IsNullOrEmpty(form.submitButtonName) ? "Submit" : form.submitButtonName,
                            continueButton = page == pageList.Last() ? "" : string.IsNullOrEmpty(form.continueButtonName) ? "Continue" : form.continueButtonName,
                            saveButton = !form.useUserProperty ? "" : string.IsNullOrEmpty(form.saveButtonName) ? "Save" : form.saveButtonName
                        };
                        formWidgetViewData.formPageList.Add(currentEditingPage);
                    }
                }
                formWidgetViewData.formPageAddLink = cp.Content.GetAddLink(FormPageModel.tableMetadata.contentName, "formid=" + form.id, false, formWidgetViewData.isEditing);
                return formWidgetViewData;
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                return null;
            }
        }

        /// <summary>
        /// Process submitted contact form. 
        /// Returns true if the form has already been submitted, or successfully commits
        /// process the page in the request "srcPageId"
        /// set the savedAnswers.currentPageid to the next page
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="formId"></param>
        /// <param name="userFormResponse"></param>
        /// <returns></returns>
        public static bool processRequest(CPBaseClass cp, int formId, ref FormResponseModel userFormResponse) {
            string returnHtml = string.Empty;
            int hint = 0;
            try {
                string button = cp.Doc.GetText("button");
                int srcPageId = cp.Doc.GetInteger("srcPageId");
                //
                // -- handle no button
                if (string.IsNullOrWhiteSpace(button) || srcPageId.Equals(0)) { return false; }
                //
                var form = DbBaseModel.create<FormModel>(cp, formId);
                //form ??= FormModel.createFormFromWizard(cp, formWidget);
                //
                // -- handle reset button
                string resetButton = string.IsNullOrEmpty(form.resetButtonName) ? "Reset" : form.resetButtonName;
                if (userFormResponse != null && form.addResetButton && !string.IsNullOrEmpty(resetButton) && button.Equals(resetButton)) {
                    DbBaseModel.delete<FormResponseModel>(cp, userFormResponse.ccguid);
                    userFormResponse = null;
                    return false;
                }
                //
                // -- verify the users response
                if (userFormResponse is null) {
                    userFormResponse = DbBaseModel.addDefault<FormResponseModel>(cp);
                    userFormResponse.formId = form.id;
                    userFormResponse.dateAdded ??= DateTime.Now;
                    userFormResponse.name = $"Form {form.name} started {userFormResponse.dateAdded.Value.ToString("g")} by {cp.User.Name}";
                }
                userFormResponse.formId = form.id;
                userFormResponse.visitid = cp.Visit.Id;
                userFormResponse.memberId = cp.User.Id;
                //
                // -- determine the page to process
                List<FormPageModel> pageList = FormPageModel.getPageList(cp, form.id);
                var currentPage = pageList.Find((x) => x.id == srcPageId);
                if (currentPage is null) {
                    //
                    // -- invalid page, exit no processing
                    return false;
                }
                //
                // -- setup the savedAnswers
                FormResponseDataModel savedAnswers = null;
                if (!string.IsNullOrEmpty(userFormResponse.formResponseData)) { savedAnswers = cp.JSON.Deserialize<FormResponseDataModel>(userFormResponse.formResponseData); }
                if (savedAnswers is null) { savedAnswers = new FormResponseDataModel(); }
                if (savedAnswers.pageDict is null) { savedAnswers.pageDict = []; }
                if (savedAnswers.activity is null) { savedAnswers.activity = []; }
                if (!savedAnswers.pageDict.TryGetValue(currentPage.id, out FormResponseDataPageModel savedAnswersForm_Page)) {
                    savedAnswersForm_Page = new FormResponseDataPageModel {
                        questionDict = []
                    };
                    savedAnswers.pageDict.Add(currentPage.id, savedAnswersForm_Page);
                }
                //
                // -- handle previous button
                string backButton = string.IsNullOrEmpty(form.backButtonName) ? "Previous" : form.backButtonName;
                if (button == backButton) {
                    if (currentPage == pageList.First()) { return false; }
                    var previousPage = pageList.First();
                    foreach (var page in pageList) {
                        if (page == currentPage) { break; }
                        previousPage = page;
                    }
                    savedAnswers.currentPageid = previousPage.id;
                    // -- mark the previous page not complete
                    if (!savedAnswers.pageDict.TryGetValue(currentPage.id, out FormResponseDataPageModel savedAnswersForm_Page2)) {
                        savedAnswersForm_Page2 = new FormResponseDataPageModel {
                            questionDict = []
                        };
                        savedAnswers.pageDict.Add(currentPage.id, savedAnswersForm_Page);
                    }
                    savedAnswersForm_Page2.isCompleted = false;
                    savedAnswersForm_Page2.isStarted = true;
                    userFormResponse.formResponseData = cp.JSON.Serialize(savedAnswers);
                    userFormResponse.save(cp);
                    return true;
                }
                //
                // -- legacy results
                var htmlVersion = new StringBuilder();
                var textVersion = new StringBuilder();
                //
                //// remove any bad characters from the custom content name
                //string customContentName = form.saveCustomContent;
                //// replace custom content name with no nonalphanumeric characters
                //// includes spaces since this is content "[^A-Za-z0-9 ]+"
                //customContentName = Regex.Replace(customContentName, "[^A-Za-z0-9 ]+", "");
                //
                bool returnInvalidAnswer = false;
                foreach (var formPage_Question in FormQuestionModel.getQuestionList(cp, currentPage.id)) {
                    if (!savedAnswersForm_Page.questionDict.TryGetValue(formPage_Question.id, out FormResponseDataPageQuestionModel savedAnswersForm_Page_Question)) {
                        savedAnswersForm_Page_Question = new FormResponseDataPageQuestionModel {
                            question = formPage_Question.name,
                            textAnswer = "",
                            choiceAnswerDict = [],
                        };
                        savedAnswersForm_Page.questionDict.Add(formPage_Question.id, savedAnswersForm_Page_Question);
                    }
                    string requestAnswer_Text = cp.Doc.GetText("formField_" + formPage_Question.id);
                    //
                    // -- invalid request. Mark the field and continue to check all the fields, then return
                    savedAnswersForm_Page_Question.invalidAnswer = false;
                    //
                    //
                    //// remove any bad characters from the customfieldname
                    //string customFieldName = formsField.name;
                    //// do not include spaces in the field name "[^A-Za-z0-9]+"
                    //customFieldName = Regex.Replace(customFieldName, "[^A-Za-z0-9]+", "");
                    //
                    //
                    // -- move this into a method called after save is complete, on the formResponseData field
                    textVersion.Append($"{Environment.NewLine}Question: " + formPage_Question.name);
                    htmlVersion.Append("<div style=\"padding-top:10px;\"> Question:" + formPage_Question.name + "</div>");
                    //
                    switch (formPage_Question.inputTypeId) {
                        case (int)FormQuestionModel.inputTypeEnum.checkbox:
                        case (int)FormQuestionModel.inputTypeEnum.radio:
                        case (int)FormQuestionModel.inputTypeEnum.select: {
                                hint = 3;
                                if (formPage_Question.required && string.IsNullOrEmpty(requestAnswer_Text)) {
                                    //
                                    // -- invalid request. Mark the field and continue to check all the fields
                                    returnInvalidAnswer = true;
                                    savedAnswersForm_Page_Question.invalidAnswer = true;
                                }
                                var requestAnswer_OptionsSelectedList = new List<string>(requestAnswer_Text.Split(','));
                                int optionPtr = 1;
                                var requestAnswer_OptionsSelectedList_Names = new List<string>();
                                foreach (var formPage_Question_Option in formPage_Question.optionList.Split(',')) {
                                    bool isSelected = requestAnswer_OptionsSelectedList.Contains(optionPtr.ToString());
                                    //
                                    // -- save answer
                                    if (!savedAnswersForm_Page_Question.choiceAnswerDict.ContainsKey(formPage_Question_Option)) {
                                        savedAnswersForm_Page_Question.choiceAnswerDict.Add(formPage_Question_Option, isSelected);
                                    } else {
                                        savedAnswersForm_Page_Question.choiceAnswerDict[formPage_Question_Option] = isSelected;
                                    }
                                    if (isSelected) {
                                        //
                                        // -- answer is true (found in request)
                                        //
                                        // -- move this into a method called after save is complete, on the formResponseData field
                                        textVersion.Append(Microsoft.VisualBasic.Constants.vbCrLf + Microsoft.VisualBasic.Constants.vbTab + formPage_Question_Option);
                                        htmlVersion.Append("<div style=\"padding-left:20px;\">" + formPage_Question_Option + "</div>");
                                        //
                                        requestAnswer_OptionsSelectedList_Names.Add(formPage_Question_Option);
                                    }
                                    optionPtr += 1;
                                }
                                break;
                            }
                        case (int)FormQuestionModel.inputTypeEnum.file: {
                                hint = 20;
                                string fieldName = "formField_" + formPage_Question.id;
                                string uploadFilename = cp.Doc.GetText(fieldName);
                                if (!string.IsNullOrEmpty(uploadFilename)) {
                                    hint = 21;
                                    var folder = DbBaseModel.createByUniqueName<LibraryFolderModel>(cp, "Form Wizard Uploads");
                                    if (folder is null) {
                                        hint = 22;
                                        folder = DbBaseModel.addDefault<LibraryFolderModel>(cp);
                                        folder.name = "Form Wizard Uploads";
                                        folder.save(cp);
                                    }
                                    string pathFilename = "";
                                    using (var cs = cp.CSNew()) {
                                        hint = 23;
                                        cs.Insert("Library Files");
                                        cs.SetFormInput("filename", fieldName);
                                        cs.SetField("folderid", folder.id);
                                        cs.SetField("description", "Form wizard upload by " + cp.User.Name);
                                        cs.Save();
                                        // 
                                        pathFilename = cs.GetText("filename");
                                        var fileDetails = cp.CdnFiles.FileDetails(pathFilename);
                                        cs.SetField("name", fileDetails.Name);
                                        cs.SetField("filesize", fileDetails.Size);
                                        cs.Save();
                                    }
                                    savedAnswersForm_Page_Question.textAnswer = pathFilename;
                                    // 
                                    //if (formPage.useauthorgcontent) {
                                    //    hint = 24;
                                    //    if (CP.Content.IsField("Organizations", formsField.name)) {
                                    //        // make sure the form's field exists in the people table
                                    //        using (var cs2 = CP.CSNew()) {
                                    //            if (cs2.Open("Organizations", "id=" + CP.User.OrganizationID)) {
                                    //                hint = 25;
                                    //                cs2.SetField(formsField.name, pathFilename);
                                    //                cs2.Save();
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    // custom content
                                    //if (form.saveTypeId.Equals(4) & !string.IsNullOrWhiteSpace(customContentName)) {
                                    //    hint = 30;
                                    //    bool verifiedContent = CustomContentController.verifyCustomContent(CP, customContentName);
                                    //    if (verifiedContent) {
                                    //        // make sure the content has this field
                                    //        if (!CP.Content.IsField(customContentName, customFieldName)) {
                                    //            hint = 31;
                                    //            // create file field
                                    //            CustomContentController.createCustomContentField(CP, customContentName, customFieldName, (int)Constants.FieldTypeIdEnum.File);
                                    //        }
                                    //        using (var csContent = CP.CSNew()) {
                                    //            // after the field should have been created, check again
                                    //            if (CP.Content.IsField(customContentName, customFieldName)) {
                                    //                hint = 32;
                                    //                if (currentAuthContentRecordId == 0) {
                                    //                    hint = 33;
                                    //                    csContent.Insert(customContentName);
                                    //                    currentAuthContentRecordId = csContent.GetInteger("id");
                                    //                }
                                    //                if (csContent.Open(customContentName, "id=" + currentAuthContentRecordId)) {
                                    //                    csContent.SetField(customFieldName, pathFilename);
                                    //                    csContent.Save();
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                }
                                textVersion.Append(Microsoft.VisualBasic.Constants.vbCrLf);
                                if (!string.IsNullOrEmpty(savedAnswersForm_Page_Question.textAnswer)) {
                                    //
                                    // -- invalid request. Mark the field and continue to check all the fields
                                    textVersion.Append(Microsoft.VisualBasic.Constants.vbTab + cp.Http.CdnFilePathPrefixAbsolute + savedAnswersForm_Page_Question.textAnswer);
                                    string filename = Path.GetFileName(savedAnswersForm_Page_Question.textAnswer);
                                    htmlVersion.Append("<div style=\"padding-left:20px;\"><a href=\"" + cp.Http.CdnFilePathPrefixAbsolute + savedAnswersForm_Page_Question.textAnswer + "\">" + filename + "</a></div>");
                                }
                                //
                                // -- test if this is a required field
                                if (formPage_Question.required && string.IsNullOrEmpty(savedAnswersForm_Page_Question.textAnswer)) {
                                    //
                                    // -- invalid request. Mark the field and continue to check all the fields
                                    returnInvalidAnswer = true;
                                    savedAnswersForm_Page_Question.invalidAnswer = true;
                                }
                                break;
                            }

                        default: {
                                savedAnswersForm_Page_Question.textAnswer = requestAnswer_Text;
                                if (formPage_Question.required && string.IsNullOrEmpty(savedAnswersForm_Page_Question.textAnswer)) {
                                    //
                                    // -- invalid request. Mark the field and continue to check all the fields
                                    returnInvalidAnswer = true;
                                    savedAnswersForm_Page_Question.invalidAnswer = true;
                                }
                                //
                                //if (form.saveTypeId.Equals(4) & !string.IsNullOrWhiteSpace(customContentName)) {
                                //    hint = 50;
                                //    hint = 10;
                                //    bool verifiedContent = CustomContentController.verifyCustomContent(CP, customContentName);
                                //    if (verifiedContent) {
                                //        // make sure the content has this field
                                //        if (!CP.Content.IsField(customContentName, customFieldName)) {
                                //            hint = 11;
                                //            // determine which kind of field to create
                                //            switch (formsField.inputtype.ToLower() ?? "") {
                                //                case "text": {
                                //                        // create a text field if this type is for text
                                //                        CustomContentController.createCustomContentField(CP, customContentName, customFieldName, (int)Constants.FieldTypeIdEnum.Text);
                                //                        break;
                                //                    }

                                //                default: {
                                //                        // this is a longtext or unknown so default to longtext
                                //                        CustomContentController.createCustomContentField(CP, customContentName, customFieldName, (int)Constants.FieldTypeIdEnum.LongText);
                                //                        break;
                                //                    }
                                //            }
                                //        }

                                //        using (var cs = CP.CSNew()) {
                                //            // after the field should have been created, check again
                                //            if (CP.Content.IsField(customContentName, customFieldName)) {
                                //                hint = 12;
                                //                if (currentAuthContentRecordId == 0) {
                                //                    hint = 13;
                                //                    cs.Insert(customContentName);
                                //                    currentAuthContentRecordId = cs.GetInteger("id");
                                //                }
                                //                if (cs.Open(customContentName, "id=" + currentAuthContentRecordId)) {
                                //                    cs.SetField(customFieldName, requestQuestionValue);
                                //                }
                                //            }
                                //        }
                                //    }
                                //} else 
                                if (currentPage.saveTypeId.Equals(3)) {
                                    // 
                                    // -- save to organization
                                    if (cp.Content.IsField("organizations", formPage_Question.name)) {
                                        using (var cs = cp.CSNew()) {
                                            // make sure the form's field exists in the people table
                                            if (cs.Open("organizations", "id=" + cp.User.OrganizationID) & cs.FieldOK(formPage_Question.name)) {
                                                cs.SetField(formPage_Question.name, requestAnswer_Text);
                                                cs.Save();
                                            }
                                        }
                                    }
                                } else if (currentPage.saveTypeId.Equals(2)) {
                                    // 
                                    // -- save to people
                                    if (cp.Content.IsField("People", formPage_Question.name)) {
                                        using (var cs = cp.CSNew()) {
                                            // make sure the form's field exists in the people table
                                            if (cs.Open("People", "id=" + cp.User.Id) & cs.FieldOK(formPage_Question.name)) {
                                                cs.SetField(formPage_Question.name, requestAnswer_Text);
                                                cs.Save();
                                            }
                                        }
                                    }
                                }
                                textVersion.Append(Microsoft.VisualBasic.Constants.vbCrLf + Microsoft.VisualBasic.Constants.vbTab + requestAnswer_Text);
                                htmlVersion.Append("<div style=\"padding-left:20px;\">" + requestAnswer_Text + "</div>");
                                break;
                            }
                    }
                }
                //
                if (returnInvalidAnswer) {
                    //
                    // -- one or more invalid answers. Save the invalid answer marker and return
                    userFormResponse.formResponseData = cp.JSON.Serialize(savedAnswers);
                    userFormResponse.save(cp);
                    return false;
                }
                //
                // -- continue to next page
                //
                // -- if last question done processing, mark this form-page complete
                string continueButton = string.IsNullOrEmpty(form.continueButtonName) ? "Continue" : form.continueButtonName;
                string saveButton = string.IsNullOrEmpty(form.saveButtonName) ? "Save" : form.saveButtonName;
                string submitButton = string.IsNullOrEmpty(form.submitButtonName) ? "Submit" : form.submitButtonName;
                if (!string.IsNullOrEmpty(button) && (button == continueButton || button == submitButton || button == saveButton)) {
                    //
                    // -- mark this page complete
                    if (button != saveButton) {
                        savedAnswersForm_Page.isCompleted = true;
                    }
                    if (currentPage == pageList.Last()) {
                        //
                        // -- this was the last page, go to thankyou page
                        userFormResponse.dateSubmitted = DateTime.Now;
                        savedAnswers.isComplete = true;
                        savedAnswers.currentPageid = 0;
                    } else if (button != saveButton) {
                        //
                        // -- go to the next page
                        bool setNext = false;
                        foreach (var page in pageList) {
                            if (setNext) {
                                savedAnswers.currentPageid = page.id;
                                break;
                            }
                            if (page.id == currentPage.id) {
                                setNext = true;
                            }
                        }
                    }
                }
                if (currentPage == pageList.Last() && (button == submitButton)) {
                    //
                    // -- the entire form is complete
                    //
                    cp.Email.sendSystem(form.notificationemailid, htmlVersion.ToString());
                    if (form.responseemailid > 0) {
                        cp.Email.sendSystem(form.responseemailid, "", cp.User.Id);
                    }
                    if (form.joingroupid != 0) {
                        cp.Group.AddUser(form.joingroupid, cp.User.Id);
                    }
                }



                userFormResponse.formResponseData = cp.JSON.Serialize(savedAnswers);
                userFormResponse.copy = textVersion.ToString();
                userFormResponse.save(cp);
                return true;
            } catch (Exception ex) {
                cp.Site.ErrorReport("hint=" + hint.ToString() + " " + ex.ToString());
                return false;
            }
        }
    }
}
