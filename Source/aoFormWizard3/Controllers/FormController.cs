//using Contensive.Addon.aoFormWizard3.Models.Domain;
//using Contensive.BaseClasses;
//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.CompilerServices;
//using System;
//using System.Collections.Generic;

//namespace Contensive.Addon.aoFormWizard3.Controllers {
//    public sealed class FormController {
//        // 
//        internal static string getSubHead(string SubHead) {
//            string result = "";            // 
//            result += "<tr><td colspan=\"4\"><IMG src=\"/ccLib/images/spacer.gif\" height=\"5\" width=\"1\"></td></tr>";
//            result += "<tr><td align=\"Left\" class=\"PageSubTitle\" colspan=\"4\"><b>" + SubHead + "</b></td></tr>";
//            result += "<tr><td colspan=\"4\" valign=\"center\"><IMG src=\"/ccLib/images/808080.gif\" height=\"1\" width=\"100%\"></td></tr>";
//            return result;
//        }
//        // 
//        internal static string get2ColumnRow(string HeaderLeft, int ValueLeft) 
//            => get2ColumnRow(HeaderLeft, ValueLeft.ToString());
//        // 
//        internal static string get2ColumnRow(string HeaderLeft, string ValueLeft) {
//            string result = "";
//            result += "<tr>";
//            result += "  <td valign=\"top\" align=\"right\">" + HeaderLeft + "</td>";
//            result += "  <td valign=\"top\" align=\"left\" colspan=\"3\">" + ValueLeft + "</td>";
//            result += "</tr>";
//            return result;
//        }
//        //
//        internal static string get4ColumnRow(string HeaderLeft, int ValueLeft, string HeaderRight, string ValueRight)
//            => get4ColumnRow(HeaderLeft, ValueLeft.ToString(), HeaderRight, ValueRight);
//        //
//        internal static string get4ColumnRow(string HeaderLeft, int ValueLeft, string HeaderRight, int ValueRight)
//            => get4ColumnRow(HeaderLeft, ValueLeft.ToString(), HeaderRight, ValueRight.ToString());
//        // 
//        internal static string get4ColumnRow(string HeaderLeft, string ValueLeft, string HeaderRight, string ValueRight) {
//            string result = "";
//            result += "<tr>";
//            result += "  <td valign=\"top\" align=\"right\" class=\"msDetailCaption\">" + HeaderLeft + "</td>";
//            result += "  <td valign=\"top\" align=\"left\" class=\"msDetailValue\">" + ValueLeft + "</td>";
//            result += "  <td valign=\"top\" align=\"right\" align=\"right\" class=\"msDetailCaption\">" + HeaderRight + "</td>";
//            result += "  <td valign=\"top\" align=\"left\" class=\"msDetailValue\">" + ValueRight + "</td>";
//            result += "  </tr>";
//            return result;
//        }
//        // 
//        // ====================================================================================================
//        // 
//        public static string getFormRow_subHead(CPBaseClass cp, string SubHead, string instructions) {
//            string result = "";
//            result += string.IsNullOrWhiteSpace(SubHead) ? "" : cp.Html5.H4(SubHead, "PageSubTitle");
//            result += string.IsNullOrWhiteSpace(instructions) ? "" : cp.Html5.P(instructions, "p-0 m-0 my-1 instructions font-italic");
//            result = string.IsNullOrWhiteSpace(result) ? "" : cp.Html5.Div(result, "p-0 m-0 mt-1 mb-3");
//            return string.IsNullOrWhiteSpace(result) ? "" : result;
//        }
//        // 
//        // ====================================================================================================
//        // 
//        public static string getFormRow_subHead(CPBaseClass cp, string SubHead) {
//            return getFormRow_subHead(cp, SubHead, "");
//        }
//        // 
//        // ====================================================================================================
//        /// <summary>
//        /// checkbox
//        /// </summary>
//        /// <param name="cp"></param>
//        /// <param name="labelHtml"></param>
//        /// <param name="inputHtml"></param>
//        /// <param name="helpHtml"></param>
//        /// <returns></returns>
//        public static string getFormRow(CPBaseClass cp, string labelHtml, string inputHtml, string helpHtml) {
//            return cp.AdminUI.GetEditRow(labelHtml, inputHtml, helpHtml);
//        }
//        // 
//        // ====================================================================================================
//        /// <summary>
//        /// edit form row that has a link not an input
//        /// </summary>
//        /// <param name="cp"></param>
//        /// <param name="label"></param>
//        /// <param name="text"></param>
//        /// <param name="instruction"></param>
//        /// <returns></returns>
//        public static string getFormRow_text(CPBaseClass cp, string label, string text, string instruction) {
//            return cp.AdminUI.GetEditRow(label, text, instruction);
//        }
//        // 
//        // ====================================================================================================
//        // 
//        public static string getFormRow_text(CPBaseClass cp, string label, string text) {
//            return cp.AdminUI.GetEditRow(label, text);
//        }
//        // 
//        // ====================================================================================================
//        // 
//        public static string getFormRow_editSelect(CPBaseClass cp, string label, string selectHtml, string selectedValue, string selectDetailHtml, string instruction) {
//            return cp.AdminUI.GetEditRow(label, selectHtml, instruction);
//        }
//        // 
//        // ====================================================================================================
//        // 
//        public static string getFormRow_editSelect(CPBaseClass cp, string label, string selectHtml, int selectedValue, string selectDetailHtml, string instruction) {
//            return cp.AdminUI.GetEditRow(label, selectHtml, instruction);
//        }
//        // 
//        // ====================================================================================================
//        /// <summary>
//        /// checkbox
//        /// </summary>
//        /// <param name="cp"></param>
//        /// <param name="label"></param>
//        /// <param name="htmlName"></param>
//        /// <param name="htmlValue"></param>
//        /// <param name="instruction"></param>
//        /// <returns></returns>
//        public static string getFormRow_editCheckbox(CPBaseClass cp, string label, string htmlName, bool htmlValue, string instruction) {
//            return cp.AdminUI.GetEditRow(label, cp.AdminUI.GetBooleanEditor(htmlName, htmlValue), instruction);
//        }
//        // 
//        // ====================================================================================================
//        // 
//        public static string getFormRow_editCheckbox(CPBaseClass cp, string label, string htmlName, bool htmlValue) {
//            return getFormRow_editCheckbox(cp, label, htmlName, htmlValue, label);
//        }
//        // 
//        // ====================================================================================================
//        /// <summary>
//        /// text, textarea, date
//        /// </summary>
//        /// <param name="cp"></param>
//        /// <param name="label"></param>
//        /// <param name="htmlName"></param>
//        /// <param name="htmlValue"></param>
//        /// <param name="instruction"></param>
//        /// <returns></returns>
//        public static string getFormRow_editDate(CPBaseClass cp, string label, string htmlName, DateTime htmlValue, string instruction) {
//            return cp.AdminUI.GetEditRow(label, cp.AdminUI.GetDateTimeEditor(htmlName, htmlValue), instruction);
//        }
//        // 
//        // ====================================================================================================
//        // 
//        public static string getFormRow_editDate(CPBaseClass cp, string label, string htmlName, DateTime htmlValue) {
//            return getFormRow_editDate(cp, label, htmlName, htmlValue, "");
//        }
//        // 
//        // ====================================================================================================
//        /// <summary>
//        /// text, textarea, date
//        /// </summary>
//        /// <param name="cp"></param>
//        /// <param name="label"></param>
//        /// <param name="htmlName"></param>
//        /// <param name="htmlValue"></param>
//        /// <param name="instruction"></param>
//        /// <returns></returns>
//        public static string getFormRow_editText(CPBaseClass cp, string label, string htmlName, string htmlValue, string instruction) {
//            return cp.AdminUI.GetEditRow(label, cp.AdminUI.GetTextEditor(htmlName, htmlValue), instruction);
//        }
//        // 
//        // ====================================================================================================
//        // 
//        public static string getFormRow_editText(CPBaseClass cp, string label, string htmlName, string htmlValue) {
//            return getFormRow_editText(cp, label, htmlName, htmlValue, "");
//        }
//        // 
//        // ====================================================================================================
//        // 
//        public static string getFormRow_editNumber(CPBaseClass cp, string label, string htmlName, double htmlValue, string instruction) {
//            return cp.AdminUI.GetEditRow(label, cp.AdminUI.GetNumberEditor(htmlName, htmlValue), instruction);
//        }
//        // 
//        // ====================================================================================================
//        /// <summary>
//        /// textarea
//        /// </summary>
//        /// <param name="cp"></param>
//        /// <param name="label"></param>
//        /// <param name="htmlName"></param>
//        /// <param name="htmlValue"></param>
//        /// <param name="instruction"></param>
//        /// <returns></returns>
//        public static string getFormRow_editTextarea(CPBaseClass cp, string label, string htmlName, string htmlValue, string instruction) {
//            return cp.AdminUI.GetEditRow(label, cp.AdminUI.GetLongTextEditor(htmlName, htmlValue), instruction);
//        }
//        // 
//        // ====================================================================================================
//        // 
//        public static string getFormRow_editTextarea(CPBaseClass cp, string label, string htmlName, string htmlValue) {
//            return getFormRow_editTextarea(cp, label, htmlName, htmlValue, "");
//        }
//        // 
//        // ====================================================================================================
//        // 
//        public static string GetFormMeetingEditRow_rightLeft(string Cell0, string Cell1, string instruction = "") {
//            string returnHtml = "";
//            returnHtml += "<tr><td align=\"right\" class=\"msEditCaption\">" + Cell0 + "</td><td width=\"100%\" align=\"Left\" class=\"msEditValue\">" + Cell1 + "</td></tr>";
//            if (!string.IsNullOrEmpty(instruction)) {
//                returnHtml += "<tr><td>&nbsp;</td><td><div class=\"instructions\">" + instruction + "</div></td></tr>";
//            }
//            returnHtml += "</tr>";
//            return returnHtml;
//        }
//        // 
//        public const string cr = Microsoft.VisualBasic.Constants.vbCrLf + Microsoft.VisualBasic.Constants.vbTab;
//        public const string cr2 = Microsoft.VisualBasic.Constants.vbCrLf + Microsoft.VisualBasic.Constants.vbTab + Microsoft.VisualBasic.Constants.vbTab;
//        // 
//        // ====================================================================================================
//        /// <summary>
//        /// Create a single cell row in a two row table
//        /// </summary>
//        /// <param name="Innards"></param>
//        /// <param name="RowClass"></param>
//        /// <returns></returns>
//        internal static string Get2ColumnSingleRow(string Innards, string RowClass = "") {
//            return "<tr><td class=\"" + RowClass + "\" colspan=\"2\" width=\"100%\">" + Innards + "</td></tr>";
//        }
//        // 
//        // ====================================================================================================
//        /// <summary>
//        /// Create a single cell row in a two row table with a caption and input
//        /// </summary>
//        /// <param name="displayCaption"></param>
//        /// <param name="FieldString"></param>
//        /// <param name="fieldHtmlId"></param>
//        /// <param name="RowClass"></param>
//        /// <returns></returns>
//        internal static string get2ColumnSingleRowInputRow(string displayCaption, string FieldString, string fieldHtmlId, string RowClass) {
//            string rowText = "<div class=\"form-group\"><label for=\"" + fieldHtmlId + "\">" + displayCaption + "</label><div class=\"ml-5\">" + FieldString + "</div></div>";
//            return Get2ColumnSingleRow(rowText, RowClass);
//        }
//        // 
//        // ====================================================================================================
//        /// <summary>
//        /// Create a two cell row in a two cell table
//        /// </summary>
//        /// <param name="leftCell"></param>
//        /// <param name="rightCell"></param>
//        /// <param name="leftCellStyleSelector"></param>
//        /// <param name="rightCellStyleSelector"></param>
//        /// <returns></returns>
//        internal static string Get2ColumnRow(string leftCell, string rightCell, string leftCellStyleSelector = "", string rightCellStyleSelector = "") {
//            return "<tr><td class=\"" + leftCellStyleSelector + "\">" + leftCell + "</td><td class=\"" + rightCellStyleSelector + "\">" + rightCell + "</td></tr>";
//        }
//        // 
//        // ====================================================================================================
//        /// <summary>
//        /// Return Yes or No
//        /// </summary>
//        /// <param name="Key"></param>
//        /// <returns></returns>
//        internal static string kmaGetYesNo(bool Key) {
//            if (Key) {
//                return "Yes";
//            }
//            return "No";
//        }
//        // 
//        // ====================================================================================================
//        /// <summary>
//        /// Create a series of html buttons from a list of buttonValues, all with name=button
//        /// </summary>
//        /// <param name="cp"></param>
//        /// <param name="buttonList"></param>
//        /// <param name="buttonName"></param>
//        /// <returns></returns>
//        internal static string getPanelButtons(CPBaseClass cp, string buttonList, string buttonName) {
//            string returnHtml = "";
//            var buttons = new List<string>();
//            // 
//            buttons.AddRange(buttonList.Split(','));
//            foreach (string button in buttons)
//                returnHtml += cp.Html5.Button(buttonName, button, "btn btn-primary");
//            return cp.Html5.Div(returnHtml, "", "buttonBarClass");
//        }
//        // 
//        // ====================================================================================================
//        /// <summary>
//        /// create an html image with the specific height and width
//        /// </summary>
//        /// <param name="Width"></param>
//        /// <param name="Height"></param>
//        /// <returns></returns>
//        internal static string kmaGetSpacer(long Width, long Height) {
//            return "<img alt=\"space\" src=\"/ccLib/images/spacer.gif\" width=\"" + Width + "\" height=\"" + Height + "\" border=\"0\">";
//        }
//        // 
//        // ====================================================================================================
//        /// <summary>
//        /// 'normalize the text field entered for startTime and endTime to a consistent format, blank or (0:00 to 23:59)
//        /// </summary>
//        /// <param name="cp"></param>
//        /// <param name="srcText"></param>
//        /// <returns></returns>
//        internal static string convertToTimeString(CPBaseClass cp, string srcText) {
//            string returnTime = "";
//            try {
//                string[] srcParts;
//                int srcHours = 0;
//                int srcMinutes = 0;
//                bool isPm = false;
//                // 
//                if (string.IsNullOrEmpty(srcText)) {
//                    returnTime = string.Empty;
//                } else {
//                    if (srcText.Length > 1) {
//                        if (srcText.Trim().Substring(srcText.Length - 2).ToLower() == "pm") {
//                            isPm = true;
//                        }
//                    }
//                    srcText = srcText.ToLower().Replace("am", "").Replace("pm", "").Trim();
//                    srcParts = srcText.Split(':');
//                    srcHours = cp.Utils.EncodeInteger(srcParts[0]);
//                    if (Information.UBound(srcParts) > 0) {
//                        srcMinutes = cp.Utils.EncodeInteger(srcParts[1]);
//                    }
//                    if (srcHours > 12) {
//                        srcHours -= 12;
//                        isPm = true;
//                    }
//                    if (srcHours > 12) {
//                        srcHours = 0;
//                    }
//                    if (srcMinutes > 59) {
//                        srcMinutes = 0;
//                    }
//                    returnTime = srcHours.ToString() + ":" + srcMinutes.ToString().PadLeft(2, '0');
//                    if (isPm) {
//                        returnTime += " pm";
//                    } else {
//                        returnTime += " am";
//                    }
//                }
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex, "convertToTimeString");
//            }
//            return returnTime;
//        }
//        // 
//        // ==========================================================================================
//        /// <summary>
//        /// create the report title html
//        /// </summary>
//        /// <param name="Title"></param>
//        /// <returns></returns>
//        internal static string GetReportTitle(string Title) {
//            return "<h2>" + Title + "</h2>";
//        }
//        // 
//        // ==========================================================================================
//        /// <summary>
//        /// create the read for the reports
//        /// </summary>
//        /// <param name="cp"></param>
//        /// <param name="selectedMeetingId"></param>
//        /// <param name="SessionID"></param>
//        /// <param name="SessionSelectionID"></param>
//        /// <returns></returns>
//        internal static string GetReportSubTitle(CPBaseClass cp, int selectedMeetingId, int SessionID = 0, int SessionSelectionID = 0) {
//            // 
//            // throw errors to caller
//            // 
//            string result = "";
//            var cs = cp.CSNew();
//            string meetingName = "";
//            string SessionName = "";
//            string SessionSelectionName = "";
//            // 
//            meetingName = GetMeetingSelect(cp, selectedMeetingId);
//            // 
//            cs.Open("Meeting Sessions", "ID=" + cp.Db.EncodeSQLNumber(SessionID));
//            if (cs.OK()) {
//                SessionName = cs.GetText("Name");
//            }
//            cs.Close();
//            // 
//            cs.Open(MeetingSessionSelectionModel.tableMetadata.contentName, "ID=" + cp.Db.EncodeSQLNumber(SessionSelectionID));
//            if (cs.OK()) {
//                SessionSelectionName = cs.GetText("Name");
//            }
//            cs.Close();
//            // 
//            result = "<br><span class=\"ccSmallHeadline3\">For Meeting: </span>" + meetingName;
//            // 
//            if (!string.IsNullOrEmpty(SessionName)) {
//                result += "<br><span class=\"ccSmallHeadline3\">For Sesion: </span>" + SessionName;
//            }
//            // 
//            if (!string.IsNullOrEmpty(SessionSelectionName)) {
//                result += "<br><span class=\"ccSmallHeadline3\">For Session Option: </span>" + SessionSelectionName;
//            }
//            // 
//            result += "<br>";
//            result += "<br>";
//            return result;
//        }


