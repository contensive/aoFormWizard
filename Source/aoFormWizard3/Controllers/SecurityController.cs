using Contensive.BaseClasses;
using System;
// -- Imports Contensive.Models.Db

namespace Contensive.Addon.aoFormWizard3.Controllers {
    // 
    // ===================================================================================
    // 
    public sealed class SecurityController {
        // 
        // ===================================================================================
        // 
        /// <summary>
        /// if the user is not authorized, return this response
        /// use as:
        /// if not cp.user.isadmin() then return getNotAuthorizedHtmlResponse(cp)
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public static string getNotAuthorizedHtmlResponse(CPBaseClass cp) {
            try {
                // 
                // 
                // -- not authenticated, block with login
                if (!cp.User.IsAuthenticated) {
                    // 
                    // --- must be authenticated to continue. Force a local login
                    cp.Doc.SetProperty("requirePassword", true);
                    return cp.Addon.Execute(Constants.guidAddonLoginPage, new CPUtilsBaseClass.addonExecuteContext() {
                        errorContextMessage = "Not authenticated for account manager",
                        addonType = CPUtilsBaseClass.addonContext.ContextPage
                    });
                }
                // 
                // -- else not authorized
                return "<p class=\">You are not authorized to use this application.</p>";

            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}