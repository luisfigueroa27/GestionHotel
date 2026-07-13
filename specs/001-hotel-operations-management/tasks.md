# Tasks: Sistema Integral de Operaciones Hoteleras

**Input**: Design documents from `/specs/001-hotel-operations-management/`

**Prerequisites**: [plan.md](plan.md) · [spec.md](spec.md) · [research.md](research.md) · [data-model.md](data-model.md) · [contracts/routes.md](contracts/routes.md) · [quickstart.md](quickstart.md)

**Stack**: C# / .NET 10 · ASP.NET Core MVC · EF Core 10 · SQL Server 2022 · MSTest

**Format**: `- [ ] [ID] [P?] [Story?] Descripción — ruta/archivo`

- **[P]**: Paralelizable (archivo diferente, sin dependencia de tarea incompleta)
- **[Story]**: Historia de usuario propietaria (US1, US2, US3)

---

## Phase 1: Análisis

**Purpose**: Confirmar estado real del codebase y preparar el campo para las fases de implementación.

T001 Analizar los requisitos del sistema hotelero.

T002 Definir los casos de uso.

T003 Elaborar la especificación funcional mediante Spec Kit.

T004 Revisar y validar la especificación.

## Phase 2: Diseño

T005 Diseñar la arquitectura por capas.

T006 Diseñar la base de datos.

T007 Modelar las entidades.

T008 Configurar Entity Framework Core.

T009 Configurar SQL Server.

## Phase 3: Implementación

T005 Diseñar la arquitectura por capas.

T006 Diseñar la base de datos.

T007 Modelar las entidades.

T008 Configurar Entity Framework Core.

T009 Configurar SQL Server.

## Phase 4: Pruebas

T019 Crear pruebas unitarias.

T020 Ejecutar pruebas.

T021 Corregir errores encontrados.


## Phase 5: Despliegue

T022 Configurar publicación.

T023 Publicar en SmarterASP.NET.

T024 Configurar SQL Server remoto.

T025 Validar funcionamiento en producción.