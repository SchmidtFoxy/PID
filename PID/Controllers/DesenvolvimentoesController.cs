using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PID.Data;
using PID.Models;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

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

            foreach (var modelError in ModelState)
            {
                foreach (var error in modelError.Value.Errors)
                {
                    Console.WriteLine($"Erro em {modelError.Key}: {error.ErrorMessage}");
                }
            }


            ViewBag.ModelStateErros = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarComentario(int DesenvolvimentoId, string Texto)
        {
            if (string.IsNullOrWhiteSpace(Texto))
                return RedirectToAction("Details", new { id = DesenvolvimentoId });

            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
                return Forbid(); // ou RedirectToAction("Login")

            var comentario = new Comentario
            {
                DesenvolvimentoId = DesenvolvimentoId,
                Texto = Texto,
                UsuarioId = usuario.Id,
                DataCriacao = DateTime.Now
            };

            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = DesenvolvimentoId });
        }


        public async Task<IActionResult> ExportarHistoricoExcel(int id)
        {
            var desenvolvimento = await _context.Desenvolvimentos
                .Include(d => d.Custos)
                .Include(d => d.Comentarios).ThenInclude(c => c.Usuario)
                .Include(d => d.HistoricoEdicoes).ThenInclude(h => h.Usuario)
                .FirstOrDefaultAsync(d => d.IdDesenvolvimento == id);

            if (desenvolvimento == null) return NotFound();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Relatório");

            int row = 1;
            ws.Cell(row++, 1).Value = "Relatório de Desenvolvimento";
            ws.Cell(row++, 1).Value = "Informações Gerais";
            ws.Cell(row++, 1).Value = "Produto";
            ws.Cell(row - 1, 2).Value = desenvolvimento.Produto;
            ws.Cell(row++, 1).Value = "Descrição";
            ws.Cell(row - 1, 2).Value = desenvolvimento.Descricao;
            ws.Cell(row++, 1).Value = "Solicitante";
            ws.Cell(row - 1, 2).Value = desenvolvimento.Solicitante;
            ws.Cell(row++, 1).Value = "Classificação";
            ws.Cell(row - 1, 2).Value = desenvolvimento.Classificacao;
            ws.Cell(row++, 1).Value = "Dificuldade";
            ws.Cell(row - 1, 2).Value = desenvolvimento.Dificuldade;
            ws.Cell(row++, 1).Value = "Status";
            ws.Cell(row - 1, 2).Value = desenvolvimento.Status;
            ws.Cell(row++, 1).Value = "Data de Início";
            ws.Cell(row - 1, 2).Value = desenvolvimento.DataInicio.ToShortDateString();
            ws.Cell(row++, 1).Value = "Data de Fim";
            ws.Cell(row - 1, 2).Value = desenvolvimento.DataFim.ToShortDateString();
            ws.Cell(row++, 1).Value = "Custo Total";
            ws.Cell(row - 1, 2).Value = desenvolvimento.Custos.Sum(c => c.Valor).ToString("C2");

            row++;
            ws.Cell(row++, 1).Value = "Comentários Recentes";
            foreach (var c in desenvolvimento.Comentarios.OrderByDescending(c => c.DataCriacao).Take(3))
            {
                ws.Cell(row++, 1).Value = $"- {c.Texto} ({c.Usuario?.NomeCompleto})";
            }

            row++;
            ws.Cell(row++, 1).Value = "Histórico de Edições";
            foreach (var h in desenvolvimento.HistoricoEdicoes.OrderBy(h => h.DataAlteracao))
            {
                ws.Cell(row++, 1).Value = $"- {h.DataAlteracao:dd/MM/yyyy} - {h.CampoAlterado}: '{h.ValorAnterior}' → '{h.ValorAtual}' por {h.Usuario?.NomeCompleto}";
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Relatorio_Desenvolvimento_{desenvolvimento.IdDesenvolvimento}.xlsx");
        }


        public async Task<IActionResult> ExportarHistoricoPdf(int id)
        {
            var desenvolvimento = await _context.Desenvolvimentos
                .Include(d => d.Custos)
                .Include(d => d.Comentarios).ThenInclude(c => c.Usuario)
                .Include(d => d.HistoricoEdicoes).ThenInclude(h => h.Usuario)
                .FirstOrDefaultAsync(d => d.IdDesenvolvimento == id);

            if (desenvolvimento == null)
                return NotFound();

            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var fontTitle = new XFont("Arial", 16, XFontStyle.Bold);
            var fontSection = new XFont("Arial", 12, XFontStyle.Bold);
            var fontText = new XFont("Arial", 10);

            int y = 40;
            gfx.DrawString("Relatório de Desenvolvimento", fontTitle, XBrushes.Black, new XRect(0, y, page.Width, 20), XStringFormats.TopCenter);
            y += 40;

            gfx.DrawString("Informações Gerais", fontSection, XBrushes.Black, new XPoint(40, y));
            y += 25;

            void DrawField(string label, string value)
            {
                gfx.DrawString(label + ":", fontText, XBrushes.Black, new XPoint(50, y));
                gfx.DrawString(value, fontText, XBrushes.Black, new XPoint(150, y));
                y += 20;
            }

            DrawField("Produto", desenvolvimento.Produto);
            DrawField("Descrição", desenvolvimento.Descricao);
            DrawField("Solicitante", desenvolvimento.Solicitante);
            DrawField("Classificação", desenvolvimento.Classificacao);
            DrawField("Dificuldade", desenvolvimento.Dificuldade);
            DrawField("Status", desenvolvimento.Status);
            DrawField("Data de Início", desenvolvimento.DataInicio.ToShortDateString());
            DrawField("Data de Fim", desenvolvimento.DataFim.ToShortDateString());
            DrawField("Custo Total", desenvolvimento.Custos.Sum(c => c.Valor).ToString("C2"));

            y += 20;
            gfx.DrawString("Comentários Recentes", fontSection, XBrushes.Black, new XPoint(40, y));
            y += 25;
            foreach (var c in desenvolvimento.Comentarios.OrderByDescending(c => c.DataCriacao).Take(3))
            {
                gfx.DrawString("- " + c.Texto + " (" + c.Usuario?.NomeCompleto + ")", fontText, XBrushes.Black, new XPoint(50, y));
                y += 20;
            }

            y += 20;
            gfx.DrawString("Histórico de Edições", fontSection, XBrushes.Black, new XPoint(40, y));
            y += 25;
            foreach (var h in desenvolvimento.HistoricoEdicoes.OrderBy(h => h.DataAlteracao))
            {
                var linha = $"- {h.DataAlteracao:dd/MM/yyyy} - {h.CampoAlterado}: '{h.ValorAnterior}' → '{h.ValorAtual}' por {h.Usuario?.NomeCompleto}";
                gfx.DrawString(linha, fontText, XBrushes.Black, new XPoint(50, y));
                y += 20;
            }

            using var stream = new MemoryStream();
            document.Save(stream, false);
            stream.Position = 0;

            return File(stream.ToArray(), "application/pdf", $"Relatorio_Desenvolvimento_{id}.pdf");
        }
    }
}