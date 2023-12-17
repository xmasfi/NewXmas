using System;
using NewShore.Persistence;

namespace NewShore.Api.FunctionalTests.Infrastructure
{
    public class DataBaseSeeder
    {
        public static void Initialize(NewShoreDbContext context)
        {
            Seedflights(context);
        }

        private static void Seedflights(NewShoreDbContext context)
        {
            throw new NotImplementedException();
        }
    }
}