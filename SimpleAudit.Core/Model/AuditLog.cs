using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAudit.Core.Model
{
	public class AuditLog
	{
		[Key]
		public Guid Id { get; set; }

		public Guid TargetId { get; set; }

		public string TargetType { get; set; }

		public string ModifiedByUserId { get; set; }

		public DateTime TimeStamp { get; set; }

		public string PropertyName { get; set; }

		public string OldValue { get; set; }

		public string NewValue { get; set; }



	}
}
