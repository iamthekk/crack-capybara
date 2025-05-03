using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class ExpressionEvaluator
{
	public static bool Evaluate<T>(string expression, out T value)
	{
		ExpressionEvaluator.Expression expression2;
		return ExpressionEvaluator.Evaluate<T>(expression, out value, out expression2);
	}

	public static bool Evaluate<T>(string expression, out T value, out ExpressionEvaluator.Expression delayed)
	{
		value = default(T);
		delayed = null;
		if (ExpressionEvaluator.TryParse<T>(expression, out value))
		{
			return true;
		}
		ExpressionEvaluator.Expression expression2 = new ExpressionEvaluator.Expression(expression);
		if (expression2.hasVariables)
		{
			value = default(T);
			delayed = expression2;
			return false;
		}
		return ExpressionEvaluator.EvaluateTokens<T>(expression2.rpnTokens, ref value, 0, 1);
	}

	internal static void SetRandomState(uint state)
	{
		ExpressionEvaluator.s_Random = new ExpressionEvaluator.PcgRandom((ulong)state, 0UL);
	}

	private static bool EvaluateTokens<T>(string[] tokens, ref T value, int index, int count)
	{
		bool flag = false;
		if (typeof(T) == typeof(float))
		{
			double num = (double)((float)((object)value));
			flag = ExpressionEvaluator.EvaluateDouble(tokens, ref num, index, count);
			value = (T)((object)((float)num));
		}
		else if (typeof(T) == typeof(int))
		{
			double num2 = (double)((int)((object)value));
			flag = ExpressionEvaluator.EvaluateDouble(tokens, ref num2, index, count);
			value = (T)((object)((int)num2));
		}
		else if (typeof(T) == typeof(long))
		{
			double num3 = (double)((long)((object)value));
			flag = ExpressionEvaluator.EvaluateDouble(tokens, ref num3, index, count);
			value = (T)((object)((long)num3));
		}
		else if (typeof(T) == typeof(double))
		{
			double num4 = (double)((object)value);
			flag = ExpressionEvaluator.EvaluateDouble(tokens, ref num4, index, count);
			value = (T)((object)num4);
		}
		return flag;
	}

	private static bool EvaluateDouble(string[] tokens, ref double value, int index, int count)
	{
		Stack<string> stack = new Stack<string>();
		foreach (string text in tokens)
		{
			if (ExpressionEvaluator.IsOperator(text))
			{
				ExpressionEvaluator.Operator @operator = ExpressionEvaluator.TokenToOperator(text);
				List<double> list = new List<double>();
				bool flag = true;
				while (stack.Count > 0 && !ExpressionEvaluator.IsCommand(stack.Peek()) && list.Count < @operator.inputs)
				{
					double num;
					flag &= ExpressionEvaluator.TryParse<double>(stack.Pop(), out num);
					list.Add(num);
				}
				list.Reverse();
				if (!flag || list.Count != @operator.inputs)
				{
					return false;
				}
				stack.Push(ExpressionEvaluator.EvaluateOp(list.ToArray(), @operator.op, index, count).ToString(CultureInfo.InvariantCulture));
			}
			else if (ExpressionEvaluator.IsVariable(text))
			{
				stack.Push((text == "#") ? index.ToString() : value.ToString(CultureInfo.InvariantCulture));
			}
			else
			{
				stack.Push(text);
			}
		}
		return stack.Count == 1 && ExpressionEvaluator.TryParse<double>(stack.Pop(), out value);
	}

	private static string[] InfixToRPN(string[] tokens)
	{
		Stack<string> stack = new Stack<string>();
		Queue<string> queue = new Queue<string>();
		foreach (string text in tokens)
		{
			if (ExpressionEvaluator.IsCommand(text))
			{
				char c = text[0];
				if (c == '(')
				{
					stack.Push(text);
				}
				else if (c == ')')
				{
					while (stack.Count > 0 && stack.Peek() != "(")
					{
						queue.Enqueue(stack.Pop());
					}
					if (stack.Count > 0)
					{
						stack.Pop();
					}
					if (stack.Count > 0 && ExpressionEvaluator.IsDelayedFunction(stack.Peek()))
					{
						queue.Enqueue(stack.Pop());
					}
				}
				else if (c == ',')
				{
					while (stack.Count > 0)
					{
						if (!(stack.Peek() != "("))
						{
							break;
						}
						queue.Enqueue(stack.Pop());
					}
				}
				else
				{
					ExpressionEvaluator.Operator @operator = ExpressionEvaluator.TokenToOperator(text);
					while (ExpressionEvaluator.NeedToPop(stack, @operator))
					{
						queue.Enqueue(stack.Pop());
					}
					stack.Push(text);
				}
			}
			else if (ExpressionEvaluator.IsDelayedFunction(text))
			{
				stack.Push(text);
			}
			else
			{
				queue.Enqueue(text);
			}
		}
		while (stack.Count > 0)
		{
			queue.Enqueue(stack.Pop());
		}
		return queue.ToArray();
	}

	private static bool NeedToPop(Stack<string> operatorStack, ExpressionEvaluator.Operator newOperator)
	{
		if (operatorStack.Count > 0 && newOperator != null)
		{
			ExpressionEvaluator.Operator @operator = ExpressionEvaluator.TokenToOperator(operatorStack.Peek());
			if (@operator != null && ((newOperator.associativity == ExpressionEvaluator.Associativity.Left && newOperator.precedence <= @operator.precedence) || (newOperator.associativity == ExpressionEvaluator.Associativity.Right && newOperator.precedence < @operator.precedence)))
			{
				return true;
			}
		}
		return false;
	}

	private static string[] ExpressionToTokens(string expression, out bool hasVariables)
	{
		hasVariables = false;
		List<string> list = new List<string>();
		string text = "";
		foreach (char c in expression)
		{
			if (ExpressionEvaluator.IsCommand(c.ToString()))
			{
				if (text.Length > 0)
				{
					list.Add(text);
				}
				list.Add(c.ToString());
				text = "";
			}
			else if (c != ' ')
			{
				text += c.ToString();
			}
			else
			{
				if (text.Length > 0)
				{
					list.Add(text);
				}
				text = "";
			}
		}
		if (text.Length > 0)
		{
			list.Add(text);
		}
		hasVariables = list.Any((string f) => ExpressionEvaluator.IsVariable(f) || ExpressionEvaluator.IsDelayedFunction(f));
		return list.ToArray();
	}

	private static bool IsCommand(string token)
	{
		if (token.Length == 1)
		{
			char c = token[0];
			if (c == '(' || c == ')' || c == ',')
			{
				return true;
			}
		}
		return ExpressionEvaluator.IsOperator(token);
	}

	private static bool IsVariable(string token)
	{
		if (token.Length == 1)
		{
			char c = token[0];
			return c == 'x' || c == 'v' || c == 'f' || c == '#';
		}
		return false;
	}

	private static bool IsDelayedFunction(string token)
	{
		ExpressionEvaluator.Operator @operator = ExpressionEvaluator.TokenToOperator(token);
		return @operator != null && (@operator.op == ExpressionEvaluator.Op.Rand || @operator.op == ExpressionEvaluator.Op.Linear);
	}

	private static bool IsOperator(string token)
	{
		return ExpressionEvaluator.s_Operators.ContainsKey(token);
	}

	private static ExpressionEvaluator.Operator TokenToOperator(string token)
	{
		ExpressionEvaluator.Operator @operator;
		if (!ExpressionEvaluator.s_Operators.TryGetValue(token, out @operator))
		{
			return null;
		}
		return @operator;
	}

	private static string PreFormatExpression(string expression)
	{
		string text = expression.Trim();
		if (text.Length == 0)
		{
			return text;
		}
		char c = text[text.Length - 1];
		if (ExpressionEvaluator.IsOperator(c.ToString()))
		{
			text = text.TrimEnd(c);
		}
		if (text.Length >= 2 && text[1] == '=')
		{
			char c2 = text[0];
			string text2 = text.Substring(2);
			if (c2 == '+')
			{
				text = "x+(" + text2 + ")";
			}
			if (c2 == '-')
			{
				text = "x-(" + text2 + ")";
			}
			if (c2 == '*')
			{
				text = "x*(" + text2 + ")";
			}
			if (c2 == '/')
			{
				text = "x/(" + text2 + ")";
			}
		}
		return text;
	}

	private static string[] FixUnaryOperators(string[] tokens)
	{
		if (tokens.Length == 0)
		{
			return tokens;
		}
		if (tokens[0] == "-")
		{
			tokens[0] = "_";
		}
		for (int i = 1; i < tokens.Length - 1; i++)
		{
			string text = tokens[i];
			string text2 = tokens[i - 1];
			if (text == "-" && ExpressionEvaluator.IsCommand(text2) && text2 != ")")
			{
				tokens[i] = "_";
			}
		}
		return tokens;
	}

	private static double EvaluateOp(double[] values, ExpressionEvaluator.Op op, int index, int count)
	{
		double num = ((values.Length >= 1) ? values[0] : 0.0);
		double num2 = ((values.Length >= 2) ? values[1] : 0.0);
		switch (op)
		{
		case ExpressionEvaluator.Op.Add:
			return num + num2;
		case ExpressionEvaluator.Op.Sub:
			return num - num2;
		case ExpressionEvaluator.Op.Mul:
			return num * num2;
		case ExpressionEvaluator.Op.Div:
			return num / num2;
		case ExpressionEvaluator.Op.Mod:
			return num % num2;
		case ExpressionEvaluator.Op.Neg:
			return -num;
		case ExpressionEvaluator.Op.Pow:
			return Math.Pow(num, num2);
		case ExpressionEvaluator.Op.Sqrt:
			if (num > 0.0)
			{
				return Math.Sqrt(num);
			}
			return 0.0;
		case ExpressionEvaluator.Op.Abs:
			return Math.Abs(num);
		case ExpressionEvaluator.Op.Sin:
			return Math.Sin(num);
		case ExpressionEvaluator.Op.Cos:
			return Math.Cos(num);
		case ExpressionEvaluator.Op.Tan:
			return Math.Tan(num);
		case ExpressionEvaluator.Op.Floor:
			return Math.Floor(num);
		case ExpressionEvaluator.Op.Ceil:
			return Math.Ceiling(num);
		case ExpressionEvaluator.Op.Round:
			return Math.Round(num);
		case ExpressionEvaluator.Op.Min:
			return Math.Min(num, num2);
		case ExpressionEvaluator.Op.Max:
			return Math.Max(num, num2);
		case ExpressionEvaluator.Op.Rand:
		{
			double num3 = (ExpressionEvaluator.s_Random.GetUInt() & 16777215U) / 16777215.0;
			return num + num3 * (num2 - num);
		}
		case ExpressionEvaluator.Op.Linear:
		{
			if (count < 1)
			{
				count = 1;
			}
			double num4 = ((count < 2) ? 0.5 : ((double)index / (double)(count - 1)));
			return num + num4 * (num2 - num);
		}
		default:
			return 0.0;
		}
	}

	private static bool TryParse<T>(string expression, out T result)
	{
		expression = expression.Replace(',', '.');
		string text = expression.ToLowerInvariant();
		if (text.Length > 1 && char.IsDigit(text[text.Length - 2]))
		{
			char[] array = new char[] { 'f', 'd', 'l' };
			text = text.TrimEnd(array);
		}
		bool flag = false;
		result = default(T);
		if (text.Length == 0)
		{
			return true;
		}
		if (typeof(T) == typeof(float))
		{
			if (text == "pi")
			{
				flag = true;
				result = (T)((object)3.14159274f);
			}
			else
			{
				float num;
				flag = float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out num);
				result = (T)((object)num);
			}
		}
		else if (typeof(T) == typeof(int))
		{
			int num2;
			flag = int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out num2);
			result = (T)((object)num2);
		}
		else if (typeof(T) == typeof(double))
		{
			if (text == "pi")
			{
				flag = true;
				result = (T)((object)3.1415926535897931);
			}
			else
			{
				double num3;
				flag = double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out num3);
				result = (T)((object)num3);
			}
		}
		else if (typeof(T) == typeof(long))
		{
			long num4;
			flag = long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out num4);
			result = (T)((object)num4);
		}
		return flag;
	}

	private static ExpressionEvaluator.PcgRandom s_Random = new ExpressionEvaluator.PcgRandom(0UL, 0UL);

	private static Dictionary<string, ExpressionEvaluator.Operator> s_Operators = new Dictionary<string, ExpressionEvaluator.Operator>
	{
		{
			"-",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Sub, 2, 2, ExpressionEvaluator.Associativity.Left)
		},
		{
			"+",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Add, 2, 2, ExpressionEvaluator.Associativity.Left)
		},
		{
			"/",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Div, 3, 2, ExpressionEvaluator.Associativity.Left)
		},
		{
			"*",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Mul, 3, 2, ExpressionEvaluator.Associativity.Left)
		},
		{
			"%",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Mod, 3, 2, ExpressionEvaluator.Associativity.Left)
		},
		{
			"^",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Pow, 5, 2, ExpressionEvaluator.Associativity.Right)
		},
		{
			"_",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Neg, 5, 1, ExpressionEvaluator.Associativity.Left)
		},
		{
			"sqrt",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Sqrt, 4, 1, ExpressionEvaluator.Associativity.Left)
		},
		{
			"abs",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Abs, 4, 1, ExpressionEvaluator.Associativity.Left)
		},
		{
			"cos",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Cos, 4, 1, ExpressionEvaluator.Associativity.Left)
		},
		{
			"sin",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Sin, 4, 1, ExpressionEvaluator.Associativity.Left)
		},
		{
			"tan",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Tan, 4, 1, ExpressionEvaluator.Associativity.Left)
		},
		{
			"floor",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Floor, 4, 1, ExpressionEvaluator.Associativity.Left)
		},
		{
			"ceil",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Ceil, 4, 1, ExpressionEvaluator.Associativity.Left)
		},
		{
			"round",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Round, 4, 1, ExpressionEvaluator.Associativity.Left)
		},
		{
			"min",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Min, 4, 2, ExpressionEvaluator.Associativity.Left)
		},
		{
			"max",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Max, 4, 2, ExpressionEvaluator.Associativity.Left)
		},
		{
			"R",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Rand, 4, 2, ExpressionEvaluator.Associativity.Left)
		},
		{
			"L",
			new ExpressionEvaluator.Operator(ExpressionEvaluator.Op.Linear, 4, 2, ExpressionEvaluator.Associativity.Left)
		}
	};

	public class Expression
	{
		internal Expression(string expression)
		{
			expression = ExpressionEvaluator.PreFormatExpression(expression);
			string[] array = ExpressionEvaluator.ExpressionToTokens(expression, out this.hasVariables);
			array = ExpressionEvaluator.FixUnaryOperators(array);
			this.rpnTokens = ExpressionEvaluator.InfixToRPN(array);
		}

		public bool Evaluate<T>(ref T value, int index = 0, int count = 1)
		{
			return ExpressionEvaluator.EvaluateTokens<T>(this.rpnTokens, ref value, index, count);
		}

		internal readonly string[] rpnTokens;

		internal readonly bool hasVariables;
	}

	private struct PcgRandom
	{
		public PcgRandom(ulong state = 0UL, ulong sequence = 0UL)
		{
			this.increment = (sequence << 1) | 1UL;
			this.state = 0UL;
			this.Step();
			this.state += state;
			this.Step();
		}

		public uint GetUInt()
		{
			ulong num = this.state;
			this.Step();
			return ExpressionEvaluator.PcgRandom.XshRr(num);
		}

		private static uint RotateRight(uint v, int rot)
		{
			return (v >> rot) | (v << -rot);
		}

		private static uint XshRr(ulong s)
		{
			return ExpressionEvaluator.PcgRandom.RotateRight((uint)(((s >> 18) ^ s) >> 27), (int)(s >> 59));
		}

		private void Step()
		{
			this.state = this.state * 6364136223846793005UL + this.increment;
		}

		private readonly ulong increment;

		private ulong state;

		private const ulong Multiplier64 = 6364136223846793005UL;
	}

	private enum Op
	{
		Add,
		Sub,
		Mul,
		Div,
		Mod,
		Neg,
		Pow,
		Sqrt,
		Abs,
		Sin,
		Cos,
		Tan,
		Floor,
		Ceil,
		Round,
		Min,
		Max,
		Rand,
		Linear
	}

	private enum Associativity
	{
		Left,
		Right
	}

	private class Operator
	{
		public Operator(ExpressionEvaluator.Op op, int precedence, int inputs, ExpressionEvaluator.Associativity associativity)
		{
			this.op = op;
			this.precedence = precedence;
			this.inputs = inputs;
			this.associativity = associativity;
		}

		public readonly ExpressionEvaluator.Op op;

		public readonly int precedence;

		public readonly ExpressionEvaluator.Associativity associativity;

		public readonly int inputs;
	}
}
