using System.Globalization;
using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Net.NetworkInformation;

namespace Wake {
	internal class Program {
		static readonly string ConfigPath = $"{AppDomain.CurrentDomain.BaseDirectory}config.json";

		private static readonly string[] NicBlacklist = { "vethernet", "vmware" };

		static void Main(string[] args) {
			try {
				var command = args.FirstOrDefault(string.Empty).ToLower();

				var pcs = Load();
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
						Wake(pc);
						break;
				}
			} catch (Exception) {
				return;
			}
		}

		static List<WakePC> Load() {
			try {
				var pcs = JsonSerializer.Deserialize<List<WakePC>>(File.ReadAllText(ConfigPath));
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

		static void Wake(WakePC pc) {
			try {
				var broadcastIP = GetBroadcastIP();
				var macAddress = pc.MAC;

				var udpClient = new UdpClient() {
					EnableBroadcast = true
				};
				udpClient.Connect(broadcastIP, 7);

				var magicPacket = BuildPacket(pc.MAC);

				udpClient.Send(magicPacket, magicPacket.Length);
				udpClient.Close();
			} catch (Exception ex) {
				Output.Error(ex.ToString());
				throw;
			}
		}

		static IPAddress GetBroadcastIP() {
			var nic = NetworkInterface.GetAllNetworkInterfaces().First(nic => 
				nic.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
				nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel &&
				!NicBlacklist.Any(name => nic.Name.StartsWith(name)) &&
				nic.SupportsMulticast &&
				nic.OperationalStatus == OperationalStatus.Up &&
				nic.GetIPProperties().GetIPv4Properties != null &&
				nic.GetIPProperties().MulticastAddresses.Count > 0) ?? throw new Exception("No nics found!");

			var ipv4Addr = nic.GetIPProperties().UnicastAddresses.First(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork);

			var ip = ipv4Addr.Address.GetAddressBytes();
			var mask = ipv4Addr.IPv4Mask.GetAddressBytes();

			byte[] broadcastIPBytes = new byte[4];

			for (int i = 0; i < 4; i++)
				broadcastIPBytes[i] = (byte)(ip[i] | ~mask[i]);

			return new IPAddress(broadcastIPBytes);
		}

		static byte[] BuildPacket(string macAddress) {
			var magicPacket = Enumerable.Repeat(Convert.ToByte(255), 17 * 6).ToArray();
			byte[] macAddrBytes = macAddress.Split([':', '-']).Select(str => byte.Parse(str, NumberStyles.HexNumber)).ToArray();

			for (int i = 1; i <= 16; i++)
				for (int j = 0; j < 6; j++)
					magicPacket[i * 6 + j] = macAddrBytes[j];

			return magicPacket;
		}
	}
}