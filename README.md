<img src="https://www.hhog.com/-/media/PublicImages/Hedgehog/Hedgehog-logo-4color-275x46.jpg" alt="Hedgehog Development" border="0"> 

================================

What is Sitecore Content as a Service?
--------------------------------
Sitecore Content as a Service is a concept that has been used on various projects as a way to get content out of Sitecore and into everywhere else it is needed. 

This code introduces an abstraction of the Sitecore CMS, a convention based mapping framework as well as a 'REST' API inside of the Sitecore web site. Three wrappers to the REST API are also provided. The JavaScript, iOS, and .NET APIs allow developers to write external applications that consume Sitecore content.

This code was presented at Sitecore Symposium 2012 (North America) by [Sean Kearney](http://seankearney.com) and should be considered protoype quality.

Blog post about this feature: [http://seankearney.com/post/Sitecore-Content-as-a-Service](http://seankearney.com/post/Sitecore-Content-as-a-Service) 

Known Issues
------------
- The code is functional, but it should be given some additional work to support things that would be expected in production code such as exception handling, logging, as well as proper use of a dependency injection container. 
- Not all Sitecore field types are supported. Examples have been provided for some of the abstractions and converters to the abstractions. Further work is required to create the remaining field types and converters.

Instructions
------------
* It is recomended that you work outside of the web root. 
* It is assumed you are working with a clean `Sitecore 6.6.0.120622` running at http://sitecore.local
* [Team Development for Sitecore](http://TeamDevelopmentForSitecore.com) version 4.0 is required for code generation, code deployment and item deployment.
* ASP.NET MVC 4 is required as this solution is using the [ASP.NET Web API](http://www.asp.net/web-api)

1. Download this code to some location outside of your Sitecore application. 
2. Configure the TDS.Master project to point to your Sitecore web site URL and file system.
3. Deploy the TDS.Master project
  
At this point your previously clean Sitecore install should now have the required code, configuration and items. You should publish your site (or put your site in live mode).

If you are not running Sitecore at the http://sitecore.local then you will need to modify the client APIs (to test the SPA and WinForm app):

* `sitecore-content-service\Applications\SinglePageApp\js\single-page-app.js`
* `sitecore-content-service\Applications\WinFormClient\App.config`

If you are not running a clean `Sitecore 6.6.0.120622` then:

1. Remove the web.config file from the `Web` project.
2. Look at the included web.config.patch file and make the required changes to your web.config file.

Note: 
If changes are made to the templates you must make sure you have [TDS Code Generation extensions](https://github.com/HedgehogDevelopment/tds-codegen) setup properly.

Usage
------------
The route is configured to work on seven actions and is defined as follows:
    
    http://[host name]/api/{action}/{*key}

The seven actions currently supported are:
- `item`
- `items`
- `parent`
- `ancestors`
- `children`
- `descendants`
- `referrers`

The `key` value will currently support a Sitecore item's path or ID.

Example Calls
-------------
Get a specific item

    http://[host name]/api/item/sitecore/content/Scaas-Demo/Content/Articles/About-The-Session

Equivalent call using the ID would be
    
    http://[host name]/api/item/9A753B49-0ADB-4AAA-A4B2-111D6104336D

Fetch all children of the Articles repository:
    
    http://[host name]/api/item/sitecore/content/Scaas-Demo/Content/Articles

The rest of the actions follow the same convention except if you require multiple items. Multiple items are called where the 'key' is a comma separated list of IDs
    
    http://[host name]/api/item/9A753B49-0ADB-4AAA-A4B2-111D6104336D,C8CEA945-6643-44EF-9AF9-8711688D68E1

Example JSON Result
-------------
This call `http://[host name]/api/item/sitecore/content/Scaas-Demo/Content/Articles/About-The-Session` yields:
    
    {
        "$type": "HedgehogDevelopment.Scaas.Models.ArticleItem, HedgehogDevelopment.Scaas.Models",
        "ArticleCopy": "html body here...",
        "ArticleDate": "2012-10-24T11:15:00",
        "ArticleImage": {
            "$type": "HedgehogDevelopment.Scaas.Content.Fields.ImageField, HedgehogDevelopment.Scaas.Content",
            "Key": "/sitecore/media library/Images/SPA/SCAAS",
            "Path": "~/media/Images/SPA/SCAAS.jpg",
            "FileSize": 10108,
            "Extension": "jpg",
            "Title": "",
            "Height": 219,
            "Width": 300,
            "AlternateText": "Sitecore Content as a Service",
            "HasValue": true
        },
        "ArticleTitle": "Welcome to Sitecore Content as a Service",
        "Key": "/sitecore/content/Scaas-Demo/Content/Articles/About-The-Session",
        "Name": "About-The-Session",
        "Path": "/Scaas-Demo/Content/Articles/About-The-Session"
    }