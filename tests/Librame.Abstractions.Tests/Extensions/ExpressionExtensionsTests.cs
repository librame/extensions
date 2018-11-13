using System;
using System.Linq.Expressions;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class ExpressionExtensionsTests
    {

        [Fact]
        public void PropertyExpressionTest()
        {
            Expression<Func<TestExpression, string>> expression = p => p.Name;

            var name = expression.AsPropertyName();
            Assert.Equal("Name", name);

            // p => p.Name
            var autoExpression = "Name".AsPropertyExpression<TestExpression, string>();
            Assert.Equal(expression.ToString(), autoExpression.ToString());

            // p => p.PropertyName > 3
            var greaterThanExpression = "Value".AsGreaterThanPropertyExpression<TestExpression, int>(3);
            var greaterThanExpression1 = "Value".AsGreaterThanPropertyExpression<TestExpression>(typeof(int), 3);
            Assert.Equal(greaterThanExpression.ToString(), greaterThanExpression1.ToString());

            // p => p.PropertyName >= 3
            var greaterThanOrEqualExpression = "Value".AsGreaterThanOrEqualPropertyExpression<TestExpression, int>(3);
            var greaterThanOrEqualExpression1 = "Value".AsGreaterThanOrEqualPropertyExpression<TestExpression>(typeof(int), 3);
            Assert.Equal(greaterThanOrEqualExpression.ToString(), greaterThanOrEqualExpression1.ToString());

            // p => p.PropertyName < 3
            var lessThanExpression = "Value".AsGreaterThanPropertyExpression<TestExpression, int>(3);
            var lessThanExpression1 = "Value".AsGreaterThanPropertyExpression<TestExpression>(typeof(int), 3);
            Assert.Equal(lessThanExpression.ToString(), lessThanExpression1.ToString());

            // p => p.PropertyName <= 3
            var lessThanOrEqualExpression = "Value".AsGreaterThanOrEqualPropertyExpression<TestExpression, int>(3);
            var lessThanOrEqualExpression1 = "Value".AsGreaterThanOrEqualPropertyExpression<TestExpression>(typeof(int), 3);
            Assert.Equal(lessThanOrEqualExpression.ToString(), lessThanOrEqualExpression1.ToString());

            // p => p.PropertyName != 3
            var notEqualExpression = "Value".AsNotEqualPropertyExpression<TestExpression, int>(3);
            var notEqualExpression1 = "Value".AsNotEqualPropertyExpression<TestExpression>(typeof(int), 3);
            Assert.Equal(notEqualExpression.ToString(), notEqualExpression1.ToString());
        }

    }

    
    public class TestExpression
    {
        public string Name { get; set; }

        public int Value { get; set; } = 5;
    }

}
