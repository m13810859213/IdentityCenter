using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                int accessTokenLifetime = 3600;
                int.TryParse(tokenTime, out accessTokenLifetime);
                var client = new Client
                {
                    ClientId = clientId,

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientName= clientName,
                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret(secret.Sha256())
                    },
                    AccessTokenLifetime = accessTokenLifetime,//设置过期时间，默认3600秒/1小时
                    // scopes that client has access to
                    AllowedScopes = { }
                };
                if (!string.IsNullOrEmpty(allowedScopes))
                {
                    client.AllowedScopes = allowedScopes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
                
                _context.Clients.Add(client.ToEntity());
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
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
