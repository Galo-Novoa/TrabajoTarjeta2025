<<<<<<< Updated upstream
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaApp
{
    internal class Colectivo
    {
        private static readonly decimal precioPasaje = 1580m;
        public string linea;

        public Colectivo(string linea) { this.linea = linea; }

        public Boleto pagarCon(Tarjeta tarjeta) {
            if (tarjeta.cobrarPasaje(precioPasaje))
            {
                Boleto boleto = new Boleto(this.linea);
                return boleto;
            }
            else { return null; }
        }
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaApp
{
    internal class Colectivo
    {
        private static readonly decimal precioPasaje = 1580m;
        public string linea;

        public Colectivo(string linea) { this.linea = linea; }

        public Boleto pagarCon(Tarjeta tarjeta) {
            if (tarjeta.cobrarPasaje(precioPasaje))
            {
                Boleto boleto = new Boleto(this.linea);
                return boleto;
            }
            else { return null; }
        }
    }
}
>>>>>>> Stashed changes
