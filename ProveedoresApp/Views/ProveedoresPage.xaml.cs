using ProveedoresApp.ViewModels;

namespace ProveedoresApp.Views;

public partial class ProveedoresPage : ContentPage
{
    public ProveedoresPage(ProveedoresViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}