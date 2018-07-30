namespace Library.Migrations
{
    using Data.Models.Common;
    using Service.Common;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Library.Data.DatabaseContextSqlServer>
    {
        IPersonService _personService;
        IGroupService _groupService;
        IMemberService _memberService;


        public Configuration(IPersonService personService, IGroupService groupService, IMemberService memberService)
        {
            AutomaticMigrationsEnabled = true;

            _personService = personService;
            _groupService = groupService;
            _memberService = memberService;
        }

        protected override void Seed(Library.Data.DatabaseContextSqlServer context)
        {


            _personService.Insert(new Person { Name = "Andrew", Email = "trudgedy@gmail.com" });
            _personService.Insert(new Person { Name = "Bob", Email = "bob@gmail.com" });
            _personService.Insert(new Person { Name = "Bot", Email = "bot@gmail.com" });
            _groupService.Insert(new Group { Name = "Humans"});
            _groupService.Insert(new Group { Name = "Sub-Humans"});
            _memberService.Insert(new Member { GroupId = 1, PersonId = 1 });

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
