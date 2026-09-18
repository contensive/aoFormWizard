using System;
using Contensive.FormWidget.Controllers;
using Contensive.FormWidget.Models.Db;
using Contensive.FormWidget.Models.View;
using Contensive.BaseClasses;
using Contensive.DesignBlockBase.Controllers;

namespace Contensive.FormWidget.Addons {
    // 
    public class FormWidget : AddonBaseClass {
        // 
        // =====================================================================================
        /// <summary>
        /// Addon api - Dynamic Form addon, Render dynamic form
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public override object Execute(CPBaseClass cp) {
            try {
                cp.Log.Debug($"FormWidget, enter");
                //
                // -- if called as a remote method without an instanceId (e.g. bot hitting /FormWidgetSubmit directly), exit gracefully
                if (!cp.User.IsEditing() && string.IsNullOrEmpty(cp.Doc.GetText("instanceId"))) {
                    cp.Log.Warn($"FormWidget, called without instanceId, user [{cp.User.Id}:{cp.User.Name}], visit [{cp.Visit.Id}], visitor [{cp.Visitor.Id}]");
                    return string.Empty;
                }
                //
                // -- these properties are passed to FormWidgetViewModel.
                // -- the allow for a single layout to handle multipage, preview and editing modes
                // -- called from the widget, the are all true if the user is editing
                // -- called from the applciatioin scoring widget, and from the submission details page, same but no editing
                cp.Doc.SetProperty("isMultipagePreviewMode", cp.User.IsEditing());
                cp.Doc.SetProperty("isEditing", cp.User.IsEditing());
                // -- use the lasted submission for the current session
                cp.Doc.SetProperty("formResponseId", 0);
                return DesignBlockController.renderWidget<FormWidgetModel, FormWidgetViewModel>(cp,
                    widgetName: "Form Widget",
                    layoutGuid: Constants.guidLayoutFormWizard,
                    layoutName: Constants.nameLayoutFormWizard,
                    layoutPathFilename: Constants.pathFilenameLayoutFormWizard,
                    layoutBS5PathFilename: Constants.pathFilenameLayoutFormWizard);

            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}