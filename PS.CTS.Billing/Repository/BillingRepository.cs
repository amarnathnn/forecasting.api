using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PS.CTS.Common.Entities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace PS.CTS.Billing.Repository
{
    public class BillingRepository:IBillingRepository
    {

        public string _connectionstr;
        
        public BillingRepository(IConfiguration configuration)
        {
            _connectionstr = configuration.GetConnectionString("ForecastingBilling");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<BillingInfo> GetBillingInfo(SearchRequest request)
        {
            var response = new List<BillingInfo>();
            try
            {
                //Specify stored proc we will call
                string SqlProc = "[dbo].[uspGenerateBillingData]";
                Database db = new SqlDatabase(_connectionstr);

                using (DbCommand command = db.GetStoredProcCommand(SqlProc))
                {
                    db.AddInParameter(command, "@Year", DbType.String, request.MonthYear.Year);
                    db.AddInParameter(command, "@MonthName", DbType.String, request.MonthYear.Month);
                    try
                    {
                        using (DataSet dataSet = db.ExecuteDataSet(command))
                        {
                            if (dataSet != null)
                            {
                                if (dataSet.Tables.Count > 0)
                                {
                                    dataSet.Tables[0].TableName = "Billing";

                                    if (dataSet.Tables["Billing"].Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dataSet.Tables["Billing"].Rows)
                                        {
                                            var billdata = new BillingInfo();
                                            billdata.Project_S_No = Convert.ToInt32(dr["Project S.No"]);
                                            billdata.MonthYear = Convert.ToString(dr["Month - YYYY"]);
                                            billdata.CustomerID = Convert.ToString(dr["Customer ID"]);
                                            billdata.CustomerName = Convert.ToString(dr["Customer Name"]);
                                            billdata.TabName = Convert.ToString(dr["Tab Name"]);
                                            billdata.ProjectID = Convert.ToString(dr["Project ID"]);
                                            billdata.ProjectName = Convert.ToString(dr["Project Name"]);
                                            billdata.AssociateID = Convert.ToInt32(dr["Associate ID"]);
                                            billdata.AssociateName = Convert.ToString(dr["Associate Name"]);
                                            billdata.Location = Convert.ToString(dr["Location"]);
                                            billdata.NoOfProjBillingdays = Convert.ToInt32(dr["#Proj days for Billing"]);
                                            billdata.NoOfAcutalLeaves = Convert.ToInt32(dr["#Acutal Leaves"]);
                                            billdata.NoOfCommLeaves = Convert.ToInt32(dr["#Comm. Leaves"]);
                                            billdata.ActualHoursBilledESA = Convert.ToInt32(dr["Actual Hours Billed in ESA (Client)"]);
                                            billdata.BLorML = Convert.ToString(dr["BL / ML / BackLog / MostLikely"]);
                                            billdata.Rate = Convert.ToDouble(dr["Rate"]);
                                            billdata.BillingAmount = Convert.ToDouble(dr["Billing Amount"]);
                                            billdata.ValidationComments = dr["ValidationComments"] != DBNull.Value ?  Convert.ToString(dr["ValidationComments"]):string.Empty;

                                            response.Add(billdata);
                                        }

                                    }

                                }

                            }
                        }
                        return response;

                    }
                    catch (SqlException ex)
                    {
                        throw new ApplicationException(string.Format("An error occured when attempting to execute \"{0}\": {1}", SqlProc, ex.Message));
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
