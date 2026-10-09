using EspacioMenuPrincipal;
using MensajesPorPantalla;

namespace BatallaDeTitanes
{
    public static class Programa
    {
       public static async Task Main(string[] args)
       {
            Mensajes.MostrarIntroduccion();
            await MenuPrincipal.MostrarMenu();
            
       }
       }
}

