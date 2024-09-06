using Microsoft.EntityFrameworkCore;
using RegistroTecnico1.Components;
using RegistroTecnico1.DAL;
using RegistroTecnico1.Service;

namespace RegistroTecnico1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Inyeccion de la base de datos(SqLite)
            var ConStr = builder.Configuration.GetConnectionString("ConStr");
            builder.Services.AddDbContext<Context>(c => c.UseSqlite(ConStr));

            //Inyeccion del Servicio(service)
            builder.Services.AddScoped<TecnicoService>();

            //Inyeccion del TipoServicio(service)
            builder.Services.AddScoped<TiposTecnicosService>();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
