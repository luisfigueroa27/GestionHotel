# Feature Specification: Sistema Integral de Operaciones Hoteleras

**Feature Branch**: `[001-hotel-operations-management]`

**Created**: 2026-07-12

**Status**: Draft

**Input**: User description: "Desarrollar un sistema para gestionar habitaciones, hospedajes, pagos, historial y servicio al cuarto, orientado a administradores del hotel."

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Gestionar habitaciones y disponibilidad operativa (Priority: P1)

Como administrador, necesito gestionar el inventario de habitaciones y su disponibilidad para mantener la operación diaria sin conflictos de ocupación.

**Why this priority**: Sin control de habitaciones disponibles, no es posible operar ingresos de huéspedes de forma confiable.

**Independent Test**: Puede probarse creando y actualizando habitaciones, cambiando su estado operativo y verificando que solo las habilitadas queden disponibles para hospedaje.

**Acceptance Scenarios**:

1. **Given** una habitación registrada como disponible, **When** el administrador inicia un hospedaje sobre esa habitación, **Then** el estado cambia a ocupada.
2. **Given** una habitación marcada en mantenimiento, **When** el administrador intenta asignarla a un hospedaje, **Then** el sistema bloquea la operación e informa que no está operativa.
3. **Given** una habitación ocupada, **When** se registra la salida del huésped, **Then** la habitación vuelve al estado disponible.

---

### User Story 2 - Registrar hospedajes y calcular cargos (Priority: P2)

Como administrador, necesito registrar ingresos y salidas de huéspedes para calcular el costo total de la estancia y facturar correctamente.

**Why this priority**: El registro de hospedajes y su cálculo económico es la base del control operativo y financiero del hotel.

**Independent Test**: Puede probarse iniciando y cerrando hospedajes con fechas válidas, verificando que el total se calcule automáticamente según la duración y cargos asociados.

**Acceptance Scenarios**:

1. **Given** una habitación disponible y un huésped registrado, **When** el administrador registra el ingreso, **Then** se crea un hospedaje activo asociado a ambos.
2. **Given** un hospedaje activo, **When** el administrador registra la salida, **Then** el sistema calcula automáticamente el importe total de la estancia.
3. **Given** un hospedaje sin salida registrada, **When** el administrador consulta el estado operativo, **Then** el hospedaje aparece como activo y la habitación como ocupada.

---

### User Story 3 - Gestionar pagos, servicios y trazabilidad (Priority: P3)

Como administrador, necesito registrar pagos y servicios de cuarto y consultar historial de operaciones para auditar la gestión financiera y operativa.

**Why this priority**: La trazabilidad completa de cargos y cobros reduce errores administrativos y facilita la conciliación.

**Independent Test**: Puede probarse agregando servicios al cuarto y pagos parciales a un hospedaje, validando saldo actualizado e historial consolidado.

**Acceptance Scenarios**:

1. **Given** un hospedaje activo, **When** el administrador registra un consumo de servicio al cuarto, **Then** el cargo se incorpora al total acumulado.
2. **Given** un hospedaje con saldo pendiente, **When** el administrador registra un pago parcial, **Then** el saldo se actualiza y la operación queda trazada en historial.
3. **Given** un hospedaje con saldo en cero, **When** el administrador intenta registrar un pago adicional que excede el total, **Then** el sistema bloquea la operación y muestra una validación.

---

### Edge Cases

