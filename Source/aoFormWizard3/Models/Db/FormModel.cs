
using System.Collections.Generic;
using Contensive.BaseClasses;
using Contensive.Models.Db;
using Microsoft.SqlServer.Server;

namespace Contensive.Addon.aoFormWizard3.Models.Db {
    public class FormModel : DbBaseModel {
        //
        public static DbBaseTableMetadataModel tableMetadata { get; private set; } = new DbBaseTableMetadataModel("Forms", "ccForms", "default", false);
        //
        // -- instance properties
        //public bool addbackbutton { get; set; }
        public string backbuttonname { get; set; }
        public bool addcancelbutton { get; set; }
        public string cancelbuttonname { get; set; }
        public string continuebuttonname { get; set; }
        public string submitbuttonname { get; set; }
        public int contentid { get; set; }
        public int formsetid { get; set; }
        // Public Property htmlbody As String
        //public string newcontentname { get; set; }
        //public int nextformid { get; set; }
        public string description { get; set; }
        /// <summary>
        /// lookup (1=no-save, 2=people-save, 3=org-save, 4=custom-content-save)
        /// </summary>
        /// <returns></returns>
        public int saveTypeId { get; set; }
        /// <summary>
        /// text field to enter a custom content where data for this form should be saved
        /// tablename for this content should be 'formWizard' + normalize(saveContent). normalize should validate the allowed characters for sql server tables.
        /// </summary>
        /// <returns></returns>
        public string saveCustomContent { get; set; }
        /// <summary>
        /// deprecated, see saveTypeId
        /// </summary>
        /// <returns></returns>
        public int authcontent { get; set; }
        /// <summary>
        /// deprecated, see saveTypeId
        /// </summary>
        /// <returns></returns>
        public bool useauthmembercontent { get; set; }
        /// <summary>
        /// deprecated, see saveTypeId
        /// </summary>
        /// <returns></returns>
        public bool useauthorgcontent { get; set; }
        //
        // ====================================================================================================
        //
        /// <summary>
        /// return the form pages in the correct order
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="formSetId"></param>
        /// <returns></returns>
        public static List<FormModel> getPageList(CPBaseClass cp, int formSetId) {
            return DbBaseModel.createList<FormModel>(cp, $"(formsetid={formSetId})", "sortorder,id");
        }
    }
}