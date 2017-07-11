using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reto4.Services
{
    public class ServiceHelper
    {
        MobileServiceClient clienteServicio = new MobileServiceClient(@"http://xamarinchampions.azurewebsites.net");

        private IMobileServiceTable<TorneoItem> _TorneoItemTable;

        public async Task<List<TorneoItem>> BuscarRegistros(string Correo)
        {
            _TorneoItemTable = clienteServicio.GetTable<TorneoItem>();
            System.Collections.Generic.List<TorneoItem> items = await _TorneoItemTable.Where(
                TorneoItem => TorneoItem.Email == Correo).ToListAsync();
            return items;
        }

        public async Task InsertarEntidad(string direccionCorreo, string reto, string AndroidId)
        {
            _TorneoItemTable = clienteServicio.GetTable<TorneoItem>();


            await _TorneoItemTable.InsertAsync(new TorneoItem
            {
                Email = direccionCorreo,
                Reto = reto,
                DeviceId = AndroidId
            });
        }
    }
}
