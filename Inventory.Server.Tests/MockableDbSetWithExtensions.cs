using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EntityFramework.Testing;
using Moq;

namespace IgiCore.Inventory.Server.Tests
{
	public abstract class MockableDbSetWithExtensions<T> : DbSet<T>
		where T : class
	{
		public abstract void AddOrUpdate(params T[] entities);
		public abstract void AddOrUpdate(Expression<Func<T, object>>
			identifierExpression, params T[] entities);
	}
}
