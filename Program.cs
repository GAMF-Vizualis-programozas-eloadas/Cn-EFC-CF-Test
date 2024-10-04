using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CnEFCF_Test
{

	[Table("TESTDATA")]
	public class Something
	{
		[Key]
		public int Id { get; set; }
		[MaxLength(50)]
		public string Name { get; set; }
	}
	public class cnContext:DbContext
	{
		public DbSet<Something> SomethingSet { get; set; }
		public cnContext() : base() {	}
		protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)
		{
			OptionsBuilder.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=master;Integrated Security=True");
			base.OnConfiguring(OptionsBuilder);
		}
	}
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Simple Entity Framework Core application with code first approach!");
			try
			{
				// Adatfelvitel
				using (var cn = new cnContext())
				{
					var res = cn.Database.EnsureCreated();
					if (cn.SomethingSet.Count() == 0)
						cn.SomethingSet.AddRange(
							new Something[]
							{
								new Something { Name = "xxx" },
								new Something { Name = "yyy" }
							});
					cn.SaveChanges();
				}
				// Lekérdezés
				using (var cn = new cnContext())
				{
					var s = cn.SomethingSet.ToList().Aggregate("", 
						(c, a) => (c.Length > 0 ? c + "\n" : "") 
						+ a.Name);
					Console.WriteLine("Stored data:\n" + s);
				}
			}
			catch (global::System.Exception exc)
			{
				Console.WriteLine(exc.Message);
			}
		}
	
	}
}
