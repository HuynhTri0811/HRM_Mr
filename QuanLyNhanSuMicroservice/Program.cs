using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuanLyNhanSuMicroservice.Extensions;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.API.Filters;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.ConfigureSerilog();

// Add framework services
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ResponseLoggingFilter>();
});
builder.Services.AddHttpContextAccessor();

// Configure custom microservice configurations
builder.ConfigureSwagger();
builder.ConfigureDatabase();
builder.ConfigureAuthentication();
builder.ConfigureMessaging();
builder.ConfigureObservability();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Auto migrate database on startup
app.MigrateDatabase();

app.UseAuthentication();
app.UseAuthorization();

// Setup Metrics & Prometheus
app.UseObservability();

app.MapControllers();

try
{
    Log.Information("Ứng dụng QuanLyNhanSuMicroservice đang khởi động...");

    // Kiểm tra kết nối MongoDB
    await app.CheckMongoAsync(builder.Configuration);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Ứng dụng QuanLyNhanSuMicroservice gặp lỗi nghiêm trọng khi khởi động.");
}
finally
{
    Log.CloseAndFlush();
}
