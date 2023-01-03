using Advogado.Data;
using Advogado.Helpers;
using Advogado.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Advogado.Controllers
{
    [Authorize(Roles = "Administrador, Secretaria")]
    public class CasoController : Controller
    {
        private readonly MainContext _db = new MainContext();

        public ActionResult Index()
        {
            var list = _db.Casos.OrderByDescending(x => x.DataAbertura).Include(x => x.Cliente).Include(x => x.TipoBeneficio).ToList();
            return View(list);
        }

        public ActionResult Novo()
        {
            ViewBag.Responsaveis = _db.Funcionarios.ToList();
            return View();
        }

        public ActionResult Editar(int id)
        {
            ViewBag.Responsaveis = _db.Funcionarios.ToList();
            return View(new Caso { CasoId = id });
        }

        public void ValidarEstrangeiro(Caso model)
        {
            if (model.Cliente.ClienteEstrangeiro == true)
            {
                if (string.IsNullOrEmpty(model.Cliente.DocumentoIdentificacaoEstrangeiro)) throw new Exception("Documento do estrangeiro não preenchido!");
            }
            else
            {
                if (string.IsNullOrEmpty(model.Cliente.Rg)) throw new Exception("RG não preenchido!");
                if (model.Cliente.OrgaoExpedidorRg == 0) throw new Exception("Orgão expedidor do RG não preenchido!");
                if (model.Cliente.EstadoOrgaoExpedidor == 0) throw new Exception("Estado emissor do RG não preenchido!");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarCasoComDadosPessoais(Caso model)
        {
            try
            {
                ValidarEstrangeiro(model);
                ModelState.Remove("Nome");
                ModelState.Remove("Cpf");
                ModelState.Remove("DataNascimento");
                ModelState.Remove("GrauParentesco");
                if (model.CasoId == 0) ModelState.Remove("CasoId");
                if (model.CasoId != 0) ModelState.Remove("Cliente.Cpf");
                if (model.ClienteId != 0) ModelState.Remove("Cliente.Cpf");
                if (!ModelState.IsValid) throw new Exception("Preencha todos os campos corretamente!");
                if (model.ClienteId == 0 && !model.Cliente.ValidarCpf()) throw new Exception("O CPF informado é inválido!");
                if (model.CasoId == 0)
                {
                    var cliente = model.ClienteId != 0 ? _db.Clientes.Find(model.ClienteId) : null;

                    if (model.ClienteId != 0)
                    {                       
                        cliente.Atualizar(model.Cliente);
                        _db.Entry(cliente).State = EntityState.Modified;
                        model.Cliente = null;
                    }

                    model.DataAbertura = DateTime.Now;
                    model.SituacaoCaso = SituacaoCaso.Pendente;
                    model.Ativo = true;

                    var tarefas = new Collection<Tarefa>
                    {
                        new Tarefa
                        {
                            Titulo = $"Preencher todo o questionário do caso para o cliente {(cliente != null ? cliente.Nome : model.Cliente.Nome)}",
                            Descricao = "Esta tarefa será finalizada quando o questionário do caso for completado",
                            DataCriacao = DateTime.Now,
                            DataExpiracao = DateTime.Now.AddDays(PrazosConfig.PrazoDiasEntregaQuestionario),
                            TipoTarefa = TipoTarefa.EntregaQuestionario                            
                        },
                        new Tarefa
                        {
                            Titulo = $"Anexar toda a documentação ao caso do cliente {(cliente != null ? cliente.Nome : model.Cliente.Nome)}",
                            Descricao = "Esta tarefa será finalizada quando toda a documentação do caso for anexada",
                            DataCriacao = DateTime.Now,
                            DataExpiracao = DateTime.Now.AddDays(PrazosConfig.PrazoDiasEntregaDocumentacao),
                            TipoTarefa = TipoTarefa.EntregaDocumentacao
                        },
                        new Tarefa
                        {
                            Titulo = $"Protocolar o processo do cliente {(cliente != null ? cliente.Nome : model.Cliente.Nome)} administrativamente no INSS",
                            Descricao = "Por favor finalizar esta tarefa quando estiver pronta",
                            DataCriacao = DateTime.Now,
                            DataExpiracao = DateTime.Now.AddDays(PrazosConfig.PrazoDiasProtocoloAdministrativo),
                            TipoTarefa = TipoTarefa.ProtocoloAdministrativo
                        },
                        new Tarefa
                        {
                             Titulo = $"Protocolar o processo do cliente {(cliente != null ? cliente.Nome : model.Cliente.Nome)} judicialmente",
                            Descricao = "Por favor finalizar esta tarefa quando estiver pronta",
                            DataCriacao = DateTime.Now,
                            DataExpiracao = DateTime.Now.AddDays(PrazosConfig.PrazoDiasProtocoloJudicial),
                            TipoTarefa = TipoTarefa.ProtocoloJudicial
                        }
                    };

                    model.Tarefas = tarefas;
                    var retorno = _db.Casos.Add(model);
                    _db.SaveChanges();
                    return Json(new { Success = "Os dados pessoais foram salvos com sucesso!", retorno.CasoId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var novo = _db.Casos.Include(x => x.Cliente).Include(x => x.GruposFamiliares).First(x => x.CasoId == model.CasoId);
                    BalancearListaGruposFamiliares(novo, model);
                    novo.AtualizarCaso(model);
                    novo.AtualizarCliente(model);
                    _db.Entry(novo).State = EntityState.Modified;
                    _db.SaveChanges();
                    return Json(new { Success = "Os dados pessoais foram alterados com sucesso!", novo.CasoId, novo.ClienteId }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarCasoComQuestionario(List<Resposta> Respostas, int casoId)
        {
            try
            {
                var respostasNovas = Respostas.Where(x => x.RespostaId == 0);
                var respostasExistentes = Respostas.Where(x => x.RespostaId != 0);
                respostasNovas = respostasNovas.Where(x => x.RespostaDescritiva?.Trim() != "" || x.AlternativaId != 0 || (x.AlternativasIds != null && x.AlternativasIds.Any())).ToList();
                respostasExistentes = respostasExistentes.Where(x => x.RespostaDescritiva?.Trim() != "" || x.AlternativaId != 0 || (x.AlternativasIds != null && x.AlternativasIds.Any())).ToList();
                foreach (var respostaNova in respostasNovas)
                {
                    if (respostaNova.AlternativaId != 0)
                    {
                        respostaNova.Alternativas = new Collection<Alternativa>
                        {
                            _db.Alternativas.Find(respostaNova.AlternativaId)
                        };
                    }
                    if (respostaNova.AlternativasIds != null && respostaNova.AlternativasIds.Any())
                    {
                        respostaNova.Alternativas = new Collection<Alternativa>();
                        foreach (var alternativaId in respostaNova.AlternativasIds)
                        {
                            respostaNova.Alternativas.Add(_db.Alternativas.Find(alternativaId));
                        }
                    }
                }
                foreach (var respostaExistente in respostasExistentes)
                {
                    if (respostaExistente.AlternativaId != 0)
                    {
                        respostaExistente.Alternativas = new Collection<Alternativa>
                        {
                            _db.Alternativas.Find(respostaExistente.AlternativaId)
                        };
                    }
                    if (respostaExistente.AlternativasIds != null && respostaExistente.AlternativasIds.Any())
                    {
                        respostaExistente.Alternativas = new Collection<Alternativa>();
                        foreach (var alternativaId in respostaExistente.AlternativasIds)
                        {
                            respostaExistente.Alternativas.Add(_db.Alternativas.Find(alternativaId));
                        }
                    }
                }
                if (respostasNovas != null && respostasNovas.Any()) _db.Respostas.AddRange(respostasNovas);
                if (respostasExistentes != null && respostasExistentes.Any())
                {
                    foreach (var respostaNova in respostasExistentes)
                    {
                        var pergunta = _db.Perguntas.Find(respostaNova.PerguntaId);
                        if (pergunta.MultiplaAlternativa == false)
                        {
                            var respostaAntiga = _db.Respostas.Find(respostaNova.RespostaId);
                            respostaAntiga.AtualizarRespostaDescritiva(respostaNova.RespostaDescritiva);
                            _db.Entry(respostaAntiga).State = EntityState.Modified;
                        }
                        else
                        {
                            var respostaAntiga = _db.Respostas.Include(x => x.Alternativas).First(x => x.RespostaId == respostaNova.RespostaId);

                            if (respostaNova.Alternativas == null || !respostaNova.Alternativas.Any())
                            {
                                respostaAntiga.Alternativas = null;
                            }
                            else
                            {
                                BalancearListaAlternativas(respostaAntiga, respostaNova);

                            }
                        }
                    }
                }

                var questoesObrigatorias = _db.Perguntas.Where(x => x.TiposBeneficios.Any(y => y.Casos.Any(z => z.CasoId == casoId)) && x.Ativo && x.Obrigatoria);
                var quantidadeQuestoesObrigatorias = questoesObrigatorias.Count();
                var respostasValidasObrigatorias = Respostas.Where(x => questoesObrigatorias.Select(y => y.PerguntaId).Contains(x.PerguntaId) && (!string.IsNullOrEmpty(x.RespostaDescritiva) || x.AlternativaId != 0 || (x.AlternativasIds != null && x.AlternativasIds.Any())));
                var quantidadeRespostasValidasObrigatorias = respostasValidasObrigatorias.Count();

                if (quantidadeQuestoesObrigatorias == quantidadeRespostasValidasObrigatorias)
                {
                    var encontrarTarefa = _db.Tarefas.FirstOrDefault(x => x.TipoTarefa == TipoTarefa.EntregaQuestionario && x.CasoId == casoId);
                    if (encontrarTarefa != null && !encontrarTarefa.Finalizada)
                    {
                        encontrarTarefa.Solucao = "Tarefa atendida com o preenchimento do questionário completo";
                        encontrarTarefa.DataFinalizacao = DateTime.Now;
                        encontrarTarefa.FuncionarioResponsavelId = Funcionario.FuncionarioLogado().FuncionarioId;
                        encontrarTarefa.Finalizada = true;
                    }
                }

                _db.SaveChanges();
                return Json(new { Success = "O questionário foi salvo com sucesso!", CasoId = casoId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SalvarCasoComAnexo(Arquivo model, HttpPostedFileBase arquivoBinario)
        {
            try
            {
                if (arquivoBinario == null) throw new Exception("Por favor anexe o arquivo!");
                model.Nome = arquivoBinario.FileName;
                model = AnexarDocumento(model, arquivoBinario);
                if (model.ArquivoId == 0)
                {
                    _db.Arquivos.Add(model);
                }
                else
                {
                    var novo = _db.Arquivos.Find(model.ArquivoId);
                    novo.AtualizarDocumento(model);
                    _db.Entry(novo).State = EntityState.Modified;
                }

                var tiposArquivosObrigatorios = _db.TiposArquivos.Where(x => x.Obrigatorio && x.TiposBeneficios.Any(y => y.Casos.Any(z => z.CasoId == model.CasoId)) && x.Ativo);

                var arquivosAnexadosObrigatorios = _db.Arquivos.Where(x => x.CasoId == model.CasoId && x.TipoArquivo.Obrigatorio);

                var tipoDeArquivoAnexado = _db.TiposArquivos.Find(model.TipoArquivoId);

                var quantidadeTiposArquivos = tiposArquivosObrigatorios.Count();

                var quantidadeArquivosAnexados = arquivosAnexadosObrigatorios.Count() + (tipoDeArquivoAnexado.Obrigatorio ? 1 : 0);

                if (quantidadeTiposArquivos == quantidadeArquivosAnexados)
                {
                    var encontrarTarefa = _db.Tarefas.FirstOrDefault(x => x.TipoTarefa == TipoTarefa.EntregaDocumentacao && x.CasoId == model.CasoId);
                    if (encontrarTarefa != null && !encontrarTarefa.Finalizada)
                    {
                        encontrarTarefa.Solucao = "Tarefa atendida com o anexo de toda a documentação do caso";
                        encontrarTarefa.DataFinalizacao = DateTime.Now;
                        encontrarTarefa.FuncionarioResponsavelId = Funcionario.FuncionarioLogado().FuncionarioId;
                        encontrarTarefa.Finalizada = true;
                    }
                }

                _db.SaveChanges();
                return Json(new { Success = "Documento anexado com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SalvarCasoComMovimentacao(Movimentacao model, HttpPostedFileBase arquivoBinario)
        {
            try
            {
                model.Data = DateTime.Now;
                if (arquivoBinario != null) model = AnexarMovimentacao(model, arquivoBinario);
                model.FuncionarioId = Funcionario.FuncionarioLogado().FuncionarioId;
                if (model.MovimentacaoId == 0)
                {
                    _db.Movimentacoes.Add(model);
                }
                else
                {
                    var novo = _db.Movimentacoes.Find(model.MovimentacaoId);
                    novo.AtualizarMovimentacao(model);
                    _db.Entry(novo).State = EntityState.Modified;
                }
                _db.SaveChanges();
                return Json(new { Success = "Documento anexado com sucesso!", model.CasoId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SalvarCasoComTarefa(Tarefa model)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Campos inválidos!");
                model.FuncionarioCriadorId = Funcionario.FuncionarioLogado().FuncionarioId;
                model.DataCriacao = DateTime.Now;
                model.TipoTarefa = TipoTarefa.Comum;
                _db.Tarefas.Add(model);
                _db.SaveChanges();
                return Json(new { Success = "Tarefa salva com sucesso!", model.CasoId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Caso model)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception();
                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index").Success();
            }
            catch (Exception e)
            {
                return View(model).Error(e.Message);
            }
        }

        public ActionResult Remover(int id)
        {
            try
            {
                //Arquivos do caso para excluir
                var arquivosDoCaso = _db.Arquivos.Where(x => x.CasoId == id);

                //Movimentações do caso para excluir
                var movimentacoesDoCaso = _db.Movimentacoes.Where(x => x.CasoId == id);

                //Grupo familiar do caso para excluir
                var grupoFamiliarDoCaso = _db.GruposFamiliares.Where(x => x.CasoId == id);

                //Tarefas do caso para excluir
                var tarefasDoCaso = _db.Tarefas.Where(x => x.CasoId == id);

                //Justificativa das tarefas do caso para excluir
                var justificativasTarefasDoCaso = _db.JustificativasTarefas.Where(x => x.Tarefa.CasoId == id);

                //Respostas do caso para excluir
                var respostasDoCaso = _db.Respostas.Include(x => x.Alternativas).Where(x => x.CasoId == id);                

                var caso = _db.Casos.Find(id);

                _db.Arquivos.RemoveRange(arquivosDoCaso);

                _db.Movimentacoes.RemoveRange(movimentacoesDoCaso);

                _db.GruposFamiliares.RemoveRange(grupoFamiliarDoCaso);

                _db.JustificativasTarefas.RemoveRange(justificativasTarefasDoCaso);

                _db.Tarefas.RemoveRange(tarefasDoCaso);    

                _db.Respostas.RemoveRange(respostasDoCaso);

                _db.Entry(caso).State = EntityState.Deleted;

                _db.SaveChanges();

                return RedirectToAction("Index").Success();
            }
            catch (Exception e)
            {
                return RedirectToAction("Index").Error(e.Message);
            }
        }

        public ActionResult DadosPessoais(int? casoId = 0)
        {
            try
            {
                if (casoId != 0)
                {
                    var caso = _db.Casos.Include(x => x.Cliente).FirstOrDefault(x => x.CasoId == casoId);
                    SetViewBag();
                    return PartialView("_DadosPessoais", caso);
                }
                else

                {
                    SetViewBag();
                    return PartialView("_DadosPessoais");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ActionResult Questionario(int casoId)
        {
            try
            {
                var caso = _db.Casos.Find(casoId);

                var perguntasDoCaso = _db.Perguntas.Include(x => x.Alternativas).Where(x => x.Ativo && x.TiposBeneficios.Any(y => y.TipoBeneficioId == caso.TipoBeneficioId)).ToList();
                var respostasDoCaso = _db.Respostas.Include(x => x.Alternativas).Where(x => x.CasoId == casoId).ToList();
                var perguntasOrdens = _db.PerguntasOrdens.Where(x => x.TipoBeneficioId == caso.TipoBeneficioId).ToList();

                //Alimenta as perguntas com respostas
                foreach (var perguntaDoCaso in perguntasDoCaso)
                {
                    var respostaDoCaso = respostasDoCaso.FirstOrDefault(x => x.PerguntaId == perguntaDoCaso.PerguntaId);
                    if (respostaDoCaso != null)
                    {
                        perguntaDoCaso.Respostas = new List<Resposta>();
                        perguntaDoCaso.Respostas.Add(respostaDoCaso);
                    }
                }

                //Ordena de acordo com o que foi definido
                if (perguntasOrdens != null && perguntasOrdens.Any())
                {
                    foreach (var pergunta in perguntasDoCaso)
                    {
                        var perguntaOrdem = perguntasOrdens.FirstOrDefault(x => pergunta.PerguntaId == x.PerguntaId);
                        if (perguntaOrdem != null)
                            pergunta.Ordem = perguntaOrdem.Ordem;
                    }
                }

                //Ordena as perguntas
                var perguntasOrdenadas = (perguntasOrdens != null && perguntasOrdens.Any()) ? perguntasDoCaso.OrderBy(x => x.Ordem).ToList() : perguntasDoCaso;

                ViewBag.CasoId = casoId;             

                return PartialView("_Questionario", perguntasOrdenadas);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ActionResult Documentos(int casoId)
        {
            try
            {
                var caso = _db.Casos.Find(casoId);

                var tiposArquivos = _db.TiposArquivos.Where(x => x.TiposBeneficios.Any(y => y.Casos.Any(z => z.CasoId == casoId)) && x.Ativo).ToList();

                var documentosOrdens = _db.TiposArquivosOrdens.Where(x => x.TipoBeneficioId == caso.TipoBeneficioId).ToList();

                //Alimenta os arquivos
                foreach (var tipoArquivo in tiposArquivos)
                {
                    var arquivo = _db.Arquivos.Where(x => x.TipoArquivoId == tipoArquivo.TipoArquivoId && x.CasoId == casoId).FirstOrDefault();
                    if (arquivo != null)
                    {
                        tipoArquivo.Arquivos = new List<Arquivo>();
                        tipoArquivo.Arquivos.Add(arquivo);
                    }
                }               

                //Ordena de acordo com o que foi definido
                if (documentosOrdens != null && documentosOrdens.Any())
                {
                    foreach (var tipoArquivo in tiposArquivos)
                    {
                        var tipoArquivoOrdem = documentosOrdens.FirstOrDefault(x => tipoArquivo.TipoArquivoId == x.TipoArquivoId);
                        if (tipoArquivoOrdem != null)
                            tipoArquivo.Ordem = tipoArquivoOrdem.Ordem;
                    }
                }

                //Ordena os documentos
                var documentosOrdenados = (documentosOrdens != null && documentosOrdens.Any()) ? tiposArquivos.OrderBy(x => x.Ordem).ToList() : tiposArquivos;

                return PartialView("_Documentos", documentosOrdenados);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ActionResult Movimentos(int casoId)
        {
            try
            {
                var movimentos = _db.Movimentacoes.Include(x => x.Funcionario).Where(x => x.CasoId == casoId).OrderByDescending(x => x.Data).ToList();
                return PartialView("_Movimentos", movimentos);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ActionResult ImprimirProcuracao(int casoId)
        {
            try
            {
                if (casoId == 0) throw new Exception("Ocorreu um erro ao tentar imprimir a procuração do caso.");
                var caso = _db.Casos.Include(x => x.Cliente).Include(x => x.RepresentanteLegal).Include(x => x.TipoBeneficio).FirstOrDefault(x => x.CasoId == casoId);
                return Json(new { Success = "Dados da impressão carregados com sucesso!", Model = caso }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ImprimirContrato(int casoId, string remuneracao)
        {
            try
            {
                if (casoId == 0) throw new Exception("Ocorreu um erro ao tentar imprimir a procuração do caso.");
                var caso = _db.Casos.Include(x => x.Cliente).Include(x => x.RepresentanteLegal).Include(x => x.TipoBeneficio).FirstOrDefault(x => x.CasoId == casoId);
                caso.TipoBeneficio.Remuneracao = remuneracao;
                return Json(new { Success = "Dados da impressão carregados com sucesso!", Model = caso }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ImprimirDeclaracaoResidencia(int casoId)
        {
            try
            {
                if (casoId == 0) throw new Exception("Ocorreu um erro ao tentar imprimir a declaração de residência.");
                var caso = _db.Casos.Include(x => x.Cliente).Include(x => x.RepresentanteLegal).FirstOrDefault(x => x.CasoId == casoId);               
                return Json(new { Success = "Dados da impressão carregados com sucesso!", Model = caso }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ImprimirFichaAtendimento(int casoId)
        {
            try
            {
                if (casoId == 0) throw new Exception("Ocorreu um erro ao tentar imprimir a ficha de atendimento.");

                var caso = _db.Casos.Include(x => x.Cliente).Include(x => x.RepresentanteLegal).Include(x => x.Respostas.Select(y => y.Pergunta)).Include(x => x.Respostas.Select(y => y.Alternativas)).FirstOrDefault(x => x.CasoId == casoId);

                var perguntasOrdens = _db.PerguntasOrdens.Where(x => x.TipoBeneficioId == caso.TipoBeneficioId).ToList();

                //Ordena de acordo com o que foi definido
                if (perguntasOrdens != null && perguntasOrdens.Any())
                {
                    foreach (var resposta in caso.Respostas)
                    {
                        var perguntaOrdem = perguntasOrdens.FirstOrDefault(x => resposta.Pergunta.PerguntaId == x.PerguntaId);
                        if (perguntaOrdem != null)
                            resposta.Pergunta.Ordem = perguntaOrdem.Ordem;
                    }
                    var listaRespostas = caso.Respostas.OrderBy(x => x.Pergunta.Ordem).ToList();
                    caso.Respostas = new Collection<Resposta>(listaRespostas);
                }

                return Json(new { Success = "Dados da impressão carregados com sucesso!", Model = caso }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BuscarCpfExistente(string cpf)
        {
            try
            {
                var cliente = _db.Clientes.FirstOrDefault(x => x.Cpf == cpf);
                return Json(new { Success = "Dados da impressão carregados com sucesso!", JaExiste = cliente != null ? true : false, Model = cliente }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BuscarRemuneracaoDoTipoDeBeneficio(int tipoBeneficioId)
        {
            try
            {
                var remuneracao = _db.TiposBeneficios.Find(tipoBeneficioId).Remuneracao;
                return Json(new { Success = remuneracao }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void BalancearListaAlternativas(Resposta respostaAntiga, Resposta respostaNova)
        {
            var alternativasIds = respostaNova.Alternativas.Select(x => x.AlternativaId).ToArray();
            respostaNova.Alternativas = new Collection<Alternativa>();
            respostaAntiga.Alternativas = respostaAntiga.Alternativas ?? new Collection<Alternativa>();
            var alternativas = _db.Alternativas.ToList();
            foreach (var alternativa in alternativas)
            {
                if (alternativasIds != null && alternativasIds.Any(x => x.Equals(alternativa.AlternativaId)))
                {
                    respostaNova.Alternativas.Add(_db.Alternativas.FirstOrDefault(x => x.AlternativaId == alternativa.AlternativaId));
                }
            }
            foreach (var alternativa in alternativas)
            {
                var alternativaAntiga = respostaAntiga.Alternativas.FirstOrDefault(x => x.AlternativaId == alternativa.AlternativaId);
                if (alternativasIds != null && alternativasIds.Contains(alternativa.AlternativaId) && alternativaAntiga == null)
                {
                    respostaAntiga.Alternativas.Add(_db.Alternativas.FirstOrDefault(x => x.AlternativaId == alternativa.AlternativaId));
                }
                else
                {
                    if (alternativaAntiga == null) continue;
                    if (alternativasIds != null && alternativasIds.Contains(alternativaAntiga.AlternativaId)) continue;
                    respostaAntiga.Alternativas.Remove(alternativaAntiga);
                }
            }
        }

        private Arquivo AnexarDocumento(Arquivo model, HttpPostedFileBase arquivoBinario)
        {
            int ByteSize = 1048576;
            int SizeInBytesMax = ByteSize * 10;
            var virtualPath = "~/Uploads/Documentos";
            var allowedExtensions = new List<string> { ".pdf", ".docx", ".doc" };
            var arquivo = new Arquivo();
            if (model.ArquivoId != 0) arquivo = _db.Arquivos.Find(model.ArquivoId);
            if (arquivoBinario != null && arquivoBinario.ContentLength > 0)
            {
                int sizeByte = arquivoBinario.ContentLength;
                if (!(sizeByte <= SizeInBytesMax)) throw new Exception("Limite de envio de arquivo atingido!");
                string extension = Path.GetExtension(arquivoBinario.FileName);
                if (!allowedExtensions.Contains(extension.ToLower())) throw new Exception("Extensão não permitida");
                var physicalPath = Server.MapPath(virtualPath);
                if (!Directory.Exists(physicalPath)) Directory.CreateDirectory(physicalPath);
                if (!Directory.Exists(physicalPath)) throw new Exception($"Diretório {physicalPath} não existe!");
                string fileName = $"{Guid.NewGuid()}{extension}";
                string filePath = Path.Combine(physicalPath, fileName);
                if (model.ArquivoId != 0)
                {
                    var fileOld = Server.MapPath(arquivo.Caminho);
                    if (System.IO.File.Exists(fileOld)) System.IO.File.Delete(fileOld);
                }
                arquivoBinario.SaveAs(filePath);
                model.Caminho = $"{virtualPath}/{fileName}";
            }
            else if (model.ArquivoId != 0)
            {
                var fileOld = Server.MapPath(arquivo.Caminho);
                if (System.IO.File.Exists(fileOld)) System.IO.File.Delete(fileOld);
                model.Caminho = string.Empty;
            }
            return model;
        }

        private Movimentacao AnexarMovimentacao(Movimentacao model, HttpPostedFileBase arquivoBinario)
        {
            int ByteSize = 1048576;
            int SizeInBytesMax = ByteSize * 10;
            var virtualPath = "~/Uploads/AnexosMovimentacao";
            var allowedExtensions = new List<string> { ".pdf", ".docx", ".doc" };
            var movimentacao = new Movimentacao();
            if (model.MovimentacaoId != 0) movimentacao = _db.Movimentacoes.Find(model.MovimentacaoId);
            if (arquivoBinario != null && arquivoBinario.ContentLength > 0)
            {
                int sizeByte = arquivoBinario.ContentLength;
                if (!(sizeByte <= SizeInBytesMax)) throw new Exception("Limite de envio de arquivo atingido!");
                string extension = Path.GetExtension(arquivoBinario.FileName);
                if (!allowedExtensions.Contains(extension.ToLower())) throw new Exception("Extensão não permitida");
                var physicalPath = Server.MapPath(virtualPath);
                if (!Directory.Exists(physicalPath)) Directory.CreateDirectory(physicalPath);
                if (!Directory.Exists(physicalPath)) throw new Exception($"Diretório {physicalPath} não existe!");
                string fileName = $"{Guid.NewGuid()}{extension}";
                string filePath = Path.Combine(physicalPath, fileName);
                if (model.MovimentacaoId != 0)
                {
                    var fileOld = Server.MapPath(movimentacao.CaminhoAnexo);
                    if (System.IO.File.Exists(fileOld)) System.IO.File.Delete(fileOld);
                }
                arquivoBinario.SaveAs(filePath);
                model.CaminhoAnexo = $"{virtualPath}/{fileName}";
                model.NomeAnexo = arquivoBinario.FileName;
            }
            else if (model.MovimentacaoId != 0)
            {
                var fileOld = Server.MapPath(movimentacao.CaminhoAnexo);
                if (System.IO.File.Exists(fileOld)) System.IO.File.Delete(fileOld);
                model.CaminhoAnexo = string.Empty;
                model.NomeAnexo = string.Empty;
            }
            return model;
        }

        public ActionResult GruposFamiliares()
        {
            return PartialView("_GruposFamiliares");
        }

        public ActionResult AtualizarGruposFamiliares(int id)
        {
            var caso = _db.Casos.Include(x => x.GruposFamiliares).FirstOrDefault(x => x.CasoId == id);
            return PartialView("_GruposFamiliares", caso);
        }

        public ActionResult ListaGruposFamiliares(Caso casoGrupoFamiliar)
        {
            casoGrupoFamiliar = casoGrupoFamiliar ?? new Caso();
            SetarUrl();
            return PartialView("_ListaGruposFamiliares", casoGrupoFamiliar.GruposFamiliares?.Where(x => x.Ativo).ToList() ?? casoGrupoFamiliar.GruposFamiliares?.ToList());
        }

        [HttpPost]
        public ActionResult AdicionarGrupoFamiliar(Caso casoGrupoFamiliar)
        {
            try
            {
                if (string.IsNullOrEmpty(casoGrupoFamiliar.Nome)) throw new Exception("Preencha todos os campos do grupo familiar corretamente!");
                if (string.IsNullOrEmpty(casoGrupoFamiliar.Cpf)) throw new Exception("Preencha todos os campos do grupo familiar corretamente!");
                if (casoGrupoFamiliar.DataNascimento == null) throw new Exception("Preencha todos os campos do grupo familiar corretamente!");
                if (string.IsNullOrEmpty(casoGrupoFamiliar.GrauParentesco)) throw new Exception("Preencha todos os campos do grupo familiar corretamente!");

                casoGrupoFamiliar.GruposFamiliares = casoGrupoFamiliar.GruposFamiliares ?? new Collection<GrupoFamiliar>();
                casoGrupoFamiliar.GruposFamiliares.Add
                (
                    new GrupoFamiliar
                    {
                        Nome = casoGrupoFamiliar.Nome,
                        Cpf = casoGrupoFamiliar.Cpf,
                        DataNascimento = casoGrupoFamiliar.DataNascimento,
                        GrauParentesco = casoGrupoFamiliar.GrauParentesco,
                        CasoId = casoGrupoFamiliar.CasoId
                    }
                );
                casoGrupoFamiliar.Nome = string.Empty;
                casoGrupoFamiliar.Cpf = string.Empty;
                casoGrupoFamiliar.GrauParentesco = string.Empty;
                SetarUrl();

                var html = PartialView("_ListaGruposFamiliares", casoGrupoFamiliar.GruposFamiliares.ToList()).RenderToString();

                return Json(new { Success = html }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult RemoverGrupoFamiliar(Caso casoGrupoFamiliar, int index)
        {
            var gruposfamiliares = casoGrupoFamiliar.GruposFamiliares;
            gruposfamiliares.RemoveAt(index);
            casoGrupoFamiliar.GruposFamiliares = gruposfamiliares;
            SetarUrl();
            return PartialView("_ListaGruposFamiliares", casoGrupoFamiliar.GruposFamiliares.ToList());
        }

        public ContentResult QuantidadeGrupoFamiliar(int casoId)
        {
            if (casoId == 0) return Content("0");
            var quantidade = _db.GruposFamiliares.Count(x => x.CasoId == casoId);
            return Content(quantidade.ToString());
        }

        public ActionResult VerificarSeTipoDeBeneficioTemGrupoFamiliar(int tipoBeneficioId)
        {
            try
            {
                var possuiGrupoFamiliar = _db.TiposBeneficios.First(x => x.TipoBeneficioId == tipoBeneficioId).HabilitarGrupoFamiliar;
                return Json(new { Success = possuiGrupoFamiliar }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void SetarUrl()
        {
            ViewBag.UrlAdd = Url.Action("AdicionarGrupoFamiliar", "Caso");
            ViewBag.UrlRemove = Url.Action("RemoverGrupoFamiliar", "Caso");
        }

        private void BalancearListaGruposFamiliares(Caso casoAntigo, Caso casoNovo)
        {
            if (casoNovo.GruposFamiliares == null || !casoNovo.GruposFamiliares.Any())
            {
                var listaToRemove = casoAntigo.GruposFamiliares.Where(x => x.Ativo).Select(s => s.GrupoFamiliarId).ToList();
                listaToRemove.ForEach(x =>
                {
                    var grupoFamiliarToRemove = casoAntigo.GruposFamiliares.FirstOrDefault(y => y.GrupoFamiliarId == x);
                    _db.GruposFamiliares.Remove(grupoFamiliarToRemove);
                });
            }
            else
            {
                var listaToAdd = casoNovo.GruposFamiliares.Where(x => x.GrupoFamiliarId == 0).ToList();
                var listaToUpdate = casoNovo.GruposFamiliares.Where(x => casoAntigo.GruposFamiliares.Any(y => y.GrupoFamiliarId == x.GrupoFamiliarId)).ToList();
                var listaToRemove = casoAntigo.GruposFamiliares.Where(x => x.Ativo && casoNovo.GruposFamiliares.All(y => y.GrupoFamiliarId != x.GrupoFamiliarId)).Select(s => s.GrupoFamiliarId).ToList();
                listaToAdd.ForEach(x => casoAntigo.GruposFamiliares.Add(x));
                listaToUpdate.ForEach(x =>
                {
                    var grupoFamiliarUpdate = casoAntigo.GruposFamiliares.FirstOrDefault(y => y.GrupoFamiliarId == x.GrupoFamiliarId);
                    if (grupoFamiliarUpdate != null)
                    {
                        grupoFamiliarUpdate.Nome = x.Nome;
                        grupoFamiliarUpdate.Cpf = x.Cpf;
                        grupoFamiliarUpdate.DataNascimento = x.DataNascimento;
                        grupoFamiliarUpdate.GrauParentesco = x.GrauParentesco;
                    }
                });
                listaToRemove.ForEach(x =>
                {
                    var grupoFamiliarToRemove = casoAntigo.GruposFamiliares.FirstOrDefault(y => y.GrupoFamiliarId == x);
                    _db.GruposFamiliares.Remove(grupoFamiliarToRemove);
                });
            }
        }

        private void SetViewBag()
        {
            ViewBag.TiposBeneficios = _db.TiposBeneficios.Where(x => x.Ativo).ToList();
            ViewBag.RepresentantesLegais = _db.Clientes.ToList();
        }
    }
}