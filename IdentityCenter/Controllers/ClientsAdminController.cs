using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityCenter.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ClientsAdminController : Controller
    {
        private readonly ConfigurationDbContext _context;

        public ClientsAdminController(ConfigurationDbContext context)
        {
            this._context = context;
        }
        // GET: Client
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clients.ToListAsync());
        }

        // GET: ClientsAdmin/Create
        public ActionResult Create()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            selectList.Add(new SelectListItem()
            {
                Text = "ClientCredentials",
                Value = "ClientCredentials"
            });
            selectList.Add(new SelectListItem()
            {
                Text = "Code",
                Value = "Code"
            });
            selectList.Add(new SelectListItem()
            {
                Text = "Implicit",
                Value = "Implicit"
            });
            selectList.Add(new SelectListItem()
            {
                Text = "Hybrid",
                Value = "Hybrid"
            });
            selectList.Add(new SelectListItem()
            {
                Text = "ResourceOwnerPassword",
                Value = "ResourceOwnerPassword"
            });
            selectList.Add(new SelectListItem()
            {
                Text = "HybridAndClientCredentials",
                Value = "HybridAndClientCredentials"
            });
            selectList.Add(new SelectListItem()
            {
                Text = "ResourceOwnerPasswordAndClientCredentials",
                Value = "ResourceOwnerPasswordAndClientCredentials"
            });
            selectList.Add(new SelectListItem()
            {
                Text = "CodeAndClientCredentials",
                Value = "CodeAndClientCredentials"
            });
            selectList.Add(new SelectListItem()
            {
                Text = "ImplicitAndClientCredentials",
                Value = "ImplicitAndClientCredentials"
            });
            ViewBag.AllowedGrantTypes = selectList;
            return View();
        }

        // POST: ClientsAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                string clientId = collection["ClientId"];
                string secret = collection["ClientSecret"];
                string tokenTime = collection["TokenTime"];
                string clientName = collection["ClientName"];
                string allowedScopes = collection["AllowedScopes"];
                string allowedGrantTypes = collection["AllowedGrantTypes"];
                string redirectUris = collection["RedirectUris"];
                string postLogoutRedirectUris = collection["PostLogoutRedirectUris"];
                int accessTokenLifetime = 3600;
                int.TryParse(tokenTime, out accessTokenLifetime);
                var client = new Client
                {
                    ClientId = clientId,

                    // no interactive user, use the clientid/secret for authentication
                    //AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientName= clientName,
                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret(secret.Sha256())
                    },
                    RedirectUris = { },
                    PostLogoutRedirectUris = { },
                    AccessTokenLifetime = accessTokenLifetime,//设置过期时间，默认3600秒/1小时
                    // scopes that client has access to
                    AllowedScopes = { }
                };
                if (!string.IsNullOrEmpty(allowedScopes))
                {
                    client.AllowedScopes = allowedScopes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
                if (!string.IsNullOrEmpty(redirectUris))
                {
                    client.RedirectUris = redirectUris.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
                if (!string.IsNullOrEmpty(postLogoutRedirectUris))
                {
                    client.PostLogoutRedirectUris = postLogoutRedirectUris.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
                switch (allowedGrantTypes)
                {
                    case "Implicit":
                        client.AllowedGrantTypes = GrantTypes.Implicit;
                        break;
                    case "ImplicitAndClientCredentials":
                        client.AllowedGrantTypes = GrantTypes.ImplicitAndClientCredentials;
                        break;
                    case "Code":
                        client.AllowedGrantTypes = GrantTypes.Code;
                        break;
                    case "CodeAndClientCredentials":
                        client.AllowedGrantTypes = GrantTypes.CodeAndClientCredentials;
                        break;
                    case "Hybrid":
                        client.AllowedGrantTypes = GrantTypes.Hybrid;
                        break;
                    case "HybridAndClientCredentials":
                        List<string> AllowedGrantList = new List<string>();
                        AllowedGrantList.Add(IdentityServerConstants.StandardScopes.OpenId);
                        AllowedGrantList.Add(IdentityServerConstants.StandardScopes.Profile);
                        foreach (var item in client.AllowedScopes)
                        {
                            AllowedGrantList.Add(item);
                        }
                        client.AllowedGrantTypes = GrantTypes.HybridAndClientCredentials;
                        client.AllowedScopes = AllowedGrantList.ToArray();
                        client.AllowOfflineAccess = true;
                        break;
                    case "ClientCredentials":
                        client.AllowedGrantTypes = GrantTypes.ClientCredentials;
                        break;
                    case "ResourceOwnerPassword":
                        client.AllowedGrantTypes = GrantTypes.ResourceOwnerPassword;
                        break;
                    case "ResourceOwnerPasswordAndClientCredentials":
                        client.AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials;
                        break;
                }
                
                _context.Clients.Add(client.ToEntity());
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                AddErrors(ex);
                return View();
            }
        }

        //
        // GET: /ClientsAdmin/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                throw new ApplicationException();
            }
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == id);
            if (client == null)
            {
                throw new ApplicationException();
            }
            return View(client);
        }

        //
        // POST: /ClientsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    throw new ApplicationException();
                }
                var client = await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == id);
                if (client == null)
                {
                    throw new ApplicationException();
                }
                try
                {
                    _context.Clients.Remove(client);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    AddErrors(ex);
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        #region Helpers

        private void AddErrors(Exception result)
        {
            ModelState.AddModelError(string.Empty, result.Message);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
