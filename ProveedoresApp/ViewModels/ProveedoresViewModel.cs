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

        private bool ValidateProveedor()
        {
            bool isValid = true;

            NombreError = string.Empty;
            EmailError = string.Empty;

            if (string.IsNullOrWhiteSpace(NuevoProveedor.Nombre))
            {
                NombreError = "El nombre es requerido";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(NuevoProveedor.Email) || !NuevoProveedor.Email.Contains("@"))
            {
                EmailError = "Email inválido";
                isValid = false;
            }

            return isValid;
        }

        [RelayCommand]
        private async Task AgregarProveedor()
        {
            // Validación
            if (!ValidateProveedor()) return;

            await _databaseService.SaveProveedorAsync(NuevoProveedor);
            NuevoProveedor = new Proveedor();
            await LoadProveedoresAsync();
        }

        [RelayCommand]
        private async Task EditarProveedor()
        {
            if (NuevoProveedor == null || NuevoProveedor.Id == 0) return;

            if (!ValidateProveedor()) return;

            await _databaseService.SaveProveedorAsync(NuevoProveedor);
            NuevoProveedor = new Proveedor();
            SelectedProveedor = null; 
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
            if (value != null)
            {
                NuevoProveedor = new Proveedor
                {
                    Id = value.Id,
                    Nombre = value.Nombre,
                    Direccion = value.Direccion,
                    Telefono = value.Telefono,
                    Email = value.Email
                };
            }
            else
            {
                NuevoProveedor = new Proveedor(); 
            }
        }
    }
}
