using JedenZilus;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<PytaniaJson>();
builder.Services.AddRazorPages();
// Add services to the container.

var app = builder.Build();
// Configure the HTTP request pipeline.
/*if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}*/
app.MapGet("/pulapytan", async (PytaniaJson db) =>
{
    var pytania = await db.GetPytaniesAsync();
    return Results.Ok(pytania);
});

app.MapGet("/pulapytan/latwe", async (PytaniaJson db) =>
{
    var pytania = await db.GetPytaniesAsync();
    var latwePytania = pytania.Where(t=>t.czylatwe).ToList();
    return Results.Ok(latwePytania);
});
app.MapGet("/pulapytan/trudne", async (PytaniaJson db) =>
{
    var pytania = await db.GetPytaniesAsync();
    var trudnePytania = pytania.Where(t => !t.czylatwe).ToList();
    return Results.Ok(trudnePytania);
});
app.MapGet("/pulapytan/{id}", async (int id, PytaniaJson db) =>
{
    var pytania = await db.GetPytaniesAsync();
    var pytanie = pytania.FirstOrDefault(p => p.id == id);
    return pytanie is not null ? Results.Ok(pytanie):Results.NotFound();
}
);
app.MapPost("/pulapytan", async (pytanie pytanie, PytaniaJson db) =>
{
    var pytania = await db.GetPytaniesAsync();
    pytanie.id = pytania.Any() ? pytania.Max(p => p.id) + 1 : 1;
    pytania.Add(pytanie);
    await db.SavePytaniaAsync(pytania);
    return Results.Created($"/pulapytan/{pytanie.id}", pytanie);
});
app.MapPut("/pulapytan/{id}", async (int id, pytanie wpisz, PytaniaJson db) =>
{
    var pytania = await db.GetPytaniesAsync();
    var pytanie = pytania.FirstOrDefault(p => p.id == id);
    if(pytanie is null) return Results.NotFound();
    pytanie.tresc = wpisz.tresc;
    pytanie.odpowiedz= wpisz.odpowiedz;
    pytanie.czylatwe = wpisz.czylatwe;
    pytanie.kategoria = wpisz.kategoria;
    await db.SavePytaniaAsync(pytania);
    return Results.NoContent();
});
app.MapDelete("/pulapytan/{id}", async (int id,PytaniaJson db) =>
{
    var pytania = await db.GetPytaniesAsync();
    var pytanie = pytania.FirstOrDefault(p => p.id == id);
    if (pytanie is null) return Results.NotFound();
    pytania.Remove(pytanie);
    await db.SavePytaniaAsync(pytania);
    return Results.NoContent();
});
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});
app.Run();
