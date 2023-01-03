$(function () {

    window.Alternativa = window.Alternativa || {}

	Alternativa.adicionarAlternativa = function () {
		$(document).on("click", ".add-alternativa", function (e) {
			e.preventDefault();
			var url = $(this).attr("href");
			var form = $(this).closest("form");
			$.ajax({
				method: "POST",
				url: url,
				data: form.serialize(),
				success: function (data) {
					$(".lista-alternativa").empty();
					$(".lista-alternativa").html(data);
				}
			});
		});
	};

	Alternativa.removerAlternativa = function () {
		$(document).on("click", ".remove-alternativa", function (e) {
			e.preventDefault();
			var url = $(this).attr("href");
			var form = $(this).closest("form");
			$("#index").val($(this).data("index"));
			$.ajax({
				method: "POST",
				url: url,
				data: form.serialize(),
				success: function (data) {
					$(".lista-alternativa").empty();
					$(".lista-alternativa").html(data);
				}
			});
		});
	};

    Alternativa.adicionarAlternativa();
    Alternativa.removerAlternativa();

});