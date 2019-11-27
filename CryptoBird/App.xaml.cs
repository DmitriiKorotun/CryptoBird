using CryptoBird.Layouts;
using CryptoBird.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CryptoBird
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DisplayRootRegistry displayRootRegistry = new DisplayRootRegistry();
        MainWindowViewModel mainWindowViewModel;

        public App()
        {
            displayRootRegistry.RegisterWindowType<MainWindowViewModel, MainWindow>();
            displayRootRegistry.RegisterWindowType<MailSendViewModel, MailSend>();
            displayRootRegistry.RegisterWindowType<SettingsViewModel, Settings>();
            displayRootRegistry.RegisterWindowType<AuthorizationViewModel, Authorization>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            //base.OnStartup(e);
            //await RunProgramLogic();
            //Shutdown();

            mainWindowViewModel = new MainWindowViewModel();          

            await displayRootRegistry.ShowModalPresentation(mainWindowViewModel);

            Shutdown();
        }

        //async Task RunProgramLogic()
        //{
        //    while (true)
        //    {
        //        mainWindowViewModel = new MainWindowViewModel();

        //        displayRootRegistry.ShowPresentation(mainWindowViewModel);

        //        while (true)
        //        {
        //            var askCloseVM = new AskVM("Do you want to close the application?");
        //            await Task.Delay(TimeSpan.FromSeconds(2));
        //            await displayRootRegistry.ShowModalPresentation(askCloseVM);
        //            if (askCloseVM.Answer == true)
        //                break;
        //        }
        //        displayRootRegistry.HidePresentation(mainVM);
        //        await Task.Delay(TimeSpan.FromSeconds(2));

        //        var askReopenVM = new AskVM("Maybe reopen again?");
        //        await displayRootRegistry.ShowModalPresentation(askReopenVM);
        //        if (askReopenVM.Answer != true)
        //            break;
        //    }
        //}
    }
}
