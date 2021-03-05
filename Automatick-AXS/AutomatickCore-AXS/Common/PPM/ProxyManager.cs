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

namespace ProxyManager
{
    public class ProxyManager : IDisposable
    {
        String urlSettings = "http://license.ticketpeers.com/PPM/LicenseSettings.asmx";
        public SortableBindingList<Proxy> Proxies;
        SettingsStruct SettingsFromServer;
        String CurrentISPIP = String.Empty;
        List<IGrouping<String, Proxy>> allGroupedProxies = null;
        int currentProxyIndex = 0;
        //List<Proxy> listUsedProxies = null;
        public Dictionary<Proxy, Timer> listUsedProxies = null;
        Timer refreshProxiesTimer = null;
        Timer updateIPAuthenticationTimer = null;

        public virtual Boolean IsAllowed
        {
            get;
            set;
        }

        public String LicenseCode
        {
            get;
            set;
        }
        public ProxyManager(String licenseCode, SortableBindingList<Proxy> customProxiesFromMainForm)
        {
            this.LicenseCode = licenseCode;
            this.Proxies = customProxiesFromMainForm;

        }

        public void setCounterToZero()
        {
            this.currentProxyIndex = 0;
        }

        public virtual void Load(List<String> _lstProxy)
        { }

        public virtual void Load()
        {
            try
            {
                listUsedProxies = new Dictionary<Proxy, Timer>();
                allGroupedProxies = new List<IGrouping<String, Proxy>>();
                if (!String.IsNullOrEmpty(LicenseCode))
                {
                    getSettingsFromServer();
                    getMyIpsFromServer();

                    updateIPAuthenticationTimer = new Timer(this.updateIPAuthenticationCallBackHandler, null, 0, SettingsFromServer.UpdateIPAuthenticationAfter * 60000);
                    refreshProxiesTimer = new Timer(this.refreshProxiesTimerCallBackHandler, null, SettingsFromServer.RefreshProxiesAfter * 60 * 60000, SettingsFromServer.RefreshProxiesAfter * 60 * 60000);
                }
            }
            catch { }
        }

        public virtual void Unload()
        {
            try
            {
                for (int i = this.Proxies.Count - 1; i >= 0; i--)
                {
                    if (this.Proxies[i].TheProxyType == Proxy.ProxyType.ISPIP || this.Proxies[i].TheProxyType == Proxy.ProxyType.MyIP)
                    {
                        this.Proxies.Remove(this.Proxies[i]);
                    }
                }

            }
            catch { }
        }

        protected void getSettingsFromServer()
        {
            SettingsFromServer = new SettingsStruct();

            System.Net.WebClient webClientCHK = new System.Net.WebClient();
            String resultCHK = webClientCHK.DownloadString(urlSettings + "/GetSettingsFromLicenseID?LicenseID=" + LicenseCode);
            if (!String.IsNullOrEmpty(resultCHK))
            {
                String[] strSplitted = resultCHK.Split(',');
                if (strSplitted.Length >= 7)
                {
                    SettingsFromServer.MaxAllowedConnectionCustomIP = int.Parse(strSplitted[0]);
                    SettingsFromServer.MaxAllowedConnectionISPIP = int.Parse(strSplitted[1]);
                    SettingsFromServer.MaxAllowedConnectionMyIp = int.Parse(strSplitted[2]);
                    SettingsFromServer.MaxAllowedCustomIPs = int.Parse(strSplitted[3]);
                    SettingsFromServer.MaxAllowedMyIPs = int.Parse(strSplitted[4]);
                    SettingsFromServer.UseCustomIPResponseTime = int.Parse(strSplitted[5]);
                    SettingsFromServer.UseMyIPResponseTime = int.Parse(strSplitted[6]);
                    try
                    {
                        SettingsFromServer.MaxPercentageOfUsage = int.Parse(strSplitted[7]);
                        SettingsFromServer.RefreshProxiesAfter = int.Parse(strSplitted[8]);
                        SettingsFromServer.notesForAutoIPAuthentication = strSplitted[9].Trim();
                        SettingsFromServer.UpdateIPAuthenticationAfter = int.Parse(strSplitted[10].Trim());
                    }
                    catch
                    {
                        SettingsFromServer.MaxPercentageOfUsage = 40;
                        SettingsFromServer.RefreshProxiesAfter = 3;
                        SettingsFromServer.notesForAutoIPAuthentication = "";
                        SettingsFromServer.UpdateIPAuthenticationAfter = 30;
                    }
                }
                else
                {
                    SettingsFromServer.MaxAllowedConnectionCustomIP = 1;
                    SettingsFromServer.MaxAllowedConnectionISPIP = 2;
                    SettingsFromServer.MaxAllowedConnectionMyIp = 1;
                    SettingsFromServer.MaxAllowedCustomIPs = 15;
                    SettingsFromServer.MaxAllowedMyIPs = 15;
                    SettingsFromServer.UseCustomIPResponseTime = 10;
                    SettingsFromServer.UseMyIPResponseTime = 10;
                    SettingsFromServer.MaxPercentageOfUsage = 40;
                    SettingsFromServer.RefreshProxiesAfter = 3;
                    SettingsFromServer.notesForAutoIPAuthentication = "";
                    SettingsFromServer.UpdateIPAuthenticationAfter = 30;
                }
            }
            else
            {
                SettingsFromServer.MaxAllowedConnectionCustomIP = 1;
                SettingsFromServer.MaxAllowedConnectionISPIP = 2;
                SettingsFromServer.MaxAllowedConnectionMyIp = 1;
                SettingsFromServer.MaxAllowedCustomIPs = 15;
                SettingsFromServer.MaxAllowedMyIPs = 15;
                SettingsFromServer.UseCustomIPResponseTime = 10;
                SettingsFromServer.UseMyIPResponseTime = 10;
                SettingsFromServer.MaxPercentageOfUsage = 40;
                SettingsFromServer.RefreshProxiesAfter = 3;
                SettingsFromServer.notesForAutoIPAuthentication = "";
                SettingsFromServer.UpdateIPAuthenticationAfter = 30;
            }

        }

