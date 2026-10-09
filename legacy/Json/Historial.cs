using EspacioPersonajes;
using System.Runtime.CompilerServices;
using System.Text.Json;
namespace EspacioHistorial
{
    public class HistorialJson
    {
        public Personaje Ganador {get; set;}
        public DateTime Hora {get; set;}

        private static string nombreArchivo = "Json/Historial.json";
        public static void CargarDatos(Personaje ganador, List<HistorialJson> listaHistorial)
        {
            
            DateTime horaActual = DateTime.Now;
            // Ajusto el año al 724
            DateTime fechaAjustada = new DateTime(724, horaActual.Month, horaActual.Day, 
                                                    horaActual.Hour, horaActual.Minute, horaActual.Second);
            HistorialJson datos = new HistorialJson(ganador, fechaAjustada);
            
            listaHistorial.Add(datos);
             // Guardo solo los 15 ultimos
            while (listaHistorial.Count > 15)
            {
                listaHistorial.RemoveAt(0);
            }
            GuardarGanador(listaHistorial);
        }
        private static void GuardarGanador(List<HistorialJson> listaHistorial)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(listaHistorial, options);
            File.WriteAllText(nombreArchivo, json);

        }
        public static List<HistorialJson> LeerGanadores()
        {
            if (!Existe(nombreArchivo))
            {
                return new List<HistorialJson>();
            }
            string json = File.ReadAllText(nombreArchivo);
            return JsonSerializer.Deserialize<List<HistorialJson>>(json);
        }

        private static bool Existe(string fileName)
        {
            return File.Exists(fileName);
        }
        public HistorialJson(Personaje ganador, DateTime hora)
        {
            Ganador = ganador;
            Hora = hora;
        }
    }
}