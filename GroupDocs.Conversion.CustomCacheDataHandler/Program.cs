using System;
using System.IO;
using GroupDocs.Conversion.Config;
using GroupDocs.Conversion.Converter.Option;
using GroupDocs.Conversion.Handler;

namespace GroupDocs.Conversion.CustomCacheDataHandler
{
    class Program
    {
        static void Main()
        {
            const string sourceFileName = "sample.doc"; //TODO: Put the source filename here
            const string resultFileName = "result.pdf"; //TODO: Put the result filename here

            // Setup Conversion configuration
            var conversionConfig = new ConversionConfig
            {
                CachePath = "cache",
                StoragePath = "."
            };
            conversionConfig.SetUseCache(true);

            var inputDataHandler = new AmazonInputDataHandler();
            var cacheDataHandler = new AmazonCacheDataHandler(conversionConfig);
            var conversionHandler = new ConversionHandler(conversionConfig, inputDataHandler, cacheDataHandler);

            var fileDescription  = inputDataHandler.GetFileDescription(sourceFileName);

            var converter = conversionHandler.GetToPdfConverter(fileDescription, new PdfOptions());
            var resultStream = converter.Convert();
            using (var file = new FileStream(resultFileName, FileMode.Create))
            {
                var buffer = new byte[16384];
                int read;
                while ((read = resultStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    file.Write(buffer, 0, read);
                }
            }
            resultStream.Dispose();

            Console.WriteLine("The conversion finished. Press <<ENTER>> to exit.");
            Console.ReadLine();
        }
    }
}
