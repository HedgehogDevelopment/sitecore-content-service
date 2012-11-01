/// Credit: http://www.techphoria414.com/Blog/2010/December/Sitecore-Item-Editors-and-MVC

namespace HedgehogDevelopment.Scaas.Web.Framework.Pipelines.HttpRequest
{
    /// <summary>
    /// Custom processor to abort the Sitecore pipline if we have a matching Route
    /// </summary>
    public class SystemWebRoutingResolver : global::Sitecore.Pipelines.HttpRequest.HttpRequestProcessor
    {
        public override void Process(global::Sitecore.Pipelines.HttpRequest.HttpRequestArgs args)
        {
            System.Web.Routing.RouteData routeData = System.Web.Routing.RouteTable.Routes.GetRouteData(new System.Web.HttpContextWrapper(args.Context));
            if (routeData != null)
            {
                args.AbortPipeline();
            }
        }
    }
}