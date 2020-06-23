#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data.Stores
{
    using Core.Services;

    /// <summary>
    /// 抽象存储初始化器。
    /// </summary>
    public abstract class AbstractStoreInitializer : AbstractService, IStoreInitializer
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStoreInitializer"/>。
        /// </summary>
        /// <param name="validator">给定的 <see cref="IStoreInitializationValidator"/>。</param>
        /// <param name="identifierGenerator">给定的 <see cref="IStoreIdentifierGenerator"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractStoreInitializer(IStoreInitializationValidator validator,
            IStoreIdentifierGenerator identifierGenerator, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Validator = validator.NotNull(nameof(validator));
            IdentifierGenerator = identifierGenerator.NotNull(nameof(identifierGenerator));
        }


        /// <summary>
        /// 验证器。
        /// </summary>
        /// <value>返回 <see cref="IStoreInitializationValidator"/>。</value>
        public IStoreInitializationValidator Validator { get; }

        /// <summary>
        /// 标识符生成器。
        /// </summary>
        public IStoreIdentifierGenerator IdentifierGenerator { get; }


        /// <summary>
        /// 需要保存变化。
        /// </summary>
        public bool RequiredSaveChanges { get; protected set; }


        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub"/>。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public virtual void Initialize(IStoreHub stores)
        {
            stores.NotNull(nameof(stores));

            // 切换为写入数据连接
            stores.Accessor.ChangeConnectionString(tenant => tenant.WritingConnectionString);

            // 如果未能成功切换，则直接直接退出
            if (!stores.Accessor.IsWritingConnectionString())
                return;

            InitializeCore(stores);

            if (RequiredSaveChanges)
            {
                stores.Accessor.SaveChanges();

                Validator.SetInitialized(stores.Accessor);

                RequiredSaveChanges = false;
            };

            // 还原为默认数据连接
            stores.Accessor.ChangeConnectionString(tenant => tenant.DefaultConnectionString);
        }

        /// <summary>
        /// 初始化核心。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub"/>。</param>
        protected abstract void InitializeCore(IStoreHub stores);
    }
}
