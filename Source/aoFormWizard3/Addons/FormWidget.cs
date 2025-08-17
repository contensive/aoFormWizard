using System;
using Contensive.Addon.aoFormWizard3.Controllers;
using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.Addon.aoFormWizard3.Models.View;
using Contensive.BaseClasses;
using Contensive.DesignBlockBase.Controllers;

namespace Contensive.Addon.aoFormWizard3.Views {
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