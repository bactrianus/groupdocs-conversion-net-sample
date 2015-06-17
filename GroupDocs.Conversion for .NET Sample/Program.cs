using System;
using System.IO;
using GroupDocs.Conversion.Config;
using GroupDocs.Conversion.Converter.Option;
using GroupDocs.Conversion.Domain;
using GroupDocs.Conversion.Handler;
using GroupDocs.Foundation.Domain;

namespace GroupDocs.Conversion.Net.Sample
{
    public class Program
    {
        private static readonly string RootFolder = Path.GetFullPath("../../AppData");
        private static ConversionHandler _conversionHandler;

        static void Main()
        {
            var storagePath = Path.Combine(RootFolder, "TestFiles");
            var cachePath = Path.Combine(RootFolder, "Cache");

            // Setup Conversion configuration
            var conversionConfig = new ConversionConfig {StoragePath = storagePath, CachePath = cachePath};
            conversionConfig.SetUseCache(true);

            _conversionHandler = new ConversionHandler(conversionConfig);

            // Set license
            //_conversionHandler.SetLicense();

            // Convert Pdf To Html
            ConvertPdfToHtml();
     
            // Convert Doc to Pdf
            ConvertDocToPdf();

            // Convert Doc to Jpg
            ConvertDocToJpg();

            // Convert Doc to Jpg with custom options
            ConvertDocToPngWithCustomOptions();

            // Convert Doc to Bmp through Pdf
            ConvertDocToBmpThroughPdf();

            Console.WriteLine("Conversion complete. Press any key to exit");
            Console.ReadKey();
        }


