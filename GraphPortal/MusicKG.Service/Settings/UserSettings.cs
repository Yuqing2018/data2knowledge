using Microsoft.Extensions.Configuration;
using MusicKG.Service.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Settings
{
    public class UserSettings
    {
        public const string UserTokenExpiredInDaysKey = "User:TokenExpiredInDays";
        public const string UserMaxExistingValidLoginsPerUserKey = "User:MaxExistingValidLoginsPerUser";

        public const string IssuerKey = "User:Issuer";
        public const string AudienceKey = "User:Audience";

        public const int DefaultTokenExpiredInDays = 7;
        public const int DefaultMaxExistingValidLoginsPerUser = 255;

        public const string DefaultIssuer = "Issuer";
        public const string DefaultAudience = "Audience";

        /// <summary>
        /// How many days does token expire.
        /// </summary>
        public int TokenExpiredInDays { get; set; } = DefaultTokenExpiredInDays;

        /// <summary>
        /// Manximum existing valid token per user.
        /// </summary>
        public int MaxExistingValidLoginsPerUser { get; set; } = DefaultMaxExistingValidLoginsPerUser;

        /// <summary>
        /// Token issuer.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Token audience.
        /// </summary>
        public string Audience { get; set; }

        public void ParseFrom(IConfiguration configuration)
        {
            if (int.TryParse(configuration.GetSection(UserTokenExpiredInDaysKey)?.Value, out int tokenExpiredInDays))
                TokenExpiredInDays = tokenExpiredInDays;

            if (int.TryParse(configuration.GetSection(UserMaxExistingValidLoginsPerUserKey)?.Value, out int maxExistingValidLoginsPerUser))
                MaxExistingValidLoginsPerUser = maxExistingValidLoginsPerUser;

            Issuer = configuration.GetSection(IssuerKey)?.Value ?? DefaultIssuer;
            Audience = configuration.GetSection(AudienceKey)?.Value ?? DefaultAudience;
        }
    }
}
