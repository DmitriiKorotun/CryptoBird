﻿using EmailAgent.Entities;
using EmailAgent.Entities.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMail.Local
{
    interface IMessageIO
    {
        void SaveMessage(MailMessage message, string filename);
        void SaveMessages(List<MailMessage> messages, string filename);
        void SaveFolder(Folder folder, string filename);

        MailMessage LoadMessage(string filename);
        List<MailMessage> LoadMessages(string filename);
        Folder LoadFolder(string filename);
    }
}
