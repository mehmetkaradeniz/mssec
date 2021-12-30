using Movies.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Client.Services
{
    public class MovieService : IMovieService
    {
        public async Task<IEnumerable<Movie>> GetAll()
        {
            var result = new List<Movie>();

            result.Add(new Movie
            {
                Id = 1,
                Genre = "Comics",
                Title = "asd",
                Rating = "9.2",
                ImageUrl = "images/src",
                ReleaseDate = DateTime.Now,
                Owner = "mw"
            });

            return await Task.FromResult(result);
        }

        public Task<bool> Exists(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Movie> Add(Movie movie)
        {
            throw new System.NotImplementedException();
        }

        public Task<Movie> Update(Movie movie)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new System.NotImplementedException();
        }

    }
}
