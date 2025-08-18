using Contensive.FormWidget;
using Contensive.FormWidget.Addons;
using Contensive.FormWidget.Controllers;
using Contensive.FormWidget.Models.Db;
using Contensive.FormWidget.Models.Domain;
using Contensive.BaseClasses;
using Contensive.Models.Db;
using System;

namespace Contensive.FormWidget.Addons {
    // ========================================================================================
    /// <summary>
    /// Meeting Edit Feature
    /// </summary>
    /// <remarks></remarks>
    public class FormEditAddon : AddonBaseClass {
        //
        public const string guidPortalFeature = "{D527F5BF-DF35-49F5-B03F-E8F6BC65A454}";
        public const string guidAddon = "{3F37844D-8C72-4C77-924E-BAD55734ACCB}";
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
                    if(!processView(app, request, ref userErrorMessage)) { return ""; }
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
        /// <param name="app"></param>
        /// <param name="cp"></param>
        /// <param name="request"></param>
        /// <param name="userErrorMessage"></param>
        public static bool processView(ApplicationModel app, RequestModel request, ref string userErrorMessage) {
            CPBaseClass cp = app.cp;
            try {
                // 
                // -- validate portal environment
                if (!cp.AdminUI.EndpointContainsPortal()) { 
                    RedirectController.redirectToFormEdit(cp, request.formId);
                    return false;
                }
                // 
                // -- cancel button
                if (request.button.Equals(Constants.buttonCancel)) {
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
                // -- ok button
                if (request.button.Equals(Constants.buttonOK)) {
                    saveForm(cp, request);
                    RedirectController.redirectToFormList(cp);
                    return false;
                }
                // 
                // -- delete button
                if (request.button.Equals(Constants.buttonDelete)) {
                    DbBaseModel.delete<FormModel>(cp, request.formId);
                    RedirectController.redirectToFormList(cp);
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
                // 
                // -- add rows
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Name";
                layoutBuilder.rowValue = cp.Html5.InputText(Constants.rnName,255, form?.name ?? "" , "form-control");
                layoutBuilder.rowHelp = "The name for this form widget. Use the name to recognize the form in a list, for example, 'membership application form' or 'contact us form'. The name does not appear on the public form.";
                //
                // -- setup layout
                layoutBuilder.title = (form == null) ? "Add Form" : "Edit Form";
                layoutBuilder.portalSubNavTitle = (form == null) ? "" : $"form: {form.name}";
                layoutBuilder.description = "This form widget has the controls for the entire set of form pages. A form widget is dropped on the website and contains one or more form-pages. Each form page contains one or more form questions.";
                layoutBuilder.callbackAddonGuid = guidAddon;
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
                if (form is null) {
                    form = DbBaseModel.addDefault<FormModel>(cp);
                    //
                    // -- important. this record becomes the current focus for the get method
                    request.formId = form.id;
                }
                form.name = request.name;
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
                // -- initialize properties (cannot use default constructor)
                button = cp.Doc.GetText(Constants.rnButton);
                formId = cp.Doc.GetInteger(Constants.rnFormId);
                //
                // -- individual fields for the record, request name and requestModel name match the field name (except id)
                name = cp.Doc.GetText("name");
            }
            private CPBaseClass cp;
            //
            public string button { get; }
            //
            public int formId { get; set; }
            //
            public string name { get; set; }
        }
    }
}