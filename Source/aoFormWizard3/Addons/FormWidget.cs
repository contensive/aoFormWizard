using System;
using Contensive.Addon.aoFormWizard3.Controllers;
using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.Addon.aoFormWizard3.Models.View;
using Contensive.BaseClasses;
using Contensive.DesignBlockBase.Controllers;

namespace Contensive.Addon.aoFormWizard3.Views {
    // 
    public class FormWidget : AddonBaseClass {
        // 
        // =====================================================================================
        /// <summary>
        /// Addon api - Dynamic Form addon, Render dynamic form
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public override object Execute(CPBaseClass cp) {
            try {
                return DesignBlockController.renderWidget<FormSetModel, FormViewModel>(cp,
                    widgetName: "Form Widget",
                    layoutGuid: Constants.guidLayoutFormWizard,
                    layoutName: Constants.nameLayoutFormWizard,
                    layoutPathFilename: Constants.pathFilenameLayoutFormWizard,
                    layoutBS5PathFilename: Constants.pathFilenameLayoutFormWizard);
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
            //const string designBlockName = "Form Wizard";
            //try {
            //    // 
            //    // -- read instanceId, guid created uniquely for this instance of the addon on a page
            //    string result = string.Empty;
            //    string settingsGuid = InstanceIdController.getSettingsGuid(cp, designBlockName, ref result);
            //    if (string.IsNullOrEmpty(settingsGuid))
            //        return result;
            //    // 
            //    // -- locate or create a data record for this guid
            //    var settings = FormSetModel.createOrAddSettings(cp, settingsGuid);
            //    if (settings is null)
            //        throw new ApplicationException("Could not create the design block settings record.");
            //    // 
            //    // 
            //    // -- process form request
            //    var request = new FormRequest() { button = cp.Doc.GetText("button") };
            //    if (FormController.processRequest(cp, settings, request)) {
            //        // 
            //        // -- simple thank you content
            //        cp.Doc.SetProperty("formwizardcomplete", true);
            //        result = cp.Html.div(settings.thankyoucopy);
            //    } else {
            //        // 
            //        // -- translate the Db model to a view model and mustache it into the layout
            //        var viewModel = Models.View.FormViewModel.create(cp, settings);
            //        if (viewModel is null)
            //            throw new ApplicationException("Could not create design block view model.");
            //        // 
            //        // -- translate view model into html
            //        result = cp.Html.Form(cp.Mustache.Render(My.Resources.Resources.FormWizard, viewModel));
            //    }
            //    // 
            //    // -- if editing enabled, add the link and wrapperwrapper
            //    return genericController.addEditWrapper(cp, result, settings.id, settings.name, FormSetModel.contentName);
            //} catch (Exception ex) {
            //    cp.Site.ErrorReport(ex);
            //    return "<!-- " + designBlockName + ", Unexpected Exception -->";
            //}
        }
    }
    //// 
    //// =====================================================================================
    ///// <summary>
    ///// Request object for main
    ///// </summary>
    //public class FormRequest {
    //    public string button;
    //}
}