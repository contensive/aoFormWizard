using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.Addon.aoFormWizard3.Models.View;
using Contensive.BaseClasses;
using Contensive.DesignBlockBase.Controllers;
using System;

namespace Contensive.Addon.aoFormWizard3.Addons {
    public class FormSubmissionScoringWidget : AddonBaseClass {
        public override object Execute(CPBaseClass cp) {
            try {
                return DesignBlockController.renderWidget<ApplicationScoreWidgetsModel, SubmissionScoringViewModel>(cp,
                    widgetName: "Application Scoring Widget",
                    layoutGuid: Constants.guidLayoutApplicationScore,
                    layoutName: Constants.nameLayoutApplicationScore,
                    layoutPathFilename: Constants.pathFilenameLayoutApplicationScore,
                    layoutBS5PathFilename: Constants.pathFilenameLayoutApplicationScore);
            }
            catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}
