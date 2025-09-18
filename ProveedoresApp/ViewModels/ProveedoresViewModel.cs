using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProveedoresApp.Models;
using ProveedoresApp.Services;
using System.Collections.ObjectModel;

namespace ProveedoresApp.ViewModels
{
    public partial class ProveedoresViewModel : ObservableObject
    {
        private readonly DataBaseService _databaseService;

        [ObservableProperty]
        private ObservableCollection<Proveedor> proveedores;

        [ObservableProperty]
        private Proveedor selectedProveedor;

        [ObservableProperty]
        private Proveedor nuevoProveedor;

        [ObservableProperty]
        private string nombreError;

        [ObservableProperty]
        private string direccionError;

        [ObservableProperty]
        private string telefonoError;

        [ObservableProperty]
        private string emailError;

        public ProveedoresViewModel()
        {
            _databaseService = new DataBaseService();
            proveedores = new ObservableCollection<Proveedor>();
            nuevoProveedor = new Proveedor();
            LoadProveedoresAsync();
        }

        [RelayCommand]
        private async Task LoadProveedoresAsync()
        {
            var lista = await _databaseService.GetProveedoresAsync();
            proveedores.Clear();
            foreach (var p in lista)
            {
                proveedores.Add(p);
            }
        }

        [RelayCommand]
        private async Task AgregarProveedor()
        {
            // Validación simple
            if (string.IsNullOrWhiteSpace(nuevoProveedor.Nombre))
            {
                nombreError = "El nombre es requerido";
                return;
            }
            nombreError = string.Empty;

            if (string.IsNullOrWhiteSpace(nuevoProveedor.Email) || !nuevoProveedor.Email.Contains("@"))
            {
                emailError = "Email inválido";
                return;
            }
            emailError = string.Empty;

            await _databaseService.SaveProveedorAsync(nuevoProveedor);
            nuevoProveedor = new Proveedor(); // Reset
            await LoadProveedoresAsync();
        }

        [RelayCommand]
        private async Task EditarProveedor()
        {
            if (selectedProveedor == null) return;

            await _databaseService.SaveProveedorAsync(selectedProveedor);
            selectedProveedor = null;
            await LoadProveedoresAsync();
        }

        [RelayCommand]
        private async Task EliminarProveedor()
        {
            if (selectedProveedor == null) return;

            await _databaseService.DeleteProveedorAsync(selectedProveedor);
            selectedProveedor = null;
            await LoadProveedoresAsync();
        }

        partial void OnSelectedProveedorChanged(Proveedor value)
        {
            // Actualiza el proveedor seleccionado para editar
            if (value != null)
            {
                nuevoProveedor = new Proveedor
                {
                    Id = value.Id,
                    Nombre = value.Nombre,
                    Direccion = value.Direccion,
                    Telefono = value.Telefono,
                    Email = value.Email
                };
            }
        }
    }
}
