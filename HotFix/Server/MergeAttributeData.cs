using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Server
{
	public class MergeAttributeData
	{
		public string Header
		{
			get
			{
				return this.m_header;
			}
		}

		public string Symol
		{
			get
			{
				return this.m_symol;
			}
		}

		public string Formula
		{
			get
			{
				return this.m_formula;
			}
		}

		public FP Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = value;
			}
		}

		private MergeAttributeData()
		{
		}

		public MergeAttributeData(string info, string[] argsName = null, string[] args = null)
		{
			List<string> list = ((argsName != null) ? new List<string>(argsName) : null);
			List<string> list2 = ((args != null) ? new List<string>(args) : null);
			this.Merge(info, list, list2);
		}

		public MergeAttributeData(string info, List<string> argsName, List<string> args)
		{
			this.Merge(info, argsName, args);
		}

		private void Merge(string info, List<string> argsName, List<string> args)
		{
			this.Split(info);
			if (string.IsNullOrEmpty(this.m_header) || string.IsNullOrEmpty(this.m_symol) || string.IsNullOrEmpty(this.m_formula))
			{
				HLog.LogError("MergeAttributeData is error");
				return;
			}
			string text = this.m_formula;
			if (argsName != null && args != null && argsName.Count == args.Count)
			{
				text = string.Copy(this.m_formula);
				for (int i = 0; i < argsName.Count; i++)
				{
					string text2 = argsName[i];
					string text3 = args[i];
					if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
					{
						text = text.Replace(text2, text3);
					}
				}
			}
			FP fp;
			if (!ExpressionEvaluator.Evaluate(text, out fp))
			{
				HLog.LogError("Evaluate is null!!!" + info + "  copyStr = " + text);
			}
			this.m_value = fp;
		}

		public MergeAttributeData Clone()
		{
			return new MergeAttributeData
			{
				m_header = this.m_header,
				m_value = this.m_value
			};
		}

		private void Split(string info)
		{
			string[] array = Regex.Split(info, "=");
			if (array.Length == 2)
			{
				this.m_header = array[0];
				this.m_symol = "=";
				this.m_formula = array[1];
				return;
			}
		}

		public void Merge(MergeAttributeData data)
		{
			if (data == null)
			{
				return;
			}
			if (data.Header != this.Header)
			{
				return;
			}
			this.m_value += data.m_value;
		}

		public void Multiply(int number)
		{
			this.m_value *= number;
		}

		public void Multiply(float number)
		{
			this.m_value *= number;
		}

		public void Plus(FP number)
		{
			this.m_value += number;
		}

		public void Minus(FP number)
		{
			this.m_value -= number;
		}

		private string m_header;

		private string m_symol;

		private string m_formula;

		private FP m_value;

		private const string EqualsSymol = "=";
	}
}
