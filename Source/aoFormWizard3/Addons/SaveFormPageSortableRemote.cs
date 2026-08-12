using System;
using System.Collections.Generic;
using System.Linq;
using Contensive.BaseClasses;
using Contensive.FormWidget.Models.Db;
using Contensive.Models.Db;

namespace Contensive.FormWidget.Addons {
    public class SaveFormPageSortableRemote : AddonBaseClass {
        public override object Execute(CPBaseClass cp) {
            try {
                if (!cp.User.IsAdmin) { return "{}"; }
                //
                List<string> argList = cp.Doc.GetText("sortlist").Split(',').ToList();
                if (argList.Count == 0) { return "{}"; }
                //
                int ptr = 0;
                foreach (var arg in argList) {
                    //
                    // -- extract pageId from format "fp{pageId}"
                    int pageId = cp.Utils.EncodeInteger(arg.Replace("fp", ""));
                    if (pageId > 0) {
                        string sortOrder = (ptr * 10).ToString("0000");
                        var page = DbBaseModel.create<FormPageModel>(cp, pageId);
                        if (page != null) {
                            page.sortOrder = sortOrder;
                            page.save(cp);
                        }
                        ptr++;
                    }
                }
                //
                cp.Cache.InvalidateAll();
                return "{}";
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}
