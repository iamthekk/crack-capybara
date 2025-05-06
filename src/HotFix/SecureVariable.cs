using System;
using System.Security.Cryptography;
using System.Text;

namespace HotFix
{
	public class SecureVariable
	{
		public int mVariable { get; private set; }

		public void UpdateVariable(int newValue)
		{
			this.mVariable = newValue;
			this.checkNum = this.CalculateCheckNum(this.mVariable);
		}

		public bool IsDataValid()
		{
			return this.checkNum == this.CalculateCheckNum(this.mVariable);
		}

		private int CalculateCheckNum(int value)
		{
			int num = 0;
			foreach (char c in value.ToString())
			{
				num ^= (int)c;
				num = (num << 6) | (num >> 26);
			}
			return num;
		}

		private string CalculateMD5(string valueStr)
		{
			HashAlgorithm hashAlgorithm = MD5.Create();
			byte[] bytes = Encoding.ASCII.GetBytes(valueStr);
			byte[] array = hashAlgorithm.ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("X2"));
			}
			return stringBuilder.ToString();
		}

		private int checkNum;
	}
}
