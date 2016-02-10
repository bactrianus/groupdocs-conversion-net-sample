using System;
using System.IO;
using GroupDocs.Conversion.Config;
using GroupDocs.Conversion.Converter.Option;
using GroupDocs.Conversion.Handler;

namespace GroupDocs.Conversion.CustomOutputDataHandler
{
    class Program
    {
        static void Main()
        {
            const string sourceFileName = "sample.doc"; //TODO: Put the source filename here

            // Setup Conversion configuration
            var conversionConfig = new ConversionConfig
            {
                OutputPath = "result",
                StoragePath = "."
            };
            conversionConfig.UseCache = true;

            var inputDataHandler = new AmazonInputDataHandler();
            var outputDataHandler = new AmazonOutputDataHandler(conversionConfig);
            var conversionHandler = new ConversionHandler(conversionConfig, inputDataHandler, outputDataHandler);

            var resultPath = conversionHandler.Convert<string>(sourceFileName, new PdfSaveOptions { OutputType = OutputType.String });

            Console.WriteLine("The conversion finished. The result can be located here: {0}. Press <<ENTER>> to exit.",  resultPath);
            Console.ReadLine();
        }

        private static void WriteStreamToFile(Stream stream, string fileName)
        {
            using (var file = new FileStream(fileName, FileMode.Create))
            {
                var buffer = new byte[16384];
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    file.Write(buffer, 0, read);
                }
            }
        }
    }
}
