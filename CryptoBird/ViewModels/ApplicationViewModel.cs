using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EmailAgent;

namespace CryptoBird.ViewModels
{
    class ApplicationViewModel : BasicViewModel, INotifyPropertyChanged
    {
        private MailMessage selectedMessage;

        public ObservableCollection<MailMessage> Messages { get; set; }
        public MailMessage SelectedMessage
        {
            get { return selectedMessage; }
            set
            {
                SetProperty(ref selectedMessage, value, "SelectedMessage");
                BrowserHtml = selectedMessage.Body; // MAYBE NOT HERE
            }
        }

        private string browserHtml;
        public string BrowserHtml
        {
            get { return browserHtml; }
            set
            {
                SetProperty(ref browserHtml, value, "BrowserHtml");
            }
        }

        public ApplicationViewModel()
        {
            var kek = new MailGetter();
            Messages = new ObservableCollection<MailMessage>(kek.Test());
        }
    }
}