        protected virtual void getMyIpsFromServer()
        {
            try
            {
                for (int i = this.Proxies.Count - 1; i >= 0; i--)
                {
                    if (this.Proxies[i].TheProxyType == Proxy.ProxyType.MyIP)
                    {
                        this.Proxies.Remove(this.Proxies[i]);
                    }
                }
            }
            catch { }

            System.Net.WebClient webClientCHK = new System.Net.WebClient();
            String resultCHK = webClientCHK.DownloadString(urlSettings + "/GetIPsFromLicenseID?LicenseID=" + LicenseCode);
            if (!String.IsNullOrEmpty(resultCHK))
            {
                String[] strSplitted = resultCHK.Split(',');
                if (strSplitted.Length > 0)
                {
                    foreach (String item in strSplitted)
                    {
                        String[] strItem = item.Split(':');
                        if (strItem.Length >= 2)
                        {
                            Proxy p = new Proxy();
                            p.TheProxyType = Proxy.ProxyType.MyIP;
                            p.Address = strItem[0].Trim();
                            p.Port = strItem[1].Trim();
                            if (strItem.Length >= 4)
                            {
                                p.UserName = strItem[2].Trim();
                                p.Password = strItem[3].Trim();
                            }
                            //p.ProxyStatus = Proxy.proxyNotVerified;
                            p.ProxyStatus = Proxy.proxyVerified;
                            Proxies.Add(p);
                        }
                    }

                    Proxies.Sort(Proxies.OrderBy(p => p.TheProxyType));

                    Thread th = new Thread(VerifyAll);
                    th.IsBackground = true;
                    th.Priority = ThreadPriority.Lowest;
                    th.Start();
                }
                else if (Proxies != null)
                {
                    if (Proxies.Count > 0)
                    {
                        for (int i = this.Proxies.Count - 1; i >= 0; i--)
                        {
                            if (this.Proxies[i].TheProxyType == Proxy.ProxyType.MyIP)
                            {
                                this.Proxies.Remove(this.Proxies[i]);
                            }
                        }
                        Thread th = new Thread(VerifyAll);
                        th.IsBackground = true;
                        th.Priority = ThreadPriority.Lowest;
                        th.Start();
                    }
                }
            }
            else if (Proxies != null)
            {
                if (Proxies.Count > 0)
                {
                    for (int i = this.Proxies.Count - 1; i >= 0; i--)
                    {
                        if (this.Proxies[i].TheProxyType == Proxy.ProxyType.MyIP)
                        {
                            this.Proxies.Remove(this.Proxies[i]);
                        }
                    }
                    Thread th = new Thread(VerifyAll);
                    th.IsBackground = true;
                    th.Priority = ThreadPriority.Lowest;
                    th.Start();
                }
            }
        }

