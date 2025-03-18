using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PID.Data;
using PID.Models;

namespace PID.Controllers
{
    public class DesenvolvimentoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DesenvolvimentoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Desenvolvimentoes
        public async Task<IActionResult> Index()
        {
            var desenvolvimentos = await _context.Desenvolvimentos
                .Include(d => d.Custos) 
                .Select(d => new Desenvolvimento
                {
                    IdDesenvolvimento = d.IdDesenvolvimento,
                    Classificacao = d.Classificacao,
                    Dificuldade = d.Dificuldade,
                    Produto = d.Produto,
                    Descricao = d.Descricao,
                    ERP = d.ERP,
                    DataInicio = d.DataInicio,
                    DataFim = d.DataFim,
                    ProjetoFinep = d.ProjetoFinep,
                    ProjetoLeiBem = d.ProjetoLeiBem,
                    Fase = d.Fase,
                    Status = d.Status,
                    Solicitante = d.Solicitante,
                    Custo = d.Custos.Sum(c => c.Valor) 
                })
                .ToListAsync();

            return View(desenvolvimentos);
        }





        // GET: Desenvolvimentoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var desenvolvimento = await _context.Desenvolvimentos
                .Include(d => d.Custos)
                .FirstOrDefaultAsync(m => m.IdDesenvolvimento == id);
            if (desenvolvimento == null)
                return NotFound();

            return View(desenvolvimento);
        }

        // GET: Desenvolvimentoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Desenvolvimentoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDesenvolvimento,Classificacao,Dificuldade,Produto,Descricao,ERP,DataInicio,DataFim,ProjetoFinep,ProjetoLeiBem,Status,Solicitante")] Desenvolvimento desenvolvimento)
        {
            if (ModelState.IsValid)
            {
                desenvolvimento.Fase = "Inicial"; // ✅ Sempre inicia com "Inicial"

                _context.Add(desenvolvimento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(desenvolvimento);
        }

        // GET: Desenvolvimentoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var desenvolvimento = await _context.Desenvolvimentos
                .Include(d => d.Custos)
                .FirstOrDefaultAsync(m => m.IdDesenvolvimento == id);
            if (desenvolvimento == null)
                return NotFound();

            return View(desenvolvimento);
        }

        // POST: Desenvolvimentoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDesenvolvimento,Classificacao,Dificuldade,Produto,Descricao,ERP,DataInicio,DataFim,ProjetoFinep,ProjetoLeiBem,Status,Solicitante")] Desenvolvimento desenvolvimento)
        {
            if (id != desenvolvimento.IdDesenvolvimento)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(desenvolvimento);
                    await _context.SaveChangesAsync();

                    // ✅ Atualiza o custo total do desenvolvimento após a edição
                    await AtualizarCustoTotal(desenvolvimento.IdDesenvolvimento);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DesenvolvimentoExists(desenvolvimento.IdDesenvolvimento))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(desenvolvimento);
        }

        // GET: Desenvolvimentoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var desenvolvimento = await _context.Desenvolvimentos
                .Include(d => d.Custos)
                .FirstOrDefaultAsync(m => m.IdDesenvolvimento == id);
            if (desenvolvimento == null)
                return NotFound();

            return View(desenvolvimento);
        }

        // POST: Desenvolvimentoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var desenvolvimento = await _context.Desenvolvimentos
                .Include(d => d.Custos)
                .FirstOrDefaultAsync(d => d.IdDesenvolvimento == id);

            if (desenvolvimento != null)
            {
                _context.Desenvolvimentos.Remove(desenvolvimento);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DesenvolvimentoExists(int id)
        {
            return _context.Desenvolvimentos.Any(e => e.IdDesenvolvimento == id);
        }

        // ✅ Método para atualizar o custo total do desenvolvimento
        private async Task AtualizarCustoTotal(int idDesenvolvimento)
        {
            var desenvolvimento = await _context.Desenvolvimentos
                .Include(d => d.Custos)
                .FirstOrDefaultAsync(d => d.IdDesenvolvimento == idDesenvolvimento);
        }
    }
}
