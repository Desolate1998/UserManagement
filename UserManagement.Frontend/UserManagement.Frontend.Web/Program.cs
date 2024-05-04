using System.Text.Json.Serialization;
using UserManagement.Frontend.Web.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpHelper>();
builder.Services.AddMvc().AddSessionStateTempDataProvider();
builder.Services.AddSession();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Login}");
app.UseSession();

app.Run();