        void getNewMyIpsFromServer(object obj)
        {
            try
            {
                if (obj != null)
                {
                    SortableBindingList<Proxy> OldProxies = (SortableBindingList<Proxy>)obj;
                    System.Net.WebClient webClientCHK = new System.Net.WebClient();
                    String resultCHK = webClientCHK.DownloadString(urlSettings + "/GetIPsFromLicenseID?LicenseID=" + LicenseCode);
                    if (!String.IsNullOrEmpty(resultCHK))
                    {
                        String[] strSplitted = resultCHK.Split(',');
                        if (strSplitted.Length > 0)
                        {
                            if (OldProxies == null)
                            {
                                OldProxies = new SortableBindingList<Proxy>();
                            }
                            else
                            {
                                SortableBindingList<Proxy> tmpMyIPProxies = new SortableBindingList<Proxy>();
                                foreach (String item in strSplitted)
                                {
                                    String[] strItem = item.Split(':');
                                    if (strItem.Length >= 2)
                                    {
                                        Proxy p = new Proxy();
                                        p.TheProxyType = Proxy.ProxyType.MyIP;
                                        p.Address = strItem[0].Trim();
                                        p.Port = strItem[1].Trim();
                                        if (strItem.Length >= 4)
                                        {
                                            p.UserName = strItem[2].Trim();
                                            p.Password = strItem[3].Trim();
                                        }

                                        //p.ProxyStatus = Proxy.proxyNotVerified;
                                        p.ProxyStatus = Proxy.proxyVerified;
                                        tmpMyIPProxies.Add(p);
                                    }
                                }
                                for (int i = OldProxies.Count - 1; i >= 0; i--)
                                {
                                    try
                                    {
                                        IEnumerable<Proxy> toCheck = tmpMyIPProxies.Where(r => (r.Address == OldProxies[i].Address && r.Port == OldProxies[i].Port));
                                        if (toCheck != null)
                                        {
                                            if (toCheck.Count() >= 1)
                                            {
                                                continue;
                                            }
                                            else if (OldProxies[i].TheProxyType == Proxy.ProxyType.MyIP)
                                            {
                                                OldProxies.RemoveAt(i);
                                            }
                                        }
                                        else if (OldProxies[i].TheProxyType == Proxy.ProxyType.MyIP)
                                        {
                                            OldProxies.RemoveAt(i);
                                        }
                                    }
                                    catch { }
                                }
                                tmpMyIPProxies.Clear();
                                tmpMyIPProxies = null;
                            }
                            if (OldProxies == null)
                            {
                                OldProxies = new SortableBindingList<Proxy>();
                            }
                            if (OldProxies != null)
                            {
                                if (OldProxies.Count <= 0)
                                {
                                    OldProxies = new SortableBindingList<Proxy>();
                                }
                            }
                            foreach (String item in strSplitted)
                            {
                                String[] strItem = item.Split(':');
                                if (strItem.Length >= 2)
                                {
                                    Proxy p = new Proxy();
                                    p.TheProxyType = Proxy.ProxyType.MyIP;
                                    p.Address = strItem[0].Trim();
                                    p.Port = strItem[1].Trim();
                                    if (strItem.Length >= 4)
                                    {
                                        p.UserName = strItem[2].Trim();
                                        p.Password = strItem[3].Trim();
                                    }
                                    //p.ProxyStatus = Proxy.proxyNotVerified;
                                    p.ProxyStatus = Proxy.proxyVerified;

                                    IEnumerable<Proxy> toCheck = OldProxies.Where(r => (r.Address == p.Address && r.Port == p.Port));

                                    if (toCheck != null)
                                    {
                                        if (toCheck.Count() >= 1)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            OldProxies.Add(p);
                                        }
                                    }
                                    else
                                    {
                                        OldProxies.Add(p);
                                    }

                                }
                            }
                            Thread th = new Thread(VerifyAll);
                            th.IsBackground = true;
                            th.Priority = ThreadPriority.Lowest;
                            th.Start();
                        }
                        else if (OldProxies != null)
                        {
                            if (OldProxies.Count > 0)
                            {
                                for (int i = this.Proxies.Count - 1; i >= 0; i--)
                                {
                                    if (this.Proxies[i].TheProxyType == Proxy.ProxyType.MyIP)
                                    {
                                        this.Proxies.Remove(this.Proxies[i]);
                                    }
                                }
                                Thread th = new Thread(VerifyAll);
                                th.IsBackground = true;
                                th.Priority = ThreadPriority.Lowest;
                                th.Start();
                            }
                        }

                    }
                    else if (OldProxies != null)
                    {
                        if (OldProxies.Count > 0)
                        {
                            for (int i = this.Proxies.Count - 1; i >= 0; i--)
                            {
                                if (this.Proxies[i].TheProxyType == Proxy.ProxyType.MyIP)
                                {
                                    this.Proxies.Remove(this.Proxies[i]);
                                }
                            }
                            Thread th = new Thread(VerifyAll);
                            th.IsBackground = true;
                            th.Priority = ThreadPriority.Lowest;
                            th.Start();
                        }
                    }
                }
            }
            catch { }
        }

