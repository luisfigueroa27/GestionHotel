# Research: Sistema Integral de Operaciones Hoteleras

## Objetivo

Este documento registra las decisiones técnicas adoptadas durante el desarrollo del Sistema Integral de Operaciones Hoteleras siguiendo el enfoque Specification-Driven Development (SDD).

---

## Decisión 1: Framework de desarrollo

### Decisión

Utilizar ASP.NET Core MVC (.NET 10).

### Justificación

ASP.NET Core MVC proporciona una arquitectura robusta para aplicaciones empresariales, facilita la separación entre presentación, lógica de negocio y acceso a datos, además de integrarse naturalmente con Entity Framework Core.

---

## Decisión 2: Motor de Base de Datos

### Decisión

Utilizar Microsoft SQL Server.

### Justificación

SQL Server ofrece integridad referencial, soporte para transacciones y una integración completa con Entity Framework Core mediante Code First.

---

## Decisión 3: Acceso a datos

### Decisión

Utilizar Entity Framework Core.

### Justificación

Permite mapear entidades del dominio a tablas relacionales, simplificando el mantenimiento y evolución del sistema.

---

## Decisión 4: Arquitectura

### Decisión

Implementar una arquitectura por capas.

### Capas

- Hotel.Web
- Hotel.Domain
- Hotel.Infrastructure
- Hotel.Tests

### Justificación

Facilita el mantenimiento, escalabilidad y reutilización del código.

---

## Decisión 5: Autenticación

### Decisión

Utilizar autenticación mediante sesiones.

### Justificación

El sistema es utilizado únicamente por administradores internos del hotel.

---

## Decisión 6: Pruebas

### Decisión

Implementar pruebas unitarias mediante MSTest.

### Justificación

Permiten validar la lógica de negocio y detectar regresiones durante el desarrollo.

---

## Decisión 7: Despliegue

### Decisión

Publicar el sistema en SmarterASP.NET.

### Justificación

Permite alojar aplicaciones ASP.NET Core utilizando IIS y SQL Server de forma sencilla.

---

## Conclusión

Las tecnologías seleccionadas cumplen los requisitos funcionales y no funcionales del proyecto, permitiendo desarrollar un sistema mantenible, escalable y alineado con el enfoque SDD.