using PS.CTS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PS.CTS.Common.Interface
{
    public interface IForecastBilling
    {
        List<ForecastingInfo> GetForecasting(SearchRequest searchRequest);

        DataTable GetForecastingExcel(SearchRequest searchRequest);

        bool UploadTemplateData(string filePath);
        bool UploadStreamData(Stream filePath);

        void UploadFiletoAzure(string fileToUpload, string filePath);
    }
}
