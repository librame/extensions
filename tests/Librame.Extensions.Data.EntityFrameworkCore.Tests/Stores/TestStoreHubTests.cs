using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class TestStoreHubTests
    {
        [Fact]
        public void AllTest()
        {
            using (var stores = TestServiceProvider.Current.GetRequiredService<TestStoreHub>())
            {
                //if (stores.Accessor.Model is Model model)
                //{
                //    var type = model.ConventionDispatcher.GetType();

                //    var scopeType = type.GetField("_immediateConventionScope", BindingFlags.NonPublic | BindingFlags.Instance);
                //    var scope = scopeType.GetValue(model.ConventionDispatcher);

                //    var conType = scopeType.FieldType.GetField("_conventionSet", BindingFlags.NonPublic | BindingFlags.Instance);
                //    var con = conType.GetValue(scope);

                //    if (con is ConventionSet conventionSet)
                //    {
                //        var buffer = Struct2Bytes(conventionSet);
                //        var obj = Bytes2Struct(buffer, typeof(ConventionSet));
                //        Assert.Equal(con, obj);
                //    }
                //}

                var categories = stores.GetCategories();
                Assert.Empty(categories);

                categories = stores.UseWriteDbConnection().GetCategories();
                Assert.NotEmpty(categories);

                var articles = stores.UseDefaultDbConnection().GetArticles();
                Assert.Empty(articles);

                articles = stores.UseWriteDbConnection().GetArticles();
                Assert.NotEmpty(articles);
            }
        }

        //public static unsafe byte[] Struct2Bytes(object obj)
        //{
        //    var size = Marshal.SizeOf(obj);
        //    var bytes = new byte[size];

        //    fixed (byte* pb = &bytes[0])
        //    {
        //        Marshal.StructureToPtr(obj, new IntPtr(pb), true);
        //    }

        //    return bytes;
        //}

        //public static unsafe object Bytes2Struct(byte[] bytes, Type type)
        //{
        //    fixed (byte* pb = &bytes[0])
        //    {
        //        return Marshal.PtrToStructure(new IntPtr(pb), type);
        //    }
        //}

    }
}
