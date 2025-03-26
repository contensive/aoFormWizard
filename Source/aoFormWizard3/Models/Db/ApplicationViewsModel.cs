using Contensive.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contensive.Addon.aoFormWizard3.Models.Db {
    public class ApplicationViewsModel : DbBaseModel {
        public static DbBaseTableMetadataModel tableMetadata { get; private set; } = new DbBaseTableMetadataModel("Application Views", "applicationViews", "default", false);        // <------ set set model Name and everywhere that matches this string
        public int responseViewed { get; set; }
        public int viewer { get; set; }
        public DateTime dateViewed { get; set; }
    }
}
