using System;
using System.IO;
using GroupDocs.Conversion.Config;
using GroupDocs.Conversion.Converter.Option;
using GroupDocs.Conversion.Handler;

namespace GroupDocs.Conversion.Google
{
    class Program
    {
        private static readonly string RootFolder = Path.GetFullPath("../../AppData");
        private static readonly string ResultPath = Path.Combine(RootFolder, "ConvertedFiles");

        static void Main(string[] args)
        {
            var storagePath = Path.Combine(RootFolder, "TestFiles");

            // Set license
            License license = new License();
            license.SetLicense("");

            // Setup Conversion configuration
            var conversionConfig = new ConversionConfig
            {
                StoragePath = storagePath,
                OutputPath = ResultPath,
                UseCache = false
            };

            var inputDataHandler = new GoogleInputHandler(conversionConfig);
            var conversionHandler = new ConversionHandler(conversionConfig, inputDataHandler);

            var resultStream = conversionHandler.Convert<Stream>("document.gdoc", new WordsSaveOptions());
            WriteStreamToFile(resultStream, "result.docx");
            resultStream.Dispose();

            Console.WriteLine("The conversion finished. Press <<ENTER>> to exit.");
            Console.ReadLine();

        }

        private static void WriteStreamToFile(Stream stream, string fileName)
        {
            if (!Directory.Exists(ResultPath))
            {
                Directory.CreateDirectory(ResultPath);
            }
            stream.Position = 0;
            using (
                var fileStream = new FileInfo(Path.Combine(ResultPath, fileName)).Open(FileMode.Create,
                    FileAccess.Write))
            {
                var buffer = new byte[16384];
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, read);
                }
            }
        }
    }
}
