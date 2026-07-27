namespace HorusApp.Services;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using HorusApp.Models;
using Microsoft.Maui.Storage;

public class AlertService
{
	private readonly HttpClient _httpClient;
	private const string TokenKey = "jwt_token";

	public AlertService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<List<MobileAlertDto>> GetAlertsAsync()
	{
		// 1. Obtener el token almacenado al iniciar sesión
		var token = await SecureStorage.Default.GetAsync(TokenKey);

		if (string.IsNullOrEmpty(token))
		{
			throw new UnauthorizedAccessException("No se encontró una sesión activa.");
		}

		// 2. Asignar el token a la cabecera Bearer
		_httpClient.DefaultRequestHeaders.Authorization =
			new AuthenticationHeaderValue("Bearer", token);

		// 3. Petición GET al endpoint de alertas
		var response = await _httpClient.GetAsync("api/mobile/alerts");

		if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
		{
			throw new UnauthorizedAccessException("La sesión ha expirado.");
		}

		if (!response.IsSuccessStatusCode)
		{
			throw new HttpRequestException($"Error del servidor: {response.StatusCode}");
		}

		// 4. Deserializar la respuesta envelope que contiene la propiedad 'alerts'
		var result = await response.Content.ReadFromJsonAsync<MobileAlertsResponseDto>();

		return result?.Alerts ?? new List<MobileAlertDto>();
	}


	public async Task<MobileAlertDto?> GetAlertByIdAsync(string id)
	{
		try
		{
			// Reemplaza _httpClient por el cliente configurado en tu servicio
			var response = await _httpClient.GetFromJsonAsync<MobileAlertDto>($"api/mobile/alerts/{id}");
			return response;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error al obtener detalle de la alerta: {ex.Message}");
			return null;
		}
	}
}