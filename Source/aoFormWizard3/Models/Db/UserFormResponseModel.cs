using Contensive.Models.Db;

namespace Contensive.Addon.aoFormWizard3.Models.Db {
    public class UserFormResponseModel : DbBaseModel {
        //
        public static DbBaseTableMetadataModel tableMetadata { get; private set; } = new DbBaseTableMetadataModel("User Form Response", "ccUserFormResponse", "default", false);
        // 
        // ====================================================================================================
        // -- instance properties
        // instancePropertiesGoHere
        public string copy { get; set; }
        public int visitid { get; set; }
    }
}