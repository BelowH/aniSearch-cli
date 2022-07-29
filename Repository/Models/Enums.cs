using System.Runtime.Serialization;
using System.Text.Json.Serialization;


namespace aniList_cli.Repository.Models;

public enum MediaSeason
{
    WINTER,
    SPRING,
    SUMMER,
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

public enum MediaFormat
{
    TV,
    TV_SHORT,
    MOVIE,
    SPECIAL,
    OVA,
    ONA,
    MUSIC,
    MANGA,
    NOVEL,
    ONE_SHOT
}

public enum MediaType
{
    ANIME,
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