using System;
using Xunit;
using AsyncReturnTypeLibrary;
using System.Threading.Tasks;

namespace AsyncReturnTypes.xUnit
{
    public class AsyncReturnTypeManagerTest
    {
        private AsyncReturnTypeManager _returnTypeMgr;

        public AsyncReturnTypeManagerTest() 
        {
            _returnTypeMgr = new AsyncReturnTypeManager();
        }
                
        [Fact]
        public void Test1()
        {

        }

        [Fact]
        public async void AsyncManagerCanPause5SecondsWithTask() 
        {
            var startTime = DateTime.UtcNow;
            await _returnTypeMgr.PauseFiveSecondsAsync();
            var diffInSeconds = (DateTime.UtcNow - startTime).TotalSeconds;
            Assert.True(diffInSeconds>=5, "Did not pause for at least five seconds");
        }

        [Fact]
        public void AsyncManagerCantPause6SecondsWithVoid() 
        {
            var startTime = DateTime.UtcNow;
            _returnTypeMgr.PauseSixSecondsAsync();
            var diffInSeconds = (DateTime.UtcNow-startTime).TotalSeconds;
            Assert.True(diffInSeconds<5,"The void method pauses for five seconds or more");        
        }

        [Fact]
        public async Task AsyncManagerCanCallWebservice() 
        {
            var startTime = DateTime.UtcNow;
            await _returnTypeMgr.CallWebServiceThatDoesNotReturnResults();
            var diffInSeconds = (DateTime.UtcNow-startTime).TotalSeconds;
            Assert.True(diffInSeconds>=2,"Did not mine for at least two seconds");
        }

        // Asynchronous Code that returns Task<T>
        [Fact]
        public async Task AsyncManagerCanCallWebServiceWithResult() 
        {
            string resultString = await _returnTypeMgr.CallWebServiceThatReturnsString();
            Assert.True(resultString.Contains("AsyncCoin"),"Did not return correct results");
        }

        // Asynchronous Code that returns ValueTask<T>
        [Fact]
        public async Task AsyncManagerCanCaptureChangedCoinPrice() 
        {
            var initialPrice = await _returnTypeMgr.GetCurrentCoinPrice();
            var cachedPrice = await _returnTypeMgr.GetCurrentCoinPrice();
            await Task.Delay(3500);
            var changedPrice = await _returnTypeMgr.GetCurrentCoinPrice();
            Assert.Equal(initialPrice,cachedPrice);
            Assert.NotEqual(cachedPrice, changedPrice);
        }
    }
}
