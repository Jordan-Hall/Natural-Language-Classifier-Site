using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using JordanHall.IbmClassifierService;
using JordanHall.IbmClassifierService.Models;

namespace Classifier.Controllers
{
    public class ClassifierController : ApiController
    {
        private readonly IIbmClasifierService clasifierService;

        public ClassifierController(IIbmClasifierService clasifierService)
        {
            this.clasifierService = clasifierService;
        }
        
        public async Task<ClassifierList> Get()
        {
            return await clasifierService.GetClassifiers(default(CancellationToken));
        }
        
        public async Task<JordanHall.IbmClassifierService.Models.Classifier> Get(string id)
        {
            return await clasifierService.GetClassifierInformation(id, default(CancellationToken));
        }
        [Route("api/Classifier/search/{id}/{query}")]
        public async Task<ClassifyResponse> GetSearch(string id, string query)
        {
            return await clasifierService.PostQuery(new ClassifyRequest() { ClassifierId = id, Query = query}, default(CancellationToken));
        }
        
        public async Task<HttpResponseMessage> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                var trainingRequest = new TrainingRequestModel()
                {
                    FileBytes = File.ReadAllBytes(provider.FileData.FirstOrDefault()?.LocalFileName),
                    Name = provider.FormData.Get("name"),
                    Language = provider.FormData.Get("language"),
                };
                return Request.CreateResponse(HttpStatusCode.OK, await clasifierService.PostTrainingData(trainingRequest, default(CancellationToken)));
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> Delete(string id)
        {
            var deleted = await clasifierService.DeleteClassifier(id, default(CancellationToken));
            return deleted
                ? new HttpResponseMessage(HttpStatusCode.OK)
                : new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }
}
