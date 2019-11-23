﻿using System;
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
using CryptoMail.Network;
using CryptoMail.Network.Infrastructure;
using CryptoMail.Local;
using CryptoMail.Local.Serialization;
using EmailAgent;
using EmailAgent.Entities.Caching;
using EmailAgent.Entities;
using CryptoMail.Network.Entities;

namespace CryptoBird.ViewModels
{
    class MainWindowViewModel : BasicViewModel, INotifyPropertyChanged
    {
        private MailMessage selectedMessage;
        public MailMessage SelectedMessage
        {
            get { return selectedMessage; }
            set
            {
                SetProperty(ref selectedMessage, value, "SelectedMessage");

                if (!(selectedMessage is null))
                {
                    if (!string.IsNullOrEmpty(selectedMessage.Body))
                        BrowserHtml = selectedMessage.Body; // MAYBE NOT HERE

                    //CMLocalController.SaveMessages(Messages.ToList(), SelectedFolder.FolderType, Properties.MailServerSettings.Default.USERNAME);

                    SelectedMessageDate = SelectedMessage.Headers["Date"];
                }
            }
        }

        private string selectedMessageDate;
        public string SelectedMessageDate
        {
            get { return selectedMessageDate; }
            set
            {
                SetProperty(ref selectedMessageDate, value, "SelectedMessageDate");
            }
        }


        private ObservableCollection<MailMessage> messages;
        public ObservableCollection<MailMessage> Messages {
            get { return messages; }
            set
            {
                messages = value;
                OnPropertyChanged("Messages");
            }
        }


        private MailFolder selectedFolder;
        public MailFolder SelectedFolder
        {
            get { return selectedFolder; }
            set
            {
                SetProperty(ref selectedFolder, value, "SelectedFolder");

                var contoller = new CMController();

                var folderInbox = FolderManager.CreateFolder(MailSpecialFolder.Inbox);

                FolderManager.UpdateFolder(
                    Properties.MailServerSettings.Default.USERNAME, UserData.Password,
                    Properties.MailServerSettings.Default.INPUT_HOST, Properties.MailServerSettings.Default.INPUT_PORT,
                    folderInbox
                    );

                FolderManager.SaveFolder(folderInbox, Properties.MailServerSettings.Default.USERNAME);

                var loadedFolder = FolderManager.LoadFolder(MailSpecialFolder.Inbox, Properties.MailServerSettings.Default.USERNAME);

                //Messages = new ObservableCollection<MailMessage>(
                //    contoller.GetAllMessages(
                //        UserData.Login, UserData.Password, Properties.MailServerSettings.Default.INPUT_HOST, Properties.MailServerSettings.Default.INPUT_PORT,
                //        selectedFolder.FolderType
                //        ));
            }
        }

        public ObservableCollection<MailFolder> Folders { get; set; }


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

            Folders = new ObservableCollection<MailFolder>(CMController.GetMailFolders());

            Messages = new ObservableCollection<MailMessage>(CMLocalController.LoadMessages(MailSpecialFolder.Inbox, Properties.MailServerSettings.Default.USERNAME));
        }

        private void DownloadAttachments()
        {
            new Controller().DownloadAttachments(Messages.IndexOf(SelectedMessage));
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
