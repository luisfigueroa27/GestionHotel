using Hotel.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Web.Controllers
{
    public class PagosController : Controller
    {
        private readonly HotelDbContext _context;

        public PagosController(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string dni)
        {
            var pagos = _context.Pagos
                .Include(p => p.Hospedaje)
                .ThenInclude(h => h.Huesped)
                .AsQueryable();

            if (!string.IsNullOrEmpty(dni))
            {
                pagos = pagos.Where(p =>
                    p.Hospedaje.Huesped.DNI
                    .Contains(dni));
            }

            return View(await pagos.ToListAsync());
        }
    }
}