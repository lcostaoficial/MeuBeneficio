using Advogado.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Advogado.Data
{
    public class JustificativaTarefaConfig : EntityTypeConfiguration<JustificativaTarefa>
    {
        public JustificativaTarefaConfig()
        {
            HasKey(x => x.JustificativaTarefaId);
            Property(x => x.JustificativaTarefaId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Descricao).IsRequired().HasMaxLength(500);
            Property(x => x.Movimentacao).IsRequired().HasMaxLength(500);

            Property(x => x.DataJustificativa).IsRequired();

            Property(x => x.TipoJustificativaTarefa).IsRequired();

            HasRequired(x => x.Funcionario).WithMany(x => x.JustificativasTarefas).HasForeignKey(x => x.FuncionarioId).WillCascadeOnDelete(false);

            HasRequired(x => x.Tarefa).WithMany(x => x.JustificativasTarefas).HasForeignKey(x => x.TarefaId).WillCascadeOnDelete(false);

            ToTable("JustificativaTarefa");
        }
    }
}