using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleAudit.Core.Attributes;
using SimpleAudit.Core.Model;

namespace SimpleAudit.Test.SimpleAudit.Core.Test.TestModals
{
	public class TestObject : IAuditEntity
	{
		[Key]
		[AuditProperty]
		public Guid Id { get; set; }

		[AuditProperty]
		public string Value1 { get; set; }

		public string Value2 { get; set; }
	}


}
