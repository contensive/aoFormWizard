using System;
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
                if (CP.Site.GetInteger("Form Wizard Version", 0) < Constants.version) {
                    // 
                    // -- version - 2, convert form.useauthmembercontent, form.useauthorgcontent, form.authcontent -> form.saveContentId (null=no-save, 1=no-save, 2=people-save, 3=org-save, 4=custom-save), saveCustomContent
                    foreach (FormModel form in DbBaseModel.createList<FormModel>(CP, "", "id")) {
                        form.saveCustomContent = Conversions.ToString(form.authcontent.Equals(0) ? "" : CP.Content.GetID(form.authcontent.ToString()));
                        form.saveTypeId = form.useauthmembercontent ? 2 : form.useauthorgcontent ? 3 : !string.IsNullOrWhiteSpace(form.saveCustomContent) ? 4 : 1;
                        form.save(CP);
                    }
                    CP.Site.SetProperty("Form Wizard Version", Constants.version);
                }
                // -- update layout
                CP.Layout.updateLayout(Constants.guidLayoutFormWizard, Constants.nameLayoutFormWizard, Constants.pathFilenameLayoutFormWizard);

                return string.Empty;
            } catch (Exception ex) {
                CP.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}