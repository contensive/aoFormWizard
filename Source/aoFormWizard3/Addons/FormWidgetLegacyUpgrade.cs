﻿using Contensive.BaseClasses;
using System;

namespace Contensive.Addon.aoFormWizard3.Views {
    // 
    public class FormWidgetLegacyUpgrade : AddonBaseClass {
        // 
        // =====================================================================================
        /// <summary>
        /// legacy form widget, send email to support and then call the new form widget
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public override object Execute(CPBaseClass cp) {
            try {
                cp.Email.send("support@contensive.com", "info@contensive.com", $"Legacy Form Widget on site [{cp.Request.Host}], page [{cp.Doc.PageId},{cp.Doc.PageName}]", "");
                return (new FormWidget()).Execute(cp);
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}