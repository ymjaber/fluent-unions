using System.Text.Json;
using FluentUnions.Serialization;

namespace FluentUnions.Tests.Serialization
{
    public class OptionSerializationTests
    {
        private readonly JsonSerializerOptions _options;

        public OptionSerializationTests()
        {
            _options = new JsonSerializerOptions();
            _options.Converters.Add(new OptionJsonConverter<int>());
            _options.Converters.Add(new OptionJsonConverter<string>());
            _options.Converters.Add(new OptionJsonConverter<Person>());
            _options.Converters.Add(new OptionJsonConverter<DateTime>());
        }

        [Fact]
        public void Serialize_Some_WritesValue()
        {
            // Arrange
            var option = Option.Some(42);

            // Act
            var json = JsonSerializer.Serialize(option, _options);

            // Assert
            Assert.Equal("42", json);
        }

        [Fact]
        public void Serialize_None_WritesNull()
        {
            // Arrange
            Option<int> option = Option.None;

            // Act
            var json = JsonSerializer.Serialize(option, _options);

            // Assert
            Assert.Equal("null", json);
        }

        [Fact]
        public void Serialize_SomeString_WritesQuotedString()
        {
            // Arrange
            var option = Option.Some("Hello World");

            // Act
            var json = JsonSerializer.Serialize(option, _options);

            // Assert
            Assert.Equal("\"Hello World\"", json);
        }

        [Fact]
        public void Serialize_SomeComplexType_WritesObject()
        {
            // Arrange
            var person = new Person { Id = 1, Name = "John Doe", Age = 30 };
            var option = Option.Some(person);

            // Act
            var json = JsonSerializer.Serialize(option, _options);

            // Assert
            var expected = "{\"Id\":1,\"Name\":\"John Doe\",\"Age\":30}";
            Assert.Equal(expected, json);
        }

        [Fact]
        public void Deserialize_Value_ReturnsSome()
        {
            // Arrange
            var json = "42";

            // Act
            var option = JsonSerializer.Deserialize<Option<int>>(json, _options);

            // Assert
            Assert.True(option.IsSome);
            Assert.Equal(42, option.Value);
        }

        [Fact]
        public void Deserialize_Null_ReturnsNone()
        {
            // Arrange
            var json = "null";

            // Act
            var option = JsonSerializer.Deserialize<Option<int>>(json, _options);

            // Assert
            Assert.True(option.IsNone);
        }

        [Fact]
        public void Deserialize_String_ReturnsSome()
        {
            // Arrange
            var json = "\"Hello World\"";

            // Act
            var option = JsonSerializer.Deserialize<Option<string>>(json, _options);

            // Assert
            Assert.True(option.IsSome);
            Assert.Equal("Hello World", option.Value);
        }

        [Fact]
        public void Deserialize_Object_ReturnsSome()
        {
            // Arrange
            var json = "{\"Id\":1,\"Name\":\"John Doe\",\"Age\":30}";

            // Act
            var option = JsonSerializer.Deserialize<Option<Person>>(json, _options);

            // Assert
            Assert.True(option.IsSome);
            Assert.Equal(1, option.Value.Id);
            Assert.Equal("John Doe", option.Value.Name);
            Assert.Equal(30, option.Value.Age);
        }

        [Fact]
        public void RoundTrip_Some_PreservesValue()
        {
            // Arrange
            var original = Option.Some(42);

            // Act
            var json = JsonSerializer.Serialize(original, _options);
            var deserialized = JsonSerializer.Deserialize<Option<int>>(json, _options);

            // Assert
            Assert.True(deserialized.IsSome);
            Assert.Equal(original.Value, deserialized.Value);
        }

        [Fact]
        public void RoundTrip_None_PreservesNone()
        {
            // Arrange
            Option<int> original = Option.None;

            // Act
            var json = JsonSerializer.Serialize(original, _options);
            var deserialized = JsonSerializer.Deserialize<Option<int>>(json, _options);

            // Assert
            Assert.True(deserialized.IsNone);
        }

        [Fact]
        public void Serialize_NestedOption_Some_WritesNestedValue()
        {
            // Arrange
            var container = new Container
            {
                Id = 1,
                OptionalName = Option.Some("Test"),
                OptionalAge = Option.None
            };

            // Act
            var json = JsonSerializer.Serialize(container, _options);

            // Assert
            var expected = "{\"Id\":1,\"OptionalName\":\"Test\",\"OptionalAge\":null}";
            Assert.Equal(expected, json);
        }

        [Fact]
        public void Deserialize_NestedOption_ReconstructsCorrectly()
        {
            // Arrange
            var json = "{\"Id\":1,\"OptionalName\":\"Test\",\"OptionalAge\":null}";

            // Act
            var container = JsonSerializer.Deserialize<Container>(json, _options);

            // Assert
            Assert.NotNull(container);
            Assert.Equal(1, container.Id);
            Assert.True(container.OptionalName.IsSome);
            Assert.Equal("Test", container.OptionalName.Value);
            Assert.True(container.OptionalAge.IsNone);
        }

        [Fact]
        public void Serialize_ArrayWithOptions_HandlesCorrectly()
        {
            // Arrange
            var options = new[]
            {
                Option.Some(1),
                Option<int>.None,
                Option.Some(3)
            };

            // Act
            var json = JsonSerializer.Serialize(options, _options);

            // Assert
            Assert.Equal("[1,null,3]", json);
        }

        [Fact]
        public void Deserialize_ArrayWithOptions_HandlesCorrectly()
        {
            // Arrange
            var json = "[1,null,3]";

            // Act
            var options = JsonSerializer.Deserialize<Option<int>[]>(json, _options);

            // Assert
            Assert.NotNull(options);
            Assert.Equal(3, options.Length);
            Assert.True(options[0].IsSome);
            Assert.Equal(1, options[0].Value);
            Assert.True(options[1].IsNone);
            Assert.True(options[2].IsSome);
            Assert.Equal(3, options[2].Value);
        }

        [Fact]
        public void Serialize_DateTime_HandlesCorrectly()
        {
            // Arrange
            var date = new DateTime(2023, 12, 25, 10, 30, 0, DateTimeKind.Utc);
            var option = Option.Some(date);

            // Act
            var json = JsonSerializer.Serialize(option, _options);

            // Assert
            Assert.Contains("2023-12-25T10:30:00", json);
        }

        [Fact]
        public void Deserialize_DateTime_HandlesCorrectly()
        {
            // Arrange
            var json = "\"2023-12-25T10:30:00Z\"";

            // Act
            var option = JsonSerializer.Deserialize<Option<DateTime>>(json, _options);

            // Assert
            Assert.True(option.IsSome);
            Assert.Equal(new DateTime(2023, 12, 25, 10, 30, 0, DateTimeKind.Utc), option.Value);
        }

        [Fact]
        public void Serialize_EmptyString_TreatsAsSome()
        {
            // Arrange
            var option = Option.Some("");

            // Act
            var json = JsonSerializer.Serialize(option, _options);

            // Assert
            Assert.Equal("\"\"", json);
        }

        [Fact]
        public void Deserialize_EmptyString_ReturnsSome()
        {
            // Arrange
            var json = "\"\"";

            // Act
            var option = JsonSerializer.Deserialize<Option<string>>(json, _options);

            // Assert
            Assert.True(option.IsSome);
            Assert.Equal("", option.Value);
        }

        // Test models
        private class Person
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public int Age { get; set; }
        }

        private class Container
        {
            public int Id { get; set; }
            public Option<string> OptionalName { get; set; }
            public Option<int> OptionalAge { get; set; }
        }
    }
}