        private static void ConvertPdfToHtml()
        {
            Console.WriteLine("Press any key to convert PDF to HTML ... ");
            Console.ReadKey();

            var resultPath = Path.Combine(RootFolder, "ConvertedFiles");
            const string fileName = @"Sample.pdf";

            var fileInfo = new FileInfo(Path.Combine(_conversionHandler.Config.StoragePath, fileName));
            var fileDescription = new FileDescription { Guid = fileInfo.Name, Name = fileInfo.Name, LastModified = fileInfo.CreationTime.Ticks };

            // Get Html converter and convert
            var htmlConverter = _conversionHandler.GetToHtmlConverter(fileDescription, new HtmlOptions());
            var result = htmlConverter.Convert();

            // Write converted stream to file
            result.Position = 0;
            using (var stream = new FileInfo(Path.Combine(resultPath, "result.html")).Open(FileMode.Create, FileAccess.Write))
            {
                var buffer = new byte[16384];
                int read;
                while ((read = result.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stream.Write(buffer, 0, read);
                }
            }
        }

        private static void ConvertDocToPdf()
        {
            Console.WriteLine("Press any key to convert DOC to PDF ... ");
            Console.ReadKey();

            var resultPath = Path.Combine(RootFolder, "ConvertedFiles");
            const string fileName = @"sample.doc";

            var fileInfo = new FileInfo(Path.Combine(_conversionHandler.Config.StoragePath, fileName));
            var fileDescription = new FileDescription { Guid = fileInfo.Name, Name = fileInfo.Name, LastModified = fileInfo.CreationTime.Ticks };

            // Get Pdf converter and convert
            var htmlConverter = _conversionHandler.GetToPdfConverter(fileDescription, new PdfOptions());
            var result = htmlConverter.Convert();

            // Write converted stream to file
            result.Position = 0;
            using (var stream = new FileInfo(Path.Combine(resultPath, "result.pdf")).Open(FileMode.Create, FileAccess.Write))
            {
                var buffer = new byte[16384];
                int read;
                while ((read = result.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stream.Write(buffer, 0, read);
                }
            }
        }

        private static void ConvertDocToJpg()
        {
            Console.WriteLine("Press any key to convert DOC to JPG ... ");
            Console.ReadKey();

            var resultPath = Path.Combine(RootFolder, "ConvertedFiles");
            const string fileName = @"sample.doc";

            var fileInfo = new FileInfo(Path.Combine(_conversionHandler.Config.StoragePath, fileName));
            var fileDescription = new FileDescription { Guid = fileInfo.Name, Name = fileInfo.Name, LastModified = fileInfo.CreationTime.Ticks };

            // Get Image converter and convert
            var htmlConverter = _conversionHandler.GetToImageConverter(fileDescription, new ImageOptions{ ConvertFileType = FileType.Jpg});
            var result = htmlConverter.Convert();

            // Write converted stream to file
            var page = 1;
            foreach (var pageStream in result)
            {
                pageStream.Position = 0;
                using (var stream = new FileInfo(Path.Combine(resultPath, String.Format("result_page{0}.jpg", page))).Open(FileMode.Create, FileAccess.Write))
                {
                    var buffer = new byte[16384];
                    int read;
                    while ((read = pageStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        stream.Write(buffer, 0, read);
                    }
                }
                page++;
            }
        }

        private static void ConvertDocToPngWithCustomOptions()
        {
            Console.WriteLine("Press any key to convert DOC to PNG with custom options ... ");
            Console.ReadKey();

            var resultPath = Path.Combine(RootFolder, "ConvertedFiles");
            const string fileName = @"sample.doc";

            var fileInfo = new FileInfo(Path.Combine(_conversionHandler.Config.StoragePath, fileName));
            var fileDescription = new FileDescription { Guid = fileInfo.Name, Name = fileInfo.Name, LastModified = fileInfo.CreationTime.Ticks };

            // Set image convert options
            var options = new ImageOptions
            {
                ConvertFileType = FileType.Png, // Set the output file format
                Width = 400, // Set the width of the conveted image. If only width or height is set, the image will keep it's aspect ratio
                CustomName = "MyAwesomeFileName", // if cache is used, then the CustomName will be used as cache file name
                UseWidthForCustomName = true, // if cache is used and CustomName is set, the cache file name will contain also the image width
                PageNumber = 1, // Convert starting from
                NumPagesToConvert = 2 // Number of pages to convert
            };

            // Get Image converter and convert
            var htmlConverter = _conversionHandler.GetToImageConverter(fileDescription, options);
            var result = htmlConverter.Convert();

            // Write converted stream to file
            var page = 1;
            foreach (var pageStream in result)
            {
                pageStream.Position = 0;
                using (var stream = new FileInfo(Path.Combine(resultPath, String.Format("result_custom_options_page{0}.png", page))).Open(FileMode.Create, FileAccess.Write))
                {
                    var buffer = new byte[16384];
                    int read;
                    while ((read = pageStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        stream.Write(buffer, 0, read);
                    }
                }
                page++;
            }
        }

        private static void ConvertDocToBmpThroughPdf()
        {
            Console.WriteLine("Press any key to convert DOC to BMP through PDF ... ");
            Console.ReadKey();

            var resultPath = Path.Combine(RootFolder, "ConvertedFiles");
            const string fileName = @"sample.doc";

            var fileInfo = new FileInfo(Path.Combine(_conversionHandler.Config.StoragePath, fileName));
            var fileDescription = new FileDescription { Guid = fileInfo.Name, Name = fileInfo.Name, LastModified = fileInfo.CreationTime.Ticks };

            // Set image convert options
            var options = new ImageOptions
            {
                ConvertFileType = FileType.Bmp, // Set the output file format
                UsePdf = true // the file will be converted to pdf and then from pdf to bmp
            };

            // Get Image converter and convert
            var htmlConverter = _conversionHandler.GetToImageConverter(fileDescription, options);
            var result = htmlConverter.Convert();

            // Write converted stream to file
            var page = 1;
            foreach (var pageStream in result)
            {
                pageStream.Position = 0;
                using (var stream = new FileInfo(Path.Combine(resultPath, String.Format("result_use_pdf_page{0}.bmp", page))).Open(FileMode.Create, FileAccess.Write))
                {
                    var buffer = new byte[16384];
                    int read;
                    while ((read = pageStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        stream.Write(buffer, 0, read);
                    }
                }
                page++;
            }
        }
    }
}
