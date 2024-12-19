using System.Data.Entity.Migrations.Design;
using System.Text.Json;
namespace JedenZilus
{
    public class PytaniaJson
    {
        private string sciezka = System.AppDomain.CurrentDomain.BaseDirectory+"/pytania/pytania.json";
        public async Task<List<pytanie>> GetPytaniesAsync()
        {
            if (!File.Exists(sciezka))
            {
                return new List<pytanie>();
            }
            var json = await File.ReadAllTextAsync(sciezka);
            return JsonSerializer.Deserialize<List<pytanie>>(json) ?? new List<pytanie>();
        }
        public async Task SavePytaniaAsync(List<pytanie> pytania)
        {
            var json = JsonSerializer.Serialize(pytania, new JsonSerializerOptions { WriteIndented=true});
            await File.WriteAllTextAsync(sciezka, json);
        }
    }
}
