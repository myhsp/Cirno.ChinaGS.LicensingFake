using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirno.ChinaGS.LicensingFake
{
    public class CryptoHelper
    {
        public string xmlStr;

        public CryptoHelper()
        {
            this.xmlStr = this.ReadLic();
        }

        public CryptoHelper(string filename)
        {
            if (File.Exists(filename))
            {
                this.xmlStr = new StreamReader(filename, Encoding.Default).ReadToEnd();
            }
            else
            {
                this.xmlStr = this.ReadLic();
            }
        }
        
        public string ReadLic()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream manifestResourceStream = assembly.GetManifestResourceStream("Cirno.ChinaGS.LicensingFake.Resources.License.lic");
            StreamReader streamReader = new StreamReader(manifestResourceStream);

            return streamReader.ReadToEnd();
        }

        public string Encrypt(string addonSymbolicName)
        {
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(this.xmlStr);
                RSAPKCS1SignatureFormatter formatter = new RSAPKCS1SignatureFormatter(provider);
                formatter.SetHashAlgorithm("SHA1");

                SHA1Managed sha1 = new SHA1Managed();
                byte[] rgbHash = sha1.ComputeHash(Encoding.ASCII.GetBytes(addonSymbolicName));

                byte[] signature = formatter.CreateSignature(rgbHash);
                string b64str = Convert.ToBase64String(signature);

                return b64str;
            }
        }

        public bool Verify(string addonSymbolicName, string crypted)
        {
            try
            {
                using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
                {
                    provider.FromXmlString(this.xmlStr);
                    RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(provider);
                    deformatter.SetHashAlgorithm("SHA1");

                    SHA1Managed sha1 = new SHA1Managed();
                    byte[] rgbHash = sha1.ComputeHash(Encoding.ASCII.GetBytes(addonSymbolicName));
                    return deformatter.VerifySignature(rgbHash, Convert.FromBase64String(crypted));
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
