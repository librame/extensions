using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class ExceptionExtensionsTests
    {

        [Fact]
        public void AsInnerMessageTest()
        {
            var ex = new ArgumentNullException("test");
            Assert.NotEmpty(ex.AsInnerMessage());
        }


        [Fact]
        public void NotDefaultTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                return str.NotDefault(nameof(str));
            });
        }


        [Fact]
        public void NotWhiteSpaceTest()
        {
            var str = " ";
            Assert.Throws<ArgumentNullException>(() =>
            {
                return str.NotWhiteSpace(nameof(str));
            });
        }


        [Fact]
        public void NotEmptyTest()
        {
            // String
            var str = string.Empty;
            Assert.Throws<ArgumentNullException>(() =>
            {
                return str.NotEmpty(nameof(str));
            });

            // IEnumerable
            IEnumerable<string> items = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                return items.NotEmpty(nameof(items));
            });

            // IList
            IList<string> list = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                return list.NotEmpty(nameof(list));
            });

            // Array
            string[] array = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                return array.NotEmpty(nameof(array));
            });
        }


        [Fact]
        public void NotGreaterTest()
        {
            var num = 3;
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                return num.NotGreater(2, nameof(num));
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                return num.NotGreater(3, nameof(num), true);
            });
        }


        [Fact]
        public void NotLesserTest()
        {
            var num = 3;
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                return num.NotLesser(4, nameof(num));
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                return num.NotLesser(3, nameof(num), true);
            });
        }


        [Fact]
        public void NotOutOfRangeTest()
        {
            var num = 30;
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                return num.NotOutOfRange(1, 9, nameof(num));
            });
        }


        [Fact]
        public void FileExistsTest()
        {
            var fileName = Path.Combine(Directory.GetCurrentDirectory().ToString(), "nofile.txt");
            Assert.Throws<FileNotFoundException>(() =>
            {
                return fileName.FileExists();
            });
        }


        [Fact]
        public void DirectoryExistsTest()
        {
            var dirName = Path.Combine(Directory.GetCurrentDirectory().ToString(), "nodirectory");
            Assert.Throws<DirectoryNotFoundException>(() =>
            {
                return dirName.DirectoryExists();
            });
        }


        [Fact]
        public void CanAssignableFromTypeTest()
        {
            var baseType = typeof(IBaseType);
            var fromType = typeof(BaseType);

            var resultType = baseType.CanAssignableFromType(fromType);

            Assert.Equal(fromType, resultType);
        }


        [Fact]
        public void SameTypeTest()
        {
            object obj = new BaseType();

            var result = obj.SameType<BaseType>(nameof(obj));

            Assert.Equal(obj, result);
        }

    }


    public interface IBaseType
    {
    }
    public class BaseType : IBaseType
    {
    }
}
