﻿// --------------------------------------------------------------------------------------------
// <copyright file="EntityPropertyInfo.cs" company="Effort Team">
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

namespace Effort.Internal.DbManagement.Schema.Configuration
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Effort.Internal.TypeConversion;

    internal class EntityPropertyInfo
    {
        private readonly string name;
        private readonly Type type;
        private readonly FacetInfo facets;
        private readonly ReadOnlyCollection<IndexInfo> indexes;

        public EntityPropertyInfo(
            string name, 
            Type type, 
            FacetInfo facets, 
            List<IndexInfo> indexes)
        {
            this.name = name;
            this.type = type;
            this.facets = facets;
            this.indexes = indexes.ToList().AsReadOnly();
        }

        public string Name
        {
            get { return this.name; }
        }

        public FacetInfo Facets
        {
            get { return this.facets; }
        }

        public Type ClrType
        {
            get { return this.type; }
        }

        public IList<IndexInfo> Indexes
        {
            get { return this.indexes; }
        }
    }
}
