﻿// --------------------------------------------------------------------------------------------
// <copyright file="IntegerFunctions.cs" company="Effort Team">
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

namespace Effort.Internal.DbCommandTreeTransformation.Functions
{
    using System;
    using System.Reflection;
    using Effort.Internal.Common;

    internal class IntegerFunctions
    {
        public static readonly MethodInfo Abs64 =
            ReflectionHelper.GetMethodInfo(() => DbFunctions.Abs(0L));

        public static readonly MethodInfo Abs32 =
            ReflectionHelper.GetMethodInfo(() => DbFunctions.Abs((int?)0));

        public static readonly MethodInfo Abs16=
            ReflectionHelper.GetMethodInfo(() => DbFunctions.Abs((short?)0));

        public static readonly MethodInfo Abs8 =
            ReflectionHelper.GetMethodInfo(() => DbFunctions.Abs((sbyte?)0));
    }
}
