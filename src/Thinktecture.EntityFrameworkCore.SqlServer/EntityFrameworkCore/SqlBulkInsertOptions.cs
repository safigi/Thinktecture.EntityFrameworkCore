using System;
using System.Data;
using System.Data.SqlClient;

namespace Thinktecture.EntityFrameworkCore
{
   /// <summary>
   /// Options used by <see cref="DbContextExtensions.BulkInsertTempTableAsync{TColumn1}"/> and similar method overloads..
   /// </summary>
   public class SqlBulkInsertOptions
   {
      /// <summary>
      /// Timeout used by <see cref="SqlBulkCopy"/>
      /// </summary>
      public TimeSpan? BulkCopyTimeout { get; set; }

      /// <summary>
      /// Options used by <see cref="SqlBulkCopy"/>.
      /// </summary>
      public SqlBulkCopyOptions SqlBulkCopyOptions { get; set; }

      /// <summary>
      /// Batch size used by <see cref="SqlBulkCopy"/>.
      /// </summary>
      public int? BatchSize { get; set; }

      /// <summary>
      /// Enables or disables a <see cref="SqlBulkCopy"/> object to stream data from an <see cref="IDataReader"/> object.
      /// Default is set to <c>true</c>.
      /// </summary>
      public bool EnableStreaming { get; set; } = true;

      /// <summary>
      /// Indication whether the name of the temp table should be unique.
      /// Default is set to <c>true</c>.
      /// </summary>
      public bool MakeTableNameUnique { get; set; } = true;
   }
}