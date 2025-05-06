using System;

namespace UnityEngine.Purchasing.Security
{
	public class GooglePlayTangle
	{
		public static byte[] Data()
		{
			if (!GooglePlayTangle.IsPopulated)
			{
				return null;
			}
			return Obfuscator.DeObfuscate(GooglePlayTangle.data, GooglePlayTangle.order, GooglePlayTangle.key);
		}

		private static byte[] data = Convert.FromBase64String("lIMNlMAqS1PgclDpqcuagKLZOOeGHP7gFAq1bFd7y4uLojjPKOUzSBtKf4B+bRHqVA2CgoQG8BHRZu788kDD4PLPxMvoRIpENc/Dw8PHwsFiz1KrV8n1wpfLEp7c+/Zr/J4lJvoQsWnhzoVs/L7Fd2Ad4LYQzZyDVsmdKcQ6H9fVZZ1Q9AVUWHmSM+GlvUlejBSR61xeq5ujp09KqNgoVmkPfhPYtd7SMNb5KKUZhIH85ZpdI/B/UVcgLPLS6SOhojk8HZjQvbiKqMz62c5nZpOaS/PB8iGxdGs4GEDDzcLyQMPIwEDDw8JXW99jMWiNLn/2ZL1wK0segXSxU9HGgyMto7oyVZ8Bn2BjpKnSHjTXQ5UXd03zyoX0528PYc/jA8DBw8LD");

		private static int[] order = new int[]
		{
			12, 10, 3, 12, 9, 13, 8, 13, 12, 11,
			11, 11, 13, 13, 14
		};

		private static int key = 194;

		public static readonly bool IsPopulated = true;
	}
}
