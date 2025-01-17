using Thinktecture.EntityFrameworkCore.Testing;

namespace Thinktecture.Extensions.DbContextExtensionsTests;

// ReSharper disable once InconsistentNaming
public class GetLastUsedRowVersionAsync : IntegrationTestsBase
{
   public GetLastUsedRowVersionAsync(ITestOutputHelper testOutputHelper)
      : base(testOutputHelper, ITestIsolationOptions.SharedTablesAmbientTransaction)
   {
   }

   [Fact]
   public async Task Should_fetch_last_used_rowversion()
   {
      var rowVersion = await ActDbContext.GetLastUsedRowVersionAsync(CancellationToken.None);
      rowVersion.Should().NotBe(0);
   }
}
