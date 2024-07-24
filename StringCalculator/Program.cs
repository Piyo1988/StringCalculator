using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace HelloWorld
{
	public class Program
	{
		public static void Main(string[] args)
		{
			while (true)
			{
				Console.Write("\nEnter string: ");
				
				//implementing asynchronous processing for string calculation
				Task t = MainAsync(args);
				var defaultMessage = "Processing your request...";
				Console.WriteLine(defaultMessage);
				t.Wait();
			}
		}

		public static async Task MainAsync(string[] args)
		{
			string input = Console.ReadLine();

			//this class is a simple string calculator with only single/multiple numbers seperated by commas
			SimpleStringCalculator strSimpleCalc = new SimpleStringCalculator();
			int genericAddResult = string.IsNullOrEmpty(input) ? 0 : await strSimpleCalc.Add(input);
			Console.WriteLine("Q1. Simple String Calculator Result: " + genericAddResult);

			//this class is more specific string calculator to handle different delimiters, negative values etc
			ComplexStringCalculator strComplexCalc = new ComplexStringCalculator();
			int addResult = string.IsNullOrEmpty(input) ? 0 : await strComplexCalc.Add(input);
			Console.WriteLine("Q2-Q6 and Bonus. Complex String Calculator Result: " + addResult);
		}
	}

	interface IStringCalculator
	{
		Task<int> Add(string val);
	}

	public class SimpleStringCalculator : IStringCalculator
	{
		public async Task<int> Add(string input)
		{
			await Task.Delay(2000);
			int total = 0;
			try
			{
				foreach (var number in input.Split(","))
				{
					total += int.Parse(number);
				}
				return total;
			}
			catch (Exception e)
			{
				//Console.WriteLine(e.Message);
				return 0;
			}
		}
	}

	public class ComplexStringCalculator : IStringCalculator
	{
		//method to return substring for the provided input string
		private string GetSubString(string input, string fromChar, string toChar)
		{
			string actualinput = input;
			List<string> groupedDelimeters = new List<string>();

			int from = !string.IsNullOrEmpty(fromChar) ? input.IndexOf(fromChar) + fromChar.Length : -1;
			int to = !string.IsNullOrEmpty(toChar) ? input.LastIndexOf(toChar) : -1;

			//handling any length delimeters
			if ((from > -1 && to > -1) && input.Contains("["))
			{
				while (input.Contains("["))
				{
					groupedDelimeters.Add(input.Split('[', ']')[1]);
					input = input.Replace("[" + input.Split('[', ']')[1] + "]", "");
				}

				string partInput = actualinput.Substring(actualinput.LastIndexOf("]") + 1);
				foreach (var item in partInput.Substring(0, partInput.IndexOf("\\n")))
				{
					groupedDelimeters.Add(item.ToString());					
				}
				//Console.WriteLine(string.Join(" ", groupedDelimeters));
			}

			//Console.WriteLine(from > -1 && to > -1 && actualinput.Contains("[")
			//   ? groupedDelimeters.ToString()
			//   : from > -1 && to > -1
			//   ? actualinput.Substring(from, to - from)
			//   : to == -1
			//	 ? actualinput.Substring(from)
			//	 : actualinput
			//);
			return from > -1 && to > -1 && actualinput.Contains("[")
			   ? String.Join("", groupedDelimeters)
			   : from > -1 && to > -1
			   ? actualinput.Substring(from, to - from)
			   : to == -1
				 ? actualinput.Substring(from)
				 : actualinput
			;
		}

		//method to loop through valid numbers to sum them
		private int AddUpNumbers(string input, List<string> allDelimeters, ref StringBuilder negatives)
		{
			int total = 0;
			int maxValueLimit = 1000;
			foreach (var value in input.Split(allDelimeters.Select(i => i.ToString()).ToArray(), StringSplitOptions.None))
			{
				int.TryParse(value, out int validNumber);				

				//check for negatives in the input to exclude from calculation
				if (validNumber < 0)
				{
					if (negatives != null && negatives.Length > 0) { negatives.Append(", "); }
					negatives.Append(validNumber);
					continue;
				};

				total += validNumber > maxValueLimit ? 0 : validNumber;
			}
			return total;
		}

		public async Task<int> Add(string input)
		{
			await Task.Delay(2000);
			int total = 0;
			StringBuilder negatives = new StringBuilder();
			List<string> allDelimeters = null;
			string delimeters = null;

			try
			{
				if (input.StartsWith("//"))
				{
					string delimeterResult = GetSubString(input, "//", @"\n");
					Console.Write("delimeter result " + delimeterResult);
					if (delimeterResult.Contains(" "))
					{
						delimeters = string.Join(",", delimeterResult.Split(" "));
					}
					else
					{
						delimeters = string.Join(",", delimeterResult.ToArray());
					}
					Console.Write("delimeter " + delimeters);
				}

				if (string.IsNullOrEmpty(delimeters))
				{
					allDelimeters = new List<string>();
					allDelimeters.Add(@"\n".ToString());
					allDelimeters.Add(",");
					total = AddUpNumbers(input, allDelimeters, ref negatives);
					Console.WriteLine(negatives);
				}
				else
				{
					allDelimeters = new List<string>();
					allDelimeters.Add(@"\n".ToString());
					foreach (var delimeter in delimeters)
					{
						allDelimeters.Add(delimeter.ToString());
					}
					
					string inputSubstring = GetSubString(input, "\\n", "");
					total = AddUpNumbers(inputSubstring, allDelimeters, ref negatives);
				}

				if (negatives.Length > 0)
				{
					throw new Exception("negatives not allowed - " + negatives);
				}
			}
			catch (Exception e)
			{
				throw;
			}
			return total;
		}
	}
}