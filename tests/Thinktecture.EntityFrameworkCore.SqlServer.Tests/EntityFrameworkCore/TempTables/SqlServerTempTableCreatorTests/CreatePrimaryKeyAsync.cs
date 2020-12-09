using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Thinktecture.TestDatabaseContext;
using Xunit;
using Xunit.Abstractions;

namespace Thinktecture.EntityFrameworkCore.TempTables.SqlServerTempTableCreatorTests
{
   [Collection("BulkInsertTempTableAsync")]
   // ReSharper disable once InconsistentNaming
   public class CreatePrimaryKeyAsync : IntegrationTestsBase
   {
      private SqlServerTempTableCreator? _sut;
      private SqlServerTempTableCreator SUT => _sut ??= (SqlServerTempTableCreator)ActDbContext.GetService<ITempTableCreator>();

      public CreatePrimaryKeyAsync(ITestOutputHelper testOutputHelper)
         : base(testOutputHelper, true)
      {
      }

      [Fact]
      public async Task Should_create_primary_key_for_keylessType()
      {
         ConfigureModel = builder => builder.ConfigureTempTable<int>();

         await using var tempTableReference = await ArrangeDbContext.CreateTempTableAsync<TempTable<int>>(new TempTableCreationOptions
                                                                                                          {
                                                                                                             TableNameProvider = DefaultTempTableNameProvider.Instance,
                                                                                                             PrimaryKeyCreation = PrimaryKeyPropertiesProviders.None
                                                                                                          });

         var entityType = ActDbContext.GetEntityType<TempTable<int>>();
         await SUT.CreatePrimaryKeyAsync(ActDbContext, PrimaryKeyPropertiesProviders.AdaptiveForced.GetPrimaryKeyProperties(entityType, entityType.GetProperties().ToList()), tempTableReference.Name);

         var constraints = await AssertDbContext.GetTempTableConstraints<TempTable<int>>().ToListAsync();
         constraints.Should().HaveCount(1)
                    .And.Subject.First().CONSTRAINT_TYPE.Should().Be("PRIMARY KEY");

         var keyColumns = await AssertDbContext.GetTempTableKeyColumns<TempTable<int>>().ToListAsync();
         keyColumns.Should().HaveCount(1)
                   .And.Subject.First().COLUMN_NAME.Should().Be(nameof(TempTable<int>.Column1));
      }

      [Fact]
      public async Task Should_create_primary_key_for_entityType()
      {
         await using var tempTableReference = await ArrangeDbContext.CreateTempTableAsync<TestEntity>(new TempTableCreationOptions
                                                                                                      {
                                                                                                         TableNameProvider = DefaultTempTableNameProvider.Instance,
                                                                                                         PrimaryKeyCreation = PrimaryKeyPropertiesProviders.None
                                                                                                      });

         var entityType = ActDbContext.GetEntityType<TestEntity>();
         await SUT.CreatePrimaryKeyAsync(ActDbContext, PrimaryKeyPropertiesProviders.AdaptiveForced.GetPrimaryKeyProperties(entityType, entityType.GetProperties().ToList()), tempTableReference.Name);

         var constraints = await AssertDbContext.GetTempTableConstraints<TestEntity>().ToListAsync();
         constraints.Should().HaveCount(1)
                    .And.Subject.First().CONSTRAINT_TYPE.Should().Be("PRIMARY KEY");

         var keyColumns = await AssertDbContext.GetTempTableKeyColumns<TestEntity>().ToListAsync();
         keyColumns.Should().HaveCount(1)
                   .And.Subject.First().COLUMN_NAME.Should().Be(nameof(TestEntity.Id));
      }

      [Fact]
      public async Task Should_not_create_primary_key_if_key_exists_and_checkForExistence_is_true()
      {
#pragma warning disable 618
         await using var tempTableReference = await ArrangeDbContext.CreateTempTableAsync<TestEntity>(new TempTableCreationOptions
                                                                                                      {
                                                                                                         TableNameProvider = NewGuidTempTableNameProvider.Instance,
                                                                                                         PrimaryKeyCreation = PrimaryKeyPropertiesProviders.None
                                                                                                      });
#pragma warning restore 618
         var entityType = ArrangeDbContext.GetEntityType<TestEntity>();
         var keyProperties = PrimaryKeyPropertiesProviders.AdaptiveForced.GetPrimaryKeyProperties(entityType, entityType.GetProperties().ToList());
         await SUT.CreatePrimaryKeyAsync(ArrangeDbContext, keyProperties, tempTableReference.Name);

         SUT.Awaiting(sut => sut.CreatePrimaryKeyAsync(ActDbContext, keyProperties, tempTableReference.Name, true))
            .Should().NotThrow();
      }

      [Fact]
      public async Task Should_throw_if_key_exists_and_checkForExistence_is_false()
      {
#pragma warning disable 618
         await using var tempTableReference = await ArrangeDbContext.CreateTempTableAsync<TestEntity>(new TempTableCreationOptions
                                                                                                      {
                                                                                                         TableNameProvider = NewGuidTempTableNameProvider.Instance,
                                                                                                         PrimaryKeyCreation = PrimaryKeyPropertiesProviders.None
                                                                                                      });
         var entityType = ArrangeDbContext.GetEntityType<TestEntity>();
         var keyProperties = PrimaryKeyPropertiesProviders.AdaptiveForced.GetPrimaryKeyProperties(entityType, entityType.GetProperties().ToList());
         await SUT.CreatePrimaryKeyAsync(ArrangeDbContext, keyProperties, tempTableReference.Name);

         // ReSharper disable once RedundantArgumentDefaultValue
         SUT.Awaiting(sut => sut.CreatePrimaryKeyAsync(ActDbContext, keyProperties, tempTableReference.Name, false))
            .Should()
            .Throw<SqlException>();
      }
   }
}