- ¿Qué ocurre cuando un administrador intenta registrar un hospedaje con fecha de salida anterior o igual a la fecha de ingreso?
- ¿Cómo responde el sistema cuando se intenta registrar un huésped sin datos obligatorios mínimos?
- ¿Qué sucede si se intenta registrar un pago sobre un hospedaje ya cerrado y sin saldo pendiente?
- ¿Cómo se comporta el sistema ante intentos de eliminar registros que tienen operaciones financieras asociadas?
- ¿Qué ocurre cuando dos administradores intentan ocupar la misma habitación de manera concurrente?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: El sistema debe permitir al administrador autenticarse mediante un usuario y contraseña para acceder al sistema.
- **FR-002**: El sistema debe permitir registrar, modificar, eliminar y consultar habitaciones del hotel.
- **FR-003**: El sistema debe permitir administrar los diferentes tipos de habitación, incluyendo nombre y características.
- **FR-004**: El sistema debe permitir registrar nuevos huéspedes y consultar su información mediante el número de DNI.
- **FR-005**: El sistema debe permitir registrar hospedajes asociando una habitación disponible con un huésped.
- **FR-006**: El sistema debe actualizar automáticamente el estado de la habitación (Disponible, Ocupada, Vencida o Mantenimiento) según las operaciones realizadas.
- **FR-007**: Extensión de hospedaje	El sistema debe permitir modificar la fecha de salida de un hospedaje cuando el huésped solicite ampliar su estadía.
- **FR-008**: El sistema debe permitir finalizar un hospedaje y actualizar automáticamente el estado de la habitación correspondiente.
- **FR-009**: El sistema debe permitir registrar pagos, calcular el saldo pendiente y actualizar el estado del pago.
-**FR-010**: El sistema debe permitir completar pagos pendientes actualizando el monto pagado y el saldo restante.
- **FR-011**: El sistema debe permitir registrar solicitudes de servicio para una habitación ocupada.
- **FR-012**: El sistema debe permitir cambiar el estado de un servicio desde Pendiente hasta Completado.
- **FR-013**: El sistema debe mostrar el historial completo de hospedajes realizados en el hotel.
- **FR-014**: El sistema debe mostrar indicadores estadísticos del hotel, incluyendo habitaciones disponibles, ocupadas, vencidas, mantenimiento e ingresos.
- **FR-015**: El sistema debe mostrar un gráfico interactivo de ingresos por fecha para apoyar la toma de decisiones.
- **FR-016**: El sistema debe permitir filtrar la información del Dashboard mediante una fecha de inicio y una fecha de fin.
- **FR-017**: El sistema debe permitir al administrador cerrar sesión de manera segura.

### Key Entities *(include if feature involves data)*

- **Habitación**: Unidad alojable del hotel, con identificador, tipo, estado operativo y política tarifaria aplicable.
- **Tipo de Habitación**: Clasificación comercial de habitación con atributos de capacidad y tarifa base.
- **Huésped**: Persona que ocupa una habitación, con datos de identificación y contacto.
- **Hospedaje**: Estancia efectiva del huésped con ingreso, salida, estado y total acumulado.
- **Pago**: Registro de abono aplicado a un hospedaje con monto, fecha y estado de conciliación.
- **Servicio de Cuarto**: Cargo adicional asociado a un hospedaje por consumos complementarios.
- **Administrador**: Usuario autorizado para operar módulos administrativos y consultar reportes.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: El 95% de los hospedajes válidos se registra en menos de 2 minutos desde el inicio del formulario por parte del administrador.
- **SC-002**: El 100% de las asignaciones de habitación evita doble ocupación simultánea de una misma habitación.
- **SC-003**: Al menos el 98% de los hospedajes cerrados presenta cálculo correcto del total según reglas de negocio definidas.
- **SC-004**: El 100% de los pagos registrados actualiza correctamente el saldo pendiente y queda visible en historial.
- **SC-005**: El 90% de las consultas administrativas de historial devuelve resultados en menos de 5 segundos para periodos mensuales.
- **SC-006**: Al menos el 85% de usuarios administrativos completa tareas críticas (check-in, check-out, registro de servicio y pago) sin asistencia adicional en pruebas de aceptación.

## Assumptions

- Los usuarios objetivo de esta versión son administradores internos del hotel con permisos operativos.
- La operación contempla una sola sede de hotel en esta fase inicial.
- Las reglas tarifarias y cargos aplicables están definidos por el negocio antes de iniciar operación productiva.
- Los administradores cuentan con acceso a conectividad estable durante el registro de operaciones.
- El alcance cubre gestión operativa interna; canales públicos de autoservicio para clientes quedan fuera de esta fase.
- La organización dispone de políticas internas para respaldo y retención de información operativa.
