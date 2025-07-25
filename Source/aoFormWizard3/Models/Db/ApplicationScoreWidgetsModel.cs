﻿using Contensive.BaseClasses;
using Contensive.DesignBlockBase.Models.Db;
using Contensive.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contensive.Addon.aoFormWizard3.Models.Db {
    public class ApplicationScoreWidgetsModel : SettingsBaseModel {
        //
        public static DbBaseTableMetadataModel tableMetadata { get; private set; } = new DbBaseTableMetadataModel("Application Score Widgets", "ccApplicationScoreWidgets", "default", false);        // <------ set set model Name and everywhere that matches this string
        // 
        // ====================================================================================================
        // -- instance properties
        public int formid { get; set; }
        public int groupAllowedToScore { get; set; }
        public string scoringInstructions { get; set; }
        public string scoringInstructionsTopOfApplication { get; set; }
        public string header { get; set; }
        public new static ApplicationScoreWidgetsModel createOrAddSettings(CPBaseClass cp, string settingsGuid, string recordNameOrSuffix) {
            try {
                // -- create object from existing record
                var result = create<ApplicationScoreWidgetsModel>(cp, settingsGuid);
                if (result is not null) {
                    return result;
                }
                // 
                // -- create default formset
                result = addDefault<ApplicationScoreWidgetsModel>(cp);
                result.name = "Application Scoring Widget " + result.id;
                result.ccguid = settingsGuid;
                result.formid = FormWidgetModel.createFirstOfList<FormWidgetModel>(cp, "", "dateadded desc")?.id ?? 0;
                result.save(cp);
                return result;
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}
