using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CryptoBird.ViewModels
{
    class AuthorizationViewModel : BasicViewModel
    {
        private int selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return selectedTabIndex; }
            set
            {
                selectedTabIndex = value;
                OnPropertyChanged("SelectedTabIndex");
                //SetProperty(ref selectedTabIndex, value, "TabIndex");
            }
        }

        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                SetProperty(ref username, value, "Username");
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                SetProperty(ref password, value, "Password");
            }
        }

        private string inputHost;
        public string InputHost
        {
            get { return inputHost; }
            set
            {
                SetProperty(ref inputHost, value, "InputHost");
            }
        }

        private int? inputPort;
        public int? InputPort
        {
            get { return inputPort; }
            set
            {
                SetProperty(ref inputPort, value, "InputPort");
            }
        }

        public ICommand GoToNextTabCommand { get; }
        public ICommand GoToPreviousTabCommand { get; }
        public ICommand AuthorizeUserCommand { get; }
        public RelayCommand<ICloseable> CloseWindowCommand { get; private set; }

        public AuthorizationViewModel()
        {
            //_children = new ObservableCollection<object>();
            //_children.Add(new Tab1ViewModel());
            //_children.Add(new Tab2ViewModel());

            GoToNextTabCommand = new RelayCommand(GoToNextTab);
            GoToPreviousTabCommand = new RelayCommand(GoToPreviousTab);
            AuthorizeUserCommand = new RelayCommand<ICloseable>(AuthorizeUser);

            this.CloseWindowCommand = new RelayCommand<ICloseable>(this.CloseWindow);
        }

        private void GoToNextTab()
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                selectedTabIndex += 1;

            }
        }

        private void GoToPreviousTab()
        {
            SelectedTabIndex -= 1;
        }

        private void AuthorizeUser(ICloseable window)
        {
            if (TryApplySettings())
            {
                CloseWindow(window);
            }
        }

        private bool TryApplySettings()
        {
            bool isSettingApplied = false;

            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password)
                && !string.IsNullOrEmpty(InputHost) && InputPort != null)
            {
                Properties.MailServerSettings.Default.USERNAME = Username;
                Properties.MailServerSettings.Default.USER_PASSWORD = Password;

                Properties.MailServerSettings.Default.INPUT_HOST = InputHost;
                Properties.MailServerSettings.Default.INPUT_PORT = (int)InputPort;

                Properties.PublicOnForm.Default.USERNAME_INPUT = Username;

                Properties.MailServerSettings.Default.Save();
                Properties.PublicOnForm.Default.Save();

                isSettingApplied = true;
            }

            return isSettingApplied;
        }

        private void CloseWindow(ICloseable window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
