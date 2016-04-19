using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using GroupDocs.Conversion.Config;
using GroupDocs.Conversion.Domain;
using GroupDocs.Conversion.Handler.Input;
using Newtonsoft.Json;

namespace GroupDocs.Conversion.Google
{
    public class GoogleInputHandler : IInputDataHandler
    {
        private static string ClientId = ""; //TODO: Put you Google ClientId 
        private static string ClientSecret = ""; //TODO: Put you Google ClientSecret 
        private readonly DriveService _dataService;

        private readonly ConversionConfig _conversionConfig;

        public GoogleInputHandler(ConversionConfig conversionConfig)
        {
            _conversionConfig = conversionConfig;
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = ClientId,
                    ClientSecret = ClientSecret
                },
                new[] { DriveService.Scope.DriveReadonly },
                "user",
                CancellationToken.None,
                new FileDataStore("GoogleAuth", true));

            _dataService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential.Result
            });
        }
        
        public FileDescription GetFileDescription(string guid)
        {
            var googleDoc = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(Path.Combine(_conversionConfig.StoragePath, guid)));
            string fileId = googleDoc.doc_id;

            var file = _dataService.Files.Get(fileId).Execute();
            
            FileDescription result = new FileDescription();

            result.Guid = file.Id;
            result.Name = file.Name + ".pdf";
            if (file.Size != null) result.Size = file.Size.Value;

            return result;
        }

        public Stream GetFile(string guid)
        {
            var request = _dataService.Files.Export(guid, "application/pdf");
            return request.ExecuteAsStream();
        }
    }
}
