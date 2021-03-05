using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotGenerator_Core
{
    public class LicenseCacheEntry
    {
        private String _licenseCode;
        private DateTime _time;

        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public String LicenseCode
        {
            get { return _licenseCode; }
            set { _licenseCode = value; }
        }

        public LicenseCacheEntry() { }

        public LicenseCacheEntry(DateTime time, String licenseCode)
        {
            this._licenseCode = licenseCode;
            this._time = time;
        }
    }
}
