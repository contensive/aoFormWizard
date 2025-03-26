using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.BaseClasses;
using Contensive.DesignBlockBase.Models.View;
using Contensive.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Contensive.Addon.aoFormWizard3.Models.View.ApplicationScoresViewModel;
using static System.Net.Mime.MediaTypeNames;

namespace Contensive.Addon.aoFormWizard3.Models.View {
    public class ApplicationScoresViewModel : DesignBlockViewBaseModel {
        public List<ApplicationScoresTableRow> submittedApplications { get; set; }
        public FormViewModel applicationViewModel { get; set; }
        public List<int> scoresDropDownOptions { get; set; }
        public List<GradeTableValues> gradeTableValues { get; set; }
        public int id { get; set; }
        public int responseId { get; set; }
        public class ApplicationScoresTableRow {
            public List<ModalDataRow> submittedApplicationsDetailsRows { get; set; }
            public string scorerFirstName { get; set; }
            public string scorerMiddleInitial { get; set; }
            public string scorerLastName { get; set; }
            public string scorerEmail { get; set; }
            public string dateSubmitted { get; set; }
            public int score { get; set; }
            public int id { get; set; }
            public double cumulativeScore { get; set; }
            public int numberOfScoresSubmitted { get; set; }

        }

        public class GradeTableValues {
            public string grade { get; set; }
            public int points { get; set; }
        }

        public class ModalDataRow {
            public string scorerName { get; set; }
            public string dateScored { get; set; }
            public string scoreGraded { get; set; }
        }

        public class ApplicationData {
            public string firstName { get; set; }
            public string middleInitial { get; set; }
            public string lastName { get; set; }
            public string resident { get; set; }
            public string residentResponse { get; set; }
            public string DOB { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zipCode { get; set; }
            public string listedSchools { get; set; }
            public string selfOther { get; set; }
            public string otherName { get; set; }
            public string otherRlshp { get; set; }
            public string employer { get; set; }
            public string position { get; set; }
            public string number { get; set; }
            public string scholasticDescription { get; set; }
            public string extraActDescription { get; set; }
            public string comActDescription { get; set; }
            public string ques1Description { get; set; }
            public string ques2Description { get; set; }
            public string benefitDescription { get; set; }
            public string transcriptURL { get; set; }
            public string recommendationLetter { get; set; }

        }


