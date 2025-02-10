using System;
using System.Collections.Generic;
using System.Linq;
using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.BaseClasses;
using Contensive.DesignBlockBase.Models.Db;
using Contensive.DesignBlockBase.Models.View;
using Contensive.Models.Db;

namespace Contensive.Addon.aoFormWizard3.Models.View {
    /// <summary>
    /// Construct the view to be displayed (the current form)
    /// </summary>
    public class FormViewModel : DesignBlockViewBaseModel {
        // 
        public int id { get; set; }
        public string formHtmlId { get; set; }
        /// <summary>
        /// has to be added to the form so it can be rendered in the callback
        /// </summary>
        public string instanceId { get; set; }
        public bool allowRecaptcha { get; set; }
        public string recaptchaHTML { get; set; }
        public string pageHeader { get; set; }
        public string pageDescription { get; set; }
        public List<FieldViewModel> listOfFieldsClass { get; set; } = new List<FieldViewModel>();
        public string fieldAddLink { get; set; }
        public string previousButton { get; set; }
        public string cancelButton { get; set; }
        public string submitButton { get; set; }
        public string continueButton { get; set; }
        public string formEditWrapper { get; set; }
        public string formdEditLink { get; set; }
        public string formAddLink { get; set; }
        // 
        public class FieldViewModel {
            public string inputtype { get; set; }
            public string caption { get; set; }
            public string headline { get; set; }
            public string fielddescription { get; set; }
            public bool @required { get; set; }
            public string name { get; set; }
            public bool isCheckbox { get; set; }
            public bool isRadio { get; set; }
            public bool isSelect { get; set; }
            public bool isTextArea { get; set; }
            public bool isDefault { get; set; }
            public int id { get; set; }
            public List<OptionClass> optionList { get; set; } = new List<OptionClass>();
            public string fieldEditWrapper { get; set; }
            public string fieldEditLink { get; set; }
        }
        // 
        public class OptionClass {
            public string optionName { get; set; }
            public int optionPtr { get; set; }
        }
        // 
        public class ButtonClass {
            public string buttonCaption { get; set; }
            public bool isVisible { get; set; }
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// Populate the view model from the entity model
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static FormViewModel create(CPBaseClass cp, FormSetModel settings) {
            try {
                // 
                // -- base fields
                var formViewData = create<FormViewModel>(cp, settings);
                formViewData.id = settings.id;
                formViewData.instanceId = settings.ccguid;
                formViewData.formHtmlId = cp.Utils.GetRandomString(4);
                formViewData.allowRecaptcha = settings.allowRecaptcha;
                formViewData.recaptchaHTML = settings.allowRecaptcha ? cp.Addon.Execute(Constants.guidAddonRecaptchav2) : "";

                // 
                var formlist = DbBaseModel.createList<FormModel>(cp, "(formsetid=" + settings.id + ")", "sortorder");
                if (formlist.Count > 0) {
                    var firstForm = formlist.First();
                    // 
                    // -- output one page with page one header
                    bool isEditing = cp.User.IsEditing();
                    formViewData.pageHeader = firstForm.name;
                    formViewData.pageDescription = firstForm.description;
                    formViewData.previousButton = firstForm.addbackbutton ? "Previous" : "";
                    formViewData.cancelButton = firstForm.addcancelbutton ? firstForm.cancelbuttonname : "";
                    formViewData.submitButton = "";
                    formViewData.continueButton = firstForm.addcontinuebutton ? firstForm.continuebuttonname : "";
                    //
                    if (isEditing) {
                        formViewData.formEditWrapper = "ccEditWrapper";
                        formViewData.formdEditLink = cp.Content.GetEditLink(FormModel.tableMetadata.contentName, firstForm.id.ToString(), false, "", isEditing);
                    }
                    //
                    foreach (var form in formlist) {
                        var formFieldList = DbBaseModel.createList<FormFieldModel>(cp, "(formid=" + form.id + ")", "sortorder, id");
                        int fieldPtr = 0;
                        foreach (var formField in formFieldList) {
                            var optionList = new List<OptionClass>();
                            int optionPtr = 1;
                            foreach (var formfieldoption in formField.optionList.Split(',')) {
                                optionList.Add(new OptionClass() {
                                    optionName = formfieldoption,
                                    optionPtr = optionPtr
                                });
                                optionPtr += 1;
                            }
                            string fieldEditLink = cp.Content.GetEditLink(FormFieldModel.tableMetadata.contentName, formField.id.ToString(), false, "Edit Question", isEditing);
                            switch (formField.inputtype.ToLower() ?? "") {
                                case "radio": {
                                        string caption = formField.caption;
                                        if (string.IsNullOrEmpty(caption)) {
                                            caption = formField.name;
                                        }
                                        formViewData.listOfFieldsClass.Add(new FieldViewModel() {
                                            caption = caption,
                                            inputtype = formField.inputtype,
                                            @required = formField.@required,
                                            name = formField.name,
                                            headline = formField.headline,
                                            fielddescription = formField.description,
                                            isCheckbox = false,
                                            isDefault = false,
                                            isTextArea = false,
                                            isRadio = true,
                                            isSelect = false,
                                            id = formField.id,
                                            optionList = optionList,
                                            fieldEditLink  = fieldEditLink,
                                            fieldEditWrapper = isEditing ? "ccEditWrapper" : ""
                                        });
                                        break;
                                    }
                                case "select": {
                                        string caption = formField.caption;
                                        if (string.IsNullOrEmpty(caption)) {
                                            caption = formField.name;
                                        }
                                        formViewData.listOfFieldsClass.Add(new FieldViewModel() {
                                            caption = caption,
                                            inputtype = formField.inputtype,
                                            @required = formField.@required,
                                            name = formField.name,
                                            headline = formField.headline,
                                            fielddescription = formField.description,
                                            isCheckbox = false,
                                            isDefault = false,
                                            isTextArea = false,
                                            isRadio = false,
                                            isSelect = true,
                                            id = formField.id,
                                            optionList = optionList,
                                            fieldEditLink = fieldEditLink,
                                            fieldEditWrapper = isEditing ? "ccEditWrapper" : ""
                                        });
                                        break;
                                    }
                                case "checkbox": {
                                        string caption = formField.caption;
                                        if (string.IsNullOrEmpty(caption)) {
                                            caption = formField.name;
                                        }
                                        formViewData.listOfFieldsClass.Add(new FieldViewModel() {
                                            caption = caption,
                                            inputtype = formField.inputtype,
                                            @required = formField.@required,
                                            name = formField.name,
                                            headline = formField.headline,
                                            fielddescription = formField.description,
                                            isCheckbox = true,
                                            isDefault = false,
                                            isTextArea = false,
                                            isRadio = false,
                                            isSelect = false,
                                            id = formField.id,
                                            optionList = optionList,
                                            fieldEditLink = fieldEditLink,
                                            fieldEditWrapper = isEditing ? "ccEditWrapper" : ""
                                        });
                                        break;
                                    }
                                case "textarea": {
                                        string caption = formField.caption;
                                        if (string.IsNullOrEmpty(caption)) {
                                            caption = formField.name;
                                        }
                                        formViewData.listOfFieldsClass.Add(new FieldViewModel() {
                                            caption = caption,
                                            inputtype = formField.inputtype,
                                            @required = formField.@required,
                                            name = formField.name,
                                            headline = formField.headline,
                                            fielddescription = formField.description,
                                            isCheckbox = false,
                                            isDefault = false,
                                            isTextArea = true,
                                            isRadio = false,
                                            isSelect = false,
                                            id = formField.id,
                                            optionList = optionList,
                                            fieldEditLink = fieldEditLink,
                                            fieldEditWrapper = isEditing ? "ccEditWrapper" : ""
                                        });
                                        break;
                                    }

                                default: {
                                        string caption = formField.caption;
                                        if (string.IsNullOrEmpty(caption)) {
                                            caption = formField.name;
                                        }
                                        formViewData.listOfFieldsClass.Add(new FieldViewModel() {
                                            caption = caption,
                                            inputtype = formField.inputtype,
                                            @required = formField.@required,
                                            name = formField.name,
                                            headline = formField.headline,
                                            fielddescription = formField.description,
                                            isCheckbox = false,
                                            isDefault = true,
                                            isTextArea = false,
                                            isRadio = false,
                                            isSelect = false,
                                            id = formField.id,
                                            optionList = optionList,
                                            fieldEditLink = fieldEditLink,
                                            fieldEditWrapper = isEditing ? "ccEditWrapper" : ""
                                        });
                                        break;
                                    }
                            }
                            fieldPtr += 1;
                        }
                        formViewData.fieldAddLink = cp.Content.GetAddLink(FormFieldModel.tableMetadata.contentName, "formid=" + form.id, false, isEditing);
                    }
                    formViewData.formAddLink = cp.Content.GetAddLink(FormModel.tableMetadata.contentName, "formsetid=" + settings.id, false, isEditing);
                }
                return formViewData;
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                return null;
            }
        }
    }
}