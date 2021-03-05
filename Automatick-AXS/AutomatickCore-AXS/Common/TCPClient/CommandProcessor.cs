using Automatick.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAPipeLibrary;
using SASharedMessages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatick.Core
{
    public class CommandProcessor : IClientCommandProcessor
    {
        public static Boolean IsRelayProxiesAllowed = false;
        private static List<String> _relayProxies = null;

        public void process(string message)
        {
            Debug.WriteLine(message);
            try
            {

                String messageType = JObject.Parse(message)["messageType"].ToString();

                ServerAutomationPiperServer.Messages.AddOrUpdate(messageType, message, (key, oldValue) => message);

                ServerAutomationPiperServer.Push(messageType);

                Console.ForegroundColor = ConsoleColor.Yellow;

                if (messageType.Equals("CapsiumServersUpdate"))
                {
                    try
                    {
                        CapsiumServersUpdate capsiumServer = JsonConvert.DeserializeObject<CapsiumServersUpdate>(message);

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }

                else if (messageType.Equals("AAXMagicUpdate"))
                {
                    try
                    {
                        AAXMagicUpdate aaxMesage = JsonConvert.DeserializeObject<AAXMagicUpdate>(message);

                        if (!String.IsNullOrEmpty(aaxMesage.AAXMagic))
                        {
                            LotIDPicker.LotIDInstance.makeLotIds(aaxMesage.AAXMagic);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }

                else if (messageType.Equals("VMServersUpdate"))
                {
                    try
                    {
                        VMServersUpdate vmServer = JsonConvert.DeserializeObject<VMServersUpdate>(message);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }

                else if (messageType.Equals("ThresholdUpdate"))
                {
                    try
                    {
                        ThresholdUpdate threshold = JsonConvert.DeserializeObject<ThresholdUpdate>(message);

                        ServerPortsPicker.ServerPortsPickerInstance.LoadBalancerWorkers = threshold.ThresholdValue;
                        ServerPortsPicker.ServerPortsPickerInstance.LoadBalancerWorkers = 50;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }

                else if (messageType.Equals("C1AudioUpdate"))
                {
                    try
                    {
                        C1AudioUpdate audio = JsonConvert.DeserializeObject<C1AudioUpdate>(message);

                        ServerPortsPicker.ServerPortsPickerInstance.AC1Credential = audio.C1Key;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }
                else if (messageType.Equals("RelayServerUpdate"))
                {
                    try
                    {
                        RelayServerUpdate RelayServerUpdate = JsonConvert.DeserializeObject<RelayServerUpdate>(message);

                        if (RelayServerUpdate.RelayServers.Count > 0)
                        {

                            _relayProxies = new List<string>();
                            _relayProxies.AddRange(RelayServerUpdate.RelayServers);
                        }
                        else
                        {
                            string relay = "104.167.11.24:8887:9875,104.167.11.25:8887:9875,104.167.11.26:8887:9875,104.167.11.27:8887:9875,104.167.11.28:8887:9875,104.167.11.29:8887:9875,64.71.79.206:8887:9875,198.101.15.220:8887:9875,64.71.79.150:8887:9875,64.71.79.149:8887:9875,64.71.74.254:8887:9875,198.101.12.193:8887:9875,198.101.12.125:8887:9875,64.71.74.172:8887:9875,64.71.74.174:8887:9875,198.101.14.107:8887:9875,198.101.12.108:8887:9875,38.130.202.57:8887:9875,64.71.78.23:8887:9875,64.71.78.132:8887:9875";
                            _relayProxies = new List<string>();
                            _relayProxies.AddRange(relay.Split(','));
                        }

                        ListShuffleExtensions.Shuffle(RelayServerUpdate.RelayServers);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }
                else if (messageType.Equals("PercentRelayUpdate"))
                {
                    try
                    {
                        PercentRelayUpdate RelayServerUpdate = JsonConvert.DeserializeObject<PercentRelayUpdate>(message);

                        if (ProxyPicker.ProxyPickerInstance.ProxyRelayManager != null)
                        {
                            if (RelayServerUpdate.PercentRelayCount == 1 && ProxyPicker.ProxyPickerInstance.ProxyRelayManager.IsAllowed)
                            {
                                IsRelayProxiesAllowed = true;

                                Dictionary<Proxy.ProxyType, int> select = new Dictionary<Proxy.ProxyType, int>();

                                int remaining = 100 - ProxyPicker.ProxyPickerInstance.RelayCounter;

                                select.Add(Proxy.ProxyType.Relay, ProxyPicker.ProxyPickerInstance.RelayCounter);
                                select.Add(Proxy.ProxyType.All, remaining);

                                ProxyPicker.ProxyPickerInstance.Selector = new ProxySelector(select);
                                ProxyPicker.ProxyPickerInstance.ProxyRelayManager.Load(_relayProxies);
                            }
                            else
                            {
                                Dictionary<Proxy.ProxyType, int> select = new Dictionary<Proxy.ProxyType, int>();

                                select.Add(Proxy.ProxyType.All, 100);

                                ProxyPicker.ProxyPickerInstance.Selector = new ProxySelector(select);
                                ProxyPicker.ProxyPickerInstance.ProxyRelayManager.Unload();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

        } 
    }
}
