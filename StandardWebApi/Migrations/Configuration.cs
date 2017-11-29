namespace StandardWebApi.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using StandardWebApi.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<StandardWebApi.Models.StandardWebApiContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(StandardWebApi.Models.StandardWebApiContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Products.AddOrUpdate(x => x.Id,
                new Product() { Id = 1, Name = "Tomato Soup", Category = "Fruits", Price = 12 },
                new Product() { Id = 2, Name = "Yo yo", Category = "Toy", Price = 3.23M },
                new Product() { Id = 3, Name = "Porche", Category = "Car", Price = 120000 },
                new Product() { Id = 4, Name = "Grapes", Category = "Fruits", Price = 1.34M }

                );

        }
    }
}
