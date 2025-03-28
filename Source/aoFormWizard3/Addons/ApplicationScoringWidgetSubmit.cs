using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.Addon.aoFormWizard3.Models.View;
using Contensive.BaseClasses;
using HtmlAgilityPack;
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
                bool userIsInGroup = false;
                var applicationScoreWidget = ApplicationScoreWidgetsModel.create<ApplicationScoreWidgetsModel>(cp, applicationId);
                if (applicationScoreWidget != null) {
                    int groupId = applicationScoreWidget.groupAllowedToScore;
                    string groupCheckSQL = $"select id as 'id' from ccMemberRules where memberid = {cp.User.Id} and groupid = {groupId}";
                    using (var cs = cp.CSNew()) {
                        if (cs.OpenSQL(groupCheckSQL)) {
                            if(cs.GetInteger("id") > 0) {
                                userIsInGroup = true;
                            }
                        }
                    }
                }

                if (userIsInGroup) {
                    var newScore = ApplicationScoresModel.create<ApplicationScoresModel>(cp, $"scorer = {cp.User.Id} and applicationscored = {applicationId}");
                    if (newScore == null) {
                        newScore = ApplicationScoresModel.addDefault<ApplicationScoresModel>(cp);
                    }
                    string userNameSQL = $"select name as 'name' from ccmembers where id = {cp.User.Id}";
                    string userName = "";
                    using (var cs = cp.CSNew()) {
                        if (cs.OpenSQL(userNameSQL)) {
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
                }
                else {
                    returnObj.success = false;
                    returnObj.errorMessage = "You are not in the group allowed to submit scores";
                }
                    ApplicationScoresViewModel viewModel = null;
                if (applicationScoreWidget != null) {
                    viewModel = ApplicationScoresViewModel.getApplicationScoreWidgetUpdate(cp, applicationScoreWidget, 0);
                }

                string layout = cp.Layout.GetLayout(Constants.guidLayoutApplicationScore, Constants.nameLayoutApplicationScore, Constants.pathFilenameLayoutApplicationScore);
                HtmlDocument ApplicationScoreDoc = new HtmlDocument();
                ApplicationScoreDoc.LoadHtml(layout);
                string newClientAssignmentTable = ApplicationScoreDoc.GetElementbyId("sectionReplace").InnerHtml;
                string newClientAssignmentFinalHTML = cp.Mustache.Render(newClientAssignmentTable, viewModel);
                returnObj.html = newClientAssignmentFinalHTML;
                
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
            public string html { get; set; }
        }
    }
}
