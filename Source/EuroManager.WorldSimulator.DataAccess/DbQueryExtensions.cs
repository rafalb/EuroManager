using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.DataAccess
{
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Db", Justification = "Preferred convention")]
    public static class DbQueryExtensions
    {
        public static DbQuery<T> ReadOnly<T>(this DbQuery<T> query, bool readOnly)
        {
            return readOnly ? query.AsNoTracking() : query;
        }
    }
}
