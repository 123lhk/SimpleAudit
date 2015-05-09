using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SimpleAudit.Core.Model;

namespace SimpleAudit.Web.Models.Product
{
	public class MyDbContext:AuditDbContext
	{
		public MyDbContext()
			: base("DefaultConnection")
		{
		}

		public DbSet<Product> Products { get; set; } 
	}
}