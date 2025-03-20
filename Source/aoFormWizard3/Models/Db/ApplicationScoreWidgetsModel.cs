using Contensive.BaseClasses;
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
    }
}
