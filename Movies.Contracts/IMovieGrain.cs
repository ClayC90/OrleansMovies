using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieGrain : IGrainWithStringKey
	{
		Task Initialize();

		Task<List<Movie>> Get();

		Task<Movie> GetDetails(int id);

		Task<Movie> GetDetails(string key);

		Task<bool> Exists(int id, string key);

		Task<List<Movie>> GetTop(int amount);

		Task<List<Movie>> GetByGenre(string genre);

		Task<Movie> GetSelectedMovieDetail(string key);

		Task<List<Movie>> Search(string searchKey);

		Task UpdateMovie(int id, Movie movie);

		Task<Movie> CreateMovie(Movie movie);
	}
}