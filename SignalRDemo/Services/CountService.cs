using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRDemo.Services
{
    public class CountService : ICountService
    {
        private int _count;

        public Task<int> GetCount()
        {
            return Task.Run(() => _count++);
        }
    }
}
