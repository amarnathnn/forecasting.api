using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using PS.CTS.Common.Entities;
using PS.CTS.Common.Interface;
using PS.CTS.Common.Utility;
using PS.CTS.Forecasting.Repository;

namespace PS.CTS.Forecasting.Domain
{
    public class ForecastBilling : IForecastBilling
    {
        private readonly IForecastingRepository _forecastingRepo;

        public ForecastBilling(IForecastingRepository forecastingRepository)
        {
            _forecastingRepo = forecastingRepository;
        }
        public List<ForecastingInfo> GetForecasting(SearchRequest searchRequest)
        {
            return _forecastingRepo.GetForecastingInfo(searchRequest);
        }

        public DataTable GetForecastingExcel(SearchRequest searchRequest)
        {
            return _forecastingRepo.GetForecastingExcel(searchRequest); 
        }

        public bool UploadTemplateData(string filePath)
        {
            return _forecastingRepo.UploadTemplateData(filePath);
        }
        public bool UploadStreamData(System.IO.Stream fileStream)
        {
            return _forecastingRepo.UploadStreamData(fileStream);
        }

        public void UploadFiletoAzure(string fileToUpload,string filePath)
        {
            _forecastingRepo.UploadFiletoAzure(fileToUpload, filePath);
        }
    }
}
