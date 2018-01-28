using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace SavingForASunnyDay.Api
{
    public class Account
    {
        public string accountNumber { get; set; }
        public string customerId { get; set; }
        public string ownerCustomerId { get; set; }
        public string name { get; set; }
        public string accountType { get; set; }
        public decimal available { get; set; }
        public decimal balance { get; set; }
        public decimal creditLimit { get; set; }
        public bool defaultAccount { get; set; }
    }

    public class TransferRequest
    {
        public string fromAccount { get; set; }
        public string toAccount { get; set; }
        public string message { get; set; }
        public decimal amount { get; set; }
    }

    public class Bank
    {
        private readonly string _userId;
        private readonly string _clientId;
        private readonly string _secret;
        private readonly string _hostName;

        public Bank(string userId, string clientId, string secret, string hostName)
        {
            _userId = userId;
            _clientId = clientId;
            _secret = secret;
            _hostName = hostName;
        }

        public async Task<List<Account>> GetAccounts()
        {
            var accountUrl = $"https://{_hostName}/Bank/Api/v1/Accounts/{_userId}";
            var token = await IdentityServer.GetToken(_userId, _clientId, _secret, _hostName);

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.Authorization] = $"Bearer {token.access_token}";
                wc.Headers[HttpRequestHeader.Accept] = "application/json";
                string result = await wc.DownloadStringTaskAsync(accountUrl);
                var list = JsonConvert.DeserializeObject<ListResult<Account>>(result);
                return list.items;
            }
        }

        public async Task<Account> GetAccount(string accountNumber)
        {
            var accountUrl = $"https://{_hostName}/Bank/Api/v1/Accounts/{_userId}/{accountNumber}";
            var token = await IdentityServer.GetToken(_userId, _clientId, _secret, _hostName);

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.Authorization] = $"Bearer {token.access_token}";
                wc.Headers[HttpRequestHeader.Accept] = "application/json";
                string result = await wc.DownloadStringTaskAsync(accountUrl);
                var list = JsonConvert.DeserializeObject<ItemResult<Account>>(result);
                return list.item;
            }
        }

        public async Task<decimal> Transfer(TransferRequest request)
        {
            var transferUrl = $"https://{_hostName}/Bank/Api/v1/Transfers/{_userId}";
            var token = await IdentityServer.GetToken(_userId, _clientId, _secret, _hostName);

            var json = JsonConvert.SerializeObject(request);
            var encodedValue = System.Text.Encoding.UTF8.GetBytes(json);

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.Authorization] = $"Bearer {token.access_token}";
                wc.Headers[HttpRequestHeader.Accept] = "application/json";
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                var result = await wc.UploadDataTaskAsync(transferUrl, encodedValue);
            }

            var account = await GetAccount(request.fromAccount);
            return account.available;
        }
    }
}
