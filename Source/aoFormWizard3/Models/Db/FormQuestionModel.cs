using Contensive.BaseClasses;
using Contensive.Models.Db;
using System.Collections.Generic;

namespace Contensive.Addon.aoFormWizard3.Models.Db {
    public class FormQuestionModel : DbBaseModel {
        public static DbBaseTableMetadataModel tableMetadata { get; private set; } = new DbBaseTableMetadataModel("Form Questions", "ccFormQuestions", "default", false);
        // 
        // ====================================================================================================
        // -- instance properties
        public int buttonactionid { get; set; }
        public string caption { get; set; }
        public string headline { get; set; }
        public string description { get; set; }
        public int contentfieldid { get; set; }
        /// <summary>
        /// should be formPageId
        /// </summary>
        public int formid { get; set; }
        ///// <summary>
        ///// Field type, string, can be "checkbox", "radio", "file", "text","textarea","select"
        ///// </summary>
        ///// <returns></returns>
        //public string inputtype { get; set; }
        //
        /// <summary>
        /// see inputTypeEnum
        /// 1 = Short Text Answer
        /// 2 = Long Text Answer
        /// 3 = Check Boxes choose one
        /// 4 = Radio Boxes choose many
        /// 5 = Upload File
        /// 6 = Select List choose one
        /// </summary>
        public int inputTypeId { get; set; }
        public string replacetext { get; set; }
        public bool @required { get; set; }
        /// <summary>
        /// Comma delimited list of options
        /// </summary>
        /// <returns></returns>
        public string optionList { get; set; }
        //
        public enum inputTypeEnum {
            text = 1,
            textarea = 2,
            checkbox = 3,
            radio = 4,
            file = 5,
            select = 6
        }
        public static string getInputTypeName(int inputTypeId) {
            return inputTypeId switch {
                2 => "TEXTAREA",
                3 => "CHECKBOX",
                4 => "RADIO",
                5 => "FILE",
                6 => "SELECT",
                _ => "TEXT"
            };
        }
        //
        // ====================================================================================================
        //
        /// <summary>
        /// return the question list in the correct order
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public static List<FormQuestionModel> getQuestionList(CPBaseClass cp, int pageId) {
            return DbBaseModel.createList<FormQuestionModel>(cp, $"(formid={pageId})", "sortorder,id");
        }

    }
}