//        // 
//        internal static string GetFormHeadline(CPBaseClass cp, bool isAsciiVersion, string Name, string copyName, string defaultCopy = "") {
//            string returnHtml = "";
//            try {
//                if (!isAsciiVersion) {
//                    returnHtml = "<h2>" + Name + "</h2>";
//                    if (!string.IsNullOrEmpty(copyName)) {
//                        if (string.IsNullOrEmpty(defaultCopy)) {
//                            returnHtml += cp.Content.GetCopy(copyName).Replace("ccEditWrapper", "");
//                        } else {
//                            returnHtml += cp.Content.GetCopy(copyName, defaultCopy).Replace("ccEditWrapper", "");
//                        }
//                    }
//                }
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex);
//            }
//            return returnHtml;
//        }
//        // '
//        // '============================================================================================
//        // '   Get Atttndee Type Select
//        // '============================================================================================
//        // '
//        // Friend Shared Function GetAttendeeTypeSelect(cp As CPBaseClass, SelectedID As Integer, meetingID As Integer) As String
//        // Return GetFormInputSelect(cp, rnAttendeeTypeID, SelectedID, "Meeting Attendee Types", "MeetingID=" & meetingID, FilenameAttendeeTypeSelect, "-- Select Attendee type --", False)
//        // End Function
//        // '
//        // '============================================================================================
//        // '   Get Meeting Select
//        // '============================================================================================
//        // '
//        // Friend Shared Function GetMeetingSelect(cp As CPBaseClass, SelectedID As Integer, Optional Width As Integer = 0) As String
//        // Dim stream As String = ""
//        // Try
//        // If cp.User.IsAdmin() Then
//        // stream = GetFormInputMeetingSelect(cp, RequestNameMeetingID, SelectedID, "Meetings", "", FilenameMeetingSelect, "-- Select Meeting --", True, Width)
//        // Else
//        // stream = GetFormInputMeetingSelect(cp, RequestNameMeetingID, SelectedID, "Meetings", "DateEnd >" & cp.Db.EncodeSQLDate(Now), FilenameMeetingSelect, "-- Select Meeting --", True, Width)
//        // End If
//        // Catch ex As Exception
//        // cp.Site.ErrorReport(ex)
//        // End Try
//        // Return stream
//        // End Function
//        // '
//        // '============================================================================================
//        // '   Get Person Select
//        // '============================================================================================
//        // '
//        // Friend Shared Function GetPeopleSelect(cp As CPBaseClass, SelectedID As Integer) As String
//        // Return GetFormInputSelect(cp, RequestNamePeopleID, SelectedID, "People", "", FilenameAttendeeSelect, "-- Select Person --", True)
//        // End Function
//        // 
//        // ============================================================================================
//        // Get Attendee Select
//        // ============================================================================================
//        // 
//        internal static string GetAttendeeSelect(CPBaseClass cp, int SelectedID) {
//            return GetFormInputSelect(cp, Constants.rnAttendeeId, SelectedID, "Meeting Attendees", "", Constants.FilenamePeopleSelect, "-- Select Attendee --", true);
//        }
//        // 
//        // ============================================================================================
//        // Get Session Select
//        // Not baked because there are too many options
//        // ============================================================================================
//        // 
//        internal static string GetSessionSelect(CPBaseClass cp, int SelectedID, int meetingID) {
//            string result = "";
//            try {
//                var CSPointer = cp.CSNew();
//                int Position;
//                // 
//                result = "<select size=\"1\" name=\"" + Constants.rnSessionId + "\">";
//                result += "<option value=\"0\">-- Select a Session --</option>";
//                if (cp.Utils.EncodeInteger(meetingID) != 0) {
//                    CSPointer.Open("Meeting Sessions", "MeetingID=" + cp.Db.EncodeSQLNumber(meetingID), "Name");
//                } else {
//                    CSPointer.Open("Meeting Sessions", "", "Name");
//                }
//                if (CSPointer.OK()) {
//                    while (CSPointer.OK()) {
//                        result += Microsoft.VisualBasic.Constants.vbCrLf + "<option value=\"" + CSPointer.GetInteger("ID") + "\">" + CSPointer.GetText("Name") + "</option>";
//                        CSPointer.GoNext();
//                    }
//                }
//                CSPointer.Close();
//                result += "</select>";
//                // 
//                // ----- Set the selected option
//                // 
//                Position = Strings.InStr(1, result, "value=\"" + SelectedID + "\"");
//                if (Position != 0) {
//                    Position = Strings.InStr(Position, result, ">");
//                    if (Position != 0) {
//                        result = Strings.Mid(result, 1, Position - 1) + " selected>" + Strings.Mid(result, Position + 1);
//                    }
//                }
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex);
//            }
//            return result;
//        }
//        // 
//        // ============================================================================================
//        // Format a number into a printable fixed digit integer
//        // ============================================================================================
//        // 
//        internal static string FormatID(int Id) {
//            string returnString = "000000" + Id.ToString();
//            return returnString.Substring(returnString.Length - 6);
//        }
//        // 
//        // =============================================================================================
//        // Get the Edit link string used for all edit tags
//        // =============================================================================================
//        // 
//        internal static string GetEditLink(string QueryString, string LinkLabel) {
//            return "<div style=\"text-align:right\">[<A href=\"?" + QueryString + "\">" + LinkLabel + "</A>]</div>";
//        }
//        // 
//        // =============================================================================================
//        /// <summary>
//        /// create the form open html element
//        /// </summary>
//        /// <param name="cp"></param>
//        /// <returns></returns>
//        internal static string formStart(CPBaseClass cp) {
//            return "<FORM METHOD=\"post\" action=\"?" + cp.Doc.RefreshQueryString + "\">";
//        }
//        // 
//        internal static string getView_GridOpen_Html(string ReportName, string reportDescription) {
//            string result = ReportName;
//            if (!string.IsNullOrEmpty(reportDescription)) {
//                result += reportDescription;
//            }
//            return result + "<table class=\"msGrid\" border=\"0\" cellpadding=\"3\" cellspacing=\"0\" width=\"100%\">";
//        }
//        // 
//        // =============================================================================================
//        // 
//        internal static string getReportGridData_Legacy(CPBaseClass cp, bool IsASCIIVersion, int ColumnCount, string[] ColumnWidthArray, string[] ColumnAlignArray, string[] ColumnTextArray) {
//            string result = "";
//            try {
//                if (!IsASCIIVersion) {
//                    result = getReportGridHead_Html_Legacy(cp, ColumnCount, ColumnWidthArray, ColumnAlignArray, ColumnTextArray);
//                } else {
//                    result = getReportGridHead_Download_Legacy(cp, ColumnCount, ColumnWidthArray, ColumnAlignArray, ColumnTextArray);
//                }
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex);
//            }
//            return result;
//        }
//        // 
//        // =============================================================================================
//        // 
//        internal static string getReportGridHead_Html_Legacy(CPBaseClass cp, int ColumnCount, string[] ColumnWidthArray, string[] ColumnAlignArray, string[] ColumnTextArray) {
//            string result = "";
//            try {
//                int ColumnPointer;
//                // 
//                result += getView_GridRowRule(false, ColumnCount);
//                result += "<tr>";
//                var loopTo = ColumnCount - 1;
//                for (ColumnPointer = 0; ColumnPointer <= loopTo; ColumnPointer++)
//                    result += "<td valign=\"top\" width=\"" + ColumnWidthArray[ColumnPointer] + "\" align=\"" + ColumnAlignArray[ColumnPointer] + "\"><NOBR>" + ColumnTextArray[ColumnPointer] + "</NOBR></td>";
//                result += "</tr>";
//                result += getView_GridRowRule(false, ColumnCount);
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex);
//            }
//            return result;
//        }
//        // 
//        // =============================================================================================
//        // 
//        internal static string getReportGridHead_Download_Legacy(CPBaseClass cp, int ColumnCount, string[] ColumnWidthArray, string[] ColumnAlignArray, string[] ColumnTextArray) {
//            string result = "";
//            try {
//                int ColumnPointer;
//                string Delimiter;
//                string Cell;
//                // 
//                Delimiter = "";
//                var loopTo = ColumnCount - 1;
//                for (ColumnPointer = 0; ColumnPointer <= loopTo; ColumnPointer++) {
//                    Cell = cp.Utils.EncodeText(ColumnTextArray[ColumnPointer]);
//                    if (string.IsNullOrEmpty(Cell)) {
//                        result += Delimiter;
//                    } else {
//                        Cell = Strings.Replace(Cell, "\"", "\"\"");
//                        Cell = Strings.Replace(Cell, Microsoft.VisualBasic.Constants.vbCrLf, " ");
//                        result += Delimiter + "\"" + Cell + "\"";
//                    }
//                    Delimiter = ",";
//                }
//                result += Microsoft.VisualBasic.Constants.vbCrLf;
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex);
//            }
//            return result;
//        }
//        // 
//        // =======================================================================================
//        // 
//        // =======================================================================================
//        // 
//        internal static string getView_GridRowRule(bool isDownload, int ColumnCount) {
//            if (!isDownload)
//                return "<tr style=\"border-top:1px solid #000;\"><td colspan=\"" + ColumnCount + "\"><img src=\"/ccLib/images/spacer.gif\" width=\"1\" height=\"1\"></td></tr>";
//            return string.Empty;
//        }
//        // 
//        // =======================================================================================
//        // 
//        // =======================================================================================
//        // 
//        internal static string getView_GridRow_SubHead(bool isDownload, string message, int ColumnCount) {
//            if (!isDownload)
//                return "<tr><td colspan=\"" + ColumnCount + "\" align=\"left\">" + message + "</td></tr>";
//            return string.Empty;
//        }
//        // 
//        // =======================================================================================
//        // 
//        internal static string getReportGridRow_Legacy(CPBaseClass cp, bool isDownload, int RowNumber, int ColumnCount, string[] ColumnAlignArray, string[] ColumnTextArray) {
//            string result = "";
//            try {
//                int ColumnPointer;
//                string rowClass;
//                string ColumnText;
//                string Delimiter;
//                string Cell;
//                // 
//                if (!isDownload) {
//                    // 
//                    // ----- If not ascii version, output
//                    // 
//                    if (RowNumber % 2 == 0) {
//                        rowClass = "evenRow";
//                    } else {
//                        rowClass = "oddRow";
//                    }
//                    if (ColumnCount > 0) {
//                        result += "<tr class=\"" + rowClass + "\">";
//                        var loopTo = ColumnCount - 1;
//                        for (ColumnPointer = 0; ColumnPointer <= loopTo; ColumnPointer++) {
//                            ColumnText = ColumnTextArray[ColumnPointer];
//                            if (string.IsNullOrEmpty(ColumnText)) {
//                                ColumnText = "&nbsp;";
//                            }
//                            result += "<td align=\"" + ColumnAlignArray[ColumnPointer] + "\"><NOBR>" + ColumnText + "</NOBR></td>";
//                        }
//                        result += "</tr>";
//                    }
//                } else {
//                    // 
//                    // ----- If ascii version, output comma delimited
//                    // 
//                    Delimiter = "";
//                    var loopTo1 = ColumnCount - 1;
//                    for (ColumnPointer = 0; ColumnPointer <= loopTo1; ColumnPointer++) {
//                        Cell = cp.Utils.EncodeText(ColumnTextArray[ColumnPointer]);
//                        if (string.IsNullOrEmpty(Cell)) {
//                            result += Delimiter;
//                        } else {
//                            Cell = Strings.Replace(Cell, "\"", "\"\"");
//                            Cell = Strings.Replace(Cell, Microsoft.VisualBasic.Constants.vbCrLf, " ");
//                            result += Delimiter + "\"" + Cell + "\"";
//                        }
//                        Delimiter = ",";
//                        // result &=  Delimiter & """" & ColumnTextArray( ColumnPointer ) & """"
//                        // Delimiter = ","
//                    }
//                    result += Microsoft.VisualBasic.Constants.vbCrLf;
//                }
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex);
//            }
//            return result;
//        }
//        // 
//        // =======================================================================================
//        // 
//        internal static string getView_GridClose_Html(string rqs, CPBaseClass cp, bool isDownload, bool BlockASCIIExport = false) {
//            string result = "";
//            try {
//                if (!isDownload) {
//                    result += "</table>";
//                    if (!BlockASCIIExport) {
//                        // 
//                        // REFACTOR -- move this to the view routines so it can be custonized (include meetingId, etc)
//                        // 
//                        result += "<BR><a href=\"?" + rqs + "&" + Constants.rnIsDownload + "=1\" target=\"_Blank\">Ascii Export version (Right click and 'Save Target As')</a>";
//                    }
//                }
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex);
//            }
//            return result;
//        }
//        // '
//        // '=======================================================================================
//        // Friend Shared Function getView_GridRow_Column(cp As CPBaseClass, isHead As Boolean, isDownload As Boolean, ByRef columnCnt As Integer, ByRef rowCnt As Integer, ColumnNumber As Double, column As Double) As String
//        // Return getView_GridRow_Column(cp, isHead, isDownload, columnCnt, rowCnt, FormatCurrency(ColumnNumber), column)
//        // End Function
//        // '
//        // '=======================================================================================
//        // Friend Shared Function getView_GridRow_Column(cp As CPBaseClass, isHead As Boolean, isDownload As Boolean, ByRef columnCnt As Integer, ByRef rowCnt As Integer, ColumnDate As Date, column As Date) As String
//        // Return getView_GridRow_Column(cp, isHead, isDownload, columnCnt, rowCnt, FormController.encodeBlankShortDate(ColumnDate), column)
//        // End Function
//        // '
//        // '=======================================================================================
//        // Friend Shared Function getView_GridRow_Column(cp As CPBaseClass, isHead As Boolean, isDownload As Boolean, ByRef columnCnt As Integer, ByRef rowCnt As Integer, ColumnBoolean As Boolean, column As Boolean) As String
//        // Return getView_GridRow_Column(cp, isHead, isDownload, columnCnt, rowCnt, kmaGetYesNo(ColumnBoolean), column)
//        // End Function
//        // '
//        // '=======================================================================================
//        // Friend Shared Function getView_GridRow_Column(cp As CPBaseClass, isHead As Boolean, isDownload As Boolean, ByRef columnCnt As Integer, ByRef rowCnt As Integer, ColumnInteger As Integer, column As Integer) As String
//        // Return getView_GridRow_Column(cp, isHead, isDownload, columnCnt, rowCnt, ColumnInteger.ToString(), column)
//        // End Function
//        // 
//        // '=======================================================================================
//        // Friend Shared Function getView_GridRow(cp As CPBaseClass, isHead As Boolean, isDownload As Boolean, ByRef columnCnt As Integer, ByRef rowCnt As Integer, rowContent As String) As String
//        // rowCnt += 1
//        // If isDownload Then
//        // If (isHead) Then
//        // Return rowContent
//        // Else
//        // Return vbCrLf & rowContent
//        // End If
//        // ElseIf isHead Then
//        // Return FormController.cr & "<tr>" & rowContent & "</tr>"
//        // Else
//        // If rowContent = "" Then
//        // rowContent = "&nbsp;"
//        // End If
//        // If (rowCnt Mod 2) = 0 Then
//        // Return FormController.cr & "<tr class=""oddRow"">" & rowContent & "</tr>"
//        // Else
//        // Return FormController.cr & "<tr class=""evenRow"">" & rowContent & "</tr>"
//        // End If
//        // End If
//        // Return String.Empty
//        // End Function
//        // '
//        // '=======================================================================================
//        // Friend Shared Function getView_GridRow_Column(cp As CPBaseClass, isHead As Boolean, isDownload As Boolean, ByRef columnCnt As Integer, ByRef rowCnt As Integer, ColumnText As String, column As Models.View.genericReportViewModel.genericReportObjectColumnClass) As String
//        // columnCnt += 1
//        // If isHead Then
//        // '
//        // ' Head
//        // '
//        // If (column.includeInHtml And Not isDownload) Then
//        // Return FormController.cr2 & "<th valign=""middle"" width=""" & column.width & """ align=""" & column.justification & """><NOBR>" & column.caption & "</NOBR></th>"
//        // ElseIf (column.includeInDownload And isDownload) Then
//        // If columnCnt = 1 Then
//        // Return """" & column.caption.Replace("""", """""") & """"
//        // Else
//        // Return ",""" & column.caption.Replace("""", """""") & """"
//        // End If
//        // End If
//        // Else
//        // '
//        // ' body
//        // '
//        // If (column.includeInHtml And Not isDownload) Then
//        // Dim bgColor As String = ""
//        // If (rowCnt Mod 2) = 0 Then
//        // bgColor = "#F0F2F5"
//        // Else
//        // bgColor = "#FFFFFF"
//        // End If
//        // If ColumnText = "" Then
//        // ColumnText = "&nbsp;"
//        // End If
//        // Return FormController.cr2 & "<td valign=""middle"" align=""" & column.justification & """><NOBR>" & ColumnText & "</NOBR></td>"
//        // ElseIf (column.includeInDownload And isDownload) Then
//        // If columnCnt = 1 Then
//        // Return """" & ColumnText.Replace("""", """""") & """"
//        // Else
//        // Return ",""" & ColumnText.Replace("""", """""") & """"
//        // End If
//        // End If
//        // End If
//        // Return String.Empty
//        // End Function
//        // ====================================================================================================
//        /// <summary>
//        /// convert the adminMessageList into an html string to be added to the view
//        /// </summary>
//        /// <param name="adminMessageList"></param>
//        /// <returns></returns>
//        internal static string renderAdminWarnings(List<string> adminMessageList) {
//            return "<div class=\"ccHintWrapperContent\"><b>Administrator</b><br><br><p>" + string.Join("</p><p>", adminMessageList.ToArray()) + "</p></div>";