        virtual public Proxy getNextProxy()
        {
            Proxy selectedProxy = null;
            if (allGroupedProxies == null)
            {
                allGroupedProxies = new List<IGrouping<String, Proxy>>();
            }

            if (listUsedProxies == null)
            {
                listUsedProxies = new Dictionary<Proxy, Timer>();
            }

            if (SettingsFromServer.MaxAllowedConnectionMyIp <= 0 && SettingsFromServer.MaxAllowedConnectionCustomIP <= 0)
            {
                getSettingsFromServer();
            }

            if (this.allGroupedProxies.Count <= 0)
            {
                try
                {
                    allGroupedProxies = new List<IGrouping<string, Proxy>>(Proxies.GroupBy(x => (!String.IsNullOrEmpty(x.Address) ? ((x.Address.Length > -1) ? x.Address.Substring(0, x.Address.Length) : x.Address) : "")));
                }
                catch (Exception)
                {
                    allGroupedProxies = new List<IGrouping<string, Proxy>>();
                }
            }

            if (this.allGroupedProxies.Count > 0)
            {
                if (this.currentProxyIndex >= this.allGroupedProxies.Count)
                {
                    this.currentProxyIndex = 0;
                }

                do
                {
                    //IEnumerable<Proxy> proxiesFiltered = this.allGroupedProxies[currentProxyIndex].Where(pr => (pr.ProxyConnections < pr.MaxProxyConnectionsAllowed && (pr.ProxyStatus == Proxy.proxyVerified) && pr.ProxyResponseTime <= pr.AllowedProxyResponseTime));
                    //IEnumerable<Proxy> proxiesFiltered = this.allGroupedProxies[currentProxyIndex].Where(pr => (pr.ProxyStatus == Proxy.proxyVerified) && ((pr.TheProxyType == Proxy.ProxyType.MyIP) ? pr.ProxyResponseTime <= SettingsFromServer.UseMyIPResponseTime : pr.ProxyResponseTime <= SettingsFromServer.UseCustomIPResponseTime) && (listUsedProxies.Count(q => q == pr) < SettingsFromServer.MaxAllowedConnectionMyIp));
                    IEnumerable<Proxy> proxiesFiltered = from pr in this.allGroupedProxies[currentProxyIndex]
                                                         where (pr.ProxyStatus == Proxy.proxyVerified) &&
                                                                ((pr.TheProxyType == Proxy.ProxyType.MyIP) ? pr.ProxyResponseTime <= SettingsFromServer.UseMyIPResponseTime : pr.ProxyResponseTime <= SettingsFromServer.UseCustomIPResponseTime) &&
                                                                (listUsedProxies.Count(q => q.Key == pr) < ((pr.TheProxyType == Proxy.ProxyType.MyIP) ? SettingsFromServer.MaxAllowedConnectionMyIp : (pr.TheProxyType == Proxy.ProxyType.Custom) ? SettingsFromServer.MaxAllowedConnectionCustomIP : SettingsFromServer.MaxAllowedConnectionISPIP))
                                                         select pr;


                    //this.allGroupedProxies[currentProxyIndex].Where(pr => (pr.ProxyStatus == Proxy.proxyVerified) && ((pr.TheProxyType == Proxy.ProxyType.MyIP) ? pr.ProxyResponseTime <= SettingsFromServer.UseMyIPResponseTime : pr.ProxyResponseTime <= SettingsFromServer.UseCustomIPResponseTime) && (listUsedProxies.Count(q => q == pr) < SettingsFromServer.MaxAllowedConnectionMyIp));

                    if (proxiesFiltered != null)
                    {
                        if (proxiesFiltered.Count() > 0)
                        {
                            selectedProxy = proxiesFiltered.OrderBy(x => x.LastUsed).Last();
                            this.currentProxyIndex++;
                            break;
                        }
                    }

                    this.currentProxyIndex++;
                } while (this.currentProxyIndex < this.allGroupedProxies.Count);

            }
            //
            try
            {
                if (selectedProxy != null)
                {
                    if (selectedProxy.TheProxyType == Proxy.ProxyType.MyIP)
                    {
                        ////IEnumerable<Proxy> proxiesFilteredAllVerified = from pr in this.Proxies
                        ////                                                where (pr.ProxyResponseTime <= SettingsFromServer.UseMyIPResponseTime && (pr.TheProxyType == Proxy.ProxyType.MyIP))
                        ////                                                select pr;
                        ////IEnumerable<Proxy> proxiesFilteredAllVerifiedCurrentlyUsed = from pr in Proxies
                        ////                                                             where (listUsedProxies.Count(q => q == pr) >= SettingsFromServer.MaxAllowedConnectionMyIp && 
                        ////                                                             pr.ProxyResponseTime <= SettingsFromServer.UseMyIPResponseTime && 
                        ////                                                             (pr.TheProxyType == Proxy.ProxyType.MyIP))
                        ////                                                             select pr;

                        IEnumerable<Proxy> proxiesFilteredAllVerified = from pr in this.Proxies
                                                                        where ((pr.TheProxyType == Proxy.ProxyType.MyIP))
                                                                        select pr;
                        IEnumerable<Proxy> proxiesFilteredAllVerifiedCurrentlyUsed = from pr in Proxies
                                                                                     where (listUsedProxies.Count(q => q.Key == pr) >= SettingsFromServer.MaxAllowedConnectionMyIp &&
                                                                                     (pr.TheProxyType == Proxy.ProxyType.MyIP))
                                                                                     select pr;
                        decimal dTotalProxies = (decimal)proxiesFilteredAllVerified.Count();
                        decimal dTotalProxiesUsed = (decimal)proxiesFilteredAllVerifiedCurrentlyUsed.Count();

                        decimal dCalculatePercent = dTotalProxiesUsed / dTotalProxies * 100;


                        if (dCalculatePercent >= SettingsFromServer.MaxPercentageOfUsage)
                        {
                            selectedProxy = null;
                        }
                    }
                }
            }
            catch { selectedProxy = null; }
            //
            if (selectedProxy != null)
            {
                selectedProxy.LastUsed = DateTime.Now;
                lock (listUsedProxies)
                {
                    if (!listUsedProxies.ContainsKey(selectedProxy))
                    {
                        // Automatically expire proxy in 10 minutes if not released.
                        TimeSpan tsDueTime = (selectedProxy.LastUsed.AddMinutes(10) - selectedProxy.LastUsed);
                        Timer autoProxyExpiryTimer = new Timer(this.autoProxyExpiryCallBackHandler, selectedProxy, tsDueTime, new TimeSpan(-1));

                        listUsedProxies.Add(selectedProxy, autoProxyExpiryTimer);                     
                    }
                }
            }
            return selectedProxy;
        }

