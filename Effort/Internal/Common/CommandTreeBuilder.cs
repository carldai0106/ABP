﻿// --------------------------------------------------------------------------------------------
// <copyright file="CommandTreeBuilder.cs" company="Effort Team">
//     Copyright (C) 2011-2014 Effort Team
//
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//
//     The above copyright notice and this permission notice shall be included in
//     all copies or substantial portions of the Software.
//
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//     THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------

namespace Effort.Internal.Common
{
    using System;
#if !EFOLD
    using System.Data.Entity.Core.Common.CommandTrees;
    using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
    using System.Data.Entity.Core.Metadata.Edm;
#else
    using System.Data.Common.CommandTrees;
    using System.Data.Metadata.Edm;
#endif
    using System.Reflection;

    /// <summary>
    ///     Create DbCommandTree objects.
    /// </summary>
    internal static class CommandTreeBuilder
    {
        /// <summary>
        ///     Creates the full database scan expression.
        /// </summary>
        /// <param name="workspace">
        ///     The workspace that contains the metadata of the database
        /// </param>
        /// <param name="entitySet">
        ///     The entity set that is being scanned.
        /// </param>
        /// <returns>
        ///     The DbCommandTree object.
        /// </returns>
        public static DbCommandTree CreateSelectAll(
            MetadataWorkspace workspace, 
            EntitySet entitySet)
        {
#if !EFOLD
            var scanExpression = DbExpressionBuilder.Scan(entitySet);

            return new DbQueryCommandTree(workspace, DataSpace.SSpace, scanExpression);
#else
            var treeConstructor =
                typeof(DbQueryCommandTree)
                .GetConstructor(
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    new Type[] { typeof(MetadataWorkspace), typeof(DataSpace), typeof(DbExpression) },
                    null);

            var expressionBuilderType = 
                typeof(DbExpression).Assembly.GetType(
                    "System.Data.Common.CommandTrees.ExpressionBuilder.DbExpressionBuilder");

            var scanExpression = expressionBuilderType
                .GetMethod("Scan")
                .Invoke(null, new object[] { entitySet });

            var tree = 
                treeConstructor.Invoke(
                    new object[] { workspace, DataSpace.SSpace, scanExpression });

            return tree as DbCommandTree;
#endif
        }
    }
}
