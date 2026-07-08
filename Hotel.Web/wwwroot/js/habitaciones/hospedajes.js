// MODAL HOSPEDAJE
function abrirModal(id, numero) {
    document.getElementById("IdHabitacion").value = id;

    document.getElementById("NumeroHabitacion").value = numero;

    let modal = new bootstrap.Modal(
        document.getElementById('modalHospedaje')
    );

    let precio =
        parseFloat(
            document.getElementById(
                "precio_" + id).value
        );

    document.getElementById(
        "precioHabitacion").value =
        precio;

    document.getElementById(
        "montoTotal").value =
        precio;

    calcularMontoTotal();

    modal.show();
}

// BUSCAR HUESPED

async function buscarHuesped() {
    let dni =
        document.getElementById(
            "dniHuesped").value;

    // VALIDAR LONGITUD

    if (dni.length < 8) {
        return;
    }

    let response = await fetch(
        '/Habitaciones/BuscarHuesped?dni='
        + dni
    );

    let data = await response.json();

    // EXISTE

    if (data.existe) {
        document.getElementById(
            "nombreCompleto").value =
            data.nombreCompleto;

        document.getElementById(
            "telefono").value =
            data.telefono;

        document.getElementById(
            "mensajeHuesped").innerHTML =
            "Huésped registrado";

        document.getElementById(
            "mensajeHuesped").className =
            "text-success";
    }

    // NUEVO

    else {
        document.getElementById(
            "nombreCompleto").value = "";

        document.getElementById(
            "telefono").value = "";

        document.getElementById(
            "mensajeHuesped").innerHTML =
            "Huésped nuevo";

        document.getElementById(
            "mensajeHuesped").className =
            "text-danger";
    }
}

// VER HOSPEDAJE

async function verHospedaje(idHabitacion) {
    let response = await fetch(
        '/Habitaciones/ObtenerHospedaje?idHabitacion='
        + idHabitacion
    );

    let data = await response.json();

    if (data != null) {
        document.getElementById("detalleHabitacion")
            .innerText = data.habitacion;

        document.getElementById("detalleHuesped")
            .innerText = data.huesped;

        document.getElementById("detalleDni")
            .innerText = data.dni;

        document.getElementById("detalleTelefono")
            .innerText = data.telefono;

        document.getElementById("detalleEntrada")
            .innerText = data.fechaEntrada;

        document.getElementById("detalleSalida")
            .innerText = data.fechaSalida;

        document.getElementById("idHospedajeFinalizar")
            .value = data.idHospedaje;

        document.getElementById(
            "idHospedajeExtension").value =
            data.idHospedaje;

        let modal = new bootstrap.Modal(
            document.getElementById(
                'modalDetalleHospedaje')
        );

        modal.show();
    }
}

// FINALIZAR HOSPEDAJE

async function finalizarHospedaje() {
    let idHospedaje =
        document.getElementById(
            "idHospedajeFinalizar").value;

    await fetch('/Habitaciones/FinalizarHospedaje', {

        method: 'POST',

        headers: {
            'Content-Type':
                'application/x-www-form-urlencoded'
        },

        body: 'idHospedaje=' + idHospedaje
    });

    Swal.fire({
        icon: 'success',
        title: 'Hospedaje Finalizado'
    }).then(() => {

        location.reload();

    });
}

// LIBERAR HABITACION

async function liberarHabitacion(idHabitacion) {
    Swal.fire({

        title: '¿Liberar habitación?',

        icon: 'question',

        showCancelButton: true,

        confirmButtonText: 'Sí'

    }).then(async (result) => {

        if (result.isConfirmed) {
            await fetch(
                '/Habitaciones/LiberarHabitacion',
                {
                    method: 'POST',

                    headers: {
                        'Content-Type':
                            'application/x-www-form-urlencoded'
                    },

                    body: 'idHabitacion=' + idHabitacion
                });

            Swal.fire({
                icon: 'success',
                title: 'Habitación Disponible'
            }).then(() => {

                location.reload();

            });
        }

    });
}

//EXTENDER HOSPEDAJE
async function extenderHospedaje() {
    let idHospedaje =
        document.getElementById(
            "idHospedajeExtension")
            .value;

    let nuevaFecha =
        document.getElementById(
            "nuevaFechaSalida")
            .value;

    if (!nuevaFecha) {
        Swal.fire({
            icon: 'warning',
            title: 'Seleccione una fecha'
        });

        return;
    }

    let response =
        await fetch(
            '/Habitaciones/ExtenderHospedaje',
            {
                method: 'POST',

                headers:
                {
                    'Content-Type':
                        'application/x-www-form-urlencoded'
                },

                body:
                    'idHospedaje=' +
                    idHospedaje +
                    '&nuevaFecha=' +
                    nuevaFecha
            });

    let data =
        await response.json();

    if (data.success) {
        Swal.fire({
            icon: 'success',
            title: 'Estadía Extendida',
            text: 'La fecha de salida fue actualizada.'
        })
            .then(() => {
                location.reload();
            });
    }
    else {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudo extender la estadía.'
        });
    }
}