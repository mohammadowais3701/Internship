using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using SortedBindingList;
using System.Net;
using System.IO;
using System.Threading;
using HtmlAgilityPack;
using Automatick.Core;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace ProxyManager
{
    public class LuminatiProxyManager : ProxyManager
    {
        const string Username = "lum-customer-ticket_finder-zone-autoax";
        const string Password = "77f7bdba16e0";
        int luminatiProxiesCount;
        string luminatiProxiesCountry;
        bool usca = false;
        public ConcurrentDictionary<String, Proxy> listUsedProxies = new ConcurrentDictionary<String, Proxy>();
        private int usingGetNextProxyMethod = 0;

        private ReaderWriterLockSlim threadLockForList = new ReaderWriterLockSlim();
        private Timer countdownIntervalTimer = null;
        private Timer timerRetryLuminatiProxies = null;
        private int otherProxyLimit = 1;

        public LuminatiProxyManager(SortableBindingList<Proxy> customProxiesFromMainForm, int count, string country, bool usca)
            : base(String.Empty, customProxiesFromMainForm)
        {
            this.luminatiProxiesCount = count;
            this.luminatiProxiesCountry = country;
            this.usca = usca;
        }

        public override void Load()
        {
            try
            {
                //listUsedProxies = new List<Proxy>();

                this.getMyIpsFromServer();

                //Schedule after 1 min
                countdownIntervalTimer = new Timer(countdownIntervalTimerCallBackHandler, null, 1 * 60 * 1000, -1);
            }
            catch (Exception)
            {

            }
        }

        public override void Unload()
        {
            try
            {
                try
                {
                    for (int i = this.Proxies.Count - 1; i >= 0; i--)
                    {
                        if (this.Proxies[i].TheProxyType == Proxy.ProxyType.ISPIP || this.Proxies[i].TheProxyType == Proxy.ProxyType.MyIP || this.Proxies[i].TheProxyType == Proxy.ProxyType.Luminati)
                        {
                            this.Proxies.Remove(this.Proxies[i]);
                        }
                    }
                }
                catch { }

                try
                {
                    if (countdownIntervalTimer != null)
                    {
                        countdownIntervalTimer.Dispose();
                    }
                }
                catch (Exception)
                {

                }


                try
                {
                    if (timerRetryLuminatiProxies != null)
                    {
                        timerRetryLuminatiProxies.Dispose();
                    }
                }
                catch (Exception)
                {

                }

            }
            catch (Exception)
            {

            }
            finally
            {
                countdownIntervalTimer = null;
                timerRetryLuminatiProxies = null;
            }

        }

        public virtual void countdownIntervalTimerCallBackHandler(object stateInfo)
        {
            try
            {
                autoReleaseProxies(20);

                sleep(1000);

                autoResetStatusOfNonWorkingProxies(5);

                rescheduleAutoProxiesReleaseTimer(1);
            }
            catch (Exception)
            {

            }
        }

        private void rescheduleAutoProxiesReleaseTimer(int minutes)
        {
            try
            {
                //reschedule after 1 min
                if (countdownIntervalTimer == null)
                {
                    countdownIntervalTimer = new Timer(countdownIntervalTimerCallBackHandler, null, minutes * 60 * 1000, -1);
                }
                else
                {
                    countdownIntervalTimer.Change(1 * 60 * 1000, -1);
                }
            }
            catch (Exception)
            {
                try
                {
                    countdownIntervalTimer.Dispose();
                }
                catch (Exception)
                {

                }

                countdownIntervalTimer = new Timer(countdownIntervalTimerCallBackHandler, null, 1 * 60 * 1000, -1);
            }
        }

        private void autoResetStatusOfNonWorkingProxies(int minutes)
        {
            //Change status of non-working/non-verfied proxy to working, that was not being used in 5 mins
            try
            {
                DateTime lastUsedDateTime = DateTime.Now.AddMinutes(-1 * minutes);

                IEnumerable<Proxy> filterProxies = this.Proxies.Where(p => p.ProxyStatus != Proxy.proxyVerified && p.LastUsed <= lastUsedDateTime);

                if (filterProxies != null)
                {
                    int filterProxiesCount = filterProxies.Count();

                    if (filterProxiesCount > 0)
                    {
                        for (int i = 0; i < filterProxiesCount; i++)
                        {
                            try
                            {
                                Proxy proxy = filterProxies.ElementAt(i);

                                proxy.ProxyStatus = Proxy.proxyVerified;
                            }
                            catch
                            {

                            }

                            sleep(100);
                        }
                    }
                }

            }
            catch (Exception)
            {

            }
        }

        private void autoReleaseProxies(int minutes)
        {
            // Check to auto release proxies that are not being used within last 20 mins
            //try
            //{
            //    DateTime lastUsedDateTime = DateTime.Now.AddMinutes(-1 * minutes);

            //    while (true)
            //    {
            //        bool gotWLock = threadLockForList.TryEnterWriteLock(500);

            //        try
            //        {
            //            if (gotWLock)
            //            {
            //                try
            //                {
            //                    //Debug.WriteLine(String.Format("{0} values removed from listUsedProxies by autoReleaseProxies()", this.listUsedProxies.RemoveAll(p => p.LastUsed <= lastUsedDateTime)));
            //                }
            //                catch (Exception ex)
            //                {
            //                    Debug.WriteLine(ex.Message);
            //                }

            //                break;
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Debug.WriteLine(ex.Message);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex.Message);
            //}
            //finally
            //{
            //    if (threadLockForList.IsWriteLockHeld)
            //    {
            //        threadLockForList.ExitWriteLock();
            //    }
            //}
        }

        protected override void getMyIpsFromServer()
        {
            try
            {
                populateLuminatiProxies();
            }
            catch (Exception)
            {

            }
        }

        private void populateLuminatiProxies()
        {
            try
            {
                int retryAttempts = 0;

            retry:

                if (retryAttempts >= 5)
                {
                    if (timerRetryLuminatiProxies != null)
                    {
                        try
                        {
                            timerRetryLuminatiProxies.Dispose();
                        }
                        catch (Exception)
                        {

                        }

                        timerRetryLuminatiProxies = null;
                    }

                    timerRetryLuminatiProxies = new Timer(delegate { populateLuminatiProxies(); }, null, 60000, -1);

                    return;
                }
                else
                {
                    if (timerRetryLuminatiProxies != null)
                    {
                        try
                        {
                            timerRetryLuminatiProxies.Dispose();
                        }
                        catch (Exception)
                        {

                        }
                    }

                    timerRetryLuminatiProxies = null;
                }

                retryAttempts++;

                String superProxyCountry = "us";

                if (this.luminatiProxiesCountry == "any" || this.luminatiProxiesCountry == "us")
                {
                    superProxyCountry = "us";
                }
                else if (this.luminatiProxiesCountry == "gb")
                {
                    superProxyCountry = "gb";
                }
                else if (this.luminatiProxiesCountry == "au")
                {
                    superProxyCountry = "gb";
                }
                else if (this.luminatiProxiesCountry == "ca")
                {
                    superProxyCountry = "us";
                }


                string[] superProxy;
                const int Port = 22225;



                try
                {
                    for (int i = this.Proxies.Count - 1; i >= 0; i--)
                    {
                        if (this.Proxies[i].TheProxyType == Proxy.ProxyType.Luminati && this.Proxies[i].IfLuminatiProxy)
                        {
                            this.Proxies.Remove(this.Proxies[i]);
                        }
                    }
                }
                catch { }

                try
                {
                    using (var client = new WebClient())
                    {
                        superProxy = client.DownloadString(String.Format(
                             "http://client.luminati.io/api/get_super_proxy?raw=1&country=" + superProxyCountry + "&user={0}&key={1}&limit={2}",
                             Username,
                             Password,
                             this.luminatiProxiesCount.ToString()
                         )).Split(',');
                    }

                    if (superProxy.Length > 0)
                    {
                        for (int k = 0; k < superProxy.Count(); k++)
                        {
                            if (this.usca)
                            {
                                if (k % 2 == 0)
                                {
                                    this.luminatiProxiesCountry = "us";
                                }
                                else
                                {
                                    this.luminatiProxiesCountry = "ca";
                                }
                            }
                            Proxy p = new Proxy(this.luminatiProxiesCountry);

                            //p.TheProxyType = Proxy.ProxyType.MyIP;
                            p.Port = Port.ToString();

                            p.UserName = Username;
                            p.Password = Password;
                            p.Address = superProxy[k];

                            if (p.IfValidLuminatiProxy)
                            {
                                p.ProxyStatus = Proxy.proxyVerified;
                                p.LastUsed = DateTime.Now;

                                this.Proxies.Add(p);
                            }
                        }

                        this.Proxies.Sort(this.Proxies.OrderBy(p => p.TheProxyType));
                    }
                    else
                    {
                        sleep(1000);
                        goto retry;
                    }

                }
                catch
                {
                    sleep(1000);
                    goto retry;
                }

            }
            catch
            {

            }
        }

        public override Proxy getNextProxy()
        {
            Proxy selectedProxy = null;

            try
            {
                IEnumerable<Proxy> proxiesFiltered = null;

                try
                {
                    proxiesFiltered = this.Proxies.Where(
                              pr => (Interlocked.Read(ref ProxyPicker.ProxyPickerInstance.globalproxyusage) < Interlocked.Read(ref ProxyPicker.ProxyPickerInstance.globalproxycount)) && pr.TheProxyType == Proxy.ProxyType.Luminati
                                  ).OrderBy(x => x.LastUsed);
                }
                catch (Exception)
                {
                    proxiesFiltered = null;
                }


                if ((proxiesFiltered != null) && (proxiesFiltered.Count() > 0))
                {
                    try
                    {
                        foreach (var item in proxiesFiltered)
                        {
                            if (!this.listUsedProxies.TryAdd(String.Format("{0}:{1}", item.Address, item.Port), item))
                            {
                                continue;
                            }
                            else
                            {
                                selectedProxy = item;
                                item.LastUsed = DateTime.Now;

                                if (item.TheProxyType == Proxy.ProxyType.Luminati)
                                {
                                    Interlocked.Increment(ref ProxyPicker.ProxyPickerInstance.globalproxyusage);
                                }

                                break;
                            }
                        }
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

            return selectedProxy;
        }

        protected void sleep(int timeOut)
        {
            try
            {
                using (AutoResetEvent are = new AutoResetEvent(false))
                {
                    are.WaitOne(timeOut);
                }

            }
            catch (Exception)
            {

            }
        }

        public override void ReleaseProxy(Proxy proxy)
        {
            try
            {
                Proxy removed = null;

                if (proxy != null)
                {
                    if (proxy.TheProxyType == Proxy.ProxyType.Luminati)
                    {
                        Interlocked.Decrement(ref ProxyPicker.ProxyPickerInstance.globalproxyusage);
                    }

                    if (this.listUsedProxies.TryRemove(String.Format("{0}:{1}", proxy.Address, proxy.Port), out removed))
                    {
                        Debug.WriteLine(String.Format("Removed {0}:{1}", proxy.Address, proxy.Port));
                    }
                    else
                    {
                        Debug.WriteLine(String.Format("Couldn't remove {0}:{1}", proxy.Address, proxy.Port));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
