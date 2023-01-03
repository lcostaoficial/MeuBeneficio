$(function () {

    "use strict";

    window.Estrangeiro = window.Estrangeiro || {};

    Estrangeiro.habilitarEstrangeiro = function () {
        $("input[name='Cliente.DocumentoIdentificacaoEstrangeiro']").prop("disabled", false);
        $("input[name='Cliente.Rg']").prop("disabled", true);
        $("select[name='Cliente.OrgaoExpedidorRg']").prop("disabled", true);
        $("select[name='Cliente.EstadoOrgaoExpedidor']").prop("disabled", true);
        $("input[name='Cliente.Rg']").val("");
        $("select[name='Cliente.OrgaoExpedidorRg']").val(0).change();
        $("select[name='Cliente.EstadoOrgaoExpedidor']").val(0).change();

        $("input[name='DocumentoIdentificacaoEstrangeiro']").prop("disabled", false);
        $("input[name='Rg']").prop("disabled", true);
        $("select[name='OrgaoExpedidorRg']").prop("disabled", true);
        $("select[name='EstadoOrgaoExpedidor']").prop("disabled", true);
        $("input[name='Rg']").val("");
        $("select[name='OrgaoExpedidorRg']").val(0).change();
        $("select[name='EstadoOrgaoExpedidor']").val(0).change();
    };

    Estrangeiro.desabilitarEstrangeiro = function () {
        $("input[name='Cliente.Rg']").prop("disabled", false);
        $("select[name='Cliente.OrgaoExpedidorRg']").prop("disabled", false);
        $("select[name='Cliente.EstadoOrgaoExpedidor']").prop("disabled", false);
        $("input[name='Cliente.DocumentoIdentificacaoEstrangeiro']").prop("disabled", true);
        $("input[name='Cliente.DocumentoIdentificacaoEstrangeiro']").val("");

        $("input[name='Rg']").prop("disabled", false);
        $("select[name='OrgaoExpedidorRg']").prop("disabled", false);
        $("select[name='EstadoOrgaoExpedidor']").prop("disabled", false);
        $("input[name='DocumentoIdentificacaoEstrangeiro']").prop("disabled", true);
        $("input[name='DocumentoIdentificacaoEstrangeiro']").val("");
    };

    Estrangeiro.desabilitarTodos = function () {
        $("input[name='Cliente.Rg']").val("");
        $("input[name='Cliente.Rg']").prop("disabled", true);
        $("select[name='Cliente.OrgaoExpedidorRg']").prop("disabled", true);
        $("select[name='Cliente.OrgaoExpedidorRg']").val(0).change();
        $("select[name='Cliente.EstadoOrgaoExpedidor']").prop("disabled", true);
        $("select[name='Cliente.EstadoOrgaoExpedidor']").val(0).change();
        $("input[name='Cliente.DocumentoIdentificacaoEstrangeiro']").prop("disabled", true);
        $("input[name='Cliente.DocumentoIdentificacaoEstrangeiro']").val("");

        $("input[name='Rg']").val("");
        $("input[name='Rg']").prop("disabled", true);
        $("select[name='OrgaoExpedidorRg']").prop("disabled", true);
        $("select[name='OrgaoExpedidorRg']").val(0).change();
        $("select[name='EstadoOrgaoExpedidor']").prop("disabled", true);
        $("select[name='EstadoOrgaoExpedidor']").val(0).change();
        $("input[name='DocumentoIdentificacaoEstrangeiro']").prop("disabled", true);
        $("input[name='DocumentoIdentificacaoEstrangeiro']").val("");
    };

    $(document).on("change", "select[name='Cliente.ClienteEstrangeiro']", function () {
        var value = $(this).val();
        if (value === 'true') {
            Estrangeiro.habilitarEstrangeiro();
        }
        else if (value === 'false') {
            Estrangeiro.desabilitarEstrangeiro();
        }
        else {
            Estrangeiro.desabilitarTodos();
        }
    });

    $(document).on("change", "select[name='ClienteEstrangeiro']", function () {
        var value = $(this).val();
        if (value === 'true') {
            Estrangeiro.habilitarEstrangeiro();
        }
        else if (value === 'false') {
            Estrangeiro.desabilitarEstrangeiro();
        }
        else {
            Estrangeiro.desabilitarTodos();
        }
    });

});