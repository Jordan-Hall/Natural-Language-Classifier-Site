using System;
using JordanHall.ClassifierService;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using JordanHall.ClassifierService.Models;
using WebGrease.Css.Extensions;

namespace JordanHall.Ibm.Nlc.SiteApi.Controllers
{
    public class ClassifierController : ApiController
    {
        private readonly IIbmClasifierService clasifierService;

        public ClassifierController(IIbmClasifierService clasifierService)
        {
            this.clasifierService = clasifierService;
        }
        
        public async Task<IEnumerable<Classifier>> Get()
        {
            return await clasifierService.GetClassifiers(default(CancellationToken));
        }
        
        public async Task<Classifier> Get(string id)
        {
            return await clasifierService.GetClassifierInformation(id, default(CancellationToken));
        }

        public async Task<ClassifyResponse> Get(ClassifyRequest classifyRequest)
        {
            return await clasifierService.PostQuery(classifyRequest, default(CancellationToken));
        }

        // POST: api/Classifier
        public async Task<StatusCodeResult> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var file in provider.FileData)
                {
                    var trainingRequest = new TrainingRequest()
                    {
                        FileBytes = File.ReadAllBytes(file.LocalFileName),
                        FileName = Path.GetFileName(file.LocalFileName)
                    };
                    await clasifierService.PostTrainingData(trainingRequest, default(CancellationToken));
                };
                    
                return StatusCode(HttpStatusCode.Accepted);
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE: api/Classifier/5
        public async void Delete(string id)
        {
            await clasifierService.DeleteClassifier(id, default(CancellationToken));
        }
    }
}
