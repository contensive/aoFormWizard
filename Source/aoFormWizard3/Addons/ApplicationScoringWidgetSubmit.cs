using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contensive.Addon.aoFormWizard3.Addons {
    public class ApplicationScoringWidgetSubmit : AddonBaseClass {
        public override object Execute(CPBaseClass cp) {
            var returnObj = new RemoteReturnObj();
            try {
                int applicationId = cp.Doc.GetInteger("applicationId");
                int score = cp.Doc.GetInteger("score");
                int responseId = cp.Doc.GetInteger("responseId");

                var newScore = ApplicationScoresModel.create<ApplicationScoresModel>(cp, $"scorer = {cp.User.Id} and applicationscored = {applicationId}");
                if (newScore == null) {
                    newScore = ApplicationScoresModel.addDefault<ApplicationScoresModel>(cp);
                }
                string userNameSQL = $"select name as 'name' from ccmembers where id = {cp.User.Id}";
                string userName = "";
                using (var cs = cp.CSNew()) { 
                    if(cs.OpenSQL(userNameSQL)) {
                        userName = cs.GetText("name");
                    }
                }

                newScore.name = $"score submitted by {userName}, on form {applicationId}";
                newScore.score = score;
                newScore.scorer = cp.User.Id;
                newScore.applicationFormScored = applicationId;
                newScore.applicationSubmittedScored = responseId;
                newScore.save(cp);

                returnObj.success = true;
                returnObj.successMessage = "Score saved";
                return returnObj;
            }
            catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                returnObj.success = false;
                returnObj.successMessage = "Unable to save score";
                return returnObj;

            }
        }

        public class RemoteReturnObj {
            public Boolean success { get; set; }
            public string successMessage { get; set; }
            public string errorMessage { get; set; }
        }
    }
}
