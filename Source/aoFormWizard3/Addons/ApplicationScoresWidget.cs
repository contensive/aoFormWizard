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
                return DesignBlockController.renderWidget<FormWidgetsModel, FormViewModel>(cp,
                    widgetName: "Form Widget",
                    layoutGuid: Constants.guidLayoutFormWizard,
                    layoutName: Constants.nameLayoutFormWizard,
                    layoutPathFilename: Constants.pathFilenameLayoutFormWizard,
                    layoutBS5PathFilename: Constants.pathFilenameLayoutFormWizard);
            }
            catch (Exception ex) { 
            
            }
        }
    }
}
