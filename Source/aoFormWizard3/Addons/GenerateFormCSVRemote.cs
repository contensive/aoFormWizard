using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.BaseClasses;
using Contensive.Models.Db;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Contensive.Addon.aoFormWizard3.Addons {
    public class GenerateFormCSVRemote : AddonBaseClass {
        private const string buttonDownloadCSV = "Download CSV";
        private const string buttonUpdateForm = "Update Form";

        public override object Execute(CPBaseClass cp) {
            try {
                int formWidgetId = cp.Doc.GetInteger("formWidgetId");
                List<FormResponseDataModel> csvRows = new List<FormResponseDataModel>();
                
                var responses = DbBaseModel.createList<FormResponseModel>(cp, $"formwidget = {formWidgetId}");
                foreach (var response in responses) {
                    FormResponseDataModel newRow = null;
                    if (!string.IsNullOrEmpty(response.formResponseData)) { 
                        newRow = cp.JSON.Deserialize<FormResponseDataModel>(response.formResponseData);
                        csvRows.Add(newRow);
                    }
                    
                }
                string filePath = "";
                if (csvRows.Count > 0) {
                    filePath = cp.CdnFiles.PhysicalFilePath + "\\Form_CSV_" + DateTime.Now.ToString("MM-dd-yyyy_hh:mm:ss");
                    using (var writer = new StreamWriter(filePath))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) {
                        csv.WriteRecords(csvRows);
                    }
                }

                var form = cp.AdminUI.CreateLayoutBuilder();

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
                    formBody.Append(cp.Html5.A(buttonDownloadCSV, new CPBase.BaseModels.HtmlAttributesA { href = filePath, download = "true" }));
                }
                form.body = formBody.ToString();
                form.addFormButton(buttonUpdateForm);
                return form.getHtml();
            }
            catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}
