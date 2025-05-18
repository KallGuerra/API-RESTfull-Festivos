using Festivo.Aplicacion.Servicios;
using Festivo.Core.Servicios;
using Festivo.Core.Repositorios;
using Festivo.Infraestructura.Persistencia.Contexto;
using Festivo.Infraestructura.Repositorio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------
// Configurar DbContext
// ------------------------------------
builder.Services.AddDbContext<FestivoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FestivosConnection"))
);

// ------------------------------------
// Registrar Repositorios
// ------------------------------------
builder.Services.AddScoped<IFestivoRepositorio, FestivoRepositorio>();
builder.Services.AddScoped<ITipoRepositorio, TipoRepositorio>();

// ------------------------------------
// Registrar Servicios
// ------------------------------------
builder.Services.AddScoped<IFestivoServicio, FestivoServicio>();
builder.Services.AddScoped<ITipoServicio, TipoServicio>();

// ------------------------------------
// Otros servicios (controladores, swagger, etc.)
// ------------------------------------
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();




var app = builder.Build();

app.UseCors(policy =>
    policy.WithOrigins("http://localhost:4200")
          .AllowAnyMethod()
          .AllowAnyHeader()
);

using (var scope = app.Services.CreateScope())
{

    var Context = scope.ServiceProvider.GetRequiredService<FestivoContext>();
    Context.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.MapRazorPages();
app.Run();
