using Advogado.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Advogado.Data
{
    public class MainContext : DbContext
    {
        public MainContext() : base("Conexao") { }
        public DbSet<Alternativa> Alternativas { get; set; }
        public DbSet<Arquivo> Arquivos { get; set; }
        public DbSet<Caso> Casos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }        
        public DbSet<Movimentacao> Movimentacoes { get; set; }
        public DbSet<Pergunta> Perguntas { get; set; }
        public DbSet<Resposta> Respostas { get; set; }
        public DbSet<TipoArquivo> TiposArquivos { get; set; }
        public DbSet<TipoBeneficio> TiposBeneficios { get; set; }
        public DbSet<GrupoFamiliar> GruposFamiliares { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<JustificativaTarefa> JustificativasTarefas { get; set; }
        public DbSet<QuestionarioSalarioMaternidade> QuestionariosSalariosMaternidades { get; set; }
        public DbSet<PerguntaOrdem> PerguntasOrdens { get; set; }
        public DbSet<TipoArquivoOrdem> TiposArquivosOrdens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Properties<string>().Configure(x => x.HasColumnType("varchar"));
            modelBuilder.Properties<string>().Configure(x => x.HasMaxLength(100));

            modelBuilder.Configurations.Add(new AlternativaConfig());
            modelBuilder.Configurations.Add(new ArquivoConfig());
            modelBuilder.Configurations.Add(new CasoConfig());
            modelBuilder.Configurations.Add(new ClienteConfig());
            modelBuilder.Configurations.Add(new FuncionarioConfig());
            modelBuilder.Configurations.Add(new GrupoFamiliarConfig());            
            modelBuilder.Configurations.Add(new MovimentacaoConfig());
            modelBuilder.Configurations.Add(new PerguntaConfig());
            modelBuilder.Configurations.Add(new RespostaConfig());
            modelBuilder.Configurations.Add(new TipoArquivoConfig());
            modelBuilder.Configurations.Add(new TipoBeneficioConfig());
            modelBuilder.Configurations.Add(new TarefaConfig());
            modelBuilder.Configurations.Add(new JustificativaTarefaConfig());
            modelBuilder.Configurations.Add(new QuestionarioSalarioMaternidadeConfig());
            modelBuilder.Configurations.Add(new PerguntaOrdemConfig());
            modelBuilder.Configurations.Add(new TipoArquivoOrdemConfig());
        }
    }
}