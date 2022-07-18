namespace aniList_cli.Repository.Models;

public enum MediaSeason
{
    WINTER,
    SPRING,
    SUMMER,
    FALL
}

public enum MediaStatus
{
    FINISHED,
    RELEASING,
    NOT_YET_RELEASED,
    CANCELLED,
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

public enum MediaListStatus
{
    CURRENT,
    PLANNING,
    COMPLETED,
    DROPPED,
    PAUSED,
    REPEATING
}