using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    [Serializable]
    public class VSTicketAccount : ITicketAccount
    {
        public bool IfSelected
        {
            get;
            set;
        }

        public string AccountName
        {
            get;
            set;
        }

        public string AccountEmail
        {
            get;
            set;
        }

        public string AccountPassword
        {
            get;
            set;
        }

        public string CardLastDigits
        {
            get;
            set;
        }

        public string CVV2
        {
            get;
            set;
        }

        public string CardPassword
        {
            get;
            set;
        }

        public int BuyingLimit
        {
            get;
            set;
        }

        public String CardNumber
        {
            get;
            set;
        }

        public String CardType
        {
            get;
            set;
        }

        public String ExpiryMonth
        {
            get;
            set;
        }

        public String ExpiryYear
        {
            get;
            set;
        }

        public String PhoneNumber
        {
            get;
            set;
        }

        public String AddressLine1
        {
            get;
            set;
        }

        public String AddressLine2
        {
            get;
            set;
        }

        public String City
        {
            get;
            set;
        }

        public String Province
        {
            get;
            set;
        }

        public String PostCode
        {
            get;
            set;
        }

        public String Country
        {
            get;
            set;
        }

        public String GroupName
        {
            get;
            set;
        }

        public VSTicketAccount()
        {
            this.IfSelected = true;
            this.BuyingLimit = 8;
            this.GroupName = "old";
        }

        public override string ToString()
        {
            String toString = this.AccountName;
            if (!String.IsNullOrEmpty(this.AccountEmail))
            {
                toString += " (" + this.AccountEmail + ")";
            }
            return toString;
        }

    }
}
