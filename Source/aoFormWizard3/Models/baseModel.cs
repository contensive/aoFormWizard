
using System;
using System.Collections.Generic;
using System.Reflection;
using Contensive.BaseClasses;

namespace Contensive.FormWidget.Contensive.Addon.AddonCollectionVb.Controllers {
    public abstract class baseModel {
        // 
        // ====================================================================================================
        // -- const must be set in derived clases
        // 
        // Public Const contentName As String = "" '<------ set content name
        // Public Const contentTableName As String = "" '<------ set to tablename for the primary content (used for cache names)
        // Public Const contentDataSource As String = "" '<----- set to datasource if not default
        // 
        // ====================================================================================================
        // -- instance properties
        public int id { get; set; }
        public string name { get; set; }
        public string ccguid { get; set; }
        public bool Active { get; set; }
        public int ContentControlID { get; set; }
        public int CreatedBy { get; set; }
        public int CreateKey { get; set; }
        public DateTime DateAdded { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string SortOrder { get; set; }
        // 
        // ====================================================================================================
        private static string derivedContentName(Type derivedType) {
            var fieldInfo = derivedType.GetField("contentName");
            if (fieldInfo is null) {
                throw new ApplicationException("Class [" + derivedType.Name + "] must declare constant [contentName].");
            } else {
                return fieldInfo.GetRawConstantValue().ToString();
            }
        }
        // 
        // ====================================================================================================
        private static string derivedContentTableName(Type derivedType) {
            var fieldInfo = derivedType.GetField("contentTableName");
            if (fieldInfo is null) {
                throw new ApplicationException("Class [" + derivedType.Name + "] must declare constant [contentTableName].");
            } else {
                return fieldInfo.GetRawConstantValue().ToString();
            }
        }
        // 
        // ====================================================================================================
        private static string contentDataSource(Type derivedType) {
            var fieldInfo = derivedType.GetField("contentTableName");
            if (fieldInfo is null) {
                throw new ApplicationException("Class [" + derivedType.Name + "] must declare constant [contentTableName].");
            } else {
                return fieldInfo.GetRawConstantValue().ToString();
            }
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// Create an empty object. needed for deserialization
        /// </summary>
        public baseModel() {
            // 
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// Add a new recod to the db and open it. Starting a new model with this method will use the default values in Contensive metadata (active, contentcontrolid, etc).
        /// include callersCacheNameList to get a list of cacheNames used to assemble this response
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        protected static T @add<T>(CPBaseClass cp) where T : baseModel {
            T result = null;
            try {
                var instanceType = typeof(T);
                string contentName = derivedContentName(instanceType);
                result = create<T>(cp, cp.Content.AddRecord(contentName));
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
            return result;
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// return a new model with the data selected. All cacheNames related to the object will be added to the cacheNameList.
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="recordId">The id of the record to be read into the new object</param>
        protected static T create<T>(CPBaseClass cp, int recordId) where T : baseModel {
            T result = null;
            try {
                if (recordId > 0) {
                    var instanceType = typeof(T);
                    string contentName = derivedContentName(instanceType);
                    var cs = cp.CSNew();
                    if (cs.Open(contentName, "(id=" + recordId.ToString() + ")")) {
                        result = loadRecord<T>(cp, cs);
                    }
                    cs.Close();
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
            return result;
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// open an existing object
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="recordGuid"></param>
        protected static T create<T>(CPBaseClass cp, string recordGuid) where T : baseModel {
            T result = null;
            try {
                var instanceType = typeof(T);
                string contentName = derivedContentName(instanceType);
                var cs = cp.CSNew();
                if (cs.Open(contentName, "(ccGuid=" + cp.Db.EncodeSQLText(recordGuid) + ")")) {
                    result = loadRecord<T>(cp, cs);
                }
                cs.Close();
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
            return result;
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// open an existing object
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="recordName"></param>
        protected static T createByName<T>(CPBaseClass cp, string recordName) where T : baseModel {
            T result = null;
            try {
                if (!string.IsNullOrEmpty(recordName)) {
                    var instanceType = typeof(T);
                    string contentName = derivedContentName(instanceType);
                    var cs = cp.CSNew();
                    if (cs.Open(contentName, "(name=" + cp.Db.EncodeSQLText(recordName) + ")", "id")) {
                        result = loadRecord<T>(cp, cs);
                    }
                    cs.Close();
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
            return result;
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// open an existing object
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="cs"></param>
        private static T loadRecord<T>(CPBaseClass cp, CPCSBaseClass cs) where T : baseModel {
            T instance = null;
            try {
                if (cs.OK()) {
                    var instanceType = typeof(T);
                    string tableName = derivedContentTableName(instanceType);
                    instance = (T)Activator.CreateInstance(instanceType);
                    foreach (PropertyInfo resultProperty in instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)) {
                        switch (resultProperty.Name.ToLower() ?? "") {
                            case "specialcasefield": {
                                    break;
                                }
                            case "sortorder": {
                                    // 
                                    // -- customization for pc, could have been in default property, db default, etc.
                                    string sortOrder = cs.GetText(resultProperty.Name);
                                    if (string.IsNullOrEmpty(sortOrder)) {
                                        sortOrder = "9999";
                                    }
                                    resultProperty.SetValue(instance, sortOrder, null);
                                    break;
                                }

                            default: {
                                    switch (resultProperty.PropertyType.Name ?? "") {
                                        case "Int32": {
                                                resultProperty.SetValue(instance, cs.GetInteger(resultProperty.Name), null);
                                                break;
                                            }
                                        case "Boolean": {
                                                resultProperty.SetValue(instance, cs.GetBoolean(resultProperty.Name), null);
                                                break;
                                            }
                                        case "DateTime": {
                                                resultProperty.SetValue(instance, cs.GetDate(resultProperty.Name), null);
                                                break;
                                            }
                                        case "Double": {
                                                resultProperty.SetValue(instance, cs.GetNumber(resultProperty.Name), null);
                                                break;
                                            }

                                        default: {
                                                resultProperty.SetValue(instance, cs.GetText(resultProperty.Name), null);
                                                break;
                                            }
                                    }

                                    break;
                                }
                        }
                    }
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
            return instance;
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// save the instance properties to a record with matching id. If id is not provided, a new record is created.
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        protected int save(CPBaseClass cp) {
            try {
                var cs = cp.CSNew();
                var instanceType = GetType();
                string contentName = derivedContentName(instanceType);
                string tableName = derivedContentTableName(instanceType);
                if (id > 0) {
                    if (!cs.Open(contentName, "id=" + id)) {
                        string message = "Unable to open record in content [" + contentName + "], with id [" + id + "]";
                        cs.Close();
                        id = 0;
                        throw new ApplicationException(message);
                    }
                } else if (!cs.Insert(contentName)) {
                    cs.Close();
                    id = 0;
                    throw new ApplicationException("Unable to insert record in content [" + contentName + "]");
                }
                foreach (PropertyInfo resultProperty in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)) {
                    switch (resultProperty.Name.ToLower() ?? "") {
                        case "id": {
                                id = cs.GetInteger("id");
                                break;
                            }
                        case "ccguid": {
                                if (string.IsNullOrEmpty(ccguid)) {
                                    ccguid = Guid.NewGuid().ToString();
                                }
                                string value;
                                value = resultProperty.GetValue(this, null).ToString();
                                cs.SetField(resultProperty.Name, value);
                                break;
                            }

                        default: {
                                switch (resultProperty.PropertyType.Name ?? "") {
                                    case "Int32": {
                                            int value;
                                            int.TryParse(resultProperty.GetValue(this, null).ToString(), out value);
                                            cs.SetField(resultProperty.Name, value.ToString());
                                            break;
                                        }
                                    case "Boolean": {
                                            bool value;
                                            bool.TryParse(resultProperty.GetValue(this, null).ToString(), out value);
                                            cs.SetField(resultProperty.Name, value.ToString());
                                            break;
                                        }
                                    case "DateTime": {
                                            DateTime value;
                                            DateTime.TryParse(resultProperty.GetValue(this, null).ToString(), out value);
                                            cs.SetField(resultProperty.Name, value.ToString());
                                            break;
                                        }
                                    case "Double": {
                                            double value;
                                            double.TryParse(resultProperty.GetValue(this, null).ToString(), out value);
                                            cs.SetField(resultProperty.Name, value.ToString());
                                            break;
                                        }

                                    default: {
                                            string value;
                                            value = resultProperty.GetValue(this, null).ToString();
                                            cs.SetField(resultProperty.Name, value);
                                            break;
                                        }
                                }

                                break;
                            }
                    }
                }
                cs.Close();
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
            return id;
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// delete an existing database record by id
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="recordId"></param>
        protected static void delete<T>(CPBaseClass cp, int recordId) where T : baseModel {
            try {
                if (recordId > 0) {
                    var instanceType = typeof(T);
                    string contentName = derivedContentName(instanceType);
                    string tableName = derivedContentTableName(instanceType);
                    cp.Content.Delete(contentName, "id=" + recordId.ToString());
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// delete an existing database record by guid
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="ccguid"></param>
        protected static void delete<T>(CPBaseClass cp, string ccguid) where T : baseModel {
            try {
                if (!string.IsNullOrEmpty(ccguid)) {
                    var instanceType = typeof(T);
                    string contentName = derivedContentName(instanceType);
                    var instance = create<baseModel>(cp, ccguid);
                    if (instance is not null) {
                        cp.Content.Delete(contentName, "(ccguid=" + cp.Db.EncodeSQLText(ccguid) + ")");
                    }
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// pattern get a list of objects from this model
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="sqlCriteria"></param>
        /// <param name="sqlOrderBy"></param>
        /// <returns></returns>
        protected static List<T> createList<T>(CPBaseClass cp, string sqlCriteria, string sqlOrderBy) where T : baseModel {
            var result = new List<T>();
            try {
                var cs = cp.CSNew();
                var ignoreCacheNames = new List<string>();
                var instanceType = typeof(T);
                string contentName = derivedContentName(instanceType);
                if (cs.Open(contentName, sqlCriteria, sqlOrderBy)) {
                    T instance;
                    do {
                        instance = loadRecord<T>(cp, cs);
                        if (instance is not null) {
                            result.Add(instance);
                        }
                        cs.GoNext();
                    }
                    while (cs.OK());
                }
                cs.Close();
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
            return result;
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// get the name of the record by it's id
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="recordId"></param>record
        /// <returns></returns>
        protected static string getRecordName<T>(CPBaseClass cp, int recordId) where T : baseModel {
            try {
                if (recordId > 0) {
                    var instanceType = typeof(T);
                    string tableName = derivedContentTableName(instanceType);
                    var cs = cp.CSNew();
                    if (cs.OpenSQL("select name from " + tableName + " where id=" + recordId.ToString())) {
                        return cs.GetText("name");
                    }
                    cs.Close();
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
            return "";
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// get the name of the record by it's guid 
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="ccGuid"></param>record
        /// <returns></returns>
        protected static string getRecordName<T>(CPBaseClass cp, string ccGuid) where T : baseModel {
            try {
                if (!string.IsNullOrEmpty(ccGuid)) {
                    var instanceType = typeof(T);
                    string tableName = derivedContentTableName(instanceType);
                    var cs = cp.CSNew();
                    if (cs.OpenSQL("select name from " + tableName + " where ccguid=" + cp.Db.EncodeSQLText(ccGuid))) {
                        return cs.GetText("name");
                    }
                    cs.Close();
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
            return "";
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// get the id of the record by it's guid 
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="ccGuid"></param>record
        /// <returns></returns>
        protected static int getRecordId<T>(CPBaseClass cp, string ccGuid) where T : baseModel {
            try {
                if (!string.IsNullOrEmpty(ccGuid)) {
                    var instanceType = typeof(T);
                    string tableName = derivedContentTableName(instanceType);
                    var cs = cp.CSNew();
                    if (cs.OpenSQL("select id from " + tableName + " where ccguid=" + cp.Db.EncodeSQLText(ccGuid))) {
                        return cs.GetInteger("id");
                    }
                    cs.Close();
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
            return 0;
        }
    }
}