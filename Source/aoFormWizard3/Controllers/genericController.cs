using System;
using Contensive.BaseClasses;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Contensive.Addon.aoFormWizard3.Controllers {
    public sealed class GenericController {
        // 
        // ====================================================================================================
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourcedate"></param>
        /// <returns></returns>
        internal static string encodeBlankShortDate(DateTime sourcedate) {
            if (sourcedate >= DateTime.Parse("1900-01-01")) {
                return sourcedate.ToShortDateString();
            } else {
                return "";
            }
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// if date is invalid, set to minValue
        /// </summary>
        /// <param name="srcDate"></param>
        /// <returns></returns>
        public static DateTime encodeMinDate(DateTime srcDate) {
            var returnDate = srcDate;
            if (srcDate < new DateTime(1900, 1, 1)) {
                returnDate = DateTime.MinValue;
            }
            return returnDate;
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// if valid date, return the short date, else return blank string 
        /// </summary>
        /// <param name="srcDate"></param>
        /// <returns></returns>
        public static string getShortDateString(DateTime srcDate) {
            string returnString = "";
            var workingDate = encodeMinDate(srcDate);
            if (!isDateEmpty(srcDate)) {
                returnString = workingDate.ToShortDateString();
            }
            return returnString;
        }
        // 
        // ====================================================================================================
        public static bool isDateEmpty(DateTime srcDate) {
            return srcDate < new DateTime(1900, 1, 1);
        }
        // 
        // ====================================================================================================
        public static string getSortOrderFromInteger(int id) {
            return id.ToString().PadLeft(7, '0');
        }
        // 
        // ====================================================================================================
        public static string getDateForHtmlInput(DateTime source) {
            if (isDateEmpty(source)) {
                return "";
            } else {
                return (source.Year + Conversions.ToDouble("-") + Conversions.ToDouble(source.Month.ToString().PadLeft(2, '0')) + Conversions.ToDouble("-") + Conversions.ToDouble(source.Day.ToString().PadLeft(2, '0'))).ToString();
            }
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// convert string into a style "height: {styleHeight};", if value is numeric it adds "px"
        /// </summary>
        /// <param name="styleheight"></param>
        /// <returns></returns>
        public static string encodeStyleHeight(string styleheight) {
            return string.IsNullOrWhiteSpace(styleheight) ? string.Empty : "overflow:hidden;height:" + styleheight + (Information.IsNumeric(styleheight) ? "px" : string.Empty) + ";";
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// convert string into a style "background-image: url(backgroundImage)
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="backgroundImage"></param>
        /// <returns></returns>
        public static string encodeStyleBackgroundImage(CPBaseClass cp, string backgroundImage) {
            return string.IsNullOrWhiteSpace(backgroundImage) ? string.Empty : "background-image: url(" + cp.Http.CdnFilePathPrefixAbsolute + backgroundImage + ");";
        }
        // 
        // 
        public static string addEditWrapper(CPBaseClass cp, string innerHtml, int recordId, string recordName, string contentName) {
            if (!cp.User.IsEditing())
                return innerHtml;
            string header = cp.Content.GetEditLink(contentName, recordId.ToString(), false, recordName, true);
            string content = cp.Html.div(innerHtml, "", "");
            return cp.Html.div(header + content, "", "ccEditWrapper");
        }
        // 
        // 
        public static string getEditLink(CPBaseClass cp, string contentName, int recordId, string Caption) {
            int contentId = cp.Content.GetID(contentName);
            if (contentId == 0)
                return string.Empty;
            return "<a href=\"/admin?af=4&aa=2&ad=1&cid=" + contentId + "&id=" + recordId + "\" class=\"ccRecordEditLink\"><span style=\"color:#0c0\"><i title=\"edit\" class=\"fas fa-cog\"></i></span>&nbsp;" + Caption + "</a>";
        }
    }
}