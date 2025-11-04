using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarjetaApp;

namespace TarjetaTest
{
    internal class Tests4
    {
        [Test]
        public void Interurbano_Cobra_3000_Pesos()
        {
            var tarjeta = new Tarjeta(5000m);
            var interurbano = new Interurbano("I1");

            Assert.That(interurbano.PagarCon(tarjeta), Is.True);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(2000m)); // 5000 - 3000 = 2000
        }

        [Test]
        public void Interurbano_Con_MedioEstudiantil_Cobra_Mitad()
        {
            var tarjeta = new MedioEstudiantil(5000m);
            var interurbano = new Interurbano("I1");

            Assert.That(interurbano.PagarCon(tarjeta), Is.True);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(3500m)); // 5000 - (3000 * 0.5) = 3500
        }

        [Test]
        public void Interurbano_Con_Jubilados_Primeros_Viajes_Gratis()
        {
            var tarjeta = new Jubilados(5000m);
            var interurbano = new Interurbano("I1");

            // Primer viaje gratis
            Assert.That(interurbano.PagarCon(tarjeta), Is.True);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(5000m));

            // Segundo viaje gratis  
            Assert.That(interurbano.PagarCon(tarjeta), Is.True);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(5000m));

            // Tercer viaje paga completo
            Assert.That(interurbano.PagarCon(tarjeta), Is.True);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(2000m)); // 5000 - 3000 = 2000
        }

        [Test]
        public void Boleto_Interurbano_Muestra_Precio_Correcto()
        {
            var tiempo = new TiempoFalso();
            var tarjeta = new Tarjeta(5000m, tiempo);
            var interurbano = new Interurbano("I1", tiempo);

            interurbano.PagarCon(tarjeta);
            var boleto = tarjeta.GetHistorialViajes()[0];

            Assert.That(boleto.GetMonto(), Is.EqualTo(3000m)); // Debe mostrar 3000, no 1580
        }
    }
}
