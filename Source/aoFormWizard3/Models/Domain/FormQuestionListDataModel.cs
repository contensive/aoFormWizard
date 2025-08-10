using Contensive.Addon.aoFormWizard3.Addons.WidgetDashboardWidgets;
using Contensive.BaseClasses;
using System;
using System.Collections.Generic;
using System.Data;

namespace Contensive.Addon.aoFormWizard3.Models.Domain {
    public class FormQuestionListDataModel {
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
            public int formQuestionId { get; set; }
            public string formQuestionName { get; set; }
            public string formQuestionSortOrder { get; set; }
            public int formPageId { get; set; }
            public string formPageName { get; set; }
            public int formId { get; set; }
            public string formWidgetName { get; set; }
        }
        public FormQuestionListDataModel(CPBaseClass cp, FormQuestionListAddon.RequestModel request, string sqlOrderBy, string searchTerm, int pageNumber, int pageSize) {
            try {
                //
                // -- sql where from search and filters
                string sqlWhere = "(1=1)";
                string sqlTerm = cp.Db.EncodeSQLTextLike(searchTerm);
                sqlWhere += string.IsNullOrEmpty(searchTerm) ? "" : $" and((q.name like {sqlTerm})or(p.name like {sqlTerm})or(w.name like {sqlTerm}))";
                if (request.formId != 0) {
                    sqlWhere += $" AND (p.formId={request.formId})";
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
                        ccFormQuestions q
                        left join ccFormPages p on q.formid=p.id
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
                        q.id as formQuestionId,q.name as formQuestionName,q.sortOrder as formQuestionSortOrder,
                        p.id as formPageId, p.name as formPageName, p.sortOrder as formPageSortOrder,
                        w.id as formWidgetId, w.name as formWidgetName
                    from 
                        ccFormQuestions q
                        left join ccFormPages p on q.formid=p.id
                        left join ccForms w on w.id=p.formid
                    where 
	                    {sqlWhere}
                    order by
                        {(string.IsNullOrEmpty(sqlOrderBy) ? "w.id,p.sortOrder,p.id,q.sortorder,q.id" : sqlOrderBy)}
                    OFFSET 
                        {(pageNumber - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY
                ";
                using (DataTable dt = cp.Db.ExecuteQuery(sql)) {
                    foreach (DataRow row in dt.Rows) {
                        rowData.Add(new RowDataModel() {
                            formQuestionId = cp.Utils.EncodeInteger(row["formQuestionId"]),
                            formQuestionName = cp.Utils.EncodeText(row["formQuestionName"]),
                            formQuestionSortOrder = cp.Utils.EncodeText(row["formQuestionSortOrder"]),
                            formPageId = cp.Utils.EncodeInteger(row["formPageId"]),
                            formPageName = cp.Utils.EncodeText(row["formPageName"]),
                            formId = cp.Utils.EncodeInteger(row["formWidgetId"]),
                            formWidgetName = cp.Utils.EncodeText(row["formWidgetName"]),
                        });
                    }
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
        }
    }
}
