using Contensive.BaseClasses;
using Contensive.FormWidget.Models.Db;
using Contensive.Models.Db;
using System;

namespace Contensive.FormWidget.Addons {
    // 
    public class OnInstallClass : AddonBaseClass {
        // 
        // =====================================================================================
        /// <summary>
        /// Executed on installation -- upgrade content.
        /// </summary>
        /// <param name="CP"></param>
        /// <returns></returns>
        public override object Execute(CPBaseClass CP) {
            try {
                //
                // -- version used to upgrade content
                const int version = 9;
                int buildVersion = CP.Site.GetInteger("Form Wizard Version", version);
                CP.Site.SetProperty("Form Wizard Version", version);
                //
                //if (buildVersion < 2) {
                //    // 
                //    // -- version - 2, convert form.useauthmembercontent, form.useauthorgcontent, form.authcontent -> form.saveContentId (null=no-save, 1=no-save, 2=people-save, 3=org-save, 4=custom-save), saveCustomContent
                //    foreach (FormModel form in DbBaseModel.createList<FormModel>(CP, "", "id")) {
                //        //form.saveCustomContent = Conversions.ToString(form.authcontent.Equals(0) ? "" : CP.Content.GetID(form.authcontent.ToString()));
                //        form.saveTypeId = form.useauthmembercontent ? 2 : form.useauthorgcontent ? 3 : !string.IsNullOrWhiteSpace(form.saveCustomContent) ? 4 : 1;
                //        form.save(CP);
                //    }
                //}
                if (buildVersion < 3) {
                    try {
                        //
                        // -- change question type from string to integer
                        if (CP.Db.IsTable("ccFormFields") && CP.Db.IsTableField("ccFormFields", "inputtypeid")) {
                            CP.Db.ExecuteNonQuery("update ccFormFields set inputtypeid=1 where inputtype='TEXT'");
                            CP.Db.ExecuteNonQuery("update ccFormFields set inputtypeid=2 where inputtype='TEXTAREA'");
                            CP.Db.ExecuteNonQuery("update ccFormFields set inputtypeid=3 where inputtype='CHECKBOX'");
                            CP.Db.ExecuteNonQuery("update ccFormFields set inputtypeid=4 where inputtype='RADIO'");
                            CP.Db.ExecuteNonQuery("update ccFormFields set inputtypeid=5 where inputtype='FILE'");
                            CP.Db.ExecuteNonQuery("update ccFormFields set inputtypeid=6 where inputtype='SELECT'");
                        }
                    } catch (Exception ex) {
                        CP.Log.Error(ex);
                    }
                    CP.Site.SetProperty("Form Wizard Version", 3);
                }
                if (buildVersion < 4) {
                    try {
                        //
                        // xml has updated table names so the new ones must be dropped
                        // before the old ones can be updated
                        string ccFormWidgetsCountSQL = "select count(id) as 'count' from cctables where name = 'ccFormWidgets'";
                        int ccFormWidgetsCount = 0;
                        using (var cs = CP.CSNew()) {
                            if (cs.OpenSQL(ccFormWidgetsCountSQL)) {
                                ccFormWidgetsCount = cs.GetInteger("count");
                            }
                        }
                        if (ccFormWidgetsCount > 0) {
                            CP.Db.ExecuteNonQuery(" IF EXISTS (select id from ccFormWidgets)" +
                                                    " begin " +
                                                    " return " +
                                                    " end" +
                                                    " else if not exists (select id from ccFormWidgets) " +
                                                    " begin " +
                                                    " DROP TABLE ccFormWidgets " +
                                                    " end");
                            CP.Db.ExecuteNonQuery("delete from cctables where name = 'ccFormWidgets'");
                        }

                        string ccFormPagesCountSQL = "select count(id) as 'count' from cctables where name = 'ccFormPages'";
                        int ccFormPagesCount = 0;
                        using (var cs = CP.CSNew()) {
                            if (cs.OpenSQL(ccFormPagesCountSQL)) {
                                ccFormPagesCount = cs.GetInteger("count");
                            }
                        }
                        if (ccFormPagesCount > 0) {
                            CP.Db.ExecuteNonQuery(" IF EXISTS (select id from ccFormPages)" +
                            " begin " +
                            " return" +
                            " end" +
                            " else if not exists (select id from ccFormPages) " +
                            " begin " +
                            " DROP TABLE ccFormPages " +
                            " end");

                            CP.Db.ExecuteNonQuery("delete from cctables where name = 'ccFormPages'");
                        }

                        string ccFormQuestionsCountSQL = "select count(id) as 'count' from cctables where name = 'ccFormQuestions'";
                        int ccFormQuestionsCount = 0;
                        using (var cs = CP.CSNew()) {
                            if (cs.OpenSQL(ccFormQuestionsCountSQL)) {
                                ccFormQuestionsCount = cs.GetInteger("count");
                            }
                        }
                        if (ccFormQuestionsCount > 0) {
                            CP.Db.ExecuteNonQuery(" IF EXISTS (select id from ccFormQuestions)" +
                            " begin " +
                            " return" +
                            " end" +
                            " else if not exists (select id from ccFormQuestions) " +
                            " begin " +
                            " DROP TABLE ccFormQuestions " +
                            " end");

                            CP.Db.ExecuteNonQuery("delete from cctables where name = 'ccFormQuestions'");
                        }

                        string ccFormResponseCountSQL = "select count(id) as 'count' from cctables where name = 'ccFormResponse'";
                        int ccFormResponseCount = 0;
                        using (var cs = CP.CSNew()) {
                            if (cs.OpenSQL(ccFormResponseCountSQL)) {
                                ccFormResponseCount = cs.GetInteger("count");
                            }
                        }
                        if (ccFormResponseCount > 0) {
                            CP.Db.ExecuteNonQuery(" IF EXISTS (select id from ccFormResponse)" +
                            " begin " +
                            " return" +
                            " end" +
                            " else if not exists (select id from ccFormResponse) " +
                            " begin " +
                            " DROP TABLE ccFormResponse " +
                            " end");

                            CP.Db.ExecuteNonQuery("delete from cctables where name = 'ccFormResponse'");
                        }
                        // -- update table names
                        string ccFormSetsCountSQL = "select count(id) as 'count' from cctables where name = 'ccFormSets'";
                        int ccFormsetsCount = 0;
                        using (var cs = CP.CSNew()) {
                            if (cs.OpenSQL(ccFormSetsCountSQL)) {
                                ccFormsetsCount = cs.GetInteger("count");
                            }
                        }
                        if (ccFormsetsCount > 0) {
                            CP.Db.ExecuteNonQuery("exec sp_rename 'ccFormSets', 'ccFormWidgets';");
                        }

                        string ccFormsCountSQL = "select count(id) as 'count' from cctables where name = 'ccForms'";
                        int ccFormsCount = 0;
                        using (var cs = CP.CSNew()) {
                            if (cs.OpenSQL(ccFormsCountSQL)) {
                                ccFormsCount = cs.GetInteger("count");
                            }
                        }
                        if (ccFormsCount > 0) {
                            CP.Db.ExecuteNonQuery("exec sp_rename 'ccForms', 'ccFormPages';");
                        }

                        string ccUserFormResponseCountSQL = "select count(id) as 'count' from cctables where name = 'ccUserFormResponse'";
                        int ccUserFormResponseCount = 0;
                        using (var cs = CP.CSNew()) {
                            if (cs.OpenSQL(ccUserFormResponseCountSQL)) {
                                ccUserFormResponseCount = cs.GetInteger("count");
                            }
                        }
                        if (ccUserFormResponseCount > 0) {
                            CP.Db.ExecuteNonQuery("exec sp_rename 'ccUserFormResponse', 'ccFormResponse';");
                        }

                        string ccFormFieldsCountSQL = "select count(id) as 'count' from cctables where name = 'ccFormFields'";
                        int ccFormFieldsCount = 0;
                        using (var cs = CP.CSNew()) {
                            if (cs.OpenSQL(ccFormFieldsCountSQL)) {
                                ccFormFieldsCount = cs.GetInteger("count");
                            }
                        }
                        if (ccFormFieldsCount > 0) {
                            CP.Db.ExecuteNonQuery("exec sp_rename 'ccFormFields', 'ccFormQuestions';");
                        }

                    } catch (Exception ex) {
                        CP.Log.Error(ex);
                    }
                    CP.Site.SetProperty("Form Wizard Version", 4);
                }
                if (buildVersion < 5) {
                    try {
                        //
                        // -- convert ccformWidgets to ccformWidgets and ccForms
                        // -- previously the widget had the form details but we want the user to be able to select the form
                        if (CP.Db.IsTableField("ccFormWidgets", "formId")) {
                            foreach (var formWidget in DbBaseModel.createList<FormWidgetModel>(CP)) {
                                if (formWidget.formId == 0) {
                                    FormModel form = FormModel.createFormFromWizard(CP, formWidget);
                                    formWidget.formId = form.id;
                                    formWidget.save(CP);
                                }
                            }
                        }
                    } catch (Exception ex) {
                        CP.Log.Error(ex);
                    }

                    //
                    // -- delete legacy fields
                    CP.Db.ExecuteNonQuery($@"delete from ccfields where contentid={CP.Content.GetID("form widgets")} and name in (
                        'addResetButton',
                        'backButtonName',
                        'continueButtonName',
                        'resetButtonName',
                        'submitButtonName',
                        'saveButtonName',
                        'joingroupid',
                        'notificationemailid',
                        'responseemailid',
                        'thankyoucopy',
                        'useUserProperty',
                        'allowRecaptcha')
                    ");

                    //using (DataTable dt = CP.Db.ExecuteQuery("select * from ccFormWidgets")) {
                    //    foreach (DataRow row in dt.Rows) {
                    //        int formId = CP.Utils.EncodeInteger(row["id"]);
                    //        if (formId == 0) {
                    //            var form = DbBaseModel.addDefault<FormModel>(CP);
                    //            form.addResetButton = CP.Utils.EncodeBoolean(row[""]);
                    //            form.backButtonName = CP.Utils.EncodeText(row[""]);
                    //            form.continueButtonName = CP.Utils.EncodeText(row[""]);
                    //            form.resetButtonName = CP.Utils.EncodeText(row["resetButtonName"]);
                    //            form.submitButtonName = CP.Utils.EncodeText(row["submitButtonName"]);
                    //            form.saveButtonName = CP.Utils.EncodeText(row["saveButtonName"]);
                    //            form.joingroupid = CP.Utils.EncodeInteger(row["joingroupid"]);
                    //            form.notificationemailid = CP.Utils.EncodeInteger(row["notificationemailid"]);
                    //            form.responseemailid = CP.Utils.EncodeInteger(row["responseemailid"]);
                    //            form.thankyoucopy = CP.Utils.EncodeText(row["thankyoucopy"]);
                    //            form.useUserProperty = CP.Utils.EncodeBoolean(row["useUserProperty"]);
                    //            form.allowRecaptcha = CP.Utils.EncodeBoolean(row["allowRecaptcha"]);
                    //            form.createdBy = CP.Utils.EncodeInteger(row["createdBy"]);
                    //            form.dateAdded = CP.Utils.EncodeDate(row["dateAdded"]);
                    //            form.modifiedDate = CP.Utils.EncodeDate(row["modifiedDate"]);
                    //            form.modifiedBy = CP.Utils.EncodeInteger(row["modifiedBy"]);
                    //        }
                    //        CP.Db.ExecuteNonQuery($"update ccFormWidgets set formId={formId} where id={CP.Utils.EncodeInteger(row["id"])}");
                    //    }
                    //}
                    CP.Site.SetProperty("Form Wizard Version", 5);
                }
                if (buildVersion < 6) {
                    //
                    // -- remove all contentcategoryId
                    CP.Db.ExecuteNonQuery($"delete from ccfields where name='contentcategoryid'");
                    CP.Site.SetProperty("Form Wizard Version", 6);
                }
                if (buildVersion < 8) {
                    //
                    // -- remove deprecated formsetid field from Form Pages content definition
                    // -- the column does not exist in ccFormPages (replaced by formid) and causes admin edit errors
                    CP.Db.ExecuteNonQuery($"delete from ccfields where contentid={CP.Content.GetID("Form Pages")} and name='formsetid'");
                    CP.Site.SetProperty("Form Wizard Version", 8);
                }
                if (buildVersion < 9) {
                    try {
                        //
                        // -- version 9: clean up duplicate records caused by GUID changes
                        // -- the XML installer runs before OnInstall. It matched records by ccguid:
                        // --   old GUIDs (patterned/sequential) were not found, so new records were inserted with new GUIDs
                        // --   the old records (old GUIDs) remain, creating duplicates
                        // -- delete the old records by their deprecated GUIDs to remove the duplicates
                        //
                        // -- deprecated addon GUIDs (ccAddons table)
                        string[] deprecatedAddonGuids = new[] {
                            "{B2C3D4E5-F6A7-4B8C-9D0E-1F2A3B4C5D6E}",  // Form Email Edit (now {10B3EDAA-7F17-47F3-8077-DECCFA3009E5})
                            "{D4E5F6A7-B8C9-4D0E-1F2A-3B4C5D6E7F80}",  // Form Button Edit (now {81C29237-0DAF-4327-9945-BC62ADA73F75})
                            "{F6A7B8C9-D0E1-4F2A-3B4C-5D6E7F809112}",  // Form Submit Edit (now {BEB3212E-6235-4ACA-8076-D503B6BFDF2A})
                            "{B8C9D0E1-F2A3-4B4C-5D6E-7F8091122334}",  // Form Feature Edit (now {974B7A89-1A09-4896-9A93-DA10CB271DF8})
                            "{A1B2C3D4-E5F6-4A7B-8C9D-0E1F2A3B4C5D}",  // SaveFormPageSortable (now {2BEA68F9-8111-49C0-88AD-8B5634AF0AA3})
                            "{D4C3B2A1-6F5E-7B4A-9D8C-1F0E3A2B5C4D}"   // SaveFormQuestionSortable (now {FBEFF46F-32B3-4219-A4A9-88E7E905FAAB})
                        };
                        foreach (string oldGuid in deprecatedAddonGuids) {
                            CP.Db.ExecuteNonQuery($"delete from ccAddons where ccguid={CP.Db.EncodeSQLText(oldGuid)}");
                        }
                        //
                        // -- deprecated portal feature GUIDs (ccAggrFunction table)
                        string[] deprecatedFeatureGuids = new[] {
                            "{A1B2C3D4-E5F6-4A7B-8C9D-0E1F2A3B4C5D}",  // Form-Emails (now {67751042-5E19-4275-B6FA-6E9C90901DB1})
                            "{C3D4E5F6-A7B8-4C9D-0E1F-2A3B4C5D6E7F}",  // Form-Buttons (now {A281B800-AE0C-4E0B-9BFB-3CE263ED2114})
                            "{E5F6A7B8-C9D0-4E1F-2A3B-4C5D6E7F8091}",  // Form-Submit (now {0F89C402-028F-4EBA-B000-EE04F3E54253})
                            "{A7B8C9D0-E1F2-4A3B-4C5D-6E7F80911223}"   // Form-Features (now {621CFCBF-00B5-41EC-AF77-503CB24AB094})
                        };
                        foreach (string oldGuid in deprecatedFeatureGuids) {
                            CP.Db.ExecuteNonQuery($"delete from ccAggrFunction where ccguid={CP.Db.EncodeSQLText(oldGuid)}");
                        }
                    } catch (Exception ex) {
                        CP.Log.Error(ex);
                    }
                    CP.Site.SetProperty("Form Wizard Version", 9);
                }
                CP.Site.SetProperty("Form Wizard Version", version);
                //
                // -- update layout
                CP.Layout.updateLayout(Constants.guidLayoutFormWizard, Constants.nameLayoutFormWizard, Constants.pathFilenameLayoutFormWizard);
                CP.Layout.updateLayout(Constants.guidLayoutApplicationScore, Constants.nameLayoutApplicationScore, Constants.pathFilenameLayoutApplicationScore);
                //
                return string.Empty;
            } catch (Exception ex) {
                CP.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}