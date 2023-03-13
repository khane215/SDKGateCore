using GateSDKs.sdk;
using Gosu.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GateSDKs.Models;
using Microsoft.Extensions.Logging;
using System.Configuration;


namespace GateSDKs.SDKGateService
{
    public class SDKService
    {
        private static string merchant_id = "1328";
        private static string secret = "3fbf7b1d2c373bb02422e5165dd32b8f";
        private static string password = "279020dc";
        private static string pathKeyFile = "1328.p12";
        //
        public string Provider { get; set; }
        public string UserName { get; set; }
        public string RequestID { get; set; }
        public string Serial { get; set; }
        public string Pin { get; set; }
        public string TelcoCode { get; set; }
        //
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpHelper _httpHelper;
        public IConfiguration Configuration { get; }
       
        public GateConfigModel gateConfigModel { get; set; }

        public SDKService(IHttpClientFactory HttpClientFactory,
                                        IConfiguration Config,
                                        string provider,
                                        string userName,
                                        string requestID,
                                        string serial,
                                        string pin,
                                        string telcoCode)
        {
            //param
            Provider = provider;
            UserName = userName;
            RequestID = requestID;
            Serial = serial;
            Pin = pin;
            TelcoCode = telcoCode;

            //http client factory
            _httpClientFactory = HttpClientFactory;
            //config
            Configuration = Config;
           
            //http helper
            if (_httpClientFactory == null)
            {
                //user http client per request
                _httpHelper = new HttpHelper();
            }
            else
            {
                //user http client factory
                _httpHelper = new HttpHelper("Common", _httpClientFactory);
            }
        }
        //
        public async Task<GATEPayResponse> XXX(string Provider,
                                        string UserName,
                                        string RequestID,
                                        string Serial,
                                        string Pin,
                                        string TelcoCode)

        {
            var response = new GATEPayResponse();
            //
            try
            {
                var request = new GATEPayRequest();
                request.Provider = Provider;
                request.UserName = UserName;
                request.RequestID = ((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds().ToString();
                request.Serial = Serial;
                request.Pin = Pin;
                request.TelcoCode = TelcoCode;
                //
                Dictionary<String, String> origins = new Dictionary<String, String>();
                //

                //string apiLink = string.Format("{0}?server_id={1}&uid={2}&time={3}&sign={4}", GetCharacterLink, request.serverId, request.accountId, request.time, request.sign);
                //              
                //var resCharacter = await _httpHelper.GetStringAsync(apiLink);
               // GATEPayResponse httpResponse = JsonConvert.DeserializeObject<GetCharacterList_HttpResponse>(resCharacter);
                //
                
            }
            catch (Exception ex)
            {
                //response.ReturnCode = 500;
               // response.MsgCode = ex.Message;
            }
            //Return
            return await Task.FromResult(response);
        }
        //
        public async Task<string> Sign(string payload)
        {
            string KEY_PASS = Configuration[string.Format("Gate:password")];
            string KEY_PATH = Configuration[string.Format("Gate:pathKeyFile")];

            try
            {
                SHA1 sha1 = SHA1.Create();
                byte[] data = Encoding.UTF8.GetBytes(payload);
                byte[] hash = sha1.ComputeHash(data);
                string base64Hash = Convert.ToBase64String(hash);
                RSACng rsaCng = new RSACng();
                RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider();
                rsaCryptoServiceProvider.ImportParameters(rsaCng.ExportParameters(true));
                //SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                X509Certificate2 x509Cert = new X509Certificate2(KEY_PATH, KEY_PASS, X509KeyStorageFlags.MachineKeySet);

                return Convert.ToBase64String(rsaCryptoServiceProvider.SignData(data, sha1));
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }
        public class GATEPayResponse
        {
            public int amount { get; set; }
            public string response_id { get; set; } = "";
            public int vendor_id { get; set; }
            public string description { get; set; } = "";
            public int request_id { get; set; }
        }
        public class GATEPayRequest
        {
            public string UserName { get; set; } = "";
            public string Provider { get; set; } = "";
            public string RequestID { get; set; } = "";
            public string Serial { get; set; } = "";
            public string Pin { get; set; } = "";
            public string TelcoCode { get; set; } = "";
        }
    }
}

