
using Contensive.BaseClasses;
using Contensive.DesignBlockBase.Models.Db;
using Contensive.Models.Db;

namespace Contensive.FormWidget.Models.Db {
    public class FormWidgetModel : SettingsBaseModel {
        //
        public static DbBaseTableMetadataModel tableMetadata { get; private set; } = new DbBaseTableMetadataModel("Form Widgets", "ccFormWidgets", "default", false);        // <------ set set model Name and everywhere that matches this string
        // 
        // ====================================================================================================
        // -- instance properties
        public int formId { get; set; }
        // 
        // ====================================================================================================
        public new static FormWidgetModel createOrAddSettings(CPBaseClass cp, string settingsGuid, string recordNameOrSuffix) {
            // 
            // -- create object from existing record
            var resultWidget = create<FormWidgetModel>(cp, settingsGuid);
            if (resultWidget is not null) {
                return resultWidget;
            }
            // 
            // -- create default formset
            resultWidget = addDefault<FormWidgetModel>(cp);
            resultWidget.name = "Form Widget " + resultWidget.id + " added to page " + cp.Doc.PageId + ", " + cp.Doc.PageName;
            resultWidget.ccguid = settingsGuid;
            resultWidget.save(cp);
            //
            // -- if there are no forms, create the sample.
            // -- else do not automatically create form. Let admin select a current form, or create one from the widget form
            if (DbBaseModel.createList<FormModel>(cp).Count == 0) {
                //
                // -- there are no forms, so this is the first, create the sample
                FormModel form = DbBaseModel.addDefault<FormModel>(cp);
                form.name = "Form 1 of the form-widget added to page [" + cp.Doc.PageId + ", " + cp.Doc.PageName + "]";
                form.addResetButton = false;
                form.resetButtonName = "Reset";
                form.backButtonName = "Previous";
                form.continueButtonName = "Continue";
                form.submitButtonName = "Complete";
                form.saveButtonName = "Save";
                form.save(cp);
                //
                resultWidget.formId = form.id;
                resultWidget.save(cp);
                // 
                // -- add form one
                FormPageModel formPageOne = addDefault<FormPageModel>(cp);
                formPageOne.name = "Form #1 of " + form.name;
                formPageOne.formid = form.id;
                formPageOne.description = "<h2>Form 1: Sample Form Content</h2>" + "<p>This form was automatically created by the Form Design Block.</p>" + "<p>A Dynamic Form is a list of Form Fields that you create and configure. Users complete the form and submit responses.</p>";
                formPageOne.sortOrder = "1";
                formPageOne.save(cp);
                // 
                // -- form 1 field A
                var formPageOneQuestionA = addDefault<FormQuestionModel>(cp);
                formPageOneQuestionA.formid = formPageOne.id;
                formPageOneQuestionA.caption = "Text Field Caption";
                formPageOneQuestionA.description = "Text Field Description";
                formPageOneQuestionA.headline = "";
                formPageOneQuestionA.inputTypeId = (int)FormQuestionModel.inputTypeEnum.text;
                formPageOneQuestionA.name = "Text Field Name";
                formPageOneQuestionA.optionList = "a,b,c,d,e,f,g";
                formPageOneQuestionA.replacetext = "replace-text";
                formPageOneQuestionA.@required = true;
                formPageOneQuestionA.sortOrder = "01";
                formPageOneQuestionA.save(cp);
                // 
                // -- form 1 field B
                var formPageOneQuestionB = addDefault<FormQuestionModel>(cp);
                formPageOneQuestionB.formid = formPageOne.id;
                formPageOneQuestionB.caption = "Checkbox Field Caption";
                formPageOneQuestionB.description = "Checkbox Field Description";
                formPageOneQuestionB.headline = "";
                formPageOneQuestionB.inputTypeId = (int)FormQuestionModel.inputTypeEnum.checkbox;
                formPageOneQuestionB.name = "Checkbox Field Name";
                formPageOneQuestionB.optionList = "a,b,c,d,e,f,g";
                formPageOneQuestionB.replacetext = "replace-text";
                formPageOneQuestionB.@required = true;
                formPageOneQuestionB.sortOrder = "02";
                formPageOneQuestionB.save(cp);
                // 
                // -- form 1 field C
                var formPageOneQuestionC = addDefault<FormQuestionModel>(cp);
                formPageOneQuestionC.formid = formPageOne.id;
                formPageOneQuestionC.caption = "Radio Field Caption";
                formPageOneQuestionC.description = "Radio Field Description";
                formPageOneQuestionC.headline = "";
                formPageOneQuestionC.inputTypeId = (int)FormQuestionModel.inputTypeEnum.radio;
                formPageOneQuestionC.name = "Radio Field Name";
                formPageOneQuestionC.optionList = "a,b,c,d,e,f,g";
                formPageOneQuestionC.replacetext = "replace-text";
                formPageOneQuestionC.@required = true;
                formPageOneQuestionC.sortOrder = "02";
                formPageOneQuestionC.save(cp);
                // 
                // -- form 1 field D
                var formPageOneQuestionD = addDefault<FormQuestionModel>(cp);
                formPageOneQuestionD.formid = formPageOne.id;
                formPageOneQuestionD.caption = "File Field Caption";
                formPageOneQuestionD.description = "File Field Description";
                formPageOneQuestionD.headline = "";
                formPageOneQuestionD.inputTypeId = (int)FormQuestionModel.inputTypeEnum.file;
                formPageOneQuestionD.name = "File Field Name";
                formPageOneQuestionD.optionList = "a,b,c,d,e,f,g";
                formPageOneQuestionD.replacetext = "replace-text";
                formPageOneQuestionD.@required = true;
                formPageOneQuestionD.sortOrder = "02";
                formPageOneQuestionD.save(cp);
                // 
                // -- add form page two
                FormPageModel formPageTwo = addDefault<FormPageModel>(cp);
                formPageTwo.name = "Form 2 of the form-widget added to page " + cp.Doc.PageId + ", " + cp.Doc.PageName;
                formPageTwo.formid = form.id;
                formPageTwo.description = "<h2>Form 2: Sample Form Content Form</h2>" + "<p>This is the second form in the form wizard.</p>";
                formPageTwo.sortOrder = "2";
                formPageTwo.save(cp);
                // 
                // -- form 2 field A
                var formPageTwoQuestionA = DbBaseModel.addDefault<FormQuestionModel>(cp);
                formPageTwoQuestionA.formid = formPageTwo.id;
                formPageTwoQuestionA.caption = "Text Field Caption";
                formPageTwoQuestionA.description = "Text Field Description";
                formPageTwoQuestionA.headline = "";
                formPageTwoQuestionA.inputTypeId = (int)FormQuestionModel.inputTypeEnum.text;
                formPageTwoQuestionA.name = "Text Field Name";
                formPageTwoQuestionA.optionList = "a,b,c,d,e,f,g";
                formPageTwoQuestionA.replacetext = "replace-text";
                formPageTwoQuestionA.@required = true;
                formPageTwoQuestionA.sortOrder = "01";
                formPageTwoQuestionA.save(cp);
                // 
                formPageOne.save(cp);
            }
            return resultWidget;
        }

    }
}