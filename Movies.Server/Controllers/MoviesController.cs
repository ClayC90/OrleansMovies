using Microsoft.AspNetCore.Mvc;
using Movies.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Server.Controllers
{
	[Route("api/[controller]")]
	public class MoviesController : Controller
	{
		private readonly IMovieClient _client;

		public MoviesController(IMovieClient client)
		{
			_client = client;
		}

		[HttpGet("")]
		public async Task<List<Movie>> List()
		{
			var result = await _client.Get();
			return result;
		}

		[HttpGet("top-{amount}")]
		public async Task<List<Movie>> Top(int amount)
		{
			var result = await _client.GetTop(amount);
			return result;
		}

		[HttpGet("genre-{genre}")]
		public async Task<List<Movie>> ByGenre(string genre)
		{
			if (string.IsNullOrWhiteSpace(genre))
				throw new ArgumentNullException($"{nameof(genre)} is required.");

			var result = await _client.GetByGenre(genre);
			return result;
		}

		[HttpGet("{key}")]
		public async Task<Movie> MovieDetailsByKey(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
				throw new ArgumentNullException($"{nameof(key)} is required.");

			var result = await _client.GetDetails(key);
			return result;
		}

		[HttpGet("search-{searchKey}")]
		public async Task<List<Movie>> SearchMovies(string searchKey)
		{
			if (string.IsNullOrWhiteSpace(searchKey))
				throw new ArgumentNullException($"{nameof(searchKey)} is required.");

			var result = await _client.Search(searchKey);
			return result;
		}

		[HttpPost("create")]
		public async Task<Movie> Create([FromBody] Movie movie)
		{
			if (movie == null)
				throw new ArgumentNullException($"{nameof(movie)} is required.");

			var result = await _client.CreateMovie(movie);
			return result;
		}

		[HttpPost("update-{id}")]
		public async Task Update(int id, [FromBody] Movie movie)
		{
			if (movie == null)
				throw new ArgumentNullException($"{nameof(movie)} is required.");

			if (id == 0)
				throw new ArgumentNullException($"{nameof(id)} is required.");

			await _client.UpdateMovie(id, movie);
		}
	}
}
