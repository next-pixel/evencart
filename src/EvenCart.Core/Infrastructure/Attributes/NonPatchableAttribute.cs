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

namespace EvenCart.Core.Infrastructure.Attributes
{
    /// <summary>
    /// Specifies that current field shouldn't be patched with patch request
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NonPatchableAttribute : Attribute
    {
        /// <summary>
        /// The role names which are not allowed to patch
        /// </summary>
        public string[] RoleNames { get; set; }
        /// <summary>
        /// Creates a non patchable attribute to identify fields which shouldn't be patched by any user
        /// </summary>
        public NonPatchableAttribute()
        {
            RoleNames = new string[] {};
        }
        /// <summary>
        /// Creates a non patchable attribute to identify fields which can not be patched by the users with specified roles
        /// </summary>
        /// <param name="roleNames"></param>
        public NonPatchableAttribute(params string[] roleNames)
        {
            RoleNames = roleNames;
        }

    }
}