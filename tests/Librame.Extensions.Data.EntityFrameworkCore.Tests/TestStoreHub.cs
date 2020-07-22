using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Data.Tests
{
    using Extensions.Data.Accessors;
    using Extensions.Data.Collections;
    using Extensions.Data.Stores;
    using Models;

    public class TestStoreHub : DataStoreHub<TestDbContextAccessor>
    {
        public TestStoreHub(IAccessor accessor)
            : base(accessor)
        {
        }


        public IPageable<DataAudit<Guid, Guid>> GetAudits()
            => Accessor.Audits.AsNoTracking().AsPagingByIndex(q => q.OrderByDescending(k => k.Id), 1, 10);

        public IPageable<DataAuditProperty<int, Guid>> GetAuditProperties()
            => Accessor.AuditProperties.AsNoTracking().AsPagingByIndex(q => q.OrderByDescending(k => k.Id), 1, 10);

        public IPageable<DataTabulation<Guid, Guid>> GetEntities()
            => Accessor.Tabulations.AsNoTracking().AsPagingByIndex(q => q.OrderByDescending(k => k.Id), 1, 10);

        public IList<DataMigration<Guid, Guid>> GetMigrations()
            => Accessor.Migrations.AsNoTracking().ToList();

        public IList<DataTenant<Guid, Guid>> GetTenants()
            => Accessor.Tenants.AsNoTracking().ToList();


        public IList<Category<int, Guid>> GetCategories()
            => Accessor.Categories.AsNoTracking().ToList();

        public IPageable<Article<Guid, int, Guid>> GetArticles()
            => Accessor.Articles.AsNoTracking().AsDescendingPagingByIndex(1, 10);


        public TestStoreHub UseWriteDbConnection()
        {
            Accessor.ChangeConnectionString(t => t.WritingConnectionString);
            return this;
        }

        public TestStoreHub UseDefaultDbConnection()
        {
            Accessor.ChangeConnectionString(t => t.DefaultConnectionString);
            return this;
        }

    }
}
