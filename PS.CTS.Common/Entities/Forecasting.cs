using System;
using System.Collections.Generic;
using System.Text;

namespace PS.CTS.Common.Entities
{
   public class ForecastingInfo
    {
        public string Project { get; set; }
        public string MonthYear { get; set; }
        public string Vendor { get; set; }
        public string Costcenter { get; set; }
        public string WorkDayID { get; set; }
        public string ResourceName { get; set; }
        public string DTSOwner { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string Group { get; set; }
        public string Location { get; set; }
        public double Hours { get; set; }
        public double Rate { get; set; }
        public double TotalAmount { get; set; }
        public string ValidationComments { get; set; }


    }
}
