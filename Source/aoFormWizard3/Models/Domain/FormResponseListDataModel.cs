using Contensive.Addon.aoFormWizard3.Addons.WidgetDashboardWidgets;
using Contensive.Addon.aoFormWizard3.Controllers;
using Contensive.BaseClasses;
using System;
using System.Collections.Generic;
using System.Data;

namespace Contensive.Addon.aoFormWizard3.Models.Domain {
    public class FormResponseListDataModel {
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
            public int formResponseId { get; set; }
            public string formResponseName { get; set; }
            public int submitterId { get; set; }
            public string submitterName { get; set; }
            public DateTime? started { get; set; }
            public DateTime? submitted { get; set; }
            /// <summary>
            /// the json encoded object with the response from the complete submission.
            /// </summary>
            public string formResponseData { get; set; }
        }
        public FormResponseListDataModel(CPBaseClass cp, FormResponseListAddon.RequestModel request, string sqlOrderBy, string searchTerm, int pageNumber, int pageSize) {
            try {
                //
                // -- sql where from search and filters
                string sqlWhere = "(1=1)";
                string sqlTerm = cp.Db.EncodeSQLTextLike(searchTerm);
                sqlWhere += string.IsNullOrEmpty(searchTerm) ? "" : $" and(r.name like {sqlTerm})";
                sqlWhere += request.onlySubmitted ? " and (r.dateSubmitted is not null)" : "";
                sqlWhere += request.formId>0 ? $"and(r.formId={cp.Db.EncodeSQLNumber(request.formId)})" : "";
                //
                // -- record count
                rowCount = 0;
                string sqlCount = @$"
                    select 
                        count(*) 
                    from 
                        ccFormResponse r 
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
                        r.id as formResponseId, r.name as formResponseName,r.formResponseData,r.dateAdded as Started,r.dateSubmitted as Submitted,
                        f.id as formId, f.name as formName,
                        m.id as submitterId, m.name as submitterName
                    from 
                        ccFormResponse r 
                        left join ccForms f on f.id=r.formid
                        left join ccmembers m on m.id=r.createdBy
                    where 
	                    {sqlWhere}
                    order by
                        {(string.IsNullOrEmpty(sqlOrderBy) ? "r.id desc" : sqlOrderBy)}
                    OFFSET 
                        {(pageNumber - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY
                ";
                using (DataTable dt = cp.Db.ExecuteQuery(sql)) {
                    foreach (DataRow row in dt.Rows) {
                        rowData.Add(new RowDataModel() {
                            formId = cp.Utils.EncodeInteger(row["formId"]),
                            formName = cp.Utils.EncodeText(row["formName"]),
                            formResponseId = cp.Utils.EncodeInteger(row["formResponseId"]),
                            formResponseName = cp.Utils.EncodeText(row["formResponseName"]),
                            formResponseData = cp.Utils.EncodeText(row["formResponseData"]),
                            submitterId = cp.Utils.EncodeInteger(row["submitterId"]),
                            submitterName = cp.Utils.EncodeText(row["submitterName"]),
                            started = GenericController.encodeNullableDate(row["started"]),
                            submitted = GenericController.encodeNullableDate(row["submitted"])
                        });
                    }
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
        }
    }
}
