using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SimpleAudit.Core.Attributes;

namespace SimpleAudit.Core.Model
{
	public class AuditDbContext : DbContext
	{
		public AuditDbContext(DbConnection connection, bool contextOwnsConnection)
			: base(connection, contextOwnsConnection)
		{
			
		}

		public AuditDbContext(string connectionString)
			: base(connectionString)
		{
			
		}

		public int SaveChangesWithAudit(string userId)
		{

			foreach (var entry in ChangeTracker.Entries<IAuditEntity>().Where(e => e.State == EntityState.Added))
			{
				if (entry.Entity.Id == Guid.Empty)
					entry.Entity.Id = GenerateGuid();
			}

			var changeInfos =
				ChangeTracker.Entries()
					.Where(t => t.State == EntityState.Modified || t.State == EntityState.Added || t.State == EntityState.Deleted)
					.Select(GetChangeInfoForEntry);



			foreach (var changeInfo in changeInfos)
			{
				var auditLogs = GetAuditLogForModifiedProperty(changeInfo, userId);
				AuditLogs.AddRange(auditLogs);
			}

			return base.SaveChanges();
		}

		public virtual DbSet<AuditLog> AuditLogs { get; set; }

		private class ChangeInfo
		{
			public string EntityName { get; set; }
			public Guid TargetId { get; set; }
			public Dictionary<string, ChangePair> ProperyChangeInfo { get; set; } 
		}

		private class ChangePair
		{
			public string OldValue { get; set; }
			public string NewValue { get; set; }
		}

		private ChangeInfo GetChangeInfoForEntry(DbEntityEntry entry)
		{
			var changeInfo = new ChangeInfo
			{
				EntityName = ObjectContext.GetObjectType(entry.Entity.GetType()).Name,
				ProperyChangeInfo = new Dictionary<string, ChangePair>(),
				TargetId = ((IAuditEntity)entry.Entity).Id
			};

			List<string> propertyNames = new List<string>();

			if (entry.State == EntityState.Deleted)
			{
				propertyNames.AddRange(entry.OriginalValues.PropertyNames);
			}
			else
			{
				propertyNames.AddRange(entry.CurrentValues.PropertyNames);
			}

			foreach (var pn in propertyNames)
			{
				if (!IsAuditRequired(entry, pn))
					continue;
				
				var changePair = new ChangePair();

				switch (entry.State)
				{
					case EntityState.Added:
						changePair.NewValue = entry.CurrentValues[pn].ToString();

						break;
					case EntityState.Modified:
						changePair.OldValue = entry.OriginalValues[pn].ToString();
						changePair.NewValue = entry.CurrentValues[pn].ToString();
						if (changePair.OldValue == changePair.NewValue)
						{
							continue;
						}
						break;
					case EntityState.Deleted:
						changePair.OldValue = entry.OriginalValues[pn].ToString();
						break;
					default:
						throw new ApplicationException("Invalid internal model state.");
				}
				
				changeInfo.ProperyChangeInfo.Add(pn, changePair);

			}

			return changeInfo;
		}

		private Guid GenerateGuid()
		{
			return Guid.NewGuid();
		}

		private List<AuditLog> GetAuditLogForModifiedProperty(ChangeInfo changeInfo, string userId)
		{
			var timestamp = DateTime.Now;
			var result = new List<AuditLog>();
			foreach (var pn in changeInfo.ProperyChangeInfo.Keys)
			{
				var changePair = changeInfo.ProperyChangeInfo[pn];
				var auditLog = new AuditLog()
				{
					Id = GenerateGuid(),
					PropertyName = pn,
					ModifiedByUserId = userId,
					TimeStamp = timestamp,
					TargetId = changeInfo.TargetId,
					TargetType = changeInfo.EntityName,
					OldValue = changePair.OldValue,
					NewValue = changePair.NewValue

				};

				result.Add(auditLog);
			}

			return result;
		}

		private bool IsAuditRequired(DbEntityEntry entry, string propertyName)
		{
			var property = entry.Entity.GetType().GetProperties().SingleOrDefault(p => p.Name == propertyName);

			if (property == null)
			{
				throw new ApplicationException("Internal exception: Property Name invalid.");
			}

			return property.GetCustomAttribute(typeof(AuditProperty)) != null;
		}
	}
}
