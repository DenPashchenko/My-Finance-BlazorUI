using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Common.Mappings;
using MyFinance.Application.Interfaces;
using MyFinance.Persistence;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var defaultCulture = builder.Configuration.GetSection("Localization").GetValue<string>("DefaultCulture");
var localizationOptions = new RequestLocalizationOptions()
{
	SupportedCultures = new List<CultureInfo> { new CultureInfo(defaultCulture) },
	SupportedUICultures = new List<CultureInfo> { new CultureInfo(defaultCulture) },
	DefaultRequestCulture = new RequestCulture(defaultCulture)
};

builder.Services.AddDbContext<DataDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
		 x => x.MigrationsAssembly(typeof(DataDbContext).Assembly.FullName));
});

builder.Services.AddScoped<IDataDbContext, DataDbContext>();

builder.Services.AddAutoMapper(config =>
{
	config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
	config.AddProfile(new AssemblyMappingProfile(typeof(IDataDbContext).Assembly));
});

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(IDataDbContext).Assembly));

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

app.UseRequestLocalization(localizationOptions);
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

public partial class Program { }