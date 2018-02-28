using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncReturnTypeLibrary
{
    public class AsyncExceptionManager
    {
        // Synchronous Code
        public string MineCoinFromForbiddenServer() 
        {
            var result = "Starting mining operation. ";
            var numberOfCoins = DontMineHere();
            if (numberOfCoins>0) {
                result += $"Success! Acquired {numberOfCoins} coins";
            }
            return result;
        }
        private int DontMineHere() 
        {
            throw new Exception("No, this is forbidden");
        }

        // Asynchronous Code
        public async Task<string> MineCoinFromForbiddenServerAsync() 
        {
            var result = "Starting mining operation. ";
            Task<int> forbiddenTask = DontMineHereAsync();
            try
            {
                var numberOfCoins = await forbiddenTask;
                if (numberOfCoins > 0) 
                {
                    result += $"Success! Acquired {numberOfCoins} coins";
                }    	
            }
            catch (System.Exception)
            {
                throw;
            }
            
            return result;
        }

        private async Task<int> DontMineHereAsync() 
        {
            await Task.Delay(1);
            throw new Exception("No, this is forbidden");           
        }

        // Parallel Code
        public string MineOnSeveralServers() 
        {
            var result = "Starting mining operation. ";
            var numberOfCoins = 0;
            var riskyTasks = new Task<int>[4];
            
            Task<int> forbiddenTask = DontMineHereAsync();
            riskyTasks[0] = forbiddenTask;
            
            Task<int> unreliableTask = UnreliableServerAsync();
            riskyTasks[1] = unreliableTask;

            Task<int> slowTask = SlowServerAsync();
            riskyTasks[2] = slowTask;

            Task<int> goodTask = MineAtThisGreatServer();
            riskyTasks[3] = goodTask;

            try
            {
                Task.WaitAll(riskyTasks);
                foreach(var operation in riskyTasks) 
                {
                    numberOfCoins += operation.Result;
                }
                if (numberOfCoins > 0) {
                    result += $"Success! Acquired {numberOfCoins} coins";
                }
            }
            catch (System.Exception)
            {
                
                throw;
            }
            return result;
        }

        private async Task<int> UnreliableServerAsync() 
        {
            await Task.Delay(1);
            throw new Exception("Server is down");
        }

        private async Task<int> SlowServerAsync() 
        {
            await Task.Delay(1);
            throw new Exception("Timeout from this server");            
        }

        private async Task<int> MineAtThisGreatServer() 
        {
            await Task.Delay(2000);
            return 100;
        }

        // Parallel Code (ForLoop)
        /*  
            Parallel loop will stop on its first unhandled exception, 
            but if more than one exception happens simultaneously, 
            the exceptions will get packaged up into an aggregate exception 
            that the loop throws
        */
        public int MineForCoinsWithParallelFor()
        {
            var coins=0;
            Parallel.For(1,500, i=> {           
                if (i % 100 == 0) 
                {
                    throw new Exception("Mining went wrong.");
                }
                else
                {
                    var rand = new Random();
                    Interlocked.Add(ref coins, rand.Next(15));
                }
            });
            return coins;
        }

    }
}