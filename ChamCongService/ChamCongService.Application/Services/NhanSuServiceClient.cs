using System.Net.Http.Headers;
using System.Text.Json;
using ChamCongService.Application.Services.DTO;

namespace ChamCongService.Application.Services
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

        public async Task<NhanVienServiceClientDto?> GetNhanVienByIdAsync(Guid nhanVienId, string token)
        {
            try
            {
                var requestUrl = $"/api/NhanVien/{nhanVienId}";
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
                    return JsonSerializer.Deserialize<NhanVienServiceClientDto>(content, jsonOptions);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                var errorMsg = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to get employee from NhanSu API. URL: {Url}, Status Code: {StatusCode}, Error: {Error}",
                    _httpClient.BaseAddress + requestUrl, response.StatusCode, errorMsg);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while getting employee from NhanSu API");
                return null;
            }
        }
    }
}
