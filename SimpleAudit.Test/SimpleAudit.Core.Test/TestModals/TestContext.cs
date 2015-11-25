using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleAudit.Core.Model;

namespace SimpleAudit.Test.SimpleAudit.Core.Test.TestModals
{
	public class MyTestContext : AuditDbContext
	{
		public MyTestContext(DbConnection  connection) : base(connection, true)
		{
		}

		public MyTestContext(string connectionString) : base(connectionString)
		{
		}

		public virtual DbSet<TestObject> TestObjects { get; set; } 
	}
}
