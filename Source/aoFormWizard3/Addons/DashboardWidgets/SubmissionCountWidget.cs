using Contensive.FormWidget.Addons;
using Contensive.FormWidget.Models.Db;
using Contensive.FormWidget.Models.Domain;
using Contensive.BaseClasses;
using Contensive.BaseModels;
using Contensive.Models.Db;
using Contensive.Processor.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Contensive.FormWidget.Addons {
    public class SubmissionCountWidget : AddonBaseClass {
        public override object Execute(CPBaseClass cp) {
            try {
                //
                // -- filter, widgetFilter = formId
                int filterFormId = cp.Doc.GetInteger("widgetFilter");
                FormModel form = DbBaseModel.create<FormModel>(cp, filterFormId);
                if (form is null) {
                    form = DbBaseModel.create<FormModel>(cp, FormModel.getLastestForm(cp));
                    filterFormId = form?.id ?? 0;
                }
                List<DashboardWidgetBaseModel_FilterOptions> filterOptions = [];
                if (form is not null) {
                    foreach (var nameValue in form.getNameIdList(cp)) {
                        filterOptions.Add(new DashboardWidgetBaseModel_FilterOptions() {
                            filterCaption = nameValue.name,
                            filterValue = nameValue.id.ToString(),
                            filterActive = (filterFormId == nameValue.id)
                        });
                    }
                }
                //
                string url = form is null ? "" : cp.AdminUI.GetPortalFeatureLink(Constants.guidPortalForms, FormResponseListAddon.guidPortalFeature) + $"&{Constants.rnFormId}={filterFormId}&{Constants.rnOnlySubmitted}=1";
                var result = new DashboardWidgetNumberModel() {
                    widgetName = "Form Submissions",
                    width = 1,
                    number = form is null ? "0" : FormResponseModel.getCount<FormResponseModel>(cp, $"(formId={filterFormId})and(dateSubmitted is not null)").ToString(),
                    subhead = form is null ? "No Forms Created" : $"Form Submissions for {form.name}",
                    description = $"Submissions to the selected form. Click to Review.",
                    refreshSeconds = 0,
                    widgetType = WidgetTypeEnum.number,
                    url = url,
                    filterOptions = filterOptions
                };
                return result;
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }

        }
    }
}