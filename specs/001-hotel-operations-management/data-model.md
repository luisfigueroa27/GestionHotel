# Data Model

## Entidades del Sistema

### Habitacion

| Campo | Tipo |
|--------|------|
| IdHabitacion | int |
| NumeroHabitacion | string |
| IdTipoHabitacion | int |
| PrecioPorNoche | decimal |
| Estado | string |
| Descripcion | string |

---

### TipoHabitacion

| Campo | Tipo |
|--------|------|
| IdTipoHabitacion | int |
| Nombre | string |
| Capacidad | int |
| Descripcion | string |

---

### Huesped

| Campo | Tipo |
|--------|------|
| IdHuesped | int |
| DNI | string |
| Nombre | string |
| Telefono | string |

---

### Hospedaje

| Campo | Tipo |
|--------|------|
| IdHospedaje | int |
| IdHabitacion | int |
| IdHuesped | int |
| FechaIngreso | datetime |
| FechaSalida | datetime |
| Total | decimal |

---

### Pago

| Campo | Tipo |
|--------|------|
| IdPago | int |
| IdHospedaje | int |
| Monto | decimal |
| FechaPago | datetime |


---

### ServicioCuarto

| Campo | Tipo |
|--------|------|
| IdServicio | int |
| IdHabitacion | int |
| Descripcion | string |
| FechaSolicitud | datetime |

---

## Relaciones

TipoHabitacion (1) -------- (*) Habitacion

Habitacion (1) -------- (*) Hospedaje

Huesped (1) -------- (*) Hospedaje

Hospedaje (1) -------- (*) Pago

Habitacion (1) -------- (*) ServicioCuarto