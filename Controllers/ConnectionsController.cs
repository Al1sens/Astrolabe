using Astrolabe.Data;
using Astrolabe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Astrolabe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConnectionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConnectionsController(AppDbContext context)
        {
            _context = context;
        }


        // Забираем все нити созвездий из PostgreSQL
        [HttpGet]
        public async Task<ActionResult<List<StarConnection>>> GetConnections()
        {
            return await _context.StarConnections.ToListAsync();
        }

        // Создаём новую нить между двумя звёздами
        [HttpPost]
        public async Task<ActionResult<StarConnection>> CreateConnection([FromBody] CreateConnectionDto dto)
        {
            if (dto.FromStarId == dto.ToStarId)
            {
                return BadRequest("Нельзя соединить звезду саму с собой.");
            }

                // Проверяем, нет ли уже такой связи (в любую сторону)
                bool exists = await _context.StarConnections.AnyAsync(c =>
                (c.FromStarId == dto.FromStarId && c.ToStarId == dto.ToStarId) ||
                (c.FromStarId == dto.ToStarId && c.ToStarId == dto.FromStarId));

                if (exists)
                {
                    return Conflict("Такая нить уже существует.");
                }
            

            var connection = new StarConnection
            {
                FromStarId = dto.FromStarId,
                ToStarId = dto.ToStarId
            };

            _context.StarConnections.Add(connection);
            await _context.SaveChangesAsync();

            return Ok(connection);
        }


            // Удаляем нить по её Id (DELETE /api/connections/{id})
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteConnection(int id)
        {
            var connection = await _context.StarConnections.FindAsync(id);
            if (connection is null)
                return NotFound();

            _context.StarConnections.Remove(connection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }


    public record CreateConnectionDto(int FromStarId, int ToStarId);

}

