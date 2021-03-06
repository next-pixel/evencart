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

using EvenCart.Infrastructure.Extensions;
using EvenCart.Infrastructure.Mvc.Attributes;
using EvenCart.Infrastructure.Mvc.Models;

namespace EvenCart.Areas.Administration.Models.Reports
{
    [FormatAsCurrencies(nameof(TotalAmount), CurrencyCodeProperty = nameof(CurrencyCode))]
    public class UserOrderReportModel : FoundationModel
    {
        public string Name { get; set; }

        public string Email { get; set; }
        
        public int Id { get; set; }

        public decimal TotalAmount { get; set; }

        public string CurrencyCode { get; set; }

        public int TotalOrders { get; set; }

        public int TotalProducts { get; set; }

        public string TotalAmountFormatted => TotalAmount.ToCurrency();
    }
}