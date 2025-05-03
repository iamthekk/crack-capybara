using System;
using System.Collections;
using System.Collections.Generic;

namespace Google.Protobuf.Collections
{
	public sealed class RepeatedField<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection
	{
		public void AddEntriesFrom(CodedInputStream input, FieldCodec<T> codec)
		{
			uint lastTag = input.LastTag;
			Func<CodedInputStream, T> valueReader = codec.ValueReader;
			if (FieldCodec<T>.IsPackedRepeatedField(lastTag))
			{
				int num = input.ReadLength();
				if (num > 0)
				{
					int num2 = input.PushLimit(num);
					while (!input.ReachedLimit)
					{
						this.Add(valueReader(input));
					}
					input.PopLimit(num2);
					return;
				}
			}
			else
			{
				do
				{
					this.Add(valueReader(input));
				}
				while (input.MaybeConsumeTag(lastTag));
			}
		}

		public int CalculateSize(FieldCodec<T> codec)
		{
			if (this.count == 0)
			{
				return 0;
			}
			uint tag = codec.Tag;
			if (codec.PackedRepeatedField)
			{
				int num = this.CalculatePackedDataSize(codec);
				return CodedOutputStream.ComputeRawVarint32Size(tag) + CodedOutputStream.ComputeLengthSize(num) + num;
			}
			Func<T, int> valueSizeCalculator = codec.ValueSizeCalculator;
			int num2 = this.count * CodedOutputStream.ComputeRawVarint32Size(tag);
			for (int i = 0; i < this.count; i++)
			{
				num2 += valueSizeCalculator(this.array[i]);
			}
			return num2;
		}

		private int CalculatePackedDataSize(FieldCodec<T> codec)
		{
			int fixedSize = codec.FixedSize;
			if (fixedSize == 0)
			{
				Func<T, int> valueSizeCalculator = codec.ValueSizeCalculator;
				int num = 0;
				for (int i = 0; i < this.count; i++)
				{
					num += valueSizeCalculator(this.array[i]);
				}
				return num;
			}
			return fixedSize * this.Count;
		}

		public void WriteTo(CodedOutputStream output, FieldCodec<T> codec)
		{
			if (this.count == 0)
			{
				return;
			}
			Action<CodedOutputStream, T> valueWriter = codec.ValueWriter;
			uint tag = codec.Tag;
			if (codec.PackedRepeatedField)
			{
				uint num = (uint)this.CalculatePackedDataSize(codec);
				output.WriteTag(tag);
				output.WriteRawVarint32(num);
				for (int i = 0; i < this.count; i++)
				{
					valueWriter(output, this.array[i]);
				}
				return;
			}
			for (int j = 0; j < this.count; j++)
			{
				output.WriteTag(tag);
				valueWriter(output, this.array[j]);
			}
		}

		private void EnsureSize(int size)
		{
			if (this.array.Length < size)
			{
				size = Math.Max(size, 8);
				T[] array = new T[Math.Max(this.array.Length * 2, size)];
				Array.Copy(this.array, 0, array, 0, this.array.Length);
				this.array = array;
			}
		}

		public void Add(T item)
		{
			ProtoPreconditions.CheckNotNullUnconstrained<T>(item, "item");
			this.EnsureSize(this.count + 1);
			T[] array = this.array;
			int num = this.count;
			this.count = num + 1;
			array[num] = item;
		}

		public void Clear()
		{
			this.array = RepeatedField<T>.EmptyArray;
			this.count = 0;
		}

		public bool Contains(T item)
		{
			return this.IndexOf(item) != -1;
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(this.array, 0, array, arrayIndex, this.count);
		}

		public bool Remove(T item)
		{
			int num = this.IndexOf(item);
			if (num == -1)
			{
				return false;
			}
			Array.Copy(this.array, num + 1, this.array, num, this.count - num - 1);
			this.count--;
			this.array[this.count] = default(T);
			return true;
		}

		public int Count
		{
			get
			{
				return this.count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public void AddRange(IEnumerable<T> values)
		{
			ProtoPreconditions.CheckNotNull<IEnumerable<T>>(values, "values");
			RepeatedField<T> repeatedField = values as RepeatedField<T>;
			if (repeatedField != null)
			{
				this.EnsureSize(this.count + repeatedField.count);
				Array.Copy(repeatedField.array, 0, this.array, this.count, repeatedField.count);
				this.count += repeatedField.count;
				return;
			}
			ICollection collection = values as ICollection;
			if (collection != null)
			{
				int num = collection.Count;
				if (default(T) == null)
				{
					using (IEnumerator enumerator = collection.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current == null)
							{
								throw new ArgumentException("Sequence contained null element", "values");
							}
						}
					}
				}
				this.EnsureSize(this.count + num);
				collection.CopyTo(this.array, this.count);
				this.count += num;
				return;
			}
			foreach (T t in values)
			{
				this.Add(t);
			}
		}

		public void Add(IEnumerable<T> values)
		{
			this.AddRange(values);
		}

		public IEnumerator<T> GetEnumerator()
		{
			int num;
			for (int i = 0; i < this.count; i = num + 1)
			{
				yield return this.array[i];
				num = i;
			}
			yield break;
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as RepeatedField<T>);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public override int GetHashCode()
		{
			int num = 0;
			for (int i = 0; i < this.count; i++)
			{
				num = num * 31 + this.array[i].GetHashCode();
			}
			return num;
		}

		public bool Equals(RepeatedField<T> other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (other.Count != this.Count)
			{
				return false;
			}
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < this.count; i++)
			{
				if (!@default.Equals(this.array[i], other.array[i]))
				{
					return false;
				}
			}
			return true;
		}

		public int IndexOf(T item)
		{
			ProtoPreconditions.CheckNotNullUnconstrained<T>(item, "item");
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < this.count; i++)
			{
				if (@default.Equals(this.array[i], item))
				{
					return i;
				}
			}
			return -1;
		}

		public void Insert(int index, T item)
		{
			ProtoPreconditions.CheckNotNullUnconstrained<T>(item, "item");
			if (index < 0 || index > this.count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.EnsureSize(this.count + 1);
			Array.Copy(this.array, index, this.array, index + 1, this.count - index);
			this.array[index] = item;
			this.count++;
		}

		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			Array.Copy(this.array, index + 1, this.array, index, this.count - index - 1);
			this.count--;
			this.array[this.count] = default(T);
		}

		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= this.count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return this.array[index];
			}
			set
			{
				if (index < 0 || index >= this.count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				ProtoPreconditions.CheckNotNullUnconstrained<T>(value, "value");
				this.array[index] = value;
			}
		}

		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		void ICollection.CopyTo(Array array, int index)
		{
			Array.Copy(this.array, 0, array, index, this.count);
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (T)((object)value);
			}
		}

		int IList.Add(object value)
		{
			this.Add((T)((object)value));
			return this.count - 1;
		}

		bool IList.Contains(object value)
		{
			return value is T && this.Contains((T)((object)value));
		}

		int IList.IndexOf(object value)
		{
			if (!(value is T))
			{
				return -1;
			}
			return this.IndexOf((T)((object)value));
		}

		void IList.Insert(int index, object value)
		{
			this.Insert(index, (T)((object)value));
		}

		void IList.Remove(object value)
		{
			if (!(value is T))
			{
				return;
			}
			this.Remove((T)((object)value));
		}

		private static readonly T[] EmptyArray = new T[0];

		private const int MinArraySize = 8;

		private T[] array = RepeatedField<T>.EmptyArray;

		private int count;
	}
}
