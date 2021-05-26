using Microsoft.VisualStudio.TestTools.UnitTesting;
using StocksAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using StocksAPI.Data;
using StocksAPI.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace StocksAPI.Controllers.Tests
{
    [TestClass()]
    public class StocksControllerTests
    {
        private StocksController Build(string symbol, Stock returns=null, Exception throws=null)
        {
            var cancelTokenSource = new CancellationTokenSource();

            var mockRepo = new Mock<IStocksRepository>();

            if (throws == null)
            {
                mockRepo.Setup(t => t.Get(symbol, cancelTokenSource.Token)).ReturnsAsync(returns);
            }
            else
            {
                mockRepo.Setup(t => t.Get(symbol, cancelTokenSource.Token)).ThrowsAsync(throws);
            }

            var stocksController = new StocksController(mockRepo.Object);
            stocksController.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                RequestAborted = cancelTokenSource.Token
            };
            return stocksController;
        }
        [TestMethod()]
        public async Task GetStockTest_WhenStockDoesntExist_ReturnsBadRequestError()
        {
            //arrange
            var symbol = "GMEEE";
            var stocksController = Build(symbol, returns: new NullStock());

            //act
            var result = await stocksController.Get(symbol) as ObjectResult;

            //assert
            Assert.AreEqual(HttpStatusCode.BadRequest, (HttpStatusCode)result.StatusCode);
            StringAssert.Contains(result.Value as string, symbol);

        }

        [TestMethod()]
        public async Task GetStockTest_WhenRequestCanceled_ReturnsBadRequestError()
        {
            //arrange
            var symbol = "GME";
            var stocksController = Build(symbol, throws: new TaskCanceledException());

            //act
            var result = await stocksController.Get(symbol) as ObjectResult;

            //assert
            Assert.AreEqual(HttpStatusCode.BadRequest, (HttpStatusCode)result.StatusCode);
            StringAssert.Contains(result.Value as string, "cancelled");

        }
        [TestMethod()]
        public async Task GetStockTest_WhenRepoThrows_ReturnsServerError()
        {
            //arrange
            var symbol = "GME";
            var stocksController = Build(symbol, throws: new NotImplementedException());

            //act
            var result = await stocksController.Get(symbol) as ObjectResult;

            //assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, (HttpStatusCode)result.StatusCode);

        }
        [TestMethod()]
        public async Task GetStockTest_ReturnsOKAndStock()
        {
            //arrange
            var symbol = "GME";
            var expectedStock = new Stock() 
            { 
                Name = "Gamestonk", 
                Symbol = symbol, 
                Price = 10_000_000, 
                QuoteTime = DateTimeOffset.Now 
            };
            var stocksController = Build(symbol, returns: expectedStock);


            //act
            var result = await stocksController.Get(symbol) as ObjectResult;

            //assert
            Assert.AreEqual(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
            Assert.AreSame(expectedStock, result.Value as Stock);
        }
    }
}