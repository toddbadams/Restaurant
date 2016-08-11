using System.Web.Http;
using System.Web.Http.Filters;
using Newtonsoft.Json.Serialization;
using Owin;
using tba.Core.Filters;

namespace tba.Restaurant.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var webApiConfiguration = new HttpConfiguration
            {
                //LocalOnly (default), Always, Never
                //IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Never 
            };
            ConfigureFilters(webApiConfiguration.Filters);
            // Web API routes
            webApiConfiguration.MapHttpAttributeRoutes();
            app.UseWebApi(webApiConfiguration);
            // Use camel case for JSON data.
            webApiConfiguration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        private static void ConfigureFilters(HttpFilterCollection filters)
        {
            filters.Add(new NotImplExceptionFilterAttribute());
            filters.Add(new EntityDoesNotExistExceptionFilterAttribute());
        }
    }
}