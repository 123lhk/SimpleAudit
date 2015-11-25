using System;
using System.Linq;
using Effort;
using NUnit.Framework;
using SimpleAudit.Test.SimpleAudit.Core.Test.TestModals;

namespace SimpleAudit.Test.SimpleAudit.Core.Test
{
	[TestFixture]
	public class CoreTest
	{

		private MyTestContext TestContext;

		[TestFixtureSetUp]
		public void Init()
		{
			var connection = DbConnectionFactory.CreateTransient();
			TestContext = new MyTestContext(connection);
			
		}

		public void CreateRecord(string userId, Guid objectId, string value1, string value2)
		{
			var testObject = new TestObject()
			{
				Id = objectId,
				Value1 = value1,
				Value2 = value2
			};

			TestContext.TestObjects.Add(testObject);
			TestContext.SaveChangesWithAudit(userId);
		}

		[Test]
		public void CreateOperation()
		{
			var userId = Guid.NewGuid().ToString();

			var objectId = Guid.NewGuid();
			var value1 = "aaa";
			var value2 = "bbb";

			CreateRecord(userId, objectId, value1, value2);

			var auditLogs = TestContext.AuditLogs.Where(x => x.TargetId == objectId && x.ModifiedByUserId == userId).ToList();

			Assert.AreEqual(2, auditLogs.Count());

			var auditLogForId = auditLogs.Single(x => x.PropertyName == "Id");
			var auditLogForValue = auditLogs.Single(x => x.PropertyName == "Value1");

			Assert.AreEqual(value1, auditLogForValue.NewValue);
			Assert.IsNullOrEmpty(auditLogForValue.OldValue);
			Assert.AreEqual(objectId, auditLogForValue.TargetId);

			Assert.AreEqual("Id", auditLogForId.PropertyName);
			Assert.AreEqual(objectId.ToString(), auditLogForId.NewValue);
			Assert.AreEqual(objectId, auditLogForId.TargetId);

		}

		[Test]
		public void UpdateOperation()
		{
			var createUserId = Guid.NewGuid().ToString();

			var objectId = Guid.NewGuid();
			var value1 = "aaa";
			var value2 = "bbb";
			var newValue = "new";

			CreateRecord(createUserId, objectId, value1, value2);

			var userId = Guid.NewGuid().ToString();
			var testObject = TestContext.TestObjects.Find(objectId);
			testObject.Value1 = newValue;
			TestContext.SaveChangesWithAudit(userId);

			var auditLogs = TestContext.AuditLogs.Where(x => x.TargetId == objectId && x.ModifiedByUserId == userId).ToList();

			Assert.AreEqual(1, auditLogs.Count());

			var auditLogForValue = auditLogs.Single(x => x.PropertyName == "Value1");

			Assert.AreEqual(newValue, auditLogForValue.NewValue);
			Assert.AreEqual(value1, auditLogForValue.OldValue);
			Assert.AreEqual(objectId, auditLogForValue.TargetId);
			Assert.AreEqual("Value1", auditLogForValue.PropertyName);

		}

		[Test]
		public void DeleteOperation()
		{
			var createUserId = Guid.NewGuid().ToString();

			var objectId = Guid.NewGuid();
			var value1 = "aaa";
			var value2 = "bbb";

			CreateRecord(createUserId, objectId, value1, value2);

			var userId = Guid.NewGuid().ToString();

			var testObject = TestContext.TestObjects.Find(objectId);
			TestContext.TestObjects.Remove(testObject);
			TestContext.SaveChangesWithAudit(userId);

			var auditLogs = TestContext.AuditLogs.Where(x => x.TargetId == objectId && x.ModifiedByUserId == userId).ToList();

			Assert.AreEqual(2, auditLogs.Count());

			var auditLogForValue = auditLogs.Single(x => x.PropertyName == "Value1");
			var auditLogForId = auditLogs.Single(x => x.PropertyName == "Id");

			Assert.AreEqual(value1, auditLogForValue.OldValue);
			Assert.IsNullOrEmpty(auditLogForValue.NewValue);
			Assert.AreEqual(objectId, auditLogForValue.TargetId);

			Assert.AreEqual("Id", auditLogForId.PropertyName);
			Assert.AreEqual(objectId.ToString(), auditLogForId.OldValue);
		}
	}
}
