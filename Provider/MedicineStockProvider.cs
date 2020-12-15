using Newtonsoft.Json;
using PharmacyMedicineSupplyService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PharmacyMedicineSupplyService.Provider
{
    public class MedicineStockProvider:IMedicineStockProvider
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(PharmacySupplyProvider));
        public async Task<int> GetStock(string medicineName)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44394")
            };
            var response = await client.GetAsync("MedicineStockInformation");
            if (!response.IsSuccessStatusCode)
            {
                _log4net.Info("No data was found in stock");
                return -1;
            }
            _log4net.Info("Retrieved data from medicine stock service");
            string stringStock = await response.Content.ReadAsStringAsync();
            var medicines = JsonConvert.DeserializeObject<List<MedicineStock>>(stringStock);
            var i = medicines.Where(x => x.Name == medicineName).FirstOrDefault();
            return i.NumberOfTabletsInStock;
        }
    }
}
