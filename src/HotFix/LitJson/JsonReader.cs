using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Server;

namespace LitJson
{
	public class JsonReader
	{
		public bool AllowComments
		{
			get
			{
				return this.lexer.AllowComments;
			}
			set
			{
				this.lexer.AllowComments = value;
			}
		}

		public bool AllowSingleQuotedStrings
		{
			get
			{
				return this.lexer.AllowSingleQuotedStrings;
			}
			set
			{
				this.lexer.AllowSingleQuotedStrings = value;
			}
		}

		public bool SkipNonMembers
		{
			get
			{
				return this.skip_non_members;
			}
			set
			{
				this.skip_non_members = value;
			}
		}

		public bool EndOfInput
		{
			get
			{
				return this.end_of_input;
			}
		}

		public bool EndOfJson
		{
			get
			{
				return this.end_of_json;
			}
		}

		public JsonToken Token
		{
			get
			{
				return this.token;
			}
		}

		public object Value
		{
			get
			{
				return this.token_value;
			}
		}

		public JsonReader(string json_text)
			: this(new StringReader(json_text), true)
		{
		}

		public JsonReader(TextReader reader)
			: this(reader, false)
		{
		}

		private JsonReader(TextReader reader, bool owned)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.parser_in_string = false;
			this.parser_return = false;
			this.read_started = false;
			this.automaton_stack = new Stack<int>();
			this.automaton_stack.Push(65553);
			this.automaton_stack.Push(65543);
			this.lexer = new Lexer(reader);
			this.end_of_input = false;
			this.end_of_json = false;
			this.skip_non_members = true;
			this.reader = reader;
			this.reader_is_owned = owned;
		}

