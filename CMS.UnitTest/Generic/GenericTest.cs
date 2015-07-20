﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CMS.UnitTest.Generic
{
     [TestClass]
    public class GenericTest
    {
        [TestMethod]
        public void IsSubClassOfGenericTest()
        {
            

            Assert.IsTrue(typeof(ChildGeneric).IsSubClassOfGeneric(typeof(BaseGeneric<>)), " 1");
            Assert.IsFalse(typeof(ChildGeneric).IsSubClassOfGeneric(typeof(WrongBaseGeneric<>)), " 2");
            Assert.IsTrue(typeof(ChildGeneric).IsSubClassOfGeneric(typeof(IBaseGeneric<>)), " 3");
            Assert.IsFalse(typeof(ChildGeneric).IsSubClassOfGeneric(typeof(IWrongBaseGeneric<>)), " 4");
            Assert.IsTrue(typeof(IChildGeneric).IsSubClassOfGeneric(typeof(IBaseGeneric<>)), " 5");
            Assert.IsFalse(typeof(IWrongBaseGeneric<>).IsSubClassOfGeneric(typeof(ChildGeneric2<>)), " 6");
            Assert.IsTrue(typeof(ChildGeneric2<>).IsSubClassOfGeneric(typeof(BaseGeneric<>)), " 7");
            Assert.IsTrue(typeof(ChildGeneric2<Class1>).IsSubClassOfGeneric(typeof(BaseGeneric<>)), " 8");
            Assert.IsTrue(typeof(ChildGeneric).IsSubClassOfGeneric(typeof(BaseGeneric<Class1>)), " 9");
            Assert.IsFalse(typeof(ChildGeneric).IsSubClassOfGeneric(typeof(WrongBaseGeneric<Class1>)), "10");
            Assert.IsTrue(typeof(ChildGeneric).IsSubClassOfGeneric(typeof(IBaseGeneric<Class1>)), "11");
            Assert.IsFalse(typeof(ChildGeneric).IsSubClassOfGeneric(typeof(IWrongBaseGeneric<Class1>)), "12");
            Assert.IsTrue(typeof(IChildGeneric).IsSubClassOfGeneric(typeof(IBaseGeneric<Class1>)), "13");
            Assert.IsFalse(typeof(BaseGeneric<Class1>).IsSubClassOfGeneric(typeof(ChildGeneric2<Class1>)), "14");
            Assert.IsTrue(typeof(ChildGeneric2<Class1>).IsSubClassOfGeneric(typeof(BaseGeneric<Class1>)), "15");
            Assert.IsFalse(typeof(ChildGeneric).IsSubClassOfGeneric(typeof(ChildGeneric)), "16");
            Assert.IsFalse(typeof(IChildGeneric).IsSubClassOfGeneric(typeof(IChildGeneric)), "17");
            Assert.IsFalse(typeof(IBaseGeneric<>).IsSubClassOfGeneric(typeof(IChildGeneric2<>)), "18");
            Assert.IsTrue(typeof(IChildGeneric2<>).IsSubClassOfGeneric(typeof(IBaseGeneric<>)), "19");
            Assert.IsTrue(typeof(IChildGeneric2<Class1>).IsSubClassOfGeneric(typeof(IBaseGeneric<>)), "20");
            Assert.IsFalse(typeof(IBaseGeneric<Class1>).IsSubClassOfGeneric(typeof(IChildGeneric2<Class1>)), "21");
            Assert.IsTrue(typeof(IChildGeneric2<Class1>).IsSubClassOfGeneric(typeof(IBaseGeneric<Class1>)), "22");
            Assert.IsFalse(typeof(IBaseGeneric<Class1>).IsSubClassOfGeneric(typeof(BaseGeneric<Class1>)), "23");
            Assert.IsTrue(typeof(BaseGeneric<Class1>).IsSubClassOfGeneric(typeof(IBaseGeneric<Class1>)), "24");
            Assert.IsFalse(typeof(IBaseGeneric<>).IsSubClassOfGeneric(typeof(BaseGeneric<>)), "25");
            Assert.IsTrue(typeof(BaseGeneric<>).IsSubClassOfGeneric(typeof(IBaseGeneric<>)), "26");
            Assert.IsTrue(typeof(BaseGeneric<Class1>).IsSubClassOfGeneric(typeof(IBaseGeneric<>)), "27");
            Assert.IsFalse(typeof(IBaseGeneric<Class1>).IsSubClassOfGeneric(typeof(IBaseGeneric<Class1>)), "28");
            Assert.IsTrue(typeof(BaseGeneric2<Class1>).IsSubClassOfGeneric(typeof(IBaseGeneric<Class1>)), "29");
            Assert.IsFalse(typeof(IBaseGeneric<>).IsSubClassOfGeneric(typeof(BaseGeneric2<>)), "30");
            Assert.IsTrue(typeof(BaseGeneric2<>).IsSubClassOfGeneric(typeof(IBaseGeneric<>)), "31");
            Assert.IsTrue(typeof(BaseGeneric2<Class1>).IsSubClassOfGeneric(typeof(IBaseGeneric<>)), "32");
            Assert.IsTrue(typeof(ChildGenericA).IsSubClassOfGeneric(typeof(BaseGenericA<,>)), "33");
            Assert.IsFalse(typeof(ChildGenericA).IsSubClassOfGeneric(typeof(WrongBaseGenericA<,>)), "34");
            Assert.IsTrue(typeof(ChildGenericA).IsSubClassOfGeneric(typeof(IBaseGenericA<,>)), "35");
            Assert.IsFalse(typeof(ChildGenericA).IsSubClassOfGeneric(typeof(IWrongBaseGenericA<,>)), "36");
            Assert.IsTrue(typeof(IChildGenericA).IsSubClassOfGeneric(typeof(IBaseGenericA<,>)), "37");
            Assert.IsFalse(typeof(IWrongBaseGenericA<,>).IsSubClassOfGeneric(typeof(ChildGenericA2<,>)), "38");
            Assert.IsTrue(typeof(ChildGenericA2<,>).IsSubClassOfGeneric(typeof(BaseGenericA<,>)), "39");
            Assert.IsTrue(typeof(ChildGenericA2<ClassA, ClassB>).IsSubClassOfGeneric(typeof(BaseGenericA<,>)), "40");
            Assert.IsTrue(typeof(ChildGenericA).IsSubClassOfGeneric(typeof(BaseGenericA<ClassA, ClassB>)), "41");
            Assert.IsFalse(typeof(ChildGenericA).IsSubClassOfGeneric(typeof(WrongBaseGenericA<ClassA, ClassB>)), "42");
            Assert.IsTrue(typeof(ChildGenericA).IsSubClassOfGeneric(typeof(IBaseGenericA<ClassA, ClassB>)), "43");
            Assert.IsFalse(typeof(ChildGenericA).IsSubClassOfGeneric(typeof(IWrongBaseGenericA<ClassA, ClassB>)), "44");
            Assert.IsTrue(typeof(IChildGenericA).IsSubClassOfGeneric(typeof(IBaseGenericA<ClassA, ClassB>)), "45");
            Assert.IsFalse(typeof(BaseGenericA<ClassA, ClassB>).IsSubClassOfGeneric(typeof(ChildGenericA2<ClassA, ClassB>)), "46");
            Assert.IsTrue(typeof(ChildGenericA2<ClassA, ClassB>).IsSubClassOfGeneric(typeof(BaseGenericA<ClassA, ClassB>)), "47");
            Assert.IsFalse(typeof(ChildGenericA).IsSubClassOfGeneric(typeof(ChildGenericA)), "48");
            Assert.IsFalse(typeof(IChildGenericA).IsSubClassOfGeneric(typeof(IChildGenericA)), "49");
            Assert.IsFalse(typeof(IBaseGenericA<,>).IsSubClassOfGeneric(typeof(IChildGenericA2<,>)), "50");
            Assert.IsTrue(typeof(IChildGenericA2<,>).IsSubClassOfGeneric(typeof(IBaseGenericA<,>)), "51");
            Assert.IsTrue(typeof(IChildGenericA2<ClassA, ClassB>).IsSubClassOfGeneric(typeof(IBaseGenericA<,>)), "52");
            Assert.IsFalse(typeof(IBaseGenericA<ClassA, ClassB>).IsSubClassOfGeneric(typeof(IChildGenericA2<ClassA, ClassB>)), "53");
            Assert.IsTrue(typeof(IChildGenericA2<ClassA, ClassB>).IsSubClassOfGeneric(typeof(IBaseGenericA<ClassA, ClassB>)), "54");
            Assert.IsFalse(typeof(IBaseGenericA<ClassA, ClassB>).IsSubClassOfGeneric(typeof(BaseGenericA<ClassA, ClassB>)), "55");
            Assert.IsTrue(typeof(BaseGenericA<ClassA, ClassB>).IsSubClassOfGeneric(typeof(IBaseGenericA<ClassA, ClassB>)), "56");
            Assert.IsFalse(typeof(IBaseGenericA<,>).IsSubClassOfGeneric(typeof(BaseGenericA<,>)), "57");
            Assert.IsTrue(typeof(BaseGenericA<,>).IsSubClassOfGeneric(typeof(IBaseGenericA<,>)), "58");
            Assert.IsTrue(typeof(BaseGenericA<ClassA, ClassB>).IsSubClassOfGeneric(typeof(IBaseGenericA<,>)), "59");
            Assert.IsFalse(typeof(IBaseGenericA<ClassA, ClassB>).IsSubClassOfGeneric(typeof(IBaseGenericA<ClassA, ClassB>)), "60");
            Assert.IsTrue(typeof(BaseGenericA2<ClassA, ClassB>).IsSubClassOfGeneric(typeof(IBaseGenericA<ClassA, ClassB>)), "61");
            Assert.IsFalse(typeof(IBaseGenericA<,>).IsSubClassOfGeneric(typeof(BaseGenericA2<,>)), "62");
            Assert.IsTrue(typeof(BaseGenericA2<,>).IsSubClassOfGeneric(typeof(IBaseGenericA<,>)), "63");
            Assert.IsTrue(typeof(BaseGenericA2<ClassA, ClassB>).IsSubClassOfGeneric(typeof(IBaseGenericA<,>)), "64");
            Assert.IsFalse(typeof(BaseGenericA2<ClassB, ClassA>).IsSubClassOfGeneric(typeof(IBaseGenericA<ClassA, ClassB>)), "65");
            Assert.IsFalse(typeof(BaseGenericA<ClassB, ClassA>).IsSubClassOfGeneric(typeof(ChildGenericA2<ClassA, ClassB>)), "66");
            Assert.IsFalse(typeof(BaseGenericA2<ClassB, ClassA>).IsSubClassOfGeneric(typeof(BaseGenericA<ClassA, ClassB>)), "67");
            Assert.IsTrue(typeof(ChildGenericA3<ClassA, ClassB>).IsSubClassOfGeneric(typeof(BaseGenericB<ClassA, ClassB, ClassC>)), "68");
            Assert.IsTrue(typeof(ChildGenericA4<ClassA, ClassB>).IsSubClassOfGeneric(typeof(IBaseGenericB<ClassA, ClassB, ClassC>)), "69");
            Assert.IsFalse(typeof(ChildGenericA3<ClassB, ClassA>).IsSubClassOfGeneric(typeof(BaseGenericB<ClassA, ClassB, ClassC>)), "68-2");
            Assert.IsTrue(typeof(ChildGenericA3<ClassA, ClassB2>).IsSubClassOfGeneric(typeof(BaseGenericB<ClassA, ClassB, ClassC>)), "68-3");
            Assert.IsFalse(typeof(ChildGenericA3<ClassB2, ClassA>).IsSubClassOfGeneric(typeof(BaseGenericB<ClassA, ClassB, ClassC>)), "68-4");
            Assert.IsFalse(typeof(ChildGenericA4<ClassB, ClassA>).IsSubClassOfGeneric(typeof(IBaseGenericB<ClassA, ClassB, ClassC>)), "69-2");
            Assert.IsTrue(typeof(ChildGenericA4<ClassA, ClassB2>).IsSubClassOfGeneric(typeof(IBaseGenericB<ClassA, ClassB, ClassC>)), "69-3");
            Assert.IsFalse(typeof(ChildGenericA4<ClassB2, ClassA>).IsSubClassOfGeneric(typeof(IBaseGenericB<ClassA, ClassB, ClassC>)), "69-4");
            Assert.IsFalse(typeof(bool).IsSubClassOfGeneric(typeof(IBaseGenericB<ClassA, ClassB, ClassC>)), "70");
            Assert.IsTrue(typeof(ChildGenericA4<,>).IsSubClassOfGeneric(typeof(IBaseGenericB<,,>)), "71");
        }
    }
}
