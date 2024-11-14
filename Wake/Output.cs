namespace Wake {
	internal static class Output {
		internal static readonly string NewLine = Environment.NewLine;

		internal static void ResetColor() {
			Console.ResetColor();
		}
		internal static void Error(string msg) { 
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(msg);
			ResetColor();
		}
		internal static void Success(string msg) {
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(msg);
			ResetColor();
		}
		internal static void Info(string msg) {
			Console.WriteLine(msg);
		}
		internal static void InfoR(string msg) {
			Console.Write(msg);
		}

		internal static void Usage() {
			Success($"Usage:" +
				$"\tWake list - list all available PCs{NewLine}" +
				$"\tWake ip - output broadcast ip{NewLine}" +
				$"\tWake [nickname] - wake PC with specific nickname");
		}
	}
}
