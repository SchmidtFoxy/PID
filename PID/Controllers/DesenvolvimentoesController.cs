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
                .Include(d => d.ProjetoPD) // Inclui o projeto vinculado
                .Include(d => d.Custos) // Inclui os custos relacionados ao desenvolvimento
                .FirstOrDefaultAsync(m => m.IdDesenvolvimento == id);

            if (desenvolvimento == null)
                return NotFound();

            return View(desenvolvimento);
        }


        // GET: Desenvolvimentoes/Create
        public IActionResult Create()
        {
            ViewData["ProjetoPDId"] = new SelectList(_context.ProjetosPD.Select(p => new
            {
                Id = p.Id,
                Descricao = $"Projeto {p.Id} - Ano {p.Ano}"
            }), "Id", "Descricao");

            return View();
        }

        // POST: Desenvolvimentoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDesenvolvimento,Classificacao,Dificuldade,Produto,Descricao,ERP,DataInicio,DataFim,ProjetoFinep,ProjetoLeiBem,Status,Solicitante,ProjetoPDId")] Desenvolvimento desenvolvimento)
        {
            if (ModelState.IsValid)
            {
                desenvolvimento.Fase = "Inicial";
                _context.Add(desenvolvimento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ProjetoPDId"] = new SelectList(_context.ProjetosPD.Select(p => new
            {
                Id = p.Id,
                Descricao = $"Projeto {p.Id} - Ano {p.Ano}"
            }), "Id", "Descricao", desenvolvimento.ProjetoPDId);

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

            ViewData["ProjetoPDId"] = new SelectList(_context.ProjetosPD.Select(p => new
            {
                Id = p.Id,
                Descricao = $"Projeto {p.Id} - Ano {p.Ano}"
            }), "Id", "Descricao", desenvolvimento.ProjetoPDId);

            return View(desenvolvimento);
        }

        // POST: Desenvolvimentoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDesenvolvimento,Classificacao,Dificuldade,Produto,Descricao,ERP,DataInicio,DataFim,ProjetoFinep,ProjetoLeiBem,Status,Solicitante,ProjetoPDId")] Desenvolvimento desenvolvimento)
        {
            if (id != desenvolvimento.IdDesenvolvimento)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(desenvolvimento);
                    await _context.SaveChangesAsync();
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

            ViewData["ProjetoPDId"] = new SelectList(_context.ProjetosPD.Select(p => new
            {
                Id = p.Id,
                Descricao = $"Projeto {p.Id} - Ano {p.Ano}"
            }), "Id", "Descricao", desenvolvimento.ProjetoPDId);

            return View(desenvolvimento);
        }

        private bool DesenvolvimentoExists(int id)
        {
            return _context.Desenvolvimentos.Any(e => e.IdDesenvolvimento == id);
        }

        private async Task AtualizarCustoTotal(int idDesenvolvimento)
        {
            var desenvolvimento = await _context.Desenvolvimentos
                .Include(d => d.Custos)
                .FirstOrDefaultAsync(d => d.IdDesenvolvimento == idDesenvolvimento);
        }



        // GET: Desenvolvimentoes/LeiBem
        public async Task<IActionResult> LeiBem()
        {
            var desenvolvimentosLeiBem = await _context.Desenvolvimentos
                .Include(d => d.Custos)
                .Where(d => d.ProjetoLeiBem) // Filtra apenas os registros com Lei do Bem = true
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
                    // Omitir a coluna Lei do Bem (não é necessário incluí-la na model)
                    Custo = d.Custos.Sum(c => c.Valor),
                    Fase = d.Fase,
                    Status = d.Status,
                    Solicitante = d.Solicitante
                })
                .ToListAsync();

            return View(desenvolvimentosLeiBem);
        }

        // GET: Desenvolvimentoes/Finep
        public async Task<IActionResult> Finep()
        {
            var desenvolvimentosFinep = await _context.Desenvolvimentos
                .Include(d => d.Custos)
                .Where(d => d.ProjetoFinep) // Filtra apenas os registros com Finep = true
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
                    ProjetoLeiBem = d.ProjetoLeiBem,
                    // Omitir a coluna Finep (não é necessário incluí-la na model)
                    Custo = d.Custos.Sum(c => c.Valor),
                    Fase = d.Fase,
                    Status = d.Status,
                    Solicitante = d.Solicitante
                })
                .ToListAsync();

            return View(desenvolvimentosFinep);
        }





    }
}