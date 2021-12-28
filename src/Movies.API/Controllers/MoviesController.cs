using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.API.Data;
using Movies.API.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("ClientIdPolicy")]
    public class MoviesController : ControllerBase
    {
        private readonly MovieDbContext _dbContext;

        public MoviesController(MovieDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Movie>>> GetAll()
        {
            return await _dbContext.Movie.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> Get(int id)
        {
            var movie = await _dbContext.Movie.FindAsync(id);
            if (movie == null)
                return NotFound();

            return movie;
        }

        [HttpPost]
        public async Task<ActionResult<Movie>> Post(Movie movie)
        {
            _dbContext.Add(movie);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = movie.Id }, movie);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Movie>> Put(int id, Movie movie)
        {
            if (id != movie.Id)
                return BadRequest();

            _dbContext.Entry(movie).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exc)
            {
                if (!MovieExists(id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Movie>> Delete(int id)
        {
            var movie = await _dbContext.Movie.FindAsync(id);
            if (movie == null)
                return NotFound();

            _dbContext.Remove(movie);
            await _dbContext.SaveChangesAsync();

            return movie;
        }

        private bool MovieExists(int id)
        {
            return _dbContext.Movie.Any(i => i.Id == id);
        }
    }
}
