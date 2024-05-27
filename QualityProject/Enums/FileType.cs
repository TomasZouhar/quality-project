using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace QualityProject.API.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FileType
    {
        [EnumMember(Value = "CSV")]
        Csv,

        [EnumMember(Value = "HTML")]
        Html,

        [EnumMember(Value = "PDF")]
        Pdf,
    }
}
