using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthenticationManager.Models
{
    public class UserAccount
    {
        public UserAccount(string carnet, string clave, int role, string nombre)
        {
            Carnet = carnet;
            Clave = clave;
            Role = role;
            Nombre = nombre;
        }

        public string Carnet { get; set; }
        public string Clave { get; set; }
        public int Role { get; set; }
        public string Nombre { get; set; }
    }
}
