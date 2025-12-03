using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.HelperDLL
{
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException("Expected a string value for TimeSpan.");

            var value = reader.GetString();
            if (string.IsNullOrEmpty(value))
                throw new JsonException("TimeSpan string cannot be empty.");

            // تحويل القيمة من "HH:mm" لـ TimeSpan
            if (TimeSpan.TryParseExact(value, @"hh\:mm", null, out var timeSpan))
            {
                return timeSpan;
            }

            throw new JsonException($"Cannot parse '{value}' to TimeSpan. Expected format is 'HH:mm'.");
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(@"hh\:mm"));
        }
    }
}
