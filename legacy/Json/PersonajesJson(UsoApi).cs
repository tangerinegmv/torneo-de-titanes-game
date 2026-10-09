using EspacioPersonajes;
using EspacioApi;
using EspacioFabricaDePersonajes;
using System.Text.Json;
using Microsoft.VisualBasic;

namespace EspacioJson
{
    public class CargarDatos
    {
        public static async Task CargandoPersonajes()
        {
            await CargarDatosPersonajesAsync();
        }

        private static async Task CargarDatosPersonajesAsync()
        {
           
            List<PersonajeDatos> listaPersonajesApi = new List<PersonajeDatos>();
            List<Personaje> listaPersonajes = new List<Personaje>();

            listaPersonajesApi = await TraerInfoAPI(listaPersonajesApi);
            listaPersonajes = FabricaDePersonajes.CreacionPersonajes(listaPersonajes, listaPersonajesApi);
            PersonajesJson.GuardarPersonajes(listaPersonajes, "Json/Personajes.json");
        }

        private static async Task<List<PersonajeDatos>> TraerInfoAPI(List<PersonajeDatos> listaPersonajes) 
        {
            try
            {
                HttpClient cliente = new HttpClient();
                var url = "https://api.attackontitanapi.com/characters/1,2,3,4,5,8,10,12,57,66,67,74,86,87,88,89,90,91,95,101,184,193,188";
                HttpResponseMessage respuesta = await cliente.GetAsync(url);
                respuesta.EnsureSuccessStatusCode();

                string responseBody = await respuesta.Content.ReadAsStringAsync();
                var contenidoApi = JsonSerializer.Deserialize<List<PersonajeDatos>>(responseBody, new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
    
                if (contenidoApi != null)
                {
                    foreach (var personaje in contenidoApi)
                    {
                        if (personaje != null)
                        {
                            listaPersonajes.Add(personaje);
                        }
                        
                    
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return null;
            }

            return listaPersonajes;
        }

    }
    public class PersonajesJson{
        public static void GuardarPersonajes(List<Personaje> Lista, string fileName)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(Lista, options);
            File.WriteAllText(fileName,json);
        }
        public static List<Personaje> LeerPersonajes(string fileName)
        {
            if (File.Exists(fileName))
            {
                String jsonString = File.ReadAllText(fileName);
                List<Personaje> personajesDeserializados = JsonSerializer.Deserialize<List<Personaje>>(jsonString);
                return personajesDeserializados;
            }else
            {
                System.Console.WriteLine("El archivo de nombre: " + fileName + " no existe");
                return null;
            }
        }
         public static bool Existe(string fileName)
        {
            return File.Exists(fileName);
        }

       public static async Task<List<Personaje>> RecargaDePersonajes(string fileName)
        {
            List<Personaje> listaPersonajes = null;

            if (Existe(fileName))
            {
                ConsoleKeyInfo tecla;
                char answer;
                bool control;

                Console.WriteLine("Existe una lista de personajes precargada, quieres usarla?(S/N)");

                do
                {
                    tecla = Console.ReadKey(intercept: true);
                    answer = tecla.KeyChar;
                    control = true;

                    if (answer == 'N' || answer == 'n')
                    {
                        Console.WriteLine("Se borrará la lista y se creará una nueva.");
                        File.Delete(fileName);
                        Console.WriteLine("Creando nuevos personajes...");
                        await CargarDatos.CargandoPersonajes();
                        listaPersonajes = PersonajesJson.LeerPersonajes(fileName);
                    }
                    else if (answer == 'S' || answer == 's')
                    {
                        Console.WriteLine("Se usará la lista de personajes precargada.");
                        await Task.Delay(3000);
                        listaPersonajes = PersonajesJson.LeerPersonajes(fileName);
                    }
                    else
                    {
                        Console.WriteLine("Ingrese S o N");
                        control = false;
                    }
                } while (!control);
            }

            return listaPersonajes;
        }

    }
        

        

       
/* 
        public void mostrarPersonajes(List<Personaje> lista)
        {
            foreach (var personaje in lista)
            {
                System.Console.WriteLine("Jugador de nombre: " + personaje.Nombre + ", Apodo: " + personaje.Apodo +", Edad: "+ personaje.Edad );
                System.Console.WriteLine("VEL.: " + personaje.Velocidad + " Destreza: " + personaje.Destreza + " Fuerza: " + personaje.Fuerza);
                System.Console.WriteLine("Nivel: " + personaje.Nivel + " Armadura: " + personaje.Armadura + " Salud: " + personaje.Salud);
            }
        } */

    }
