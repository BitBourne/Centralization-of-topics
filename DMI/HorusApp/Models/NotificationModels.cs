namespace HorusApp.Models;

public class FcmTokenRegistration
{
	public string Token { get; set; } = string.Empty;
	public string Platform { get; set; } = "android";
	public string Label { get; set; } = "dispositivo-principal";
}

public class FcmRegistrationResponse
{
	public string Status { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;
	public bool PushEnabled { get; set; }
}

public class FcmTokenUnregistration
{
	public string Token { get; set; } = string.Empty;
}

public class FcmUnregistrationResponse
{
	public string Status { get; set; } = string.Empty;
	public bool Found { get; set; }
}