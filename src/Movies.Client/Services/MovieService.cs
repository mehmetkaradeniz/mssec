using Movies.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Movies.Client.Services
{
    public class MovieService : IMovieService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MovieService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<Movie>> GetAll()
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/movies");
            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var movies = JsonConvert.DeserializeObject<List<Movie>>(content);

            return movies;
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
