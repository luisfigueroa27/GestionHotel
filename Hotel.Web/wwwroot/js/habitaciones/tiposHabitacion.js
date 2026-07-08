//ACTUALIZAR LISTA DE TIPOS DE HABITACION
async function cargarTiposHabitacion() {
    let html =
        await fetch("/Habitaciones/ListarTiposHabitacion");

    document.getElementById("listaTipos").innerHTML =
        await html.text();

    actualizarSelectTipos();
}


async function actualizarSelectTipos() {
    let response =
        await fetch("/Habitaciones/ListarTiposHabitacion");

    let div =
        document.createElement("div");

    div.innerHTML =
        await response.text();

    let opciones =
        "";

    div.querySelectorAll("span").forEach(span => {

        opciones +=
            `<option value="${span.dataset.id}">
        ${span.innerHTML}
        </option>`;

    });

    document.querySelector(
        "select[name='IdTipoHabitacion']")
        .innerHTML =
        "<option value=''>Seleccione</option>"
        + opciones;

    document.getElementById("editTipo")
        .innerHTML =
        opciones;
}

function editarTipoHabitacion(id, nombre) {
    document.getElementById("idTipoEditar")
        .value = id;

    document.getElementById("nuevoTipo")
        .value = nombre;
}

//ABRIR MODAL
function abrirTipos() {
    let modal =
        new bootstrap.Modal(
            document
                .getElementById(
                    "modalTipos"));

    modal.show();
}

//AGREGAR Y EDITAR
async function agregarTipoHabitacion() {

    let id = document.getElementById("idTipoEditar").value;

    let nombre = document
        .getElementById("nuevoTipo")
        .value
        .trim();

    if (nombre == "") {

        Swal.fire(
            "Aviso",
            "Ingrese un nombre.",
            "warning");

        return;
    }

    let url =
        id == 0
            ? "/Habitaciones/AgregarTipoHabitacion"
            : "/Habitaciones/EditarTipoHabitacion";

    let body =
        id == 0
            ? "nombre=" + encodeURIComponent(nombre)
            : "id=" + id + "&nombre=" + encodeURIComponent(nombre);

    let response = await fetch(url, {

        method: "POST",

        headers: {

            "Content-Type":
                "application/x-www-form-urlencoded"

        },

        body: body

    });

    let data = await response.json();

    if (data.success) {

        Swal.fire(
            "Éxito",
            id == 0
                ? "Tipo agregado correctamente."
                : "Tipo actualizado correctamente.",
            "success");

        document.getElementById("nuevoTipo").value = "";

        document.getElementById("idTipoEditar").value = 0;

        await cargarTiposHabitacion();

    }
    else {

        Swal.fire(
            "Aviso",
            data.mensaje,
            "warning");

    }
}

//ELIMINAR
async function eliminarTipoHabitacion(id) {

    const confirmar = await Swal.fire({
        title: "¿Eliminar tipo?",
        text: "Esta acción no se puede deshacer.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Eliminar"
    });

    if (!confirmar.isConfirmed)
        return;

    let response = await fetch(
        "/Habitaciones/EliminarTipoHabitacion",
        {
            method: "POST",
            headers: {
                "Content-Type":
                    "application/x-www-form-urlencoded"
            },
            body: "id=" + id
        });

    let data = await response.json();

    if (data.success) {

        Swal.fire(
            "Éxito",
            "Tipo eliminado.",
            "success")
        await cargarTiposHabitacion();

    } else {

        Swal.fire(
            "No se puede eliminar",
            data.mensaje,
            "error");
    }
}