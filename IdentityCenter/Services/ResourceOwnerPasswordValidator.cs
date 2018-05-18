using IdentityCenter.Models;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityCenter.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var result = await _signInManager.PasswordSignInAsync(context.UserName, context.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                context.Result = new GrantValidationResult(
                                    subject: context.UserName,
                                    authenticationMethod: "custom");
            }
            else
            {
                context.Result = new GrantValidationResult(
                                    TokenRequestErrors.InvalidGrant,
                                    "invalid custom credential");
            }
        }
    }
}
