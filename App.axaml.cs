using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ToDoList.ViewModels;
using ToDoList.Views;

namespace ToDoList
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindowViewModel = new MainWindowViewModel();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = mainWindowViewModel
                };

                desktop.Exit += (sender, e) =>
                {
                    mainWindowViewModel.SaveToDoList();
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
