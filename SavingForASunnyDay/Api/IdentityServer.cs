using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SavingForASunnyDay.Api
{
    public class IdentityServerToken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
    }

    public static class IdentityServer
    {
        public static async Task<IdentityServerToken> GetToken(string userid, string clientid, string secret, string host)
        {
            var identityServerUrl = $"https://{host}/IdentityServer/connect/token";
            var credentials = $"{clientid}:{secret}";
            var bytes = System.Text.Encoding.UTF8.GetBytes(credentials);
            var encodedText = System.Convert.ToBase64String(bytes);

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded; charset=utf-8";
                wc.Headers[HttpRequestHeader.Authorization] = $"Basic {encodedText}";
                wc.Headers[HttpRequestHeader.Accept] = "application/json";
                string HtmlResult = await wc.UploadStringTaskAsync(identityServerUrl, "grant_type=client_credentials");
                return JsonConvert.DeserializeObject<IdentityServerToken>(HtmlResult);
            }
        }
    }
}