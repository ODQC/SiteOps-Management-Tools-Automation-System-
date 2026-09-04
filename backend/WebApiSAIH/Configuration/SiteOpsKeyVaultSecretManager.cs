using System;
using System.Collections.Generic;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;

namespace WebApiSAIH.Configuration
{
    // Maps flat Key Vault secret names (which can't contain the "_" used by ConfiguracionJWT:JWT_Secret)
    // to the hierarchical configuration keys the app already reads. Only whitelisted secrets are loaded,
    // so unrelated vault entries (e.g. the SC-500 lab's "OscarUser" test secret) are ignored.
    public class SiteOpsKeyVaultSecretManager : KeyVaultSecretManager
    {
        private static readonly Dictionary<string, string> SecretNameToConfigKey = new(StringComparer.OrdinalIgnoreCase)
        {
            ["ProdConnectionString"] = "ConnectionStrings:ProdConnection",
            ["JwtSecret"] = "ConfiguracionJWT:JWT_Secret",
            ["SendGridApiKey"] = "SendGridAPIKey",
        };

        public override bool Load(SecretProperties secret) => SecretNameToConfigKey.ContainsKey(secret.Name);

        public override string GetKey(KeyVaultSecret secret) => SecretNameToConfigKey[secret.Name];
    }
}
