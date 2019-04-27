$(document).ready(function () {

    $('#iniciarSesion').click(function (e) {
        e.preventDefault();

        $(".loader").addClass("loading");

        var email = document.getElementById("email").value;

        $.ajax({
            type: "GET",
            url: '/Clientes/SearchUser?email=' + email,
            data: "",
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            async: true,
            success: function (resultado) {
                $(".loader").removeClass("loading")
                if (resultado == 'True') {

                    window.location.href = "/Home/Index";
                } else {
                    alert("No existe el correo ingresado");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

            }
        });
    });

    $('#formCrearCliente').submit(function (e) {
        e.preventDefault();
       
        var name = document.getElementById("nameCreate").value;
        var lastName = document.getElementById("lastNameCreate").value;
        var secondLastName = document.getElementById("secondLastNameCreate").value;
        var cedula = document.getElementById("cedulaCreate").value;
        var email = document.getElementById("emailCreate").value;
        var profesion = document.getElementById("profesionCreate").value;

        if (name !== "" && lastName !== "" && secondLastName !== "" && cedula !== "" && email !== "" && profesion !== "") {
          
            $.ajax({
                type: "GET",
                url: '/Clientes/findUserBool?email=' + email,
                data: "",
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                async: true,
                success: function (resultado) {

                    if (resultado == 'True') {
                        alert("Error, el correo ya existe");
                        return false;
                    } else {
                        document.getElementById('formCrearCliente').submit();
                        return true;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {

                }
            });
        } else {
            alert("Debe de llenar todos los campos");
        }
    });
});