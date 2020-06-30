using CoolCatCollects.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolCatCollects.DBUpdate
{
	class Program
	{
		static void Main(string[] args)
		{
			var configuration = new DbMigrationsConfiguration<EfContext>();

			var migrator = new DbMigrator(configuration);



			var scriptor = new MigratorScriptingDecorator(migrator);

			var script = scriptor.ScriptUpdate(sourceMigration: null, targetMigration: null);

			Console.WriteLine(script);

			Console.WriteLine("Press Enter to update.");

			Console.ReadLine();

			migrator.Update();



			var pending = migrator.GetPendingMigrations();



			Console.ReadLine();
		}
	}
}
