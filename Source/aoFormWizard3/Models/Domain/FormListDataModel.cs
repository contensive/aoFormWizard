using Contensive.FormWidget.Addons;
using Contensive.BaseClasses;
using System;
using System.Collections.Generic;
using System.Data;

namespace Contensive.FormWidget.Models.Domain {
    public class FormListDataModel {
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
            public int formId { get; set; }
            public string formName { get; set; }
            public int formResponseCount { get; set; }
        }
        public FormListDataModel(CPBaseClass cp, FormListAddon.RequestModel request, string sqlOrderBy, string searchTerm, int pageNumber, int pageSize) {
            try {
                //
                // -- sql where from search and filters
                string sqlWhere = "(1=1)";
                string sqlTerm = cp.Db.EncodeSQLTextLike(searchTerm);
                sqlWhere += string.IsNullOrEmpty(searchTerm) ? "" : $" and((f.name like {sqlTerm})or(m.name like {sqlTerm}))";
                //if (request.meetingId != 0) {
                //    sqlWhere += " AND (r.MeetingID=" + cp.Db.EncodeSQLNumber(request.meetingId) + ")";
                //}
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
                        ccForms f 
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
                        f.id as formId, f.name as formName,
                        m.id as submitterId, m.name as submitterName,
                        (select count(*) from ccFormResponse r where r.formId=f.id) as formResponseCount
                    from 
                        ccForms f 
                        left join ccmembers m on m.id=f.createdBy
                    where 
	                    {sqlWhere}
                    order by
                        {(string.IsNullOrEmpty(sqlOrderBy) ? "f.id desc" : sqlOrderBy)}
                    OFFSET 
                        {(pageNumber - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY
                ";
                using (DataTable dt = cp.Db.ExecuteQuery(sql)) {
                    foreach (DataRow row in dt.Rows) {
                        rowData.Add(new RowDataModel() {
                            formId = cp.Utils.EncodeInteger(row["formId"]),
                            formName = cp.Utils.EncodeText(row["formName"]),
                            formResponseCount = cp.Utils.EncodeInteger(row["formResponseCount"]),
                        });
                    }
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
        }
    }
}
