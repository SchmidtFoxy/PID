using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PID.Data;
using PID.Models;

namespace PID.Controllers
{
    public class DispendiosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DispendiosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dispendios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Dispendios.ToListAsync());
        }

        // GET: Dispendios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispendio = await _context.Dispendios
                .FirstOrDefaultAsync(m => m.IdDispendio == id);
            if (dispendio == null)
            {
                return NotFound();
            }

            return View(dispendio);
        }

        // GET: Dispendios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dispendios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDispendio,Descricao,ProjetoFinep,ProjetoLeiBem")] Dispendio dispendio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dispendio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dispendio);
        }

        // GET: Dispendios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispendio = await _context.Dispendios.FindAsync(id);
            if (dispendio == null)
            {
                return NotFound();
            }
            return View(dispendio);
        }

        // POST: Dispendios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDispendio,Descricao,ProjetoFinep,ProjetoLeiBem")] Dispendio dispendio)
        {
            if (id != dispendio.IdDispendio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dispendio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DispendioExists(dispendio.IdDispendio))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dispendio);
        }

        // GET: Dispendios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispendio = await _context.Dispendios
                .FirstOrDefaultAsync(m => m.IdDispendio == id);
            if (dispendio == null)
            {
                return NotFound();
            }

            return View(dispendio);
        }

        // POST: Dispendios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dispendio = await _context.Dispendios.FindAsync(id);
            if (dispendio != null)
            {
                _context.Dispendios.Remove(dispendio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DispendioExists(int id)
        {
            return _context.Dispendios.Any(e => e.IdDispendio == id);
        }
    }
}
