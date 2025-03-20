using Contensive.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contensive.Addon.aoFormWizard3.Models.Db {
    public class ApplicationScoresModel : DbBaseModel {
        public static DbBaseTableMetadataModel tableMetadata { get; private set; } = new DbBaseTableMetadataModel("Application Scores", "ccApplicationScores", "default", false);        // <------ set set model Name and everywhere that matches this string
        public int scorer { get; set; }
        public string score { get; set; }
        public int application { get; set; }
    }
}