//        }
//        // ====================================================================================================
//        /// <summary>
//        /// convert the userErrorList into an html string to be added to the view
//        /// </summary>
//        /// <param name="userErrorList"></param>
//        /// <returns></returns>
//        internal static string renderUserErrorList(List<string> userErrorList) {
//            return "<div class=\"p-3 m-2 bg-danger text-white\"><p>" + string.Join("</p><p>", userErrorList.ToArray()) + "</p></div>";



//        }
//        // ====================================================================================================
//        /// <summary>
//        /// return a url to the meeting manager in the admin site
//        /// </summary>
//        /// <returns></returns>
//        internal static string meetingManagerLink(ApplicationModel app) {
//            return app.AdminUrl + "?addonguid=" + Constants.guidAddon_MeetingManagerAdmin;
//        }
//        // ====================================================================================================
//        /// <summary>
//        /// return an html anchor tag for the meeting manager administration page
//        /// </summary>
//        /// <returns></returns>
//        internal static string meetingManagerLinkHtml(ApplicationModel app) {
//            return "<a href=\"" + meetingManagerLink(app) + "\">Meeting Manager Administration page</a>";
//        }
//        // ====================================================================================================
//        /// <summary>
//        /// return an html anchor tag for the pageId specified
//        /// </summary>
//        /// <param name="cp"></param>
//        /// <param name="siteContext"></param>
//        /// <param name="pageId"></param>
//        /// <returns></returns>
//        internal static string pageEditLinkHtml(CPBaseClass cp, ApplicationModel app, int pageId) {
//            return "<a href=\"" + app.AdminUrl + "?af=4&cid=" + cp.Content.GetID("page content") + "&id=" + pageId + "\">edit this page</a>";
//        }
//        // ====================================================================================================
//        /// <summary>
//        /// return an html anchor tag to the ecommerce settings page
//        /// </summary>
//        /// <param name="app"></param>
//        /// <returns></returns>
//        internal static string ecommerceSettingsLinkHtml(Models.Domain.ApplicationModel siteContext) {
//            return "<a href=\"" + siteContext.AdminUrl + "?addonguid=%7BF54714FB-BF05-4E9C-B71F-B44818B9CC74%7D\">Ecommerce Settings</a>";
//        }
//        // ====================================================================================================
//        // 
//        internal static string get2ColumnRowPageHead(CPBaseClass cp, string viewName, int meetingID) {
//            string Stream = "";
//            try {
//                if ((viewName ?? "") == Constants.viewName_meetingSelect) {
//                    // 
//                    // -- select a meeting, this message is not stored with the meeting
//                    Stream = cp.Content.GetCopy(Constants.viewName_meetingSelect, Constants.viewDefaultCopy_meetingSelect);
//                    Stream = Get2ColumnSingleRow(Stream, "msRowPageHead");
//                } else {
//                    // 
//                    // -- Verify exist Instructions for the actual meetingID
//                    var cs = cp.CSNew();
//                    if (cs.Open("Meetings", "id=" + meetingID)) {
//                        switch (viewName ?? "") {
//                            case Constants.viewName_userLogin: {
//                                    Stream = cs.GetText("userLoginInstruction");
//                                    Stream = Get2ColumnSingleRow(Stream, "msRowPageHead");
//                                    break;
//                                }
//                            case Constants.viewName_attendee: {
//                                    Stream = cs.GetText("attendeeInstruction");
//                                    Stream = Get2ColumnSingleRow(Stream, "msRowPageHead");
//                                    break;
//                                }
//                            case Constants.viewName_attendeeDetails: {
//                                    Stream = cs.GetText("attendeeDetailsInstruction");
//                                    Stream = Get2ColumnSingleRow(Stream, "msRowPageHead");
//                                    break;
//                                }
//                            case Constants.viewName_guests: {
//                                    Stream = cs.GetText("guestsInstruction");
//                                    Stream = Get2ColumnSingleRow(Stream, "msRowPageHead");
//                                    break;
//                                }
//                            case Constants.viewName_attendeeSessions: {
//                                    Stream = cs.GetText("sessionsInstruction");
//                                    Stream = Get2ColumnSingleRow(Stream, "msRowPageHead");
//                                    break;
//                                }
//                            case Constants.viewName_items: {
//                                    Stream = cs.GetText("itemsInstruction");
//                                    Stream = Get2ColumnSingleRow(Stream, "msRowPageHead");
//                                    break;
//                                }
//                            case Constants.viewName_review: {
//                                    Stream = cs.GetText("reviewInstruction");
//                                    Stream = Get2ColumnSingleRow(Stream, "msRowPageHead");
//                                    break;
//                                }
//                            case Constants.viewName_complete: {
//                                    Stream = cs.GetText("completeInstruction");
//                                    Stream = Get2ColumnSingleRow(Stream, "msRowPageHead");
//                                    break;
//                                }

