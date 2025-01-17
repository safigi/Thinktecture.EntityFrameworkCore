using System.Data.Common;
using Thinktecture.Logging;

namespace Thinktecture.EntityFrameworkCore.Testing;

/// <summary>
/// Options for the <see cref="ITestDbContextProvider{T}"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class TestDbContextProviderOptions<T>
   where T : DbContext
{
   /// <summary>
   /// Master database connection.
   /// </summary>
   public DbConnection MasterConnection { get; set; }

   /// <summary>
   /// Determines whether and how to migrate the database.
   /// </summary>
   public IMigrationExecutionStrategy MigrationExecutionStrategy { get; set; }

   /// <summary>
   /// Options that use the <see cref="MasterConnection"/>.
   /// </summary>
   public DbContextOptions<T> MasterDbContextOptions { get; set; }

   /// <summary>
   /// Options that create a new connection.
   /// </summary>
   public DbContextOptions<T> DbContextOptions { get; set; }

   /// <summary>
   /// Contains executed commands if this feature was activated.
   /// </summary>
   public TestingLoggingOptions TestingLoggingOptions { get; set; }

   /// <summary>
   /// Callback to execute on every creation of a new <see cref="DbContext"/>.
   /// </summary>
   public IReadOnlyList<Action<T>> ContextInitializations { get; set; }

   /// <summary>
   /// Contains executed commands if this feature was activated.
   /// </summary>
   public IReadOnlyCollection<string>? ExecutedCommands { get; set; }

   /// <summary>
   /// Initializes new instance of <see cref="TestDbContextProviderOptions{T}"/>.
   /// </summary>
   protected TestDbContextProviderOptions(
      DbConnection masterConnection,
      IMigrationExecutionStrategy migrationExecutionStrategy,
      DbContextOptions<T> masterDbContextOptions,
      DbContextOptions<T> dbContextOptions,
      TestingLoggingOptions testingLoggingOptions,
      IReadOnlyList<Action<T>> contextInitializations)
   {
      MasterConnection = masterConnection;
      MigrationExecutionStrategy = migrationExecutionStrategy;
      MasterDbContextOptions = masterDbContextOptions;
      DbContextOptions = dbContextOptions;
      ContextInitializations = contextInitializations;
      TestingLoggingOptions = testingLoggingOptions;
   }
}
