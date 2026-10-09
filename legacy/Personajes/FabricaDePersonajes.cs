using EspacioPersonajes;
using EspacioApi;
using System.Text.Json;
using System.Net;
using System.Linq;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
namespace EspacioFabricaDePersonajes
{
   

    public class FabricaDePersonajes
    {
        //tipo de personaje aleatorio
        private static T DecideTipo<T>() where T : Enum
        {
            Random random = new Random();

            T[] valores = (T[])Enum.GetValues(typeof(T));
            int indice = random.Next(valores.Length);

            return valores[indice];
        }

        //genera una fecha de nacimiento aleatoria
        private static DateTime FechaNac(object edad)
        {   
            //int currentYear = 724;
            Random random = new Random();
            DateTime start = new DateTime(700, 1, 1);
            DateTime end = new DateTime(705, 12, 31);
            int range = (end - start).Days;
            DateTime FecNac = start.AddDays(random.Next(range + 1));
            return FecNac;
        }

        private static string Genero(string genero)
        {
            if (genero == "Male")
            {
                return "Hombre";
            }else
            {
                return "Mujer";
            }
        }

        private static int Edad(object edad)
        {
            Random random = new Random();

            if (edad is int edadInt)
            {
                return edadInt;
            }
            else if (edad is string edadStr)
            {
                if (edadStr.ToLower() == "unknown")
                {
                    return random.Next(19, 36);
                }
                else if (int.TryParse(edadStr, out int edadParseada))
                {
                    return edadParseada;
                }
            }
            else if (edad is JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == JsonValueKind.String && jsonElement.GetString().ToLower() == "unknown")
                {
                    return random.Next(19, 36);
                }
                else if (jsonElement.ValueKind == JsonValueKind.Number && jsonElement.TryGetInt32(out int edadJson))
                {
                    return edadJson;
                }
                else if (jsonElement.ValueKind == JsonValueKind.String && int.TryParse(jsonElement.GetString(), out int edadParseada))
                {
                    return edadParseada;
                }
            }

            // Si no se puede determinar la edad, devolver un número aleatorio
            return random.Next(19, 36);
        }


        private static Personaje NuevoPersonaje(PersonajeDatos personaje)
        {
            Random random = new Random();
            int edad = Edad(personaje.age);

            var Nuevo = new Personaje
            {
                Nombre = personaje.name,
                Tipo = DecideTipo<TipoPersonaje>(),
                Edad = edad,
                FecNac = FechaNac(edad),
                Genero = Genero(personaje.gender),
                Salud = 100
            };
            if (Nuevo.Tipo == TipoPersonaje.Colosal)
            {
                Nuevo.Fuerza = random.Next(5, 8);       
                Nuevo.Velocidad = random.Next(3, 7);    
                Nuevo.Destreza = random.Next(2, 5);     
                Nuevo.Nivel = random.Next(1, 6);        
                Nuevo.Armadura = random.Next(2, 4);     
            }

            if (Nuevo.Tipo == TipoPersonaje.Acorazado)
            {
                Nuevo.Fuerza = random.Next(6, 9);       
                Nuevo.Velocidad = random.Next(4, 7);    
                Nuevo.Destreza = random.Next(2, 5);     
                Nuevo.Nivel = random.Next(1, 6);        
                Nuevo.Armadura = random.Next(4, 6);   
            }

            if (Nuevo.Tipo == TipoPersonaje.Ataque)
            {
                Nuevo.Fuerza = random.Next(6, 10);      
                Nuevo.Velocidad = random.Next(5, 8);   
                Nuevo.Destreza = random.Next(3, 6);     
                Nuevo.Nivel = random.Next(1, 6);       
                Nuevo.Armadura = random.Next(2, 4);   
            }

            if (Nuevo.Tipo == TipoPersonaje.Bestia)
            {
                Nuevo.Fuerza = random.Next(5, 8);    
                Nuevo.Velocidad = random.Next(4, 8);   
                Nuevo.Destreza = random.Next(2, 5);   
                Nuevo.Nivel = random.Next(1, 6);     
                Nuevo.Armadura = random.Next(5, 8);   
            }

            return Nuevo;
        }

    

        public static List<Personaje> CreacionPersonajes(List<Personaje> listaPersonajes, List<PersonajeDatos> personajeDatos)
        {   

            Random random = new Random();
            List<PersonajeDatos> DiezPersonajesAleatorios = personajeDatos.OrderBy(x => random.Next()).Take(10).ToList();
            foreach (var personaje in DiezPersonajesAleatorios)
            {
                Personaje personajeNuevo = NuevoPersonaje(personaje);
                listaPersonajes.Add(personajeNuevo);
            }
            return listaPersonajes;
        } 
        /* public static void mostrarPersonajes(List<Personaje> lista)
        {
            int i=0;
            foreach (var personaje in lista)
            {
                System.Console.WriteLine(i+ "Jugador de nombre: " + personaje.Nombre + ", Genero: " + personaje.Genero +", Edad: "+ personaje.Edad );
                System.Console.WriteLine("VEL.: " + personaje.Velocidad + " Destreza: " + personaje.Destreza + " Fuerza: " + personaje.Fuerza);
                System.Console.WriteLine("Nivel: " + personaje.Nivel + " Armadura: " + personaje.Armadura + " Salud: " + personaje.Salud);
                i++;
            }
        } */
    }
       

    

    
}
