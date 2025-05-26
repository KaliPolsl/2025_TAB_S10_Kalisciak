using AquaparkApp.Shared;
using System.Net.Http.Json;

namespace AquaparkApp.Client.Services
{
    public class AtrakcjaService
    {
        private readonly HttpClient _http;

        public AtrakcjaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Atrakcja>> GetAtrakcjeAsync()
        {
            return await _http.GetFromJsonAsync<List<Atrakcja>>("api/Atrakcja");
        }
    }
}
