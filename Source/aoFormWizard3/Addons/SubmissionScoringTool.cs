using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.Addon.aoFormWizard3.Models.View;
using Contensive.BaseClasses;
using Contensive.DesignBlockBase.Controllers;
using Contensive.Models.Db;
using System;

namespace Contensive.Addon.aoFormWizard3.Addons {
    public class SubmissionScoringTool : AddonBaseClass {
        public override object Execute(CPBaseClass cp) {
            try {
                //
                // -- set the flag to signal tool features (and widget should not show)
                cp.Doc.SetProperty("isSubmissionScoringTool", true);
                //
                // -- create a SubmissionScoringWidget record for this user
                string widgetInstanceGuid = $"SubmissionScoringTool, user-{cp.User.Id}";
                var settings = DbBaseModel.create< ApplicationScoreWidgetsModel >(cp, widgetInstanceGuid);
                settings = settings ?? DbBaseModel.addDefault<ApplicationScoreWidgetsModel>(cp);
                cp.Doc.SetProperty("instanceid", widgetInstanceGuid);
                const string submissionScoringWidgetGuid = "{7D3BD03E-5A92-4228-8278-3115CF6C7B1E}";
                return cp.Addon.Execute(submissionScoringWidgetGuid);
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}
