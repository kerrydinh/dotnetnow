using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetNow.Infrastructure.GoogleAPI
{
    public class DriveManager
    {
        private string _clientId { get; set; }
        private string _clientSecret { get; set; }
        private string[]  _scopes = new string[] { DriveService.Scope.Drive,
                               DriveService.Scope.DriveFile,};
        public DriveManager(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public async Task<DriveService> Authorize()
        {
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
            {
                ClientId = _clientId,
                ClientSecret = _clientSecret
            }, _scopes,
            Environment.UserName, CancellationToken.None, new FileDataStore("MyAppsToken"));

            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "OriSer",

            });
            service.HttpClient.Timeout = TimeSpan.FromMinutes(100);
            return service;
        }

        public async Task<Google.Apis.Drive.v3.Data.File> Upload(DriveService service, byte[] file, FileDetail fileDetail)
        {
            Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
            body.Name = fileDetail.Name;
            body.Description = fileDetail.Description;
            body.MimeType = fileDetail.ContentType;
            MemoryStream stream = new System.IO.MemoryStream(file);
            FilesResource.CreateMediaUpload request = service.Files.Create(body, stream, fileDetail.ContentType);
            request.SupportsTeamDrives = true;
            await request.UploadAsync();

            Permission newPermission = new Permission();
            newPermission.Type = "anyone";
            newPermission.Role = "reader";
            var setPermissionRequest = service.Permissions.Create(newPermission, request.ResponseBody.Id);
            setPermissionRequest.Execute();

            if (setPermissionRequest.FileId != null)
            {
                return request.ResponseBody;
            }

            return null;
        }
    }
}
