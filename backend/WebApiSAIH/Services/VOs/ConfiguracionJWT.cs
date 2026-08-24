using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAIH_Backend.Servicios.Autenticacion
{
    public class ConfiguracionJWT
    {
        public string JWT_Secret { get; set; }
        public string Client_URL { get; set; }
    }
}
