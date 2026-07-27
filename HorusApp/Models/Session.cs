using System.Text.Json.Serialization;

namespace HorusApp.Models;

public class Session
{
	public string Token { get; set; } = string.Empty;
	public string Username { get; set; } = string.Empty;
	public string Role { get; set; } = string.Empty;
	public double ExpiresAt { get; set; }

	public Session() { }

	public Session(string token, string username, string role, double expiresAt)
	{
		Token = token;
		Username = username;
		Role = role;
		ExpiresAt = expiresAt;
	}
}

internal record LoginApiRequest(
	[property: JsonPropertyName("username")] string Username,
	[property: JsonPropertyName("password")] string Password
);

internal record LoginApiResponse(
	[property: JsonPropertyName("token")] string Token,
	[property: JsonPropertyName("username")] string Username,
	[property: JsonPropertyName("role")] string Role,
	[property: JsonPropertyName("expires_at")] double ExpiresAt
);