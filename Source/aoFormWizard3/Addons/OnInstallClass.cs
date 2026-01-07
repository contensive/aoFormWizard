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
                const int version = 5;
                int buildVersion = CP.Site.GetInteger("Form Wizard Version", 0);
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
                        if (CP.Db.IsTable("ccFormFields")) {
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
                }
                if (buildVersion < 5) {
                    try {
                        //
                        // -- convert ccformWidgets to ccformWidgets and ccForms 
                        // -- previously the widget had the form details but we want the user to be able to select the form
                        foreach (var formWidget in DbBaseModel.createList<FormWidgetModel>(CP)) {
                            if (formWidget.formId == 0) {
                                FormModel form = FormModel.createFormFromWizard(CP, formWidget);
                                formWidget.formId = form.id;
                                formWidget.save(CP);
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
                }
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