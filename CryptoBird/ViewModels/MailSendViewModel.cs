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

        public ICommand SendMessageCommand { get; }

        public MailSendViewModel()
        {
            SendMessageCommand = new RelayCommand(SendMessage);

            From = UserData.Login;
        }

        private void SendMessage()
        {
            var messageSender = new MailSender("smtp.gmail.com", 587);

            new Controller().SendEncryptedMessage(From, To, Body, Subject, UserData.Login, UserData.Password, "smtp.gmail.com", 587);

            //new Controller().SendPublicKeyRequest(From, To, UserData.Login, UserData.Password, "smtp.gmail.com", 587);

            //messageSender.Send(From, To, Body, Subject, UserData.Login, UserData.Password, "smtp.gmail.com", 587);
        }
    }
}