//                            default: {
//                                    Stream = "";
//                                    break;
//                                }
//                        }
//                    }
//                    cs.Close();
//                }
//                // 
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex, "get2ColumnRowPageHead");
//            }
//            return Stream;
//        }
//        // ====================================================================================================
//        // 
//        internal static string Get2ColumnRow(CPBaseClass cp, string leftCell, string rightCell, string leftCellStyleSelector = "", string rightCellStyleSelector = "") {
//            string Stream = "";
//            try {
//                // 
//                Stream += "<tr>";
//                Stream += "<TD";
//                if (!string.IsNullOrEmpty(leftCellStyleSelector)) {
//                    Stream += " class=\"" + leftCellStyleSelector + "\"";
//                }
//                Stream += ">" + leftCell + "</td>";
//                if (!string.IsNullOrEmpty(rightCellStyleSelector)) {
//                    Stream += "<td class=\"" + rightCellStyleSelector + "\">" + rightCell + "</td>";
//                } else {
//                    Stream += "<TD>" + rightCell + "</td>";
//                }
//                Stream += "</tr>";
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex, "Get2ColumnRow");
//            }
//            return Stream;
//        }
//        // ====================================================================================================
//        // 
//        internal static string Get4ColumnRow(CPBaseClass cp, string Cell1, string Cell2, string Cell3, string Cell4, string RowClass) {
//            string Stream = "";
//            try {
//                Stream += "<tr>";
//                Stream += "<td class=\"" + RowClass + "\">" + Cell1 + "</td>";
//                Stream += "<td class=\"" + RowClass + "\">" + Cell2 + "</td>";
//                Stream += "<td class=\"" + RowClass + "\">" + Cell3 + "</td>";
//                Stream += "<td class=\"" + RowClass + "\">" + Cell4 + "</td>";
//                Stream += "</tr>";
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex, "Get4ColumnRow");
//            }
//            return Stream;
//        }
//        // 
//        // ====================================================================================================
//        /// <summary>
//        /// Generic 5 column row
//        /// </summary>
//        /// <param name="Cell1"></param>
//        /// <param name="Cell2"></param>
//        /// <param name="Cell3"></param>
//        /// <param name="Cell4"></param>
//        /// <param name="Cell5"></param>
//        /// <param name="RowClass"></param>
//        /// <returns></returns>
//        /// <remarks></remarks>
//        internal static string Get5ColumnRow(CPBaseClass cp, string Cell1, string Cell2, string Cell3, string Cell4, string Cell5, string RowClass) {
//            string Stream = "";
//            try {
//                Stream += "<tr>";
//                Stream += "<td width=\"20%\" class=\"" + RowClass + "\">" + Cell1 + "</td>";
//                Stream += "<td width=\"20%\" class=\"" + RowClass + "\">" + Cell2 + "</td>";
//                Stream += "<td width=\"20%\" class=\"" + RowClass + "\">" + Cell3 + "</td>";
//                Stream += "<td width=\"20%\" class=\"" + RowClass + "\">" + Cell4 + "</td>";
//                Stream += "<td width=\"20%\" class=\"" + RowClass + "\">" + Cell5 + "</td>";
//                Stream += "</tr>";
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex, "Get5ColumnRow");
//            }
//            return Stream;
//        }

