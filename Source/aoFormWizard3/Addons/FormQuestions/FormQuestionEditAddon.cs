using Contensive.FormWidget.Controllers;
using Contensive.FormWidget.Models.Db;
using Contensive.FormWidget.Models.Domain;
using Contensive.BaseClasses;
using Contensive.Models.Db;
using System;

namespace Contensive.FormWidget.Addons {
    //
    // ========================================================================================
    /// <summary>
    /// Meetings
    /// </summary>
    /// <remarks></remarks>
    public class FormQuestionEditAddon : AddonBaseClass {
        //
        public const string guidPortalFeature = "{e5da4e7c-6e4b-48b8-8b88-3e4c9538733c}";
        public const string guidAddon = "{2cb0f9fa-0573-4c71-a785-1e0798af4989}";
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
        // ====================================================================================================
        /// <summary>
        /// Process the view
        /// </summary>
        /// <param name="app"></param>
        /// <param name="cp"></param>
        /// <param name="request"></param>
        /// <param name="errorMessage"></param>
        public static bool processView(ApplicationModel app, RequestModel request, ref string errorMessage) {
            CPBaseClass cp = app.cp;
            try {
                // 
                // -- validate portal environment
                if (!cp.AdminUI.EndpointContainsPortal()) {
                    RedirectController.redirectToFormQuestionEdit(cp, request.formId, request.formPageId, request.formQuestionId);
                    return false;
                }
                // 
                // -- cancel button
                if (request.button.Equals(Constants.buttonCancel)) {
                    RedirectController.redirectToFormQuestionList(cp, request.formId, request.formPageId);
                    return false;
                }
                //
                // -- form pagewidget required
                if (request.formPageId <= 0) {
                    RedirectController.redirectToFormPageList(cp, request.formId);
                    return false;
                }
                //
                // --form required
                if (request.formId <= 0) {
                    RedirectController.redirectToFormList(cp);
                    return false;
                }
                // 
                // -- save button
                if (request.button.Equals(Constants.buttonSave)) {
                    saveForm(cp, request);
                    return true;
                }
                // 
                // -- OK button
                if (request.button.Equals(Constants.buttonOK)) {
                    saveForm(cp, request);
                    RedirectController.redirectToFormQuestionList(cp, request.formId, request.formPageId);
                    return false;
                }
                // 
                // -- delete button
                if (request.button.Equals(Constants.buttonDelete)) {
                    foreach (var formQuestion in DbBaseModel.createList<FormQuestionModel>(cp, $"formid={request.formPageId}")) {
                        DbBaseModel.delete<FormQuestionModel>(cp, formQuestion.id);
                    }
                    DbBaseModel.delete<FormPageModel>(cp, request.formPageId);
                    RedirectController.redirectToFormQuestionList(cp, request.formId, request.formPageId);
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
        public static string getView(ApplicationModel app, RequestModel request, string userErrorMessage) {
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
                FormPageModel formPage = DbBaseModel.create<FormPageModel>(cp, request.formPageId);
                if (formPage == null) {
                    RedirectController.redirectToFormPageList(cp, request.formId);
                    return "";
                }
                FormQuestionModel formQuestion = DbBaseModel.create<FormQuestionModel>(cp, request.formQuestionId);
                //
                // -- add rows
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Name";
                layoutBuilder.rowValue = cp.Html5.InputText(Constants.rnName, 255, formQuestion?.name ?? "", "form-control");
                layoutBuilder.rowHelp = "Not displayed on the site. This is a short name you give to this question so you can find it easily later.";
                //
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Question";
                layoutBuilder.rowValue = cp.Html5.InputText("caption", 255, formQuestion?.caption ?? "", "form-control");
                layoutBuilder.rowHelp = "The question you are asking (h3 headline).";
                //
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Description";
                layoutBuilder.rowValue = cp.Html5.InputHtml("description", 300, formQuestion?.description ?? "");
                layoutBuilder.rowHelp = "Optional. This description appears as regular size text below the question and above the answer area. Use this area for instructions and description to help the user understand the input required.";
                //
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Required";
                layoutBuilder.rowValue = cp.Html5.CheckBox("required", formQuestion?.required ?? false);
                layoutBuilder.rowHelp = "Check this box if the field is required for the form to be submitted.";
                //
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Question Type";
                layoutBuilder.rowValue = cp.Html5.SelectList("inputTypeId", formQuestion?.inputTypeId.ToString() ?? "", "Short Text Answer,Long Text Answer,Check Boxes choose one,Radio Boxes choose many,Upload File,Select List choose one", "form-control");
                layoutBuilder.rowHelp = "The type of field: Text for short text fields, TextArea for longer text fields, Radio (to choose one of the listed options), Checkbox (to select multiple listed options), Select (to select one listed option in a dropdown) and File (to upload an image file).";
                //
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Option List";
                layoutBuilder.rowValue = cp.Html5.InputTextArea("optionList", 255, formQuestion?.optionList ?? "", "form-control");
                layoutBuilder.rowHelp = "A short list of choices separated by commas for radio or checkbox fields. For example to create a choice between red and blue, enter \"red,blue\" (without the quotes).";
                //
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Section Headline";
                layoutBuilder.rowValue = cp.Html5.InputText("headline", 255, formQuestion?.headline ?? "", "form-control");
                layoutBuilder.rowHelp = "An optional large bold (h2 headline) caption that appears above the question. Use a headline to divide a form's questions into sections where appropriate.";
                //
                // -- setup layout
                layoutBuilder.title = (formQuestion == null) ? "Add Form Question" : "Edit Form Question";
                layoutBuilder.portalSubNavTitleList.Add($"form: '{form.name}'");
                layoutBuilder.portalSubNavTitleList.Add((formPage == null ? "New Page" : $"page: {formPage.name}"));
                layoutBuilder.portalSubNavTitleList.Add((formQuestion == null ? "New Question" : $"question: {formQuestion.name}"));
                layoutBuilder.description = "A form question is a single question presented to the user on a form page. Each form page can contain one or more questions. A form can include one or more for pages.";
                layoutBuilder.callbackAddonGuid = guidAddon;
                layoutBuilder.failMessage = userErrorMessage;
                // 
                // -- add buttons
                layoutBuilder.addFormButton(Constants.buttonOK);
                layoutBuilder.addFormButton(Constants.buttonSave);
                layoutBuilder.addFormButton(Constants.buttonCancel);
                layoutBuilder.addFormButton(Constants.buttonDelete);
                // 
                // -- add hiddens
                //
                // -- set rqs for subnav links
                cp.Doc.AddRefreshQueryString(Constants.rnFormId, request.formId);
                cp.Doc.AddRefreshQueryString(Constants.rnFormPageId, request.formPageId);
                cp.Doc.AddRefreshQueryString(Constants.rnFormQuestionId, request.formQuestionId);
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
                var formQuestion = DbBaseModel.create<FormQuestionModel>(cp, request.formQuestionId);
                if (formQuestion is null) {
                    formQuestion = DbBaseModel.addDefault<FormQuestionModel>(cp);
                    //
                    // -- important. this record becomes the current focus for the get method
                    request.formQuestionId = formQuestion.id;
                }
                formQuestion.name = request.name;
                formQuestion.headline = request.headline;
                formQuestion.caption = request.caption;
                formQuestion.description = request.description;
                formQuestion.required = request.required;
                formQuestion.inputTypeId = request.inputTypeId;
                formQuestion.optionList = request.optionList;
                formQuestion.formid = request.formPageId;
                formQuestion.save(cp);
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
                // -- initialize properties (cannot use default constructor)
                button = cp.Doc.GetText(Constants.rnButton);
                formId = cp.Doc.GetInteger(Constants.rnFormId);
                formPageId = cp.Doc.GetInteger(Constants.rnFormPageId);
                formQuestionId = cp.Doc.GetInteger(Constants.rnFormQuestionId);
                //
                // -- individual fields for the record, request name and requestModel name match the field name (except id)
                name = cp.Doc.GetText("name");
                headline = cp.Doc.GetText("headline");
                caption = cp.Doc.GetText("caption");
                description = cp.Doc.GetText("description");
                required = cp.Doc.GetBoolean("required");
                inputTypeId = cp.Doc.GetInteger("inputTypeId");
                optionList = cp.Doc.GetText("optionList");
            }
            private CPBaseClass cp;
            //
            public string button { get; }
            /// <summary>
            /// form for this question
            /// </summary>
            public int formId { get; set; }
            /// <summary>
            /// page for this question
            /// </summary>
            public int formPageId { get; set; }
            /// <summary>
            /// id of the question, 0 to add new question
            /// </summary>
            public int formQuestionId { get; set; }
            //
            // -- fields for the record
            //
            public string name { get; set; }
            //
            public string headline { get; set; }
            //
            public string caption { get; set; }
            //
            public string description { get; set; }
            //
            public bool required { get; set; }
            //
            public int inputTypeId { get; set; }
            //
            public string optionList { get; set; }
        }
    }
}