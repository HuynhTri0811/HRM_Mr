using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using QuyetDinhService.QuyetDinhService.Application.Services.DTO;

namespace QuyetDinhService.QuyetDinhService.Application.Services
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

        public async Task<bool> UpdateLuongAsync(Guid nhanVienId, decimal luongMoi, string token)
        {
            try
            {
                var requestUrl = $"/api/NhanVien/{nhanVienId}/update-luong";

                var payload = new UpdateLuongRequestDTO
                {
                    LuongCoBan = luongMoi,
                    PhuCap = 0
                };

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var content = new StringContent(JsonSerializer.Serialize(payload, jsonOptions), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUrl);
                request.Content = content;

                if (!string.IsNullOrEmpty(token))
                {
                    var actualToken = token.StartsWith("Bearer ") ? token.Substring(7) : token;
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", actualToken);
                }

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorMsg = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to update luong in NhanSu API. URL: {Url}, Status Code: {StatusCode}, Error: {Error}",
                    _httpClient.BaseAddress + requestUrl, response.StatusCode, errorMsg);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while updating luong in NhanSu API");
                return false;
            }
        }

        public async Task<bool> UpdateBoNhiemAsync(Guid nhanVienId, Guid chucVuMoi, decimal phuCapMoi, string token)
        {
            try
            {
                var requestUrl = $"/api/NhanVien/{nhanVienId}/update-chuc-vu";

                var payload = new UpdateBoNhiemRequestDTO
                {
                    ChucVuMoi = chucVuMoi,
                    PhuCapMoi = phuCapMoi
                };

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var content = new StringContent(JsonSerializer.Serialize(payload, jsonOptions), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUrl);
                request.Content = content;

                if (!string.IsNullOrEmpty(token))
                {
                    var actualToken = token.StartsWith("Bearer ") ? token.Substring(7) : token;
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", actualToken);
                }

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorMsg = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to update bo nhiem in NhanSu API. URL: {Url}, Status Code: {StatusCode}, Error: {Error}",
                    _httpClient.BaseAddress + requestUrl, response.StatusCode, errorMsg);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while updating bo nhiem in NhanSu API");
                return false;
            }
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

        public async Task<ChucVuServiceClientDto?> GetChucVuByIdAsync(Guid chucVuId, string token)
        {
            try
            {
                var requestUrl = $"/api/ChucVu/{chucVuId}";

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
                    return JsonSerializer.Deserialize<ChucVuServiceClientDto>(content, jsonOptions);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                var errorMsg = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to get chuc vu from NhanSu API. URL: {Url}, Status Code: {StatusCode}, Error: {Error}",
                    _httpClient.BaseAddress + requestUrl, response.StatusCode, errorMsg);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while getting chuc vu from NhanSu API");
                return null;
            }
        }
    }
}
