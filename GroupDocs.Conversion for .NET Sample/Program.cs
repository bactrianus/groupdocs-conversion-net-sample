using System;
using System.Collections.Generic;
using System.IO;
using GroupDocs.Conversion.Config;
using GroupDocs.Conversion.Converter.Option;
using GroupDocs.Conversion.Handler;
using GroupDocs.Foundation.Domain;

namespace GroupDocs.Conversion.Net.Sample
{
    public class Program
    {
        private static readonly string RootFolder = Path.GetFullPath("../../AppData");
        private static ConversionHandler _conversionHandler;
        private static readonly string ResultPath = Path.Combine(RootFolder, "ConvertedFiles");

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

            //Convert Doc to PDF and return the path of the converted file
            ConvertDocToPdfReturnPath();

            Console.WriteLine("Conversion complete. Press any key to exit");
            Console.ReadKey();
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

        private static void ConvertPdfToHtml()
        {
            Console.WriteLine("Press any key to convert PDF to HTML ... ");
            Console.ReadKey();
           
            // Convert document
            var result = _conversionHandler.Convert<Stream>("sample.pdf", new HtmlSaveOptions());
            // Write converted stream to file
            WriteStreamToFile(result, "result.html");
        }

        private static void ConvertDocToPdf()
        {
            Console.WriteLine("Press any key to convert DOC to PDF ... ");
            Console.ReadKey();

            // Convert document
            var result = _conversionHandler.Convert<Stream>("sample.doc", new PdfSaveOptions());
            // Write converted stream to file
            WriteStreamToFile(result, "result.pdf");
        }

        private static void ConvertDocToJpg()
        {
            Console.WriteLine("Press any key to convert DOC to JPG ... ");
            Console.ReadKey();

            // Convert document
            var result = _conversionHandler.Convert<IList<Stream>>("sample.doc", new ImageSaveOptions{ ConvertFileType = FileType.Jpg});
            // Write converted stream to file
            var page = 1;
            foreach (var pageStream in result)
            {
                WriteStreamToFile(pageStream, string.Format("result_page{0}.jpg", page));
                pageStream.Dispose();
            }
        }

        private static void ConvertDocToPngWithCustomOptions()
        {
            Console.WriteLine("Press any key to convert DOC to PNG with custom options ... ");
            Console.ReadKey();
            
            // Set image convert options
            var options = new ImageSaveOptions
            {
                ConvertFileType = FileType.Png, // Set the output file format
                Width = 400, // Set the width of the conveted image. If only width or height is set, the image will keep it's aspect ratio
                CustomName = "MyAwesomeFileName", // if cache is used, then the CustomName will be used as cache file name
                UseWidthForCustomName = true, // if cache is used and CustomName is set, the cache file name will contain also the image width
                PageNumber = 1, // Convert starting from
                NumPagesToConvert = 2 // Number of pages to convert
            };

            // Convert document
            var result = _conversionHandler.Convert<IList<Stream>>("sample.doc", options);
            // Write converted stream to file
            var page = 1;
            foreach (var pageStream in result)
            {
                WriteStreamToFile(pageStream, string.Format("result_custom_options_page{0}.png", page));
                pageStream.Dispose();
            }
        }

        private static void ConvertDocToBmpThroughPdf()
        {
            Console.WriteLine("Press any key to convert DOC to BMP through PDF ... ");
            Console.ReadKey();
            
            // Set image convert options
            var options = new ImageSaveOptions
            {
                ConvertFileType = FileType.Bmp, // Set the output file format
                UsePdf = true // the file will be converted to pdf and then from pdf to bmp
            };

            // Convert document
            var result = _conversionHandler.Convert<IList<Stream>>("sample.doc", options);
            // Write converted stream to file
            var page = 1;
            foreach (var pageStream in result)
            {
                WriteStreamToFile(pageStream, string.Format("result_use_pdf_page{0}.bmp", page));
                pageStream.Dispose();
            }
        }

        private static void ConvertDocToPdfReturnPath()
        {
            Console.WriteLine("Press any key to convert DOC to PDF and return the path of the converted file ... ");
            Console.ReadKey();

            // Convert document
            var convertedPath = _conversionHandler.Convert<string>("sample.doc", new PdfSaveOptions());
            Console.WriteLine("Converted file path is: " + convertedPath);
        }
    }
}
