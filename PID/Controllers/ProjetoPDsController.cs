using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PID.Data;
using PID.Models;

namespace PID.Controllers
{
    public class ProjetoPDsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjetoPDsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjetoPDs
        public async Task<IActionResult> Index()
        {
            var projetos = await _context.ProjetosPD
                .Include(p => p.Desenvolvimentos)
                .Include(p => p.Dispendio)
                .ToListAsync();

            return View(projetos);
        }

        // GET: ProjetoPDs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var projetoPD = await _context.ProjetosPD
                .Include(p => p.Desenvolvimentos)
                .Include(p => p.Dispendio)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (projetoPD == null)
                return NotFound();

            return View(projetoPD);
        }

        // GET: ProjetoPDs/Create
        public IActionResult Create()
        {
            ViewData["IdDispendio"] = new SelectList(_context.Dispendios, "IdDispendio", "Descricao");
            return View();
        }

        // POST: ProjetoPDs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ano,IdDispendio,ProjetoFinep,ProjetoLeiBem")] ProjetoPD projetoPD)
        {
            if (!_context.Dispendios.Any(d => d.IdDispendio == projetoPD.IdDispendio))
                ModelState.AddModelError("IdDispendio", "Dispendio selecionado não existe.");

            if (!ModelState.IsValid)
            {
                ViewData["IdDispendio"] = new SelectList(_context.Dispendios, "IdDispendio", "Descricao", projetoPD.IdDispendio);
                return View(projetoPD);
            }

            _context.Add(projetoPD);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ProjetoPDs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var projetoPD = await _context.ProjetosPD.FindAsync(id);
            if (projetoPD == null)
                return NotFound();

            ViewData["IdDispendio"] = new SelectList(_context.Dispendios, "IdDispendio", "Descricao", projetoPD.IdDispendio);
            return View(projetoPD);
        }

        // POST: ProjetoPDs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ano,IdDispendio,ProjetoFinep,ProjetoLeiBem")] ProjetoPD projetoPD)
        {
            if (id != projetoPD.Id)
                return NotFound();

            if (!_context.Dispendios.Any(d => d.IdDispendio == projetoPD.IdDispendio))
                ModelState.AddModelError("IdDispendio", "Dispendio selecionado não existe.");

            if (!ModelState.IsValid)
            {
                ViewData["IdDispendio"] = new SelectList(_context.Dispendios, "IdDispendio", "Descricao", projetoPD.IdDispendio);
                return View(projetoPD);
            }

            try
            {
                _context.Update(projetoPD);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjetoPDExists(projetoPD.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ProjetoPDs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var projetoPD = await _context.ProjetosPD
                .Include(p => p.Desenvolvimentos)
                .Include(p => p.Dispendio)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (projetoPD == null)
                return NotFound();

            return View(projetoPD);
        }

        // POST: ProjetoPDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projetoPD = await _context.ProjetosPD.FindAsync(id);
            if (projetoPD != null)
                _context.ProjetosPD.Remove(projetoPD);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjetoPDExists(int id)
        {
            return _context.ProjetosPD.Any(e => e.Id == id);
        }
    }
}
