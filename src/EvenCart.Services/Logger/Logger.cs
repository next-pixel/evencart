﻿#region License
// Copyright (c) Sojatia Infocrafts Private Limited.
// The following code is part of EvenCart eCommerce Software (https://evencart.co) Dual Licensed under the terms of
// 
// 1. GNU GPLv3 with additional terms (available to read at https://evencart.co/license)
// 2. EvenCart Proprietary License (available to read at https://evencart.co/license/commercial-license).
// 
// You can select one of the above two licenses according to your requirements. The usage of this code is
// subject to the terms of the license chosen by you.
#endregion

using System;
using EvenCart.Core.Infrastructure;
using EvenCart.Data.Entity.Settings;
using EvenCart.Data.Entity.Users;
using EvenCart.Data.Enum;

namespace EvenCart.Services.Logger
{
    public abstract class Logger : ILogger
    {
        public abstract void Log<T>(LogLevel logLevel, string message, Exception exception = null, User user = null);

        public bool ShouldLog(LogLevel logLevel)
        {
            var systemSettings = DependencyResolver.Resolve<SystemSettings>();
            return systemSettings.MinimumLogLevel < logLevel && systemSettings.MinimumLogLevel != LogLevel.Trace;
        }
    }
}