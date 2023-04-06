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
    /// Spare part.
    /// </summary>
    // *** Start programmer edit section *** (SparePart CustomAttributes)

    // *** End programmer edit section *** (SparePart CustomAttributes)
    [AutoAltered()]
    [Caption("Spare part")]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("SparePartE", new string[] {
            "Name as \'Name\'",
            "Quantity as \'Quantity\'",
            "Used as \'Used\'",
            "ProducingCountry as \'Producing country\'",
            "ProducingCountry.Name as \'Name\'"}, Hidden=new string[] {
            "ProducingCountry.Name"})]
    [MasterViewDefineAttribute("SparePartE", "ProducingCountry", ICSSoft.STORMNET.LookupTypeEnum.Standard, "", "Name")]
    public class SparePart : ICSSoft.STORMNET.DataObject
    {
        
        private string fName;
        
        private int fQuantity;
        
        private bool fUsed = false;
        
        private IIS.AsyncOpenXmlReportsSample.ProducingCountry fProducingCountry;
        
        private IIS.AsyncOpenXmlReportsSample.Car fCar;
        
        // *** Start programmer edit section *** (SparePart CustomMembers)

        // *** End programmer edit section *** (SparePart CustomMembers)

        
        /// <summary>
        /// Name.
        /// </summary>
        // *** Start programmer edit section *** (SparePart.Name CustomAttributes)

        // *** End programmer edit section *** (SparePart.Name CustomAttributes)
        [StrLen(255)]
        public virtual string Name
        {
            get
            {
                // *** Start programmer edit section *** (SparePart.Name Get start)

                // *** End programmer edit section *** (SparePart.Name Get start)
                string result = this.fName;
                // *** Start programmer edit section *** (SparePart.Name Get end)

                // *** End programmer edit section *** (SparePart.Name Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (SparePart.Name Set start)

                // *** End programmer edit section *** (SparePart.Name Set start)
                this.fName = value;
                // *** Start programmer edit section *** (SparePart.Name Set end)

                // *** End programmer edit section *** (SparePart.Name Set end)
            }
        }
        
        /// <summary>
        /// Quantity.
        /// </summary>
        // *** Start programmer edit section *** (SparePart.Quantity CustomAttributes)

        // *** End programmer edit section *** (SparePart.Quantity CustomAttributes)
        public virtual int Quantity
        {
            get
            {
                // *** Start programmer edit section *** (SparePart.Quantity Get start)

                // *** End programmer edit section *** (SparePart.Quantity Get start)
                int result = this.fQuantity;
                // *** Start programmer edit section *** (SparePart.Quantity Get end)

                // *** End programmer edit section *** (SparePart.Quantity Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (SparePart.Quantity Set start)

                // *** End programmer edit section *** (SparePart.Quantity Set start)
                this.fQuantity = value;
                // *** Start programmer edit section *** (SparePart.Quantity Set end)

                // *** End programmer edit section *** (SparePart.Quantity Set end)
            }
        }
        
        /// <summary>
        /// Used.
        /// </summary>
        // *** Start programmer edit section *** (SparePart.Used CustomAttributes)

        // *** End programmer edit section *** (SparePart.Used CustomAttributes)
        public virtual bool Used
        {
            get
            {
                // *** Start programmer edit section *** (SparePart.Used Get start)

                // *** End programmer edit section *** (SparePart.Used Get start)
                bool result = this.fUsed;
                // *** Start programmer edit section *** (SparePart.Used Get end)

                // *** End programmer edit section *** (SparePart.Used Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (SparePart.Used Set start)

                // *** End programmer edit section *** (SparePart.Used Set start)
                this.fUsed = value;
                // *** Start programmer edit section *** (SparePart.Used Set end)

                // *** End programmer edit section *** (SparePart.Used Set end)
            }
        }
        
        /// <summary>
        /// Spare part.
        /// </summary>
        // *** Start programmer edit section *** (SparePart.ProducingCountry CustomAttributes)

        // *** End programmer edit section *** (SparePart.ProducingCountry CustomAttributes)
        public virtual IIS.AsyncOpenXmlReportsSample.ProducingCountry ProducingCountry
        {
            get
            {
                // *** Start programmer edit section *** (SparePart.ProducingCountry Get start)

                // *** End programmer edit section *** (SparePart.ProducingCountry Get start)
                IIS.AsyncOpenXmlReportsSample.ProducingCountry result = this.fProducingCountry;
                // *** Start programmer edit section *** (SparePart.ProducingCountry Get end)

                // *** End programmer edit section *** (SparePart.ProducingCountry Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (SparePart.ProducingCountry Set start)

                // *** End programmer edit section *** (SparePart.ProducingCountry Set start)
                this.fProducingCountry = value;
                // *** Start programmer edit section *** (SparePart.ProducingCountry Set end)

                // *** End programmer edit section *** (SparePart.ProducingCountry Set end)
            }
        }
        
        /// <summary>
        /// мастеровая ссылка на шапку IIS.AsyncOpenXmlReportsSample.Car.
        /// </summary>
        // *** Start programmer edit section *** (SparePart.Car CustomAttributes)

        // *** End programmer edit section *** (SparePart.Car CustomAttributes)
        [Agregator()]
        [NotNull()]
        public virtual IIS.AsyncOpenXmlReportsSample.Car Car
        {
            get
            {
                // *** Start programmer edit section *** (SparePart.Car Get start)

                // *** End programmer edit section *** (SparePart.Car Get start)
                IIS.AsyncOpenXmlReportsSample.Car result = this.fCar;
                // *** Start programmer edit section *** (SparePart.Car Get end)

                // *** End programmer edit section *** (SparePart.Car Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (SparePart.Car Set start)

                // *** End programmer edit section *** (SparePart.Car Set start)
                this.fCar = value;
                // *** Start programmer edit section *** (SparePart.Car Set end)

                // *** End programmer edit section *** (SparePart.Car Set end)
            }
        }
        
        /// <summary>
        /// Class views container.
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "SparePartE" view.
            /// </summary>
            public static ICSSoft.STORMNET.View SparePartE
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("SparePartE", typeof(IIS.AsyncOpenXmlReportsSample.SparePart));
                }
            }
        }
    }
    
    /// <summary>
    /// Detail array of SparePart.
    /// </summary>
    // *** Start programmer edit section *** (DetailArrayDetailArrayOfSparePart CustomAttributes)

    // *** End programmer edit section *** (DetailArrayDetailArrayOfSparePart CustomAttributes)
    public class DetailArrayOfSparePart : ICSSoft.STORMNET.DetailArray
    {
        
        // *** Start programmer edit section *** (IIS.AsyncOpenXmlReportsSample.DetailArrayOfSparePart members)

        // *** End programmer edit section *** (IIS.AsyncOpenXmlReportsSample.DetailArrayOfSparePart members)

        
        /// <summary>
        /// Construct detail array.
        /// </summary>
        /// <summary>
        /// Returns object with type SparePart by index.
        /// </summary>
        /// <summary>
        /// Adds object with type SparePart.
        /// </summary>
        public DetailArrayOfSparePart(IIS.AsyncOpenXmlReportsSample.Car fCar) : 
                base(typeof(SparePart), ((ICSSoft.STORMNET.DataObject)(fCar)))
        {
        }
        
        public IIS.AsyncOpenXmlReportsSample.SparePart this[int index]
        {
            get
            {
                return ((IIS.AsyncOpenXmlReportsSample.SparePart)(this.ItemByIndex(index)));
            }
        }
        
        public virtual void Add(IIS.AsyncOpenXmlReportsSample.SparePart dataobject)
        {
            this.AddObject(((ICSSoft.STORMNET.DataObject)(dataobject)));
        }
    }
}
