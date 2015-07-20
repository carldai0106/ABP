﻿// --------------------------------------------------------------------------------------------
// <copyright file="ModificationContextHelper.cs" company="Effort Team">
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

namespace Effort.Internal.StorageSchema
{
    using System;
    using Effort.Internal.Common.XmlProcessing;

    internal class ModificationContextHelper
    {
        public static readonly string OriginalProvider = "OriginalProvider";

        public static readonly string NewProvider = "NewProvider";

        public static readonly string TypeConverter = "TypeConverter";

        public static StorageTypeConverter GetTypeConverter(IModificationContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("context");
            }

            StorageTypeConverter converter = context.Get<StorageTypeConverter>(TypeConverter, null);

            if (converter != null)
            {
                return converter;
            }

            // Create the converter
            // Get the provider informations first
            IProviderInformation originalProvider =
                context.Get<IProviderInformation>(ModificationContextHelper.OriginalProvider, null);

            IProviderInformation newProvider =
                context.Get<IProviderInformation>(ModificationContextHelper.NewProvider, null);

            if (originalProvider == null)
            {
                throw new ArgumentException("", "context");
            }

            if (newProvider == null)
            {
                throw new ArgumentException("", "context");
            }

            converter = new StorageTypeConverter(originalProvider, newProvider);

            // Store for future usage
            context.Set(TypeConverter, converter);
            
            return converter;
        }
    }
}
