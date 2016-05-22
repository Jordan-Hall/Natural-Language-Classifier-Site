﻿using System.Web.Http;
using JordanHall.ClassifierService;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;

namespace JordanHall.Ibm.Nlc.SiteApi
{
    public class IocConfig
    {
        public static void Register()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

            // Register your types, for instance using the scoped lifestyle:
            container.Register<IIbmClasifierService, IbmClasifierService>(Lifestyle.Scoped);

            // This is an extension method from the integration package.
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}