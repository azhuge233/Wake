using System.Text.Json.Serialization;

namespace Wake {

	public class Config {
		[JsonPropertyName("UseConfigBroadcastIP")] 
		public bool UseConfigBroadcastIP { get; set; }

		[JsonPropertyName("BroadcastIP")]
		public string BroadcastIP { get; set; }

		[JsonPropertyName("PCList")]
		public List<WakePC> PCList { get; set; }
	}

	public class WakePC {
		[JsonPropertyName("NickName")]
		public string Nickname { get; set; }

		[JsonPropertyName("MAC")]
		public string MAC { get; set; }
	}
}
