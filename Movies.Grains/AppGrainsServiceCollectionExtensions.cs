namespace Microsoft.Extensions.DependencyInjection
{
	public static class AppGrainsServiceCollectionExtensions
	{

		public static IServiceCollection AddAppGrains(this IServiceCollection services) => services;

		public static IServiceCollection AddAppHotsGrains(this IServiceCollection services) => services;

		public static IServiceCollection AddAppLoLGrains(this IServiceCollection services) => services;
	}
}
