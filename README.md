# Primer Examen — Sistema de Reservas de Restaurante

**Estudiante:** David González
**Carnet:** C23740
**Curso:** Lenguajes para Aplicaciones Comerciales — Universidad de Costa Rica
**Repositorio:** [`C23740-PrimerExamen`](https://github.com/1KATZUNO/C23740-PrimerExamen)

Aplicación web ASP.NET Core MVC (.NET 10) para gestionar las reservas de mesas de un restaurante. El diseño respeta la separación de responsabilidades en cuatro capas (`Model`, `DA`, `BL`, `UI`); **toda la lógica y los cálculos viven en la capa BL** y la UI únicamente delega y presenta.

## Arquitectura

```
C23740-PrimerExamen.slnx
├── Restaurante.Model           Entidad Mesa y enum EstadoMesa
├── Restaurante.DA              AppDbContext (EF Core + SQL Server)
│                               + IMesaRepository / MesaRepository
│                               + DependencyInjection.AddDataAccess()
├── C23740-PrimerExamen (BL)    IMesaService / MesaService
│                               · CalcularDepositoGarantia (regla del enunciado)
│                               · Toda la lógica de negocio
└── Restaurante.UI              ASP.NET Core MVC
    ├── Controllers             Mesas, MesasDisponibles, MesasReservadas
    ├── Models                  ReservaViewModel, ErrorViewModel
    └── Views/                  Razor + Bootstrap 5
```

> La conexión a la base de datos vive en **Restaurante.DA**. La UI sólo registra `services.AddDataAccess(connectionString)` y `services.AddBusinessLogic()`.

## Base de datos

- Motor: **SQL Server Express** (`.\SQLEXPRESS`).
- Base de datos: **`ExamenRestaurante`**.
- Tabla: **`dbo.Mesas`** (script provisto en [`script_sql/01_CreacionTablaMesas.sql`](script_sql/01_CreacionTablaMesas.sql)).

⚠️ Según el enunciado, el diseño de la base de datos **no debe modificarse** por la aplicación. Por eso el proyecto NO usa migraciones, NO ejecuta `EnsureCreated()`, NO siembra registros: solo consume el esquema existente.

### Antes de ejecutar

1. Asegurarse de que el servicio `SQL Server (SQLEXPRESS)` esté **Running** (SQL Server Configuration Manager).
2. Crear la base `ExamenRestaurante` y ejecutar `script_sql/01_CreacionTablaMesas.sql` (o usar SSMS / Azure Data Studio).
3. Ajustar la cadena de conexión en [`Restaurante.UI/appsettings.json`](Restaurante.UI/appsettings.json) si el nombre de la instancia es distinto a `.\SQLEXPRESS`.

## Cómo ejecutar

```bash
dotnet restore C23740-PrimerExamen.slnx
dotnet build   C23740-PrimerExamen.slnx
dotnet run --project Restaurante.UI/Restaurante.UI.csproj
```

O abrir `C23740-PrimerExamen.slnx` en **Visual Studio Community 2022** y presionar F5.

La aplicación arranca en `Restaurante.UI` y por defecto redirige a `/Mesas` (Módulo de Registro de mesas).

## Módulos implementados

### 1. Registro de mesas (`/Mesas`)

| Funcionalidad | Acción |
|---|---|
| Listar todas las mesas (Número, Estado, Capacidad, Sección, Precio por persona) | `Index` |
| Filtrar por número de mesa | `Index?busqueda=` |
| Agregar mesa (Estado = Disponible automáticamente) | `Crear` |
| Editar mesa (Número, Capacidad, Sección, Precio) | `Editar/{id}` |

### 2. Mesas Disponibles (`/MesasDisponibles`)

| Funcionalidad | Acción |
|---|---|
| Listar mesas con `Estado = 1` (Disponible) | `Index` |
| Filtrar por número de mesa | `Index?busqueda=` |
| Reservar mesa (Identificación, Nombre, Fecha, Cantidad de comensales) | `Reservar/{id}` |
| Cálculo del depósito de garantía | `MesaService.CalcularDepositoGarantia` |
| Cambiar estado a `Reservada (2)` al confirmar la reserva | automático |

### 3. Mesas Reservadas (`/MesasReservadas`)

| Funcionalidad | Acción |
|---|---|
| Listar mesas con `Estado = 2` (Reservada), incluye Nombre del cliente, Fecha y Comensales | `Index` |
| Filtrar por nombre del cliente | `Index?busqueda=` |
| Liberar mesa (vuelve a `Disponible (1)`) | `Liberar/{id}` (POST) |
| Detalle de la reserva (todos los campos + Identificación + Depósito) | `Detalle/{id}` |

## Regla de cálculo del depósito de garantía

Implementada en `MesaService.CalcularDepositoGarantia(decimal precioPorPersona, int cantidadComensales)`:

| Comensales | Porcentaje aplicado |
|---|---|
| `< 4` | **100 %** del precio por persona × comensales |
| `4 .. 7` (≥ 4 y < 8) | **75 %** del precio por persona × comensales |
| `≥ 8` | **50 %** del precio por persona × comensales |

Las constantes (`UmbralComensalesIntermedio = 4`, `UmbralComensalesAlto = 8`, porcentajes 1.00 / 0.75 / 0.50) están nombradas para evitar magic numbers.

## Clean code

- **Separación estricta de capas:** la UI no instancia DbContext ni hace cálculos; el repositorio no conoce reglas de negocio; el servicio no sabe nada de HTTP ni de vistas.
- **Inyección de dependencias** vía `AddDataAccess()` (DA) y `AddBusinessLogic()` (BL) registradas en `Program.cs`.
- **Validaciones server-side** con `ModelState.IsValid` + Data Annotations en `Mesa` y `ReservaViewModel`; mensajes en español visibles vía `asp-validation-for`.
- **AsNoTracking** en consultas de solo lectura.
- **Naming en español** consistente con el dominio del examen.
