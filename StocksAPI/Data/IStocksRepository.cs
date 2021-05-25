using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StocksAPI.Models;

namespace StocksAPI.Data
{
    public interface IStocksRepository
    {
        public Task<Stock> Get(string symbol, CancellationToken cancellationToken);
    }
}
