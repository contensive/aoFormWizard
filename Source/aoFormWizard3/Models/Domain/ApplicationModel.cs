using Contensive.BaseClasses;
using System;

namespace Contensive.Addon.aoFormWizard3.Models.Domain {
    // 
    public class ApplicationModel : IDisposable {
        // 
        // -- instance properties
        public CPBaseClass cp;
        // 
        // ====================================================================================================
        /// <summary>
        /// Create an empty object. needed for deserialization
        /// </summary>
        public ApplicationModel( CPBaseClass cp ) {
            this.cp = cp;
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// when additional attendees are added, if they are new entries, add them to 0=nothing, 1=accounts or 2=organizations.
        /// </summary>
        /// <returns></returns>
        public int additionalAttendeeAddTo {
            get {
                if (_additionalAttendeeAddTo is null) {
                    _additionalAttendeeAddTo = cp.Site.GetInteger("MeetingSmart Additional Attendee Add to", 0);
                }
                return (int)_additionalAttendeeAddTo;
            }
            set {
                _additionalAttendeeAddTo = value;
            }
        }
        private int? _additionalAttendeeAddTo = default;
        // 
        // ====================================================================================================
        /// <summary>
        /// When additional attendees are added, the registrant can select from a list of 0=nothing, 1=accounts or 2=organizations.
        /// </summary>
        /// <returns></returns>
        public int additionalAttendeeSelectionMode {
            get {
                if (_additionalAttendeeSelectionMode is null) {
                    _additionalAttendeeSelectionMode = cp.Site.GetInteger("MeetingSmart Additional Attendee Selection Mode", 0);
                }
                return (int)_additionalAttendeeSelectionMode;
            }
            set {
                _additionalAttendeeSelectionMode = value;
            }
        }
        private int? _additionalAttendeeSelectionMode = default;
        // 
        // ====================================================================================================
        // 
        public string freeCaption {
            get {
                if (_freecaption is null) {
                    _freecaption = cp.Site.GetText("MeetingSmart Free Caption", "Free");
                }
                return _freecaption;
            }
            set {
                _freecaption = value;
            }
        }
        private string _freecaption = null;
        // 
        // ====================================================================================================
        /// <summary>
        /// Symbol for currency
        /// </summary>
        /// <returns></returns>
        public string monetarySymbol {
            get {
                if (_monetarySymbol is null) {
                    _monetarySymbol = cp.Site.GetText("Meeting Manager Monetary Symbol", "$");
                }
                return _monetarySymbol;
            }
            set {
                // 
                // does not set site property, just mocks the site property for testing
                // 
                _monetarySymbol = value;
            }
        }
        private string _monetarySymbol = null;
        // 
        // ====================================================================================================
        /// <summary>
        /// true if a cancellation email should be sent to the each attendee and the registrant
        /// </summary>
        /// <returns></returns>
        public bool allowCancellationEmail {
            get {
                if (_allowCancellationEmail is null) {
                    _allowCancellationEmail = cp.Site.GetBoolean("Meeting Manager Allow Send Cancellation Email", false);
                }
                return (bool)_allowCancellationEmail;
            }
            set {
                // 
                // does not set site property, just mocks the site property for testing
                // 
                _allowCancellationEmail = value;
            }
        }
        private bool? _allowCancellationEmail = default;
        // 
        // ====================================================================================================
        /// <summary>
        /// Symbol for currency
        /// </summary>
        /// <returns></returns>
        public string AdminUrl {
            get {
                if (_AdminUrl is null) {
                    _AdminUrl = cp.Site.GetText("AdminUrl");
                }
                return _AdminUrl;
            }
            set {
                // 
                // does not set site property, just mocks the site property for testing
                // 
                _AdminUrl = value;
            }
        }
        private string _AdminUrl = null;
        // 
        // ====================================================================================================
        /// <summary>
        /// if true, the system will login by email address as well as username 
        /// </summary>
        /// <returns></returns>
        public bool allowEmailLogin {
            get {
                if (_allowEmailLogin is null) {
                    _allowEmailLogin = cp.Site.GetBoolean("Meeting Manager Allow Send Cancellation Email", false);
                }
                return (bool)_allowEmailLogin;
            }
            set {
                // 
                // does not set site property, just mocks the site property for testing
                // 
                _allowEmailLogin = value;
            }
        }
        private bool? _allowEmailLogin = default;
        // 
        // ====================================================================================================
        /// <summary>
        /// if true, the system will login by email address as well as username 
        /// </summary>
        /// <returns></returns>
        public bool allowNonSecureEcommerce {
            get {
                if (_allowNonSecureEcommerce is null) {
                    _allowNonSecureEcommerce = cp.Site.GetBoolean("ACCOUNT BILLING ALLOW NON-SECURE ECOMMERCE");
                }
                return (bool)_allowNonSecureEcommerce;
            }
            set {
                // 
                // does not set site property, just mocks the site property for testing
                // 
                _allowNonSecureEcommerce = value;
            }
        }
        private bool? _allowNonSecureEcommerce = default;
        // 
        // ====================================================================================================
        /// <summary>
        /// if true, the system will login by email address as well as username 
        /// </summary>
        /// <returns></returns>
        public bool allowDuplicateEmails {
            get {
                if (_allowDuplicateEmails is null) {
                    _allowDuplicateEmails = !cp.Site.GetBoolean("Meeting Force Guest Unique Email");
                }
                return (bool)_allowDuplicateEmails;
            }
            set {
                // 
                // does not set site property, just mocks the site property for testing
                // 
                _allowDuplicateEmails = value;
            }
        }
        private bool? _allowDuplicateEmails = default;
        #region  IDisposable Support 
        protected bool disposed = false;
        // 
        // ==========================================================================================
        /// <summary>
        /// dispose
        /// </summary>
        /// <param name="disposing"></param>
        /// <remarks></remarks>
        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {
                    // 
                    // ----- call .dispose for managed objects
                    // 
                }
                // 
                // Add code here to release the unmanaged resource.
                // 
            }
            disposed = true;
        }
        // Do not change or add Overridable to these methods.
        // Put cleanup code in Dispose(ByVal disposing As Boolean).
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~ApplicationModel() {
            Dispose(false);
        }
#endregion
    }
}