$(function () {

    window.Pergunta = window.Pergunta || {};

    Pergunta.AlternativasDiv = $("#alternativas");

    Pergunta.Controles = {
        CarregarPaginaAlternativa: function CarregarPaginaAlternativa() {
            var url = Pergunta.AlternativasDiv.data("url");
            $.ajax({
                method: "GET",
                url: url,                
                success: function (result) {
                    Pergunta.AlternativasDiv.html(result);
                    $("#MultiplaResposta").bootstrapToggle();
                    $("#CaixaSelecao").bootstrapToggle();
                }
            });
        },
        DescarregarPaginaAlternativa: function DescarregarPaginaAlternativa() {
            $("#alternativas").html("");
        }        
    };

    Pergunta.Eventos = {
        Carregar: function Carregar() {
            $("#MultiplaAlternativa").change(function () {
                var value = $(this).val();
                if (value === "True") {
                    Pergunta.Controles.CarregarPaginaAlternativa();
                }
                else {
                    Pergunta.Controles.DescarregarPaginaAlternativa();
                }
            });
        }
    };

    Pergunta.Eventos.Carregar();

});