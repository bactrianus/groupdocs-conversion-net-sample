#GroupDocs.Conversion .NET Samples
![Alt text](https://media.licdn.com/media/p/7/005/059/258/39b2da3.png "GroupDocs")

<br/>
This sample solution demonstrate the usage of [GroupDocs.Conversion for .NET library]( http://groupdocs.com/dot-net/document-conversion-library). 


##How-to install/run
1. Download sample sources to any directory.
2. Open "GroupDocs.Conversion for .NET Sample.sln" with Visual Studio
3. Rebuild and run sample project

***

##Project details and configuration

###GroupDocs.Conversion for .NET Sample

The sample demonstrate the following file conversions:
* PDF to HTML
* DOC to PDF
* DOC to JPG
* DOC to PNG with custom conversion options
* DOC to BMP through PDF

###GroupDocs.Conversion.CustomInputDataHandler<br>
This is an example of custom IInputDataHandler. It shows how to use Amazon S3 as file storage without using cache.
<br>

***Configuration***

1. Open **AmazonInputHandler.cs** and change **bucketName** with your file input bucket name<br>
    ` private static string bucketName = ""; //TODO: Put your input bucketname here `
2. Open **App.config** and set your Amazon access and secret keys 
```html
  <appSettings>
    <add key="AWSAccessKey" value=""/>
    <add key="AWSSecretKey" value="" />
  </appSettings>  
```
 
###GroupDocs.Conversion.CustomCacheDataHandler<br>
This is an example of custom IInputDataHandler and ICacheDataHandler. It shows how to use Amazon S3 as file storage and cache storage.

***Configuration***

1. Open **AmazonInputHandler.cs** and change **bucketName** with your file input bucket name<br>
`private static string bucketName = ""; //TODO: Put your input bucketname here`
2. Open **AmazonCacheDataHandler.cs** and set your cache files bucket name<br>
`private static string bucketName = ""; //TODO: Put your cache bucketname here`
3. Set your Amazon access and secret keys in **App.config** 
```html
  <appSettings>
    <add key="AWSAccessKey" value=""/>
    <add key="AWSSecretKey" value="" />
  </appSettings>  
```

***

### Note

If you set a valid license please delete your cache.
