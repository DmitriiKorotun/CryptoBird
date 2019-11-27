using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailAgent
{
    public class EmailMessageSimplified
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public bool IsHtml { get; set; }
        //public string 
        public EmailMessageSimplified()
        {

        }

        public EmailMessageSimplified(string body)
        {
            Body = body;
        }

        public EmailMessageSimplified(string body, string subject)
        {
            Subject = subject;
            Body = body;
        }

        public EmailMessageSimplified(string body, string subject, bool isHtml)
        {
            Subject = subject;
            Body = body;
            IsHtml = isHtml;
        }

        public EmailMessageSimplified(string body, string subject, string from, string to)
        {
            From = from;
            To = to;
            Subject = subject;
            Body = body;
        }

        public EmailMessageSimplified(string body, string subject, string from, string to, bool isHtml)
        {
            From = from;
            To = to;
            Subject = subject;
            Body = body;
            IsHtml = isHtml;
        }
    }
}
