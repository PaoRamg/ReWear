using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rewear.Data;

namespace Rewear.Controllers
{
    public class PublicacionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublicacionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Publicacions
        public async Task<IActionResult> Index()
        {
            // Obtener todas las publicaciones junto con sus categorías y estados de prenda
            var publicaciones = await _context.Publicaciones
                .Include(p => p.Categoria)
                .Include(p => p.EstadoPrenda)
                .ToListAsync();

            return View(publicaciones);
        }

        // GET: Publicacions/Create
        public IActionResult Create()
        {
            return View();
        }
    }
}