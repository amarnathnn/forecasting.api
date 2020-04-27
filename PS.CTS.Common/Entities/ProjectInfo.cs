using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PS.CTS.Common.Entities
{
    public class AccountInfo
    {
       
        public string Account_Name { get; set; }
        public string ESA_ProjectName { get; set; }
        public string ESA_ProjectID { get; set; }
        public string ESA_Project_StartDate { get; set; }
        public DateTime ESA_Project_EndDate { get; set; }
        public string Client_ProjectCode { get; set; }
        public string Client_CostCenter { get; set; }
        public string DTS_Owner { get; set; }
        public string Funding_Type { get; set; }

        public string Customer_Name { get; set; }
        public string Customer_Address { get; set; }
        public string SOW_ID { get; set; }

        public DateTime SOW_StartDate { get; set; }
        public DateTime SOW_EndDate { get; set; }
        public string CR_ID { get; set; }

        public DateTime CR_StartDate { get; set; }
        public DateTime CR_EndDate { get; set; }
        public string PO_ID { get; set; }
    }
}
