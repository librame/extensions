#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Authorization;
using Librame.Data;
using System;
using System.Collections.Generic;

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="Enum"/> 样式实用工具。
    /// </summary>
    public class EnumStyleUtility
    {
        /// <summary>
        /// 默认键。
        /// </summary>
        protected const string KEY_DEFAULT = "Default";
        /// <summary>
        /// 危险键。
        /// </summary>
        protected const string KEY_DANGER = "Danger";
        /// <summary>
        /// 重要键。
        /// </summary>
        protected const string KEY_IMPORTANT = "Important";
        /// <summary>
        /// 信息键。
        /// </summary>
        protected const string KEY_INFO = "Info";
        /// <summary>
        /// 主要键。
        /// </summary>
        protected const string KEY_PRIMARY = "Primary";
        /// <summary>
        /// 成功键。
        /// </summary>
        protected const string KEY_SUCCESS = "Success";
        /// <summary>
        /// 警告键。
        /// </summary>
        protected const string KEY_WARNING = "Warning";

        /// <summary>
        /// 样式集合。
        /// </summary>
        protected static readonly IDictionary<string, string> Styles = null;

        static EnumStyleUtility()
        {
            if (ReferenceEquals(Styles, null))
            {
                Styles = new Dictionary<string, string>();

                Styles.Add(KEY_DEFAULT, "label label-default");
                Styles.Add(KEY_DANGER, "label label-danger");
                Styles.Add(KEY_IMPORTANT, "label label-important");
                Styles.Add(KEY_INFO, "label label-info");
                Styles.Add(KEY_PRIMARY, "label label-primary");
                Styles.Add(KEY_SUCCESS, "label label-success");
                Styles.Add(KEY_WARNING, "label label-warning");
            }
        }


        /// <summary>
        /// 获取帐户状态枚举项带样式的描述内容。
        /// </summary>
        /// <param name="status">给定的枚举项。</param>
        /// <returns>返回描述内容。</returns>
        public static string GetDescriptionWithStyle(DataStatus status)
        {
            var text = EnumUtility.GetDescription(status);
            var style = string.Empty;

            switch (status)
            {
                case DataStatus.Default:
                    style = Styles[KEY_DEFAULT];
                    break;

                case DataStatus.Public:
                    style = Styles[KEY_SUCCESS];
                    break;

                case DataStatus.Internal:
                    style = Styles[KEY_INFO];
                    break;

                case DataStatus.Private:
                    style = Styles[KEY_INFO];
                    break;

                case DataStatus.Protected:
                    style = Styles[KEY_INFO];
                    break;

                case DataStatus.Deleted:
                    style = Styles[KEY_IMPORTANT];
                    break;

                case DataStatus.Obsoleted:
                    style = Styles[KEY_WARNING];
                    break;

                case DataStatus.Locked:
                    style = Styles[KEY_WARNING];
                    break;

                default:
                    goto case DataStatus.Default;
            }

            style = string.Format("<span class=\"{0}\">{1}</span>", style, text);

            return style;
        }


        /// <summary>
        /// 获取帐户状态枚举项带样式的描述内容。
        /// </summary>
        /// <param name="status">给定的枚举项。</param>
        /// <returns>返回描述内容。</returns>
        public static string GetDescriptionWithStyle(AccountStatus status)
        {
            var text = EnumUtility.GetDescription(status);
            var style = string.Empty;

            switch (status)
            {
                case AccountStatus.Default:
                    style = Styles[KEY_DEFAULT];
                    break;

                case AccountStatus.Active:
                    style = Styles[KEY_SUCCESS];
                    break;

                case AccountStatus.Banned:
                    style = Styles[KEY_IMPORTANT];
                    break;

                case AccountStatus.Inactive:
                    style = Styles[KEY_INFO];
                    break;

                case AccountStatus.Pending:
                    style = Styles[KEY_WARNING];
                    break;

                default:
                    goto case AccountStatus.Default;
            }

            style = string.Format("<span class=\"{0}\">{1}</span>", style, text);

            return style;
        }

    }
}
