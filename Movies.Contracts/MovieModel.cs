using System.Collections.Generic;

namespace Movies.Contracts
{
	/// <summary>
	/// Contains a list of all movies
	/// </summary>
	public class MoviesModel
	{
		public List<Movie> movies { get; set; }
	}

	public class Movie
	{
		public Movie()
		{
			genres = new string[] { };
		}

		public int id { get; set; }
		public string key { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public string[] genres { get; set; }
		public decimal rate { get; set; }
		public string length { get; set; }
		public string img { get; set; }
	}
}