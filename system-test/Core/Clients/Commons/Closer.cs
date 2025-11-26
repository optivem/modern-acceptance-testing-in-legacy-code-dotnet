using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optivem.EShop.SystemTest.Core.Clients.Commons
{
    public static class Closer
    {
        public static async Task CloseAsync<T>(T client) where T : IAsyncDisposable
        {
            if (client != null)
            {
                await client.DisposeAsync();
            }
        }

        public static void Close<T>(T client) where T : IDisposable
        {
            client?.Dispose();
        }
    }

}
