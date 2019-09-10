﻿using System;
using Microsoft.AspNetCore.Mvc;

namespace EvenCart.Infrastructure.Routing
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DynamicRouteAttribute : HttpGetAttribute
    {
        /// <summary>
        /// The setting name under UrlSettings group that contains the dynamic route template
        /// </summary>
        public string SettingName { get; set; }

        /// <summary>
        /// The entity type that the parameter to the action uses
        /// </summary>
        public string SeoEntityName { get; set; }

        /// <summary>
        /// The suffix that'll be added to the dynamic template. Default is blank.
        /// </summary>
        public string TemplateSuffix { get; set; } = "";

        /// <summary>
        /// The prefix that'll be added to the dynamic template. Default is blank.
        /// </summary>
        public string TemplatePrefix { get; set; } = "";
        /// <summary>
        /// The url template to be used. If this is set, the SettingName and TemplateSuffix and Prefix are ignored
        /// </summary>
        public string DynamicTemplate { get; set; }

        /// <summary>
        /// The parameter name passed to the method. Default is id
        /// </summary>
        public string ParameterName { get; set; } = "id";
    }
}