        public static ApplicationScoresViewModel create(CPBaseClass cp, ApplicationScoreWidgetsModel settings) {
            int hint = 0;
            try {
                hint = 1;
                var viewModel = new ApplicationScoresViewModel();
                viewModel.submittedApplications = new List<ApplicationScoresTableRow>();
                viewModel.gradeTableValues = new List<GradeTableValues>();
                List<int> points = Enumerable.Range(1, 10).ToList();
                points.Reverse();
                viewModel.scoresDropDownOptions = points;
                viewModel.id = settings.submissionid;
                var applicationFormModel = DbBaseModel.createFirstOfList<FormWidgetsModel>(cp, "", "dateadded desc");
                if (applicationFormModel != null) {
                    var currentResponse = FormResponseModel.create<FormResponseModel>(cp, settings.submissionid);
                    if (currentResponse != null) {
                        viewModel.responseId = currentResponse.id;
                        viewModel.applicationViewModel = FormViewModel.createForScoringWidget(cp, applicationFormModel, currentResponse.memberId);
                    }
                }
                
                string[] grades = { "", "", "", "", "", "F", "D", "C", "B", "A"};

                foreach (var i in points) {
                    viewModel.gradeTableValues.Add( new GradeTableValues { grade = grades[i-1], points = i });
                }
                List<FormResponseModel> applications = DbBaseModel.createList<FormResponseModel>(cp);
                if (applications != null) {
                    hint = 2;
                    foreach (var application in applications) {
                        hint = 3;
                        var newRow = new ApplicationScoresTableRow();
                        newRow.id = application.id;
                        newRow.submittedApplicationsDetailsRows = GetScoreDetailsModalData(cp, application.id);
                        var formResponses = FormResponseModel.createList<FormResponseModel>(cp, "");
                        //FormResponseModel responseModel = null;
                        hint = 4;
                        /*
                        foreach (var formResponse in formResponses) {
                            hint = 5;
                            if (!string.IsNullOrEmpty(formResponse.formResponseData)) {
                                var responseData = cp.JSON.Deserialize<FormResponseDataModel>(formResponse.formResponseData).pageDict;
                                if (responseData != null) {
                                    if (responseData.ContainsKey(applicationId)) {
                                        responseModel = formResponse;
                                        break;
                                    }
                                }
                            }
                        }
                        */
                            hint = 6;
                            
                            var applicationScoresData = ApplicationScoresModel.createList<ApplicationScoresModel>(cp, "");
                            string applicationSubmittedInfo = $"select firstname as 'firstname', lastname as 'lastname', " +
                                                              $"middleInitial as 'middleInitial', " +
                                                              $"email as 'email' from ccFormResponse " +
                                                              $"left join ccMembers on ccFormResponse.memberid = ccMembers.id " +
                                                              $"where ccFormResponse.memberid = {application.memberId}";
                            using (var cs = cp.CSNew()) {
                                if (cs.OpenSQL(applicationSubmittedInfo)) {
                                    string firstName = cs.GetText("firstname");
                                    string lastName = cs.GetText("lastname");
                                    string middleName = cs.GetText("middleInitial");
                                    string email = cs.GetText("email");
                                    hint = 7;
                                    newRow.scorerFirstName = firstName;
                                    newRow.scorerLastName = lastName;
                                    newRow.scorerMiddleInitial = middleName;
                                    newRow.scorerEmail = email;
                                    hint = 8;
                                    var score = applicationScoresData.Where(x => x.scorer == cp.User.Id && x.applicationSubmittedScored == application.id).OrderByDescending(x => x.dateAdded).FirstOrDefault();
                                    newRow.dateSubmitted = application.dateAdded.Value.ToString("MM/dd/yyyy");
                                    newRow.score = score != null ? score.score : 0;
                                    var scoresByGrader = applicationScoresData.Where(x => x.applicationSubmittedScored == application.id && x.score > 0).ToList();
                                    newRow.cumulativeScore = scoresByGrader.Count() > 0? scoresByGrader.Sum(x => x.score) / scoresByGrader.Count() : 0;
                                    newRow.numberOfScoresSubmitted = applicationScoresData.Count(x => x.score > 0 && x.applicationSubmittedScored == application.id);
                                    hint = 9;
                                    viewModel.submittedApplications.Add(newRow);
                                }
                            }
                        
                    }
                }
                hint = 10;
                return viewModel;
            }
            catch (Exception ex) {
                cp.Site.ErrorReport("hint: " + hint + " " + ex);
                throw;
            }
        }

        public static List<ModalDataRow> GetScoreDetailsModalData(CPBaseClass cp, int applicationId) {
            try {
                var rows = new List<ModalDataRow>();

                var applicationInfoList = DbBaseModel.createList<ApplicationScoresModel>(cp, $"applicationSubmittedScored = {applicationId}");
                if (applicationInfoList != null) {
                    foreach (var applicationInfo in applicationInfoList) {
                        var row = new ModalDataRow();
                        row.dateScored = applicationInfo.dateAdded.Value.ToString("MM/dd/yyyy");
                        row.scoreGraded = applicationInfo.score.ToString();

                        string graderNameSQL = $"select name as 'name' from ccmembers where id = {applicationInfo.scorer}";
                        using (var cs = cp.CSNew()) {
                            if (cs.OpenSQL(graderNameSQL)) {
                                row.scorerName = cs.GetText("name");
                            }
                        }
                        rows.Add(row);
                    }
                }
                return rows;
            }
            catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }

        /*
        public static ApplicationData getApplicationData(CPBaseClass cp, FormResponseModel responseModel) { 
            try {
                ApplicationData appData = new ApplicationData();
                var responseData = cp.JSON.Deserialize(responseModel.formResponseData);
                appData.firstName = responseData["number"].Value<string>();
                return appData;
            }
            catch(Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
        */
    }
}
