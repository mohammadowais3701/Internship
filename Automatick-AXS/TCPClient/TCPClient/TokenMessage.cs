using Automatick.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TCPClient
{
    public class TokenMessage
    {
        private String command;
        private String licenseCode;
        private String hardDiskSerial;
        private String processorID;
        private String type;
        private String appPrefix;
        private ClientProxy proxy;
        private String clientId;
        private String referer;
        private String _serviceName = String.Empty;
        private String _username = String.Empty;
        private String _password = String.Empty;
        private String _key = String.Empty;
        private String _ip = String.Empty;
        private String _host = String.Empty;
        private String _port = String.Empty;
        private Boolean audio;
        private String messageType;

        public String Site { get; set; }

        public String ServiceName
        {
            get { return _serviceName; }
            set { _serviceName = value; }
        }

        public String MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }

        public String Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public String Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public String Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

        public String Host
        {
            get { return _host; }
            set { _host = value; }
        }

        public String Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public String Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public String ClientId
        {
            get { return clientId; }
            set { clientId = value; }
        }

        public ClientProxy Proxy
        {
            get { return proxy; }
            set { proxy = value; }
        }

        public String Type
        {
            get { return type; }
            set { type = value; }
        }

        public String Command
        {
            get { return command; }
            set { command = value; }
        }

        public String LicenseCode
        {
            get { return licenseCode; }
            set { licenseCode = value; }
        }

        public String HardDiskSerial
        {
            get { return hardDiskSerial; }
            set { hardDiskSerial = value; }
        }

        public String ProcessorID
        {
            get { return processorID; }
            set { processorID = value; }
        }

        public String AppPrefix
        {
            get { return appPrefix; }
            set { appPrefix = value; }
        }

        public String Referer
        {
            get { return referer; }
            set { referer = value; }
        }
        public Boolean Audio
        {
            get { return audio; }
            set { audio = value; }
        }

        public TokenMessage()
        {
            this.Proxy = new ClientProxy();
            this.messageType = "TokenMessage";
        }
    }
}