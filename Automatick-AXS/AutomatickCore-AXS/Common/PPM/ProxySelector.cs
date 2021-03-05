using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automatick.Core
{
    public class ProxySelector
    {
        private List<Proxy.ProxyType> ProxySelectionCriteria;
        private int pointer = 0;
        private Object locker = new object();

        public ProxySelector(Dictionary<Proxy.ProxyType, int> proxySelectionCriteria)
        {
            this.ProxySelectionCriteria = new List<Proxy.ProxyType>();
            this.ValidateCriteria(proxySelectionCriteria);
            this.Distribute(proxySelectionCriteria);
            ProxySelectionCriteria.Shuffle();
        }

        private void ValidateCriteria(Dictionary<Proxy.ProxyType, int> proxySelectionCriteria)
        {
            if (proxySelectionCriteria != null)
            {
                int sum = 0;

                foreach (int val in proxySelectionCriteria.Values)
                {
                    sum += val;
                }

                if (!sum.Equals(100))
                {
                    throw new Exception("Invalid criteria specified. Criteria should sum up to 100%");
                }
            }
        }

        private void Distribute(Dictionary<Proxy.ProxyType, int> proxySelectionCriteria)
        {
            try
            {
                foreach (var item in proxySelectionCriteria)
                {
                    for (int i = 0; i < proxySelectionCriteria[item.Key]; i++)
                    {
                        ProxySelectionCriteria.Add(item.Key);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public Proxy.ProxyType SelectProxy()
        {
            Proxy.ProxyType result = Proxy.ProxyType.Custom;

            try
            {
                lock (this.locker)
                {
                    try
                    {
                        if (pointer == (ProxySelectionCriteria.Count - 1))
                            pointer = 0;

                        result = this.ProxySelectionCriteria[pointer];
                        pointer++;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }
    }

    public static class ListShuffleExtensions
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
