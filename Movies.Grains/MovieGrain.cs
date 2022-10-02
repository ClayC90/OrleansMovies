using Microsoft.Extensions.Logging;
using Movies.Contracts;
using Newtonsoft.Json;
using Orleans;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Grains
{
	//[StorageProvider(ProviderName = "Default")]
	public class MovieGrain : Grain<List<Movie>>, IMovieGrain
	{
		private const string _fileName = @"movies.json";
		private readonly ILogger _logger;

		public MovieGrain(ILogger<MovieGrain> logger)
		{
			_logger = logger;
		}

		public Task Initialize()
		{
			_logger.LogInformation("Movie grain initializing...");

			var jsonData = System.IO.File.ReadAllText(_fileName);
			var model = JsonConvert.DeserializeObject<MoviesModel>(jsonData);

			State = model.movies;

			_logger.LogInformation("Movie Grain Initialized.");
			return Task.CompletedTask;
		}

		public Task WriteStateAsync()
		{
			var json = JsonConvert.SerializeObject(State);
			System.IO.File.WriteAllText(_fileName, json);

			base.WriteStateAsync();

			return Task.CompletedTask;
		}

		public Task<List<Movie>> Get()
		{
			_logger.LogInformation("Getting all movies..");
			return Task.FromResult(State);
		}

		public Task<Movie> GetDetails(int id)
		{
			_logger.LogInformation("Getting movie details with id - {0}.", id);

			var result = Get().Result
				.SingleOrDefault(x => x.id.Equals(id));

			return Task.FromResult(result);
		}
		
		public Task<Movie> GetDetails(string key)
		{
			_logger.LogInformation("Getting movie details with key - {0}.", key);

			var result = Get().Result
				.SingleOrDefault(x => x.key.Equals(key, StringComparison.OrdinalIgnoreCase));

			return Task.FromResult(result);
		}
		
		public Task<bool> Exists(int id, string key)
		{
			_logger.LogInformation("Checking movie existance with id - {0} or key - {1}.", id, key);

			var result = Get().Result
				.Any(x => x.id.Equals(id) ||
						  x.key.Equals(key, StringComparison.OrdinalIgnoreCase));

			return Task.FromResult(result);
		}

		public Task<List<Movie>> GetTop(int amount = 5)
		{
			_logger.LogInformation("Getting top {0} highest rated movies.", amount);

			var result = Get().Result
				.OrderByDescending(x => x.rate)
				.Take(amount)
				.ToList();

			return Task.FromResult(result);
		}

		public Task<List<Movie>> GetByGenre(string genre)
		{
			_logger.LogInformation("Getting movies with genre - {0}", genre);

			var result = Get().Result
				.Where(x => x.genres.Contains(genre))
				.ToList();

			return Task.FromResult(result);
		}

		public Task<Movie> GetSelectedMovieDetail(string key)
		{
			_logger.LogInformation("Getting movies with key - {0}", key);

			var result = Get().Result
				.FirstOrDefault(x => x.key.Equals(key, StringComparison.OrdinalIgnoreCase));

			return Task.FromResult(result);
		}

		public Task<List<Movie>> Search(string searchKey)
		{
			_logger.LogInformation("Getting movies based on search filters - {0}", searchKey);

			//trim search keys incase trailing whitespaces were included.
			searchKey = searchKey.Trim();

			var items = Get().Result
				.Where(x => x.id.ToString().Equals(searchKey) ||
							x.key.Equals(searchKey, StringComparison.OrdinalIgnoreCase) ||
							x.name.ToLowerInvariant().Contains(searchKey.ToLowerInvariant()) ||
							x.rate.ToString().Equals(searchKey) ||
							x.genres.Contains(searchKey));

			return Task.FromResult(items.ToList());
		}

		public Task UpdateMovie(int id, Movie movie)
		{
			_logger.LogInformation("Updating movie with id - {0}", id);

			var item = GetDetails(id);
			if (item == null)
			{
				_logger.LogError("Movies does not exist with id - {0}", id);
				throw new Exception("Movie not found");
			}

			item.Result.length = movie.length;
			item.Result.img = movie.img;
			item.Result.rate = movie.rate;
			item.Result.description = movie.description;
			item.Result.name = movie.name;
			item.Result.genres = movie.genres;

			this.WriteStateAsync();
			return Task.CompletedTask;
		}

		public Task<Movie> CreateMovie(Movie movie)
		{
			_logger.LogInformation("Creating movie with key - {0}", movie.key);
			
			var movieExists = Exists(movie.id, movie.key).Result;
			if (movieExists)
			{
				_logger.LogError("Movie with id - {0} or key - {1} alredy exists", movie.id, movie.key);
				throw new Exception("Movie already exists.");
			}
			
			movie.id = Get().Result.Max(u => u.id) + 1;
			State.Add(movie);

			this.WriteStateAsync();
			return Task.FromResult(movie);
		}
	}
}
