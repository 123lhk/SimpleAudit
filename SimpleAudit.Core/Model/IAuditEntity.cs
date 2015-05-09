using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAudit.Core.Model
{
	public interface IAuditEntity
	{
		Guid Id { get; set; }

	}
}
