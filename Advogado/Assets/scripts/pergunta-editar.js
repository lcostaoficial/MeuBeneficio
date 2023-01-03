$(function () {

    window.PerguntaEditar = window.PerguntaEditar || {};

    PerguntaEditar.AlternativasDiv = $("#alternativas");
    PerguntaEditar.InputId = $("#PerguntaId");
    PerguntaEditar.Count = PerguntaEditar.InputId.data("count");
    PerguntaEditar.Url = PerguntaEditar.InputId.data("url");

    if (PerguntaEditar.Count > 0) {
        $.ajax({
            method: "GET",
            url: PerguntaEditar.Url,
            success: function (result) {
                PerguntaEditar.AlternativasDiv.html(result);
                $("#MultiplaResposta").bootstrapToggle();
                $("#CaixaSelecao").bootstrapToggle();
            }
        });
    }
});