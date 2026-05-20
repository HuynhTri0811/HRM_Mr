using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TinhLuongService.Extensions;
using TinhLuongService.API.Filters;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.ConfigureSerilog();

// Add framework services
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ResponseLoggingFilter>();
    options.Filters.Add<ConcurrencyExceptionFilter>();
});
builder.Services.AddHttpContextAccessor();

// Configure microservice configurations
builder.ConfigureSwagger();
builder.ConfigureDatabase();
builder.ConfigureAuthentication();
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
    Log.Information("Ứng dụng TinhLuongService đang khởi động...");

    // Kiểm tra kết nối MongoDB
    await app.CheckMongoAsync(builder.Configuration);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Ứng dụng TinhLuongService gặp lỗi nghiêm trọng khi khởi động.");
}
finally
{
    Log.CloseAndFlush();
}
