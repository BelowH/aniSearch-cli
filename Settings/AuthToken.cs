using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace aniList_cli.Settings;

public class AuthToken
{

    private static string _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar +
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
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(_path,json);
        }
        catch (Exception)
        {
            // ignore
        }
    }

    /// <summary>
    ///  loads token data if File exists.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException">throws if file not found</exception>
    public static AuthToken Load()
    {
        if (!File.Exists(_path)) throw new FileNotFoundException("token.json not found");
        
        string json = File.ReadAllText(_path);
        AuthToken authToken = JsonConvert.DeserializeObject<AuthToken>(json);
        return authToken;
    }
    
    [JsonProperty(PropertyName = "token")]
    public string? Token { get; set; }


    [JsonProperty(PropertyName = "expireDate")]
    public DateTimeOffset? ExpireDate { get; set; }
}