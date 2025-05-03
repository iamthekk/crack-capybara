using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Framework.Aes
{
	public class AesTools
	{
		static AesTools()
		{
			if (AesTools.AES_IvKey.Length != 16)
			{
				Debug.LogError("IV length must be 16 bytes.");
			}
			if (AesTools.AES_SecretKey.Length != 32)
			{
				Debug.LogError("Key length must be 32 bytes.");
			}
		}

		private static byte[] GenerateKey(string input, int length)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(input);
			Array.Resize<byte>(ref bytes, length);
			return bytes;
		}

		public static byte[] AesEncrypt(byte[] dataToEncrypt)
		{
			byte[] array;
			try
			{
				using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
				{
					rijndaelManaged.KeySize = 256;
					rijndaelManaged.BlockSize = 128;
					rijndaelManaged.Mode = CipherMode.CBC;
					rijndaelManaged.Padding = PaddingMode.PKCS7;
					rijndaelManaged.Key = AesTools.AES_SecretKey;
					rijndaelManaged.IV = AesTools.AES_IvKey;
					using (ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV))
					{
						using (MemoryStream memoryStream = new MemoryStream())
						{
							using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
							{
								cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
							}
							array = memoryStream.ToArray();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("AesTools AesEncrypt =" + ex.ToString());
				array = dataToEncrypt;
			}
			return array;
		}

		public static byte[] AesDecrypt(byte[] dataToDecrypt)
		{
			byte[] array;
			try
			{
				using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
				{
					rijndaelManaged.KeySize = 256;
					rijndaelManaged.BlockSize = 128;
					rijndaelManaged.Mode = CipherMode.CBC;
					rijndaelManaged.Padding = PaddingMode.PKCS7;
					rijndaelManaged.Key = AesTools.AES_SecretKey;
					rijndaelManaged.IV = AesTools.AES_IvKey;
					using (ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV))
					{
						using (MemoryStream memoryStream = new MemoryStream())
						{
							using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
							{
								cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
							}
							array = memoryStream.ToArray();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("AesTools AesDecrypt =" + ex.ToString());
				array = dataToDecrypt;
			}
			return array;
		}

		private static byte[] AES_SecretKey = AesTools.GenerateKey("hgjj1100#$#!!C>?<>{{::@g", 32);

		private static byte[] AES_IvKey = AesTools.GenerateKey("@#^*B>!", 16);

		public const string EncryptFileName = "HotFix.dll";
	}
}
