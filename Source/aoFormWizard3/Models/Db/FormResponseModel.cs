using Contensive.Models.Db;
using System;
using System.Collections.Generic;

namespace Contensive.Addon.aoFormWizard3.Models.Db {
    public class FormResponseModel : DbBaseModel {
        //
        public static DbBaseTableMetadataModel tableMetadata { get; private set; } = new DbBaseTableMetadataModel("Form Response", "ccFormResponse", "default", false);
        // 
        // ====================================================================================================
        // -- instance properties
        //
        /// <summary>
        /// a human readable version of the last formResponseData.formData entered
        /// </summary>
        public string copy { get; set; }
        /// <summary>
        /// Json encoded object with the response from the complete submission.
        /// A user's form is prepopulated from the last response that is not completed, and it saved as completed when the form is submitted.
        /// see FormResponseDataModel
        /// </summary>
        public string formResponseData { get; set; }
        public int visitid { get; set; }
        public int memberId { get; set; }
        /// <summary>
        /// should be formWidgetId
        /// </summary>
        public int formWidget { get; set; }
        public DateTime dateSubmitted { get; set; }
    }
    public class FormResponseDataModel {
        /// <summary>
        /// a list of the page submissions for this response
        /// each line should include
        /// - date and time of the submission
        /// - login details if the user changes
        /// - what form was submitted
        /// - what for was requested
        /// </summary>
        public List<FormResponseDataActivityModel> activity { get; set; }
        /// <summary>
        /// a dictionary of the form pages and the response for each page.
        /// key is the form id
        /// </summary>
        public Dictionary<int, FormResponseDataPageModel> pageDict { get; set; }
        //
        /// <summary>
        /// the id of the form-page the current user is working on. 
        /// Set when they hit continue.
        /// </summary>
        public int currentPageid { get; set; }
        //
        /// <summary>
        /// if true, the user has completed the form and submitted it. Display the form's thank you message
        /// </summary>
        public bool isComplete { get; set; }
        /// <summary>
        /// if true, the user has successfully completed the recaptcha
        /// </summary>
        public bool recaptchaSuccess { get; set; }
    }
    //
    /// <summary>
    /// The response for a single page of the form.
    /// key is the formField id
    /// </summary>
    public class FormResponseDataPageModel {
        /// <summary>
        /// true if this page was submnitted by the user. This can be ussed like "intentially left blank" when displaying answers.
        /// </summary>
        public bool isStarted { get; set; }
        /// <summary>
        /// set true when the user completes this page. The final submit cannot be completed until all pages are completed.
        /// when entering the form, go to the first non-complete page.
        /// </summary>
        public bool isCompleted { get; set; }
        public Dictionary<int, FormResponseDataPageQuestionModel> questionDict { get; set; }
    }
    //
    /// <summary>
    /// The response for a question on a single page of the form
    /// </summary>
    public class FormResponseDataPageQuestionModel {
        public string question { get; set; }
        /// <summary>
        /// the answer to text questions
        /// </summary>
        public string textAnswer { get; set; }
        //
        /// <summary>
        /// a list of the choices and true or false if it was selected. For radio and checkboxes
        /// the key is the choice in the question
        /// </summary>
        public Dictionary<string,bool> choiceAnswerDict { get; set; }
        //
        /// <summary>
        /// if a form is started, any answer marked invalid answer should display with the invalid answer messaage
        /// </summary>
        public bool invalidAnswer { get; set; }
    }
    public class FormResponseDataActivityModel {
        public string activityDate { get; set; }
        public int userId { get; set; }
        public string userName { get; set; }
        public string details { get; set; } 

    }
}