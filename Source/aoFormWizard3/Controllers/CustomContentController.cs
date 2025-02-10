using System;
using System.Text.RegularExpressions;
using Contensive.BaseClasses;

namespace Contensive.Addon.aoFormWizard3.Controllers {
    public class CustomContentController {
        public static bool verifyCustomContent(CPBaseClass cp, string customContentName) {
            try {
                bool status = false;
                bool createTable = false;
                using (var cs = cp.CSNew()) {
                    if (!cs.Open("Content", "name=" + cp.Db.EncodeSQLText(customContentName))) {
                        createTable = true;
                    } else {
                        // this table exists
                        status = true;
                    }
                }

                if (createTable) {
                    string tableName = "";
                    // remove any spaces since this is for sql table "[^A-Za-z0-9]+"
                    string sqlContentName = Regex.Replace(customContentName, "[^A-Za-z0-9]+", "");
                    tableName = "formwizard" + sqlContentName;
                    int tableid = cp.Content.AddContent(customContentName, tableName);
                    if (tableid <= 0) {
                        cp.Site.ErrorReport("formwizard verifyCustomContent: could not create content for " + customContentName);
                        cp.Site.LogAlarm("formwizard verifyCustomContent: could not create content for " + customContentName);
                        return false;
                    }
                    status = true;
                }

                return status;
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                cp.Site.LogAlarm("formwizard verifyCustomContent: " + ex.ToString());
                return false;
            }
        }
        public static bool createCustomContentField(CPBaseClass cp, string customContentName, string fieldName, int fieldType) {
            try {
                bool status = false;
                bool tableExists = false;
                using (var cs = cp.CSNew()) {
                    if (cs.Open("Content", "name=" + cp.Db.EncodeSQLText(customContentName))) {
                        tableExists = true;
                    }
                }

                if (tableExists) {
                    int fieldid = cp.Content.AddContentField(customContentName, fieldName, fieldType);
                    if (fieldid <= 0) {
                        cp.Site.ErrorReport("formwizard createCustomContentField: could not create content field for content:" + customContentName + " and field:" + fieldName);
                        cp.Site.LogAlarm("formwizard createCustomContentField: could not create content field for content:" + customContentName + " and field:" + fieldName);
                        return false;
                    }
                    status = true;
                }

                return status;
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                cp.Site.LogAlarm("formwizard createCustomContentField: " + ex.ToString());
                return false;
            }
        }
    }
}