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
    public class CustoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Custoes
        public async Task<IActionResult> Index()
        {
            var custos = _context.Custos.Include(c => c.Desenvolvimento).Include(c => c.Dispendio);
            return View(await custos.ToListAsync());
        }

        // NOVA VIEW: Custos Agrupados
        public async Task<IActionResult> CustosAgrupados()
        {
            var custos = await _context.Custos
                .Include(c => c.Desenvolvimento)
                .Include(c => c.Dispendio)
                .ToListAsync();

            var custosAgrupados = custos
                .GroupBy(c => new { c.Dispendio.Descricao, c.Desenvolvimento.Classificacao })
                .Select(g => new
                {
                    Dispendio = g.Key.Descricao,
                    Desenvolvimento = g.Key.Classificacao,
                    Total = g.Sum(c => c.Valor),
                    CustosDetalhados = g.Select(c => new {
                        c.Id,
                        c.Descricao,
                        c.Valor,
                        c.Data
                    }).ToList()
                }).ToList();

            return View(custosAgrupados);
        }

        // GET: Custoes/Create
        public IActionResult Create()
        {
            ViewData["IdDesenvolvimento"] = new SelectList(_context.Desenvolvimentos, "IdDesenvolvimento", "Classificacao");
            ViewData["IdDispendio"] = new SelectList(_context.Dispendios, "IdDispendio", "Descricao");
            return View();
        }

        // POST: Custoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdDispendio,Descricao,IdDesenvolvimento,Valor,Data")] Custo custo)
        {
            if (!ModelState.IsValid)
            {
                TempData["Erro"] = "Erro ao criar custo. Verifique os campos preenchidos.";
                return View(custo);
            }

            try
            {
                _context.Add(custo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Erro ao salvar no banco. Verifique os logs.";
            }

            return View(custo);
        }

        // GET: Custos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var custo = await _context.Custos
                .Include(c => c.Desenvolvimento)
                .Include(c => c.Dispendio)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (custo == null)
                return NotFound();

            return View(custo);
        }

        // GET: Custoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var custo = await _context.Custos.FindAsync(id);
            if (custo == null)
                return NotFound();

            ViewData["IdDesenvolvimento"] = new SelectList(_context.Desenvolvimentos, "IdDesenvolvimento", "Classificacao", custo.IdDesenvolvimento);
            ViewData["IdDispendio"] = new SelectList(_context.Dispendios, "IdDispendio", "Descricao", custo.IdDispendio);
            return View(custo);
        }

        // POST: Custoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdDispendio,Descricao,IdDesenvolvimento,Valor,Data")] Custo custo)
        {
            if (id != custo.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(custo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustoExists(custo.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(custo);
        }

        // DELETE: Custoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var custo = await _context.Custos
                .Include(c => c.Desenvolvimento)
                .Include(c => c.Dispendio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (custo == null)
                return NotFound();

            return View(custo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var custo = await _context.Custos.FindAsync(id);
            if (custo != null)
            {
                _context.Custos.Remove(custo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CustoExists(int id)
        {
            return _context.Custos.Any(e => e.Id == id);
        }
    }
}
