$(function () {

    "use strict";

    window.Caso = window.Caso || {};

    Caso.habilitarCpf = function () {
        $(".cpf").prop("disabled", false);
    };

    Caso.desabilitarCpf = function () {
        $(".cpf").prop("disabled", true);
    };

    Caso.fixarCasoId = function (id) {
        $("#CasoId").val(id);
    };

    Caso.fixarClienteId = function (id) {
        $("#ClienteId").val(id);
    };

    Caso.habilitarEnvioDocumento = function () {
        $("#menu-documentos").removeClass("disabled");
        $("#documentos").removeClass("disabled");
    };

    Caso.habilitarEnvioQuestionario = function () {
        $("#menu-questionario").removeClass("disabled");
        $("#questionario").removeClass("disabled");
    };

    Caso.habilitarImpressao = function () {
        $("#menu-impressao").removeClass("disabled");
        $("#impressao").removeClass("disabled");
    };

    Caso.habilitarAcompanhamento = function () {
        $("#menu-acompanhamento").removeClass("disabled");
        $("#acompanhamento").removeClass("disabled");
    };

    Caso.habilitarTarefas = function () {
        $("#menu-tarefas").removeClass("disabled");
        $("#tarefas").removeClass("disabled");
    };

    Caso.iniciarCarregamento = function () {
        swal({
            title: "",
            text: "Aguarde por favor",
            imageUrl: "../../Assets/images/loading.gif",
            showConfirmButton: false
        });
    };

    Caso.pararCarregamento = function () {
        swal.close();
    };

    Caso.dadosPessoais = function (casoId = 0) {
        Caso.iniciarCarregamento();
        var url = $("#dadospessoais").data("url");

        $.ajax({
            method: "GET",
            url: url,
            data: { casoId: casoId },
            dataType: "html",
            success: function success(result) {

                $("#frm-conteudo-dadospessoais").html(result);
                $(".selectpicker").selectpicker();

                GlobalMask.carregarMascaras();

                if (casoId !== 0) {
                    Caso.Controles.CarregarPaginaGrupoFamiliar();
                }

            },
            error: function error(XMLHttpRequest, textStatus, errorThrown) {
                swal("Mensagem", errorThrown, "error");
            },
            complete: function complete() {
                Caso.pararCarregamento();
            }
        });
    };

    Caso.anexarArquivo = function (tipoArquivoId, arquivoId) {
        var casoId = $("#CasoId").val();
        $("form#frmSalvarCasoComAnexo #ArquivoId").val(arquivoId);
        $("form#frmSalvarCasoComAnexo #CasoId").val(casoId);
        $("form#frmSalvarCasoComAnexo #TipoArquivoId").val(tipoArquivoId);
        $("#modalAnexarDocumento").modal("show");
    };

    Caso.novoAcompanhamento = function () {
        var casoId = $("#CasoId").val();
        $("form#frmSalvarCasoComMovimentacao #CasoId").val(casoId);
        $("#modalNovoAcompanhamento").modal("show");
    };

    Caso.novaTarefa = function () {
        var casoId = $("#CasoId").val();
        $("form#frmSalvarCasoComTarefa #CasoId").val(casoId);
        $("#modalNovaTarefa").modal("show");
    };

    Caso.finalizarTarefa = function (tarefaId) {
        $("#modalDetalhesTarefa").modal("hide");
        $("form#frmFinalizarTarefa #TarefaId").val(tarefaId);
        $("#modalFinalizarTarefa").modal("show");
    };

    Caso.alterarResponsavel = function (tarefaId) {
        $("#modalDetalhesTarefa").modal("hide");
        $("form#frmAlteracaoResponsavel #TarefaId").val(tarefaId);
        $("#modalAlterarResponsavel").modal("show");
    };

    Caso.alterarPrazo = function (tarefaId) {
        $("#modalDetalhesTarefa").modal("hide");
        $("form#frmAlterarPrazo #TarefaId").val(tarefaId);
        $("#modalAlterarPrazo").modal("show");
    };

    Caso.novaTarefaComData = function (data) {
        var casoId = $("#CasoId").val();
        $("form#frmSalvarCasoComTarefa #CasoId").val(casoId);
        $("form#frmSalvarCasoComTarefa #DataExpiracao").val(data);
        $("#modalNovaTarefa").modal("show");
    };

    Caso.carregarQuestionario = function (casoId) {
        Caso.iniciarCarregamento();
        var url = $("#questionario").data("url");
        $.ajax({
            method: "GET",
            url: url,
            data: { casoId: casoId },
            dataType: "html",
            success: function success(result) {
                $("#frm-conteudo-questionario").html(result);
                $(".selectpicker").selectpicker();
            },
            error: function error(XMLHttpRequest, textStatus, errorThrown) {
                swal("Mensagem", errorThrown, "error");
            },
            complete: function complete() {
                Caso.pararCarregamento();
            }
        });
    };

    Caso.carregarDocumentos = function (casoId) {
        Caso.iniciarCarregamento();
        var url = $("#documentos").data("url");
        $.ajax({
            method: "GET",
            url: url,
            data: { casoId: casoId },
            dataType: "html",
            success: function success(result) {
                $("#frm-conteudo-documento").html(result);
            },
            error: function error(XMLHttpRequest, textStatus, errorThrown) {
                swal("Mensagem", errorThrown, "error");
            },
            complete: function complete() {
                Caso.pararCarregamento();
            }
        });
    };

    Caso.carregarAcompanhamentos = function (casoId) {
        Caso.iniciarCarregamento();
        var url = $("#acompanhamento").data("url");
        $.ajax({
            method: "GET",
            url: url,
            data: { casoId: casoId },
            dataType: "html",
            success: function success(result) {
                $("#frm-conteudo-acompanhamento").html(result);
            },
            error: function error(XMLHttpRequest, textStatus, errorThrown) {
                swal("Mensagem", errorThrown, "error");
            },
            complete: function complete() {
                Caso.pararCarregamento();
            }
        });
    };

    Caso.validarCpf = function (cpf) {
        var numeros, digitos, soma, i, resultado, digitos_iguais;
        digitos_iguais = 1;
        if (cpf.length < 11)
            return false;
        for (i = 0; i < cpf.length - 1; i++)
            if (cpf.charAt(i) != cpf.charAt(i + 1)) {
                digitos_iguais = 0;
                break;
            }
        if (!digitos_iguais) {
            numeros = cpf.substring(0, 9);
            digitos = cpf.substring(9);
            soma = 0;
            for (i = 10; i > 1; i--)
                soma += numeros.charAt(10 - i) * i;
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(0))
                return false;
            numeros = cpf.substring(0, 10);
            soma = 0;
            for (i = 11; i > 1; i--)
                soma += numeros.charAt(11 - i) * i;
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(1))
                return false;
            return true;
        }
        else
            return false;
    };

    Caso.buscarCpfExistente = function (cpf) {
        Caso.iniciarCarregamento();
        var url = $("input[name='Cliente.Cpf']").data("url");
        $.ajax({
            type: "GET",
            url: url,
            data: { cpf: cpf },
            success: function success(result) {
                if (result.Success) {
                    if (result.JaExiste) {
                        $("#ClienteId").val(result.Model.ClienteId);
                        $("#RepresentanteLegalId").val(result.Model.Estado).change();
                        $("input[name='Cliente.Nome']").val(result.Model.Nome);
                        $("input[name='Cliente.NomeMae']").val(result.Model.NomeMae);
                        $("input[name='Cliente.Profissao']").val(result.Model.Profissao);
                        $("input[name='Cliente.Naturalidade']").val(result.Model.Naturalidade);
                        $("input[name='Cliente.Nacionalidade']").val(result.Model.Nacionalidade);
                        $("input[name='Cliente.Naturalidade']").val(result.Model.Naturalidade);
                        $("input[name='Cliente.DataNascimento']").val(result.Model.DataNascimentoFormatado);
                        $("select[name='Cliente.EstadoNascimento']").val(result.Model.EstadoNascimento).change();
                        $("select[name='Cliente.EstadoCivil']").val(result.Model.EstadoCivil).change();
                        $("select[name='Cliente.ClienteEstrangeiro']").val(`${result.Model.ClienteEstrangeiro}`).change();
                        $("input[name='Cliente.DocumentoIdentificacaoEstrangeiro']").val(result.Model.DocumentoIdentificacaoEstrangeiro);
                        $("input[name='Cliente.Rg']").val(result.Model.Rg);
                        $("select[name='Cliente.OrgaoExpedidorRg']").val(result.Model.OrgaoExpedidorRg).change();
                        $("select[name='Cliente.EstadoOrgaoExpedidor']").val(result.Model.EstadoOrgaoExpedidor).change();
                        if (result.Model.ClienteEstrangeiro === true) {
                            $("input[name='Cliente.DocumentoIdentificacaoEstrangeiro']").prop("disabled", false);
                        }
                        else {
                            $("input[name='Cliente.Rg']").prop("disabled", false);
                            $("select[name='Cliente.OrgaoExpedidorRg']").prop("disabled", false);
                            $("select[name='Cliente.EstadoOrgaoExpedidor']").prop("disabled", false);
                        }
                        $("input[name='Cliente.Email']").val(result.Model.Email);
                        $("input[name='Cliente.TelefoneMovel']").val(result.Model.TelefoneMovel);
                        $("input[name='Cliente.TelefoneFixo']").val(result.Model.TelefoneFixo);
                        $("input[name='Cliente.Cep']").val(result.Model.Cep);
                        $("input[name='Cliente.Rua']").val(result.Model.Rua);
                        $("input[name='Cliente.Numero']").val(result.Model.Numero);
                        $("input[name='Cliente.Bairro']").val(result.Model.Bairro);
                        $("input[name='Cliente.Cidade']").val(result.Model.Cidade);
                        $("select[name='Cliente.Estado']").val(result.Model.Estado).change();
                        Caso.desabilitarCpf();
                    }
                    else {
                        $("#ClienteId").val(0);
                    }
                }
                if (result.Error) {
                    swal("Mensagem", result.Error, "warning");
                }
            },
            error: function error(XMLHttpRequest, textStatus, errorThrown) {
                swal("Mensagem", errorThrown, "error");
            },
            complete: function complete() {
                Caso.pararCarregamento();
            }
        });
    };

    Caso.inicializar = function () {
        var casoId = $("#menu-dadospessoais").data("casoId");
        if (casoId === 0) {
            Caso.dadosPessoais();
        }
        else {
            Caso.dadosPessoais(casoId);
            Caso.desabilitarCpf();
            Caso.habilitarEnvioQuestionario();
            Caso.habilitarEnvioDocumento();
            Caso.habilitarImpressao();
            Caso.habilitarAcompanhamento();
            Caso.habilitarTarefas();
            Caso.carregarQuestionario(casoId);
            Caso.carregarDocumentos(casoId);
            Caso.carregarAcompanhamentos(casoId);
        }
    };

    Caso.Controles = {
        CarregarPaginaGrupoFamiliar: function CarregarPaginaGrupoFamiliar() {
            var urlCarregarGruposFamiliaresNaoExistente = $("#gruposfamiliares").data("url");
            var urlCarregarGruposFamiliaresExistente = $("#CasoId").data("url");
            var quantidadeGrupoFamiliar = $("#CasoId").data("count");

            var inputTipoBeneficio = $("#TipoBeneficioId");
            var tipoBeneficioId = inputTipoBeneficio.val();
            var url = inputTipoBeneficio.data("url");

            $.ajax({
                type: "GET",
                url: url,
                data: { tipoBeneficioId: tipoBeneficioId },
                success: function success(result) {

                    if (result.Success === true) {

                        if (quantidadeGrupoFamiliar > 0) {
                            $.ajax({
                                method: "GET",
                                url: urlCarregarGruposFamiliaresExistente,
                                success: function (result) {
                                    $("#gruposfamiliares").html(result);
                                    Caso.adicionarGrupoFamiliar();
                                    Caso.removerGrupoFamiliar();
                                    GlobalMask.carregarMascaras();
                                }
                            });
                        }
                        else {
                            $.ajax({
                                method: "GET",
                                url: urlCarregarGruposFamiliaresNaoExistente,
                                success: function (result) {
                                    $("#gruposfamiliares").html(result);
                                    Caso.adicionarGrupoFamiliar();
                                    Caso.removerGrupoFamiliar();
                                    GlobalMask.carregarMascaras();
                                }
                            });
                        }
                    }

                    if (result.Error) {
                        swal("Mensagem", result.Error, "warning");
                    }
                },
                error: function error(XMLHttpRequest, textStatus, errorThrown) {
                    swal("Mensagem", errorThrown, "error");
                }
            });
        },
        DescarregarPaginaGrupoFamiliar: function DescarregarPaginaGrupoFamiliar() {
            $("#gruposfamiliares").html("");
        }
    };

    Caso.adicionarGrupoFamiliar = function () {
        $(document).on("click", ".add-grupofamiliar", function (e) {
            e.preventDefault();
            var url = $(this).attr("href");
            var form = $(this).closest("form");
            $.ajax({
                method: "POST",
                url: url,
                data: form.serialize(),
                success: function (result) {
                    if (result.Success) {
                        $(".lista-gruposfamiliares").html(result.Success);
                        GlobalMask.carregarMascaras();
                    }
                    if (result.Error) {
                        swal("Mensagem", result.Error, "warning");
                    }
                },
                error: function error(XMLHttpRequest, textStatus, errorThrown) {
                    swal("Mensagem", errorThrown, "error");
                }
            });
        });
    };

    Caso.removerGrupoFamiliar = function () {
        $(document).on("click", ".remove-grupofamiliar", function (e) {
            e.preventDefault();
            var url = $(this).attr("href");
            var form = $(this).closest("form");
            $("#index").val($(this).data("index"));
            $.ajax({
                method: "POST",
                url: url,
                data: form.serialize(),
                success: function (data) {
                    $(".lista-gruposfamiliares").html(data);
                    GlobalMask.carregarMascaras();
                }
            });
        });
    };

    Caso.Eventos = {
        Carregar: function Carregar() {
            $(document).on("change", "#TipoBeneficioId", function () {
                var selecao = $(this);
                var tipoBeneficioId = selecao.val();
                var url = selecao.data("url");
                $.ajax({
                    type: "GET",
                    url: url,
                    data: { tipoBeneficioId: tipoBeneficioId },
                    success: function success(result) {

                        if (result.Success === true) {
                            Caso.Controles.CarregarPaginaGrupoFamiliar();
                        }
                        else {
                            Caso.Controles.DescarregarPaginaGrupoFamiliar();
                        }

                        if (result.Error) {
                            swal("Mensagem", result.Error, "warning");
                        }
                    },
                    error: function error(XMLHttpRequest, textStatus, errorThrown) {
                        swal("Mensagem", errorThrown, "error");
                    }
                });
            });

            $(document).on("blur", "input[name='Cliente.Cpf']", function () {
                var input = $(this);
                var cpf = input.val();
                cpf = cpf.replace('.', '');
                cpf = cpf.replace('.', '');
                cpf = cpf.replace('-', '');
                var result = Caso.validarCpf(cpf);
                if (result === false) {
                    input.val("");
                    $("#ClienteId").val(0);
                    swal("Mensagem", "O CPF informado não é válido!", "warning");
                }
                else {
                    Caso.buscarCpfExistente($("input[name='Cliente.Cpf']").val());
                }
            });

            $(".selectpicker").selectpicker();

            $("a[data-toggle='tab']").on("show.bs.tab", function (e) {
                var $target = $(e.target);
                if ($target.parent().hasClass("disabled")) {
                    swal("Mensagem", "Antes desta etapa preencha e salve todos os dados pessoais.", "warning");
                    return false;
                }
            });

            $(document).on("submit", "form#frmSalvarCasoComDadosPessoais", function (e) {
                e.preventDefault();
                var form = $(this);
                swal({
                    title: 'Deseja realmente salvar as alterações?',
                    text: 'As alterações nos dados só se efetivarão após confirmação',
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Confirmar',
                    cancelButtonText: 'Cancelar',
                    confirmButtonClass: 'confirm-class',
                    cancelButtonClass: 'cancel-class',
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            $.ajax({
                                type: "POST",
                                url: form.attr("action"),
                                data: form.serialize(),
                                success: function success(result) {
                                    if (result.Success) {
                                        Caso.desabilitarCpf();
                                        Caso.habilitarEnvioQuestionario();
                                        Caso.habilitarEnvioDocumento();
                                        Caso.habilitarImpressao();
                                        Caso.habilitarAcompanhamento();
                                        Caso.habilitarTarefas();
                                        Caso.fixarCasoId(result.CasoId);
                                        swal({
                                            title: "Mensagem",
                                            text: result.Success,
                                            type: "success"
                                        }, function () {
                                            Caso.dadosPessoais(result.CasoId);
                                            Caso.carregarQuestionario(result.CasoId);
                                            Caso.carregarDocumentos(result.CasoId);
                                            Caso.carregarAcompanhamentos(result.CasoId);

                                            //Próxima aba após salvar os dados pessoais
                                            $('.nav-tabs a[href="#questionario"]').tab('show')
                                        });

                                    }
                                    if (result.Error) {
                                        swal("Mensagem", result.Error, "warning");
                                    }
                                },
                                error: function error(XMLHttpRequest, textStatus, errorThrown) {
                                    swal("Mensagem", errorThrown, "error");
                                }
                            });
                        } else {
                            swal(
                                'Cancelada',
                                'Operação cancelada com sucesso!',
                                'error'
                            );
                        }
                    });
            });

            $(document).on("submit", "form#frmSalvarCasoComQuestionario", function (e) {
                e.preventDefault();
                $.ajax({
                    type: "POST",
                    url: $(this).attr("action"),
                    data: $(this).serialize(),
                    success: function success(result) {
                        if (result.Success) {

                            swal({
                                title: "Mensagem",
                                text: result.Success,
                                type: "success"
                            }, function () {
                                Caso.carregarQuestionario(result.CasoId);
                            });

                        }
                        if (result.Error) {
                            swal("Mensagem", result.Error, "warning");
                        }
                    },
                    error: function error(XMLHttpRequest, textStatus, errorThrown) {
                        swal("Mensagem", errorThrown, "error");
                    }
                });
            });

            $(document).on("submit", "form#frmSalvarCasoComAnexo", function (e) {
                e.preventDefault();
                var casoId = $("form#frmSalvarCasoComAnexo #CasoId").val();
                var formData = new FormData();
                var arquivo = $("form#frmSalvarCasoComAnexo #ArquivoBinario")[0].files[0];
                formData.append("CasoId", casoId);
                formData.append("ArquivoId", $("form#frmSalvarCasoComAnexo #ArquivoId").val());
                formData.append("TipoArquivoId", $("form#frmSalvarCasoComAnexo #TipoArquivoId").val());
                formData.append("arquivoBinario", arquivo);
                $.ajax({
                    type: "POST",
                    url: $(this).attr("action"),
                    data: formData,
                    dataType: "json",
                    contentType: false,
                    processData: false,
                    success: function success(result) {
                        if (result.Success) {
                            $("#modalAnexarDocumento").modal("hide");
                            swal({
                                title: "Mensagem",
                                text: result.Success,
                                type: "success"
                            }, function () {
                                Caso.carregarDocumentos(casoId);
                            });
                        }
                        if (result.Error) {
                            swal("Mensagem", result.Error, "warning");
                        }
                    },
                    error: function error(XMLHttpRequest, textStatus, errorThrown) {
                        swal("Mensagem", errorThrown, "error");
                    }
                });
            });

            $(document).on("submit", "form#frmSalvarCasoComMovimentacao", function (e) {
                e.preventDefault();
                var formData = new FormData();
                var casoId = $("form#frmSalvarCasoComMovimentacao #CasoId").val();
                var arquivo = $("form#frmSalvarCasoComMovimentacao #ArquivoBinario")[0].files[0];
                formData.append("CasoId", casoId);
                formData.append("Descricao", $("form#frmSalvarCasoComMovimentacao #Descricao").val());
                formData.append("TipoMovimento", $("form#frmSalvarCasoComMovimentacao #TipoMovimento").val());
                formData.append("arquivoBinario", arquivo);
                $.ajax({
                    type: "POST",
                    url: $(this).attr("action"),
                    data: formData,
                    dataType: "json",
                    contentType: false,
                    processData: false,
                    success: function success(result) {
                        $("#modalNovoAcompanhamento").modal("hide");
                        if (result.Success) {
                            swal({
                                title: "Mensagem",
                                text: result.Success,
                                type: "success"
                            }, function () {
                                Caso.carregarAcompanhamentos(result.CasoId);
                            });
                        }
                        if (result.Error) {
                            swal("Mensagem", result.Error, "warning");
                        }
                    },
                    error: function error(XMLHttpRequest, textStatus, errorThrown) {
                        swal("Mensagem", errorThrown, "error");
                    }
                });
            });

            $(document).on("submit", "form#frmSalvarCasoComTarefa", function (e) {
                e.preventDefault();
                $.ajax({
                    type: "POST",
                    url: $(this).attr("action"),
                    data: $(this).serialize(),
                    success: function success(result) {

                        $("#modalNovaTarefa").modal("hide");

                        if (result.Success) {
                            swal("Mensagem", result.Success, "success");
                            Tarefas.atualizarTarefas();
                        }

                        if (result.Error) {
                            swal("Mensagem", result.Error, "warning");
                        }
                    },
                    error: function error(XMLHttpRequest, textStatus, errorThrown) {
                        swal("Mensagem", errorThrown, "error");
                    }
                });
            });

            $(".modal").on("hidden.bs.modal", function () {
                $(this).find('form')[0].reset();
            });
        }
    };

    Caso.Eventos.Carregar();

    Caso.inicializar();

});