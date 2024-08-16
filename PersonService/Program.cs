using Microsoft.EntityFrameworkCore;
using PersonService.Data;
using PersonService.Data.Interceptors;
using PersonService.Mappers;
using PersonService.Middlewares;
using PersonService.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<PersonInterceptor>();
        builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("InMemoryDbName")!);
            var interceptor = serviceProvider.GetRequiredService<PersonInterceptor>();
            options.AddInterceptors(interceptor);
        });
        builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
        builder.Services.AddScoped<IPersonService, PersonService.Services.Impl.PersonService>();


        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseRouting();
        app.UseMiddleware<ResponseLoggingMiddleware>();

        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var scopedContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            DataSeed.SeedPersons(scopedContext);
        }

        await app.RunAsync();
    }
}