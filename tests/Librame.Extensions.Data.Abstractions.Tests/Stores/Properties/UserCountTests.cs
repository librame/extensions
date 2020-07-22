using System;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class UserCountTests
    {
        [Fact]
        public void UserCountTest()
        {
            var userCount = new TestUserCount();

            for (var i = 0; i < 10; i++)
            {
                ProgressiveTest(i + 1);
            }

            for (var i = userCount.FavoriteCount; i >= 0; i--)
            {
                DegressiveTest(i - 1);
            }

            void ProgressiveTest(long target)
            {
                userCount.ProgressiveSupporterCount();
                userCount.ProgressiveObjectorCount();
                userCount.ProgressiveFavoriteCount();
                userCount.ProgressiveRetweetCount();

                Assert.Equal(target, userCount.SupporterCount);
                Assert.Equal(target, userCount.ObjectorCount);
                Assert.Equal(target, userCount.FavoriteCount);
                Assert.Equal(target, userCount.RetweetCount);
            }

            void DegressiveTest(long target)
            {
                userCount.DegressiveSupporterCount();
                userCount.DegressiveObjectorCount();
                userCount.DegressiveFavoriteCount();
                userCount.DegressiveRetweetCount();

                Assert.Equal(target, userCount.SupporterCount);
                Assert.Equal(target, userCount.ObjectorCount);
                Assert.Equal(target, userCount.FavoriteCount);
                Assert.Equal(target, userCount.RetweetCount);
            }
        }


        [Fact]
        public void CommentUserCountTest()
        {
            var userCount = new TestCommentUserCount();

            for (var i = 0; i < 10; i++)
            {
                ProgressiveTest(i + 1);
            }

            for (var i = userCount.CommentCount; i >= 0; i--)
            {
                DegressiveTest(i - 1);
            }

            void ProgressiveTest(long target)
            {
                userCount.ProgressiveCommentCount();
                userCount.ProgressiveCommenterCount();

                Assert.Equal(target, userCount.CommentCount);
                Assert.Equal(target, userCount.CommenterCount);
            }

            void DegressiveTest(long target)
            {
                userCount.DegressiveCommentCount();
                userCount.DegressiveCommenterCount();

                Assert.Equal(target, userCount.CommentCount);
                Assert.Equal(target, userCount.CommenterCount);
            }
        }


        [Fact]
        public void VisitUserCountTest()
        {
            var userCount = new TestVisitUserCount();

            for (var i = 0; i < 10; i++)
            {
                ProgressiveTest(i + 1);
            }

            void ProgressiveTest(long target)
            {
                userCount.ProgressiveVisitCount();
                userCount.ProgressiveVisitorCount();

                Assert.Equal(target, userCount.VisitCount);
                Assert.Equal(target, userCount.VisitorCount);
            }
        }

    }
}
