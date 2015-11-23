using System;
using System.IO;
using GroupDocs.Conversion.Config;
using GroupDocs.Conversion.Converter.Option;
using GroupDocs.Conversion.Handler;

namespace GroupDocs.Conversion.CustomInputDataHandler
{
    class Program
    {
        static void Main()
        {
            const string sourceFileName = "sample.doc"; //TODO: Put the source filename here
            const string resultFileName = "result.pdf"; //TODO: Put the result filename here

            // Setup Conversion configuration
            var conversionConfig = new ConversionConfig();
            conversionConfig.SetUseCache(false);

            var inputDataHandler = new AmazonInputDataHandler();
            var conversionHandler = new ConversionHandler(conversionConfig, inputDataHandler);

            var resultStream = conversionHandler.Convert<Stream>(sourceFileName, new PdfSaveOptions());
            WriteStreamToFile(resultStream, resultFileName);
            resultStream.Dispose();

            Console.WriteLine("The conversion finished. Press <<ENTER>> to exit.");
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
