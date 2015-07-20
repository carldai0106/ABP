﻿// --------------------------------------------------------------------------------------------
// <copyright file="DbRelationInfo.cs" company="Effort Team">
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

namespace Effort.Internal.DbManagement.Schema
{
    using System;
    using System.Reflection;
    using NMemory.Indexes;

    internal class DbRelationInfo
    {
        public DbRelationInfo(
            string primaryTable, 
            string foreignTable,
            IKeyInfo primaryKeyInfo,
            IKeyInfo foreignKeyInfo, 
            Delegate primaryToForeignConverter,
            Delegate foreignToPrimaryConverter,
            bool cascadedDelete)
        {
            this.PrimaryTable = primaryTable;
            this.ForeignTable = foreignTable;
            this.PrimaryKeyInfo = primaryKeyInfo;
            this.ForeignKeyInfo = foreignKeyInfo;
            this.PrimaryToForeignConverter = primaryToForeignConverter;
            this.ForeignToPrimaryConverter = foreignToPrimaryConverter;
            this.CascadedDelete = cascadedDelete;
        }

        public string PrimaryTable { get; private set; }

        public string ForeignTable { get; private set; }

        // NMemory.Indexes.AnonymousTypeKeyInfo<TPrimary, TPrimaryKey>
        public IKeyInfo PrimaryKeyInfo { get; private set; }

        // NMemory.Indexes.AnonymousTypeKeyInfo<TForeign, TForeignKey>
        public IKeyInfo ForeignKeyInfo { get; private set; }

        // Func<TPrimaryKey, TForeignKey>
        public Delegate PrimaryToForeignConverter { get; private set; }

        // Func<TForeignKey, TPrimaryKey>
        public Delegate ForeignToPrimaryConverter { get; private set; }

        public bool CascadedDelete { get; private set; }
    }
}
