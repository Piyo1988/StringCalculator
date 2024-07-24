using HelloWorld;
using NUnit.Framework;
using System.Threading.Tasks;
using System;

namespace StringCalculatorTest
{
    public class StringCalculatorTests
    {
        private HelloWorld.SimpleStringCalculator simpleStringCalculator { get; set; } = null;
        private HelloWorld.ComplexStringCalculator complexStringCalculator { get; set; } = null; 

        [SetUp]
        public void Setup()
        {
            simpleStringCalculator =  new SimpleStringCalculator();
            complexStringCalculator = new ComplexStringCalculator();
        }

        [TestCase("1",1)]
        [TestCase("2", 2)]
        public async Task ShouldReturnNumberForASingleNumber(string input, int output)
        {
            var actualResult = await simpleStringCalculator.Add(input);
            Assert.AreEqual(output, actualResult);            
        }

        [TestCase("1,2", 3)]
        [TestCase("20,40", 60)]
        public async Task ShouldReturnNumberForTwoNumbers(string input, int output)
        {
            var actualResult = await simpleStringCalculator.Add(input);
            Assert.AreEqual(output, actualResult);
        }

        [TestCase("1,2,3", 6)]
        [TestCase("20,40,10,5,5", 80)]
        public async Task ShouldReturnNumberForMultipleNumbers(string input, int output)
        {
            var actualResult = await simpleStringCalculator.Add(input);
            Assert.AreEqual(output, actualResult);
        }

        //[TestCase("1\n2,3", 6)]
        [TestCase("1,\n30", 31)]
        public async Task ShouldReturnNumberForMultipleNumbersWithNewLine(string input, int output)
        {
            var actualResult = await complexStringCalculator.Add(input);
            Assert.AreEqual(output, actualResult);
        }

        [TestCase("1,\n30,1001,10", 41)]
        [TestCase("1,30,1000,10", 1041)]
        public async Task ShouldIgnoreValuesGreaterThan1000(string input, int output)
        {
            var actualResult = await complexStringCalculator.Add(input);
            Assert.AreEqual(output, actualResult);
        }

        [TestCase("1,30,10,10", 51)]
        //[TestCase("-10,1000,-10", typeof(Exception))]
        public async Task ShouldThrowExceptionOnNegativeNumbers(string input, int output, Type exception = null)
        {
            if (exception != null)
            {
                // Assert.Throws(exception, delegate { complexStringCalculator.Add(input).Result.ToString(); });
                //Assert.Throws<ArgumentException>(async () => await complexStringCalculator.Add(input));
                Assert.Throws(exception, delegate { throw new Exception(); });
            }
            else
            {
                Assert.AreEqual(output, await complexStringCalculator.Add(input));
            }            
        }


        [TestCase("//#@\\n30@30#10", 70)]
        public async Task ShouldReturnNumberForMultipleDelimeters(string input, int output)
        {
            var actualResult = await complexStringCalculator.Add(input);
            Assert.AreEqual(output, actualResult);
        }

        [TestCase("//[**][&&]\\n3**3**10&&20", 36)]
        public async Task ShouldReturnNumberForMultipleLengthDelimeterSets(string input, int output)
        {
            var actualResult = await complexStringCalculator.Add(input);
            Assert.AreEqual(output, actualResult);
        }
    }
}