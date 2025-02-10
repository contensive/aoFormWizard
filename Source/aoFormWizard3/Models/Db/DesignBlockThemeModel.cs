using Contensive.Models.Db;

namespace Contensive.Addon.aoFormWizard3.Models.Db {
    public class DesignBlockThemeModel : DbBaseModel {
        public static DbBaseTableMetadataModel tableMetadata { get; private set; } = new DbBaseTableMetadataModel("Design Block Themes", "dbThemes", "default", false);
    }
}