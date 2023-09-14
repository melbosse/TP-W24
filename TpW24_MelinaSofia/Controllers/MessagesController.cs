using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TpW24_MelinaSofia.Models;
using TpW24_MelinaSofia.Tools;

namespace TpW24_MelinaSofia.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ForumSofiaMelinaContext _context;

        public MessagesController(ForumSofiaMelinaContext context)
        {
            _context = context;
        }
  
        // GET: Messages
        public async Task<IActionResult> Index(int?id, int?pageNumber, int? custPageSize)
        {
            var pageIndex = pageNumber ?? 1;
            var pageSize = custPageSize?? 1;
            if (pageSize !=1)
                ViewData["custPageSize"] = pageSize;
            var source = _context.Messages.Where(m=>m.SujetId ==id).Include(m => m.Sujet).Include(m=>m.User);
            return View(await PaginatedList<Message>.CreateAsync(source, pageIndex, pageSize));
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Messages == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.Sujet)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.MsgId == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Create
        [Authorize]
        public IActionResult Create(int? id)
        {
            ViewData["SujetId"] = new SelectList(_context.Sujets, "SujetId", "SujetId");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("MsgId,SujetId,UserId,Texte,Date,Actif,Description")] Message message)
        {
            if (ModelState.IsValid)
            {
      
                message.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                message.Date = DateTime.Now;
                _context.Add(message);
                await _context.SaveChangesAsync();
                /// <param name="actionName">The name of the action.</param>
                /// <param name="controllerName">The name of the controller.</param>
                /// <param name="routeValues">The parameters for a route.</param>
                return RedirectToAction("Index", "Messages", new { id = message.SujetId });
            }
            ViewData["SujetId"] = new SelectList(_context.Sujets, "SujetId", "SujetId", message.SujetId);
            return View(message);
        }

        // GET: Messages/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Messages == null)
            {
                return NotFound();
            }

            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            ViewData["SujetId"] = new SelectList(_context.Sujets, "SujetId", "SujetId", message.SujetId);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("MsgId,SujetId,UserId,Texte,Date,Actif,Description")] Message message)
        {
            if (id != message.MsgId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    message.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    message.Date = DateTime.Now;
                    message.Actif = true;
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.MsgId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Messages", new { id = message.SujetId });
            }
            ViewData["SujetId"] = new SelectList(_context.Sujets, "SujetId", "SujetId", message.SujetId);
            return View(message);
        }

        // GET: Messages/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Messages == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.Sujet)
                .FirstOrDefaultAsync(m => m.MsgId == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Messages == null)
            {
                return Problem("Entity set 'ForumSofiaMelinaContext.Messages'  is null.");
            }
            var message = await _context.Messages.FindAsync(id);
            if (message != null)
            {
                _context.Messages.Remove(message);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
          return _context.Messages.Any(e => e.MsgId == id);
        }
    }
}
