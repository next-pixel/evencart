﻿using System;
using System.Collections.Generic;
using System.Linq;
using EvenCart.Core.Infrastructure;
using EvenCart.Data.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace EvenCart.Infrastructure.Routing
{
    public static class RouteFinder
    {
        private static IList<RouteInfo> _routes;
        public static IList<RouteInfo> GetAllRoutes(string controller = null, string area = null)
        {
            if (_routes == null)
            {
                var actionDescriptorCollectionProvider = DependencyResolver.Resolve<IActionDescriptorCollectionProvider>();
                _routes = actionDescriptorCollectionProvider.ActionDescriptors.Items
                    .Where(x => x.AttributeRouteInfo?.Name != null && !x.AttributeRouteInfo.Name.StartsWith(ApplicationConfig.ApiEndpointName))
                    .Select(x => new RouteInfo() {
                        Action = x.RouteValues["Action"],
                        Controller = x.RouteValues["Controller"],
                        Area = x.RouteValues["Area"],
                        Name = x.AttributeRouteInfo.Name,
                        Template = x.AttributeRouteInfo.Template
                    })
                    .ToList();
            }

            var result = !controller.IsNullEmptyOrWhiteSpace()
                ? _routes.Where(x => x.Controller.Equals(controller, StringComparison.InvariantCultureIgnoreCase))
                    .ToList()
                : _routes;

            result = result.Where(x => x.Area == area).ToList();
            return result;
        }
    }
}