﻿using RoastedMarketplace.Core.Infrastructure.Routing;

namespace RoastedMarketplace.Core.Plugins
{
    public interface IPlugin
    {
        /// <summary>
        /// Override this method to include operations that you wish to be perform on module installation
        /// </summary>
        void Install();

        /// <summary>
        /// Override this method to include operations that you wish to be perform on module uninstallation
        /// </summary>
        void Uninstall();

        /// <summary>
        /// Gets the configuration page route data
        /// </summary>
        /// <returns></returns>
        RouteData GetConfigurationPageRouteData();

        /// <summary>
        /// Gets the display page route data
        /// </summary>
        /// <returns></returns>
        RouteData GetDisplayPageRouteData();
    }
}