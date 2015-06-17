using System;
using Amazon;
using Amazon.S3;
using Amazon.S3.IO;
using GroupDocs.Conversion.Config;
using GroupDocs.Conversion.Converter.Option;
using GroupDocs.Conversion.Domain;
using GroupDocs.Conversion.Handler.Cache;
using GroupDocs.Foundation.Utils.Wrapper.Stream;

namespace GroupDocs.Conversion.CustomCacheDataHandler
{
    public class AmazonCacheDataHandler: ICacheDataHandler
    {
        private static string bucketName = ""; //TODO: Put you bucketname here 
        private readonly ConversionConfig _conversionConfig;
        private readonly AmazonS3 _client;

        public AmazonCacheDataHandler(ConversionConfig conversionConfig)
        {
            _conversionConfig = conversionConfig;
            _client = new AmazonS3Client(RegionEndpoint.EUWest1);
        }

        
        public bool Exists(CacheFileDescription cacheFileDescription)
        {
            if (!_conversionConfig.IsUseCache())
            {
                return false;
            }

            if (cacheFileDescription == null)
            {
                throw new System.Exception("CacheFileDescription is not set");
            }

            if (cacheFileDescription.LastModified == 0)
            {
                return false;
            }

            if (string.IsNullOrEmpty(cacheFileDescription.Guid))
            {
                throw new System.Exception("CacheFileDescription is not set");
            }


            if (string.IsNullOrEmpty(_conversionConfig.StoragePath))
            {
                throw new System.Exception("Storage path is not set");
            }


            var key = GetCachePath(_conversionConfig.CachePath, cacheFileDescription);
            S3FileInfo fileInfo = new S3FileInfo(_client, bucketName, key);
            if (!fileInfo.Exists)
            {
                return false;
            }
            return (fileInfo.LastWriteTimeUtc >= DateTime.UtcNow.AddMinutes(-5));
        }

        public GroupDocsInputStream GetInputStream(CacheFileDescription cacheFileDescription)
        {
            if (cacheFileDescription == null || String.IsNullOrEmpty(cacheFileDescription.Guid) ||
                cacheFileDescription.LastModified == 0)
            {
                throw new System.Exception("CacheFileDescription is not set");
            }

            var key = GetCachePath(_conversionConfig.CachePath, cacheFileDescription);
            var fileInfo = new S3FileInfo(_client, bucketName, key);

            if (!fileInfo.Exists)
            {
                throw new System.Exception("File not found");
            }

            return new GroupDocsInputStream(fileInfo.OpenRead());

        }

        public GroupDocsOutputStream GetOutputSaveStream(CacheFileDescription cacheFileDescription)
        {
            try
            {
                if (!_conversionConfig.IsUseCache())
                {
                    return new GroupDocsOutputStream();
                }

                if (cacheFileDescription == null || String.IsNullOrEmpty(cacheFileDescription.Guid))
                {
                    throw new System.Exception("CacheFileDescription is not set");
                }

                string key = GetCachePath(_conversionConfig.CachePath, cacheFileDescription);
                S3FileInfo fileInfo = new S3FileInfo(_client, bucketName, key);
                return new GroupDocsOutputStream(fileInfo.Create());
            }
            catch (System.Exception e)
            {
                throw new System.Exception(e.Message);
            }
        }

        private string GetCachePath(string path, CacheFileDescription cacheFileDescription)
        {
            if (cacheFileDescription.Options == null)
            {
                throw new System.Exception("CacheFileDescription.Options is not set");
            }
            string filePath;
            string fileName;

            if (cacheFileDescription.Options is ImageOptions)
            {
                if (!string.IsNullOrEmpty(cacheFileDescription.Options.CustomName))
                {
                    if (((ImageOptions)cacheFileDescription.Options).UseWidthForCustomName)
                    {
                        fileName = string.Format("{0}_{1}.{2}", cacheFileDescription.Options.CustomName,
                            ((ImageOptions)cacheFileDescription.Options).Width,
                            cacheFileDescription.Options.ConvertFileType.ToString().ToLower());
                    }
                    else
                    {
                        fileName = string.Format("{0}.{1}", cacheFileDescription.Options.CustomName,
                            cacheFileDescription.Options.ConvertFileType.ToString().ToLower());
                    }
                }
                else
                {
                    fileName = string.Format("{0}.{1}", cacheFileDescription.BaseName,
                            cacheFileDescription.Options.ConvertFileType.ToString().ToLower());
                }
                filePath = string.Format(@"{0}\{1}\{2}\{3}", path, cacheFileDescription.Guid,
                    cacheFileDescription.Options.PageNumber, fileName);
            }
            else
            {
                fileName = !string.IsNullOrEmpty(cacheFileDescription.Options.CustomName)
                ? string.Format("{0}.{1}", cacheFileDescription.Options.CustomName, cacheFileDescription.Options.ConvertFileType.ToString().ToLower())
                : string.Format("{0}.{1}", cacheFileDescription.BaseName, cacheFileDescription.Options.ConvertFileType.ToString().ToLower());

                filePath = string.Format(@"{0}\{1}\{2}",path, cacheFileDescription.Guid, fileName);
            }
            return filePath;
        }
    }
}
