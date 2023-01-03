using Advogado.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Advogado.Data
{
    public class TipoBeneficioConfig : EntityTypeConfiguration<TipoBeneficio>
    {
        public TipoBeneficioConfig()
        {
            HasKey(x => x.TipoBeneficioId);
            Property(x => x.TipoBeneficioId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Descricao).IsRequired().HasMaxLength(500);
            Property(x => x.Remuneracao).IsRequired().HasMaxLength(255);
            Property(x => x.HabilitarGrupoFamiliar).IsRequired();
            Property(x => x.Ativo).IsRequired();

            HasMany(x => x.TiposArquivos).WithMany(x => x.TiposBeneficios).Map(x =>
            {
                x.MapLeftKey("TipoBeneficioId");
                x.MapRightKey("TipoArquivoId");
                x.ToTable("TipoBeneficioArquivo");
            });

            HasMany(x => x.Perguntas).WithMany(x => x.TiposBeneficios).Map(x =>
            {
                x.MapLeftKey("TipoBeneficioId");
                x.MapRightKey("PerguntaId");
                x.ToTable("TipoBeneficioPergunta");
            });

            ToTable("TipoBeneficio");
        }
    }
}