//        // ====================================================================================================
//        // 
//        internal static string GetCaptionMeetingDate(CPBaseClass cp, DateTime dateStart, DateTime dateEnd) {
//            string returnMeetingDate = "";
//            if (dateStart > DateTime.MinValue) {
//                returnMeetingDate = returnMeetingDate + formatDateLogOrShort(cp, dateStart, false);
//            }
//            if (dateEnd > DateTime.MinValue) {
//                if (!string.IsNullOrEmpty(returnMeetingDate)) {
//                    returnMeetingDate = returnMeetingDate + " - ";
//                }
//                returnMeetingDate = returnMeetingDate + formatDateLogOrShort(cp, dateEnd, false);
//            }
//            return returnMeetingDate;
//        }
//        // 
//        // ====================================================================================================
//        // 
//        internal static string setRefreshQueryString(CPBaseClass cp, int formId, int pageId, int sectionId, int global_MeetingId, int sessionId, int sessionSelectionId, bool showArchives) {
//            string returnQs = "";
//            try {
//                returnQs = cp.Doc.RefreshQueryString;
//                if (pageId != 0) {
//                    returnQs = cp_Utils_ModifyQueryString(cp, returnQs, Constants.RequestNamePageID, pageId.ToString(), true);
//                }
//                if (sectionId != 0) {
//                    returnQs = cp_Utils_ModifyQueryString(cp, returnQs, Constants.RequestNameSectionID, sectionId.ToString(), true);
//                }
//                returnQs = cp_Utils_ModifyQueryString(cp, returnQs, Constants.rnMeetingId, global_MeetingId.ToString(), true);
//                returnQs = cp_Utils_ModifyQueryString(cp, returnQs, Constants.rnSessionId, sessionId.ToString(), true);
//                returnQs = cp_Utils_ModifyQueryString(cp, returnQs, Constants.rnSessionSelectionId, sessionSelectionId.ToString(), true);
//                returnQs = cp_Utils_ModifyQueryString(cp, returnQs, Constants.rnShowArchives, Conversions.ToString(showArchives), true);
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex);
//            }
//            return returnQs;
//        }
//        // 
//        // ====================================================================================================
//        // 
//        internal static string cp_Utils_ModifyQueryString(CPBaseClass cp, string WorkingQuery, string queryName, string queryValue, bool addIfMissing = true) {
//            return cp.Utils.ModifyQueryString(WorkingQuery, queryName, queryValue, addIfMissing);
//        }
//        // 
//        // ====================================================================================================
//        // 
//        internal static string cp_Utils_ModifyQueryString(CPBaseClass cp, string WorkingQuery, string queryName, int queryValue, bool addIfMissing = true) {
//            return cp.Utils.ModifyQueryString(WorkingQuery, queryName, queryValue.ToString(), addIfMissing);
//        }
//        // 
//        // ====================================================================================================
//        // 
//        internal static string cp_Html_InputText(CPBaseClass cp, string htmlName, string htmlValue, string height, string size = "40", bool isPassword = false) {
//            if (htmlValue is null)
//                htmlValue = string.Empty;
//            if (isPassword) {
//                return cp.Html5.InputPassword(htmlName, 255);
//            } else if (height == "1") {
//                return cp.Html5.InputText(htmlName, 255, htmlValue);
//            } else {
//                return cp.Html5.InputTextArea(htmlName, 255, htmlValue);
//            }
//        }
//        // 
//        // ====================================================================================================
//        /// <summary>
//        /// Returns a column header with sort feature
//        /// </summary>
//        /// <param name="cp"></param>
//        /// <param name="view"></param>
//        /// <param name="global_meetingID"></param>
//        /// <param name="sortDirection"></param>
//        /// <param name="sortFieldSelected"></param>
//        /// <param name="WorkingQueryString"></param>
//        /// <param name="Caption"></param>
//        /// <param name="columnSortField"></param>
//        /// <returns></returns>
//        /// <remarks></remarks>
//        internal static string GetSortableColumnheader(CPBaseClass cp, int view, int global_meetingID, string sortDirection, string sortFieldSelected, string WorkingQueryString, string Caption, string columnSortField) {
//            string Stream = "";
//            try {
//                // 
//                Stream = "<a href=\"" + cp.Request.Page + "?" + WorkingQueryString;
//                Stream += "&" + Constants.rnDstViewId_deleteme + "=" + view;
//                Stream += "&" + Constants.rnMeetingId + "=" + global_meetingID;
//                Stream += "&" + Constants.RequestNameSortDirection + "=" + GetFieldSortDirection(columnSortField, sortDirection, sortFieldSelected);
//                Stream += "&" + Constants.RequestNameSortField + "=" + sortFieldSelected;
//                Stream += "\">";
//                Stream += Caption;
//                Stream += "</a>";
//                // 
//                Stream = Strings.Replace(Stream, "?&", "?" + cp.Doc.RefreshQueryString);
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex);
//            }
//            return Stream;
//        }
//        // 
//        // ====================================================================================================
//        /// <summary>
//        /// Returns the sort field direction
//        /// </summary>
//        /// <param name="CurrentSortField"></param>
//        /// <param name="CurrentSortDirection"></param>
//        /// <param name="SelectedSortField"></param>
//        /// <returns></returns>
//        /// <remarks></remarks>
//        internal static string GetFieldSortDirection(string CurrentSortField, string CurrentSortDirection, string SelectedSortField) {
//            if ((CurrentSortField ?? "") == (SelectedSortField ?? "")) {
//                switch (CurrentSortDirection ?? "") {
//                    case "asc": {
//                            return "desc";
//                        }
//                    case "desc": {
//                            return "asc";
//                        }

