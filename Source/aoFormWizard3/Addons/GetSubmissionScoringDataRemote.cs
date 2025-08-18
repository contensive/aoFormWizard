using Contensive.FormWidget.Models.Db;
using Contensive.FormWidget.Models.View;
using Contensive.BaseClasses;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contensive.FormWidget.Addons {
    public class GetSubmissionScoringDataRemote : AddonBaseClass {
        public override object Execute(CPBaseClass cp) {
            var returnObj = new submissionScoringWidgetDataModel();
            try {
                int selectedFormResponseId = cp.Doc.GetInteger("submissionId");
                int scoreWidgetId = cp.Doc.GetInteger("scoreWidgetId");
                int sortBy = cp.Doc.GetInteger("sortBy");
                var applicationScoreWidget = ApplicationScoreWidgetsModel.create<ApplicationScoreWidgetsModel>(cp, scoreWidgetId);
                if (applicationScoreWidget == null) {
                    //
                    // -- widget not found
                    returnObj.success = false;
                    returnObj.errorMessage = "Application score widget not found.";
                    return returnObj;
                }
                //
                // -- verify they is a view record for the current user
                var submissionViews = ApplicationViewsModel.getCount<ApplicationViewsModel>(cp, $"viewer = {cp.User.Id} and responseViewed = {selectedFormResponseId}");
                if(submissionViews <= 0) {
                    var submissionView = ApplicationViewsModel.addDefault<ApplicationViewsModel>(cp);
                    submissionView.viewer = cp.User.Id;
                    submissionView.responseViewed = selectedFormResponseId;
                    submissionView.dateViewed = DateTime.Now;
                    submissionView.save(cp);
                }
                //
                // -- create view model to populate the layout
                SubmissionScoringViewModel viewModel = SubmissionScoringViewModel.getSubmissionScoringViewModel(cp, applicationScoreWidget, selectedFormResponseId, sortBy);
                //
                //-- get layout
                string layout = cp.Layout.GetLayout(Constants.guidLayoutApplicationScore, Constants.nameLayoutApplicationScore, Constants.pathFilenameLayoutApplicationScore);
                HtmlDocument ApplicationScoreDoc = new HtmlDocument();
                ApplicationScoreDoc.LoadHtml(layout);
                string newClientAssignmentTable = ApplicationScoreDoc.GetElementbyId("sectionReplace").InnerHtml;
                string newClientAssignmentFinalHTML = cp.Mustache.Render(newClientAssignmentTable, viewModel);
                //
                // -- return a json object with the html
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

    public class submissionScoringWidgetDataModel {
        public string html { get; set; }
        public Boolean success { get; set; }
        public string successMessage { get; set; }
        public string errorMessage { get; set; }
    }
}
