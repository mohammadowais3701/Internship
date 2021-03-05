using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    public interface ITicketAccount
    {
        Boolean IfSelected
        {
            get;
            set;
        }

        String AccountName
        {
            get;
            set;
        }

        String AccountEmail
        {
            get;
            set;
        }

        String AccountPassword
        {
            get;
            set;
        }

        String CardLastDigits
        {
            get;
            set;
        }

        String CVV2
        {
            get;
            set;
        }

        String CardPassword
        {
            get;
            set;
        }

        int BuyingLimit
        {
            get;
            set;
        }


         string Mobile
        {
            get;
            set;
        }
         string CardType
        {
            get;
            set;
        }
       


         string Country
        {
            get;
            set;
        }

         string Title
        {
            get;
            set;
        }

         string FirstName
        {
            get;
            set;
        }

         string LastName
        {
            get;
            set;
        }

         string Address1
        {
            get;
            set;
        }

         string Address2
        {
            get;
            set;
        }

         string Address3
        {
            get;
            set;
        }

         string Town
        {
            get;
            set;
        }

        string PostCode
        {
            get;
            set;
        }

         string EmailAddress
        {
            get;
            set;
        }

         string ConfirmEmail
        {
            get;
            set;
        }

         string Telephone
        {
            get;
            set;
        }

         string CardNumber
        {
            get;
            set;
        }

         string StartMonth
        {
            get;
            set;
        }


         string StartYear
        {
            get;
            set;
        }

         string ExpiryMonth
        {
            get;
            set;
        }

         string ExpiryYear
        {
            get;
            set;
        }

         string IssueNum
        {
            get;
            set;
        }


         string CCVNum
        {
            get;
            set;
        }
    }
}
