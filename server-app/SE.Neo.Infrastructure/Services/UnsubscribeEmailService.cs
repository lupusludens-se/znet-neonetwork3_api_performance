using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SE.Neo.Infrastructure.Services
{
    public class UnsubscribeEmailService : IUnsubscribeEmailService
    {
        private readonly ILogger<UnsubscribeEmailService> _logger;
        private readonly UnsubscribeSettingsConfig _unsubscribeSettingsConfig;


        public UnsubscribeEmailService(ILogger<UnsubscribeEmailService> logger, IOptions<UnsubscribeSettingsConfig> unsubscribeSettingsConfig)
        {
            _logger = logger;
            _unsubscribeSettingsConfig = unsubscribeSettingsConfig.Value;
        }

        public async Task<string> EncryptAsync(string clearText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = DeriveKeyFromKey(_unsubscribeSettingsConfig.SecretKey);
                aes.IV = DeriveKeyFromKey(_unsubscribeSettingsConfig.IVKey);

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream output = new())
                {
                    using (CryptoStream cs = new(output, encryptor, CryptoStreamMode.Write))
                    {
                        await cs.WriteAsync(Encoding.Unicode.GetBytes(clearText));
                        await cs.FlushFinalBlockAsync();

                        return Convert.ToBase64String(output.ToArray());
                    }
                }
            }
        }

        public async Task<string> DecryptAsync(byte[] encrypted)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = DeriveKeyFromKey(_unsubscribeSettingsConfig.SecretKey);
                aes.IV = DeriveKeyFromKey(_unsubscribeSettingsConfig.IVKey);

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream input = new(encrypted))
                {
                    using (CryptoStream cryptoStream = new(input, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream output = new())
                        {
                            await cryptoStream.CopyToAsync(output);
                            return Encoding.Unicode.GetString(output.ToArray());
                        }
                    }
                }
            }
        }

        private byte[] DeriveKeyFromKey(string key)
        {
            return Encoding.UTF8.GetBytes(key);
        }
    }
}