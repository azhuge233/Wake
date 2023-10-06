using System.Text.Json.Serialization;

namespace Wake {
	public class WakePC {
		[JsonPropertyName("NickName")]
		public string Nickname { get; set; }

		[JsonPropertyName("IPAddress")]
		public string IPAddr { get; set; }

		[JsonPropertyName("MAC")]
		public string MAC { get; set; }
	}
}
