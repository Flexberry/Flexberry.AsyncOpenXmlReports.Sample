﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace IIS.AsyncOpenXmlReportsSample.Templates.MailTemplates
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "D:\Work\Projects\Flexberry.AsyncOpenXmlReports.Sample\src\AsyncOpenXmlReportsSample\ODataBackend\Templates\MailTemplates\T4MailTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class T4MailTemplate : T4MailTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">\r\n<html xmlns=\"http" +
                    "://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>");
            
            #line 9 "D:\Work\Projects\Flexberry.AsyncOpenXmlReports.Sample\src\AsyncOpenXmlReportsSample\ODataBackend\Templates\MailTemplates\T4MailTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Subject));
            
            #line default
            #line hidden
            this.Write(@"</title>

</head>
<body>
<table border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%;border-collapse:collapse"">
<tbody>
<tr>
<td valign=""top"" style=""padding:0cm 0cm 0cm 0cm"">
  <div align=""center"">
    <table border=""1"" cellspacing=""0"" cellpadding=""0"" width=""0"" style=""width:450.0pt;border-collapse:collapse;border:none"">
      <tbody>
        <tr>
          <td valign=""top"" style=""border-top:none;border-left:solid #eeeeee 1.0pt;border-bottom:none;border-right:solid #eeeeee 1.0pt;padding:0cm 0cm 0cm 0cm"">
            <div align=""center"">
              <table border=""0"" cellspacing=""0"" cellpadding=""0"" width=""0"" style=""width:100%;border-collapse:collapse"">
                <tbody>
                  <tr>
                  <td align=""center"" width=""100%"" style=""width:33.0%;background:#730c86;padding:15.0pt 15.0pt 15.0pt 15.0pt"""">
                    <a href=""");
            
            #line 27 "D:\Work\Projects\Flexberry.AsyncOpenXmlReports.Sample\src\AsyncOpenXmlReportsSample\ODataBackend\Templates\MailTemplates\T4MailTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.PublishedSiteUrl));
            
            #line default
            #line hidden
            this.Write(@""" target=""_blank""><img width=""150"" alt=""Flexberry platform"" src=""cid:logoIconCid""></a>
                  </td> 
                  </tr>
                </tbody>
              </table>
            </div>
          </td>
        </tr>
        <tr>
          <td valign=""top"" style=""border-top:none;border-left:solid #eeeeee 1.0pt;border-bottom:none;border-right:solid #eeeeee 1.0pt;background:white;padding:0cm 0cm 0cm 0cm"">
            <div align=""center"">
              <table border=""0"" cellspacing=""0"" cellpadding=""0"" width=""0"" style=""width:450.0pt;border-collapse:collapse"">
                <tbody>
                  <tr>
                    <td valign=""top"" style=""padding:0cm 0cm 0cm 0cm"">
                      <table border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%;border-collapse:collapse"">
                        <tbody>
                          <tr>
                            <td valign=""top"" style=""padding:22.5pt 22.5pt 22.5pt 22.5pt"">
                              <div>
                                <h2><span style=""font-family:&quot;Arial&quot;,sans-serif;color:#333333"">");
            
            #line 47 "D:\Work\Projects\Flexberry.AsyncOpenXmlReports.Sample\src\AsyncOpenXmlReportsSample\ODataBackend\Templates\MailTemplates\T4MailTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Greetings));
            
            #line default
            #line hidden
            this.Write(@"</span></h2>
                                <table border=""0"" cellspacing=""0"" cellpadding=""0"" width=""0"" style=""width:450.0pt;border-collapse:collapse"">
                                  <tbody>
                                    <tr>
                                      <td style=""padding:0cm 0cm 0cm 0cm"">
                                        <table border=""0"" cellspacing=""0"" cellpadding=""0"" align=""left"" width=""0"" style=""width:362.5pt;border-collapse:collapse"">
                                          <tbody>
                                            <tr>
                                              <td style=""padding:6.0pt 6.0pt 12.0pt 6.0pt"">
                                                <p><span style=""font-size:11.0pt;line-height:15pt;font-family:&quot;Arial&quot;,sans-serif;color:#333333"">");
            
            #line 56 "D:\Work\Projects\Flexberry.AsyncOpenXmlReports.Sample\src\AsyncOpenXmlReportsSample\ODataBackend\Templates\MailTemplates\T4MailTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.HtmlMessage));
            
            #line default
            #line hidden
            this.Write("</span></p>\r\n                                                <p></p>\r\n           " +
                    "                                     \r\n                                         " +
                    "     </td>\r\n                                            </tr>\r\n                 " +
                    "                           <tr>\r\n                                              <" +
                    "td style=\"border:none;border-top:solid #dddddd 1.0pt;padding:6.0pt 6.0pt 6.0pt 6" +
                    ".0pt\">\r\n                                                <p><span style=\"font-siz" +
                    "e:11.0pt;line-height:15pt;font-family:&quot;Arial&quot;,sans-serif;color:#a2a2a2" +
                    "\">\tС уважением, команда Flexberry Platform</span></p>\r\n                         " +
                    "                     </td>\r\n                                            </tr>\r\n " +
                    "                                         </tbody>\r\n                             " +
                    "           </table>\r\n                                      </td>\r\n              " +
                    "                      </tr>\r\n                                  </tbody>\r\n       " +
                    "                         </table>\r\n                              </div>\r\n       " +
                    "                     </td>\r\n                          </tr>\r\n                   " +
                    "     </tbody>\r\n                      </table>\r\n                    </td>\r\n      " +
                    "            </tr>\r\n                </tbody>\r\n              </table>\r\n           " +
                    " </div>\r\n          </td>\r\n        </tr>\r\n        <tr>\r\n          <td valign=\"top" +
                    "\" style=\"border:solid #eeeeee 1.0pt;border-top:none;background-color:#f4f4f4;pad" +
                    "ding:0cm 0cm 0cm 0cm\">\r\n            <div align=\"center\">\r\n              <table b" +
                    "order=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"0\" style=\"width:450.0pt;border-" +
                    "collapse:collapse;background-color:#f4f4f4\">\r\n                <tbody>\r\n         " +
                    "         <tr>\r\n                    <td valign=\"top\" style=\"padding:22.5pt 22.5pt" +
                    " 22.5pt 22.5pt\">\r\n                      <table border=\"0\" cellspacing=\"0\" cellpa" +
                    "dding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse:collapse;background-c" +
                    "olor:#f4f4f4\">\r\n                        <tbody>\r\n                          <tr>\r" +
                    "\n                            <td style=\"border:none;background-color:#f4f4f4\">\r\n" +
                    "                              <p align=\"center\" style=\"text-align:center\">\r\n    " +
                    "                            <span style=\"font-size:8.0pt;font-family:&quot;Arial" +
                    "&quot;,sans-serif;color:#333333\">\r\n                                  <a href=\"ht" +
                    "tps://vk.com/flexberry\" target=\"_blank\"><img alt=\"Vk\" src=\"cid:SocIconVkCid\"></a" +
                    ">&nbsp; &nbsp;\r\n                                  <a href=\"https://twitter.com/F" +
                    "lexberry\" target=\"_blank\"><img alt=\"Twitter\" src=\"cid:SocIconTwCid\"></a>&nbsp; &" +
                    "nbsp;\r\n                                  <a href=\"https://github.com/Flexberry\" " +
                    "target=\"_blank\"><img alt=\"Github\" src=\"cid:SocIconGhCid\"></a>&nbsp; &nbsp;\r\n    " +
                    "                              <a href=\"https://www.youtube.com/user/FlexberryPLA" +
                    "TFORM\" target=\"_blank\"><img alt=\"Youtube\" src=\"cid:SocIconYtCid\"></a>&nbsp; &nbs" +
                    "p;\r\n                                  <a href=\"https://t.me/flexberry\" target=\"_" +
                    "blank\"><img alt=\"Telegram\" src=\"cid:SocIconTgCid\"></a>\r\n                        " +
                    "        </span>\r\n                              </p>\r\n                           " +
                    " </td>\r\n                          </tr>\r\n                        </tbody>\r\n     " +
                    "                 </table>\r\n                    </td>\r\n                  </tr>\r\n " +
                    "               </tbody>\r\n              </table>\r\n            </div>\r\n          <" +
                    "/td>\r\n        </tr>\r\n      </tbody>\r\n    </table>\r\n  </div>\r\n</td>\r\n</tr>\r\n</tbo" +
                    "dy>\r\n</table>\r\n</body>\r\n</html>");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public class T4MailTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
