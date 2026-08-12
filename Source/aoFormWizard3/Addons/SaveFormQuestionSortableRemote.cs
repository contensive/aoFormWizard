using System;
using System.Collections.Generic;
using System.Linq;
using Contensive.BaseClasses;
using Contensive.FormWidget.Models.Db;
using Contensive.Models.Db;

namespace Contensive.FormWidget.Addons {
    public class SaveFormQuestionSortableRemote : AddonBaseClass {
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
                    // -- extract questionId from format "fq{questionId}"
                    int questionId = cp.Utils.EncodeInteger(arg.Replace("fq", ""));
                    if (questionId > 0) {
                        string sortOrder = (ptr * 10).ToString("0000");
                        var question = DbBaseModel.create<FormQuestionModel>(cp, questionId);
                        if (question != null) {
                            question.sortOrder = sortOrder;
                            question.save(cp);
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
