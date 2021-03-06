﻿using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
using GroupDocs.Conversion.Domain;
using GroupDocs.Conversion.Handler.Input;

namespace GroupDocs.Conversion.CustomOutputDataHandler
{
    public class AmazonInputDataHandler: IInputDataHandler
    {
        private static string bucketName = ""; //TODO: Put you bucketname here
        private readonly AmazonS3Client _client;

        public AmazonInputDataHandler()
        {
            _client = new AmazonS3Client(RegionEndpoint.EUWest1);
        }

        public FileDescription GetFileDescription(string guid)
        {
            FileDescription result = new FileDescription();

            S3FileInfo fileInfo = new S3FileInfo(_client, bucketName, guid);


            result.Guid = guid;
            result.Name = fileInfo.Name;
            result.Size = fileInfo.Length;
            result.LastModified = fileInfo.LastWriteTimeUtc.Ticks;

            return result;
        }

        public Stream GetFile(string guid)
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = guid
            };

            var result = new MemoryStream();
            using (var response = _client.GetObject(request))
            {
                byte[] buffer = new byte[16384]; //16*1024
                int read;
                while ((read = response.ResponseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    result.Write(buffer, 0, read);
                }

            }
            return result;
        }
    }
}
