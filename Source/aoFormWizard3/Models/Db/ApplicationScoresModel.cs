using Contensive.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contensive.FormWidget.Models.Db {
    public class ApplicationScoresModel : DbBaseModel {
        public static DbBaseTableMetadataModel tableMetadata { get; private set; } = new DbBaseTableMetadataModel("Application Scores", "ccApplicationScores", "default", false);        // <------ set set model Name and everywhere that matches this string
        public int scorer { get; set; }
        public int score { get; set; }
        public int applicationFormScored { get; set; }
        public int applicationSubmittedScored { get; set; }
        public string comment { get; set; }
    }
}