//                    default: {
//                            return "asc";
//                        }
//                }
//            } else {
//                return "asc";
//            }
//        }
//        // 
//        // ====================================================================================================
//        // 
//        internal static string encodeBlankShortDate(DateTime sourcedate) {
//            if (sourcedate >= DateTime.Parse("1900-01-01")) {
//                return sourcedate.ToShortDateString();
//            } else {
//                return "";
//            }
//        }
//        // 
//        // ====================================================================================================
//        // 
//        internal static string GetSessionDateAndTimeString(CPBaseClass cp, DateTime sessionDate, string sessionFromTime, string sessionToTime) {
//            string returnString = "";
//            try {
//                string outputFromTime;
//                string outputToTime;
//                // 
//                if (sessionDate > DateTime.MinValue) {
//                    returnString += formatDateLogOrShort(cp, sessionDate, false);
//                    outputFromTime = convertToTimeString(cp, sessionFromTime);
//                    outputToTime = convertToTimeString(cp, sessionToTime);
//                    if (!string.IsNullOrEmpty(outputFromTime) & !string.IsNullOrEmpty(outputToTime)) {
//                        returnString += ", " + outputFromTime;
//                        if (!string.IsNullOrEmpty(outputToTime)) {
//                            returnString += " to " + outputToTime;
//                        }
//                    }
//                }
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex);
//            }
//            return returnString;
//        }
//        // 
//        // ====================================================================================================
//        // 
//        public static string encodeActiveContentPersonalization(CPBaseClass cp, string Copy, CPCSBaseClass CSPointer) {
//            string RawCopy = Copy;
//            try {
//                int posStart;
//                int posEnd;
//                string TriggerString;
//                string ReplaceString;
//                string FieldValue = "";
//                string InquiryString;
//                string[] ValArray;
//                int ValArrayPointer;
//                string[] inValArray;
//                int inValArrayPointer;
//                string Value;
//                string FindString;
//                // 
//                TriggerString = "<AC type=\"PERSONALIZATION";
//                // 
//                if (Strings.InStr(1, RawCopy, TriggerString) != 0) {
//                    while (Strings.InStr(1, RawCopy, TriggerString) != 0) {
//                        if ((Strings.Left(RawCopy, Strings.Len(TriggerString)) ?? "") == (TriggerString ?? "")) {
//                            posStart = 1;
//                            posEnd = Strings.InStr(posStart, RawCopy, ">", Microsoft.VisualBasic.Constants.vbTextCompare) + 1;
//                        } else {
//                            posStart = Strings.InStr(1, RawCopy, TriggerString, Microsoft.VisualBasic.Constants.vbTextCompare);
//                            posEnd = Strings.InStr(posStart, RawCopy, ">", Microsoft.VisualBasic.Constants.vbTextCompare) + 1;
//                        }
//                        // 
//                        FindString = Strings.Mid(RawCopy, posStart, posEnd - posStart);
//                        // 
//                        InquiryString = Strings.Replace(FindString, "<AC ", "");
//                        InquiryString = Strings.Replace(InquiryString, ">", "");
//                        // 
//                        ValArray = Strings.Split(InquiryString, " ");
//                        ValArrayPointer = Information.UBound(ValArray);
//                        var loopTo = ValArrayPointer;
//                        for (ValArrayPointer = 0; ValArrayPointer <= loopTo; ValArrayPointer++) {
//                            Value = Strings.Left(ValArray[ValArrayPointer], Strings.Len(ValArray[ValArrayPointer]) - 1);
//                            inValArray = Strings.Split(Value, "\"", Compare: Microsoft.VisualBasic.Constants.vbTextCompare);
//                            inValArrayPointer = Information.UBound(inValArray);
//                            var loopTo1 = inValArrayPointer;
//                            for (inValArrayPointer = 0; inValArrayPointer <= loopTo1; inValArrayPointer++) {
//                                if (Strings.InStr(1, inValArray[inValArrayPointer], "Field=", Microsoft.VisualBasic.Constants.vbTextCompare) != 0) {
//                                    FieldValue = inValArray[inValArrayPointer];
//                                }
//                            }
//                        }
//                        // 
//                        switch (FieldValue ?? "") {
//                            case "Field=Name": {
//                                    ReplaceString = CSPointer.GetText("Name");
//                                    break;
//                                }
//                            case "Field=Email": {
//                                    ReplaceString = CSPointer.GetText("Email");
//                                    break;
//                                }
//                            case "Field=FirstName": {
//                                    ReplaceString = CSPointer.GetText("FirstName");
//                                    break;
//                                }
//                            case "Field=LastName": {
//                                    ReplaceString = CSPointer.GetText("LastName");
//                                    break;
//                                }

