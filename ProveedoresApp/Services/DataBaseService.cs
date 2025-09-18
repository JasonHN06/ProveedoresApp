using SQLite;
using ProveedoresApp.Models;

namespace ProveedoresApp.Services
{
    public class DataBaseService
    {
        private SQLiteAsyncConnection _database;
        private readonly string _dbPath;

        public DataBaseService()
        {
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, "proveedores.db3");
            _database = new SQLiteAsyncConnection(_dbPath);
            InitializeDatabase();
        }

        private async void InitializeDatabase()
        {
            await _database.CreateTableAsync<Proveedor>();
        }

        public async Task<List<Proveedor>> GetProveedoresAsync()
        {
            return await _database.Table<Proveedor>().ToListAsync();
        }

        public async Task<Proveedor> GetProveedorAsync(int id)
        {
            return await _database.GetAsync<Proveedor>(id);
        }

        public async Task<int> SaveProveedorAsync(Proveedor proveedor)
        {
            if (proveedor.Id != 0)
            {
                return await _database.UpdateAsync(proveedor);
            }
            else
            {
                return await _database.InsertAsync(proveedor);
            }
        }

        public async Task<int> DeleteProveedorAsync(Proveedor proveedor)
        {
            return await _database.DeleteAsync(proveedor);
        }
    }
}
