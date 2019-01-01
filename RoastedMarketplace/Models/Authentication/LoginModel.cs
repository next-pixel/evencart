﻿using FluentValidation;
using RoastedMarketplace.Infrastructure.Mvc.Models;
using RoastedMarketplace.Infrastructure.Mvc.Validator;

namespace RoastedMarketplace.Models.Authentication
{
    public class LoginModel : FoundationModel, IRequiresValidations<LoginModel>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public void SetupValidationRules(ModelValidator<LoginModel> v)
        {
            v.RuleFor(x => x.Email).NotEmpty().EmailAddress();
            v.RuleFor(x => x.Password).NotEmpty();
        }
    }
}