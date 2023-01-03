$(function () {

    "use strict";

    window.Tarefas = window.Tarefas || {};

    Tarefas.atualizarTarefas = function () {
        $("#calendar").fullCalendar("refetchEvents");
    };

    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var currentTab = $(e.target).text();
        if (currentTab === "Tarefas") {
            Caso.iniciarCarregamento();
            Tarefas.carregarCalendario();
            Tarefas.atualizarTarefas();
            Caso.pararCarregamento();
        }
    });

    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        todayHighlight: true,
        language: 'pt-BR'
    });

    Tarefas.excluirTarefa = function (tarefaId, url) {
        swal({
            title: 'Deseja realmente excluir esta tarefa?',
            text: 'A exclusão não poderá ser desfeita futuramente',
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
        }, function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    type: "GET",
                    url: url,
                    data: { tarefaId: tarefaId },
                    success: function success(result) {
                        if (result.success) {
                            $("#modalDetalhesTarefa").modal("hide");
                            Tarefas.atualizarTarefas();
                            swal("Mensagem", result.success, "success");
                        }
                        if (result.error) {
                            swal("Mensagem", result.error, "warning");
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
    };

    Tarefas.assumirTarefa = function (tarefaId, url) {
        $.ajax({
            type: "GET",
            url: url,
            data: { tarefaId: tarefaId },
            success: function success(result) {
                $("#modalDetalhesTarefa").modal("hide");
                Tarefas.atualizarTarefas();
                swal("Mensagem", result.success, "success");
            },
            error: function error(XMLHttpRequest, textStatus, errorThrown) {
                swal("Mensagem", errorThrown, "error");
            }
        });
    };

    Tarefas.detalhesTarefa = function (tarefaId) {
        var $modal = $("#modalDetalhesTarefa");
        $.ajax({
            type: "POST",
            url: $modal.data("url"),
            data: { tarefaId: tarefaId },
            success: function success(result) {
                $modal.html(result);
                $modal.modal("show");
            },
            error: function error(XMLHttpRequest, textStatus, errorThrown) {
                swal("Mensagem", errorThrown, "error");
            }
        });
    };

    Tarefas.alterarPrazo = function (tarefaId, justificativa, novaDataExpiracao) {

    };

    Tarefas.alterarResponsavel = function (tarefaId, justificativa, novoFuncionarioResponsavelId) {

    };

    Tarefas.carregarEventos = function () {

        $(document).on("submit", "form#frmFinalizarTarefa", function (e) {
            e.preventDefault();
            $.ajax({
                type: "POST",
                url: $(this).attr("action"),
                data: $(this).serialize(),
                success: function success(result) {

                    $("#modalFinalizarTarefa").modal("hide");

                    if (result.success) {
                        swal("Mensagem", result.success, "success");
                        Tarefas.atualizarTarefas();
                    }

                    if (result.error) {
                        swal("Mensagem", result.error, "warning");
                    }
                },
                error: function error(XMLHttpRequest, textStatus, errorThrown) {
                    swal("Mensagem", errorThrown, "error");
                }
            });
        });

        $(document).on("submit", "form#frmAlteracaoResponsavel", function (e) {
            e.preventDefault();
            $.ajax({
                type: "POST",
                url: $(this).attr("action"),
                data: $(this).serialize(),
                success: function success(result) {

                    $("#modalAlterarResponsavel").modal("hide");

                    if (result.success) {
                        swal("Mensagem", result.success, "success");
                        Tarefas.atualizarTarefas();
                    }

                    if (result.error) {
                        swal("Mensagem", result.error, "warning");
                    }
                },
                error: function error(XMLHttpRequest, textStatus, errorThrown) {
                    swal("Mensagem", errorThrown, "error");
                }
            });
        });

        $(document).on("submit", "form#frmAlterarPrazo", function (e) {
            e.preventDefault();
            $.ajax({
                type: "POST",
                url: $(this).attr("action"),
                data: $(this).serialize(),
                success: function success(result) {

                    $("#modalAlterarPrazo").modal("hide");

                    if (result.success) {
                        swal("Mensagem", result.success, "success");
                        Tarefas.atualizarTarefas();
                    }

                    if (result.error) {
                        swal("Mensagem", result.error, "warning");
                    }
                },
                error: function error(XMLHttpRequest, textStatus, errorThrown) {
                    swal("Mensagem", errorThrown, "error");
                }
            });
        });

    };

    Tarefas.carregarCalendario = function () {

        if ($('#calendar').length) {

            $('#calendar').fullCalendar({

                locale: 'pt-br',
                eventRender: function (eventObj, $el) {
                    $el.popover({
                        title: eventObj.title,
                        content: eventObj.description,
                        trigger: 'hover',
                        placement: 'bottom',
                        container: 'body'
                    });

                    if (eventObj.author) {
                        $el.find(".fc-title").prepend("<i style='cursor: help' title='Responsável: " + eventObj.author + "' class='fa fa-" + "user" + "'></i> ");
                    }
                },

                eventClick: function (info) {
                    Tarefas.detalhesTarefa(info.id);
                },

                dayClick: function (date, allDay, jsEvent, view) {
                    var dataFormatada = date.format('DD/MM/YYYY');
                    Caso.novaTarefaComData(dataFormatada);
                },

                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,listYear'
                },

                buttonIcons: {
                    prev: 'none fa fa-angle-left',
                    next: 'none fa fa-angle-right',
                    prevYear: 'none fa fa-angle-left',
                    nextYear: 'none fa fa-angle-right'
                },

                editable: true,
                eventLimit: true,
                navLinks: true,
                droppable: true,

                eventDrop: function (info) {
                    console.log(info);

                    Tarefas.atualizarTarefas();

                    //alert(info.event.title + " was dropped on " + info.event.start.toISOString());
                    //if (!confirm("Are you sure about this change?")) {
                    //    info.revert();
                    //}
                },

                events: {
                    url: $("#calendar").data("url") + "?casoId=" + $("#CasoId").val(),
                    error: function () {
                        Caso.pararCarregamento();
                        alert('erro');
                    }
                }
            });
        }
    };

    Tarefas.carregarEventos();

});