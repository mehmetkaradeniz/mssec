using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Client.Services
{
    public class MovieService : IMovieService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MovieService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
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

        public async Task<bool> Exists(int id)
        {
            return await GetById(id) != null;
        }

        public async Task<Movie> GetById(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/movies/" + id);
            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var movie = JsonConvert.DeserializeObject<Movie>(content);

            return movie;
        }

        public async Task<Movie> Add(Movie movie)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");
            var content = new StringContent(JsonConvert.SerializeObject(movie), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/api/movies", content).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var createdMovie = JsonConvert.DeserializeObject<Movie>(responseContent);

            return createdMovie;
        }

        public async Task<Movie> Update(Movie movie)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");
            var content = new StringContent(JsonConvert.SerializeObject(movie), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync("/api/movies/" + movie.Id, content).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var updatedMovie = JsonConvert.DeserializeObject<Movie>(responseContent);

            return updatedMovie;
        }

        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");
            var response = await httpClient.DeleteAsync("/api/movies/" + id).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var deletedMovie = JsonConvert.DeserializeObject<Movie>(responseContent);
        }

        public async Task<OnlyAdminViewModel> GetUserInfo()
        {
            var idpClient = _httpClientFactory.CreateClient("IDPClient");
            var metadataResponse = await idpClient.GetDiscoveryDocumentAsync();
            if (metadataResponse.IsError)
                throw new HttpRequestException("Something went wrong during requesting the discovery document");

            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var userInfoResponse = await idpClient.GetUserInfoAsync(new UserInfoRequest()
            {
                Address = metadataResponse.UserInfoEndpoint,
                Token = accessToken
            });
            if (userInfoResponse.IsError)
                throw new HttpRequestException("Something went wrong during requesting the user info");

            var userInfoDict = new Dictionary<string, string>();

            foreach (var claim in userInfoResponse.Claims)
                userInfoDict.Add(claim.Type, claim.Value);

            return new OnlyAdminViewModel(userInfoDict);
        }
    }
}
