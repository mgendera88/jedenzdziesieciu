using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace JedenZilus.Pages
{
    public class grańskoModel : PageModel
    {
        private readonly PytaniaJson _db;
        public grańskoModel (PytaniaJson db)
        {
            _db = db;
        }
        public List<pytanie> Pytania { get; set; } = new List<pytanie>();
        public async Task<IActionResult>OnPostLatweAsync()
        {
            var pytania = await _db.GetPytaniesAsync();
            Pytania = pytania.Where(p => p.czylatwe).ToList();
            var random = new Random();
            Pytania = Pytania.OrderBy(x => random.Next(1, Pytania.Count)).Take(1).ToList();
            var pytanie = Pytania.FirstOrDefault();
            pytania.Remove(pytanie);
            await _db.SavePytaniaAsync(pytania);
            return Page();
        }
        public async Task<IActionResult>OnPostTrudneAsync()
        {
            var pytania = await _db.GetPytaniesAsync();
            Pytania = pytania.Where(p => !p.czylatwe).ToList();
            var random = new Random();
            Pytania = Pytania.OrderBy(x => random.Next(1, Pytania.Count)).Take(1).ToList();
            var pytanie = Pytania.FirstOrDefault();
            pytania.Remove(pytanie);
            await _db.SavePytaniaAsync(pytania);
            return Page();
        }
        public async Task<IActionResult>OnPostLosoweAsync()
        {
            var pytania = await _db.GetPytaniesAsync();
            Pytania = pytania;
            var random = new Random();
            Pytania = Pytania.OrderBy(x=>random.Next(1,Pytania.Count)).Take(1).ToList();
            var pytanie = Pytania.FirstOrDefault();
            pytania.Remove(pytanie);
            await _db.SavePytaniaAsync(pytania);
            return Page();
        }
    }
}
