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

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using EvenCart.Core;
using EvenCart.Core.Data;
using EvenCart.Core.Infrastructure;
using EvenCart.Data.Entity.Settings;
using EvenCart.Data.Extensions;
using EvenCart.Infrastructure.Mvc;
using EvenCart.Services.HttpServices;
using Microsoft.AspNetCore.Http;

namespace EvenCart.Infrastructure.Middleware
{
    public class GoogleRecaptchaMiddleware
    {
        const string VerificationUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";

        private readonly RequestDelegate _next;
        private readonly IpAddressAccessRegister _accessRegister;
        public GoogleRecaptchaMiddleware(RequestDelegate next)
        {
            _next = next;
            _accessRegister = new IpAddressAccessRegister();
        }

        public async Task Invoke(HttpContext context)
        {
            if (HttpMethods.IsPost(context.Request.Method) && RequestHelper.IsApiCall())
            {
                if (!ValidateCaptcha(context))
                {
                    var dataSerializer = DependencyResolver.Resolve<IDataSerializer>();
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await context.Response.WriteAsync(dataSerializer.Serialize(new
                    {
                        success = false,
                        error = "Unable to verify the request",
                        error_code = ErrorCodes.CaptchaValidationRequired
                    }));
                    return;
                }
            }
            await _next(context);
        }

        private bool IsCaptchaValidationRequired(HttpContext context)
        {
            var ipAllowed = _accessRegister.RecordRequest();
            if (!ipAllowed)
                return true;
            var securitySettings = DependencyResolver.Resolve<SecuritySettings>();
            //trigger captcha if honeypot is trapped
            var honeyPotKey = securitySettings.HoneypotFieldName;
            var isBotSubmission = context.Request.Form.Any(x => x.Key == honeyPotKey && !x.Value.FirstOrDefault().IsNullEmptyOrWhiteSpace());
            return isBotSubmission && securitySettings.EnableCaptcha;
        }

        private bool ValidateCaptcha(HttpContext context)
        {
            if (!IsCaptchaValidationRequired(context))
                return true;
            var captchaResponse = context.Request.Form["g-recaptcha-response"].FirstOrDefault();
            if (captchaResponse.IsNullEmptyOrWhiteSpace())
                return false;
            var securitySettings = DependencyResolver.Resolve<SecuritySettings>();
            var requestProvider = DependencyResolver.Resolve<IRequestProvider>();
            var response = requestProvider.Get<dynamic>(string.Format(VerificationUrl, securitySettings.SiteSecret, captchaResponse));
            return response != null && response.success == "true";
        }
    }

    internal class IpAddressAccessRegister
    {

        #region Private fields

        private static readonly Dictionary<string, short> IpAdresses = new Dictionary<string, short>();
        private static readonly Stack<string> Banned = new Stack<string>();
        private static Timer _timer = CreateTimer();
        private static Timer _bannedTimer = CreateBanningTimer();

        #endregion

        private const int BANNED_REQUESTS = 10;
        private const int REDUCTION_INTERVAL = 1000; // 1 second
        private const int RELEASE_INTERVAL = 5 * 60 * 1000; // 5 minutes

        public bool RecordRequest()
        {
            var ip = WebHelper.GetClientIpAddress();
            if (Banned.Contains(ip))
            {
                return false;
            }

            CheckIpAddress(ip);
            return true;
        }

        /// <summary>
        /// Checks the requesting IP address in the collection
        /// and bans the IP if required.
        /// </summary>
        private static void CheckIpAddress(string ip)
        {
            if (!IpAdresses.ContainsKey(ip))
            {
                IpAdresses[ip] = 1;
            }
            else if (IpAdresses[ip] == BANNED_REQUESTS)
            {
                Banned.Push(ip);
                IpAdresses.Remove(ip);
            }
            else
            {
                IpAdresses[ip]++;
            }
        }

        #region Timers

        /// <summary>
        /// Creates the timer that subtract a request
        /// from the _IpAddress dictionary.
        /// </summary>
        private static Timer CreateTimer()
        {
            var timer = GetTimer(REDUCTION_INTERVAL);
            timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
            return timer;
        }

        /// <summary>
        /// Creates the timer that removes 1 banned IP address
        /// every time the timer is elapsed.
        /// </summary>
        /// <returns></returns>
        private static Timer CreateBanningTimer()
        {
            var timer = GetTimer(RELEASE_INTERVAL);
            timer.Elapsed += delegate
            {
                if (Banned.Count > 0)
                    Banned.Pop();
            };
            return timer;
        }

        /// <summary>
        /// Creates a simple timer instance and starts it.
        /// </summary>
        /// <param name="interval">The interval in milliseconds.</param>
        private static Timer GetTimer(int interval)
        {
            var timer = new Timer {Interval = interval};
            timer.Start();

            return timer;
        }

        /// <summary>
        /// Subtracts a request from each IP address in the collection.
        /// </summary>
        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var keys = IpAdresses.Keys.ToList();
            foreach (var key in keys)
            {
                IpAdresses[key]--;
                if (IpAdresses[key] == 0)
                    IpAdresses.Remove(key);
            }
        }

        #endregion
    }
}