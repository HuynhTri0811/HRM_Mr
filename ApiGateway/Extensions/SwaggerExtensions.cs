using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;

namespace ApiGateway.Extensions
{
    public static class SwaggerExtensions
    {
        public static void ConfigureSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        public static void UseGatewaySwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/nhansu/v1/swagger.json", "Nhân Sự Service");
                options.SwaggerEndpoint("/swagger/chamcong/v1/swagger.json", "Chấm Công Service");
                options.SwaggerEndpoint("/swagger/quyetdinh/v1/swagger.json", "Quyết Định Service");
                options.SwaggerEndpoint("/swagger/tinhluong/v1/swagger.json", "Tính Lương Service");
                options.RoutePrefix = "swagger"; // Truy cập Swagger tại http://localhost:8080/swagger
            });
        }

        public static void MapSwaggerRoutes(this WebApplication app)
        {
            // Endpoint tự động gộp và biên dịch Swagger của từng microservice
            app.MapGet("/swagger/{service}/v1/swagger.json", async (string service, HttpClient httpClient, IConfiguration configuration) =>
            {
                string? targetUrl = service switch
                {
                    "nhansu" => configuration["ReverseProxy:Clusters:nhansu-cluster:Destinations:destination1:Address"],
                    "chamcong" => configuration["ReverseProxy:Clusters:chamcong-cluster:Destinations:destination1:Address"],
                    "quyetdinh" => configuration["ReverseProxy:Clusters:quyetdinh-cluster:Destinations:destination1:Address"],
                    "tinhluong" => configuration["ReverseProxy:Clusters:tinhluong-cluster:Destinations:destination1:Address"],
                    _ => null
                };

                if (string.IsNullOrEmpty(targetUrl))
                {
                    return Results.NotFound($"Không tìm thấy dịch vụ {service} trong cấu hình.");
                }

                try
                {
                    string downstreamSwaggerUrl = $"{targetUrl.TrimEnd('/')}/swagger/v1/swagger.json";
                    var jsonString = await httpClient.GetStringAsync(downstreamSwaggerUrl);

                    var node = System.Text.Json.Nodes.JsonNode.Parse(jsonString);
                    if (node is System.Text.Json.Nodes.JsonObject rootObj &&
                        rootObj.TryGetPropertyValue("paths", out var pathsNode) &&
                        pathsNode is System.Text.Json.Nodes.JsonObject pathsObj)
                    {
                        var newPaths = new System.Text.Json.Nodes.JsonObject();
                        foreach (var property in pathsObj.ToList())
                        {
                            string newKey = property.Key;
                            if (property.Key.StartsWith("/api/"))
                            {
                                newKey = "/api/" + service + property.Key.Substring(4);
                            }

                            pathsObj.Remove(property.Key);
                            newPaths.Add(newKey, property.Value);
                        }
                        rootObj["paths"] = newPaths;

                        // Xóa trường servers (nếu có) để Swagger UI gửi request tương đối từ host Gateway
                        rootObj.Remove("servers");
                    }

                    return Results.Content(node?.ToJsonString() ?? jsonString, "application/json", System.Text.Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Không thể kết nối lấy tài liệu Swagger từ {service}: {ex.Message}");
                }
            });
        }
    }
}
