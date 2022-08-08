using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace aniList_cli.Settings;

public class AuthToken
{

    private static string _path = Path.GetDirectoryName(AppContext.BaseDirectory) + Path.DirectorySeparatorChar +
                                 "Config" + Path.DirectorySeparatorChar + "token.json";
    
    public AuthToken()
    {
        
    }
    
    public AuthToken(JwtSecurityToken token)
    {
        Token = token.RawData;
        ExpireDate = DateTimeOffset.FromUnixTimeSeconds((long)token.Payload.Exp!);
    }
    
    /// <summary>
    ///  write token value and expireDate to file.
    /// </summary>
    public void Save()
    {
        try
        {
            string json = JsonSerializer.Serialize(this);
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            File.WriteAllText(_path,json);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    ///  loads token data if File exists.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException">throws if file not found or deserialized file was null</exception>
    public static AuthToken Load()
    {
        if (!File.Exists(_path)) throw new FileNotFoundException("token.json not found");
        
        string json = File.ReadAllText(_path);
        AuthToken? authToken = JsonSerializer.Deserialize<AuthToken>(json);
        if (authToken == null)
        {
            throw new FileNotFoundException("token.json could not be parsed. (was null)");
        }
        return authToken;
    }
    
    [JsonPropertyName("token")]
    public string? Token { get; set; }
    
    [JsonPropertyName("expireDate")]
    public DateTimeOffset? ExpireDate { get; set; }
}