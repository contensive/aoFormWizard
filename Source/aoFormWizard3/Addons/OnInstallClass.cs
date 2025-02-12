using System;
using System.Data;
using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.BaseClasses;
using Contensive.Models.Db;
using Microsoft.VisualBasic.CompilerServices;

namespace Contensive.Addon.aoFormWizard3.Views {
    // 
    public class OnInstallClass : AddonBaseClass {
        // 
        // =====================================================================================
        /// <summary>
        /// Executed on installation -- upgrade content.
        /// </summary>
        /// <param name="CP"></param>
        /// <returns></returns>
        public override object Execute(CPBaseClass CP) {
            try {
                // 
                // -- version used to upgrade content
                const int version = 3;
                int buildVersion = CP.Site.GetInteger("Form Wizard Version", 0);
                //
                //if (buildVersion < 2) {
                //    // 
                //    // -- version - 2, convert form.useauthmembercontent, form.useauthorgcontent, form.authcontent -> form.saveContentId (null=no-save, 1=no-save, 2=people-save, 3=org-save, 4=custom-save), saveCustomContent
                //    foreach (FormModel form in DbBaseModel.createList<FormModel>(CP, "", "id")) {
                //        //form.saveCustomContent = Conversions.ToString(form.authcontent.Equals(0) ? "" : CP.Content.GetID(form.authcontent.ToString()));
                //        form.saveTypeId = form.useauthmembercontent ? 2 : form.useauthorgcontent ? 3 : !string.IsNullOrWhiteSpace(form.saveCustomContent) ? 4 : 1;
                //        form.save(CP);
                //    }
                //}
                if (buildVersion < 3) {
                    //
                    // -- change question type from string to integer
                    CP.Db.ExecuteNonQuery("update ccFormFields set inputtypeid=1 where inputtype='TEXT'");
                    CP.Db.ExecuteNonQuery("update ccFormFields set inputtypeid=2 where inputtype='TEXTAREA'");
                    CP.Db.ExecuteNonQuery("update ccFormFields set inputtypeid=3 where inputtype='CHECKBOX'");
                    CP.Db.ExecuteNonQuery("update ccFormFields set inputtypeid=4 where inputtype='RADIO'");
                    CP.Db.ExecuteNonQuery("update ccFormFields set inputtypeid=5 where inputtype='FILE'");
                    CP.Db.ExecuteNonQuery("update ccFormFields set inputtypeid=6 where inputtype='SELECT'");
                }
                CP.Site.SetProperty("Form Wizard Version", version);
                //
                // -- update layout
                CP.Layout.updateLayout(Constants.guidLayoutFormWizard, Constants.nameLayoutFormWizard, Constants.pathFilenameLayoutFormWizard);
                //
                return string.Empty;
            } catch (Exception ex) {
                CP.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}