using Contensive.Models.Db;

namespace Contensive.FormWidget.Models.Db {
    public class DesignBlockThemeModel : DbBaseModel {
        public static DbBaseTableMetadataModel tableMetadata { get; private set; } = new DbBaseTableMetadataModel("Design Block Themes", "dbThemes", "default", false);
    }
}