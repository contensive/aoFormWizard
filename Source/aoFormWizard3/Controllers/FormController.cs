using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.BaseClasses;
using Contensive.Models.Db;

namespace Contensive.Addon.aoFormWizard3.Controllers {
    public sealed class FormController {
        /// <summary>
        /// Process submitted contact form. Returns true if the form has already been submitted, or successfully commits
        /// </summary>
        /// <param name="CP"></param>
        /// <param name="settings"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool processRequest(CPBaseClass CP, FormSetModel settings, Views.FormRequest request) {
            string returnHtml = string.Empty;
            int hint = 0;
            try {
                if (string.IsNullOrWhiteSpace(request.button) || request.button.Equals("cancel"))
                    return false;
                var htmlVersion = new StringBuilder();
                var textVersion = new StringBuilder();
                int currentAuthContentRecordId = 0;

                foreach (var form in DbBaseModel.createList<FormModel>(CP, "(formsetid=" + settings.id + ")", "sortorder")) {
                    // remove any bad characters from the custom content name
                    string customContentName = form.saveCustomContent;
                    // replace custom content name with no nonalphanumeric characters
                    // includes spaces since this is content "[^A-Za-z0-9 ]+"
                    customContentName = Regex.Replace(customContentName, "[^A-Za-z0-9 ]+", "");

                    hint = 1;
                    foreach (var formsField in DbBaseModel.createList<FormFieldModel>(CP, "(formid=" + form.id + ")", "sortOrder,id")) {
                        // remove any bad characters from the customfieldname
                        string customFieldName = formsField.name;
                        // do not include spaces in the field name "[^A-Za-z0-9]+"
                        customFieldName = Regex.Replace(customFieldName, "[^A-Za-z0-9]+", "");

                        hint = 2;
                        textVersion.Append(Microsoft.VisualBasic.Constants.vbCrLf + "Question: " + formsField.name);
                        htmlVersion.Append("<div style=\"padding-top:10px;\"> Question:" + formsField.name + "</div>");
                        switch (formsField.inputtype.ToLower() ?? "") {
                            case "checkbox":
                            case "radio":
                            case "select": {
                                    hint = 3;
                                    string answerNumberCommaList = CP.Doc.GetText("formField_" + formsField.id);
                                    var answerNumberList = new List<string>(answerNumberCommaList.Split(','));
                                    int optionPtr = 1;
                                    var answerTextList = new List<string>();
                                    foreach (var formfieldoption in formsField.optionList.Split(',')) {
                                        if (answerNumberList.Contains(optionPtr.ToString())) {
                                            hint = 4;
                                            textVersion.Append(Microsoft.VisualBasic.Constants.vbCrLf + Microsoft.VisualBasic.Constants.vbTab + formfieldoption);
                                            htmlVersion.Append("<div style=\"padding-left:20px;\">" + formfieldoption + "</div>");
                                            if (!string.IsNullOrWhiteSpace(customContentName)) {
                                                answerTextList.Add(formfieldoption);
                                            }
                                        }
                                        optionPtr += 1;
                                    }
                                    // custom content
                                    if (form.saveTypeId.Equals(4) & !string.IsNullOrWhiteSpace(customContentName)) {
                                        hint = 10;
                                        bool verifiedContent = CustomContentController.verifyCustomContent(CP, customContentName);
                                        if (verifiedContent) {
                                            // make sure the content has this field
                                            if (!CP.Content.IsField(customContentName, customFieldName)) {
                                                hint = 11;
                                                // create a text field
                                                CustomContentController.createCustomContentField(CP, customContentName, customFieldName, (int)Constants.FieldTypeIdEnum.Text);
                                            }

                                            using (var cs = CP.CSNew()) {
                                                // after the field should have been created, check again
                                                if (CP.Content.IsField(customContentName, customFieldName)) {
                                                    hint = 12;
                                                    if (currentAuthContentRecordId == 0) {
                                                        hint = 13;
                                                        cs.Insert(customContentName);
                                                        currentAuthContentRecordId = cs.GetInteger("id");
                                                    }
                                                    if (cs.Open(customContentName, "id=" + currentAuthContentRecordId)) {
                                                        hint = 14;
                                                        // list inot a string with commas
                                                        string value = string.Join(",", answerTextList);
                                                        cs.SetField(customFieldName, value);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    break;
                                }
                            case "file": {
                                    hint = 20;
                                    string fieldName = "formField_" + formsField.id;
                                    string uploadFilename = CP.Doc.GetText(fieldName);
                                    if (!string.IsNullOrEmpty(uploadFilename)) {
                                        hint = 21;
                                        var folder = DbBaseModel.createByUniqueName<LibraryFolderModel>(CP, "Form Wizard Uploads");
                                        if (folder is null) {
                                            hint = 22;
                                            folder = DbBaseModel.addDefault< LibraryFolderModel>(CP);
                                            folder.name = "Form Wizard Uploads";
                                            folder.save(CP);
                                        }
                                        string pathFilename = "";
                                        using (var cs = CP.CSNew()) {
                                            hint = 23;
                                            cs.Insert("Library Files");
                                            cs.SetFormInput("filename", fieldName);
                                            cs.SetField("folderid", folder.id);
                                            cs.SetField("description", "Form wizard upload by " + CP.User.Name);
                                            cs.Save();
                                            // 
                                            pathFilename = cs.GetText("filename");
                                            var fileDetails = CP.CdnFiles.FileDetails(pathFilename);
                                            cs.SetField("name", fileDetails.Name);
                                            cs.SetField("filesize", fileDetails.Size);
                                            cs.Save();
                                        }
                                        // 
                                        if (form.useauthorgcontent) {
                                            hint = 24;
                                            if (CP.Content.IsField("Organizations", formsField.name)) {
                                                // make sure the form's field exists in the people table
                                                using (var cs2 = CP.CSNew()) {
                                                    if (cs2.Open("Organizations", "id=" + CP.User.OrganizationID)) {
                                                        hint = 25;
                                                        cs2.SetField(formsField.name, pathFilename);
                                                        cs2.Save();
                                                    }
                                                }
                                            }
                                        }
                                        // custom content
                                        if (form.saveTypeId.Equals(4) & !string.IsNullOrWhiteSpace(customContentName)) {
                                            hint = 30;
                                            bool verifiedContent = CustomContentController.verifyCustomContent(CP, customContentName);
                                            if (verifiedContent) {
                                                // make sure the content has this field
                                                if (!CP.Content.IsField(customContentName, customFieldName)) {
                                                    hint = 31;
                                                    // create file field
                                                    CustomContentController.createCustomContentField(CP, customContentName, customFieldName, (int)Constants.FieldTypeIdEnum.File);
                                                }
                                                using (var csContent = CP.CSNew()) {
                                                    // after the field should have been created, check again
                                                    if (CP.Content.IsField(customContentName, customFieldName)) {
                                                        hint = 32;
                                                        if (currentAuthContentRecordId == 0) {
                                                            hint = 33;
                                                            csContent.Insert(customContentName);
                                                            currentAuthContentRecordId = csContent.GetInteger("id");
                                                        }
                                                        if (csContent.Open(customContentName, "id=" + currentAuthContentRecordId)) {
                                                            csContent.SetField(customFieldName, pathFilename);
                                                            csContent.Save();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        textVersion.Append(Microsoft.VisualBasic.Constants.vbCrLf + Microsoft.VisualBasic.Constants.vbTab + CP.Http.CdnFilePathPrefixAbsolute + pathFilename);
                                        htmlVersion.Append("<div style=\"padding-left:20px;\"><a href=\"" + CP.Http.CdnFilePathPrefixAbsolute + pathFilename + "\">" + uploadFilename + "</a></div>");

                                    }

                                    break;
                                }

                            default: {
                                    if (form.saveTypeId.Equals(4) & !string.IsNullOrWhiteSpace(customContentName)) {
                                        hint = 50;
                                        hint = 10;
                                        bool verifiedContent = CustomContentController.verifyCustomContent(CP, customContentName);
                                        if (verifiedContent) {
                                            // make sure the content has this field
                                            if (!CP.Content.IsField(customContentName, customFieldName)) {
                                                hint = 11;
                                                // determine which kind of field to create
                                                switch (formsField.inputtype.ToLower() ?? "") {
                                                    case "text": {
                                                            // create a text field if this type is for text
                                                            CustomContentController.createCustomContentField(CP, customContentName, customFieldName, (int)Constants.FieldTypeIdEnum.Text);
                                                            break;
                                                        }

                                                    default: {
                                                            // this is a longtext or unknown so default to longtext
                                                            CustomContentController.createCustomContentField(CP, customContentName, customFieldName, (int)Constants.FieldTypeIdEnum.LongText);
                                                            break;
                                                        }
                                                }
                                            }

                                            using (var cs = CP.CSNew()) {
                                                // after the field should have been created, check again
                                                if (CP.Content.IsField(customContentName, customFieldName)) {
                                                    hint = 12;
                                                    if (currentAuthContentRecordId == 0) {
                                                        hint = 13;
                                                        cs.Insert(customContentName);
                                                        currentAuthContentRecordId = cs.GetInteger("id");
                                                    }
                                                    if (cs.Open(customContentName, "id=" + currentAuthContentRecordId)) {
                                                        cs.SetField(customFieldName, CP.Doc.GetText("formField_" + formsField.id));
                                                    }
                                                }
                                            }
                                        }
                                    } else if (form.saveTypeId.Equals(3)) {
                                        hint = 60;
                                        // 
                                        // save to organization
                                        if (CP.Content.IsField("organizations", formsField.name)) {
                                            using (var cs = CP.CSNew()) {
                                                // make sure the form's field exists in the people table
                                                if (cs.Open("organizations", "id=" + CP.User.OrganizationID) & cs.FieldOK(formsField.name)) {
                                                    cs.SetField(formsField.name, CP.Doc.GetText("formField_" + formsField.id));
                                                    cs.Save();
                                                }
                                            }
                                        }
                                    } else if (form.saveTypeId.Equals(2)) {
                                        hint = 70;
                                        // 
                                        // -- save to people table
                                        if (CP.Content.IsField("People", formsField.name)) {
                                            using (var cs = CP.CSNew()) {
                                                // make sure the form's field exists in the people table
                                                if (cs.Open("People", "id=" + CP.User.Id) & cs.FieldOK(formsField.name)) {
                                                    cs.SetField(formsField.name, CP.Doc.GetText("formField_" + formsField.id));
                                                    cs.Save();
                                                }
                                            }
                                        }
                                    }
                                    textVersion.Append(Microsoft.VisualBasic.Constants.vbCrLf + Microsoft.VisualBasic.Constants.vbTab + CP.Doc.GetText("formField_" + formsField.id));
                                    htmlVersion.Append("<div style=\"padding-left:20px;\">" + CP.Doc.GetText("formField_" + formsField.id) + "</div>");
                                    break;
                                }
                        }
                    }
                }
                CP.Email.sendSystem(settings.notificationemailid, htmlVersion.ToString());
                if (settings.responseemailid > 0) {
                    CP.Email.sendSystem(settings.responseemailid, "", CP.User.Id);
                }
                if (settings.joingroupid != 0) {
                    CP.Group.AddUser(settings.joingroupid, CP.User.Id);
                }
                var userFormResponse = DbBaseModel.addDefault< UserFormResponseModel>(CP);
                userFormResponse.visitid = CP.Visit.Id;
                userFormResponse.copy = textVersion.ToString();
                userFormResponse.name = "Form Set " + settings.name + " completed on " + DateTime.Now.ToString("MM/dd/yyyy") + " by " + CP.User.Name;
                userFormResponse.save(CP);
                return true;
            } catch (Exception ex) {
                CP.Site.ErrorReport("hint=" + hint.ToString() +" " + ex.ToString());
                return false;
            }
        }
    }
    public class FormControllerOptionClass {
        public string optionName { get; set; }
        public int optionPtr { get; set; }
    }
}