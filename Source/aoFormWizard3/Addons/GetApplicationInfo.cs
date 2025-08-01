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
    public class GetApplicationInfo : AddonBaseClass {
        public override object Execute(CPBaseClass cp) {
            var returnObj = new RemoteReturnObj();
            try {
                int formResponseId = cp.Doc.GetInteger("submissionId");
                int scoreWidgetId = cp.Doc.GetInteger("scoreWidgetId");
                int sortBy = cp.Doc.GetInteger("sortBy");
                //check if there is a view for this user
                var responseViews = ApplicationViewsModel.getCount<ApplicationViewsModel>(cp, $"viewer = {cp.User.Id} and responseViewed = {formResponseId}");
                if(responseViews <= 0) {
                    var newView = ApplicationViewsModel.addDefault<ApplicationViewsModel>(cp);
                    newView.viewer = cp.User.Id;
                    newView.responseViewed = formResponseId;
                    newView.dateViewed = DateTime.Now;
                    newView.save(cp);
                }
                var applicationScoreWidget = ApplicationScoreWidgetsModel.create<ApplicationScoreWidgetsModel>(cp, scoreWidgetId);
                ApplicationScoresViewModel viewModel = null;
                if (applicationScoreWidget != null) { 
                    viewModel = ApplicationScoresViewModel.getApplicationScoreWidgetUpdate(cp, applicationScoreWidget, formResponseId, sortBy);
                }
                //get layout
                string layout = cp.Layout.GetLayout(Constants.guidLayoutApplicationScore, Constants.nameLayoutApplicationScore, Constants.pathFilenameLayoutApplicationScore);
                HtmlDocument ApplicationScoreDoc = new HtmlDocument();
                ApplicationScoreDoc.LoadHtml(layout);
                string newClientAssignmentTable = ApplicationScoreDoc.GetElementbyId("sectionReplace").InnerHtml;
                string newClientAssignmentFinalHTML = cp.Mustache.Render(newClientAssignmentTable, viewModel);
                returnObj.html = newClientAssignmentFinalHTML;
                returnObj.success = true;
                returnObj.successMessage = "";

                return returnObj;
            }
            catch (Exception ex) { 
                cp.Site.ErrorReport(ex);
                returnObj.success = false;
                returnObj.errorMessage = "Unable to get application";
                return returnObj;
            }
        }
    }

    public class RemoteReturnObj {
        public string html { get; set; }
        public Boolean success { get; set; }
        public string successMessage { get; set; }
        public string errorMessage { get; set; }
    }
}
