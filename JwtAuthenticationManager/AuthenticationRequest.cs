using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthenticationManager
{
    public class AuthenticationRequest
    {
        public string Carnet { get; set; }
        public string Clave { get; set; }
    }
}
