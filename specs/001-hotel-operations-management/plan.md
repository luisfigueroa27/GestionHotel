# Implementation Plan: Sistema Integral de Operaciones Hoteleras

**Branch**: `001-hotel-operations-management` | **Date**: 2026-07-13 | **Spec**: [spec.md](spec.md)

**Input**: Feature specification from `/specs/001-hotel-operations-management/spec.md`

## Summary

Sistema web de gestión hotelera orientado a administradores internos. Cubre los módulos de habitaciones (con estado operativo), huéspedes, hospedajes (check-in/check-out con cálculo automático de costo), servicio al cuarto, pagos (parciales y totales con control de saldo) e historial consultable. La arquitectura es ASP.NET Core MVC + EF Core 10 sobre SQL Server, con Autenticación mediante sesiones de ASP.NET Core. 

## Technical Context

**Language/Version**: C# 13 / .NET 10

**Primary Dependencies**: ASP.NET Core MVC 10, Entity Framework Core 10.0.8, Microsoft.EntityFrameworkCore.SqlServer 10.0.8, Bootstrap 5, MSTest

**Storage**: SQL Server 2022 vía EF Core (Code-First, migraciones activas)

**Testing**: MSTest — pruebas unitarias de controladores con DbContext mockeado

**Target Platform**: IIS en SmarterASP.NET (Windows Server compatible con .NET 10)

**Project Type**: web-service (ASP.NET Core MVC multi-módulo)

**Performance Goals**: Respuesta de operaciones CRUD < 2 s; historial mensual < 5 s (SC-001, SC-005)

**Constraints**: Acceso solo autenticado (sesión activa); integridad referencial en SQL Server; sin doble ocupación concurrente de habitación

**Scale/Scope**: Una sede hotelera; usuarios administradores internos; 6 controladores; 7 entidades de dominio

## Constitution Check

*GATE: evaluado antes de Phase 0. Re-evaluado post Phase 1.*

| Principio constitucional | Estado | Detalle |
|--------------------------|--------|---------|
| I. Specification-Driven Development | ✅ PASS | spec.md generado y validado sin marcadores pendientes |
| II. Layered Architecture | ⚠️ PARCIAL | Carpeta `Hotel.Infrastructure/Repositories/` existe pero vacía. Controladores inyectan `HotelDbContext` directamente. **La aplicación utiliza directamente HotelDbContext para el acceso a datos mediante Entity Framework Core.** |
| III. Code Quality and Testing | ✅ PASS | Tests MSTest existentes para 5 controladores |
| IV. Security and Data Integrity | ⚠️ PARCIAL | Autenticación por sesión (no ASP.NET Core Identity). Justificado: el sistema ya operativo usa sesión. La capa de sesión valida correctamente acceso en todos los controladores. |
| V. Deployment and Maintainability | ✅ PASS | Objetivo: SmarterASP.NET; migraciones EF Core activas; appsettings configurado |
| Technology Standards | ⚠️ DESVIACIÓN MENOR | Proyecto usa .NET 10; constitución dice .NET 9. .NET 10 es upgrade compatible; no hay impacto negativo. |

**Veredicto**: Sin bloqueos. Las desviaciones II y IV están justificadas y controladas por este plan.

**Re-evaluación post Phase 1**: ✅ El diseño de repositorios genéricos y la vinculación de ServicioCuarto a Hospedaje resuelven las brechas de la Capa II.

## Project Structure

### Documentation (this feature)

```text
specs/001-hotel-operations-management/
├── plan.md              # Este archivo
├── research.md          # Phase 0 — decisiones técnicas resueltas
├── data-model.md        # Phase 1 — entidades, campos y relaciones
├── quickstart.md        # Phase 1 — guía de validación end-to-end
├── contracts/
│   └── routes.md        # Phase 1 — contratos de rutas MVC
└── tasks.md             # Phase 2 (generado por /speckit.tasks)
```

### Source Code (repository root)

```textHotel.Domain/
├── Entities/
│   ├── Administrador.cs
│   ├── Habitacion.cs
│   ├── TipoHabitacion.cs
│   ├── Huesped.cs
│   ├── Hospedaje.cs
│   ├── ServicioCuarto.cs
│   └── Pago.cs
└

Hotel.Infrastructure/
├── Data/
│   └── HotelDbContext.cs
|
└── Migrations/

Hotel.Web/
├── Controllers/
│   ├── LoginController.cs
│   ├── DashboardController.cs
│   ├── HabitacionesController.cs
│   ├── HuespedController.cs    
│   ├── ServicioCuartoController.cs
│   ├── PagosController.cs
│   └── HistorialController.cs
├── Models/                  (ViewModels)
├── Views/
└── Program.cs

Hotel.Tests/
├── Controllers/
│   ├── HabitacionesControllerTest.cs
│   ├── ServicioCuartoControllerTest.cs
│   ├── PagosControllerTest.cs
│   ├── HistorialControllerTest.cs
│   └── LoginControllerTest.cs
└── Integration/
```

**Structure Decision**: Arquitectura en 4 proyectos según constitución (Domain, Infrastructure, Web, Tests).

## Complexity Tracking

| Violación | Justificación | Alternativa rechazada |
|-----------|---------------|----------------------|
| Sesión en lugar de ASP.NET Identity | Sistema ya operativo;
| Controladores inyectan DbContext directamente (estado actual) | Implementación existente estable; la transición a repositorios se realiza incrementalmente por módulo | Reescribir todos los controladores en paralelo introduce riesgo de regresión sin cobertura suficiente |
