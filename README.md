# Torneo de Titanes (AOT AU)

[![C#](https://img.shields.io/badge/.NET_8-C%23-512BD4)](https://dotnet.microsoft.com/) [![GitHub](https://img.shields.io/badge/repo-tangerinegmv%2Ftorneo--de--titanes--game-181717?logo=github)](https://github.com/tangerinegmv/torneo-de-titanes-game)

RPG de combate por turnos ambientado en un universo alternativo de *Attack on Titan*: eliges tu Titán, enfrentas a rivales en un torneo y desatas técnicas definitivas hasta coronarte como **Titán Primordial**.

El proyecto fue refactorizado a una **arquitectura limpia en capas** sobre .NET 8, con una interfaz interactiva construida con [Spectre.Console](https://spectreconsole.net/), inyección de dependencias, patrón *Observer*/*Strategy*, cliente API tipado y 48 pruebas unitarias.

---

## Características del Sistema de Batalla

Sistema de combate por turnos **interactivo y dinámico**:

- **Furia y Técnicas Definitivas**: cada Titán carga un medidor de furia al atacar y recibir daño. Al completarlo puede desatar una habilidad definitiva exclusiva.
- **Cooldowns**: las habilidades especiales quedan recargando tras usarse, lo que obliga a planificar turnos.
- **Energía**: ataques y habilidades consumen energía; defenderse la recupera.
- **Escudo y Postura Defensiva**: defender otorga escudo, reduce el daño y **contraataca** al agresor.
- **Estados alterados**: sangrado, veneno, quemadura, regeneración, aturdimiento, fortificación y debilidad, cada uno con duración y efectos propios.
- **Esquivas y golpes críticos**: dependen de la velocidad, la destreza y la variación del azar.
- **4 clases de Titán** (Colosal, Acorazado, Bestia, Ataque), cada una con atributos, una habilidad especial y una técnica definitiva.
- **IA adversaria**: el rival decide sus acciones según su salud, la del oponente, su energía y su furia.
- **Progresión**: cada victoria mejora las estadísticas del campeón según su tipo de Titán.
- **Rendirse con penalización**: abandonar el combate es posible, pero debilita (de forma persistente) las estadísticas de tu Titán.
- **Eliminación temprana**: si tu Titán pierde o se rinde, el torneo termina y el vencedor de la ronda queda registrado como campeón.
- **Navegación flexible**: al elegir tu Titán puedes volver al menú principal.
- **Interfaz viva**: barras de salud/energía/furia, estados visibles, registro de eventos coloreado y menús de combate animados.

---

## Arquitectura

Solución con separación de responsabilidades (*Clean Architecture*):

```
TorneoDeTitanes.sln
├── src/
│   ├── TorneoDeTitanes.Core/       # Dominio: modelos, combate, habilidades, reglas (sin dependencias de UI)
│   └── TorneoDeTitanes.Console/    # Presentación + composición: Spectre.Console, DI, repositorios JSON, API
├── tests/
│   └── TorneoDeTitanes.Core.Tests/ # xUnit: motor de combate, daño, habilidades, fábrica, dominio
└── legacy/                         # Prototipo monolítico original del taller (fuera de la solución)
```

### Patrones y prácticas

| Práctica | Dónde |
| --- | --- |
| **Layered / Clean Architecture** | `Core` (dominio puro) vs `Console` (presentación e infraestructura) |
| **Dependency Injection** | `ComposicionDeDependencias` registra servicios del host (última versión de `IServiceCollection`) |
| **Options Pattern** | `OpcionesJuego` se enlaza con la sección `Juego` de `appsettings.json` |
| **Strategy Pattern** | `ISelectorAccion` (jugador humano e IA) y `ICalculadoraDano` intercambiables |
| **Observer Pattern** | `ICombateObserver` desacopla el motor de la presentación del usuario |
| **Typed `HttpClient` + `IHttpClientFactory`** | `AttackOnTitanApiClient` consume la API pública |
| **Repositorios JSON** | `IPersonajeRepositorio`, `IHistorialRepositorio` (persistencia desconectada) |
| **Pruebas unitarias aisladas** | dobles (`IRandomProvider`, selectores y observadores) para resultados deterministas |
| **Nullable + Analizadores .NET (warnings = errors)** | `Directory.Build.props` con `TreatWarningsAsErrors` |

---

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (o superior)

## Cómo ejecutar

```bash
git clone https://github.com/tangerinegmv/torneo-de-titanes-game
cd torneo-de-titanes-game

dotnet restore
dotnet run --project src/TorneoDeTitanes.Console
```

## Cómo ejecutar las pruebas

```bash
dotnet test
```

El motor de combate es determinista bajo prueba: se inyectan generadores de números aleatorios controlados para validar daño, críticos, esquivas, estados, cooldowns y ultimates.

## Configuración

Toda la configuración vive en [`src/TorneoDeTitanes.Console/appsettings.json`](src/TorneoDeTitanes.Console/appsettings.json):

```jsonc
{
  "Juego": {
    "RutaPersonajes": "Data/Personajes.json",   // roster persistido
    "RutaHistorial": "Data/Historial.json",      // campeones del torneo
    "ApiUrl": "https://api.attackontitanapi.com/characters/1,2,...",
    "CantidadPersonajes": 10,                    // tamaño del roster
    "RondasTorneo": 9,                           // rondas del torneo
    "MaximoHistorial": 15,                       // registros conservados
    "VelocidadAnimacionMs": 500                  // pausa entre eventos
  }
}
```

---

## Uso de la API

El juego consume la [API pública de Attack on Titan](https://www.attackontitanapi.com/) para poblar el roster con personajes del anime. Si la API no responde, se usan **personajes de respaldo** incluidos en el proyecto, por lo que el juego siempre arranca.

La API devuelve, entre otros campos: `id`, `name`, `img`, `alias`, `species`, `gender`, `age`, `height`, `status`, `occupation`, `groups` y `roles`. Estos datos se adaptan en `FabricaDePersonajes` a las estadísticas de combate del juego (fuerza, velocidad, destreza, armadura, tipo de Titán y nivel).

---

## Roadmap

- [x] Motor de combate interactivo con furia, ultimates y cooldowns
- [x] Rendirse con penalización de estadísticas y eliminación temprana del torneo
- [x] Arquitectura limpia + DI + Options + `IHttpClientFactory`
- [x] Suite de pruebas unitarias
- [x] Analizadores .NET con warnings como errores
- [x] Coeficientes de cobertura (`coverlet`, integrado en CI)
- [x] CI con GitHub Actions (`build` + `test` + cobertura)
- [ ] Distribución *self-contained*
- [ ] Modo historia con rivales únicos por ronda

## Contacto

- **Correo**: gimenezmariavictoria@gmail.com
- **GitHub**: [tangerinegmv](https://github.com/tangerinegmv)

> El **código original del taller** (prototipo monolítico) se conserva en [`legacy/`](legacy); el código principal vive bajo `src/`.