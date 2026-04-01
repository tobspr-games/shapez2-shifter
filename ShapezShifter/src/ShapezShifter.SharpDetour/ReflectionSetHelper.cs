using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using MonoMod.RuntimeDetour;

namespace ShapezShifter.SharpDetour
{
    [PublicAPI]
    public static class ReflectionSetHelper
    {
        public static Hook Set<TStruct, TField>(this ref TStruct structure, Expression<Func<TStruct, TField>> expr,
            TField value)
            where TStruct : struct
        {
            if (TryGetAccessedField(expr, out var fieldInfo))
            {
                object str = structure;
                fieldInfo.SetValue(str, value);
                structure = (TStruct)str;
                return null;
            }

            if (TryGetAccessedProperty(expr, out var property))
            {
                return new Hook(property.GetGetMethod(), new GetterStructDelegate<TStruct, TField>(GetterRef));

                TField GetterRef(ref TStruct s)
                {
                    return value;
                }
            }

            throw new Exception($"Expected member to be a field or property, but got {expr.GetType()}");
        }

        public static Hook Set<TObject, TField>(this TObject obj, Expression<Func<TObject, TField>> expr, TField value)
            where TObject : class
        {
            if (TryGetAccessedField(expr, out var fieldInfo))
            {
                fieldInfo.SetValue(obj, value);
                return null;
            }


            if (TryGetAccessedProperty(expr, out var property))
            {
                return new Hook(property.GetGetMethod(true), new GetterClassDelegate<TObject, TField>(Getter));

                TField Getter(TObject o)
                {
                    return value;
                }
            }

            throw new Exception($"Expected member to be a field or property, but got {expr.GetType()}");
        }


        private static bool TryGetAccessedField<TObject, TField>(Expression<Func<TObject, TField>> expression,
            out FieldInfo field)
        {
            if (expression.Body is not MemberExpression memberExpr)
            {
                field = null;
                return false;
            }

            if (memberExpr.Expression is not ParameterExpression parameter)
            {
                field = null;
                return false;
            }

            if (parameter != expression.Parameters[(Index)0])
            {
                field = null;
                return false;
            }

            if (memberExpr.Member is not FieldInfo fieldInfo)
            {
                field = null;
                return false;
            }

            field = fieldInfo;
            return true;
        }

        public static bool TryGetAccessedProperty<TObject, TField>(Expression<Func<TObject, TField>> expression,
            out PropertyInfo property)
        {
            if (expression.Body is not MemberExpression memberExpr)
            {
                property = null;
                return false;
            }

            if (memberExpr.Expression is not ParameterExpression parameter)
            {
                property = null;
                return false;
            }

            if (parameter != expression.Parameters[(Index)0])
            {
                property = null;
                return false;
            }

            if (memberExpr.Member is not PropertyInfo propertyInfo)
            {
                property = null;
                return false;
            }

            property = propertyInfo;
            return true;
        }

        private delegate TField GetterStructDelegate<TObject, out TField>(ref TObject obj);

        private delegate TField GetterClassDelegate<in TObject, out TField>(TObject obj);
    }
}