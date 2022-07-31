using System.Runtime.Serialization;
using System.Text.Json.Serialization;


namespace aniList_cli.Repository.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MediaSeason
{
    [EnumMember(Value = "WINTER")]
    WINTER,
    [EnumMember(Value = "SPRING")]
    SPRING,
    [EnumMember(Value = "SUMMER")]
    SUMMER,
    [EnumMember(Value = "FALL")]
    FALL
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MediaStatus
{
    [EnumMember(Value = "FINISHED")]
    FINISHED,
    [EnumMember(Value = "RELEASING")]
    RELEASING,
    [EnumMember(Value = "NOT_YET_RELEASED")]
    NOT_YET_RELEASED,
    [EnumMember(Value = "CANCELLED")]
    CANCELLED,
    [EnumMember(Value = "HIATUS")]
    HIATUS
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MediaFormat
{
    [EnumMember(Value = "TV")]
    TV,
    [EnumMember(Value = "TV_SHORT")]
    TV_SHORT,
    [EnumMember(Value = "MOVIE")]
    MOVIE,
    [EnumMember(Value = "SPECIAL")]
    SPECIAL,
    [EnumMember(Value = "OVA")]
    OVA,
    [EnumMember(Value = "ONA")]
    ONA,
    [EnumMember(Value = "MUSIC")]
    MUSIC,
    [EnumMember(Value = "MANGA")]
    MANGA,
    [EnumMember(Value = "NOVEL")]
    NOVEL,
    [EnumMember(Value = "ONE_SHOT")]
    ONE_SHOT
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MediaType
{
    [EnumMember(Value = "ANIME")]
    ANIME,
    [EnumMember(Value = "MANGA")]
    MANGA
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MediaListStatus
{
    [EnumMember(Value = "CURRENT")]
    CURRENT,
    [EnumMember(Value = "PLANNING")]
    PLANNING,
    [EnumMember(Value = "COMPLETED")]
    COMPLETED,
    [EnumMember(Value = "DROPPED")]
    DROPPED,
    [EnumMember(Value = "PAUSED")]
    PAUSED,
    [EnumMember(Value = "REPEATING")]
    REPEATING
}