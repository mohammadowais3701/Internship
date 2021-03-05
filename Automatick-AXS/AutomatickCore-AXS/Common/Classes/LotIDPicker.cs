using AccessRights;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TCPClient;

namespace Automatick.Core
{
    public class LotIDPicker
    {
        long currentLotIDIndex = 0;
        static LotIDPicker _lotIDPicker = null;
        private IMainForm _mainForm = null;
        private ApplicationPermissions _appPermissions = null;
        public List<String> LotID = null;
        private TcpClient client = null;
        private Boolean isConnected = false;
        Boolean result = false;
        private static LicenseCore _license;
        private String ipAddress = "mainserver.ticketpeers.com";
        private int port = 44000;
        private System.Threading.Timer _checkConnectionTimer = null;

        private object locker = new object();

        private LotIDPicker(IMainForm mainForm)
        {
            this._mainForm = mainForm;
            this._appPermissions = mainForm.AppPermissions;
            this.LotID = new List<string>();
            try
            {
                if (_checkConnectionTimer == null)
                {
                    //TimerCallback tcb = new TimerCallback(checkConnection);

                    //int time = 5 * 1000 * 60;
                    //this._checkConnectionTimer = new System.Threading.Timer(tcb, null, time, time);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public static LotIDPicker LotIDInstance
        {
            get
            {
                if (_lotIDPicker == null)
                {
                    throw new Exception("Lot ID picker Object not created");
                }
                return _lotIDPicker;
            }
        }

        public static void createLotIDInstance(IMainForm mainForm, LicenseCore lic)
        {
            _lotIDPicker = new LotIDPicker(mainForm);
            _license = lic;

        }

        public void setLotIDs()
        {
            try
            {
                String lotId = this.getLotId();

                this.makeLotIds(lotId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            #region commentOut
            //foreach (AccessList obj in allControlsAccesses)
            //{
            //    try
            //    {
            //        if (!String.IsNullOrEmpty(obj.name))
            //        {
            //            if (this.LotID == null)
            //            {
            //                this.LotID = new List<string>();
            //            }

            //            if (obj.name.Split(',') != null && obj.name.Split(',').Length > 1)
            //            {
            //                this.LotID = obj.name.Split(',').ToList();
            //            }
            //            else
            //            {
            //                this.LotID.Add(obj.name);
            //            }
            //        }

            //    }
            //    catch { }
            //}
            #endregion
        }

        String OLD_LOTS = String.Empty;

        public void makeLotIds(String lotId)
        {
            try
            {
                if (!String.IsNullOrEmpty(lotId))
                {
                    try
                    {
                        lock (locker)
                        {
                            Debug.WriteLine(lotId);

                            if (this.LotID == null)
                            {
                                this.LotID = new List<string>();

                                if (lotId.Split(',') != null && lotId.Split(',').Length > 1)
                                {
                                    this.LotID = lotId.Split(',').ToList();
                                }
                                else
                                {
                                    this.LotID.Add(lotId);
                                }
                            }

                            if (!OLD_LOTS.Equals(lotId))
                            {
                                OLD_LOTS = lotId;
                                this.LotID.Clear();

                                if (lotId.Split(',') != null && lotId.Split(',').Length > 1)
                                {
                                    this.LotID = lotId.Split(',').ToList();
                                }
                                else
                                {
                                    this.LotID.Add(lotId);
                                }
                                Interlocked.Exchange(ref currentLotIDIndex, 0);
                            }

                            foreach (var item in this.LotID)
                            {
                                try
                                {
                                    String[] args = item.Split('$');

                                    IEnumerable<AXSTicket> filterTickets = null;
                                    SortedBindingList.SortableBindingList<AXSTicket> tickets = this._mainForm.AppStartUp.Tickets;

                                    if (!String.IsNullOrEmpty(args[1]))
                                    {
                                        filterTickets = tickets.Where(p => (!String.IsNullOrEmpty(p.WR) && p.WR.Equals(args[0])) && (!String.IsNullOrEmpty(p.EventID) && p.EventID.Equals(args[1])));
                                    }
                                    else
                                    {
                                        filterTickets = tickets.Where(p => (!String.IsNullOrEmpty(p.WR) && p.WR.Equals(args[0])) && String.IsNullOrEmpty(p.EventID));
                                    }

                                    if (filterTickets != null)
                                    {
                                        if (filterTickets.Count() > 0)
                                        {
                                            for (int i = 0; i < filterTickets.Count(); i++)
                                            {
                                                AXSTicket ticket = filterTickets.ElementAt(i);

                                                if (ticket.VerifiedLotID == null)
                                                {
                                                    ticket.VerifiedLotID = new List<string>();
                                                }

                                                if (!ticket.VerifiedLotID.Contains(args[2]))
                                                {
                                                    ticket.VerifiedLotID.Add(args[2]);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
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
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public string GetNextLotID()
        {
            string LotID = string.Empty;
            try
            {
                lock (locker)
                {
                    Debug.WriteLine(Interlocked.Read(ref currentLotIDIndex));
                    if (Interlocked.Read(ref currentLotIDIndex) >= this.LotID.Count)
                    {
                        Interlocked.Exchange(ref currentLotIDIndex, 0);
                    }

                    if (this.LotID.Count > 0)
                    {
                        LotID = this.LotID[Convert.ToInt32(currentLotIDIndex)];
                        Interlocked.Increment(ref currentLotIDIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return LotID;
        }

        private String getLotId()
        {
            String lotId = String.Empty;
            try
            {
                client = new TcpClient(ipAddress, port);

                NetworkStream stream = client.GetStream();

                ServerRequestMessage msg = new ServerRequestMessage();

                msg.Command = "getAAXMagic";

                msg.AppPrefix = _license.AppPreFix;

                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(_license.LicenseCode);

                msg.LicenseCode = System.Convert.ToBase64String(plainTextBytes);

                msg.ProcessorID = _license.ProcessorID;

                msg.HardDiskSerial = _license.HardDiskSerial;

                byte[] buffer = Encoding.UTF8.GetBytes(TCPEncryptor.Encrypt(JsonConvert.SerializeObject(msg)) + "<EOF>");

                stream.Write(buffer, 0, buffer.Length);

                this.isConnected = true;

                String message = Msg.ReadMessage(stream);

                lotId = TCPEncryptor.Decrypt(message);

                Debug.WriteLine(lotId);

                result = true;

                Thread th = new Thread(this.takeUpdatedLotIds);

                th.Start();

                return lotId;
            }
            catch (Exception e)
            {
                client.Close();
                isConnected = false;
                Debug.WriteLine(e.Message + Environment.NewLine + e.StackTrace);

            }
            return lotId;
        }

        public List<String> getEventsLots(String wr, String eventID)
        {
            List<String> VerifiedLotID = new List<string>();
            try
            {
                List<String> str = this.LotID.Where(pred => pred.Split('$')[0].Equals(wr) && pred.Split('$')[1].Equals(eventID)).ToList();

                foreach (var item in str)
                {
                    String[] args = item.Split('$');

                    if (VerifiedLotID == null)
                    {
                        VerifiedLotID = new List<string>();
                    }

                    if (!VerifiedLotID.Contains(args[2]))
                    {
                        VerifiedLotID.Add(args[2]);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return VerifiedLotID;
        }

        private void takeUpdatedLotIds()
        {
            String lotId = String.Empty;
            TCPEncryptor encryptor = new TCPEncryptor();
            try
            {
                while (result)
                {
                    string message = Msg.ReadMessage(client.GetStream());

                    lotId = TCPEncryptor.Decrypt(message);

                    if (!String.IsNullOrEmpty(lotId))
                    {
                        if (!lotId.Equals("Zinda ho kay mar gayay"))
                        {
                            this.makeLotIds(lotId);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (client != null)
                    client.Close();
                isConnected = false;
            }
        }

        private void checkConnection(object obj)
        {
            if (!this.isConnected)
            {
                this.setLotIDs();
            }
        }

        public void closeClient()
        {
            if (client != null)
            {
                client.GetStream().Close();
                client.Close();
                client = null;
            }
        }

    }
}
