﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IIS.AsyncOpenXmlReportsSample
{
    using System;
    using System.Xml;
    using ICSSoft.STORMNET;
    
    
    // *** Start programmer edit section *** (Using statements)

    // *** End programmer edit section *** (Using statements)


    /// <summary>
    /// User report.
    /// </summary>
    // *** Start programmer edit section *** (UserReport CustomAttributes)

    // *** End programmer edit section *** (UserReport CustomAttributes)
    [AutoAltered()]
    [Caption("User report")]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("UserReportE", new string[] {
            "UserName",
            "ReportTaskStartTime",
            "Status",
            "File"})]
    [View("UserReportL", new string[] {
            "UserName",
            "ReportTaskStartTime",
            "Status",
            "File"})]
    public class UserReport : ICSSoft.STORMNET.DataObject
    {
        
        private string fUserName;
        
        private System.DateTime fReportTaskStartTime = DateTime.Now;
        
        private IIS.AsyncOpenXmlReportsSample.ReportStatusType fStatus;
        
        private ICSSoft.STORMNET.UserDataTypes.WebFile fFile;
        
        // *** Start programmer edit section *** (UserReport CustomMembers)

        // *** End programmer edit section *** (UserReport CustomMembers)

        
        /// <summary>
        /// File.
        /// </summary>
        // *** Start programmer edit section *** (UserReport.File CustomAttributes)

        // *** End programmer edit section *** (UserReport.File CustomAttributes)
        public virtual ICSSoft.STORMNET.UserDataTypes.WebFile File
        {
            get
            {
                // *** Start programmer edit section *** (UserReport.File Get start)

                // *** End programmer edit section *** (UserReport.File Get start)
                ICSSoft.STORMNET.UserDataTypes.WebFile result = this.fFile;
                // *** Start programmer edit section *** (UserReport.File Get end)

                // *** End programmer edit section *** (UserReport.File Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (UserReport.File Set start)

                // *** End programmer edit section *** (UserReport.File Set start)
                this.fFile = value;
                // *** Start programmer edit section *** (UserReport.File Set end)

                // *** End programmer edit section *** (UserReport.File Set end)
            }
        }
        
        /// <summary>
        /// ReportTaskStartTime.
        /// </summary>
        // *** Start programmer edit section *** (UserReport.ReportTaskStartTime CustomAttributes)

        // *** End programmer edit section *** (UserReport.ReportTaskStartTime CustomAttributes)
        public virtual System.DateTime ReportTaskStartTime
        {
            get
            {
                // *** Start programmer edit section *** (UserReport.ReportTaskStartTime Get start)

                // *** End programmer edit section *** (UserReport.ReportTaskStartTime Get start)
                System.DateTime result = this.fReportTaskStartTime;
                // *** Start programmer edit section *** (UserReport.ReportTaskStartTime Get end)

                // *** End programmer edit section *** (UserReport.ReportTaskStartTime Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (UserReport.ReportTaskStartTime Set start)

                // *** End programmer edit section *** (UserReport.ReportTaskStartTime Set start)
                this.fReportTaskStartTime = value;
                // *** Start programmer edit section *** (UserReport.ReportTaskStartTime Set end)

                // *** End programmer edit section *** (UserReport.ReportTaskStartTime Set end)
            }
        }
        
        /// <summary>
        /// Status.
        /// </summary>
        // *** Start programmer edit section *** (UserReport.Status CustomAttributes)

        // *** End programmer edit section *** (UserReport.Status CustomAttributes)
        public virtual IIS.AsyncOpenXmlReportsSample.ReportStatusType Status
        {
            get
            {
                // *** Start programmer edit section *** (UserReport.Status Get start)

                // *** End programmer edit section *** (UserReport.Status Get start)
                IIS.AsyncOpenXmlReportsSample.ReportStatusType result = this.fStatus;
                // *** Start programmer edit section *** (UserReport.Status Get end)

                // *** End programmer edit section *** (UserReport.Status Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (UserReport.Status Set start)

                // *** End programmer edit section *** (UserReport.Status Set start)
                this.fStatus = value;
                // *** Start programmer edit section *** (UserReport.Status Set end)

                // *** End programmer edit section *** (UserReport.Status Set end)
            }
        }
        
        /// <summary>
        /// UserName.
        /// </summary>
        // *** Start programmer edit section *** (UserReport.UserName CustomAttributes)

        // *** End programmer edit section *** (UserReport.UserName CustomAttributes)
        [StrLen(255)]
        public virtual string UserName
        {
            get
            {
                // *** Start programmer edit section *** (UserReport.UserName Get start)

                // *** End programmer edit section *** (UserReport.UserName Get start)
                string result = this.fUserName;
                // *** Start programmer edit section *** (UserReport.UserName Get end)

                // *** End programmer edit section *** (UserReport.UserName Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (UserReport.UserName Set start)

                // *** End programmer edit section *** (UserReport.UserName Set start)
                this.fUserName = value;
                // *** Start programmer edit section *** (UserReport.UserName Set end)

                // *** End programmer edit section *** (UserReport.UserName Set end)
            }
        }
        
        /// <summary>
        /// Class views container.
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "UserReportE" view.
            /// </summary>
            public static ICSSoft.STORMNET.View UserReportE
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("UserReportE", typeof(IIS.AsyncOpenXmlReportsSample.UserReport));
                }
            }
            
            /// <summary>
            /// "UserReportL" view.
            /// </summary>
            public static ICSSoft.STORMNET.View UserReportL
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("UserReportL", typeof(IIS.AsyncOpenXmlReportsSample.UserReport));
                }
            }
        }
    }
}
