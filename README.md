# Sistema Integral de Operaciones Hoteleras

Proyecto desarrollado utilizando Specification-Driven Development (SDD) y Scrum.

## Tecnologías

- ASP.NET Core MVC (.NET 9)
- SQL Server
- Entity Framework Core
- MSTest

## Arquitectura

- Hotel.Web
- Hotel.Domain
- Hotel.Infrastructure
- Hotel.Tests

## Documentación SDD

/specs/

/.specify/

/.github/agents/

/.github/prompts/

## Flujo de desarrollo

Constitution

↓

Specification

↓

Plan

↓

Tasks

↓

Implementation

↓

Testing

↓

Deployment

# Base de Datos

Este directorio contiene la copia de seguridad de la base de datos utilizada por el proyecto.

Archivo:
- Hotel.bak

Motor:
- Microsoft SQL Server

Restauración:
1. Abrir SQL Server Management Studio.
2. Crear una nueva base de datos o seleccionar "Restore Database".
3. Elegir Device.
4. Seleccionar HotelBD.bak.
5. Restaurar la base de datos.