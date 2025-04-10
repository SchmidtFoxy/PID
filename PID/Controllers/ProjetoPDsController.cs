using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
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

        public async Task<IActionResult> Index()
        {
            var projetos = await _context.ProjetosPD
                .Include(p => p.Desenvolvimentos)
                    .ThenInclude(d => d.Custos)
                .ToListAsync();

            foreach (var projeto in projetos)
            {
                foreach (var desenvolvimento in projeto.Desenvolvimentos)
                {
                    desenvolvimento.Custo = desenvolvimento.Custos.Sum(c => c.Valor);
                }
            }

            return View(projetos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var projetoPD = await _context.ProjetosPD
                .Include(p => p.Desenvolvimentos)
                    .ThenInclude(d => d.Custos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (projetoPD == null)
                return NotFound();

            foreach (var desenvolvimento in projetoPD.Desenvolvimentos)
            {
                desenvolvimento.Custo = desenvolvimento.Custos.Sum(c => c.Valor);
            }

            return View(projetoPD);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descricao,Ano,ProjetoFinep,ProjetoLeiBem")] ProjetoPD projetoPD)
        {
            if (!ModelState.IsValid)
                return View(projetoPD);

            _context.Add(projetoPD);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var projetoPD = await _context.ProjetosPD.FindAsync(id);
            if (projetoPD == null)
                return NotFound();

            return View(projetoPD);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,Ano,ProjetoFinep,ProjetoLeiBem")] ProjetoPD projetoPD)
        {
            if (id != projetoPD.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(projetoPD);

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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var projetoPD = await _context.ProjetosPD
                .Include(p => p.Desenvolvimentos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (projetoPD == null)
                return NotFound();

            return View(projetoPD);
        }

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

        [HttpGet]
        public async Task<IActionResult> ExportarProjetoExcel(int id)
        {
            var projeto = await _context.ProjetosPD
                .Include(p => p.Desenvolvimentos)
                    .ThenInclude(d => d.Custos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (projeto == null) return NotFound();

            foreach (var d in projeto.Desenvolvimentos)
            {
                d.Custo = d.Custos.Sum(c => c.Valor);
            }

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Projeto");

            ws.Cell(1, 1).Value = "Projeto";
            ws.Cell(2, 1).Value = "Título"; ws.Cell(2, 2).Value = projeto.Titulo;
            ws.Cell(3, 1).Value = "Descrição"; ws.Cell(3, 2).Value = projeto.Descricao;
            ws.Cell(4, 1).Value = "Ano"; ws.Cell(4, 2).Value = projeto.Ano;
            ws.Cell(5, 1).Value = "Finep"; ws.Cell(5, 2).Value = projeto.ProjetoFinep ? "Sim" : "Não";
            ws.Cell(6, 1).Value = "Lei do Bem"; ws.Cell(6, 2).Value = projeto.ProjetoLeiBem ? "Sim" : "Não";
            ws.Cell(7, 1).Value = "Custo Total"; ws.Cell(7, 2).Value = projeto.Desenvolvimentos.Sum(d => d.Custo);

            var linha = 9;
            ws.Cell(linha, 1).Value = "Desenvolvimentos";
            linha++;
            ws.Cell(linha, 1).Value = "Produto";
            ws.Cell(linha, 2).Value = "Classificação";
            ws.Cell(linha, 3).Value = "Dificuldade";
            ws.Cell(linha, 4).Value = "Status";
            ws.Cell(linha, 5).Value = "Início";
            ws.Cell(linha, 6).Value = "Fim";
            ws.Cell(linha, 7).Value = "Custo";

            foreach (var d in projeto.Desenvolvimentos)
            {
                linha++;
                ws.Cell(linha, 1).Value = d.Produto;
                ws.Cell(linha, 2).Value = d.Classificacao;
                ws.Cell(linha, 3).Value = d.Dificuldade;
                ws.Cell(linha, 4).Value = d.Status;
                ws.Cell(linha, 5).Value = d.DataInicio.ToShortDateString();
                ws.Cell(linha, 6).Value = d.DataFim.ToShortDateString();
                ws.Cell(linha, 7).Value = d.Custo;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"ProjetoPD_{projeto.Id}.xlsx");
        }

        [HttpGet]
        public async Task<IActionResult> ExportarProjetoPdf(int id)
        {
            var projeto = await _context.ProjetosPD
                .Include(p => p.Desenvolvimentos)
                    .ThenInclude(d => d.Custos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (projeto == null) return NotFound();

            foreach (var d in projeto.Desenvolvimentos)
            {
                d.Custo = d.Custos.Sum(c => c.Valor);
            }

            var doc = new PdfDocument();
            var page = doc.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Verdana", 12);

            double y = 40;
            gfx.DrawString($"Projeto P&D #{projeto.Id} - {projeto.Titulo}", font, XBrushes.Black, new XRect(40, y, page.Width, page.Height), XStringFormats.TopLeft);
            y += 30;

            gfx.DrawString("Descrição: " + projeto.Descricao, font, XBrushes.Black, new XRect(40, y, page.Width - 80, page.Height), XStringFormats.TopLeft);
            y += 25;

            gfx.DrawString($"Ano: {projeto.Ano}", font, XBrushes.Black, new XRect(40, y, page.Width, page.Height), XStringFormats.TopLeft);
            y += 25;

            gfx.DrawString($"Finep: {(projeto.ProjetoFinep ? "Sim" : "Não")} | Lei do Bem: {(projeto.ProjetoLeiBem ? "Sim" : "Não")} | Custo Total: R$ {projeto.Desenvolvimentos.Sum(d => d.Custo):N2}", font, XBrushes.Black, new XRect(40, y, page.Width, page.Height), XStringFormats.TopLeft);
            y += 35;

            gfx.DrawString("Desenvolvimentos:", font, XBrushes.Black, new XRect(40, y, page.Width, page.Height), XStringFormats.TopLeft);
            y += 25;

            foreach (var d in projeto.Desenvolvimentos)
            {
                if (y > page.Height - 100)
                {
                    page = doc.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    y = 40;
                }

                gfx.DrawString($"Produto: {d.Produto} | Classificação: {d.Classificacao} | Dificuldade: {d.Dificuldade}", font, XBrushes.Black, new XRect(40, y, page.Width - 80, page.Height), XStringFormats.TopLeft);
                y += 20;
                gfx.DrawString($"Status: {d.Status} | Início: {d.DataInicio:dd/MM/yyyy} | Fim: {d.DataFim:dd/MM/yyyy} | Custo: R$ {d.Custo:N2}", font, XBrushes.Black, new XRect(40, y, page.Width - 80, page.Height), XStringFormats.TopLeft);
                y += 30;
            }

            using var stream = new MemoryStream();
            doc.Save(stream, false);
            stream.Position = 0;

            return File(stream.ToArray(), "application/pdf", $"ProjetoPD_{projeto.Id}.pdf");
        }
    }
}
