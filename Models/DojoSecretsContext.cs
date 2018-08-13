using Microsoft.EntityFrameworkCore;
namespace DojoSecrets.Models
{
	public class DojoSecretsContext : DbContext
	{
	// base() calls the parent class' constructor passing the "options" parameter along
	public DojoSecretsContext(DbContextOptions<DojoSecretsContext> options) : base(options) { }
	}
}