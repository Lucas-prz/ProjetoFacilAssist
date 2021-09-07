using Microsoft.IdentityModel.Tokens;

namespace WS_AcessoDB.ApiService
{
    public class AcessoConfig
    {
        public SecurityKey ChaveAcesso { get; }
        public SigningCredentials CredenciasAcesso  { get; }

        public AcessoConfig()
        {
            CredenciasAcesso = new SigningCredentials(ChaveAcesso, SecurityAlgorithms.HmacSha256);
        }
    }
}
