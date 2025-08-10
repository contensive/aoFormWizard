
using System.Collections.Generic;
using Contensive.BaseClasses;
using Contensive.Models.Db;
using Microsoft.SqlServer.Server;

namespace Contensive.Addon.aoFormWizard3.Models.Db {
    public class FormPageModel : DbBaseModel {
        //
        public static DbBaseTableMetadataModel tableMetadata { get; private set; } = new DbBaseTableMetadataModel("Form Pages", "ccFormPages", "default", false);
        //
        // -- instance properties
        public int contentid { get; set; }
        /// <summary>
        /// the ccForms record under which the form page is presented
        /// Widgets point to a form
        /// forms have form pages
        /// form pages have form questions
        /// </summary>
        public int formid { get; set; }
        /// <summary>
        /// description
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// lookup (1=no-save, 2=people-save, 3=org-save, 4=custom-content-save)
        /// </summary>
        /// <returns></returns>
        public int saveTypeId { get; set; }
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
        /// <param name="formId"></param>
        /// <returns></returns>
        public static List<FormPageModel> getPageList(CPBaseClass cp, int formId) {
            return DbBaseModel.createList<FormPageModel>(cp, $"(formid={formId})", "sortorder,id");
        }
    }
}