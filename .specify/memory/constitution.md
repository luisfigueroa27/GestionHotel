# Hotel Management System Constitution

## Core Principles

### I. Specification-Driven Development (SDD)

Todo desarrollo del Sistema de Gestión de Habitaciones debe comenzar con una especificación funcional antes de escribir o modificar código. Cada funcionalidad debe definir claramente su objetivo, alcance, requisitos funcionales, criterios de aceptación y restricciones de negocio. Ninguna implementación deberá realizarse sin una especificación previamente aprobada.

### II. Layered Architecture

El sistema deberá mantener una arquitectura por capas para garantizar la separación de responsabilidades y facilitar el mantenimiento del software.

La estructura del proyecto está compuesta por:

- **Hotel.Web:** Capa de presentación desarrollada con ASP.NET Core MVC. Contiene controladores, vistas, autenticación e interacción con el usuario.
- **Hotel.Domain:** Contiene las entidades del dominio, reglas de negocio, interfaces y modelos centrales del sistema.
- **Hotel.Infrastructure:** Implementa el acceso a datos mediante Entity Framework Core, el contexto de base de datos y los repositorios.
- **Hotel.Tests:** Contiene las pruebas unitarias del sistema para validar el correcto funcionamiento de las funcionalidades implementadas.

Cada capa únicamente podrá comunicarse con las capas permitidas por la arquitectura establecida.

---

### III. Code Quality and Testing

Todo cambio funcional deberá ser validado mediante pruebas antes de ser considerado finalizado.

El proyecto utilizará:

- MSTest para pruebas unitarias.
- Entity Framework Core para persistencia de datos.
- SQL Server como motor de base de datos.

Las pruebas deberán verificar como mínimo:

- Reglas de negocio.
- Validaciones.
- Operaciones CRUD.
- Casos límite.
- Integridad de los datos.

El proyecto deberá mantener una cobertura de pruebas adecuada que permita validar la estabilidad del sistema antes de cada despliegue.

---

### IV. Security and Data Integrity

Todas las funcionalidades administrativas deberán requerir autenticación mediante ASP.NET Core Identity.

El sistema deberá garantizar:

- Validación de datos de entrada.
- Protección frente a registros duplicados.
- Integridad referencial de la base de datos.
- Control de acceso únicamente para usuarios autorizados.

La información almacenada en SQL Server deberá mantenerse consistente durante todas las operaciones del sistema.

---

### V. Deployment and Maintainability

El sistema deberá poder desplegarse en un servidor IIS compatible con ASP.NET Core utilizando SmarterASP.NET como plataforma de hospedaje.

Todo despliegue deberá verificar:

- Compilación sin errores.
- Migraciones de base de datos aplicadas correctamente.
- Archivo appsettings configurado.
- Conectividad con SQL Server.
- Funcionamiento correcto de los módulos principales.

La documentación técnica deberá mantenerse sincronizada con el estado real del sistema.

---

# Technology Standards

El proyecto adopta las siguientes tecnologías oficiales:

| Tecnología | Versión |
|------------|----------|
| ASP.NET Core MVC | .NET 9 |
| Entity Framework Core | 9 |
| SQL Server | 2022 |
| Bootstrap | 5 |
| MSTest | Última estable |
| Git | Control de versiones |
| GitHub | Repositorio remoto |
| SmarterASP.NET | Plataforma de despliegue |

---

# Development Workflow

El proyecto seguirá el siguiente flujo de desarrollo basado en Specification-Driven Development (SDD):

1. Definición de la especificación.
2. Validación y aclaración de requisitos.
3. Elaboración del plan de implementación.
4. Generación de tareas.
5. Desarrollo de la funcionalidad.
6. Pruebas unitarias.
7. Pruebas de integración.
8. Despliegue.
9. Actualización de la documentación.

Cada funcionalidad desarrollada deberá completar este ciclo antes de considerarse terminada.

---

# Coding Standards

El código deberá cumplir las siguientes normas:

- Utilizar nombres descriptivos para clases, métodos y variables.
- Mantener una única responsabilidad por clase.
- Evitar duplicación de código.
- Utilizar Dependency Injection.
- Seguir las convenciones oficiales de C#.
- Documentar únicamente cuando sea necesario.
- Mantener el proyecto organizado por capas.

---

# Governance

Esta Constitución establece los principios que rigen el desarrollo del Sistema de Gestión de Habitaciones.

Toda nueva funcionalidad deberá respetar los principios aquí definidos.

Las modificaciones importantes de arquitectura deberán actualizar este documento antes de su implementación.

La documentación generada mediante Specification-Driven Development formará parte del historial del proyecto y servirá como evidencia del proceso de desarrollo.

---

**Version:** 1.0.0

**Ratified:** 05/07/2026

**Last Amended:** 05/07/2026