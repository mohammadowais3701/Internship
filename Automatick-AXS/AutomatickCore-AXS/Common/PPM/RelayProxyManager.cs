using Automatick.Core;
using SortedBindingList;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxyManager
{
    public class RelayProxyManager : ProxyManager
    {
        //private List<Proxy> listUsedProxies = null;
        public ConcurrentDictionary<String, Proxy> listUsedProxies = new ConcurrentDictionary<String, Proxy>();
        private int usingGetNextProxyMethod = 0;

        private ReaderWriterLockSlim threadLockForList = new ReaderWriterLockSlim();
        private Timer countdownIntervalTimer = null;
        private Timer timerRetryLuminatiProxies = null;
        private int otherProxyLimit = 1;
        private String Username = String.Empty;
        private String Password = String.Empty;

        public override Boolean IsAllowed
        {
            get;
            set;
        }

        public RelayProxyManager(SortableBindingList<Proxy> customProxiesFromMainForm, String username, String password, Boolean isAllowed)
            : base(String.Empty, customProxiesFromMainForm)
        {
            this.Proxies = customProxiesFromMainForm;
            this.Username = username;
            this.Password = password;
            this.IsAllowed = isAllowed;
        }

        public override void Load(List<String> _lstProxy)
        {
            try
            {
                if (_lstProxy.Count > 0)
                {
                    this.populateRelayProxies(_lstProxy);

                    //Schedule after 1 min
                    countdownIntervalTimer = new Timer(countdownIntervalTimerCallBackHandler, null, 1 * 60 * 1000, -1);
                }
            }
            catch (Exception)
            {

            }
        }

        public virtual void populateRelayProxies(List<String> _proxies)
        {
            try
            {
                try
                {
                    for (int i = this.Proxies.Count - 1; i >= 0; i--)
                    {
                        if (this.Proxies[i].TheProxyType == Proxy.ProxyType.Relay)
                        {
                            this.Proxies.Remove(this.Proxies[i]);
                        }
                    }
                }
                catch { }

                for (int k = 0; k < _proxies.Count(); k++)
                {
                    String[] prox = _proxies[k].Split(':');

                    Proxy p = new Proxy();

                    p.TheProxyType = Proxy.ProxyType.Relay;
                    p.Address = prox[0];
                    p.Port = prox[1];
                    p.ProxyStatus = Proxy.proxyVerified;
                    p.LastUsed = DateTime.Now;
                    p.userName = Username;
                    p.Password = Password;

                    if (prox.Length == 3)
                    {
                        p.ReportingPort = prox[2];
                    }
                    else if (prox.Length == 5)
                    {
                        p.userName = prox[2];
                        p.Password = prox[3];
                        p.ReportingPort = prox[4];
                    }

                    this.Proxies.Add(p);
                }

                this.Proxies.Sort(this.Proxies.OrderBy(p => p.TheProxyType));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
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
                        if (this.Proxies[i].TheProxyType == Proxy.ProxyType.Relay)
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

        public override Proxy getNextProxy()
        {
            Proxy selectedProxy = null;

            try
            {
                IEnumerable<Proxy> proxiesFiltered = null;

                try
                {
                    proxiesFiltered = this.Proxies.Where(
                                                   pr => (Interlocked.Read(ref ProxyPicker.ProxyPickerInstance.globalproxyusage) < Interlocked.Read(ref ProxyPicker.ProxyPickerInstance.globalproxycount)) && pr.TheProxyType == Proxy.ProxyType.Relay
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
                            //if (!this.listUsedProxies.TryAdd(String.Format("{0}:{1}", item.Address, item.Port), item))
                            //{
                            //    continue;
                            //}
                            //else
                            {
                                selectedProxy = item;
                                item.LastUsed = DateTime.Now;

                                Interlocked.Increment(ref ProxyPicker.ProxyPickerInstance.globalproxyusage);

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
                //// Do this if proxy would not be realeased due to any/unknown reason, therefore timer will expire/remove this from listUsedProxies.
                //if (proxy != null)
                //{
                //    //lock (proxy)
                //    {
                //        proxy.LastUsed = DateTime.Now.AddMinutes(1);
                //    }
                //}

                Proxy removed = null;

                if (proxy != null)
                {
                    Interlocked.Decrement(ref ProxyPicker.ProxyPickerInstance.globalproxyusage);

                    //if (this.listUsedProxies.TryRemove(String.Format("{0}:{1}", proxy.Address, proxy.Port), out removed))
                    //{
                    //    Debug.WriteLine(String.Format("Removed {0}:{1}", proxy.Address, proxy.Port));
                    //}
                    //else
                    //{
                    //    Debug.WriteLine(String.Format("Couldn't remove {0}:{1}", proxy.Address, proxy.Port));
                    //}
                }
            }
            catch (Exception)
            {

            }

            try
            {
                //if (proxy != null)
                //{
                //    lock (this.listUsedProxies)
                //    {
                //        try
                //        {

                //            if (threadLockForList.TryEnterReadLock(500))
                //            {
                //                if (this.listUsedProxies.Contains(proxy))
                //                {
                //                    threadLockForList.ExitReadLock();

                //                    if (threadLockForList.TryEnterWriteLock(500))
                //                    {
                //                        listUsedProxies.Remove(proxy);
                //                        threadLockForList.ExitWriteLock();
                //                    }
                //                }
                //            }
                //        }
                //        catch
                //        {

                //        }
                //        finally
                //        {
                //            if (threadLockForList.IsReadLockHeld)
                //            {
                //                threadLockForList.ExitReadLock();
                //            }

                //            if (threadLockForList.IsWriteLockHeld)
                //            {
                //                threadLockForList.ExitWriteLock();
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception)
            {

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
            try
            {
                DateTime lastUsedDateTime = DateTime.Now.AddMinutes(-1 * minutes);

                //for (int i = this.listUsedProxies.Count - 1; i >= 0; i--)
                //{
                //    try
                //    {
                //        if (threadLockForList.TryEnterReadLock(3000))
                //        {
                //            Proxy proxy = this.listUsedProxies[i];
                //            if (proxy.LastUsed <= lastUsedDateTime)
                //            {
                //                threadLockForList.ExitReadLock();

                //                if (threadLockForList.TryEnterWriteLock(3000))
                //                {
                //                    lock (this.listUsedProxies)
                //                    {
                //                        this.listUsedProxies.Remove(proxy);
                //                    }

                //                    threadLockForList.ExitWriteLock();
                //                }
                //            }
                //        }
                //    }
                //    finally
                //    {
                //        if (threadLockForList.IsReadLockHeld)
                //        {
                //            threadLockForList.ExitReadLock();
                //        }

                //        if (threadLockForList.IsWriteLockHeld)
                //        {
                //            threadLockForList.ExitWriteLock();
                //        }
                //    }
                //    sleep(100);
                //}
            }
            catch { }
        }
    }
}
