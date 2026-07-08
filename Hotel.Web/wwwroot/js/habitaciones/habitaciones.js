// EDITAR Y ELIMINAR HABITACION

async function editarHabitacion(id) {
    let response =
        await fetch(
            '/Habitaciones/ObtenerHabitacion?id=' + id);

    let data = await response.json();

    if (data == null)
        return;

    document.getElementById(
        "editId").value =
        data.idHabitacion;

    document.getElementById(
        "editNumero").value =
        data.numeroHabitacion;

    document.getElementById("editTipo").value =
        data.idTipoHabitacion;

    document.getElementById(
        "editPrecio").value =
        data.precio;

    document.getElementById(
        "editEstado").value =
        data.estado;

    document.getElementById(
        "editPiso").value =
        data.piso;

    document.getElementById(
        "editCapacidad").value =
        data.capacidad;

    document.getElementById(
        "editComodidades").value =
        data.comodidades;

    let modal =
        new bootstrap.Modal(
            document.getElementById(
                "modalEditarHabitacion"));

    modal.show();
}

async function eliminarHabitacion(id) {
    Swal.fire({
        title: '¿Eliminar habitación?',
        icon: 'warning',
        showCancelButton: true
    }).then(async (result) => {

        if (result.isConfirmed) {
            let response =
                await fetch(
                    '/Habitaciones/EliminarHabitacion',
                    {
                        method: 'POST',

                        headers: {
                            'Content-Type':
                                'application/x-www-form-urlencoded'
                        },

                        body: 'idHabitacion=' + id
                    });

            let data =
                await response.json();

            if (data.success) {
                location.reload();
            }
            else {
                Swal.fire(
                    'Error',
                    data.mensaje,
                    'error');
            }
        }
    });
}

// MODAL HABITACION
function abrirModalHabitacion() {
    let modal = new bootstrap.Modal(
        document.getElementById('modalHabitacion')
    );

    modal.show();
}

async function validarNumeroHabitacion() {
    let numero =
        document.getElementById(
            "numeroHabitacion")
            .value;

    let mensaje =
        document.getElementById(
            "mensajeNumeroHabitacion");

    if (numero.trim() === "") {
        mensaje.style.display =
            "none";

        return;
    }

    let response =
        await fetch(
            `/Habitaciones/ValidarNumeroHabitacion?numero=${numero}`);

    let data =
        await response.json();

    if (data.existe) {
        mensaje.innerHTML =
            "⚠ Habitación ya registrada";

        mensaje.style.display =
            "block";

        document
            .getElementById(
                "numeroHabitacion")
            .classList
            .add("is-invalid");
    }
    else {
        mensaje.style.display =
            "none";

        document
            .getElementById(
                "numeroHabitacion")
            .classList
            .remove("is-invalid");
    }
}

//VALIDAR FORMULARIO HABITACION

function validarFormularioHabitacion() {
    let input =
        document.getElementById(
            "numeroHabitacion");

    if (input.classList.contains(
        "is-invalid")) {
        Swal.fire({
            icon: "error",
            title: "Error",
            text: "El número de habitación ya existe"
        });

        return false;
    }

    return true;
}

// FILTRAR HABITACIONES
function filtrarHabitaciones() {
    let filtro =
        document.getElementById(
            "buscarHabitacion")
            .value
            .toLowerCase();

    let tabla =
        document.getElementById(
            "tablaHabitaciones");

    let filas =
        tabla.getElementsByTagName(
            "tr");

    for (let i = 1;
        i < filas.length;
        i++) {
        let texto =
            filas[i]
                .innerText
                .toLowerCase();

        if (texto.indexOf(
            filtro) > -1) {
            filas[i].style.display =
                "";
        }
        else {
            filas[i].style.display =
                "none";
        }
    }
}
