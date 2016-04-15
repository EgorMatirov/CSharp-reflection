using System;
using NUnit.Framework;
using Reflection_Algebra;

namespace Tests
{
    public class DerivativeCalculatorTests
    {
        [Test]
        public void ShouldCorrectlyParseSimpleLambdaWithAddition()
        {
            const int expected = 2;

            var derivative = DerivativeCalculator.GetDerivative(x => x + 3 + x);
            var actual = derivative(Math.PI / 3);

            // It's not real equality but I have no idea how to compare them.
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShouldCorrectlyParseSimpleLambdaWithMultiplication()
        {
            const int expected = 8;

            var derivative = DerivativeCalculator.GetDerivative(x =>4*x*2);
            var actual = derivative(Math.PI / 3);

            // It's not real equality but I have no idea how to compare them.
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShouldCorrectlyParseSimpleLambdaWithSin()
        {
            var expected = Math.Cos(Math.PI/3);

            var derivative = DerivativeCalculator.GetDerivative(x => Math.Sin(x));
            var actual = derivative(Math.PI / 3);

            // It's not real equality but I have no idea how to compare them.
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShouldCorrectlyParseComplexLambda()
        {
            const double expected = 12 * (Math.PI/3) + 24;

            var derivative = DerivativeCalculator.GetDerivative(x => x*6*(x+4));
            var actual = derivative(Math.PI / 3);

            // It's not real equality but I have no idea how to compare them.
            Assert.AreEqual(expected, actual);
        }
    }
}
