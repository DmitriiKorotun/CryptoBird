using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CryptoBird.ViewModels
{
    class SettingsViewModel : BasicViewModel
    {
        private string inputServerName;
        public string InputServerName
        {
            get { return inputServerName; }
            set
            {
                SetProperty(ref inputServerName, value, "InputServerName");
            }
        }

        private int inputServerPort;
        public int InputServerPort
        {
            get { return inputServerPort; }
            set
            {
                SetProperty(ref inputServerPort, value, "InputServerPort");
            }
        }

        private string inputMailType;
        public string InputMailType
        {
            get { return inputMailType; }
            set
            {
                SetProperty(ref inputMailType, value, "InputMailType");
            }
        }

        private string outputServerName;
        public string OutputServerName
        {
            get { return outputServerName; }
            set
            {
                SetProperty(ref outputServerName, value, "OutputServerName");
            }
        }

        private int outputServerPort;
        public int OutputServerPort
        {
            get { return outputServerPort; }
            set
            {
                SetProperty(ref outputServerPort, value, "OutputServerPort");
            }
        }

        public ICommand SaveSettingsCommand { get; }
        public RelayCommand<ICloseable> CloseWindowCommand { get; private set; }

        public SettingsViewModel()
        {
            InputServerName = Properties.MailServerSettings.Default.INPUT_SERVER_NAME;
            InputServerPort = Properties.MailServerSettings.Default.INPUT_PORT;
            InputMailType = Properties.MailServerSettings.Default.INPUT_METHOD;
            OutputServerName = Properties.MailServerSettings.Default.OUTPUT_SERVER_NAME;
            OutputServerPort = Properties.MailServerSettings.Default.OUTPUT_PORT;

            SaveSettingsCommand = new RelayCommand(SaveSettings);
            this.CloseWindowCommand = new RelayCommand<ICloseable>(this.CloseWindow);
        }

        private void SaveSettings()
        {
            Properties.MailServerSettings.Default.INPUT_SERVER_NAME = InputServerName;
            Properties.MailServerSettings.Default.INPUT_PORT = InputServerPort;
            Properties.MailServerSettings.Default.INPUT_METHOD = InputMailType;
            Properties.MailServerSettings.Default.OUTPUT_SERVER_NAME = OutputServerName;
            Properties.MailServerSettings.Default.OUTPUT_PORT = OutputServerPort;

            Properties.MailServerSettings.Default.Save();
        }

        private void CloseWindow(ICloseable window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        public class RelayCommand<T> : ICommand
        {
            private Action<T> action;
            public RelayCommand(Action<T> action) => this.action = action;
            public bool CanExecute(object parameter) => true;
#pragma warning disable CS0067
            public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067
            public void Execute(object parameter) => action((T)parameter);
        }

        public class RelayCommand : ICommand
        {
            private Action action;
            public RelayCommand(Action action) => this.action = action;
            public bool CanExecute(object parameter) => true;
#pragma warning disable CS0067
            public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067
            public void Execute(object parameter) => action();
        }
    }
}