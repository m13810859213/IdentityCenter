using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityCenter.Data;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityCenter.Controllers
{
    public class IdentityResourcesController : Controller
    {
        private readonly ConfigurationDbContext _context;

        public IdentityResourcesController(ConfigurationDbContext context)
        {
            this._context = context;
        }
        // GET: IdentityResources
        public async Task<IActionResult> Index()
        {
            if (!_context.IdentityResources.Any())
            {
                foreach (var resource in Config.GetIdentityResources())
                {
                    _context.IdentityResources.Add(resource.ToEntity());
                }
                _context.SaveChanges();
            }
            var result=await _context.IdentityResources.ToListAsync();
            return View(result);
        }

        // GET: IdentityResources/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: IdentityResources/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IdentityResources/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: IdentityResources/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: IdentityResources/Edit/5
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

        // GET: IdentityResources/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: IdentityResources/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}