using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace HotFix
{
	public sealed class Bson
	{
		public static byte[] ToBson(object obj, bool cache = false)
		{
			if (obj == null)
			{
				return null;
			}
			byte[] bytes;
			using (Bson.Writer writer = new Bson.Writer(null))
			{
				writer.Encode(obj);
				bytes = writer.Bytes;
			}
			return bytes;
		}

		public static void ToBson(object obj, Bson.Writer writer)
		{
			if (obj == null || writer == null)
			{
				return;
			}
			writer.Encode(obj);
		}

		public static T ToObject<T>(byte[] bson)
		{
			T t;
			if (bson == null || bson.Length == 0)
			{
				t = default(T);
				return t;
			}
			object obj;
			if (Bson._bsonObjects.TryGetValue(bson, out obj) && obj != null)
			{
				return (T)((object)obj);
			}
			using (Bson.Reader reader = new Bson.Reader(bson))
			{
				obj = reader.Decode(typeof(T));
				t = (T)((object)obj);
			}
			return t;
		}

		public static T ToObject<T>(Bson.Reader reader)
		{
			if (reader == null)
			{
				return default(T);
			}
			return (T)((object)reader.Decode(typeof(T)));
		}

		public static void Clear()
		{
			Bson._bsonObjects.Clear();
			Bson._propertyMetas.Clear();
			Bson._arrayMetas.Clear();
			Bson._objectMetas.Clear();
		}

		private static IList<Bson.PropertyMeta> AddPropertyMetas(Type type)
		{
			IList<Bson.PropertyMeta> list;
			if (Bson._propertyMetas.TryGetValue(type, out list) && list != null)
			{
				return list;
			}
			list = new List<Bson.PropertyMeta>();
			foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				if (!(propertyInfo.Name == "Item"))
				{
					Bson.PropertyMeta propertyMeta = new Bson.PropertyMeta
					{
						Info = propertyInfo,
						IsField = false
					};
					list.Add(propertyMeta);
				}
			}
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
			{
				Bson.PropertyMeta propertyMeta2 = new Bson.PropertyMeta
				{
					Info = fieldInfo,
					IsField = true
				};
				list.Add(propertyMeta2);
			}
			Bson._propertyMetas[type] = list;
			return list;
		}

		private static Bson.ArrayMeta AddArrayMeta(Type type)
		{
			Bson.ArrayMeta arrayMeta;
			if (Bson._arrayMetas.TryGetValue(type, out arrayMeta))
			{
				return arrayMeta;
			}
			arrayMeta = new Bson.ArrayMeta
			{
				IsArray = type.IsArray
			};
			if (type.GetInterface("System.Collections.IList") != null)
			{
				arrayMeta.IsList = true;
			}
			foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				if (!(propertyInfo.Name != "Item"))
				{
					ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
					if (indexParameters.Length == 1)
					{
						arrayMeta.ItemType = ((indexParameters[0].ParameterType == typeof(int)) ? propertyInfo.PropertyType : typeof(object));
					}
				}
			}
			Bson._arrayMetas[type] = arrayMeta;
			return arrayMeta;
		}

		private static Bson.ObjectMeta AddObjectMeta(Type type)
		{
			Bson.ObjectMeta objectMeta;
			if (Bson._objectMetas.TryGetValue(type, out objectMeta))
			{
				return objectMeta;
			}
			objectMeta = default(Bson.ObjectMeta);
			if (type.GetInterface("System.Collections.IDictionary") != null)
			{
				objectMeta.IsDict = true;
			}
			objectMeta.Properties = new Dictionary<string, Bson.PropertyMeta>();
			foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				if (propertyInfo.Name == "Item")
				{
					ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
					if (indexParameters.Length == 1)
					{
						objectMeta.ElemType = ((indexParameters[0].ParameterType == typeof(string)) ? propertyInfo.PropertyType : typeof(object));
					}
				}
				else
				{
					Bson.PropertyMeta propertyMeta = new Bson.PropertyMeta
					{
						Info = propertyInfo,
						Type = propertyInfo.PropertyType
					};
					objectMeta.Properties.Add(propertyInfo.Name, propertyMeta);
				}
			}
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
			{
				Bson.PropertyMeta propertyMeta2 = new Bson.PropertyMeta
				{
					Info = fieldInfo,
					IsField = true,
					Type = fieldInfo.FieldType
				};
				objectMeta.Properties.Add(fieldInfo.Name, propertyMeta2);
			}
			Bson._objectMetas[type] = objectMeta;
			return objectMeta;
		}

		private static readonly Dictionary<byte[], object> _bsonObjects = new Dictionary<byte[], object>(Bson.ByteArrayComparer.Default);

		private static readonly IDictionary<Type, IList<Bson.PropertyMeta>> _propertyMetas = new Dictionary<Type, IList<Bson.PropertyMeta>>();

		private static readonly IDictionary<Type, Bson.ArrayMeta> _arrayMetas = new Dictionary<Type, Bson.ArrayMeta>();

		private static readonly IDictionary<Type, Bson.ObjectMeta> _objectMetas = new Dictionary<Type, Bson.ObjectMeta>();

		private enum ValueType : byte
		{
			Bool = 1,
			SByte,
			Byte,
			Char,
			Short,
			UShort,
			Int,
			UInt,
			Int64,
			UInt64,
			Decimal,
			Float,
			Double,
			DateTime,
			Enum,
			String,
			Array,
			Vector2,
			Vector3,
			Vector4,
			Color,
			Color32,
			Quaternion,
			Bounds,
			Rect,
			Matrix,
			Object
		}

		private struct PropertyMeta
		{
			public MemberInfo Info { readonly get; set; }

			public bool IsField { readonly get; set; }

			public Type Type { readonly get; set; }
		}

		private struct ArrayMeta
		{
			public Type ItemType { readonly get; set; }

			public bool IsArray { readonly get; set; }

			public bool IsList { readonly get; set; }
		}

		private struct ObjectMeta
		{
			public Type ElemType { readonly get; set; }

			public bool IsDict { readonly get; set; }

			public IDictionary<string, Bson.PropertyMeta> Properties { readonly get; set; }
		}

		private sealed class ByteArrayComparer : IEqualityComparer<byte[]>
		{
			public static Bson.ByteArrayComparer Default
			{
				get
				{
					return new Bson.ByteArrayComparer();
				}
			}

			public bool Equals(byte[] left, byte[] right)
			{
				if (left == null || right == null)
				{
					return left == right;
				}
				if (left == right)
				{
					return true;
				}
				if (left.Length != right.Length)
				{
					return false;
				}
				for (int i = 0; i < left.Length; i++)
				{
					if (left[i] != right[i])
					{
						return false;
					}
				}
				return true;
			}

			public int GetHashCode(byte[] obj)
			{
				if (obj == null)
				{
					throw new ArgumentNullException("obj");
				}
				int num = 0;
				int num2 = 0;
				foreach (byte b in obj)
				{
					num += (int)b;
					num2 += num;
				}
				return num ^ num2;
			}
		}

		public sealed class Reader : IDisposable
		{
			public Reader(byte[] bson)
				: this(new MemoryStream(bson))
			{
			}

			public Reader(MemoryStream stream)
			{
				this._stream = stream ?? new MemoryStream();
				this._reader = new BinaryReader(this._stream);
			}

			public object Decode(Type type)
			{
				Type type2 = Nullable.GetUnderlyingType(type) ?? type;
				switch (this._reader.ReadByte())
				{
				case 1:
					if (type2 != typeof(bool))
					{
						HLog.LogError(string.Format("Bson type is bool, but expect {0}", type2));
					}
					return this._reader.ReadBoolean();
				case 2:
					if (type2 != typeof(sbyte))
					{
						HLog.LogError(string.Format("Bson type is sbyte, but expect {0}", type2));
					}
					return this._reader.ReadSByte();
				case 3:
					if (type2 != typeof(byte))
					{
						HLog.LogError(string.Format("Bson type is byte, but expect {0}", type2));
					}
					return this._reader.ReadByte();
				case 4:
					if (type2 != typeof(char))
					{
						HLog.LogError(string.Format("Bson type is char, but expect {0}", type2));
					}
					return this._reader.ReadChar();
				case 5:
					if (type2 != typeof(short))
					{
						HLog.LogError(string.Format("Bson type is short, but expect {0}", type2));
					}
					return this._reader.ReadInt16();
				case 6:
					if (type2 != typeof(ushort))
					{
						HLog.LogError(string.Format("Bson type is ushort, but expect {0}", type2));
					}
					return this._reader.ReadUInt16();
				case 7:
					if (type2 != typeof(int))
					{
						HLog.LogError(string.Format("Bson type is int, but expect {0}", type2));
					}
					return this._reader.ReadInt32();
				case 8:
					if (type2 != typeof(uint))
					{
						HLog.LogError(string.Format("Bson type is uint, but expect {0}", type2));
					}
					return this._reader.ReadUInt32();
				case 9:
					if (type2 != typeof(long))
					{
						HLog.LogError(string.Format("Bson type is long, but expect {0}", type2));
					}
					return this._reader.ReadInt64();
				case 10:
					if (type2 != typeof(ulong))
					{
						HLog.LogError(string.Format("Bson type is ulong, but expect {0}", type2));
					}
					return this._reader.ReadUInt64();
				case 11:
					if (type2 != typeof(decimal))
					{
						HLog.LogError(string.Format("Bson type is decimal, but expect {0}", type2));
					}
					return this._reader.ReadDecimal();
				case 12:
					if (type2 != typeof(float))
					{
						HLog.LogError(string.Format("Bson type is float, but expect {0}", type2));
					}
					return this._reader.ReadSingle();
				case 13:
					if (type2 != typeof(double))
					{
						HLog.LogError(string.Format("Bson type is double, but expect {0}", type2));
					}
					return this._reader.ReadDouble();
				case 14:
					if (type2 != typeof(DateTime))
					{
						HLog.LogError(string.Format("Bson type is DateTime, but expect {0}", type2));
					}
					return this.ReadDateTime();
				case 15:
					if (!type2.IsEnum)
					{
						HLog.LogError(string.Format("Bson type is Enum, but expect {0}", type2));
					}
					return this.ReadEnum(type2);
				case 16:
					if (type2 != typeof(string))
					{
						HLog.LogError(string.Format("Bson type is string, but expect {0}", type2));
					}
					return this._reader.ReadString();
				case 17:
					if (!type2.IsArray && type2.GetInterface("System.Collections.IList") == null)
					{
						HLog.LogError(string.Format("Bson type is Array, but expect {0}", type2));
					}
					return this.ReadArray(type2);
				case 18:
					if (type2 != typeof(Vector2))
					{
						HLog.LogError(string.Format("Bson type is vector2, but expect {0}", type2));
					}
					return this.ReadVector2();
				case 19:
					if (type2 != typeof(Vector3))
					{
						HLog.LogError(string.Format("Bson type is vector3, but expect {0}", type2));
					}
					return this.ReadVector3();
				case 20:
					if (type2 != typeof(Vector4))
					{
						HLog.LogError(string.Format("Bson type is vector4, but expect {0}", type2));
					}
					return this.ReadVector4();
				case 21:
					if (type2 != typeof(Color))
					{
						HLog.LogError(string.Format("Bson type is color, but expect {0}", type2));
					}
					return this.ReadColor();
				case 22:
					if (type2 != typeof(Color32))
					{
						HLog.LogError(string.Format("Bson type is color32, but expect {0}", type2));
					}
					return this.ReadColor32();
				case 23:
					if (type2 != typeof(Quaternion))
					{
						HLog.LogError(string.Format("Bson type is quaternion, but expect {0}", type2));
					}
					return this.ReadQuaternion();
				case 24:
					if (type2 != typeof(Bounds))
					{
						HLog.LogError(string.Format("Bson type is bounds, but expect {0}", type2));
					}
					return this.ReadBounds();
				case 25:
					if (type2 != typeof(Rect))
					{
						HLog.LogError(string.Format("Bson type is rect, but expect {0}", type2));
					}
					return this.ReadRect();
				case 26:
					if (type2 != typeof(Matrix4x4))
					{
						HLog.LogError(string.Format("Bson type is matrix4x4, but expect {0}", type2));
					}
					return this.ReadMatrix();
				case 27:
					return this.ReadObject(type2);
				default:
					return null;
				}
			}

			private DateTime ReadDateTime()
			{
				long num = this._reader.ReadInt64();
				return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) + new TimeSpan(num * 10000L);
			}

			private object ReadEnum(Type type)
			{
				return Enum.ToObject(type, this._reader.ReadInt32());
			}

			private object ReadArray(Type type)
			{
				Bson.ArrayMeta arrayMeta = Bson.AddArrayMeta(type);
				IList list;
				Type type2;
				if (arrayMeta.IsArray)
				{
					list = new ArrayList();
					type2 = type.GetElementType();
				}
				else
				{
					list = (IList)Activator.CreateInstance(type);
					type2 = arrayMeta.ItemType;
				}
				long num = (long)this._reader.ReadInt32() + this._reader.BaseStream.Position;
				while (this._reader.BaseStream.Position < num)
				{
					object obj = this.Decode(type2);
					list.Add(obj);
				}
				object obj2;
				if (arrayMeta.IsArray)
				{
					obj2 = Array.CreateInstance(type2, list.Count);
					for (int i = 0; i < list.Count; i++)
					{
						((Array)obj2).SetValue(list[i], i);
					}
				}
				else
				{
					obj2 = list;
				}
				return obj2;
			}

			private Vector2 ReadVector2()
			{
				Vector2 vector = default(Vector2);
				vector.x = this._reader.ReadSingle();
				vector.y = this._reader.ReadSingle();
				return vector;
			}

			private Vector3 ReadVector3()
			{
				Vector3 vector = default(Vector3);
				vector.x = this._reader.ReadSingle();
				vector.y = this._reader.ReadSingle();
				vector.z = this._reader.ReadSingle();
				return vector;
			}

			private Vector4 ReadVector4()
			{
				Vector4 vector = default(Vector4);
				vector.x = this._reader.ReadSingle();
				vector.y = this._reader.ReadSingle();
				vector.z = this._reader.ReadSingle();
				vector.w = this._reader.ReadSingle();
				return vector;
			}

			private Color ReadColor()
			{
				Color color = default(Color);
				color.r = this._reader.ReadSingle();
				color.g = this._reader.ReadSingle();
				color.b = this._reader.ReadSingle();
				color.a = this._reader.ReadSingle();
				return color;
			}

			private Color32 ReadColor32()
			{
				Color32 color = default(Color32);
				color.r = this._reader.ReadByte();
				color.g = this._reader.ReadByte();
				color.b = this._reader.ReadByte();
				color.a = this._reader.ReadByte();
				return color;
			}

			private Quaternion ReadQuaternion()
			{
				Quaternion quaternion = default(Quaternion);
				quaternion.x = this._reader.ReadSingle();
				quaternion.y = this._reader.ReadSingle();
				quaternion.z = this._reader.ReadSingle();
				quaternion.w = this._reader.ReadSingle();
				return quaternion;
			}

			private Bounds ReadBounds()
			{
				Bounds bounds = default(Bounds);
				Vector3 vector = default(Vector3);
				vector.x = this._reader.ReadSingle();
				vector.y = this._reader.ReadSingle();
				vector.z = this._reader.ReadSingle();
				bounds.center = vector;
				vector = default(Vector3);
				vector.x = this._reader.ReadSingle();
				vector.y = this._reader.ReadSingle();
				vector.z = this._reader.ReadSingle();
				bounds.extents = vector;
				return bounds;
			}

			private Rect ReadRect()
			{
				Rect rect = default(Rect);
				rect.x = this._reader.ReadSingle();
				rect.y = this._reader.ReadSingle();
				rect.width = this._reader.ReadSingle();
				rect.height = this._reader.ReadSingle();
				return rect;
			}

			private Matrix4x4 ReadMatrix()
			{
				Matrix4x4 matrix4x = default(Matrix4x4);
				matrix4x.m00 = this._reader.ReadSingle();
				matrix4x.m10 = this._reader.ReadSingle();
				matrix4x.m20 = this._reader.ReadSingle();
				matrix4x.m30 = this._reader.ReadSingle();
				matrix4x.m01 = this._reader.ReadSingle();
				matrix4x.m11 = this._reader.ReadSingle();
				matrix4x.m21 = this._reader.ReadSingle();
				matrix4x.m31 = this._reader.ReadSingle();
				matrix4x.m02 = this._reader.ReadSingle();
				matrix4x.m12 = this._reader.ReadSingle();
				matrix4x.m22 = this._reader.ReadSingle();
				matrix4x.m32 = this._reader.ReadSingle();
				matrix4x.m03 = this._reader.ReadSingle();
				matrix4x.m13 = this._reader.ReadSingle();
				matrix4x.m23 = this._reader.ReadSingle();
				matrix4x.m33 = this._reader.ReadSingle();
				return matrix4x;
			}

			private object ReadObject(Type type)
			{
				Bson.ObjectMeta objectMeta = Bson.AddObjectMeta(type);
				object obj = Activator.CreateInstance(type);
				long num = (long)this._reader.ReadInt32() + this._reader.BaseStream.Position;
				while (this._reader.BaseStream.Position < num)
				{
					string text = this._reader.ReadString();
					Bson.PropertyMeta propertyMeta;
					if (objectMeta.Properties.TryGetValue(text, out propertyMeta))
					{
						object obj2 = this.Decode(propertyMeta.Type);
						if (propertyMeta.IsField)
						{
							((FieldInfo)propertyMeta.Info).SetValue(obj, obj2);
						}
						else
						{
							PropertyInfo propertyInfo = (PropertyInfo)propertyMeta.Info;
							if (propertyInfo.CanWrite)
							{
								propertyInfo.SetValue(obj, obj2, null);
							}
						}
					}
					else
					{
						object obj3 = this.Decode(objectMeta.ElemType);
						if (objectMeta.IsDict)
						{
							((IDictionary)obj).Add(text, obj3);
						}
						else
						{
							HLog.LogError(string.Format("The type {0} doesn't have the property '{1}'", type, text));
						}
					}
				}
				return obj;
			}

			public void Dispose()
			{
				if (this._isDisposed)
				{
					return;
				}
				BinaryReader reader = this._reader;
				if (reader != null)
				{
					reader.Close();
				}
				MemoryStream stream = this._stream;
				if (stream != null)
				{
					stream.Dispose();
				}
				this._isDisposed = true;
			}

			private readonly MemoryStream _stream;

			private readonly BinaryReader _reader;

			private bool _isDisposed;
		}

		public sealed class Writer : IDisposable
		{
			public byte[] Bytes
			{
				get
				{
					if (this._stream == null)
					{
						return null;
					}
					byte[] array = new byte[this._stream.Length];
					Array.Copy(this._stream.GetBuffer(), array, array.Length);
					return array;
				}
			}

			public Writer(MemoryStream stream = null)
			{
				this._stream = stream ?? new MemoryStream();
				this._writer = new BinaryWriter(this._stream);
			}

			public void Encode(object obj)
			{
				if (obj == null)
				{
					HLog.LogError("Obj is null, encode return.");
					return;
				}
				if (obj is bool)
				{
					bool flag = (bool)obj;
					this.Write(flag);
					return;
				}
				if (obj is sbyte)
				{
					sbyte b = (sbyte)obj;
					this.Write(b);
					return;
				}
				if (obj is byte)
				{
					byte b2 = (byte)obj;
					this.Write(b2);
					return;
				}
				if (obj is char)
				{
					char c = (char)obj;
					this.Write(c);
					return;
				}
				if (obj is short)
				{
					short num = (short)obj;
					this.Write(num);
					return;
				}
				if (obj is ushort)
				{
					ushort num2 = (ushort)obj;
					this.Write(num2);
					return;
				}
				if (obj is int)
				{
					int num3 = (int)obj;
					this.Write(num3);
					return;
				}
				if (obj is uint)
				{
					uint num4 = (uint)obj;
					this.Write(num4);
					return;
				}
				if (obj is long)
				{
					long num5 = (long)obj;
					this.Write(num5);
					return;
				}
				if (obj is ulong)
				{
					ulong num6 = (ulong)obj;
					this.Write(num6);
					return;
				}
				if (obj is decimal)
				{
					decimal num7 = (decimal)obj;
					this.Write(num7);
					return;
				}
				if (obj is float)
				{
					float num8 = (float)obj;
					this.Write(num8);
					return;
				}
				if (obj is double)
				{
					double num9 = (double)obj;
					this.Write(num9);
					return;
				}
				if (obj is DateTime)
				{
					DateTime dateTime = (DateTime)obj;
					this.Write(dateTime);
					return;
				}
				if (obj is Enum)
				{
					this.WriteEnum(obj);
					return;
				}
				string text = obj as string;
				if (text != null)
				{
					this.Write(text);
					return;
				}
				IList list = obj as IList;
				if (list != null)
				{
					this.Write(list);
					return;
				}
				IDictionary dictionary = obj as IDictionary;
				if (dictionary != null)
				{
					this.Write(dictionary);
					return;
				}
				if (obj is Vector2)
				{
					Vector2 vector = (Vector2)obj;
					this.Write(vector);
					return;
				}
				if (obj is Vector3)
				{
					Vector3 vector2 = (Vector3)obj;
					this.Write(vector2);
					return;
				}
				if (obj is Vector4)
				{
					Vector4 vector3 = (Vector4)obj;
					this.Write(vector3);
					return;
				}
				if (obj is Color)
				{
					Color color = (Color)obj;
					this.Write(color);
					return;
				}
				if (obj is Color32)
				{
					Color32 color2 = (Color32)obj;
					this.Write(color2);
					return;
				}
				if (obj is Quaternion)
				{
					Quaternion quaternion = (Quaternion)obj;
					this.Write(quaternion);
					return;
				}
				if (obj is Bounds)
				{
					Bounds bounds = (Bounds)obj;
					this.Write(bounds);
					return;
				}
				if (obj is Rect)
				{
					Rect rect = (Rect)obj;
					this.Write(rect);
					return;
				}
				if (obj is Matrix4x4)
				{
					Matrix4x4 matrix4x = (Matrix4x4)obj;
					this.Write(matrix4x);
					return;
				}
				this.Write(obj);
			}

			private void Write(bool value)
			{
				this._writer.Write(1);
				this._writer.Write(value);
			}

			private void Write(sbyte value)
			{
				this._writer.Write(2);
				this._writer.Write(value);
			}

			private void Write(byte value)
			{
				this._writer.Write(3);
				this._writer.Write(value);
			}

			private void Write(char value)
			{
				this._writer.Write(4);
				this._writer.Write(value);
			}

			private void Write(short value)
			{
				this._writer.Write(5);
				this._writer.Write(value);
			}

			private void Write(ushort value)
			{
				this._writer.Write(6);
				this._writer.Write(value);
			}

			private void Write(int value)
			{
				this._writer.Write(7);
				this._writer.Write(value);
			}

			private void Write(uint value)
			{
				this._writer.Write(8);
				this._writer.Write(value);
			}

			private void Write(long value)
			{
				this._writer.Write(9);
				this._writer.Write(value);
			}

			private void Write(ulong value)
			{
				this._writer.Write(10);
				this._writer.Write(value);
			}

			private void Write(decimal value)
			{
				this._writer.Write(11);
				this._writer.Write(value);
			}

			private void Write(float value)
			{
				this._writer.Write(12);
				this._writer.Write(value);
			}

			private void Write(double value)
			{
				this._writer.Write(13);
				this._writer.Write(value);
			}

			private void Write(DateTime value)
			{
				TimeSpan timeSpan = ((value.Kind == DateTimeKind.Local) ? (value - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()) : (value - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)));
				this._writer.Write(14);
				this._writer.Write((long)timeSpan.TotalSeconds * 1000L);
			}

			private void WriteEnum(object value)
			{
				this._writer.Write(15);
				this._writer.Write((int)value);
			}

			private void Write(string value)
			{
				this._writer.Write(16);
				this._writer.Write(value);
			}

			private void Write(IList value)
			{
				using (Bson.Writer writer = new Bson.Writer(null))
				{
					foreach (object obj in value)
					{
						if (obj != null)
						{
							writer.Encode(obj);
						}
					}
					int num = (int)writer._stream.Length;
					this._writer.Write(17);
					this._writer.Write(num);
					this._writer.Write(writer._stream.GetBuffer(), 0, num);
				}
			}

			private void Write(IDictionary value)
			{
				using (Bson.Writer writer = new Bson.Writer(null))
				{
					foreach (object obj in value)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						if (dictionaryEntry.Value != null)
						{
							writer._writer.Write(dictionaryEntry.Key.ToString());
							writer.Encode(dictionaryEntry.Value);
						}
					}
					int num = (int)writer._stream.Length;
					this._writer.Write(27);
					this._writer.Write(num);
					this._writer.Write(writer._stream.GetBuffer(), 0, num);
				}
			}

			private void Write(Vector2 value)
			{
				this._writer.Write(18);
				this._writer.Write(value.x);
				this._writer.Write(value.y);
			}

			private void Write(Vector3 value)
			{
				this._writer.Write(19);
				this._writer.Write(value.x);
				this._writer.Write(value.y);
				this._writer.Write(value.z);
			}

			private void Write(Vector4 value)
			{
				this._writer.Write(20);
				this._writer.Write(value.x);
				this._writer.Write(value.y);
				this._writer.Write(value.z);
				this._writer.Write(value.w);
			}

			private void Write(Color value)
			{
				this._writer.Write(21);
				this._writer.Write(value.r);
				this._writer.Write(value.g);
				this._writer.Write(value.b);
				this._writer.Write(value.a);
			}

			private void Write(Color32 value)
			{
				this._writer.Write(22);
				this._writer.Write(value.r);
				this._writer.Write(value.g);
				this._writer.Write(value.b);
				this._writer.Write(value.a);
			}

			private void Write(Quaternion value)
			{
				this._writer.Write(23);
				this._writer.Write(value.x);
				this._writer.Write(value.y);
				this._writer.Write(value.z);
				this._writer.Write(value.w);
			}

			private void Write(Bounds value)
			{
				this._writer.Write(24);
				this._writer.Write(value.center.x);
				this._writer.Write(value.center.y);
				this._writer.Write(value.center.z);
				this._writer.Write(value.extents.x);
				this._writer.Write(value.extents.y);
				this._writer.Write(value.extents.z);
			}

			private void Write(Rect value)
			{
				this._writer.Write(25);
				this._writer.Write(value.x);
				this._writer.Write(value.y);
				this._writer.Write(value.width);
				this._writer.Write(value.height);
			}

			private void Write(Matrix4x4 value)
			{
				this._writer.Write(26);
				this._writer.Write(value.m00);
				this._writer.Write(value.m10);
				this._writer.Write(value.m20);
				this._writer.Write(value.m30);
				this._writer.Write(value.m01);
				this._writer.Write(value.m11);
				this._writer.Write(value.m21);
				this._writer.Write(value.m31);
				this._writer.Write(value.m02);
				this._writer.Write(value.m12);
				this._writer.Write(value.m22);
				this._writer.Write(value.m32);
				this._writer.Write(value.m03);
				this._writer.Write(value.m13);
				this._writer.Write(value.m23);
				this._writer.Write(value.m33);
			}

			private void Write(object value)
			{
				using (Bson.Writer writer = new Bson.Writer(null))
				{
					foreach (Bson.PropertyMeta propertyMeta in Bson.AddPropertyMetas(value.GetType()))
					{
						if (propertyMeta.IsField)
						{
							object value2 = ((FieldInfo)propertyMeta.Info).GetValue(value);
							if (value2 != null)
							{
								writer._writer.Write(propertyMeta.Info.Name);
								writer.Encode(value2);
							}
						}
						else
						{
							PropertyInfo propertyInfo = (PropertyInfo)propertyMeta.Info;
							if (propertyInfo.CanRead && propertyInfo.CanWrite)
							{
								object value3 = propertyInfo.GetValue(value, null);
								if (value3 != null)
								{
									writer._writer.Write(propertyMeta.Info.Name);
									writer.Encode(value3);
								}
							}
						}
					}
					int num = (int)writer._stream.Length;
					this._writer.Write(27);
					this._writer.Write(num);
					this._writer.Write(writer._stream.GetBuffer(), 0, num);
				}
			}

			public void Dispose()
			{
				if (this._isDisposed)
				{
					return;
				}
				BinaryWriter writer = this._writer;
				if (writer != null)
				{
					writer.Close();
				}
				MemoryStream stream = this._stream;
				if (stream != null)
				{
					stream.Dispose();
				}
				this._isDisposed = true;
			}

			private readonly MemoryStream _stream;

			private readonly BinaryWriter _writer;

			private bool _isDisposed;
		}
	}
}
