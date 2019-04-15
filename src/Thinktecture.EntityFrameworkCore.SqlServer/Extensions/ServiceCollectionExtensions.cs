using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Thinktecture.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace Thinktecture
{
   /// <summary>
   /// Extensions for <see cref="IServiceCollection"/>.
   /// </summary>
   public static class ServiceCollectionExtensions
   {
      /// <summary>
      /// Adds a database context to dependency injection and registers additional components
      /// that handle with database schema changes at runtime.
      /// </summary>
      /// <param name="services">Service collection.</param>
      /// <param name="optionsAction">Allows further configuration of the database context.</param>
      /// <typeparam name="T">Type of the database context.</typeparam>
      /// <returns>Provided <paramref name="services"/>.</returns>
      /// <exception cref="ArgumentNullException">Service collection is <c>null</c>.</exception>
      public static IServiceCollection AddSchemaAwareSqlServerDbContext<T>([NotNull] this IServiceCollection services,
                                                                           [CanBeNull] Action<DbContextOptionsBuilder> optionsAction = null)
         where T : DbContext, IDbContextSchema
      {
         if (services == null)
            throw new ArgumentNullException(nameof(services));

         return services.AddDbContext<T>(builder =>
                                         {
                                            builder.AddSchemaAwareSqlServerComponents();
                                            optionsAction?.Invoke(builder);
                                         });
      }
   }
}