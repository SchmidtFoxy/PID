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
            Console.WriteLine("🔍 Iniciando criação de Custo...");

            // Exibe os valores recebidos pelo POST
            Console.WriteLine($"📥 IdDispendio recebido: {custo.IdDispendio}");
            Console.WriteLine($"📥 IdDesenvolvimento recebido: {custo.IdDesenvolvimento}");
            Console.WriteLine($"📥 Valor recebido: {custo.Valor}");
            Console.WriteLine($"📥 Data recebida: {custo.Data}");

            // Confere se os IDs existem no banco de dados
            var desenvolvimento = await _context.Desenvolvimentos.FirstOrDefaultAsync(d => d.IdDesenvolvimento == custo.IdDesenvolvimento);
            if (desenvolvimento == null)
            {
                ModelState.AddModelError("IdDesenvolvimento", "O Desenvolvimento selecionado não existe.");
                Console.WriteLine("❌ Erro: Desenvolvimento não encontrado no banco!");
            }

            var dispendio = await _context.Dispendios.FirstOrDefaultAsync(d => d.IdDispendio == custo.IdDispendio);
            if (dispendio == null)
            {
                ModelState.AddModelError("IdDispendio", "O Dispendio selecionado não existe.");
                Console.WriteLine("❌ Erro: Dispendio não encontrado no banco!");
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("❌ ModelState inválido!");
                TempData["Erro"] = "Erro ao criar custo. Verifique os campos preenchidos.";
                ViewData["IdDesenvolvimento"] = new SelectList(_context.Desenvolvimentos, "IdDesenvolvimento", "Classificacao", custo.IdDesenvolvimento);
                ViewData["IdDispendio"] = new SelectList(_context.Dispendios, "IdDispendio", "Descricao", custo.IdDispendio);
                return View(custo);
            }

            try
            {
                _context.Add(custo);
                await _context.SaveChangesAsync();
                Console.WriteLine("✅ Custo criado com sucesso!");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 Exceção ao salvar no banco: {ex.Message}");
                TempData["Erro"] = "Erro ao salvar no banco. Verifique os logs.";
            }

            ViewData["IdDesenvolvimento"] = new SelectList(_context.Desenvolvimentos, "IdDesenvolvimento", "Classificacao", custo.IdDesenvolvimento);
            ViewData["IdDispendio"] = new SelectList(_context.Dispendios, "IdDispendio", "Descricao", custo.IdDispendio);
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
                    Console.WriteLine("✅ Custo atualizado com sucesso!");
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

            ViewData["IdDesenvolvimento"] = new SelectList(_context.Desenvolvimentos, "IdDesenvolvimento", "Classificacao", custo.IdDesenvolvimento);
            ViewData["IdDispendio"] = new SelectList(_context.Dispendios, "IdDispendio", "Descricao", custo.IdDispendio);
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
                Console.WriteLine("✅ Custo excluído com sucesso!");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CustoExists(int id)
        {
            return _context.Custos.Any(e => e.Id == id);
        }
    }
}
