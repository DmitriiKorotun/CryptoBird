using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Network.Infrastructure
{
    class MailParser
    {
        public enum Token
        {
            NONE,
            SEND_PUBLIC_KEYS,
            RECEIVE_PUBLIC_KEYS           
        }

        public Token SearchForToken(MailMessage message)
        {
            Token token;

            switch (message.Subject)
            {
                case "9B9817CC57EBA9DF0067A197FB7FBE9F":
                    token = Token.RECEIVE_PUBLIC_KEYS;
                    break;
                case "0FB6AE9A0CE9D11793D83DCA4867C578":
                    token = Token.SEND_PUBLIC_KEYS;
                    break;
                default:
                    token = Token.NONE;
                    break;
            }

            return token;
        }
    }
}
