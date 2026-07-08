//CALCULAR SALDO
function calcularSaldo() {
    let monto =
        parseFloat(
            document.getElementById(
                "montoTotal").value
        );

    let pago =
        parseFloat(
            document.getElementById(
                "pago").value
        ) || 0;

    let saldo = monto - pago;

    document.getElementById(
        "saldoPendiente").value =
        saldo.toFixed(2);
}

//CALCULAR MONTO
function calcularMontoTotal() {
    let fechaEntrada =
        new Date(
            document.getElementById(
                "fechaEntrada").value
        );

    let fechaSalida =
        new Date(
            document.getElementById(
                "fechaSalida").value
        );

    let precio =
        parseFloat(
            document.getElementById(
                "precioHabitacion").value
        ) || 0;

    // VALIDAR FECHAS

    if (!fechaSalida || !precio) {
        return;
    }

    // DIFERENCIA EN DIAS

    let diferencia =
        fechaSalida - fechaEntrada;

    let dias =
        diferencia / (1000 * 60 * 60 * 24);

    // MINIMO 1 DIA

    if (dias <= 0) {
        dias = 1;
    }

    // CALCULAR TOTAL

    let total = dias * precio;

    document.getElementById(
        "montoTotal").value =
        total.toFixed(2);

    // ACTUALIZAR SALDO

    calcularSaldo();
}
