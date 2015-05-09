using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SimpleAudit.Core.Attributes;
using SimpleAudit.Core.Model;

namespace SimpleAudit.Web.Models.Product
{
	public class Product : IAuditEntity
	{
		[Key]
		public Guid Id { get; set; }

		public void SaveFromViewModel(Product viewModel)
		{
			this.Made = viewModel.Made;
			this.Name = viewModel.Name;
			this.Price = viewModel.Price;
		}

		[AuditProperty]
		public string Name { get; set; }
		[AuditProperty]
		public string Made { get; set; }
		public decimal Price { get; set; }

	}
}