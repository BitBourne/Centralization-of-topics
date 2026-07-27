using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using HorusApp.Models;

namespace HorusApp.Services;

public interface INotificationService
{
	Task<FcmRegistrationResponse> RegisterTokenAsync(string token, string jwtToken, string label = "dispositivo-principal");
	Task<FcmUnregistrationResponse> UnregisterTokenAsync(string fcmToken, string jwtToken);
}

public class NotificationService : INotificationService
{
	private readonly HttpClient _httpClient;
	private readonly JsonSerializerOptions _jsonOptions;

	public NotificationService(HttpClient httpClient)
	{
		_httpClient = httpClient;
		_jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
	}

	public async Task<FcmRegistrationResponse> RegisterTokenAsync(string token, string jwtToken, string label = "dispositivo-principal")
	{
		try
		{
			var registration = new FcmTokenRegistration
			{
				Token = token,
				Platform = "android",
				Label = label
			};

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
			var response = await _httpClient.PostAsJsonAsync("api/mobile/register-token", registration, _jsonOptions);

			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadFromJsonAsync<FcmRegistrationResponse>(_jsonOptions);
				return result ?? new FcmRegistrationResponse { Status = "error", Message = "Respuesta vacía" };
			}

			return new FcmRegistrationResponse { Status = "error", Message = $"Error de servidor: {response.StatusCode}" };
		}
		catch (HttpRequestException ex)
		{
			return new FcmRegistrationResponse { Status = "error", Message = $"Error de red: {ex.Message}" };
		}
	}

	public async Task<FcmUnregistrationResponse> UnregisterTokenAsync(string fcmToken, string jwtToken)
	{
		try
		{
			var unregistration = new FcmTokenUnregistration { Token = fcmToken };

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

			var request = new HttpRequestMessage(HttpMethod.Delete, "api/mobile/unregister-token")
			{
				Content = JsonContent.Create(unregistration, options: _jsonOptions)
			};

			var response = await _httpClient.SendAsync(request);

			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadFromJsonAsync<FcmUnregistrationResponse>(_jsonOptions);
				return result ?? new FcmUnregistrationResponse { Status = "error", Found = false };
			}

			return new FcmUnregistrationResponse { Status = $"error: {response.StatusCode}", Found = false };
		}
		catch (HttpRequestException ex)
		{
			return new FcmUnregistrationResponse { Status = $"error_red: {ex.Message}", Found = false };
		}
	}
}