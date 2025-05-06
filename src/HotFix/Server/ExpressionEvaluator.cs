using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Server
{
	public class ExpressionEvaluator
	{
		public static bool Evaluate(string expression, out FP value)
		{
			ExpressionEvaluator.Expression expression2;
			return ExpressionEvaluator.Evaluate(expression, out value, out expression2);
		}

		public static bool Evaluate(string expression, out FP value, out ExpressionEvaluator.Expression delayed)
		{
			value = default(FP);
			delayed = null;
			if (ExpressionEvaluator.TryParse(expression, out value))
			{
				return true;
			}
			ExpressionEvaluator.Expression expression2 = new ExpressionEvaluator.Expression(expression);
			if (expression2.hasVariables)
			{
				value = default(FP);
				delayed = expression2;
				return false;
			}
			return ExpressionEvaluator.EvaluateTokens(expression2.rpnTokens, ref value, 0, 1);
		}

		internal static void SetRandomState(uint state)
		{
			ExpressionEvaluator.s_Random = new ExpressionEvaluator.PcgRandom((ulong)state, 0UL);
		}

		private static bool EvaluateTokens(string[] tokens, ref FP value, int index, int count)
		{
			FP fp = value;
			bool flag = ExpressionEvaluator.EvaluateFP(tokens, ref fp, index, count);
			value = fp;
			return flag;
		}

		private static bool EvaluateFP(string[] tokens, ref FP value, int index, int count)
		{
			Stack<string> stack = new Stack<string>();
			foreach (string text in tokens)
			{
				if (ExpressionEvaluator.IsOperator(text))
				{
					ExpressionEvaluator.Operator @operator = ExpressionEvaluator.TokenToOperator(text);
					List<FP> list = new List<FP>();
					bool flag = true;
					while (stack.Count > 0 && !ExpressionEvaluator.IsCommand(stack.Peek()) && list.Count < @operator.inputs)
					{
						FP fp;
						flag &= ExpressionEvaluator.TryParse(stack.Pop(), out fp);
						list.Add(fp);
					}
					list.Reverse();
					if (!flag || list.Count != @operator.inputs)
					{
						return false;
					}
					stack.Push("fp" + ExpressionEvaluator.EvaluateOp(list.ToArray(), @operator.op, index, count).ToString());
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
			return stack.Count == 1 && ExpressionEvaluator.TryParse(stack.Pop(), out value);
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

		private static FP EvaluateOp(FP[] values, ExpressionEvaluator.Op op, int index, int count)
		{
			FP fp = ((values.Length >= 1) ? values[0] : 0);
			FP fp2 = ((values.Length >= 2) ? values[1] : 0);
			switch (op)
			{
			case ExpressionEvaluator.Op.Add:
				return fp + fp2;
			case ExpressionEvaluator.Op.Sub:
				return fp - fp2;
			case ExpressionEvaluator.Op.Mul:
				return fp * fp2;
			case ExpressionEvaluator.Op.Div:
				return fp / fp2;
			case ExpressionEvaluator.Op.Mod:
				return fp % fp2;
			case ExpressionEvaluator.Op.Neg:
				return -fp;
			case ExpressionEvaluator.Op.Pow:
				return FP.Pow(fp, (int)fp2);
			case ExpressionEvaluator.Op.Sqrt:
				if (!(fp <= 0))
				{
					return FPMath.Sqrt(fp);
				}
				return 0;
			case ExpressionEvaluator.Op.Abs:
				return FPMath.Abs(fp);
			case ExpressionEvaluator.Op.Sin:
				return FPMath.Sin(fp);
			case ExpressionEvaluator.Op.Cos:
				return FPMath.Cos(fp);
			case ExpressionEvaluator.Op.Tan:
				return FPMath.Tan(fp);
			case ExpressionEvaluator.Op.Floor:
				return FPMath.Floor(fp);
			case ExpressionEvaluator.Op.Ceil:
				return FPMath.Ceiling(fp);
			case ExpressionEvaluator.Op.Round:
				return FPMath.Round(fp);
			case ExpressionEvaluator.Op.Min:
				return FPMath.Min(fp, fp2);
			case ExpressionEvaluator.Op.Max:
				return FPMath.Max(fp, fp2);
			case ExpressionEvaluator.Op.Rand:
			{
				double num = (ExpressionEvaluator.s_Random.GetUInt() & 16777215U) / 16777215.0;
				return fp + num * (fp2 - fp);
			}
			case ExpressionEvaluator.Op.Linear:
			{
				if (count < 1)
				{
					count = 1;
				}
				double num2 = ((count < 2) ? 0.5 : ((double)index / (double)(count - 1)));
				return fp + num2 * (fp2 - fp);
			}
			default:
				return 0;
			}
		}

		private static bool TryParse(string expression, out FP result)
		{
			expression = expression.Replace(',', '.');
			string text = expression.ToLowerInvariant();
			if (text.Length > 1 && char.IsDigit(text[text.Length - 2]))
			{
				char[] array = new char[] { 'f', 'd', 'l' };
				text = text.TrimEnd(array);
			}
			result = default(FP);
			if (text.Length == 0)
			{
				return true;
			}
			bool flag;
			if (text.StartsWith("fp"))
			{
				FP fp;
				if (FP.TryFromString(text.Substring(2, text.Length - 2), out fp))
				{
					result = fp;
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			else if (text == "pi")
			{
				flag = true;
				result = FP.PI;
			}
			else
			{
				double num;
				flag = double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out num);
				result = num;
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
}
