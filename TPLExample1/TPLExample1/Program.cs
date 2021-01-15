using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TPLExample1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
      
       
            var fun = new Func<int,CancellationToken,int>((n,t) => {
                int sum = 0;
                try
                {
                    for (int i = 1; i <= n; i++)
                    {
                        if (t.IsCancellationRequested)
                        {
                            Console.WriteLine("Cancellation Request by User");
                            return sum;
                        }
                        sum += i;
                        Console.WriteLine(i);
                        Task.Delay(500).Wait();
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
                return sum;
            });
            try
            {
                Task<int> t1 = new Task<int>(() => { return fun(10, token); });
                Task<float> t2 = t1.ContinueWith((mytask) =>
                {
                    return (float)Math.Sqrt(mytask.Result);
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
                t1.Start();
                ConsoleKeyInfo key;
                do
                {
                    while (!Console.KeyAvailable)
                    {
                        if (t1.IsCompleted)
                        {
                            goto finsh;
                        }

                    }
                    key = Console.ReadKey(true);

                } while (key.Key != ConsoleKey.Escape);


                source.Cancel();
finsh:
                Console.WriteLine(t1.Result);
                Console.WriteLine(t2.Result);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

        }

        
    }
}
