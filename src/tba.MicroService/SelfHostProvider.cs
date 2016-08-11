using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Filters;
using Microsoft.Owin.Hosting;
using Owin;
using tba.Core.Filters;

namespace tba.MicroService
{
    public class SelfHostProvider : IDisposable
    {
        private readonly StartOptions _options;
        private IDisposable _server;

        public SelfHostProvider(StartOptions options)
        {
            _options = options;
        }

        public void Dispose()
        {
            _server.Dispose();
        }

        public void Connect()
        {
            _server = WebApp.Start<StartUp>(_options);
        }
    }

    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            ConfigureFormatters(config.Formatters);
            ConfigureFilters(config.Filters);
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }

        private static void ConfigureFormatters(ICollection<MediaTypeFormatter> formatters)
        {
            formatters.Clear();
            formatters.Add(new JsonMediaTypeFormatter());
        }

        private static void ConfigureFilters(HttpFilterCollection filters)
        {
            filters.Clear();
            filters.Add(new NotImplExceptionFilterAttribute());
            filters.Add(new EntityDoesNotExistExceptionFilterAttribute());
        }
    }
}
