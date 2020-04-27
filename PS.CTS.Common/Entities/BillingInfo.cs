namespace PS.CTS.Common.Entities
{
    public class BillingInfo
    {
        public int Project_S_No { get; set; }
        public string MonthYear { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string TabName { get; set; }
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int AssociateID { get; set; }
        public string AssociateName { get; set; }
        public string Location { get; set; }
        public double BillingRate { get; set; }
        public int NoOfProjBillingdays { get; set; }
        public int NoOfAcutalLeaves { get; set; }
        public int NoOfCommLeaves { get; set; }
        public double ActualHoursBilledESA { get; set; }
        public string BLorML { get; set; }
        public double Rate { get; set; }
        public double BillingAmount { get; set; }
        public string ValidationComments { get; set; }

    }
}
