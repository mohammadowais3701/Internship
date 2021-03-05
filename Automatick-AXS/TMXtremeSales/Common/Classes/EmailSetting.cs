using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SortedBindingList;
using System.Threading;
using System.Net.Mail;

namespace Automatick.Core
{
    [Serializable]
    public class EmailSetting
    {
        public String EmailAddress
        {
            get;
            set;
        }
        public String EmailPassword
        {
            get;
            set;
        }
        public String SMTPServer
        {
            get;
            set;
        }
        public decimal SMTPPort
        {
            get;
            set;
        }
        public Boolean IsSSLRequired
        {
            get;
            set;
        }
        public SortableBindingList<Email> EmailAddresses
        {
            get;
            set;
        }

        public EmailSetting()
        {
            this.SMTPPort = 587;
            this.EmailAddresses = new SortableBindingList<Email>();
        }

        public void sendFoundEmail(ITicketSearch search)
        {
            if (search.Ticket.ifSendEmail)
            {
                Thread th = new Thread(new ParameterizedThreadStart(this.sendFoundEmailThreadHandler));
                th.Priority = ThreadPriority.Highest;
                th.SetApartmentState(ApartmentState.STA);
                th.IsBackground = true;
                th.Start(search);
            }                
        }

        private void sendFoundEmailThreadHandler(Object o)
        {
            ITicketSearch search = (ITicketSearch)o;
            try
            {
                if (this.EmailAddresses == null)
                {                    
                    search.MoreInfo = "Email could not be sent because no email(s) specified in the email settings. " + search.MoreInfo;
                    return;
                }
                else if (this.EmailAddresses.Count <=0)
                {
                    search.MoreInfo = "Email could not be sent because no email(s) specified in the email settings. " + search.MoreInfo;
                    return;
                }
                else if (String.IsNullOrEmpty(this.EmailAddress) || String.IsNullOrEmpty(this.EmailPassword) || String.IsNullOrEmpty(this.SMTPServer))
                {
                    search.MoreInfo = "Email did not send because invalid sender email configuration. " + search.MoreInfo;
                    return;
                }
                
                string subject, body, ToAddress;
                
                search.MoreInfo = "Sending email. " + search.MoreInfo;
                
                IEnumerable<String> strEmails = from p in EmailAddresses
                                                select p.EmailAddress;                

                String[] toEmails  = strEmails.ToArray();

                ToAddress = String.Join(",", toEmails);

                body = string.Format(global::Automatick.Properties.Resources.EmailBody, search.Ticket.TicketName, search.Quantity, search.Section, search.Row, search.Seat, search.Price, search.Ticket.URL, ((AXSSearch)search).TmEvent.EventName);
                
                subject = "T ALERT: " + search.Ticket.TicketName + " Tickets Found";

                using (MailMessage mailMsg = new MailMessage(this.EmailAddress, ToAddress.Trim()))
                {
                    mailMsg.IsBodyHtml = true;
                    mailMsg.From = new MailAddress(this.EmailAddress, "T ALERT", Encoding.ASCII);
                    mailMsg.IsBodyHtml = true;
                    mailMsg.Subject = subject.Trim();
                    mailMsg.Body = body;

                    SmtpClient client = new SmtpClient(this.SMTPServer, (int)this.SMTPPort);
                    client.Credentials = new System.Net.NetworkCredential(this.EmailAddress, this.EmailPassword);
                    client.EnableSsl = this.IsSSLRequired;
                    client.Send(mailMsg);
                }

                search.MoreInfo = search.MoreInfo.Replace("Sending email.", "Email sent.");
            }
            catch (Exception ex)
            {
                search.MoreInfo = search.MoreInfo.Replace("Sending email.", "Email did not send." + ex.Message);
            }
        }
    }
}
