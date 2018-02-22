using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
//using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityCenter.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ApiResourcesController : Controller
    {
        private readonly ConfigurationDbContext _context;

        public ApiResourcesController(ConfigurationDbContext context)
        {
            this._context = context;
        }
        // GET: Scopes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApiResources.ToListAsync());
        }

        // GET: Scopes/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Scopes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Scopes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                //IdentityServer4.EntityFramework.Entities.ApiResource
                string name = collection["Name"];
                string displayName = collection["DisplayName"];
                var api = new ApiResource(name, displayName);
                _context.ApiResources.Add(api.ToEntity());
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Scopes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Scopes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Scopes/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                throw new ApplicationException();
            }
            return View(entity);
        }

        // POST: Scopes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
                    if (entity == null)
                    {
                        throw new ApplicationException();
                    }
                    try
                    {
                        _context.ApiResources.Remove(entity);
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
            catch
            {
                return View();
            }
        }
        private void AddErrors(Exception result)
        {
            ModelState.AddModelError(string.Empty, result.Message);
        }
    }
}