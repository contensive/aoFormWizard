
using System;
using System.Collections.Generic;
using Contensive.BaseClasses;
using Contensive.Models.Db;

namespace Contensive.FormWidget.Models.Db {
    public class DesignBlockFontModel : DbBaseModel {
        public static DbBaseTableMetadataModel tableMetadata { get; private set; } = new DbBaseTableMetadataModel("Design Block Fonts", "dbFonts", "default", false);
        // 
        // ====================================================================================================
        // -- instance properties
    }
}