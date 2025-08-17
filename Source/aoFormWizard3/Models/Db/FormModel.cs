
using Contensive.BaseClasses;
using Contensive.DesignBlockBase.Models.Db;
using Contensive.Models.Db;
using System.Data;

namespace Contensive.Addon.aoFormWizard3.Models.Db {
    public class FormModel : DbBaseModel {
        //
        public static DbBaseTableMetadataModel tableMetadata { get; private set; } = new DbBaseTableMetadataModel("Forms", "ccForms", "default", false);        // <------ set set model Name and everywhere that matches this string
        // 
        // ====================================================================================================
        // -- instance properties
        public bool addResetButton { get; set; }
        public string backButtonName { get; set; }
        public string continueButtonName { get; set; }
        public string resetButtonName { get; set; }
        public string submitButtonName { get; set; }
        public string saveButtonName { get; set; }
        public int joingroupid { get; set; }
        public int notificationemailid { get; set; }
        public int responseemailid { get; set; }
        public string thankyoucopy { get; set; }
        public bool useUserProperty { get; set; }
        public bool allowRecaptcha { get; set; }
        //
        /// <summary>
        /// get the latest form
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public static int getLastestForm(CPBaseClass cp) {
            using (DataTable dt = cp.Db.ExecuteQuery("select top 1 id from ccForms order by id desc")) {
                if (dt.Rows.Count == 0) { return 0; }
                return cp.Utils.EncodeInteger(dt.Rows[0]["id"]);
            }
        }
        //
        /// <summary>
        /// Create form from the legacy form widget.
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="formWidget"></param>
        /// <returns></returns>
        public static FormModel createFormFromWizard(CPBaseClass cp, FormWidgetModel formWidget) {
            //
            // -- test for upgrade from formWidget to form records
            if (formWidget.id==0 && cp.Db.IsTableField("ccFormWidgets", "backButtonName")) {
                cp.Log.Debug("aoFormWizard.FormSetViewModel.create(), upgrade from formWidget to form records");
                //
                // -- point formpage to form, not to widget
                cp.Db.ExecuteNonQuery($"update ccFormPages set formId=f.id from ccFormWidgets w left join ccforms f on f.id=w.formid where w.id={formWidget.id}");
                //
                // -- point response to form, not to widget
                cp.Db.ExecuteNonQuery($"update ccFormResponses set formId=f.id from ccFormWidgets w left join ccforms f on f.id=w.formid where w.id={formWidget.id}");
                //
                // -- point ccApplicationScoreWidgets to form, not to widget
                cp.Db.ExecuteNonQuery($"update ccApplicationScoreWidgets set formId=f.id from ccFormWidgets w left join ccforms f on f.id=w.formid where w.id={formWidget.id}");
                //
                // -- copy the formWidget settings to the form
                using (DataTable dt = cp.Db.ExecuteQuery($"select * from ccFormWidgets where id={formWidget.id}")) {
                    if (dt.Rows.Count > 0) {
                        DataRow row = dt.Rows[0];
                        var form = addDefault<FormModel>(cp);
                        form.addResetButton = cp.Utils.EncodeBoolean(row["addResetButton"]);
                        form.backButtonName = cp.Utils.EncodeText(row["backButtonName"]);
                        form.continueButtonName = cp.Utils.EncodeText(row["continueButtonName"]);
                        form.resetButtonName = cp.Utils.EncodeText(row["resetButtonName"]);
                        form.submitButtonName = cp.Utils.EncodeText(row["submitButtonName"]);
                        form.saveButtonName = cp.Utils.EncodeText(row["saveButtonName"]);
                        form.joingroupid = cp.Utils.EncodeInteger(row["joingroupid"]);
                        form.notificationemailid = cp.Utils.EncodeInteger(row["notificationemailid"]);
                        form.responseemailid = cp.Utils.EncodeInteger(row["responseemailid"]);
                        form.thankyoucopy = cp.Utils.EncodeText(row["thankyoucopy"]);
                        form.useUserProperty = cp.Utils.EncodeBoolean(row["useUserProperty"]);
                        form.allowRecaptcha = cp.Utils.EncodeBoolean(row["allowRecaptcha"]);
                        form.createdBy = cp.Utils.EncodeInteger(row["createdBy"]);
                        form.dateAdded = cp.Utils.EncodeDate(row["dateAdded"]);
                        form.modifiedDate = cp.Utils.EncodeDate(row["modifiedDate"]);
                        form.modifiedBy = cp.Utils.EncodeInteger(row["modifiedBy"]);
                        form.save(cp);
                        //
                        // -- each form page should now reference the form by it's forsetId, not the formWidget
                        //
                        formWidget.formId = form.id;
                        formWidget.save(cp);
                        return form;
                    }
                }
            }
            //
            // todo -- display form for user to select CREATE or SELECT EXISTING
            //
            return DbBaseModel.addDefault<FormModel>(cp);
        }
    }
}