using CryptoMail.Entities;
using CryptoMail.Infrastructure;
using EmailAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CryptoBird.ViewModels
{
    class MailSendViewModel : BasicViewModel
    {
        private string _from;
        public string From
        {
            get => _from;
            set => SetProperty(ref _from, value, "From");
        }

        private string _to;
        public string To
        {
            get => _to;
            set => SetProperty(ref _to, value, "To");
        }

        private string _subject;
        public string Subject
        {
            get => _subject;
            set => SetProperty(ref _subject, value, "Subject");
        }

        private string _body;
        public string Body
        {
            get => _body;
            set => SetProperty(ref _body, value, "Body");
        }

        private MailTechnicalPassport _technicalPassport;
        public MailTechnicalPassport TechnicalPassport
        {
            get => _technicalPassport;
            set => SetProperty(ref _technicalPassport, value, "TechnicalPassport");
        }

        public ICommand SendMessageCommand { get; }

        public MailSendViewModel()
        {
            SendMessageCommand = new RelayCommand(SendMessage);

            From = UserData.Login;

            TechnicalPassport = new MailTechnicalPassport();
            TechnicalPassport.UserCreditentials.Login = UserData.Login;
            TechnicalPassport.UserCreditentials.Password = UserData.Password;
            TechnicalPassport.Provider.Host = "smtp.gmail.com";
            TechnicalPassport.Provider.Port = 587;
        }

        private void SendMessage()
        {
            new CMController().SendMessage(From, To, Body, Subject, TechnicalPassport);
        }
    }
}
