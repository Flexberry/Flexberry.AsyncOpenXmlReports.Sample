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
    /// Brand.
    /// </summary>
    // *** Start programmer edit section *** (Brand CustomAttributes)

    // *** End programmer edit section *** (Brand CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.@this)]
    [View("BrandE", new string[] {
            "Name as \'Name\'"})]
    [View("BrandL", new string[] {
            "Name as \'Name\'"})]
    public class Brand : ICSSoft.STORMNET.DataObject
    {
        
        private string fName;
        
        // *** Start programmer edit section *** (Brand CustomMembers)

        // *** End programmer edit section *** (Brand CustomMembers)

        
        /// <summary>
        /// Name.
        /// </summary>
        // *** Start programmer edit section *** (Brand.Name CustomAttributes)

        // *** End programmer edit section *** (Brand.Name CustomAttributes)
        [StrLen(255)]
        public virtual string Name
        {
            get
            {
                // *** Start programmer edit section *** (Brand.Name Get start)

                // *** End programmer edit section *** (Brand.Name Get start)
                string result = this.fName;
                // *** Start programmer edit section *** (Brand.Name Get end)

                // *** End programmer edit section *** (Brand.Name Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Brand.Name Set start)

                // *** End programmer edit section *** (Brand.Name Set start)
                this.fName = value;
                // *** Start programmer edit section *** (Brand.Name Set end)

                // *** End programmer edit section *** (Brand.Name Set end)
            }
        }
        
        /// <summary>
        /// Class views container.
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "BrandE" view.
            /// </summary>
            public static ICSSoft.STORMNET.View BrandE
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("BrandE", typeof(IIS.AsyncOpenXmlReportsSample.Brand));
                }
            }
            
            /// <summary>
            /// "BrandL" view.
            /// </summary>
            public static ICSSoft.STORMNET.View BrandL
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("BrandL", typeof(IIS.AsyncOpenXmlReportsSample.Brand));
                }
            }
        }
    }
}
