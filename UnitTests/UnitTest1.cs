using TPUMStart;

namespace UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void TestCalculatorAdd()
        {
            Calculator calculator = new Calculator();

            Assert.Equal(2.75f, calculator.Add(1.5f, 1.25f));
        }

        [Fact]
        public void TestCalculatorSubtract()
        {
            Calculator calculator = new Calculator();

            Assert.Equal(0.25f, calculator.Subtract(1.5f, 1.25f));
        }

        [Fact]
        public void TestCalculatorMultiply()
        {
            Calculator calculator = new Calculator();

            Assert.Equal(1.875f, calculator.Multiply(1.5f, 1.25f));
        }

        [Fact]
        public void TestCalculatorDivide()
        {
            Calculator calculator = new Calculator();

            Assert.Equal(1.2f, calculator.Divide(1.5f, 1.25f));
        }
    }
}