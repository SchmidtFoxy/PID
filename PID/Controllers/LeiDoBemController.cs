using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PID.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PID.Controllers
{
    public class LeiDoBemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeiDoBemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LeiDoBem
        public async Task<IActionResult> Index()
        {
            var projetosLeiBem = await _context.ProjetosPD
                .Include(p => p.Desenvolvimentos)
                .Include(p => p.Dispendio)
                .Where(p => p.ProjetoLeiBem)
                .ToListAsync();

            return View(projetosLeiBem);
        }
    }
}