		private static IDictionary<int, IDictionary<int, int[]>> PopulateParseTable()
		{
			IDictionary<int, IDictionary<int, int[]>> dictionary = new Dictionary<int, IDictionary<int, int[]>>();
			JsonReader.TableAddRow(dictionary, ParserToken.Array);
			JsonReader.TableAddCol(dictionary, ParserToken.Array, 91, new int[] { 91, 65549 });
			JsonReader.TableAddRow(dictionary, ParserToken.ArrayPrime);
			JsonReader.TableAddCol(dictionary, ParserToken.ArrayPrime, 34, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(dictionary, ParserToken.ArrayPrime, 91, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(dictionary, ParserToken.ArrayPrime, 93, new int[] { 93 });
			JsonReader.TableAddCol(dictionary, ParserToken.ArrayPrime, 123, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(dictionary, ParserToken.ArrayPrime, 65537, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(dictionary, ParserToken.ArrayPrime, 65538, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(dictionary, ParserToken.ArrayPrime, 65539, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(dictionary, ParserToken.ArrayPrime, 65540, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddRow(dictionary, ParserToken.Object);
			JsonReader.TableAddCol(dictionary, ParserToken.Object, 123, new int[] { 123, 65545 });
			JsonReader.TableAddRow(dictionary, ParserToken.ObjectPrime);
			JsonReader.TableAddCol(dictionary, ParserToken.ObjectPrime, 34, new int[] { 65546, 65547, 125 });
			JsonReader.TableAddCol(dictionary, ParserToken.ObjectPrime, 125, new int[] { 125 });
			JsonReader.TableAddRow(dictionary, ParserToken.Pair);
			JsonReader.TableAddCol(dictionary, ParserToken.Pair, 34, new int[] { 65552, 58, 65550 });
			JsonReader.TableAddRow(dictionary, ParserToken.PairRest);
			JsonReader.TableAddCol(dictionary, ParserToken.PairRest, 44, new int[] { 44, 65546, 65547 });
			JsonReader.TableAddCol(dictionary, ParserToken.PairRest, 125, new int[] { 65554 });
			JsonReader.TableAddRow(dictionary, ParserToken.String);
			JsonReader.TableAddCol(dictionary, ParserToken.String, 34, new int[] { 34, 65541, 34 });
			JsonReader.TableAddRow(dictionary, ParserToken.Text);
			JsonReader.TableAddCol(dictionary, ParserToken.Text, 91, new int[] { 65548 });
			JsonReader.TableAddCol(dictionary, ParserToken.Text, 123, new int[] { 65544 });
			JsonReader.TableAddRow(dictionary, ParserToken.Value);
			JsonReader.TableAddCol(dictionary, ParserToken.Value, 34, new int[] { 65552 });
			JsonReader.TableAddCol(dictionary, ParserToken.Value, 91, new int[] { 65548 });
			JsonReader.TableAddCol(dictionary, ParserToken.Value, 123, new int[] { 65544 });
			JsonReader.TableAddCol(dictionary, ParserToken.Value, 65537, new int[] { 65537 });
			JsonReader.TableAddCol(dictionary, ParserToken.Value, 65538, new int[] { 65538 });
			JsonReader.TableAddCol(dictionary, ParserToken.Value, 65539, new int[] { 65539 });
			JsonReader.TableAddCol(dictionary, ParserToken.Value, 65540, new int[] { 65540 });
			JsonReader.TableAddRow(dictionary, ParserToken.ValueRest);
			JsonReader.TableAddCol(dictionary, ParserToken.ValueRest, 44, new int[] { 44, 65550, 65551 });
			JsonReader.TableAddCol(dictionary, ParserToken.ValueRest, 93, new int[] { 65554 });
			return dictionary;
		}

		private static void TableAddCol(IDictionary<int, IDictionary<int, int[]>> parse_table, ParserToken row, int col, params int[] symbols)
		{
			parse_table[(int)row].Add(col, symbols);
		}

		private static void TableAddRow(IDictionary<int, IDictionary<int, int[]>> parse_table, ParserToken rule)
		{
			parse_table.Add((int)rule, new Dictionary<int, int[]>());
		}

		private void ProcessNumber(string number)
		{
			if (number.IndexOf('.') != -1 || number.IndexOf('e') != -1 || number.IndexOf('E') != -1)
			{
				if (this.useFp)
				{
					this.token = JsonToken.Fp;
					this.token_value = FP.FromString(number);
					return;
				}
				double num;
				if (double.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out num))
				{
					this.token = JsonToken.Double;
					this.token_value = num;
					return;
				}
			}
			int num2;
			if (int.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out num2))
			{
				this.token = JsonToken.Int;
				this.token_value = num2;
				return;
			}
			long num3;
			if (long.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out num3))
			{
				this.token = JsonToken.Long;
				this.token_value = num3;
				return;
			}
			ulong num4;
			if (ulong.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out num4))
			{
				this.token = JsonToken.Long;
				this.token_value = num4;
				return;
			}
			this.token = JsonToken.Int;
			this.token_value = 0;
		}

		private void ProcessSymbol()
		{
			if (this.current_symbol == 91)
			{
				this.token = JsonToken.ArrayStart;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 93)
			{
				this.token = JsonToken.ArrayEnd;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 123)
			{
				this.token = JsonToken.ObjectStart;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 125)
			{
				this.token = JsonToken.ObjectEnd;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 34)
			{
				if (this.parser_in_string)
				{
					this.parser_in_string = false;
					this.parser_return = true;
					return;
				}
				if (this.token == JsonToken.None)
				{
					this.token = JsonToken.String;
				}
				this.parser_in_string = true;
				return;
			}
			else
			{
				if (this.current_symbol == 65541)
				{
					this.token_value = this.lexer.StringValue;
					return;
				}
				if (this.current_symbol == 65539)
				{
					this.token = JsonToken.Boolean;
					this.token_value = false;
					this.parser_return = true;
					return;
				}
				if (this.current_symbol == 65540)
				{
					this.token = JsonToken.Null;
					this.parser_return = true;
					return;
				}
				if (this.current_symbol == 65537)
				{
					this.ProcessNumber(this.lexer.StringValue);
					this.parser_return = true;
					return;
				}
				if (this.current_symbol == 65546)
				{
					this.token = JsonToken.PropertyName;
					return;
				}
				if (this.current_symbol == 65538)
				{
					this.token = JsonToken.Boolean;
					this.token_value = true;
					this.parser_return = true;
				}
				return;
			}
		}

		private bool ReadToken()
		{
			if (this.end_of_input)
			{
				return false;
			}
			this.lexer.NextToken();
			if (this.lexer.EndOfInput)
			{
				this.Close();
				return false;
			}
			this.current_input = this.lexer.Token;
			return true;
		}

		public void Close()
		{
			if (this.end_of_input)
			{
				return;
			}
			this.end_of_input = true;
			this.end_of_json = true;
			if (this.reader_is_owned)
			{
				using (this.reader)
				{
				}
			}
			this.reader = null;
		}

		public bool Read()
		{
			if (this.end_of_input)
			{
				return false;
			}
			if (this.end_of_json)
			{
				this.end_of_json = false;
				this.automaton_stack.Clear();
				this.automaton_stack.Push(65553);
				this.automaton_stack.Push(65543);
			}
			this.parser_in_string = false;
			this.parser_return = false;
			this.token = JsonToken.None;
			this.token_value = null;
			if (!this.read_started)
			{
				this.read_started = true;
				if (!this.ReadToken())
				{
					return false;
				}
			}
			while (!this.parser_return)
			{
				this.current_symbol = this.automaton_stack.Pop();
				this.ProcessSymbol();
				if (this.current_symbol == this.current_input)
				{
					if (!this.ReadToken())
					{
						if (this.automaton_stack.Peek() != 65553)
						{
							throw new JsonException("Input doesn't evaluate to proper JSON text");
						}
						return this.parser_return;
					}
				}
				else
				{
					int[] array;
					try
					{
						array = JsonReader.parse_table[this.current_symbol][this.current_input];
					}
					catch (KeyNotFoundException ex)
					{
						HLog.LogException(ex);
						throw;
					}
					if (array[0] != 65554)
					{
						for (int i = array.Length - 1; i >= 0; i--)
						{
							this.automaton_stack.Push(array[i]);
						}
					}
				}
			}
			if (this.automaton_stack.Peek() == 65553)
			{
				this.end_of_json = true;
			}
			return true;
		}

		private static readonly IDictionary<int, IDictionary<int, int[]>> parse_table = JsonReader.PopulateParseTable();

		private Stack<int> automaton_stack;

		private int current_input;

		private int current_symbol;

		private bool end_of_json;

		private bool end_of_input;

		private Lexer lexer;

		private bool parser_in_string;

		private bool parser_return;

		private bool read_started;

		private TextReader reader;

		private bool reader_is_owned;

		private bool skip_non_members;

		private object token_value;

		private JsonToken token;

		public bool useFp;
	}
}
