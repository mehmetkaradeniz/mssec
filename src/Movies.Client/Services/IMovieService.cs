using Movies.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Client.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAll();
        Task<bool> Exists(int id);
        Task<Movie> GetById(int id);
        Task<Movie> Add(Movie movie);
        Task<Movie> Update(Movie movie);
        Task Delete(int id);
        Task<OnlyAdminViewModel> GetUserInfo();
    }
}
