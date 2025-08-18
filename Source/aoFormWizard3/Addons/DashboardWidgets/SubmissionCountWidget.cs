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
                foreach (var nameValue in form.getNameIdList(cp)) {
                    filterOptions.Add(new DashboardWidgetBaseModel_FilterOptions() {
                        filterCaption = nameValue.name,
                        filterValue = nameValue.id.ToString(),
                        filterActive = (filterFormId == nameValue.id)
                    });
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
        //
        public static double getAccountsReceivable(CPBaseClass cp, int widgetFilter) {
            DateTime dateDue = DateTime.MinValue;
            if (widgetFilter == 0) {
                //
                // -- total due
                dateDue = DateTime.MinValue;
            } else if (widgetFilter == 1) {
                //
                // -- current invoices
                dateDue = DateTime.Now;
            } else if (widgetFilter == 2) {
                //
                // -- over 30 days past due
                dateDue = DateTime.Now.AddDays(-30);
            } else if (widgetFilter == 3) {
                dateDue = DateTime.Now.AddDays(-60);
            } else if (widgetFilter > 3) {
                dateDue = DateTime.Now.AddDays(-90);
            }
            string sql = $@"
                select
                    SUM(o.TotalCharge)
                from 
	                orders o
	                left join abaccounts a on a.id=o.accountId
                where 1=1
	                and a.Closed=0
	                and o.dateCanceled is null
	                and o.paidByTransactionId=0
	                {(widgetFilter > 0 ? $"and o.dateDue<{dateDue}" : "")}";
            using DataTable dt = cp.Db.ExecuteQuery($"select count(*) as cnt from ccvisits where (lastVisitTime >{cp.Db.EncodeSQLDate(DateTime.Now.AddMinutes(-30))})");
            if (dt?.Rows != null) {
                return cp.Utils.EncodeNumber(dt.Rows[0][0]);
            }
            return 0.0;
        }
    }
}