//                            default: {
//                                    ReplaceString = "";
//                                    break;
//                                }
//                        }
//                        // 
//                        RawCopy = Strings.Replace(RawCopy, FindString, ReplaceString, Compare: Microsoft.VisualBasic.Constants.vbTextCompare);
//                        // 
//                    }
//                }
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex);
//            }
//            return RawCopy;
//        }
//        // 
//        // ====================================================================================================
//        // 
//        internal static string formatDateLogOrShort(CPBaseClass cp, DateTime Value, bool DisplayTimeSeconds = false) {
//            if (DisplayTimeSeconds) {
//                return Value.ToLongDateString();
//            } else {
//                return Value.ToShortDateString();
//            }
//        }
//        // 
//        // ====================================================================
//        // 
//        public static string get4ColumnSubHead(CPBaseClass cp, string SubHead) {
//            string result = "";
//            try {
//                result = "<tr><td><IMG src=\"/ccLib/images/spacer.gif\" height=\"5\" width=\"1\"></td></tr>";
//                result = result + "<tr><td>&nbsp;</td><td align=\"Left\" class=\"PageSubTitle\" colspan=\"3\"><b>" + SubHead + "</b></td></tr>";
//                result = result + "<tr><td>&nbsp;</td><td colspan=\"3\" valign=\"center\"><IMG src=\"/ccLib/images/808080.gif\" height=\"1\" width=\"100%\"></td></tr>";

//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex);
//            }
//            return result;
//        }
//        // 
//        // ====================================================================================================
//        // 
//        public static string get4ColumnNameValueRow(CPBaseClass cp, string Cell0, string Cell1) {
//            string result = "";
//            try {
//                result = "<tr>";
//                result = result + "<td align=\"right\" style=\"width:25%\">" + Cell0 + "</td>";
//                result = result + "<td align=\"Left\" colspan=\"3\">" + Cell1 + "</td>";
//                result = result + "</tr>";
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex);
//            }
//            return result;
//        }
//        // 
//        // ====================================================================================================
//        // 
//        public static string get4ColumnRow(CPBaseClass cp, string HeaderLeft, string ValueLeft, string HeaderRight, string ValueRight) {
//            string result = "";
//            try {
//                result = "<tr>";
//                result = result + "  <td valign=\"top\" width=\"25%\" style=\"width:25%\" align=\"right\">" + HeaderLeft + "</td>";
//                result = result + "  <td valign=\"top\" width=\"25%\" style=\"width:25%\" align=\"left\">" + ValueLeft + "</td>";
//                result = result + "  <td valign=\"top\" width=\"25%\" style=\"width:25%\" align=\"right\">" + HeaderRight + "</td>";
//                result = result + "  <td valign=\"top\" width=\"25%\" style=\"width:25%\" align=\"left\">" + ValueRight + "</td>";
//                result = result + "  </tr>";
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex);
//            }
//            return result;
//        }
//        // 
//        // ============================================================================================
//        // Get Meeting Select
//        // ============================================================================================
//        // 
//        public static string GetMeetingSelect(CPBaseClass cp, int SelectedID, int Width = 0) {
//            string result = "";
//            try {
//                if (cp.User.IsAdmin) {
//                    result = GetFormInputMeetingSelect(cp, Constants.rnMeetingId, SelectedID, "Meetings", "", Constants.FilenameMeetingSelect, "-- Select Meeting --", true, Width);
//                } else {
//                    result = GetFormInputMeetingSelect(cp, Constants.rnMeetingId, SelectedID, "Meetings", "DateEnd >" + cp.Db.EncodeSQLDate(DateTime.Now), Constants.FilenameMeetingSelect, "-- Select Meeting --", true, Width);
//                }
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex);
//            }
//            return result;
//        }
//        // 
//        // ============================================================================================
//        // Get Person Select
//        // ============================================================================================
//        // 
//        public static string GetPeopleSelect(CPBaseClass cp, int SelectedID) {
//            return GetFormInputSelect(cp, Constants.RequestNamePeopleID, SelectedID, "People", "", Constants.FilenameAttendeeSelect, "-- Select Person --", true);
//        }
//        // 
//        // ============================================================================================
//        // 
//        public static string GetFormInputMeetingSelect(CPBaseClass cp, string RequestName, int SelectedID, string ContentName, string ContentCriteria, string BakeFilename, string SelectOnePhrase, bool AllowBake, int Width = 0) {
//            string result = "";
//            try {
//                int Position;
//                var CSPointer = cp.CSNew();
//                string TableName;
//                string sqlCriteria = "";
//                // 
//                if (!string.IsNullOrEmpty(cp.Utils.EncodeText(ContentCriteria))) {
//                    sqlCriteria = " " + cp.Utils.EncodeText(ContentCriteria) + " ";
//                }

