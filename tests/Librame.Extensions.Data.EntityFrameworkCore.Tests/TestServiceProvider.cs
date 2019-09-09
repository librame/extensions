using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Librame.Extensions.Data.Tests
{
    internal static class TestServiceProvider
    {
        static TestServiceProvider()
        {
            Current = Current.EnsureSingleton(() =>
            {
                var services = new ServiceCollection();

                services.AddLibrame()
                    .AddData(options =>
                    {
                        options.DefaultTenant.DefaultConnectionString = "Data Source=.;Initial Catalog=librame_data_default;Integrated Security=True";
                        options.DefaultTenant.WritingConnectionString = "Data Source=.;Initial Catalog=librame_data_writing;Integrated Security=True";
                        options.DefaultTenant.WritingSeparation = true;
                    })
                    .AddAccessor<TestDbContextAccessor>((options, optionsBuilder) =>
                    {
                        optionsBuilder.UseSqlServer(options.DefaultTenant.DefaultConnectionString,
                            sql => sql.MigrationsAssembly(typeof(TestServiceProvider).GetSimpleAssemblyName()));
                    })
                    .AddStoreHubWithAccessor<TestStoreHub>()
                    .AddInitializerWithAccessor<TestStoreInitializer>()
                    .AddIdentifier<TestStoreIdentifier>();

                //if (!services.TryReplace<IMigrationsModelDiffer, TestMigrationsModelDiffer>())
                //    services.AddScoped<IMigrationsModelDiffer, TestMigrationsModelDiffer>();

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }


        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public static TValue GetOrAddNew<TKey, TValue>(
            this IDictionary<TKey, TValue> source,
            TKey key)
            where TValue : new()
        {
            if (!source.TryGetValue(key, out var value))
            {
                value = new TValue();
                source.Add(key, value);
            }

            return value;
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            type = type.GetTypeInfo().BaseType;

            while (type != null)
            {
                yield return type;

                type = type.GetTypeInfo().BaseType;
            }
        }
        public static bool IsSameAs(this MemberInfo propertyInfo, MemberInfo otherPropertyInfo)
            => propertyInfo == null
                ? otherPropertyInfo == null
                : (otherPropertyInfo == null
                    ? false
                    : Equals(propertyInfo, otherPropertyInfo)
                      || (propertyInfo.Name == otherPropertyInfo.Name
                          && (propertyInfo.DeclaringType == otherPropertyInfo.DeclaringType
                              || propertyInfo.DeclaringType.GetTypeInfo().IsSubclassOf(otherPropertyInfo.DeclaringType)
                              || otherPropertyInfo.DeclaringType.GetTypeInfo().IsSubclassOf(propertyInfo.DeclaringType)
                              || propertyInfo.DeclaringType.GetTypeInfo().ImplementedInterfaces.Contains(otherPropertyInfo.DeclaringType)
                              || otherPropertyInfo.DeclaringType.GetTypeInfo().ImplementedInterfaces.Contains(propertyInfo.DeclaringType))));

    }
}
