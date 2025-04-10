using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<Usuario> _userManager;

        public DesenvolvimentoesController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var desenvolvimento = await _context.Desenvolvimentos
                .Include(d => d.ProjetoPD)
                .Include(d => d.Custos)
                .Include(d => d.HistoricoEdicoes).ThenInclude(h => h.Usuario)
                .Include(d => d.Comentarios).ThenInclude(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.IdDesenvolvimento == id);

            if (desenvolvimento == null)
                return NotFound();

            return View(desenvolvimento);
        }

        public IActionResult Create()
        {
            ViewData["ProjetoPDId"] = new SelectList(_context.ProjetosPD.Select(p => new
            {
                Id = p.Id,
                Descricao = $"Projeto {p.Id} - Ano {p.Ano}"
            }), "Id", "Descricao");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDesenvolvimento,Classificacao,Dificuldade,Produto,Descricao,ERP,DataInicio,DataFim,ProjetoFinep,ProjetoLeiBem,Fase,Status,Solicitante,ProjetoPDId")] Desenvolvimento desenvolvimento)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDesenvolvimento,Classificacao,Dificuldade,Produto,Descricao,ERP,DataInicio,DataFim,ProjetoFinep,ProjetoLeiBem,Fase,Status,Solicitante,ProjetoPDId")] Desenvolvimento desenvolvimento)
        {
            if (id != desenvolvimento.IdDesenvolvimento)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var original = await _context.Desenvolvimentos.AsNoTracking().FirstOrDefaultAsync(d => d.IdDesenvolvimento == id);
                    if (original == null) return NotFound();

                    var usuario = await _userManager.GetUserAsync(User);

                    void AdicionarHistorico(string campo, string anterior, string atual)
                    {
                        if (anterior != atual)
                        {
                            _context.HistoricoEdicoes.Add(new HistoricoEdicaoDesenvolvimento
                            {
                                IdDesenvolvimento = id,
                                CampoAlterado = campo,
                                ValorAnterior = anterior,
                                ValorAtual = atual,
                                DataAlteracao = DateTime.Now,
                                UsuarioId = usuario?.Id
                            });
                        }
                    }

                    AdicionarHistorico("Classificação", original.Classificacao, desenvolvimento.Classificacao);
                    AdicionarHistorico("Produto", original.Produto, desenvolvimento.Produto);
                    AdicionarHistorico("Descrição", original.Descricao, desenvolvimento.Descricao);
                    AdicionarHistorico("ERP", original.ERP.ToString(), desenvolvimento.ERP.ToString());
                    AdicionarHistorico("Status", original.Status, desenvolvimento.Status);
                    AdicionarHistorico("Solicitante", original.Solicitante, desenvolvimento.Solicitante);

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

        public async Task<IActionResult> LeiBem()
        {
            var desenvolvimentosLeiBem = await _context.Desenvolvimentos
                .Include(d => d.Custos)
                .Where(d => d.ProjetoLeiBem)
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
                    Custo = d.Custos.Sum(c => c.Valor),
                    Fase = d.Fase,
                    Status = d.Status,
                    Solicitante = d.Solicitante
                })
                .ToListAsync();

            return View(desenvolvimentosLeiBem);
        }

        public async Task<IActionResult> Finep()
        {
            var desenvolvimentosFinep = await _context.Desenvolvimentos
                .Include(d => d.Custos)
                .Where(d => d.ProjetoFinep)
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
