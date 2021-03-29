using JWTSandbox.Authentication.API.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace JWTSandbox.Authentication.API.Services.Audiences
{
    public interface IAudienceService
    {
        Audience FindAudienceById(string audienceId);
        Audience FindAudienceByName(string audienceName);
    }

    public class AudienceService : IAudienceService
    {
        public static string AUDIENCE_1_ID = "099153c2625149bc8ecb3e85e03f0022";

        private List<Audience> _audiencesList = new List<Audience>();

        public AudienceService()
        {
            //fetch this from the db
            _audiencesList.Add(new Audience
                                {
                                    AudienceId = AUDIENCE_1_ID,
                                    AudienceSecret = "WFgz3M7NGvMBFMtNyI6ggQ==",
                                    Name = "JWTSandbox.Audience.Api"
                                });
        }

        public Audience FindAudienceById(string audienceId)
        {
            return _audiencesList.FirstOrDefault(x => x.AudienceId == audienceId);
        }

        public Audience FindAudienceByName(string audienceName)
        {
            return _audiencesList.FirstOrDefault(x => x.Name == audienceName);
        }

        //public static Audience AddAudience(string name)
        //{
        //    var clientId = Guid.NewGuid().ToString("N");

        //    var key = new byte[32];
        //    RNGCryptoServiceProvider.Create().GetBytes(key);
        //    var base64Secret = TextEncodings.Base64Url.Encode(key);

        //    Audience newAudience = new Audience { ClientId = clientId, Base64Secret = base64Secret, Name = name };
        //    AudiencesList.TryAdd(clientId, newAudience);
        //    return newAudience;
        //}
    }
}
