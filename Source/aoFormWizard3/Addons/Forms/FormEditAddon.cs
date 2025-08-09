using Contensive.Addon.aoFormWizard3;
using Contensive.Addon.aoFormWizard3.Addons.WidgetDashboardWidgets;
using Contensive.Addon.aoFormWizard3.Controllers;
using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.Addon.aoFormWizard3.Models.Domain;
using Contensive.BaseClasses;
using Contensive.Models.Db;
using System;

namespace Contensive.Addon.aoFormWizard3.Addons.WidgetDashboardWidgets {
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
                // -- validate portal environment
                if (!cp.AdminUI.EndpointContainsPortal()) { return cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, guidPortalFeature, ""); }
                // 
                string errorMessage = "";
                var request = new RequestModel(cp);
                using (var app = new ApplicationModel(cp)) {
                    processView(app, request, ref errorMessage);
                    return getView(app, request);
                }
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
                var layoutBuilder = cp.AdminUI.CreateLayoutBuilderNameValue();
                //
                FormWidgetModel formWidget = DbBaseModel.create<FormWidgetModel>(cp, request.formWidgetId);
                //
                layoutBuilder.addRow();
                layoutBuilder.rowName = "Name";
                layoutBuilder.rowValue = cp.Html5.InputText(Constants.rnName,255, formWidget?.name ?? "" , "form-control");
                layoutBuilder.rowHelp = "The name for this form widget. Use the name to recognize the form in a list, for example, 'membership application form' or 'contact us form'. The name does not appear on the public form.";
                //
                layoutBuilder.title = (formWidget == null) ? "Add Form" : "Edit Form";
                layoutBuilder.portalSubNavTitle = (formWidget == null) ? "Add Form" : formWidget.name;
                layoutBuilder.description = "This form widget has the controls for the entire set of form pages. A form widget is dropped on the website and contains one or more form-pages. Each form page contains one or more form questions.";
                layoutBuilder.callbackAddonGuid = guidAddon;
                // 
                // -- add buttons
                layoutBuilder.addFormButton(Constants.buttonOK);
                layoutBuilder.addFormButton(Constants.buttonSave);
                layoutBuilder.addFormButton(Constants.ButtonCancel);
                // 
                // -- add hiddens
                //
                // -- feature subnav link querystring - clicks must include these values
                cp.Doc.AddRefreshQueryString(Constants.rnFormWidgetId, request.formWidgetId);
                //
                return layoutBuilder.getHtml();
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
        /// <param name="cp"></param>
        /// <param name="request"></param>
        /// <param name="errorMessage"></param>
        public static void processView(ApplicationModel app, RequestModel request, ref string errorMessage) {
            CPBaseClass cp = app.cp;
            try {
                switch (request.button ?? "") {
                    case Constants.buttonSave: {
                            saveFormWidget(cp, request);
                            return;
                        }
                    case Constants.buttonOK: {
                            saveFormWidget(cp, request);
                            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormListAddon.guidPortalFeature, "");
                            return;
                        }
                    case Constants.buttonDelete: {
                            foreach (var formPage in DbBaseModel.createList<FormPageModel>(cp, $"formsetid={request.formWidgetId}")) {
                                foreach (var formQuestion in DbBaseModel.createList<FormQuestionModel>(cp, $"formid={formPage.id}")) {
                                    DbBaseModel.delete<FormQuestionModel>(cp, formQuestion.id);
                                }
                                DbBaseModel.delete<FormPageModel>(cp, formPage.id);
                            }
                            DbBaseModel.delete<FormWidgetModel>(cp, request.formWidgetId);
                            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormListAddon.guidPortalFeature, "");
                            return;
                        }
                    case Constants.ButtonCancel: {
                            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormListAddon.guidPortalFeature, "");
                            return;
                        }
                    default: {
                            return;
                        }
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
        // 
        // ====================================================================================================
        // 
        private static void saveFormWidget(CPBaseClass cp, RequestModel request) {
            try {
                var formWidget = DbBaseModel.create<FormWidgetModel>(cp, request.formWidgetId);
                if (formWidget is null) {
                    formWidget = DbBaseModel.addDefault<FormWidgetModel>(cp);
                }
                formWidget.name = request.name;
                formWidget.save(cp);
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
                formWidgetId = cp.Doc.GetInteger(Constants.rnFormWidgetId);
                //
                // -- individual fields for the record, request name and requestModel name match the field name (except id)
                name = cp.Doc.GetText("name");
            }
            private CPBaseClass cp;
            //
            public string button { get; }
            //
            public int formWidgetId { get; set; }
            //
            public string name { get; set; }
        }
    }
}