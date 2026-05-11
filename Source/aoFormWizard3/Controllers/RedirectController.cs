using Contensive.FormWidget.Addons;
using Contensive.BaseClasses;
using System;

namespace Contensive.FormWidget.Controllers {
    public class RedirectController {
        // 
        // -- Redirect to form overview dashboard
        //
        public static void redirectToFormPortal(CPBaseClass cp) {
            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, "");
        }
        // 
        // -- Form
        //
        public static void redirectToFormList(CPBaseClass cp) {
            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormListAddon.guidPortalFeature);
        }
        // 
        public static void redirectToFormAdd(CPBaseClass cp) {
            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormEditAddon.guidPortalFeature);
        }
        // 
        public static void redirectToFormEdit(CPBaseClass cp, int formId) {
            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormEditAddon.guidPortalFeature, $"&{Constants.rnFormId}={formId}");
        }
        //
        // -- Form Emails
        //
        public static void redirectToFormEmailEdit(CPBaseClass cp, int formId) {
            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormEmailEditAddon.guidPortalFeature, $"&{Constants.rnFormId}={formId}");
        }
        //
        // -- Form Buttons
        //
        public static void redirectToFormButtonEdit(CPBaseClass cp, int formId) {
            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormButtonEditAddon.guidPortalFeature, $"&{Constants.rnFormId}={formId}");
        }
        //
        // -- Form Submit
        //
        public static void redirectToFormSubmitEdit(CPBaseClass cp, int formId) {
            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormSubmitEditAddon.guidPortalFeature, $"&{Constants.rnFormId}={formId}");
        }
        //
        // -- Form Features
        //
        public static void redirectToFormFeatureEdit(CPBaseClass cp, int formId) {
            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormFeatureEditAddon.guidPortalFeature, $"&{Constants.rnFormId}={formId}");
        }
        //
        // -- Form Page
        //
        public static void redirectToFormPageList(CPBaseClass cp, int formId) {
            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormPageListAddon.guidPortalFeature, $"&{Constants.rnFormId}={formId}");
        }
        // 
        public static void redirectToFormPageAdd(CPBaseClass cp, int formId) {
            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormPageEditAddon.guidPortalFeature, $"&{Constants.rnFormId}={formId}");
        }
        // 
        public static void redirectToFormPageEdit(CPBaseClass cp, int formId, int formPageId) {
            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormPageEditAddon.guidPortalFeature, $"&{Constants.rnFormId}={formId}&{Constants.rnFormPageId}={formPageId}");
        }
        //
        // -- Form Question
        //
        public static void redirectToFormQuestionList(CPBaseClass cp, int formId, int formPageId) {
            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormQuestionListAddon.guidPortalFeature, $"&{Constants.rnFormId}={formId}&{Constants.rnFormPageId}={formPageId}");
        }
        // 
        public static void redirectToFormQuestionAdd(CPBaseClass cp, int formId, int formPageId) {
            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormQuestionEditAddon.guidPortalFeature, $"&{Constants.rnFormId}={formId}&{Constants.rnFormPageId}={formPageId}");
        }
        // 
        public static void redirectToFormQuestionEdit(CPBaseClass cp, int formId, int formPageId, int formQuestionId) {
            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormQuestionEditAddon.guidPortalFeature, $"&{Constants.rnFormId}={formId}&{Constants.rnFormPageId}={formPageId}&{Constants.rnFormQuestionId}={formQuestionId}");
        }
        //
        // -- Form Response / Submission
        //
        public static void redirectToFormResponseList(CPBaseClass cp, int formId) {
            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormResponseListAddon.guidPortalFeature, $"&{Constants.rnFormId}={formId}");
        }
        // 
        public static void redirectToFormResponseDetail(CPBaseClass cp, int formId, int formResponseId) {
            cp.AdminUI.RedirectToPortalFeature(Constants.guidPortalForms, FormResponseDetailsAddon.guidPortalFeature, $"&{Constants.rnFormId}={formId}&{Constants.rnFormResponseId}={formResponseId}");
        }
    }
}