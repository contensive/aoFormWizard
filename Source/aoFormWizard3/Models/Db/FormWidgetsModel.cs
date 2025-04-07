
using Contensive.BaseClasses;
using Contensive.DesignBlockBase.Models.Db;
using Contensive.Models.Db;

namespace Contensive.Addon.aoFormWizard3.Models.Db {
    public class FormWidgetsModel : SettingsBaseModel {
        //
        public static DbBaseTableMetadataModel tableMetadata { get; private set; } = new DbBaseTableMetadataModel("Form Widgets", "ccFormWidgets", "default", false);        // <------ set set model Name and everywhere that matches this string
        // 
        // ====================================================================================================
        // -- instance properties
        public string backButtonName { get; set; }
        public bool addResetButton { get; set; }
        public string resetButtonName { get; set; }
        public string continueButtonName { get; set; }
        public string submitButtonName { get; set; }
        public string saveButtonName { get; set; }

        public int joingroupid { get; set; }
        public int notificationemailid { get; set; }
        public int responseemailid { get; set; }
        public string thankyoucopy { get; set; }        
        public bool useUserProperty { get; set; }
        public bool allowRecaptcha { get; set; }
        // 
        // ====================================================================================================
        public new static FormWidgetsModel createOrAddSettings(CPBaseClass cp, string settingsGuid, string recordNameOrSuffix) {
            // 
            // -- create object from existing record
            var result = create<FormWidgetsModel>(cp, settingsGuid);
            if (result is not null) {
                return result; 
            }
            // 
            // -- create default formset
            result = addDefault<FormWidgetsModel>(cp);
            result.name = "Dynamic Form " + result.id + " added to page " + cp.Doc.PageId + ", " + cp.Doc.PageName;
            result.addResetButton = false;
            result.resetButtonName = "Reset";
            result.backButtonName = "Previous";
            result.continueButtonName = "Continue";
            result.submitButtonName = "Complete";
            result.saveButtonName = "Save";
            result.ccguid = settingsGuid;
            result.save(cp);
            // 
            // -- add form one
            var formOne = addDefault<FormPagesModel>(cp);
            formOne.name = "Form #1 of " + result.name;
            formOne.formsetid = result.id;
            formOne.description = "<h2>Form 1: Sample Form Wizard Form</h2>" + "<p>This form was automatically created by the Form Wizard Design Block.</p>" + "<p>A Dynamic Form is a list of Form Fields that you create and configure. Users complete the form and submit responses.</p>";

            formOne.sortOrder = "1";
            formOne.save(cp);
            // 
            // -- form 1 field A
            var formOneFieldA = DbBaseModel.addDefault<FormQuestionsModel>(cp);
            formOneFieldA.formid = formOne.id;
            formOneFieldA.caption = "Text Field Caption";
            formOneFieldA.description = "Text Field Description";
            formOneFieldA.headline = "";
            formOneFieldA.inputTypeId = (int)FormQuestionsModel.inputTypeEnum.text;
            formOneFieldA.name = "Text Field Name";
            formOneFieldA.optionList = "a,b,c,d,e,f,g";
            formOneFieldA.replacetext = "replace-text";
            formOneFieldA.@required = true;
            formOneFieldA.sortOrder = "01";
            formOneFieldA.save(cp);
            // 
            // -- form 1 field B
            var formOneFieldB = DbBaseModel.addDefault<FormQuestionsModel>(cp);
            formOneFieldB.formid = formOne.id;
            formOneFieldB.caption = "Checkbox Field Caption";
            formOneFieldB.description = "Checkbox Field Description";
            formOneFieldB.headline = "";
            formOneFieldB.inputTypeId = (int)FormQuestionsModel.inputTypeEnum.checkbox;
            formOneFieldB.name = "Checkbox Field Name";
            formOneFieldB.optionList = "a,b,c,d,e,f,g";
            formOneFieldB.replacetext = "replace-text";
            formOneFieldB.@required = true;
            formOneFieldB.sortOrder = "02";
            formOneFieldB.save(cp);
            // 
            // -- form 1 field C
            var formOneFieldC = DbBaseModel.addDefault<FormQuestionsModel>(cp);
            formOneFieldC.formid = formOne.id;
            formOneFieldC.caption = "Radio Field Caption";
            formOneFieldC.description = "Radio Field Description";
            formOneFieldC.headline = "";
            formOneFieldC.inputTypeId = (int)FormQuestionsModel.inputTypeEnum.radio;
            formOneFieldC.name = "Radio Field Name";
            formOneFieldC.optionList = "a,b,c,d,e,f,g";
            formOneFieldC.replacetext = "replace-text";
            formOneFieldC.@required = true;
            formOneFieldC.sortOrder = "02";
            formOneFieldC.save(cp);
            // 
            // -- form 1 field D
            var formOneFieldD = DbBaseModel.addDefault<FormQuestionsModel>(cp);
            formOneFieldD.formid = formOne.id;
            formOneFieldD.caption = "File Field Caption";
            formOneFieldD.description = "File Field Description";
            formOneFieldD.headline = "";
            formOneFieldD.inputTypeId = (int)FormQuestionsModel.inputTypeEnum.file;
            formOneFieldD.name = "File Field Name";
            formOneFieldD.optionList = "a,b,c,d,e,f,g";
            formOneFieldD.replacetext = "replace-text";
            formOneFieldD.@required = true;
            formOneFieldD.sortOrder = "02";
            formOneFieldD.save(cp);
            // 
            // -- add form two
            /*
            var formTwo = DbBaseModel.addDefault<FormModel>(cp);
            formTwo.name = "Form #2 of " + result.name;
            formTwo.formsetid = result.id;
            formOne.description = "<h2>Form 2: Sample Form Wizard Form</h2>" + "<p>This is the second form in the form wizard.</p>";
            formOne.sortOrder = "2";
            formTwo.save(cp);
            // 
            // -- form 2 field A
            var formTwoFieldA = DbBaseModel.addDefault<FormFieldModel>(cp);
            formTwoFieldA.formid = formTwo.id;
            formTwoFieldA.caption = "Text Field Caption";
            formTwoFieldA.description = "Text Field Description";
            formTwoFieldA.headline = "";
            formTwoFieldA.inputTypeId = (int)FormFieldModel.inputTypeEnum.text;
            formTwoFieldA.name = "Text Field Name";
            formTwoFieldA.optionList = "a,b,c,d,e,f,g";
            formTwoFieldA.replacetext = "replace-text";
            formTwoFieldA.@required = true;
            formTwoFieldA.sortOrder = "01";
            formTwoFieldA.save(cp);
            // 
            */
            formOne.save(cp);
            // 
            // -- create custom content
            // 
            return result;
        }

    }
}