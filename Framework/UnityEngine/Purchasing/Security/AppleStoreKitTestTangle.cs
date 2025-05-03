using System;

namespace UnityEngine.Purchasing.Security
{
	public class AppleStoreKitTestTangle
	{
		public static byte[] Data()
		{
			if (!AppleStoreKitTestTangle.IsPopulated)
			{
				return null;
			}
			return Obfuscator.DeObfuscate(AppleStoreKitTestTangle.data, AppleStoreKitTestTangle.order, AppleStoreKitTestTangle.key);
		}

		private static byte[] data = Convert.FromBase64String("EeIEhXVow4z/gH/qChdjKx/6P7xGnrYH9awsM8Vs1CnFUxJ4M9ee1g09jwwHD48MDA3Wcp0xIX4C1porTKhzBIC3MDDAfVrV4OLY13rFngkQHgwM8gkIPQ4MDPI9AwsOWBACDAEFXnlif2hGZHk8Bj0ECw5YCQseLz0ACwQni0WL+gAMDAwIDQ6PDAIfjtuKdWCUsFO+B+2Vw/vnzoNgZOa6TYwwijB9XNzJUICo0XJnVRfTOD8+OFcaADk9PT4/Ojw6OD8+OFeB7HzWR0j+IBSbDluMq0njeHpWXgY0QcZCY82+UrqYt7SlUEd5puJibqaalybOJ9Xdv+zYVwA0FmJqJaReeWJ/aEZkeT0TGgA/PT05PTw8OjwcPQILDlgJBgEFXnlif2hGZHk8RYv6AAwEDBsFXnlif2hGZHk9jwyU9r6HWJLiECW1UifcfpI4Sjq2SwkOAQVeeWJ/aEZkeTwcPQILDlgJ6mLtFusAZcAoY857BfhHBqN3eTHCaTFgjVQ+gcc7CyccFoBEwwXyazwXnI20sugBxjG2A0lvK/cJJLtEALY1nWYLyfEA6JhdwJzFtyj5VcR5PBw9AgsOWAkHAQVeeWJ/aEZkec/261IfzA8ODA0MrjY9ND0CCw5YPY8OeT2PD1GtDg8MDw8MDD0ACwTCdaFQtzNSXSZRJIHQz9onGtIvudrpdaWipWCqzNdlIGvBwiQGXSHuyaQGvsJvAva5sfrWiVMl5koA/OAHAQVeeWJ/aEZkeTwcPQILDlgJBg9YXjwaPRgLBCeLRYv6AAwEDBsFzUolRuZXzCYEWyGopwQOdj880eWGofiSvQQ1rmN2xitdBxfshNx/e4h1RcPNyB8cZwIBoyIIx2J3aXItBj0ECw5YCQseD1hePBo9GAsEJ4sM8gkJDg8PiT0bCw5YECgMDPIJAbDN2xBNk1q0NZ16e1/fpUZ9Y7Vq3nsd//1k4cBvlHz0bYeJRTXJsIY9UjwcPQILDlgJDgEFXnlif2hGZGootKZhPmig1c+XdUUC5MHMKgDL+gAMDAYIDQ6PDAwNvw3tMfzl5G8ni0WL+gAMDAYIDT1SPBw9AgsOWG17Ol/h8qEUUAEYIocC+ITF2iYQIXjrQbwf3vDuElMAcLVV2q1x6XI9BwsFJgsMCAgKDg49AAsEJ4tFixPhJpwIqjBC");

		private static int[] order = new int[]
		{
			22, 15, 14, 21, 28, 28, 13, 41, 42, 41,
			24, 20, 40, 13, 24, 40, 33, 24, 24, 36,
			33, 23, 41, 27, 41, 25, 26, 41, 41, 41,
			37, 36, 38, 33, 41, 38, 41, 42, 38, 41,
			40, 43, 43, 43, 44
		};

		private static int key = 13;

		public static readonly bool IsPopulated = true;
	}
}
