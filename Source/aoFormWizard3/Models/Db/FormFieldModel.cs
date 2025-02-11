using Contensive.BaseClasses;
using Contensive.Models.Db;
using System.Collections.Generic;

namespace Contensive.Addon.aoFormWizard3.Models.Db {
    public class FormFieldModel : DbBaseModel {
        public static DbBaseTableMetadataModel tableMetadata { get; private set; } = new DbBaseTableMetadataModel("Form Fields", "ccFormFields", "default", false);
        // 
        // ====================================================================================================
        // -- instance properties
        public int buttonactionid { get; set; }
        public string caption { get; set; }
        public string headline { get; set; }
        public string description { get; set; }
        public int contentfieldid { get; set; }
        public int formid { get; set; }
        /// <summary>
        /// Field type, string, can be "checkbox", "radio", "file", "text","textarea","select"
        /// </summary>
        /// <returns></returns>
        public string inputtype { get; set; }
        public string replacetext { get; set; }
        public bool @required { get; set; }
        /// <summary>
        /// Comma delimited list of options
        /// </summary>
        /// <returns></returns>
        public string optionList { get; set; }
        //
        // ====================================================================================================
        //
        /// <summary>
        /// return the question list in the correct order
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public static List<FormFieldModel> getQuestionList(CPBaseClass cp, int pageId) {
            return DbBaseModel.createList<FormFieldModel>(cp, $"(formid={pageId})", "sortorder,id");
        }

    }
}