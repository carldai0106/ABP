using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UnitTest.Generic
{
    public static class ReflexionExtension
    {
        public static bool InheritsOrImplements(this Type child, Type parent)
        {
            if (child == parent)
                return false;

            var currentChild = parent.IsGenericTypeDefinition && child.IsGenericType ? child.GetGenericTypeDefinition() : child;

            while (currentChild != typeof(object))
            {
                if (parent == currentChild || HasAnyInterfaces(parent, currentChild))
                    return true;

                currentChild = currentChild.BaseType != null && parent.IsGenericTypeDefinition && currentChild.BaseType.IsGenericType
                                    ? currentChild.BaseType.GetGenericTypeDefinition()
                                    : currentChild.BaseType;

                if (currentChild == null)
                    return false;
            }
            return false;
        }

        private static bool HasAnyInterfaces(Type parent, Type child)
        {
            return child.GetInterfaces().Any(childInterface =>
            {
                var currentInterface = parent.IsGenericTypeDefinition && childInterface.IsGenericType
                    ? childInterface.GetGenericTypeDefinition()
                    : childInterface;

                return currentInterface == parent;
            });

        }

        public static bool IsSubClassOfGeneric(this Type child, Type parent)
        {
            if (child == parent)
                return false;

            if (child.IsSubclassOf(parent))
                return true;
            
            
            var parameters = parent.GetGenericArguments();

            var isParameterLessGeneric = !(parameters.Length > 0 &&
                ((parameters[0].Attributes & TypeAttributes.BeforeFieldInit) == TypeAttributes.BeforeFieldInit));

            while (child != null && child != typeof(object))
            {
                var cur = GetFullTypeDefinition(child);
                if (parent == cur ||
                    (isParameterLessGeneric &&
                     cur.GetInterfaces().Select(GetFullTypeDefinition).Contains(GetFullTypeDefinition(parent))))
                {
                    return true;
                }

                if (!isParameterLessGeneric)
                {
                    if (GetFullTypeDefinition(parent) == cur && !cur.IsInterface)
                    {
                        if (VerifyGenericArguments(GetFullTypeDefinition(parent), cur))
                        {
                            if (VerifyGenericArguments(parent, child))
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        //foreach (
                        //   var item in
                        //       child.GetInterfaces()
                        //           .Where(i => GetFullTypeDefinition(parent) == GetFullTypeDefinition(i)))
                        //{
                        //    if (VerifyGenericArguments(parent, item))
                        //        return true;
                        //}

                        if (child.GetInterfaces()
                            .Where(i => GetFullTypeDefinition(parent) == GetFullTypeDefinition(i)).Any(item => VerifyGenericArguments(parent, item)))
                        {
                            return true;
                        }
                    }
                }
                child = child.BaseType;
            }

            return false;
        }

        private static Type GetFullTypeDefinition(Type type)
        {
            return type.IsGenericType ? type.GetGenericTypeDefinition() : type;
        }

        private static bool VerifyGenericArguments(Type parent, Type child)
        {
            var childArguments = child.GetGenericArguments();
            var parentArguments = parent.GetGenericArguments();
            if (childArguments.Length == parentArguments.Length)
            {
                for (int i = 0; i < childArguments.Length; i++)
                {
                    if (childArguments[i].Assembly != parentArguments[i].Assembly ||
                        childArguments[i].Name != parentArguments[i].Name ||
                        childArguments[i].Namespace != parentArguments[i].Namespace)
                    {
                        if (!childArguments[i].IsSubclassOf(parentArguments[i]))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
