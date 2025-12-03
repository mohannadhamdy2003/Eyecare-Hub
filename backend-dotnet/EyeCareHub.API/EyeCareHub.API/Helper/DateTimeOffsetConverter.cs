using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;
using System;

namespace EyeCareHub.API.Helper
{
    public class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => DateTimeOffset.Parse(reader.GetString());

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString("dddd، dd MMMM yyyy", new CultureInfo("ar-EG")));
    }
}
