using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace LotGenerator_Core
{
    public sealed class LicenseCache
    {
        private List<LicenseCacheEntry> _cache;
        private Timer _timer;
        private Object thisLock = new Object();
        private int mins = 1;
        private int checkIn = 1;

        public LicenseCache()
        {
            _cache = new List<LicenseCacheEntry>();
            _timer = new Timer(Callback, null, 0, checkIn * 60 * 1000);
        }

        private void Callback(object state)
        {
            lock (thisLock)
            {
                try
                {
                    int count = _cache.RemoveAll(p => (DateTime.Now.Subtract(p.Time).Minutes > mins));
                    if (count > 0)
                    {
                        Debug.WriteLine("[LicenseCache toRemove]=" + count);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(String.Format("[LicenseCache CallbackException]={0}", e.Message));
                }
            }
        }

        public void add(LicenseCacheEntry entry)
        {
            lock (thisLock)
            {
                try
                {
                    this._cache.Add(entry);
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine(e.Message);
                    throw e;
                }
            }
        }

        public bool hasLicense(String licenseCode)
        {
            try
            {
                lock (thisLock)
                {
                    if (this._cache.Where(p => p.LicenseCode.Equals(licenseCode)).ToList().Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
