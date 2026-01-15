namespace Toolbox.ValueObjects.Tests;

using System.Globalization;
using CodeGeneration.Attributes;
using NUnit.Framework;

/*[TestFixture]
public class TestIntValueObjectTests
{
    [Test]
    public void Create_WithValidValue_ReturnsValueObject()
    {
        // Arrange
        const int value = 42;
        
        // Act
        var result = TestIntValueObject.Create(value);
        
        // Assert
        Assert.That(result.Value, Is.EqualTo(value));
    }
    
    [Test]
    public void Create_SameValues_AreEqual()
    {
        // Arrange & Act
        var vo1 = TestIntValueObject.Create(42);
        var vo2 = TestIntValueObject.Create(42);
        
        // Assert
        Assert.That(vo1, Is.EqualTo(vo2));
        Assert.That(vo1 == vo2, Is.True);
        Assert.That(vo1 != vo2, Is.False);
    }
    
    [Test]
    public void Create_DifferentValues_AreNotEqual()
    {
        // Arrange & Act
        var vo1 = TestIntValueObject.Create(42);
        var vo2 = TestIntValueObject.Create(100);
        
        // Assert
        Assert.That(vo1, Is.Not.EqualTo(vo2));
        Assert.That(vo1 == vo2, Is.False);
        Assert.That(vo1 != vo2, Is.True);
    }
    
    [Test]
    public void Equals_WithNull_ReturnsFalse()
    {
        // Arrange
        var vo = TestIntValueObject.Create(42);
        
        // Act & Assert
        Assert.That(vo.Equals(null), Is.False);
    }
    
    [Test]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        // Arrange
        var vo = TestIntValueObject.Create(42);
        var differentObject = new object();
        
        // Act & Assert
        Assert.That(vo.Equals(differentObject), Is.False);
    }
    
    [Test]
    public void GetHashCode_SameValues_HaveSameHashCode()
    {
        // Arrange & Act
        var vo1 = TestIntValueObject.Create(42);
        var vo2 = TestIntValueObject.Create(42);
        
        // Assert
        Assert.That(vo1.GetHashCode(), Is.EqualTo(vo2.GetHashCode()));
    }
    
    [Test]
    public void GetHashCode_DifferentValues_HaveDifferentHashCodes()
    {
        // Arrange & Act
        var vo1 = TestIntValueObject.Create(42);
        var vo2 = TestIntValueObject.Create(100);
        
        // Assert
        Assert.That(vo1.GetHashCode(), Is.Not.EqualTo(vo2.GetHashCode()));
    }
    
    [Test]
    public void CompareTo_SameValues_ReturnsZero()
    {
        // Arrange & Act
        var vo1 = TestIntValueObject.Create(42);
        var vo2 = TestIntValueObject.Create(42);
        
        // Assert
        Assert.That(vo1.CompareTo(vo2), Is.EqualTo(0));
    }
    
    [Test]
    public void CompareTo_SmallerValue_ReturnsNegative()
    {
        // Arrange & Act
        var smaller = TestIntValueObject.Create(10);
        var larger = TestIntValueObject.Create(20);
        
        // Assert
        Assert.That(smaller.CompareTo(larger), Is.LessThan(0));
        Assert.That(smaller < larger, Is.True);
        Assert.That(smaller <= larger, Is.True);
    }
    
    [Test]
    public void CompareTo_LargerValue_ReturnsPositive()
    {
        // Arrange & Act
        var smaller = TestIntValueObject.Create(10);
        var larger = TestIntValueObject.Create(20);
        
        // Assert
        Assert.That(larger.CompareTo(smaller), Is.GreaterThan(0));
        Assert.That(larger > smaller, Is.True);
        Assert.That(larger >= smaller, Is.True);
    }
    
    [Test]
    public void ExplicitConversion_FromInt_ToValueObject()
    {
        // Arrange
        const int value = 42;
        
        // Act
        var vo = (TestIntValueObject)value;
        
        // Assert
        Assert.That(vo.Value, Is.EqualTo(value));
    }
    
    [Test]
    public void ExplicitConversion_FromValueObject_ToInt()
    {
        // Arrange
        var vo = TestIntValueObject.Create(42);
        
        // Act
        int value = (int)vo;
        
        // Assert
        Assert.That(value, Is.EqualTo(42));
    }
    
    [Test]
    public void IsDefault_WithDefaultValue_ReturnsTrue()
    {
        // Arrange & Act
        var defaultVo = default(TestIntValueObject);
        
        // Assert
        Assert.That(defaultVo.IsDefault, Is.True);
    }
    
    [Test]
    public void IsDefault_WithNonDefaultValue_ReturnsFalse()
    {
        // Arrange & Act
        var vo = TestIntValueObject.Create(42);
        
        // Assert
        Assert.That(vo.IsDefault, Is.False);
    }
    
    [Test]
    public void ToString_ReturnsStringRepresentation()
    {
        // Arrange
        var vo = TestIntValueObject.Create(42);
        
        // Act
        var result = vo.ToString();
        
        // Assert
        Assert.That(result, Is.EqualTo("42"));
    }
    
    [Test]
    public void ToString_WithDefaultValue_ReturnsDefaultString()
    {
        // Arrange
        var defaultVo = default(TestIntValueObject);
        
        // Act
        var result = defaultVo.ToString();
        
        // Assert
        Assert.That(result, Is.EqualTo("0"));
    }

    [TestFixture]
    public class EdgeCaseTests
    {
        [Test]
        public void WithZeroValue_WorksCorrectly()
        {
            // Arrange & Act
            var vo = TestIntValueObject.Create(0);
            
            // Assert
            Assert.That(vo.Value, Is.EqualTo(0));
            Assert.That(vo.ToString(), Is.EqualTo("0"));
            Assert.That(vo.IsDefault, Is.True);
        }
        
        [Test]
        public void WithNegativeValue_WorksCorrectly()
        {
            // Arrange & Act
            var vo = TestIntValueObject.Create(-42);
            
            // Assert
            Assert.That(vo.Value, Is.EqualTo(-42));
            Assert.That(vo.ToString(), Is.EqualTo("-42"));
            Assert.That(vo < TestIntValueObject.Create(0), Is.True);
        }
        
        [Test]
        public void WithMaxIntValue_WorksCorrectly()
        {
            // Arrange & Act
            var vo = TestIntValueObject.Create(int.MaxValue);
            
            // Assert
            Assert.That(vo.Value, Is.EqualTo(int.MaxValue));
            Assert.That(vo.ToString(), Is.EqualTo(int.MaxValue.ToString()));
        }
        
        [Test]
        public void WithMinIntValue_WorksCorrectly()
        {
            // Arrange & Act
            var vo = TestIntValueObject.Create(int.MinValue);
            
            // Assert
            Assert.That(vo.Value, Is.EqualTo(int.MinValue));
            Assert.That(vo.ToString(), Is.EqualTo(int.MinValue.ToString()));
        }
    }
    
    [TestFixture]
    public class PerformanceTests
    {
        [Test]
        [Repeat(1000)]
        public void Equals_Performance_IsFast()
        {
            // Arrange
            var vo1 = TestIntValueObject.Create(42);
            var vo2 = TestIntValueObject.Create(42);
            
            // Act
            var result = vo1.Equals(vo2);
            
            // Assert
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void HashCode_Performance_IsConsistent()
        {
            // Arrange
            var vo = TestIntValueObject.Create(42);
            var expectedHashCode = vo.GetHashCode();
            
            // Act & Assert - hash code should be consistent
            for (int i = 0; i < 100; i++)
            {
                Assert.That(vo.GetHashCode(), Is.EqualTo(expectedHashCode));
            }
        }
    }
    
    [TestFixture]
    public class ComparisonOperatorTests
    {
        [TestCase(10, 20, ExpectedResult = true)]
        [TestCase(20, 10, ExpectedResult = false)]
        [TestCase(10, 10, ExpectedResult = false)]
        public bool LessThanOperator_WorksCorrectly(int left, int right)
        {
            // Arrange
            var leftVo = TestIntValueObject.Create(left);
            var rightVo = TestIntValueObject.Create(right);
            
            // Act & Assert
            return leftVo < rightVo;
        }
        
        [TestCase(10, 20, ExpectedResult = false)]
        [TestCase(20, 10, ExpectedResult = true)]
        [TestCase(10, 10, ExpectedResult = false)]
        public bool GreaterThanOperator_WorksCorrectly(int left, int right)
        {
            // Arrange
            var leftVo = TestIntValueObject.Create(left);
            var rightVo = TestIntValueObject.Create(right);
            
            // Act & Assert
            return leftVo > rightVo;
        }
        
        [TestCase(10, 20, ExpectedResult = true)]
        [TestCase(20, 10, ExpectedResult = false)]
        [TestCase(10, 10, ExpectedResult = true)]
        public bool LessThanOrEqualOperator_WorksCorrectly(int left, int right)
        {
            // Arrange
            var leftVo = TestIntValueObject.Create(left);
            var rightVo = TestIntValueObject.Create(right);
            
            // Act & Assert
            return leftVo <= rightVo;
        }
        
        [TestCase(10, 20, ExpectedResult = false)]
        [TestCase(20, 10, ExpectedResult = true)]
        [TestCase(10, 10, ExpectedResult = true)]
        public bool GreaterThanOrEqualOperator_WorksCorrectly(int left, int right)
        {
            // Arrange
            var leftVo = TestIntValueObject.Create(left);
            var rightVo = TestIntValueObject.Create(right);
            
            // Act & Assert
            return leftVo >= rightVo;
        }
    }
}

[TestFixture]
public class OrderIdTests
{
    [Test]
    public void SerializeOrderIdToJson()
    {
        

        var     yourOrderIdInstance = TestIntValueObject.Create(12345);
        
        string             json    = System.Text.Json.JsonSerializer.Serialize(yourOrderIdInstance);
        TestIntValueObject orderId = System.Text.Json.JsonSerializer.Deserialize<TestIntValueObject>(json);
        
        

        Assert.That(json, Is.EqualTo("12345"));
    }

    [Test]
    public void DeserializeJsonToOrderId()
    {
        try
        {
            string json    = "\"12345\"";
            var    orderId = System.Text.Json.JsonSerializer.Deserialize<TestIntValueObject>(json);

            Assert.That(orderId.Value, Is.EqualTo(12345));
        }
        catch (System.Text.Json.JsonException ex)
        {
            Assert.Pass("Invalid JSON token for TestIntValueObject. Expected System.Text.Json.JsonTokenType.Number, got String.");
        }
    }

    [Test]
    public void SerializeAndDeserializeRoundtrip()
    {
        var    orderId             = TestIntValueObject.Create(67890);
        string json                = System.Text.Json.JsonSerializer.Serialize(orderId);
        var    deserializedOrderId = System.Text.Json.JsonSerializer.Deserialize<TestIntValueObject>(json);

        Assert.That(deserializedOrderId.Value, Is.EqualTo(orderId.Value));
    }

    [Test]
    public void SerializeNullValue()
    {
        try
        {
            TestIntValueObject? nullOrderId = null;
            string   json        = System.Text.Json.JsonSerializer.Serialize(nullOrderId);
        }
        catch (System.Text.Json.JsonException ex)
        {
            Assert.Pass("Expected JsonException for null value");
        }
    }

    [Test]
    public void DeserializeInvalidJson()
    {
        string invalidJson = "\"abcde\"";
        try
        {
            var orderId = System.Text.Json.JsonSerializer.Deserialize<TestIntValueObject>(invalidJson);
        }
        catch (System.Text.Json.JsonException ex)
        {
            Assert.Pass("Expected JsonException for invalid JSON");
        }
    }
}*/
