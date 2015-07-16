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

            var converter = conversionHandler.GetPdfConverter(sourceFileName);
            var resultStream = converter.Convert<Stream>(new PdfOptions());
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
