using StocksAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksAPI.Data
{
    public class StocksRepository : IStocksRepository
    {
        public Task<Stock> Get(string symbol)
        {
            //Don't need to implement yet, since the unit test doesn't use the real one
            throw new NotImplementedException();
        }
    }
}
