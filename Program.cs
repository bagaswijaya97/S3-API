using Microsoft.OpenApi.Models; // Tambahkan ini

var builder = WebApplication.CreateBuilder(args);

// Tambahkan konfigurasi lain jika diperlukan
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "S3StorageAPI", Version = "v1" });
});

var app = builder.Build();

// Konfigurasi middleware lain jika diperlukan
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "S3StorageAPI v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();