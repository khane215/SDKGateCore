using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace GateSDKs.utils
{
    public class SignedRequest
    {
        public static String TAG = "SignedRequest";
        protected Session session;
        protected String encodeRawSignedRequest;

        public SignedRequest(Session session)
        {
            this.session = session;
        }

        public String sign(String payload)
        {
            String KEY_PASS = this.getSession().getAppPassword();
            String KEY_PATH = this.getSession().getAppKeyFile();

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

        public String make(Dictionary<String, String> parameters)
        {
            String rawSignRequest = JsonSerializer.Serialize(parameters);
            this.encodeRawSignedRequest = Utility.Base64Encode(rawSignRequest);
            return this.encodeRawSignedRequest;
        }

        public String getEncodeRawSignedRequest()
        {
            return this.encodeRawSignedRequest;
        }

        public String getToken()
        {
            return Utility.md5(this.encodeRawSignedRequest);
        }

        public Session getSession()
        {
            return session;
        }
    }
}
