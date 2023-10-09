using System.Text.Json.Serialization;

namespace Wake {
	public class Config {
		[JsonPropertyName("BroadcastIP")]
		public string BroadcastIP { get; set; }
		
		[JsonPropertyName("PCList")]
		public List<WakePC> WakePCs { get; set; }
	}
	public class WakePC {
		[JsonPropertyName("NickName")]
		public string Nickname { get; set; }

		[JsonPropertyName("MAC")]
		public string MAC { get; set; }
	}
}
