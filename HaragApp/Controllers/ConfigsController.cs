using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaragApp.Data;
using HaragApp.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace HaragApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class ConfigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConfigsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Configs
        public  IActionResult Index()
        {
            if(_context.Configs.ToList().Count == 0)
            {
                return View(new Configs());
            }
            else
            {
                return View(_context.Configs.ToList()[0]);
            }
        }


        // GET: Configs/Create
        public IActionResult Create()
        {
            var data = _context.Configs.ToList();
            if(data.Count == 0 || data == null)
            {
                return View();

            }else
            {
                return RedirectToAction("Edit", new { id = data[0].ID , configs = data[0] });
            }
        }

        // POST: Configs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(Configs configs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(configs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(configs);
        }

        // GET: Configs/Edit/5
        public IActionResult Edit()
        {
            var configs = _context.Configs.ToList()[0];
            if (configs == null)
            {
                return NotFound();
            }
            return View(configs);
        }

        // POST: Configs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Configs configs)
        {
            if (id != configs.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(configs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(configs);
        }


   
    }
}
