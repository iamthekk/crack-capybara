using System;
using System.Net;
using System.Text;

namespace LocalModels
{
	public abstract class BaseLocalBean : IBeanBuilder
	{
		protected BaseLocalBean()
		{
			this.position = 0;
		}

		public abstract BaseLocalBean createBean();

		public int readFromBytes(byte[] raws, int startPos)
		{
			this.raws = raws;
			this.position = startPos;
			try
			{
				this.messageLength = this.readInt();
				if (!this.readImpl())
				{
					return -1;
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
				throw;
			}
			return this.position;
		}

		public int getLength()
		{
			return this.messageLength;
		}

		protected void readBytes(byte[] datas, int buffLength)
		{
			int i = 0;
			while (i < buffLength)
			{
				datas[i] = this.raws[this.position];
				i++;
				this.position++;
			}
		}

		protected short readShort()
		{
			byte[] array = new byte[2];
			this.readBytes(array, 2);
			return IPAddress.NetworkToHostOrder(BitConverter.ToInt16(array, 0));
		}

		protected bool readBool()
		{
			return this.readShort() == 1;
		}

		protected int readInt()
		{
			byte[] array = new byte[4];
			this.readBytes(array, 4);
			return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(array, 0));
		}

		protected long readLong()
		{
			byte[] array = new byte[8];
			this.readBytes(array, 8);
			return IPAddress.NetworkToHostOrder(BitConverter.ToInt64(array, 0));
		}

		protected DateTime readDate()
		{
			long num = this.readLong();
			num *= (long)BaseLocalBean.time_factor;
			num += BaseLocalBean.t19700101;
			return new DateTime(num);
		}

		protected float readFloat()
		{
			byte[] array = new byte[4];
			this.readBytes(array, 4);
			return BitConverter.ToSingle(BitConverter.GetBytes(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(array, 0))), 0);
		}

		protected double readDouble()
		{
			byte[] array = new byte[8];
			this.readBytes(array, 8);
			return BitConverter.Int64BitsToDouble(IPAddress.NetworkToHostOrder(BitConverter.ToInt64(array, 0)));
		}

		protected string readLocalString()
		{
			short num = this.readShort();
			byte[] array = new byte[(int)(num - 2)];
			this.readBytes(array, (int)(num - 2));
			string text;
			try
			{
				text = BaseLocalBean.encoding.GetString(array);
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
				text = "";
			}
			return text;
		}

		protected string readCommonString()
		{
			string text = this.readLocalString();
			return this.toCommonString(text);
		}

		protected int[] readArrayint()
		{
			short num = this.readShort();
			int[] array = new int[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				array[i] = this.readInt();
			}
			return array;
		}

		protected long[] readArraylong()
		{
			short num = this.readShort();
			long[] array = new long[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				array[i] = this.readLong();
			}
			return array;
		}

		protected float[] readArrayfloat()
		{
			short num = this.readShort();
			float[] array = new float[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				array[i] = this.readFloat();
			}
			return array;
		}

		protected string[] readArraystring()
		{
			short num = this.readShort();
			string[] array = new string[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				array[i] = this.readLocalString();
			}
			return array;
		}

		protected string toCommonString(string key)
		{
			if (key == null)
			{
				return key;
			}
			return key;
		}

		public abstract bool readImpl();

		private int messageLength;

		private int position;

		private static readonly long t19700101 = new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks;

		private static readonly int time_factor = 10000;

		private static readonly Encoding encoding = Encoding.UTF8;

		private byte[] raws;
	}
}
