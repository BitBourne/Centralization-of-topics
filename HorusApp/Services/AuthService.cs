using System.Net.Http.Json;
using HorusApp.Models;

namespace HorusApp.Services;

public interface IAuthService
{
	Task<Session> LoginAsync(string username, string password);
}

public class AuthService : IAuthService
{
	private readonly HttpClient _httpClient;
	private const string TokenKey = "jwt_token";

	public AuthService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<Session> LoginAsync(string username, string password)
	{
		if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
		{
			throw new ArgumentException("El usuario y la contraseña no pueden estar vacíos.");
		}

		var requestBody = new LoginApiRequest(username, password);
		var response = await _httpClient.PostAsJsonAsync("api/auth/login", requestBody);

		if (!response.IsSuccessStatusCode)
		{
			throw new HttpRequestException("Credenciales incorrectas o servidor no disponible.");
		}

		var result = await response.Content.ReadFromJsonAsync<LoginApiResponse>();
		if (result == null) throw new Exception("Error al procesar la respuesta del servidor.");

		await SecureStorage.Default.SetAsync(TokenKey, result.Token);

		return new Session(result.Token, result.Username, result.Role, result.ExpiresAt);
	}
}