using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Google.Protobuf.Collections
{
	public sealed class MapField<TKey, TValue> : IDeepCloneable<MapField<TKey, TValue>>, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IEquatable<MapField<TKey, TValue>>, IDictionary, ICollection
	{
		public MapField<TKey, TValue> Clone()
		{
			MapField<TKey, TValue> mapField = new MapField<TKey, TValue>();
			if (typeof(IDeepCloneable<TValue>).IsAssignableFrom(typeof(TValue)))
			{
				using (LinkedList<KeyValuePair<TKey, TValue>>.Enumerator enumerator = this.list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<TKey, TValue> keyValuePair = enumerator.Current;
						mapField.Add(keyValuePair.Key, ((IDeepCloneable<TValue>)((object)keyValuePair.Value)).Clone());
					}
					return mapField;
				}
			}
			mapField.Add(this);
			return mapField;
		}

		public void Add(TKey key, TValue value)
		{
			if (this.ContainsKey(key))
			{
				throw new ArgumentException("Key already exists in map", "key");
			}
			this[key] = value;
		}

		public bool ContainsKey(TKey key)
		{
			ProtoPreconditions.CheckNotNullUnconstrained<TKey>(key, "key");
			return this.map.ContainsKey(key);
		}

		private bool ContainsValue(TValue value)
		{
			EqualityComparer<TValue> comparer = EqualityComparer<TValue>.Default;
			return this.list.Any((KeyValuePair<TKey, TValue> pair) => comparer.Equals(pair.Value, value));
		}

		public bool Remove(TKey key)
		{
			ProtoPreconditions.CheckNotNullUnconstrained<TKey>(key, "key");
			LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
			if (this.map.TryGetValue(key, out linkedListNode))
			{
				this.map.Remove(key);
				linkedListNode.List.Remove(linkedListNode);
				return true;
			}
			return false;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
			if (this.map.TryGetValue(key, out linkedListNode))
			{
				value = linkedListNode.Value.Value;
				return true;
			}
			value = default(TValue);
			return false;
		}

		public TValue this[TKey key]
		{
			get
			{
				ProtoPreconditions.CheckNotNullUnconstrained<TKey>(key, "key");
				TValue tvalue;
				if (this.TryGetValue(key, out tvalue))
				{
					return tvalue;
				}
				throw new KeyNotFoundException();
			}
			set
			{
				ProtoPreconditions.CheckNotNullUnconstrained<TKey>(key, "key");
				if (value == null)
				{
					ProtoPreconditions.CheckNotNullUnconstrained<TValue>(value, "value");
				}
				KeyValuePair<TKey, TValue> keyValuePair = new KeyValuePair<TKey, TValue>(key, value);
				LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
				if (this.map.TryGetValue(key, out linkedListNode))
				{
					linkedListNode.Value = keyValuePair;
					return;
				}
				linkedListNode = this.list.AddLast(keyValuePair);
				this.map[key] = linkedListNode;
			}
		}

		public ICollection<TKey> Keys
		{
			get
			{
				return new MapField<TKey, TValue>.MapView<TKey>(this, (KeyValuePair<TKey, TValue> pair) => pair.Key, new Func<TKey, bool>(this.ContainsKey));
			}
		}

		public ICollection<TValue> Values
		{
			get
			{
				return new MapField<TKey, TValue>.MapView<TValue>(this, (KeyValuePair<TKey, TValue> pair) => pair.Value, new Func<TValue, bool>(this.ContainsValue));
			}
		}

		public void Add(IDictionary<TKey, TValue> entries)
		{
			ProtoPreconditions.CheckNotNull<IDictionary<TKey, TValue>>(entries, "entries");
			foreach (KeyValuePair<TKey, TValue> keyValuePair in entries)
			{
				this.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		public void Clear()
		{
			this.list.Clear();
			this.map.Clear();
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			TValue tvalue;
			return this.TryGetValue(item.Key, out tvalue) && EqualityComparer<TValue>.Default.Equals(item.Value, tvalue);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			if (item.Key == null)
			{
				throw new ArgumentException("Key is null", "item");
			}
			LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode;
			if (this.map.TryGetValue(item.Key, out linkedListNode) && EqualityComparer<TValue>.Default.Equals(item.Value, linkedListNode.Value.Value))
			{
				this.map.Remove(item.Key);
				linkedListNode.List.Remove(linkedListNode);
				return true;
			}
			return false;
		}

		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public override bool Equals(object other)
		{
			return this.Equals(other as MapField<TKey, TValue>);
		}

		public override int GetHashCode()
		{
			EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
			int num = 0;
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this.list)
			{
				int num2 = num;
				TKey key = keyValuePair.Key;
				num = num2 ^ (key.GetHashCode() * 31 + @default.GetHashCode(keyValuePair.Value));
			}
			return num;
		}

		public bool Equals(MapField<TKey, TValue> other)
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
			EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
			{
				TValue tvalue;
				if (!other.TryGetValue(keyValuePair.Key, out tvalue))
				{
					return false;
				}
				if (!@default.Equals(tvalue, keyValuePair.Value))
				{
					return false;
				}
			}
			return true;
		}

		public void AddEntriesFrom(CodedInputStream input, MapField<TKey, TValue>.Codec codec)
		{
			MapField<TKey, TValue>.Codec.MessageAdapter messageAdapter = new MapField<TKey, TValue>.Codec.MessageAdapter(codec);
			do
			{
				messageAdapter.Reset();
				input.ReadMessage(messageAdapter);
				this[messageAdapter.Key] = messageAdapter.Value;
			}
			while (input.MaybeConsumeTag(codec.MapTag));
		}

		public void WriteTo(CodedOutputStream output, MapField<TKey, TValue>.Codec codec)
		{
			MapField<TKey, TValue>.Codec.MessageAdapter messageAdapter = new MapField<TKey, TValue>.Codec.MessageAdapter(codec);
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this.list)
			{
				messageAdapter.Key = keyValuePair.Key;
				messageAdapter.Value = keyValuePair.Value;
				output.WriteTag(codec.MapTag);
				output.WriteMessage(messageAdapter);
			}
		}

		public int CalculateSize(MapField<TKey, TValue>.Codec codec)
		{
			if (this.Count == 0)
			{
				return 0;
			}
			MapField<TKey, TValue>.Codec.MessageAdapter messageAdapter = new MapField<TKey, TValue>.Codec.MessageAdapter(codec);
			int num = 0;
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this.list)
			{
				messageAdapter.Key = keyValuePair.Key;
				messageAdapter.Value = keyValuePair.Value;
				num += CodedOutputStream.ComputeRawVarint32Size(codec.MapTag);
				num += CodedOutputStream.ComputeMessageSize(messageAdapter);
			}
			return num;
		}

		void IDictionary.Add(object key, object value)
		{
			this.Add((TKey)((object)key), (TValue)((object)value));
		}

		bool IDictionary.Contains(object key)
		{
			return key is TKey && this.ContainsKey((TKey)((object)key));
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new MapField<TKey, TValue>.DictionaryEnumerator(this.GetEnumerator());
		}

		void IDictionary.Remove(object key)
		{
			ProtoPreconditions.CheckNotNull<object>(key, "key");
			if (!(key is TKey))
			{
				return;
			}
			this.Remove((TKey)((object)key));
		}

		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)this.Select((KeyValuePair<TKey, TValue> pair) => new DictionaryEntry(pair.Key, pair.Value)).ToList<DictionaryEntry>()).CopyTo(array, index);
		}

		bool IDictionary.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		ICollection IDictionary.Keys
		{
			get
			{
				return (ICollection)this.Keys;
			}
		}

		ICollection IDictionary.Values
		{
			get
			{
				return (ICollection)this.Values;
			}
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

		object IDictionary.this[object key]
		{
			get
			{
				ProtoPreconditions.CheckNotNull<object>(key, "key");
				if (!(key is TKey))
				{
					return null;
				}
				TValue tvalue;
				this.TryGetValue((TKey)((object)key), out tvalue);
				return tvalue;
			}
			set
			{
				this[(TKey)((object)key)] = (TValue)((object)value);
			}
		}

		private readonly Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> map = new Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>>();

		private readonly LinkedList<KeyValuePair<TKey, TValue>> list = new LinkedList<KeyValuePair<TKey, TValue>>();

		private class DictionaryEnumerator : IDictionaryEnumerator, IEnumerator
		{
			internal DictionaryEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> enumerator)
			{
				this.enumerator = enumerator;
			}

			public bool MoveNext()
			{
				return this.enumerator.MoveNext();
			}

			public void Reset()
			{
				this.enumerator.Reset();
			}

			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			public DictionaryEntry Entry
			{
				get
				{
					return new DictionaryEntry(this.Key, this.Value);
				}
			}

			public object Key
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this.enumerator.Current;
					return keyValuePair.Key;
				}
			}

			public object Value
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this.enumerator.Current;
					return keyValuePair.Value;
				}
			}

			private readonly IEnumerator<KeyValuePair<TKey, TValue>> enumerator;
		}

		public sealed class Codec
		{
			public Codec(FieldCodec<TKey> keyCodec, FieldCodec<TValue> valueCodec, uint mapTag)
			{
				this.keyCodec = keyCodec;
				this.valueCodec = valueCodec;
				this.mapTag = mapTag;
			}

			internal uint MapTag
			{
				get
				{
					return this.mapTag;
				}
			}

			private readonly FieldCodec<TKey> keyCodec;

			private readonly FieldCodec<TValue> valueCodec;

			private readonly uint mapTag;

			internal class MessageAdapter : IMessage
			{
				internal TKey Key { get; set; }

				internal TValue Value { get; set; }

				internal MessageAdapter(MapField<TKey, TValue>.Codec codec)
				{
					this.codec = codec;
				}

				internal void Reset()
				{
					this.Key = this.codec.keyCodec.DefaultValue;
					this.Value = this.codec.valueCodec.DefaultValue;
				}

				public void MergeFrom(CodedInputStream input)
				{
					uint num;
					while ((num = input.ReadTag()) != 0U)
					{
						if (num == this.codec.keyCodec.Tag)
						{
							this.Key = this.codec.keyCodec.Read(input);
						}
						else if (num == this.codec.valueCodec.Tag)
						{
							this.Value = this.codec.valueCodec.Read(input);
						}
						else
						{
							input.SkipLastField();
						}
					}
					if (this.Value == null)
					{
						this.Value = this.codec.valueCodec.Read(new CodedInputStream(MapField<TKey, TValue>.Codec.MessageAdapter.ZeroLengthMessageStreamData));
					}
				}

				public void WriteTo(CodedOutputStream output)
				{
					this.codec.keyCodec.WriteTagAndValue(output, this.Key);
					this.codec.valueCodec.WriteTagAndValue(output, this.Value);
				}

				public int CalculateSize()
				{
					return this.codec.keyCodec.CalculateSizeWithTag(this.Key) + this.codec.valueCodec.CalculateSizeWithTag(this.Value);
				}

				private static readonly byte[] ZeroLengthMessageStreamData = new byte[1];

				private readonly MapField<TKey, TValue>.Codec codec;
			}
		}

		private class MapView<T> : ICollection<T>, IEnumerable<T>, IEnumerable, ICollection
		{
			internal MapView(MapField<TKey, TValue> parent, Func<KeyValuePair<TKey, TValue>, T> projection, Func<T, bool> containsCheck)
			{
				this.parent = parent;
				this.projection = projection;
				this.containsCheck = containsCheck;
			}

			public int Count
			{
				get
				{
					return this.parent.Count;
				}
			}

			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			public bool IsSynchronized
			{
				get
				{
					return false;
				}
			}

			public object SyncRoot
			{
				get
				{
					return this.parent;
				}
			}

			public void Add(T item)
			{
				throw new NotSupportedException();
			}

			public void Clear()
			{
				throw new NotSupportedException();
			}

			public bool Contains(T item)
			{
				return this.containsCheck(item);
			}

			public void CopyTo(T[] array, int arrayIndex)
			{
				if (arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException("arrayIndex");
				}
				if (arrayIndex + this.Count > array.Length)
				{
					throw new ArgumentException("Not enough space in the array", "array");
				}
				foreach (T t in this)
				{
					array[arrayIndex++] = t;
				}
			}

			public IEnumerator<T> GetEnumerator()
			{
				return this.parent.list.Select(this.projection).GetEnumerator();
			}

			public bool Remove(T item)
			{
				throw new NotSupportedException();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			public void CopyTo(Array array, int index)
			{
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (index + this.Count > array.Length)
				{
					throw new ArgumentException("Not enough space in the array", "array");
				}
				foreach (T t in this)
				{
					array.SetValue(t, index++);
				}
			}

			private readonly MapField<TKey, TValue> parent;

			private readonly Func<KeyValuePair<TKey, TValue>, T> projection;

			private readonly Func<T, bool> containsCheck;
		}
	}
}