        public virtual void ReleaseProxy(Proxy proxy)
        {
            try
            {
                if (proxy != null)
                {
                    if (listUsedProxies.ContainsKey(proxy))
                    {
                        lock (listUsedProxies)
                        {
                            try
                            {
                                listUsedProxies[proxy].Dispose();

                                GC.SuppressFinalize(listUsedProxies[proxy]);
                                GC.Collect();
                            }
                            catch { }

                            listUsedProxies[proxy] = null;
                            listUsedProxies.Remove(proxy);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void VerifyAll()
        {
            try
            {
                return;

                foreach (Proxy item in this.Proxies)
                {
                    item.ProxyStatus = Proxy.proxyWaiting;
                }

                foreach (Proxy item in this.Proxies)
                {
                    item.ProxyStatus = Proxy.proxyVerifying;
                    verifyProxies(item);
                }
            }
            catch { }
        }

        public void verifyProxies(object obj)
        {
            Proxy proxy = obj as Proxy;

            try
            {
                WebRequest request = WebRequest.Create("http://www.ticketmaster.com");
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                if (proxy.TheProxyType != Proxy.ProxyType.ISPIP)
                {
                    request.Proxy = new WebProxy("http://" + proxy.Address + ":" + proxy.Port.ToString());
                    if (!String.IsNullOrEmpty(proxy.UserName) && !String.IsNullOrEmpty(proxy.Password))
                    {
                        request.Proxy.Credentials = new NetworkCredential(proxy.UserName, proxy.Password);
                    }
                }
                request.Timeout = 30000;//60000 * 2;
                proxy.ProxyResponseTime = 0;
                TimeSpan ts = DateTime.Now.TimeOfDay;

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            string responseFromServer = "";

                            char[] buff = new char[5000];
                            reader.ReadBlock(buff, 0, 5000);
                            responseFromServer = new string(buff);

                            responseFromServer = responseFromServer.ToLower();
                            if (responseFromServer.Contains("403 Forbidden".ToLower()) || responseFromServer.Contains("Forbidden".ToLower()) || responseFromServer.Contains("You don't have permission to access".ToLower()))
                            {
                                proxy.ProxyStatus = Proxy.proxyBlocked;
                            }
                            else if (responseFromServer.Contains("<script type=\"text/javascript\">".ToLower()))
                            {
                                proxy.ProxyStatus = Proxy.proxyVerified;

                                ts = DateTime.Now.TimeOfDay.Subtract(ts);

                                if (ts.Seconds > 0)
                                {
                                    proxy.ProxyResponseTime = ts.Seconds;
                                }
                            }
                            else
                            {
                                proxy.ProxyStatus = Proxy.proxyNotWork;
                            }

                            return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("unable to connect to the remote server"))
                {
                    proxy.ProxyStatus = Proxy.proxyNotWork;
                }
                else if (ex.Message.ToLower().Contains("the operation has timed out"))
                {
                    proxy.ProxyStatus = Proxy.proxyTimeOut;
                }
                else
                {
                    proxy.ProxyStatus = Proxy.proxyNotWork;
                }

            }
        }

        private void UpdateIpAuthenticationThread()
        {
            Thread th = new Thread(new ParameterizedThreadStart(this.updateIpAuthentication));
            th.Priority = ThreadPriority.Lowest;
            th.IsBackground = true;
            th.SetApartmentState(ApartmentState.STA);
            th.Start(this.SettingsFromServer);
        }

        private string getMyISPIP()
        {

            String result = String.Empty;

            HtmlAgilityPack.BrowserSession bs = new HtmlAgilityPack.BrowserSession();
            try
            {
                result = bs.Get("http://who.is/users/ip");

                HtmlAgilityPack.HtmlNode hnode = bs.HtmlDocument.DocumentNode.SelectSingleNode("//a");
                if (hnode != null)
                {
                    result = hnode.InnerText.Trim();
                }
            }
            catch (Exception)
            {
                result = "";
            }

            return result;
        }

        void updateIpAuthentication(object o)
        {
            SettingsStruct settingsServer = (SettingsStruct)o;
            try
            {

                try
                {
                    String ip = getMyISPIP();
                    if (!String.IsNullOrEmpty(ip))
                    {
                        System.Net.WebClient webClientCHK = new System.Net.WebClient();
                        String resultCHK = webClientCHK.DownloadString(urlSettings + "/UpdateLastUsedIP?LicenseID=" + LicenseCode + "&IP=" + ip);
                    }
                }
                catch (Exception)
                {

                }
            }
            catch (Exception)
            {

            }
        }

        void refreshProxiesTimerCallBackHandler(Object stateInfo)
        {
            Thread th = new Thread(new ParameterizedThreadStart(getNewMyIpsFromServer));
            th.IsBackground = true;
            th.SetApartmentState(ApartmentState.STA);
            th.Priority = ThreadPriority.Lowest;
            th.Start(this.Proxies);
        }

        private void autoProxyExpiryCallBackHandler(Object stateInfo)
        {
            try
            {
                if (stateInfo is Proxy)
                {
                    Proxy proxy = (Proxy)stateInfo;
                    this.ReleaseProxy(proxy);
                }
            }
            catch (Exception)
            {

            }
        }

        void updateIPAuthenticationCallBackHandler(Object stateInfo)
        {
            UpdateIpAuthenticationThread();
        }

        public virtual void failed(String address)
        {
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (updateIPAuthenticationTimer != null)
                {
                    updateIPAuthenticationTimer.Dispose();
                }
                if (refreshProxiesTimer != null)
                {
                    refreshProxiesTimer.Dispose();
                }
            }
            catch (Exception)
            {

            }
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
