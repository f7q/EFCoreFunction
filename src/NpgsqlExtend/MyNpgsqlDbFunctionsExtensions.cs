using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore
{
    public static class MyNpgsqlDbFunctionsExtensions
    {
        /// <summary>
        ///     An implementation of the PostgreSQL PGroonga @&~ operation, which is an insensitive LIKE.
        /// </summary>
        /// <param name="_">The DbFunctions instance.</param>
        /// <param name="column">The string that is column.</param>
        /// <param name="param">The param</param>
        /// <returns>true if there is a match.</returns>
        public static bool FTS(
            this DbFunctions _,
            string column,
            string param)
            => FTSCore(column, param);


        private static bool FTSCore(string column, string param)
        {

            if (column == null
                || param == null)
            {
                return false;
            }

            /*if (column.Equals(param))
            {
                return true;
            }*/

            if (column.Length == 0
                || param.Length == 0)
            {
                return false;
            }

            return column.Equals(param);
        }
    }
}