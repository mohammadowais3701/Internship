using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TCPClient
{
    public class PortRequest : BasicMessage
    {
        private String licenseCode;
        private String hardDiskSerial;
        private String processorID;
        private String appPrefix;

        public PortRequest()
        {
            
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
    }
}
