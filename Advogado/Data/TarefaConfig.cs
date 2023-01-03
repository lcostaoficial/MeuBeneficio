using Advogado.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Advogado.Data
{
    public class TarefaConfig : EntityTypeConfiguration<Tarefa>
    {
        public TarefaConfig()
        {
            HasKey(x => x.TarefaId);
            Property(x => x.TarefaId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Titulo).IsRequired().HasMaxLength(255);
            Property(x => x.Descricao).IsRequired().HasColumnType("nvarchar(max)");
            Property(x => x.Solucao).IsOptional().HasColumnType("nvarchar(max)");
            Property(x => x.DataCriacao).IsRequired();
            Property(x => x.DataExpiracao).IsRequired();
            Property(x => x.DataInicio).IsOptional();
            Property(x => x.DataFinalizacao).IsOptional();            
            Property(x => x.TipoTarefa).IsRequired();            

            HasOptional(x => x.FuncionarioCriador).WithMany(x => x.TarefasCriadas).HasForeignKey(x => x.FuncionarioCriadorId).WillCascadeOnDelete(false);
            HasOptional(x => x.FuncionarioResponsavel).WithMany(x => x.TarefasResponsaveis).HasForeignKey(x => x.FuncionarioResponsavelId).WillCascadeOnDelete(false);

            Property(x => x.Finalizada).IsRequired();

            HasRequired(x => x.Caso).WithMany(x => x.Tarefas).HasForeignKey(x => x.CasoId).WillCascadeOnDelete(false);

            ToTable("Tarefa");
        }
    }
}