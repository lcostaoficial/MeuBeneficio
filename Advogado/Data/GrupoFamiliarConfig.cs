using Advogado.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Advogado.Data
{
    public class GrupoFamiliarConfig : EntityTypeConfiguration<GrupoFamiliar>
    {
        public GrupoFamiliarConfig()
        {
            HasKey(x => x.GrupoFamiliarId);
            Property(x => x.GrupoFamiliarId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nome).IsRequired().HasMaxLength(255);
            Property(x => x.Cpf).HasMaxLength(14).IsRequired();
            Property(x => x.DataNascimento).IsRequired();           
            Property(x => x.GrauParentesco).IsRequired().HasMaxLength(255);
            Property(x => x.Ativo).IsRequired();

            HasRequired(x => x.Caso).WithMany(x => x.GruposFamiliares).HasForeignKey(x => x.CasoId).WillCascadeOnDelete(false);

            ToTable("GrupoFamiliar");
        }
    }
}