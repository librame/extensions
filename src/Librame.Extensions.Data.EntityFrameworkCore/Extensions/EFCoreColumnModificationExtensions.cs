#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore.Update
{
    /// <summary>
    /// <see cref="ColumnModification"/> 静态扩展。
    /// </summary>
    public static class EFCoreColumnModificationExtensions
    {
        private static readonly Type _columnType
            = typeof(ColumnModification);

        private static readonly FieldInfo _parameterNameField
            = _columnType.GetNonPublicField("_parameterName");

        private static readonly FieldInfo _originalParameterNameField
            = _columnType.GetNonPublicField("_originalParameterName");

        private static readonly FieldInfo _valueField
            = _columnType.GetNonPublicField("_value");

        private static readonly FieldInfo _originalValueField
            = _columnType.GetNonPublicField("_originalValue");

        private static readonly FieldInfo _sharedColumnModificationsField
            = _columnType.GetNonPublicField("_sharedColumnModifications");


        /// <summary>
        /// 尝试更新列修改。
        /// </summary>
        /// <param name="updateColumn">给定的更新 <see cref="ColumnModification"/>。</param>
        /// <param name="fixedColumn">给定的固定 <see cref="ColumnModification"/>。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static void TryUpdate(this ColumnModification updateColumn, ColumnModification fixedColumn)
        {
            updateColumn.NotNull(nameof(updateColumn));
            fixedColumn.NotNull(nameof(fixedColumn));

            if (updateColumn.ParameterName != fixedColumn.ParameterName)
                _parameterNameField.SetValue(updateColumn, fixedColumn.ParameterName);

            if (updateColumn.OriginalParameterName != fixedColumn.OriginalParameterName)
                _originalParameterNameField.SetValue(updateColumn, fixedColumn.OriginalParameterName);

            if (updateColumn.Value != fixedColumn.Value)
                _valueField.SetValue(updateColumn, fixedColumn.Value);

            if (updateColumn.OriginalValue != fixedColumn.OriginalValue)
                _originalValueField.SetValue(updateColumn, fixedColumn.OriginalParameterName);

            var sharedColumns = _sharedColumnModificationsField.GetValue(fixedColumn);
            if (_sharedColumnModificationsField.GetValue(updateColumn) != sharedColumns)
                _sharedColumnModificationsField.SetValue(updateColumn, sharedColumns);
        }

    }
}
