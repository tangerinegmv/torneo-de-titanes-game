using EspacioHistorial;
using EspacioPersonajes;
using MensajesPorPantalla;
using EspacioAsciiArt;
using FinJuego;
using EspacioAyuda;

namespace EspacioPelea
{
    public class Pelea
    {
        public static Random random = new Random();

        public static async Task InicioCombate(List<Personaje> listaPersonajes, Personaje PersonajeElegido)
        {
            
            Ayuda.ExplicacionDePelea(PersonajeElegido);
            int ronda = 0;
            var Ganador = new Personaje();
            AsciiArt.MsjeComienzo();

            await Task.Delay(1000);

            Ganador = Combate(listaPersonajes, PersonajeElegido, ronda, Ganador);
            
            AsciiArt.MsjeFin();
            Console.WriteLine("Presione una tecla para continuar...");
            Console.ReadKey(intercept: true);
            Console.Clear();
            await FinalDeJuego.Final(Ganador);
            

            
        }

        private static Personaje Combate(List<Personaje> listaPersonajes, Personaje PersonajeElegido, int ronda, Personaje Ganador)
        {
            while (listaPersonajes.Count > 0)
            {
                PersonajeElegido.Salud = 100;
                if (ronda == 9) break;
                 Mensajes.MostrarMensajeRonda(ronda+1);  

                int IndiceContrincante = random.Next(0, listaPersonajes.Count);
                var Contrincante = listaPersonajes[IndiceContrincante];
                listaPersonajes.Remove(Contrincante);

                Console.WriteLine($"{PersonajeElegido.Nombre} (TITÁN {PersonajeElegido.Tipo}) VS {Contrincante.Nombre} (TITÁN {Contrincante.Tipo})");
                Console.WriteLine("==============================================================================================================");
                
                Ganador = DinamicaDePelea(PersonajeElegido, Contrincante);

                Console.WriteLine();
                Console.WriteLine(Ganador.Nombre.ToUpper() + " GANA ESTA RONDA!");
                MejorarEstadisticas(Ganador);
                PersonajeElegido = Ganador;
               
                if (ronda != 8)
                {
                    Console.WriteLine("\nPresione una tecla para jugar la siguiente ronda...");
                    Console.ReadKey(intercept: true);
                }else
                {
                    Task.Delay(500);
                }
                
                

                ronda++;
            }

            return Ganador;
        }
    

        private static Personaje DinamicaDePelea(Personaje prota, Personaje contrincante)
        {
            int turno = random.Next(0, 2);
            bool primeraVuelta = true;

            while (prota.Salud > 0 && contrincante.Salud > 0)
            {
                if (turno == 1)
                {
                    if (primeraVuelta)
                    {
                        Console.WriteLine($"COMIENZA ATACANDO: {prota.Nombre}");
                    }
                    Ataque(prota, contrincante);
                    turno = 0;
                }
                else
                {
                    if (primeraVuelta)
                    {
                        Console.WriteLine($"COMIENZA ATACANDO: {contrincante.Nombre}");
                    }
                    Ataque(contrincante, prota);
                    turno = 1;
                }
                primeraVuelta = false;
            }

            return prota.Salud > 0 ? prota : contrincante;
        }

        private static void Ataque(Personaje ataca, Personaje defiende)
        {
            int daño = Daño(ataca);
            defiende.Salud = defiende.Salud - daño;
            if (defiende.Salud < 0)
            {
                defiende.Salud = 0;
            }
            Console.WriteLine($"\nEl Titán {ataca.Tipo} ({ataca.Nombre}) ataca al Titán {defiende.Tipo} ({defiende.Nombre}) y causa {daño} puntos de daño. ");
            Console.WriteLine($"Salud restante de {defiende.Nombre}: {defiende.Salud}");
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
        }

        private static int Daño(Personaje personaje)
        {
            double Ataque = (personaje.Destreza) * (personaje.Fuerza) * (personaje.Nivel);
            double Efectividad = random.Next(1, 101);
            double Defensa = (personaje.Armadura) * (personaje.Velocidad);
            double CteDeAjuste = 50.0;
            double dañoProvocado = ((Ataque * Efectividad) - Defensa) / CteDeAjuste;
            if (dañoProvocado>100)
            {
                dañoProvocado =random.Next(80,100);
            }
            if (dañoProvocado<0)
            {
                dañoProvocado=0;
            }
            return (int)dañoProvocado;
        }

        private static void MejorarEstadisticas(Personaje personaje)
        {
            // Mejora de estadísticas por tipo de personaje
            switch (personaje.Tipo)
            {
                case TipoPersonaje.Colosal:
                    personaje.Fuerza += random.Next(1, 3);
                    personaje.Armadura = Math.Min(personaje.Armadura + random.Next(1, 3), 10);
                    break;

                case TipoPersonaje.Acorazado:
                    personaje.Armadura = Math.Min(personaje.Armadura + random.Next(2, 4), 10);
                    personaje.Fuerza += random.Next(1, 2);
                    break;

                case TipoPersonaje.Ataque:
                    personaje.Fuerza += random.Next(1, 3);
                    personaje.Velocidad += random.Next(1, 3);
                    personaje.Armadura = Math.Min(personaje.Armadura + random.Next(1, 2), 10);
                    break;

                case TipoPersonaje.Bestia:
                    personaje.Armadura = Math.Min(personaje.Armadura + random.Next(2, 4), 10);
                    personaje.Fuerza += random.Next(1, 3);
                    break;
            }

            
            personaje.Nivel = Math.Min(personaje.Nivel + 1, 5);

            Console.WriteLine($"{personaje.Nombre} ha mejorado sus estadísticas tras la victoria!");
            
        }

    }
} 

