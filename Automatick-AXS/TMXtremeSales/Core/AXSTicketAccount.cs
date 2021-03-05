using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    [Serializable]
    public class AXSTicketAccount:ITicketAccount
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


        #region AXS Account

        public string Mobile
        {
            get;
            set;
        }
        public string CardType
        {
            get;
            set;
        }
      

        public string Country
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string Address1
        {
            get;
            set;
        }

        public string Address2
        {
            get;
            set;
        }

        public string Address3
        {
            get;
            set;
        }

        public string Town
        {
            get;
            set;
        }

        public string PostCode
        {
            get;
            set;
        }

        public string EmailAddress
        {
            get;
            set;
        }

        public string ConfirmEmail
        {
            get;
            set;
        }

        public string Telephone
        {
            get;
            set;
        }

        public string CardNumber
        {
            get;
            set;
        }

        public string StartMonth
        {
            get;
            set;
        }


        public string StartYear
        {
            get;
            set;
        }

        public string ExpiryMonth
        {
            get;
            set;
        }

        public string ExpiryYear
        {
            get;
            set;
        }

        public string IssueNum
        {
            get;
            set;
        }


        public string CCVNum
        {
            get;
            set;
        }

       

        #endregion
        public AXSTicketAccount()
        {
            this.IfSelected = true;
            this.BuyingLimit = 8;
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
