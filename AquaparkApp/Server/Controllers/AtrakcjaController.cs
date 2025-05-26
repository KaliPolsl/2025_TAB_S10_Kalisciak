using AquaparkApp.Server.Data;
using AquaparkApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtrakcjaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AtrakcjaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Atrakcja>>> GetAtrakcje()
        {
            return await _context.Atrakcja.ToListAsync();
        }
    }
}
