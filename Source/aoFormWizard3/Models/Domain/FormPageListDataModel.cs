using Contensive.Addon.aoFormWizard3.Addons.WidgetDashboardWidgets;
using Contensive.BaseClasses;
using System;
using System.Collections.Generic;
using System.Data;

namespace Contensive.Addon.aoFormWizard3.Models.Domain {
    public class FormPageListDataModel {
        /// <summary>
        /// the data.
        /// </summary>
        public List<RowDataModel> rowData { get; set; } = [];
        /// <summary>
        /// the number of rows in the entire dataset, not what is selected on the page.
        /// </summary>
        public int rowCount { get; set; } = 0;
        //
        public class RowDataModel {
            // 
            public int formPageId { get; set; }
            public string formPageName { get; set; }
            public int formId { get; set; }
            public string formName { get; set; }
            public int formQuestionCount { get; set; }
            public string formPageSortOrder { get; set; }
        }
        public FormPageListDataModel(CPBaseClass cp, FormPageListAddon.RequestModel request, string sqlOrderBy, string searchTerm, int pageNumber, int pageSize) {
            try {
                //
                // -- sql where from search and filters
                string sqlWhere = "(1=1)";
                //
                // -- form is required
                sqlWhere += $" and (p.formid={request.formId})";
                //
                string sqlTerm = cp.Db.EncodeSQLTextLike(searchTerm);
                sqlWhere += string.IsNullOrEmpty(searchTerm) ? "" : $" and(f.name like {sqlTerm})";
                if (request.formId != 0) {
                }
                //if (request.filterNotConfirmed) { sqlWhere += $"and(r.confirmationdate is null)"; }
                //if (request.filterCancelled) { sqlWhere += $"and(r.cancellationdate is not null)"; }
                //if (request.filterFromDate>DateTime.MinValue) { sqlWhere += $"and((r.registrationdate is null)or(r.registrationdate>'1/1/1'))"; }
                //
                // -- record count
                rowCount = 0;
                string sqlCount = @$"
                    select 
                        count(*) 
                    from
                        ccFormPages p 
                        left join ccForms f on f.id=p.formid
                    where 
                        {sqlWhere}
                ";
                using (DataTable dt = cp.Db.ExecuteQuery(sqlCount)) {
                    if (dt?.Rows != null && dt.Rows.Count == 1) {
                        rowCount = cp.Utils.EncodeInteger(dt.Rows[0][0]);
                    }
                }
                //
                // -- output data
                string sql = @$"
                    select
                        p.id as formPageId, p.name as formPageName, p.sortOrder as formPageSortOrder,
                        f.id as formId, f.name as formName,
                        (select count(*) from ccFormQuestions where formid=p.id) as formQuestionCount
                    from 
                        ccFormPages p 
                        left join ccForms f on f.id=p.formid
                    where 
	                    {sqlWhere}
                    order by
                        {(string.IsNullOrEmpty(sqlOrderBy) ? "p.sortorder,p.id" : sqlOrderBy)}
                    OFFSET 
                        {(pageNumber - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY
                ";
                using (DataTable dt = cp.Db.ExecuteQuery(sql)) {
                    foreach (DataRow row in dt.Rows) {
                        rowData.Add(new RowDataModel() {
                            formPageId = cp.Utils.EncodeInteger(row["formPageId"]),
                            formPageName = cp.Utils.EncodeText(row["formPageName"]),
                            formId = cp.Utils.EncodeInteger(row["formId"]),
                            formName = cp.Utils.EncodeText(row["formName"]),
                            formQuestionCount = cp.Utils.EncodeInteger(row["formQuestionCount"]),
                            formPageSortOrder = cp.Utils.EncodeText(row["formPageSortOrder"]),
                        });
                    }
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
        }
    }
}
