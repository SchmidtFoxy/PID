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
    public class HistoricoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HistoricoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Historicoes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HistoricoEdicoes.Include(h => h.Desenvolvimento);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Historicoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var desenvolvimento = await _context.Desenvolvimentos
                .Include(d => d.HistoricoEdicoes)  // Carrega as edições
                .ThenInclude(he => he.Usuario)    // Carrega o usuário que fez a alteração
                .Include(d => d.Comentarios)      // Carrega os comentários
                .ThenInclude(c => c.Usuario)      // Carrega o usuário que fez o comentário
                .FirstOrDefaultAsync(m => m.IdDesenvolvimento == id);

            if (desenvolvimento == null)
            {
                return NotFound();
            }

            return View(desenvolvimento);
        }


        // GET: Historicoes/Create
        public IActionResult Create()
        {
            ViewData["IdDesenvolvimento"] = new SelectList(_context.Desenvolvimentos, "IdDesenvolvimento", "Classificacao");
            return View();
        }

        // POST: Historicoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdDesenvolvimento,CampoAlterado,ValorAnterior,ValorAtual,DataAlteracao,UsuarioId")] HistoricoEdicaoDesenvolvimento historico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(historico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDesenvolvimento"] = new SelectList(_context.Desenvolvimentos, "IdDesenvolvimento", "Classificacao", historico.IdDesenvolvimento);
            return View(historico);
        }

        // GET: Historicoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historico = await _context.HistoricoEdicoes.FindAsync(id);
            if (historico == null)
            {
                return NotFound();
            }
            ViewData["IdDesenvolvimento"] = new SelectList(_context.Desenvolvimentos, "IdDesenvolvimento", "Classificacao", historico.IdDesenvolvimento);
            return View(historico);
        }

        // POST: Historicoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdDesenvolvimento,CampoAlterado,ValorAnterior,ValorAtual,DataAlteracao,UsuarioId")] HistoricoEdicaoDesenvolvimento historico)
        {
            if (id != historico.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(historico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistoricoEdicaoDesenvolvimentoExists(historico.Id))
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
            ViewData["IdDesenvolvimento"] = new SelectList(_context.Desenvolvimentos, "IdDesenvolvimento", "Classificacao", historico.IdDesenvolvimento);
            return View(historico);
        }

        // GET: Historicoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historico = await _context.HistoricoEdicoes
                .Include(h => h.Desenvolvimento)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (historico == null)
            {
                return NotFound();
            }

            return View(historico);
        }

        // POST: Historicoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var historico = await _context.HistoricoEdicoes.FindAsync(id);
            if (historico != null)
            {
                _context.HistoricoEdicoes.Remove(historico);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistoricoEdicaoDesenvolvimentoExists(int id)
        {
            return _context.HistoricoEdicoes.Any(e => e.Id == id);
        }
    }

}
