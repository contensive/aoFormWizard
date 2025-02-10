
//using System.Collections.Generic;
//using Contensive.BaseClasses;

//namespace Contensive.Addon.aoFormWizard3.Models.Db {
//    public class LibraryFolderModel : BaseModel {        // <------ set set model Name and everywhere that matches this string
//        // 
//        // ====================================================================================================
//        // -- const
//        public const string contentName = "Library Folders";      // <------ set content name
//        public const string contentTableName = "ccLibraryFolders";   // <------ set to tablename for the primary content (used for cache names)
//        private new const string contentDataSource = "default";             // <------ set to datasource if not default
//        // 
//        // ====================================================================================================
//        // -- instance properties
//        // instancePropertiesGoHere
//        public string description { get; set; }
//        public int parentid { get; set; }
//        // 
//        // ====================================================================================================
//        public static LibraryFolderModel @add(CPBaseClass cp) {
//            return @add<LibraryFolderModel>(cp);
//        }
//        // 
//        // ====================================================================================================
//        public static LibraryFolderModel create(CPBaseClass cp, int recordId) {
//            return create<LibraryFolderModel>(cp, recordId);
//        }
//        // 
//        // ====================================================================================================
//        public static LibraryFolderModel create(CPBaseClass cp, string recordGuid) {
//            return create<LibraryFolderModel>(cp, recordGuid);
//        }
//        // 
//        // ====================================================================================================
//        public static LibraryFolderModel createByName(CPBaseClass cp, string recordName) {
//            return createByName<LibraryFolderModel>(cp, recordName);
//        }
//        // 
//        // ====================================================================================================
//        public new void save(CPBaseClass cp) {
//            base.save(cp);
//        }
//        // 
//        // ====================================================================================================
//        public static void delete(CPBaseClass cp, int recordId) {
//            delete<LibraryFolderModel>(cp, recordId);
//        }
//        // 
//        // ====================================================================================================
//        public static void delete(CPBaseClass cp, string ccGuid) {
//            delete<LibraryFolderModel>(cp, ccGuid);
//        }
//        // 
//        // ====================================================================================================
//        public static List<LibraryFolderModel> createList(CPBaseClass cp, string sqlCriteria, string sqlOrderBy = "id") {
//            return createList<LibraryFolderModel>(cp, sqlCriteria, sqlOrderBy);
//        }
//        // 
//        // ====================================================================================================
//        public static string getRecordName(CPBaseClass cp, int recordId) {
//            return getRecordName<LibraryFolderModel>(cp, recordId);
//        }
//        // 
//        // ====================================================================================================
//        public static string getRecordName(CPBaseClass cp, string ccGuid) {
//            return getRecordName<LibraryFolderModel>(cp, ccGuid);
//        }
//        // 
//        // ====================================================================================================
//        public static int getRecordId(CPBaseClass cp, string ccGuid) {
//            return getRecordId<LibraryFolderModel>(cp, ccGuid);
//        }
//        // 
//        // ====================================================================================================
//        public static int getCount(CPBaseClass cp, string sqlCriteria) {
//            return getCount<LibraryFolderModel>(cp, sqlCriteria);
//        }
//        // 
//        // ====================================================================================================
//        public string getUploadPath(string fieldName) {
//            return getUploadPath<LibraryFolderModel>(fieldName);
//        }

//    }
//}