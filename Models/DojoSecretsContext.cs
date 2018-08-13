using Microsoft.EntityFrameworkCore;
namespace DojoSecrets.Models
{
	public class DojoSecretsContext : DbContext
	{
		// base() calls the parent class' constructor passing the "options" parameter along
		public DojoSecretsContext(DbContextOptions<DojoSecretsContext> options) : base(options) { }

		public DbSet<User> users {get;set;}
		public DbSet<Secret> secrets {get;set;}
		public DbSet<Like> likes {get;set;}
	}
}