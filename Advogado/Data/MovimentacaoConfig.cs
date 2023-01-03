using Advogado.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Advogado.Data
{
    public class MovimentacaoConfig : EntityTypeConfiguration<Movimentacao>
    {
        public MovimentacaoConfig()
        {
            HasKey(x => x.MovimentacaoId);
            Property(x => x.MovimentacaoId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.NomeAnexo).IsOptional();
            Property(x => x.CaminhoAnexo).IsOptional();
            Property(x => x.Data).IsRequired();
            Property(x => x.Descricao).IsRequired().HasMaxLength(500);
            Property(x => x.TipoMovimento).IsRequired();

            HasRequired(x => x.Funcionario).WithMany(x => x.Movimentacoes).HasForeignKey(x => x.FuncionarioId).WillCascadeOnDelete(false);
            HasRequired(x => x.Caso).WithMany(x => x.Movimentacoes).HasForeignKey(x => x.CasoId).WillCascadeOnDelete(false);

            ToTable("Movimentacao");
        }
    }
}