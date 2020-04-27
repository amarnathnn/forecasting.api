using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using PS.CTS.Common.Entities;
using PS.CTS.Common.Utility;
using Spire.Xls;

namespace PS.CTS.Forecasting.Repository
{
    public class ForecastingRepository : IForecastingRepository
    {


        public ForecastingRepository(IConfiguration configuration)
        {
            ForecstingConnection = configuration.GetConnectionString("ForecastingBilling");
        }

        public string ForecstingConnection
        {
            get; set;
        }
        public List<ForecastingInfo> GetForecastingInfo(SearchRequest request)
        {
            var response = new List<ForecastingInfo>();

            try
            {
                //Specify stored proc we will call
                string SqlProc = "[DBO].[uspGenerateForecastData]";
                Database db = new SqlDatabase(ForecstingConnection);

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
                                    dataSet.Tables[0].TableName = "Forecasting";

                                    if (dataSet.Tables["Forecasting"].Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dataSet.Tables["Forecasting"].Rows)
                                        {
                                            var forecastData = new ForecastingInfo();
                                            forecastData.Project = Convert.ToString(dr["Project"]);
                                            forecastData.MonthYear = Convert.ToString(dr["Month - YYYY"]);
                                            forecastData.Vendor = dr["Vendor"] != DBNull.Value ? Convert.ToString(dr["Vendor"]) : string.Empty;
                                            forecastData.Costcenter = dr["Cost Center"] != DBNull.Value ? Convert.ToString(dr["Cost Center"]) : string.Empty;
                                            forecastData.WorkDayID = dr["Workday ID"] != DBNull.Value ? Convert.ToString(dr["Workday ID"]) : string.Empty;
                                            forecastData.ResourceName = Convert.ToString(dr["Resource Name"]);
                                            forecastData.DTSOwner = dr["DTS Owner"] != DBNull.Value ? Convert.ToString(dr["DTS Owner"]) : string.Empty;
                                            forecastData.PurchaseOrderNo = dr["Purchase Order#"] != DBNull.Value ? Convert.ToString(dr["Purchase Order#"]) : string.Empty;
                                            forecastData.Group = dr["Group"] != DBNull.Value ? Convert.ToString(dr["Group"]) : string.Empty;
                                            forecastData.Location = dr["Location"] != DBNull.Value ? Convert.ToString(dr["Location"]) : string.Empty;
                                            forecastData.Hours = Convert.ToDouble(dr["Hours"]);
                                            forecastData.Rate = Convert.ToDouble(dr["Rate"]);
                                            forecastData.TotalAmount = Convert.ToDouble(dr["Total"]);
                                            forecastData.ValidationComments = dr["ValidationComments"] != DBNull.Value ? Convert.ToString(dr["ValidationComments"]) : string.Empty;

                                            response.Add(forecastData);
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

        public DataTable GetForecastingExcel(SearchRequest request)
        {
            var dtForecasting = new DataTable();

            try
            {
                //Specify stored proc we will call
                string SqlProc = "[DBO].[uspGenerateForecastData]";
                Database db = new SqlDatabase(ForecstingConnection);

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
                                    dtForecasting = dataSet.Tables[0];
                                }
                            }
                        }
                        return dtForecasting;
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

        public bool UploadTemplateData(string filePath)
        {
            bool isUpload;
            try
            {
                FileTemplateParser cParser = new FileTemplateParser();
                var dt = ConvertCsvToDataTableFreeSpire(filePath);
                SqlConnection con = new SqlConnection(ForecstingConnection);

                if (filePath.Contains("Allocation"))
                {
                    LoadAllocationsTemplateData(con, dt);
                }
                else if (filePath.Contains("Projects"))
                {
                    LoadProjectsTemplateData(con, dt);
                }
                else if (filePath.Contains("Users"))
                {
                    LoadUsersTemplateData(con, dt);
                }
                else if (filePath.Contains("AssociateLeave"))
                {
                    LoadAssociateLeaveTemplateData(con, dt);
                }
                else
                {
                    throw new Exception("Invalid Upload Type");
                }
                isUpload = true;


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpload;
        }

        public bool UploadStreamData(System.IO.Stream fileStream)
        {
            bool isUpload;
            try
            {
                FileTemplateParser cParser = new FileTemplateParser();
                DataTable dtUsers = ConvertExcelToDataTableFreeSpire(fileStream, "Users");
                DataTable dtProjects = ConvertExcelToDataTableFreeSpire(fileStream, "Projects");
                DataTable dtAllocation = ConvertExcelToDataTableFreeSpire(fileStream, "Allocation");
                DataTable dtAssociateLeave = ConvertExcelToDataTableFreeSpire(fileStream, "AssociateLeave");

                SqlConnection con = new SqlConnection(ForecstingConnection);
                LoadAllocationsTemplateData(con, dtAllocation);
                LoadProjectsTemplateData(con, dtProjects);
                LoadUsersTemplateData(con, dtUsers);
                LoadAssociateLeaveTemplateData(con, dtAssociateLeave);
                isUpload = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpload;
        }
        public DataTable ConvertCsvToDataTableFreeSpire(string filepath)
        {
            Workbook workbook = new Workbook();
            //Load csv File  
            workbook.LoadFromFile(filepath, ",");
            Worksheet worksheet = workbook.Worksheets[0];
            //Convert the first worksheet to datatable  
            DataTable dt = worksheet.ExportDataTable();
            return dt;
        }
        public DataTable ConvertExcelToDataTableFreeSpire(System.IO.Stream fileStream, string worksheetName)
        {
            Workbook workbook = new Workbook();
            //Load csv File  
            workbook.LoadFromStream(fileStream);
            Worksheet worksheet = workbook.Worksheets[worksheetName];
            //foreach (Worksheet worksheet1 in workbook.Worksheets)
            //{
            //    string name = worksheet1.Name;
            //}
            int cnt = workbook.Worksheets.Count();
            //Convert the first worksheet to datatable  
            DataTable dt = worksheet.ExportDataTable();
            return dt;
        }

         
        private void LoadUsersTemplateData(SqlConnection con, DataTable dt)
        {
            using (var command = new SqlCommand("uspInsertTemplateLoadUsersData", con) { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.Add(new SqlParameter("@UsersTemplateLoadType", dt));
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }


        }

        private void LoadProjectsTemplateData(SqlConnection con, DataTable dt)
        {
            using (var command = new SqlCommand("uspInsertTemplateLoadProjectsData", con) { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.Add(new SqlParameter("@ProjectsTemplateLoadType", dt));
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }


        private void LoadAllocationsTemplateData(SqlConnection con, DataTable dt)
        {
            using (var command = new SqlCommand("uspInsertTemplateLoadAllocationsData", con) { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.Add(new SqlParameter("@AllocationsTemplateLoadType", dt));
                con.Open();
                command.ExecuteNonQuery();
                con.Close();

            }

        }

        private void LoadAssociateLeaveTemplateData(SqlConnection con, DataTable dt)
        {
            using (var command = new SqlCommand("uspInsertTemplateLoadAssociateLeaveData", con) { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.Add(new SqlParameter("@AssociateLeaveTemplateLoadType", dt));
                con.Open();
                command.ExecuteNonQuery();
                con.Close();

            }
        }

        public void UploadFiletoAzure(string fileToUpload,string filePath)
        {
          
            FileStorageHandler azureBlob = new FileStorageHandler();

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            string inputFileName = fileToUpload;
            string uploadFileName = Path.GetFileNameWithoutExtension(inputFileName) + "_" + timestamp + Path.GetExtension(inputFileName);

            azureBlob.upload_ToBlob(filePath + uploadFileName, "forecastbillingcontainer");
            //azureBlob.download_FromBlob(uploadFileName, "forecastbillingcontainer");

        }

        private void DatatabletoExcel(DataTable dt)
        {

            Workbook book = new Workbook();
            Worksheet sheet = book.Worksheets[0];
            sheet.InsertDataTable(dt, true, 1, 1);
            sheet.Name = "Forecast";
           // book.SaveToFile(resultName, ExcelVersion.Version2010);
           //convert workbook to stream and store to azure 

        }

    }
}
