using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers;
public class JwtSettings
{
    public const string SectionName = "JwtSettings";

    public string SecretKey { init; get; } = null!;
    public string Issuer { init; get; } = null!;
    public string Audience { init; get; } = null!;
    public int ExpiryInMinutes { init; get; }
    public int RefreshExpiryInMinutes { init; get; }
}