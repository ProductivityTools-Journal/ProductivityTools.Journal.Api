using Microsoft.EntityFrameworkCore;
using ProductivityTools.Journal.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducvitityTools.Journal.Queries
{
    static class QueriesHelper
    {
        internal static void ValidateOnershipCall(DbContext context, string email, int[] treeIds)
        {

            var o = DatabaseHelpers.ExecutVerifyOwnership(context, email, treeIds);
            if (o == false)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}

