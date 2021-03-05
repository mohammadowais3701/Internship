using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TCPClient
{
    public class PortResponse
    {
        private List<String> _ports = new List<string>();
        private String _error;
        private int _leaseTime;

        public int LeaseTime
        {
            get { return _leaseTime; }
            set { _leaseTime = value; }
        }

        public String Error
        {
            get { return _error; }
            set { _error = value; }
        }

        public List<String> Ports
        {
            get { return _ports; }
            set { _ports = value; }
        }
    }
}
