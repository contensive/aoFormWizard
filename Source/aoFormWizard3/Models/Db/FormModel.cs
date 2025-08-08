
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
        public string backButtonName { get; set; }
        public bool addResetButton { get; set; }
        public string resetButtonName { get; set; }
        public string continueButtonName { get; set; }
        public string submitButtonName { get; set; }
        public string saveButtonName { get; set; }

        public int joingroupid { get; set; }
        public int notificationemailid { get; set; }
        public int responseemailid { get; set; }
        public string thankyoucopy { get; set; }
        public bool useUserProperty { get; set; }
        public bool allowRecaptcha { get; set; }

        public static FormModel createFormFromWizard(CPBaseClass cp, FormWidgetModel settings) {
            FormModel form = DbBaseModel.addDefault<FormModel>(cp);
            settings.formId = form.id;
            settings.save(cp);
            //
            // -- test for upgrade from formWidget to form records
            if (cp.Db.IsTableField("ccFormWidgets", "backButtonName")) {
                cp.Log.Debug("aoFormWizard.FormSetViewModel.create(), upgrade from formWidget to form records");
                //
                // -- copy the formWidget settings to the form
                using (DataTable dt = cp.Db.ExecuteQuery($"select * from ccFormWidgets where id = {settings.id}")) {
                    if (dt.Rows.Count > 0) {
                        DataRow dr = dt.Rows[0];
                        form.addResetButton = cp.Utils.EncodeBoolean(dr["addResetButton"]);
                        form.resetButtonName = cp.Utils.EncodeText(dr["resetButtonName"]);
                        form.backButtonName = cp.Utils.EncodeText(dr["backButtonName"]);
                        form.continueButtonName = cp.Utils.EncodeText(dr["continueButtonName"]);
                        form.submitButtonName = cp.Utils.EncodeText(dr["submitButtonName"]);
                        form.saveButtonName = cp.Utils.EncodeText(dr["saveButtonName"]);
                        form.allowRecaptcha = cp.Utils.EncodeBoolean(dr["allowRecaptcha"]);
                        form.thankyoucopy = cp.Utils.EncodeText(dr["thankyoucopy"]);
                        form.useUserProperty = cp.Utils.EncodeBoolean(dr["useUserProperty"]);
                        form.save(cp);
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