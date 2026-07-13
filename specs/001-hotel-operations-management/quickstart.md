# Quick Start

## Requisitos

- Visual Studio 2022
- .NET 9 SDK
- SQL Server
- Git

---

## Clonar el proyecto

```bash
git clone https://github.com/luisfigueroa27/GestionHotel.git
```

## Restaurar paquetes

```bash
dotnet restore
```

## Aplicar migraciones

```bash
dotnet ef database update
```

## Ejecutar el proyecto

```bash
dotnet run --project Hotel.Web
```

---

## Escenarios de Validación

### Escenario 1

Iniciar sesión con un usuario administrador.

Resultado esperado:

Se muestra el Dashboard.

---

### Escenario 2

Registrar una nueva habitación.

Resultado esperado:

La habitación aparece en el listado.

---

### Escenario 3

Registrar un hospedaje.

Resultado esperado:

La habitación cambia a estado "Ocupada".

---

### Escenario 4

Registrar un pago.

Resultado esperado:

El pago queda asociado al hospedaje.

---

### Escenario 5

Registrar un servicio al cuarto.

Resultado esperado:

El servicio queda registrado correctamente.

---

### Escenario 6

Consultar historial.

Resultado esperado:

Se muestran hospedajes, pagos y servicios registrados.

---

### Escenario 7

Cerrar hospedaje.

Resultado esperado:

La habitación vuelve al estado "Disponible".