//                TableName = cp.Content.GetTable(ContentName);

//                result = "<select id=\"msMeetingSelect\" size=\"1\" name=\"" + RequestName + "\"";
//                if (Width != 0) {
//                    result += " style=\"width=" + Width + "\" ";
//                }
//                result += ">";
//                result += "<option value=\"0\">" + SelectOnePhrase + "</option>";
//                CSPointer.Open(ContentName, sqlCriteria, "Name");
//                if (CSPointer.OK()) {
//                    while (CSPointer.OK()) {
//                        result += Microsoft.VisualBasic.Constants.vbCrLf + "<option value=\"" + CSPointer.GetInteger("ID") + "\">" + CSPointer.GetText("Name") + "</option>";
//                        CSPointer.GoNext();
//                    }
//                }
//                CSPointer.Close();
//                result += "</select>";
//                // 
//                // ----- Set the selected option
//                // 
//                Position = Strings.InStr(1, result, "value=\"" + SelectedID + "\"");
//                if (Position != 0) {
//                    Position = Strings.InStr(Position, result, ">");
//                    if (Position != 0) {
//                        result = Strings.Mid(result, 1, Position - 1) + " selected>" + Strings.Mid(result, Position + 1);
//                    }
//                }
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex);
//            }
//            return result;
//        }
//        // '
//        // '============================================================================================
//        // '   Get a Generic Baked Select box
//        // '============================================================================================
//        // '
//        // Friend Shared Function GetFormInputMeetingSelect(cp As CPBaseClass, RequestName As String, SelectedID As Integer, ContentName As String, ContentCriteria As String, BakeFilename As String, SelectOnePhrase As String, AllowBake As Boolean, Optional Width As Integer = 0) As String
//        // Dim Stream As String = ""
//        // Try
//        // Dim Position As Integer
//        // Dim CSPointer As CPCSBaseClass = cp.CSNew()
//        // Dim TableName As String
//        // Dim sqlCriteria As String = ""
//        // '
//        // If cp.Utils.EncodeText(ContentCriteria) <> "" Then
//        // sqlCriteria = " " & cp.Utils.EncodeText(ContentCriteria) & " "
//        // End If
//        // TableName = cp.Content.GetTable(ContentName)
//        // Stream = "<select size=""1"" name=""" & RequestName & """"
//        // If Width <> 0 Then
//        // Stream &= " style=""width=" & Width & """ "
//        // End If
//        // Stream &= " id=""msReportMeetingSelect"">"
//        // Stream &= "<option value=""0"">" & SelectOnePhrase & "</option>"
//        // CSPointer.Open(ContentName, sqlCriteria, "Name")
//        // If CSPointer.OK() Then
//        // Do While CSPointer.OK()
//        // Stream &= vbCrLf & "<option value=""" & CSPointer.GetInteger("ID") & """>" & CSPointer.GetText("Name") & "</option>"
//        // CSPointer.GoNext()
//        // Loop
//        // End If
//        // Call CSPointer.Close()
//        // Stream &= "</select>"
//        // Stream = Stream
//        // '
//        // ' ----- Set the selected option
//        // '
//        // Position = InStr(1, Stream, "value=""" & SelectedID & """")
//        // If Position <> 0 Then
//        // Position = InStr(Position, Stream, ">")
//        // If Position <> 0 Then
//        // Stream = Mid(Stream, 1, Position - 1) & " selected>" & Mid(Stream, Position + 1)
//        // End If
//        // End If
//        // Catch ex As Exception
//        // cp.Site.ErrorReport(ex)
//        // End Try
//        // Return Stream
//        // End Function
//        // 
//        // ============================================================================================
//        // Get a Generic Baked Select box
//        // ============================================================================================
//        // 
//        public static string GetFormInputSelect(CPBaseClass cp, string RequestName, int SelectedID, string ContentName, string ContentCriteria, string BakeFilename, string SelectOnePhrase, bool AllowBake) {
//            string result = "";
//            try {
//                string[] LineBuffer;
//                bool BakeContent;
//                int Position;
//                var CSPointer = cp.CSNew();
//                string SQL;
//                string TableName;
//                string sqlCriteria = "";
//                // 
//                if (!string.IsNullOrEmpty(cp.Utils.EncodeText(ContentCriteria))) {
//                    sqlCriteria = " WHERE " + cp.Utils.EncodeText(ContentCriteria) + " ";
//                }
//                // 
//                // ----- Get Baked Content
//                // 
//                BakeContent = true;
//                TableName = cp.Content.GetTable(ContentName);
//                result = cp.CdnFiles.Read(BakeFilename);

//                if (!string.IsNullOrEmpty(result) & AllowBake) {
//                    LineBuffer = Strings.Split(result, Microsoft.VisualBasic.Constants.vbCrLf);
//                    SQL = "Select ModifiedDate from " + TableName + sqlCriteria + " Order by ModifiedDate Desc;";
//                    CSPointer.OpenSQL(SQL);
//                    if (CSPointer.OK()) {
//                        if (CSPointer.GetDate("ModifiedDate") <= cp.Utils.EncodeDate(LineBuffer[0])) {
//                            BakeContent = false;
//                            LineBuffer[0] = "";
//                            result = Strings.Join(LineBuffer, Microsoft.VisualBasic.Constants.vbCrLf);
//                        }
//                    }
//                    CSPointer.Close();
//                }
//                // 
//                // ----- Bake it
//                // 
//                if (BakeContent) {

//                    result = "<select size=\"1\" name=\"" + RequestName + "\">";
//                    result += "<option value=\"0\">" + SelectOnePhrase + "</option>";
//                    CSPointer.Open(ContentName, cp.Utils.EncodeText(ContentCriteria), "Name");
//                    if (CSPointer.OK()) {
//                        while (CSPointer.OK()) {
//                            result += Microsoft.VisualBasic.Constants.vbCrLf + "<option value=\"" + CSPointer.GetInteger("ID") + "\">" + CSPointer.GetText("Name") + "</option>";
//                            CSPointer.GoNext();
//                        }
//                    }
//                    CSPointer.Close();
//                    result += "     </select>";
//                    // 
//                    // Save what you baked
//                    // 
//                    cp.CdnFiles.Save(BakeFilename, Conversions.ToString(DateTime.Now) + Microsoft.VisualBasic.Constants.vbCrLf + result);
//                }
//                result = result;
//                // 
//                // ----- Set the selected option
//                // 
//                Position = Strings.InStr(1, result, "value=\"" + SelectedID + "\"");
//                if (Position != 0) {
//                    Position = Strings.InStr(Position, result, ">");
//                    if (Position != 0) {
//                        result = Strings.Mid(result, 1, Position - 1) + " selected>" + Strings.Mid(result, Position + 1);
//                    }
//                }
//            } catch (Exception ex) {
//                cp.Site.ErrorReport(ex);
//            }
//            return result;
//        }
//    }
//}