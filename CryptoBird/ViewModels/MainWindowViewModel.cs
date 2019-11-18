using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CryptoMail;
using EmailAgent;
using MimeKit;

namespace CryptoBird.ViewModels
{
    class MainWindowViewModel : BasicViewModel, INotifyPropertyChanged
    {
        private MimeMessage selectedMessage;

        public ObservableCollection<MimeMessage> Messages { get; set; }
        public MimeMessage SelectedMessage
        {
            get { return selectedMessage; }
            set
            {
                SetProperty(ref selectedMessage, value, "SelectedMessage");

                BrowserHtml = selectedMessage.HtmlBody; // MAYBE NOT HERE
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



        public ICommand DownloadEnquiredCommand { get; }

        public MainWindowViewModel()
        {
            DownloadEnquiredCommand = new RelayCommand(DownloadAttachments);

            Messages = new ObservableCollection<MimeMessage>(new Controller().GetMimeMessages());
        }

        private void DownloadAttachments()
        {
            DownloadManager.DownloadAttachments(SelectedMessage);
        }

        // Закрытые поля команд
        private ICommand _openMailSendWindow;

        private ICommand _openSettingsWindow;

        // Свойства доступные только для чтения для обращения к командам и их инициализации
        public ICommand OpenMailSendWindow
        {
            get
            {
                if (_openMailSendWindow == null)
                {
                    _openMailSendWindow = new OpenMailSendWindowCommand(this);
                }
                return _openMailSendWindow;
            }
        }
        public ICommand OpenSettingsWindow
        {
            get
            {
                if (_openSettingsWindow == null)
                {
                    _openSettingsWindow = new OpenSettingsWindowCommand(this);
                }
                return _openSettingsWindow;
            }
        }

        abstract class WindowCommand : ICommand
        {
            protected MainWindowViewModel _mainWindowVeiwModel;

            public WindowCommand(MainWindowViewModel mainWindowVeiwModel)
            {
                _mainWindowVeiwModel = mainWindowVeiwModel;
            }

            public event EventHandler CanExecuteChanged;

            public abstract bool CanExecute(object parameter);

            public abstract void Execute(object parameter);
        }

        class OpenMailSendWindowCommand : WindowCommand
        {
            public OpenMailSendWindowCommand(MainWindowViewModel mainWindowVeiwModel) : base(mainWindowVeiwModel)
            {
            }
            public override bool CanExecute(object parameter)
            {
                return true;
            }
            public override async void Execute(object parameter)
            {
                var displayRootRegistry = (Application.Current as App).displayRootRegistry;

                var mailSendViewModel = new MailSendViewModel();
                await displayRootRegistry.ShowModalPresentation(mailSendViewModel);
            }
        }

        class OpenSettingsWindowCommand : WindowCommand
        {
            public OpenSettingsWindowCommand(MainWindowViewModel mainWindowVeiwModel) : base(mainWindowVeiwModel)
            {
            }
            public override bool CanExecute(object parameter)
            {
                return true;
            }
            public override async void Execute(object parameter)
            {
                var displayRootRegistry = (Application.Current as App).displayRootRegistry;

                var settingsWindowViewModel = new SettingsViewModel();
                await displayRootRegistry.ShowModalPresentation(settingsWindowViewModel);
            }
        }
    }
}
