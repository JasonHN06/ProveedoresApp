using ProveedoresApp.ViewModels;
using ProveedoresApp.Views;

namespace ProveedoresApp;

public partial class App : Application
{
    public App(ProveedoresViewModel viewModel)
    {
        InitializeComponent();
        MainPage = new ProveedoresPage(viewModel);
    }
}
