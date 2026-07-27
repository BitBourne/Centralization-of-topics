namespace HorusApp.Models;

using System.Text.Json.Serialization;

public class MobileAlertsResponseDto
{
	[JsonPropertyName("total")]
	public int Total { get; set; }
		
	[JsonPropertyName("alerts")]
	public List<MobileAlertDto> Alerts { get; set; } = new();

	[JsonPropertyName("limit")]
	public int Limit { get; set; }

	[JsonPropertyName("offset")]
	public int Offset { get; set; }
}

public class MobileAlertDto
{
	[JsonPropertyName("id")]
	public string Id { get; set; } = string.Empty;

	[JsonPropertyName("rule_id")]
	public string RuleId { get; set; } = string.Empty;

	[JsonPropertyName("rule_name")]
	public string RuleName { get; set; } = string.Empty;

	[JsonPropertyName("rule_description")]
	public string RuleDescription { get; set; } = string.Empty;

	[JsonPropertyName("level")]
	public int Level { get; set; }

	[JsonPropertyName("event_type")]
	public string EventType { get; set; } = string.Empty;

	[JsonPropertyName("agent_id")]
	public string AgentId { get; set; } = string.Empty;

	[JsonPropertyName("src_ip")]
	public string? SrcIp { get; set; }

	[JsonPropertyName("dst_user")]
	public string? DstUser { get; set; }

	[JsonPropertyName("action")]
	public string Action { get; set; } = string.Empty;

	[JsonPropertyName("raw_log")]
	public string? RawLog { get; set; }

	[JsonPropertyName("path")]
	public string? Path { get; set; }

	[JsonPropertyName("mitre")]
	public MitreDto? Mitre { get; set; }

	[JsonPropertyName("created_at")]
	public double CreatedAtUnix { get; set; }

	[JsonPropertyName("time_ago")]
	public double TimeAgo { get; set; }

	[JsonPropertyName("can_block")]
	public bool CanBlock { get; set; }

	[JsonPropertyName("suggested_actions")]
	public List<string> SuggestedActions { get; set; } = new();

	[JsonPropertyName("severity_label")]
	public string SeverityLabel { get; set; } = string.Empty;

	// Agrega estas propiedades dentro de tu clase MobileAlertDto
	public string FirstTactic => Mitre?.Tactic?.FirstOrDefault() ?? "N/A";
	public string FirstTechnique => Mitre?.Technique?.FirstOrDefault() ?? "N/A";


	// --- PROPIEDADES CALCULADAS PARA LA UI ---

	// Formateador de tiempo relativo seguro para XAML (evita errores de sintaxis)
	public string TimeAgoFormatted
	{
		get
		{
			if (TimeAgo < 60) return $"Hace\n{TimeAgo:F0}s";
			if (TimeAgo < 3600) return $"Hace\n{TimeAgo / 60:F1}m";
			return $"Hace\n{TimeAgo / 3600:F1}h";
		}
	}

	public Color SeverityColor
	{
		get
		{
			if (string.IsNullOrWhiteSpace(SeverityLabel))
				return Colors.Gray;

			return SeverityLabel.ToLower().Trim() switch
			{
				"high" or "alta" or "critical" or "critica" => Color.FromArgb("#D32F2F"),
				"medium" or "media" => Color.FromArgb("#F57C00"),
				"low" or "baja" or "info" => Color.FromArgb("#388E3C"),
				_ => Colors.Gray
			};
		}
	}
}

public class MitreDto
{
	[JsonPropertyName("tactic")]
	public List<string> Tactic { get; set; } = new();

	[JsonPropertyName("technique")]
	public List<string> Technique { get; set; } = new();
}