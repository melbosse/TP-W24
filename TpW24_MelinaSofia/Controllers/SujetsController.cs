using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TpW24_MelinaSofia.Models;
using TpW24_MelinaSofia.ViewModels;

namespace TpW24_MelinaSofia.Controllers
{
    public class SujetsController : Controller
    {
        private readonly ForumSofiaMelinaContext _context;

        public SujetsController(ForumSofiaMelinaContext context)
        {
            _context = context;
        }

        // GET: Sujets

        public async Task<IActionResult> Index(int? id)//Methode - SIGNATURE
        {
            var sujetMesgs = _context.Sujets.Where(s => s.CatId == id).Include(s => s.Cat).Select(s => new
            SujetMsg
            {
                SujetId = s.SujetId,
                CatId = s.CatId,
                UserId= s.UserId,
                UserName= s.User,
                Titre = s.Titre,
                Texte = s.Texte,
                Date = s.Date,
                Vues = s.Vues,
                Actif = s.Actif,
                DernierMsg = _context.Messages.Where(m=>m.SujetId == s.SujetId).OrderByDescending(m=>m.Date).Include(m=>m.User).FirstOrDefault(),
                
                TotalMessages = s.Messages.Count
            });
            return View (await sujetMesgs.ToListAsync());

        }
        public async Task<IActionResult> ListeUser(int? id)//Methode - SIGNATURE
        {
            var listeUsages = _context.AspNetUsers.Select(a => new
            ListeUser
            {
                UserId = a.Id,
                UserName = a.UserName,
                TotalSujets = a.Sujets.Count(),
                TotalMsg = a.Messages.Count(),
                DateMsg = a.Messages.Select(m=>m.Date).OrderByDescending(m=>m.Date).FirstOrDefault(),
                DateSujet = a.Sujets.Select(s=>s.Date).OrderByDescending(s=>s.Date).FirstOrDefault(),                  
                DateLastAction = a.Messages.Select(m => m.Date).OrderByDescending(m => m.Date).Union(a.Sujets.Select(s => s.Date).OrderByDescending(s => s.Date)).Max()
            });

            return View(await listeUsages.ToListAsync());


        }

        // GET: Sujets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sujets == null)
            {
                return NotFound();
            }

            var sujet = await _context.Sujets
                .Include(s=>s.User)
                .Include(s => s.Cat)
                .FirstOrDefaultAsync(m => m.SujetId == id);
            if (sujet == null)
            {
                return NotFound();
            }

            return View(sujet);
        }

        // GET: Sujets/Create
        [Authorize]
        public IActionResult Create( int? id)
        {
            ViewData["CatId"] = id;

            return View();
        }

        // POST: Sujets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("SujetId,CatId,UserId,Titre,Texte,Date,Vues,Actif")] Sujet sujet)
        {
            if (ModelState.IsValid)
            {
                sujet.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                sujet.Date= DateTime.Now;
                _context.Add(sujet);
                await _context.SaveChangesAsync();
                /// <param name="actionName">The name of the action.</param>
                /// <param name="controllerName">The name of the controller.</param>
                /// <param name="routeValues">The parameters for a route.</param>
                return RedirectToAction("Index", "Sujets", new { id = sujet.CatId});
            }
           
            ViewData["CatId"] = new SelectList(_context.Categories, "CatId", "CatId", sujet.CatId);
        
            return View(sujet);
        }

        // GET: Sujets/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sujets == null)
            {
                return NotFound();
            }

            var sujet = await _context.Sujets.FindAsync(id);
            if (sujet == null)
            {
                return NotFound();
            }
            ViewData["CatId"] = id;
            return View(sujet);
        }

        // POST: Sujets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("SujetId,CatId,UserId,Titre,Texte,Date,Vues,Actif")] Sujet sujet)
        {
            if (id != sujet.SujetId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    sujet.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    sujet.Date = DateTime.Now;
                    sujet.Vues = 1;
                    sujet.Actif = true;
                    _context.Update(sujet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SujetExists(sujet.SujetId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Sujets", new { id = sujet.CatId });
            }
            ViewData["CatId"] = new SelectList(_context.Categories, "CatId", "CatId", sujet.CatId);
            return View(sujet);
        }

        // GET: Sujets/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sujets == null)
            {
                return NotFound();
            }

            var sujet = await _context.Sujets
                .Include(s => s.Cat)
                .FirstOrDefaultAsync(m => m.SujetId == id);
            if (sujet == null)
            {
                return NotFound();
            }

            return View(sujet);
        }

        // POST: Sujets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sujets == null)
            {
                return Problem("Entity set 'ForumSofiaMelinaContext.Sujets'  is null.");
            }
            var sujet = await _context.Sujets.FindAsync(id);
            if (sujet != null)
            {
                _context.Sujets.Remove(sujet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SujetExists(int id)
        {
          return _context.Sujets.Any(e => e.SujetId == id);
        }
    }
}
