using Microsoft.AspNetCore.Mvc;
using PS.CTS.Common.Entities;
using PS.CTS.Common.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace PS.CTS.Forecasting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForecastingController : ControllerBase
    {
        private readonly IForecastBilling _forecastBilling;
        public ForecastingController(IForecastBilling forecastBilling)
        {
            _forecastBilling = forecastBilling;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // GET api/values/5
        [HttpPost]
        [Route("getforecastingdetails")]
        public IActionResult GetForecastingDetails(SearchRequest request)
        {
            try
            {
                var resp = _forecastBilling.GetForecasting(request);

                return StatusCode((int)HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // GET api/values/5
        [HttpPost]
        [Route("getforecastingdetailsexcel")]
        public IActionResult GetForecastingDetailsExcel(SearchRequest request)
        {
            try
            {
                var resp = _forecastBilling.GetForecastingExcel(request);

                return StatusCode((int)HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // POST api/values
        [HttpPost, Route("uploadinfo")]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                //var folderName = Path.Combine("Sourcefiles", "Forecasting");


                var pathToSave = Path.Combine("C:\\ForecastBilling\\Sourcefiles", "Forecasting");
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(pathToSave, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    _forecastBilling.UploadTemplateData(fullPath);

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        // POST api/values
        [HttpPost("uploadFiles")]
        public IActionResult UploadExcel()
        {
            try
            {
                var files = Request.Form.Files;
                long size = files.Sum(f => f.Length);

                // full path to file in temp location
                //var filePath = Path.GetTempFileName();

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        using (var fileStram = formFile.OpenReadStream())
                        {

                            _forecastBilling.UploadStreamData(fileStram);
                        }
                    }
                }

                // process uploaded files
                // Don't rely on or trust the FileName property without validation.

                //return Ok(new { count = files.Count, size });
                return Ok(new { count = files.Count, size });

            }
            catch (Exception ex)
            {
                //return StatusCode(500, $"Internal server error: {ex}");
                throw ex;
            }
        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
