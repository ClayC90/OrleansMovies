using Movies.Contracts;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.GrainClients
{
	public class MovieClient : IMovieClient
	{
		private readonly IGrainFactory _grainFactory;

		public MovieClient(IGrainFactory grainFactory)
		{
			_grainFactory = grainFactory ?? throw new ArgumentNullException(nameof(grainFactory));
		}

		private IMovieGrain Grain => _grainFactory.GetGrain<IMovieGrain>("Movie");

		public Task<List<Movie>> Get() => Grain.Get();

		public Task<Movie> GetDetails(int id) => Grain.GetDetails(id);

		public Task<Movie> GetDetails(string key) => Grain.GetDetails(key);

		public Task<List<Movie>> GetTop(int amount) => Grain.GetTop(amount);

		public Task<List<Movie>> GetByGenre(string genre) => Grain.GetByGenre(genre);

		public Task<Movie> GetSelectedMovieDetail(string key) => Grain.GetSelectedMovieDetail(key);

		public Task<List<Movie>> Search(string searchKey) => Grain.Search(searchKey);

		public Task UpdateMovie(int id, Movie movie) => Grain.UpdateMovie(id, movie);

		public Task<Movie> CreateMovie(Movie movie) => Grain.CreateMovie(movie);
	}
}
