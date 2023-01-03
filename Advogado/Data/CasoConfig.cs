using Advogado.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Advogado.Data
{
    public class CasoConfig : EntityTypeConfiguration<Caso>
    {
        public CasoConfig()
        {
            HasKey(x => x.CasoId);
            Property(x => x.CasoId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.DataAbertura).IsRequired();
            Property(x => x.SituacaoCaso).IsRequired();
            Property(x => x.Ativo).IsRequired();

            HasRequired(x => x.TipoBeneficio).WithMany(x => x.Casos).HasForeignKey(x => x.TipoBeneficioId).WillCascadeOnDelete(false);

            HasRequired(x => x.Cliente).WithMany(x => x.Casos).HasForeignKey(x => x.ClienteId).WillCascadeOnDelete(false);

            HasOptional(x => x.RepresentanteLegal).WithMany(x => x.CasosRepresentantesLegais).HasForeignKey(x => x.RepresentanteLegalId).WillCascadeOnDelete(false);

            ToTable("Caso");
        }
    }
}