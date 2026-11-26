using Astrolabe.Data;
using Astrolabe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Astrolabe.Controllers
{

    // Компактный объект только для передачи новых координат с фронтенда, конструкторы у которых поля = аргументам конструктора

    public record UpdatePositionDto(int PosX, int PosY);
    public record UpdateStarDetailsDto( int Stage, int Difficulty, string? Notes);

    [ApiController]
    [Route("api/[controller]")]

    public class StarsController : ControllerBase
    {
        private readonly AppDbContext _context;
        //Получаем готовый AppDbContext через Dependency Injection (из Program.cs)
        public StarsController(AppDbContext context)
        {
            _context = context;
        }


        //Получить список всех звёзд (GET /api/stars)
        [HttpGet]
        public async Task<ActionResult<List<StarNode>>> GetStars()
        {
            return await _context.Stars.ToListAsync();
        }

        // Добавить новую звезду на карту (POST /api/stars)
        [HttpPost]
        public async Task<ActionResult<StarNode>> CreateStar(StarNode star)
        {
            _context.Stars.Add(star);
            await _context.SaveChangesAsync();
            return Ok(star);
        }

        // Сохраняем координаты 
        [HttpPut("{id}/position")]
        public async Task<IActionResult> UpdatePosition(int id, [FromBody] UpdatePositionDto dto)
        {
            // Ищем звезду в PostgreSQL по её Id
            var star = await _context.Stars.FindAsync(id);
            if(star is null)
                return NotFound();

            star.PosX = dto.PosX;
            star.PosY = dto.PosY;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // PUT /api/stars/{id}/details — обновляем статус "Изучено" и заметку
        [HttpPut("{id}/details")]
        public async Task<IActionResult> UodateDetails(int id, [FromBody] UpdateStarDetailsDto dto)
        {
            var star = await _context.Stars.FindAsync(id);
            if (star is null)
            {
                return NotFound();
            }

            star.Stage = dto.Stage;
            star.Difficulty = dto.Difficulty;
            star.Notes = dto.Notes;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE /api/stars/{id} — гасим звезду (нити удалятся каскадно в БД)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStar(int id)
        {
            var star = await _context.Stars.FindAsync(id);
            if(star is null)
            {
                return NotFound();
            }

            _context.Stars.Remove(star);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
    // Компактный объект только для передачи новых координат с фронтенда

}