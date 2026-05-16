using System.Net.Http.Headers;
using System.Text.Json;
using TinhLuongService.Application.Services.DTO;
using TinhLuongService.Application.Services.Interface;
using Microsoft.Extensions.Logging;

namespace TinhLuongService.Application.Services
{
    public class NhanSuServiceClient : INhanSuServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NhanSuServiceClient> _logger;

        public NhanSuServiceClient(HttpClient httpClient, ILogger<NhanSuServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<NhanVienServiceClientDto>> GetAllNhanVienAsync(string token)
        {
            try
            {
                var requestUrl = "/api/NhanVien";
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

                if (!string.IsNullOrEmpty(token))
                {
                    var actualToken = token.StartsWith("Bearer ") ? token.Substring(7) : token;
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", actualToken);
                }

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonOptions = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    return JsonSerializer.Deserialize<IEnumerable<NhanVienServiceClientDto>>(content, jsonOptions) ?? Enumerable.Empty<NhanVienServiceClientDto>();
                }

                var errorMsg = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to get all employees from NhanSu API. URL: {Url}, Status Code: {StatusCode}, Error: {Error}",
                    _httpClient.BaseAddress + requestUrl, response.StatusCode, errorMsg);
                return Enumerable.Empty<NhanVienServiceClientDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while getting all employees from NhanSu API");
                return Enumerable.Empty<NhanVienServiceClientDto>();
            }
        }
    }
}
