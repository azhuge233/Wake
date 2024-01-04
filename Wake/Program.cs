using System.Globalization;
using System.Net.Sockets;
using System.Net;
using System.Text.Json;

namespace Wake {
	internal class Program {
		static readonly string ConfigPath = $"{AppDomain.CurrentDomain.BaseDirectory}config.json";

		static void Main(string[] args) {
			try {
				var command = args.FirstOrDefault(string.Empty).ToLower();

				var config = Load();
				var broadcastIP = config.BroadcastIP;
				var pcs = config.WakePCs;
				var nicknames = pcs.Select(pc => pc.Nickname.ToLower()).ToHashSet();

				if (args.Length == 0) {
					Output.Usage();
					return;
				} else if (args.Length != 1) {
					Output.Error("Invalid arguments.");
					Output.Usage();
					return;
				} else if (command != "list" && !nicknames.Contains(command)) {
					Output.Error($"No PC with nickname: {args.Last()}");
					return;
				}

				switch (command) {
					case "list":
						List(pcs);
						break;
					default:
						var pc = pcs.First(pc => pc.Nickname.ToLower().Equals(command));
						Wake(broadcastIP, pc);
						break;
				}
			} catch (Exception) {
				return;
			}
		}

		static Config Load() {
			try {
				var pcs = JsonSerializer.Deserialize<Config>(File.ReadAllText(ConfigPath));
				return pcs;
			} catch (Exception ex) {
				Output.Error(ex.ToString());
				throw;
			}
		}

		static void List(List<WakePC> pcs) {
			pcs.ForEach(pc => Output.Info($"NickName: {pc.Nickname}{Output.NewLine}" +
				$"MAC: {pc.MAC}{Output.NewLine}"));
		}

		static void Wake(string broadcastIP, WakePC pc) {
			try {
				var ipAddress = broadcastIP;
				var macAddress = pc.MAC;

				var udpClient = new UdpClient() {
					EnableBroadcast = true
				};
				udpClient.Connect(IPAddress.Parse(ipAddress), new Random().Next(20000, 60000));

				var magicPacket = Enumerable.Repeat(Convert.ToByte(255), 17 * 6).ToArray();
				byte[] macAddrBytes = macAddress.Split([':', '-']).Select(str => byte.Parse(str, NumberStyles.HexNumber)).ToArray();

				for (int i = 1; i <= 16; i++)
					for (int j = 0; j < 6; j++)
						magicPacket[i * 6 + j] = macAddrBytes[j];

				udpClient.Send(magicPacket, magicPacket.Length);
				udpClient.Close();
			} catch (Exception ex) {
				Output.Error(ex.ToString());
				throw;
			}
		}
	}
}