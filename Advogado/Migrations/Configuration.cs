namespace Advogado.Migrations
{   
    using System.Data.Entity.Migrations;    

    internal sealed class Configuration : DbMigrationsConfiguration<Data.MainContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Data.MainContext context)
        {

        }
    }
}