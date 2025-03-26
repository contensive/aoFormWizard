using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.Addon.aoFormWizard3.Models.View;
using Contensive.BaseClasses;
using Contensive.DesignBlockBase.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contensive.Addon.aoFormWizard3.Addons {
    public class ApplicationScoresWidget : AddonBaseClass {
        public override object Execute(CPBaseClass cp) {
            try {
                return DesignBlockController.renderWidget<ApplicationScoreWidgetsModel, ApplicationScoresViewModel>(cp,
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
