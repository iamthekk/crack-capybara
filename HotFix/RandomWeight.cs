using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Server;

public class RandomWeight<T>
{
	public int Count
	{
		get
		{
			return this._all.Count;
		}
	}

	public RandomWeight(int seed = 1)
	{
		this._all = new List<ValueTuple<int, T>>();
		this._random = new XRandom(seed);
	}

	public void Remove(T value)
	{
		ValueTuple<int, T> valueTuple = this._all.FirstOrDefault(([TupleElementNames(new string[] { "weight", "value" })] ValueTuple<int, T> item) => object.Equals(value, item.Item2));
		this._totalWeight -= valueTuple.Item1;
		this._all.Remove(valueTuple);
	}

	public void Add(int weight, T value)
	{
		this._totalWeight += weight;
		this._all.Add(new ValueTuple<int, T>(weight, value));
	}

	public T Peek()
	{
		return this.GetRandom().Item2;
	}

	public T[] Peek(int count)
	{
		T[] array = new T[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = this.Peek();
		}
		return array;
	}

	public T Dequeue()
	{
		ValueTuple<int, T> random = this.GetRandom();
		this._all.Remove(random);
		this._totalWeight -= random.Item1;
		return random.Item2;
	}

	public T[] Dequeue(int count)
	{
		T[] array = new T[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = this.Dequeue();
		}
		return array;
	}

	[return: TupleElementNames(new string[] { "weight", "value" })]
	private ValueTuple<int, T> GetRandom()
	{
		int num = this._random.Range(0, this._totalWeight) + 1;
		for (int i = 0; i < this._all.Count; i++)
		{
			ValueTuple<int, T> valueTuple = this._all[i];
			num -= valueTuple.Item1;
			if (num <= 0)
			{
				return valueTuple;
			}
		}
		return default(ValueTuple<int, T>);
	}

	public void Clear()
	{
		this._all.Clear();
		this._totalWeight = 0;
	}

	private XRandom _random;

	private int _totalWeight;

	[TupleElementNames(new string[] { "weight", "value" })]
	private readonly List<ValueTuple<int, T>> _all;
}
