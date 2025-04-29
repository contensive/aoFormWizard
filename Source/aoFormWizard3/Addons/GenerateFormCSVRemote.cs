using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.BaseClasses;
using Contensive.Models.Db;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Contensive.Addon.aoFormWizard3.Addons {
    public class GenerateFormCSVRemote : AddonBaseClass {
        private const string buttonDownloadCSV = "Download CSV";
        private const string buttonUpdateForm = "Generate CSV";

        public override object Execute(CPBaseClass cp) {
            try {
                int formWidgetId = cp.Doc.GetInteger("formWidgetId");
                string filePath = "";
                string fileName = "";
                var form = cp.AdminUI.CreateLayoutBuilder();
                List<Dictionary<string, string>> csvRows = new List<Dictionary<string, string>>();

                if (formWidgetId > 0) {
                    var responses = DbBaseModel.createList<FormResponseModel>(cp, $"formwidget = {formWidgetId}");
                    foreach (var response in responses) {
                        FormResponseDataModel newRow = null;
                        if (!string.IsNullOrEmpty(response.formResponseData)) {
                            newRow = cp.JSON.Deserialize<FormResponseDataModel>(response.formResponseData);
                            Dictionary<string, string> questionAnswers = new Dictionary<string, string>();
                            questionAnswers.Add("Date submitted", response.dateSubmitted.ToString("MM/dd/yyyy hh:mm:ss"));
                            foreach (var value in newRow.pageDict.Values) {
                                foreach (var item in value.questionDict) {
                                    string answer = "";
                                    if(string.IsNullOrEmpty(item.Value.textAnswer) && item.Value.choiceAnswerDict.Values.Count > 0) {
                                        foreach(var choice in item.Value.choiceAnswerDict) {
                                            answer += choice.Key + ": " + choice.Value + "\n";
                                        }
                                    }
                                    else {
                                        answer = item.Value.textAnswer;
                                    }
                                        
                                    questionAnswers.Add(item.Value.question, answer);
                                }
                            }
                            csvRows.Add(questionAnswers);
                        }

                    }


                    
                    if (responses.Count > 0) {
                        if (csvRows.Count > 0) {
                            fileName = "Form_CSV_" + DateTime.Now.ToString("MM-dd-yyyy_hh_mm_ss") + ".csv";
                            filePath = cp.CdnFiles.PhysicalFilePath + fileName;
                            using (var writer = new StreamWriter(filePath))
                            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) {
                                var headings = new List<string>(csvRows.First().Keys);
                                foreach (var heading in headings) {
                                    csv.WriteField(heading);
                                }

                                csv.NextRecord();

                                foreach (var item in csvRows) {
                                    foreach (var heading in headings) {
                                        csv.WriteField(item[heading]);
                                    }

                                    csv.NextRecord();
                                }
                            }
                        }
                    }
                    else {
                        form.warningMessage = "This form has no submissions";                        
                    }
                }
                
                form.title = "Generate form CSV tool";
                form.description = "This form creates a csv file from the responses of a form";
                form.isOuterContainer = true;
                form.includeForm = true;
                StringBuilder formBody = new();

                var formWidgetsList = FormWidgetsModel.createList<FormWidgetsModel>(cp, "", "id desc");
                var formWidgetsIds = formWidgetsList.Select(x => x.id.ToString()).ToList();
                formBody.Append(cp.Html5.H4("Select form"));
                
                formBody.Append(cp.Html5.Div(cp.AdminUI.GetLookupListEditor("formWidgetId", formWidgetsIds, 0, "inviteUsersAddToGroup", false, false)));
                if (formWidgetId > 0) {
                    formBody.Append("<b>" + cp.Html5.A(buttonDownloadCSV, new CPBase.BaseModels.HtmlAttributesA { href = cp.Http.CdnFilePathPrefix + fileName, download = fileName }));
                }
                form.body = formBody.ToString();
                form.addFormButton(buttonUpdateForm);
                return form.getHtml(cp);
            }
            catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}
