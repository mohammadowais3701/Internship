using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient
{
    public class ServerRequestMessage
    {
        private String appPrefix;
        private String command;
        private String licenseCode;
        private String hardDiskSerial;
        private String processorID;

        public String AppPrefix
        {
            get { return appPrefix; }
            set { appPrefix = value; }
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

    }
}
