using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatick.Core
{
    public static class GoodProxies
    {
        static Random randomGenerator = new Random();
        public static ConcurrentBag<Proxy> _goodProxyList
        {
            get;
            set;
        }


        static public void Add(Proxy p)
        {
            if (_goodProxyList == null) _goodProxyList = new ConcurrentBag<Proxy>();

            if (p != null)
            {
                _goodProxyList.Add(p);
            }
        }

        //public static bool Sort()//this ConcurrentBag<Proxy> p
        //{
        //    try
        //    {
        //        _goodProxyList.OrderBy(cc => cc.ProxySortOrder == Proxy.ProxyPriority.TicketFound);
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public static Proxy getFoundProxy()
        {
            try
            {
                return GoodProxies._goodProxyList.Where(p => p.ProxySortOrder == Proxy.ProxyPriority.TicketFound).ElementAt(randomGenerator.Next(GoodProxies._goodProxyList.Count));
            }
            catch
            {
                return null;
            }
        }

        public static Proxy getProxy()
        {
            try
            {
                return GoodProxies._goodProxyList.Where(p => (p.ProxySortOrder == Proxy.ProxyPriority.FirstPage)).ElementAt(randomGenerator.Next(GoodProxies._goodProxyList.Count));
            }
            catch
            {
                return null;
            }
        }

    }
}
