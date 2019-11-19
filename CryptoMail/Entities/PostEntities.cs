using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Entities
{
    public class MailTechnicalPassport
    {
        public class Creditentials
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }

        public class MailProvider
        {
            public int Port { get; set; }
            public string Host { get; set; }           
        }

        public Creditentials UserCreditentials { get; set; }
        public MailProvider Provider { get; set; }

        public MailTechnicalPassport()
        {
            UserCreditentials = new Creditentials();
            Provider = new MailProvider();
        }
    }

    public class MessagePathInfo
    {
        public string From { get; set; }
        public string To { get; set; }
    }
}
