using Advogado.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Advogado.Data
{
    public class FuncionarioConfig : EntityTypeConfiguration<Funcionario>
    {
        public FuncionarioConfig()
        {
            HasKey(x => x.FuncionarioId);
            Property(x => x.FuncionarioId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nome).IsRequired().HasMaxLength(255);
            Property(x => x.Cpf).IsRequired().HasMaxLength(14);
            Property(x => x.Senha).IsRequired().HasMaxLength(255);
            Property(x => x.Perfil).IsRequired();
            Property(x => x.Ativo);
          
            ToTable("Funcionario");
        }
    }
}