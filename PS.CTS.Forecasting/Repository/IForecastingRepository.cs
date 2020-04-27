using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PS.CTS.Common.Entities;
using System.Data;
using System.IO;

namespace PS.CTS.Forecasting.Repository
{
    public interface IForecastingRepository
    {

        List<ForecastingInfo> GetForecastingInfo(SearchRequest request);

        bool UploadTemplateData(string filePath);
        bool UploadStreamData(Stream fileStream);
        DataTable GetForecastingExcel(SearchRequest searchRequest);

        void UploadFiletoAzure(string fileToUpload, string filePath);
    }
}
