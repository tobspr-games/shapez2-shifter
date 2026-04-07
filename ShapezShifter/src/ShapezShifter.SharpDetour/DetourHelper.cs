using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using MonoMod.RuntimeDetour;

// ReSharper disable ArgumentsStyleNamedExpression
namespace ShapezShifter.SharpDetour
{
    /// <summary>
    /// Collection of type-safe (ish) methods designed to help creating detours
    /// </summary>
    /// <remarks>
    /// Notice that the implementation uses a lot of reflection (so does MonoMod) and thus we are very limited in what we
    /// can validate statically
    /// </remarks>
    [PublicAPI]
    public static class DetourHelper
    {
        #region Instance prefix with no return

        public static Hook CreatePrefixHook<TObject>(Expression<Action<TObject>> original, Action<TObject> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(actualMethodBody, target: (Action<Action<TObject>, TObject>)Patch);

            void Patch(Action<TObject> orig, TObject self)
            {
                prefix(self);
                orig(self);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0>(
            Expression<Action<TObject, TArg0>> original,
            Func<TArg0, TArg0> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(actualMethodBody, target: (Action<Action<TObject, TArg0>, TObject, TArg0>)Patch);

            void Patch(Action<TObject, TArg0> orig, TObject self, TArg0 arg0)
            {
                arg0 = prefix(arg0);
                orig(self, arg0);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TArg1>(
            Expression<Action<TObject, TArg0, TArg1>> original,
            Func<TObject, TArg0, TArg1, (TArg0, TArg1)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(
                actualMethodBody,
                target: (Action<Action<TObject, TArg0, TArg1>, TObject, TArg0, TArg1>)Patch);

            void Patch(Action<TObject, TArg0, TArg1> orig, TObject self, TArg0 arg0, TArg1 arg1)
            {
                (arg0, arg1) = prefix(self, arg0, arg1);
                orig(self, arg0, arg1);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TArg1, TArg2>(
            Expression<Action<TObject, TArg0, TArg1, TArg2>> original,
            Func<TObject, TArg0, TArg1, TArg2, (TArg0, TArg1, TArg2)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(
                actualMethodBody,
                target: (Action<Action<TObject, TArg0, TArg1, TArg2>, TObject, TArg0, TArg1, TArg2>)Patch);

            void Patch(Action<TObject, TArg0, TArg1, TArg2> orig, TObject self, TArg0 arg0, TArg1 arg1, TArg2 arg2)
            {
                (arg0, arg1, arg2) = prefix(self, arg0, arg1, arg2);
                orig(self, arg0, arg1, arg2);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TArg1, TArg2, TArg3>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, (TArg0, TArg1, TArg2, TArg3)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(
                actualMethodBody,
                target: (Action<Action<TObject, TArg0, TArg1, TArg2, TArg3>, TObject, TArg0, TArg1, TArg2, TArg3>)
                Patch);

            void Patch(
                Action<TObject, TArg0, TArg1, TArg2, TArg3> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3)
            {
                (arg0, arg1, arg2, arg3) = prefix(self, arg0, arg1, arg2, arg3);
                orig(self, arg0, arg1, arg2, arg3);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TArg1, TArg2, TArg3, TArg4>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, (TArg0, TArg1, TArg2, TArg3, TArg4)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(
                actualMethodBody,
                target: (Action<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4>, TObject, TArg0, TArg1, TArg2, TArg3,
                    TArg4>)Patch);

            void Patch(
                Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4)
            {
                (arg0, arg1, arg2, arg3, arg4) = prefix(self, arg0, arg1, arg2, arg3, arg4);
                orig(self, arg0, arg1, arg2, arg3, arg4);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, (TArg0, TArg1, TArg2, TArg3, TArg4, TArg5)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(
                actualMethodBody,
                target: (Action<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>, TObject, TArg0, TArg1, TArg2,
                    TArg3, TArg4, TArg5>)Patch);

            void Patch(
                Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5)
            {
                (arg0, arg1, arg2, arg3, arg4, arg5) = prefix(self, arg0, arg1, arg2, arg3, arg4, arg5);
                orig(self, arg0, arg1, arg2, arg3, arg4, arg5);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, (TArg0, TArg1, TArg2, TArg3, TArg4, TArg5,
                TArg6)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(
                actualMethodBody,
                target: (Action<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>, TObject, TArg0, TArg1,
                    TArg2, TArg3, TArg4, TArg5, TArg6>)Patch);

            void Patch(
                Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5,
                TArg6 arg6)
            {
                (arg0, arg1, arg2, arg3, arg4, arg5, arg6) = prefix(self, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
                orig(self, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            }
        }

        #endregion

        #region Instance prefix with return

        public static Hook CreatePrefixHook<TObject, TReturn>(
            Expression<Func<TObject, TReturn>> original,
            Action<TObject> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(actualMethodBody, target: (Func<Func<TObject, TReturn>, TObject, TReturn>)Patch);

            TReturn Patch(Func<TObject, TReturn> orig, TObject self)
            {
                prefix(self);
                return orig(self);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TReturn>(
            Expression<Func<TObject, TArg0, TReturn>> original,
            Func<TObject, TArg0, TArg0> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(
                actualMethodBody,
                target: (Func<Func<TObject, TArg0, TReturn>, TObject, TArg0, TReturn>)Patch);

            TReturn Patch(Func<TObject, TArg0, TReturn> orig, TObject self, TArg0 arg0)
            {
                arg0 = prefix(self, arg0);
                return orig(self, arg0);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TArg1, TReturn>(
            Expression<Func<TObject, TArg0, TArg1, TReturn>> original,
            Func<TObject, TArg0, TArg1, (TArg0, TArg1)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(
                actualMethodBody,
                target: (Func<Func<TObject, TArg0, TArg1, TReturn>, TObject, TArg0, TArg1, TReturn>)Patch);

            TReturn Patch(Func<TObject, TArg0, TArg1, TReturn> orig, TObject self, TArg0 arg0, TArg1 arg1)
            {
                (arg0, arg1) = prefix(self, arg0, arg1);
                return orig(self, arg0, arg1);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TArg1, TArg2, TReturn>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TReturn>> original,
            Func<TObject, TArg0, TArg1, TArg2, (TArg0, TArg1, TArg2)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(
                actualMethodBody,
                target: (Func<Func<TObject, TArg0, TArg1, TArg2, TReturn>, TObject, TArg0, TArg1, TArg2, TReturn>)
                Patch);

            TReturn Patch(
                Func<TObject, TArg0, TArg1, TArg2, TReturn> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2)
            {
                (arg0, arg1, arg2) = prefix(self, arg0, arg1, arg2);
                return orig(self, arg0, arg1, arg2);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TArg1, TArg2, TArg3, TReturn>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TArg3, TReturn>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, (TArg0, TArg1, TArg2, TArg3)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(
                actualMethodBody,
                target: (Func<Func<TObject, TArg0, TArg1, TArg2, TArg3, TReturn>, TObject, TArg0, TArg1, TArg2, TArg3,
                    TReturn>)Patch);

            TReturn Patch(
                Func<TObject, TArg0, TArg1, TArg2, TArg3, TReturn> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3)
            {
                (arg0, arg1, arg2, arg3) = prefix(self, arg0, arg1, arg2, arg3);
                return orig(self, arg0, arg1, arg2, arg3);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TReturn>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TReturn>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, (TArg0, TArg1, TArg2, TArg3, TArg4)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(
                actualMethodBody,
                target: (Func<Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TReturn>, TObject, TArg0, TArg1, TArg2,
                    TArg3, TArg4, TReturn>)Patch);

            TReturn Patch(
                Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TReturn> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4)
            {
                (arg0, arg1, arg2, arg3, arg4) = prefix(self, arg0, arg1, arg2, arg3, arg4);
                return orig(self, arg0, arg1, arg2, arg3, arg4);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, (TArg0, TArg1, TArg2, TArg3, TArg4, TArg5)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(
                actualMethodBody,
                target: (Func<Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>, TObject, TArg0, TArg1,
                    TArg2, TArg3, TArg4, TArg5, TReturn>)Patch);

            TReturn Patch(
                Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TReturn> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5)
            {
                (arg0, arg1, arg2, arg3, arg4, arg5) = prefix(self, arg0, arg1, arg2, arg3, arg4, arg5);
                return orig(self, arg0, arg1, arg2, arg3, arg4, arg5);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, (TArg0, TArg1, TArg2, TArg3, TArg4, TArg5,
                TArg6)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(
                actualMethodBody,
                target: (Func<Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>, TObject, TArg0,
                    TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>)Patch);

            TReturn Patch(
                Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5,
                TArg6 arg6)
            {
                (arg0, arg1, arg2, arg3, arg4, arg5, arg6) = prefix(self, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
                return orig(self, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            }
        }

        #endregion

        #region Static prefix with no return

        public static Hook CreateStaticPrefixHook<TObject>(Expression<Action<TObject>> original, Action prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, target: (Action<Action>)Patch);

            void Patch(Action orig)
            {
                prefix();
                orig();
            }
        }

        public static Hook CreateStaticPrefixHook<TObject, TArg0>(
            Expression<Action<TArg0>> original,
            Func<TArg0, TArg0> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, target: (Action<Action<TArg0>, TArg0>)Patch);

            void Patch(Action<TArg0> orig, TArg0 arg0)
            {
                arg0 = prefix(arg0);
                orig(arg0);
            }
        }

        public static Hook CreateStaticPrefixHook<TObject, TArg0, TArg1>(
            Expression<Action<TArg0, TArg1>> original,
            Func<TArg0, TArg1, (TArg0, TArg1)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, target: (Action<Action<TArg0, TArg1>, TArg0, TArg1>)Patch);

            void Patch(Action<TArg0, TArg1> orig, TArg0 arg0, TArg1 arg1)
            {
                (arg0, arg1) = prefix(arg0, arg1);
                orig(arg0, arg1);
            }
        }

        public static Hook CreateStaticPrefixHook<TObject, TArg0, TArg1, TArg2>(
            Expression<Action<TObject, TArg0, TArg1, TArg2>> original,
            Func<TArg0, TArg1, TArg2, (TArg0, TArg1, TArg2)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, target: (Action<Action<TArg0, TArg1, TArg2>, TArg0, TArg1, TArg2>)Patch);

            void Patch(Action<TArg0, TArg1, TArg2> orig, TArg0 arg0, TArg1 arg1, TArg2 arg2)
            {
                (arg0, arg1, arg2) = prefix(arg0, arg1, arg2);
                orig(arg0, arg1, arg2);
            }
        }

        public static Hook CreateStaticPrefixHook<TObject, TArg0, TArg1, TArg2, TArg3>(
            Expression<Action<TArg0, TArg1, TArg2, TArg3>> original,
            Func<TArg0, TArg1, TArg2, TArg3, (TArg0, TArg1, TArg2, TArg3)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TArg0, TArg1, TArg2, TArg3>, TArg0, TArg1, TArg2, TArg3>)Patch);

            void Patch(Action<TArg0, TArg1, TArg2, TArg3> orig, TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3)
            {
                (arg0, arg1, arg2, arg3) = prefix(arg0, arg1, arg2, arg3);
                orig(arg0, arg1, arg2, arg3);
            }
        }

        public static Hook CreateStaticPrefixHook<TObject, TArg0, TArg1, TArg2, TArg3, TArg4>(
            Expression<Action<TArg0, TArg1, TArg2, TArg3, TArg4>> original,
            Func<TArg0, TArg1, TArg2, TArg3, TArg4, (TArg0, TArg1, TArg2, TArg3, TArg4)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TArg0, TArg1, TArg2, TArg3, TArg4>, TArg0, TArg1, TArg2, TArg3, TArg4>)Patch);

            void Patch(
                Action<TArg0, TArg1, TArg2, TArg3, TArg4> orig,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4)
            {
                (arg0, arg1, arg2, arg3, arg4) = prefix(arg0, arg1, arg2, arg3, arg4);
                orig(arg0, arg1, arg2, arg3, arg4);
            }
        }

        public static Hook CreateStaticPrefixHook<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>(
            Expression<Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>> original,
            Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, (TArg0, TArg1, TArg2, TArg3, TArg4, TArg5)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>, TArg0, TArg1, TArg2, TArg3, TArg4,
                    TArg5>)Patch);

            void Patch(
                Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5> orig,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5)
            {
                (arg0, arg1, arg2, arg3, arg4, arg5) = prefix(arg0, arg1, arg2, arg3, arg4, arg5);
                orig(arg0, arg1, arg2, arg3, arg4, arg5);
            }
        }

        public static Hook CreateStaticPrefixHook<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(
            Expression<Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> original,
            Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, (TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6)>
                prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>, TArg0, TArg1, TArg2, TArg3,
                    TArg4, TArg5, TArg6>)Patch);

            void Patch(
                Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> orig,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5,
                TArg6 arg6)
            {
                (arg0, arg1, arg2, arg3, arg4, arg5, arg6) = prefix(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
                orig(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            }
        }

        #endregion

        #region Static prefix with return

        public static Hook CreateStaticPrefixHook<TResult>(Expression<Func<TResult>> original, Action prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, target: (Func<Func<TResult>, TResult>)Patch);

            TResult Patch(Func<TResult> orig)
            {
                prefix();
                return orig();
            }
        }

        public static Hook CreateStaticPrefixHook<TArg0, TResult>(
            Expression<Func<TArg0, TResult>> original,
            Func<TArg0, TArg0> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, target: (Func<Func<TArg0, TResult>, TArg0, TResult>)Patch);

            TResult Patch(Func<TArg0, TResult> orig, TArg0 arg0)
            {
                arg0 = prefix(arg0);
                return orig(arg0);
            }
        }

        public static Hook CreateStaticPrefixHook<TArg0, TArg1, TResult>(
            Expression<Func<TArg0, TArg1, TResult>> original,
            Func<TArg0, TArg1, (TArg0, TArg1)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, target: (Func<Func<TArg0, TArg1, TResult>, TArg0, TArg1, TResult>)Patch);

            TResult Patch(Func<TArg0, TArg1, TResult> orig, TArg0 arg0, TArg1 arg1)
            {
                (arg0, arg1) = prefix(arg0, arg1);
                return orig(arg0, arg1);
            }
        }

        public static Hook CreateStaticPrefixHook<TArg0, TArg1, TArg2, TResult>(
            Expression<Func<TArg0, TArg1, TArg2, TResult>> original,
            Func<TArg0, TArg1, TArg2, (TArg0, TArg1, TArg2)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(
                actualMethodBody,
                target: (Func<Func<TArg0, TArg1, TArg2, TResult>, TArg0, TArg1, TArg2, TResult>)Patch);

            TResult Patch(Func<TArg0, TArg1, TArg2, TResult> orig, TArg0 arg0, TArg1 arg1, TArg2 arg2)
            {
                (arg0, arg1, arg2) = prefix(arg0, arg1, arg2);
                return orig(arg0, arg1, arg2);
            }
        }

        public static Hook CreateStaticPrefixHook<TArg0, TArg1, TArg2, TArg3, TResult>(
            Expression<Func<TArg0, TArg1, TArg2, TArg3, TResult>> original,
            Func<TArg0, TArg1, TArg2, TArg3, (TArg0, TArg1, TArg2, TArg3)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(
                actualMethodBody,
                target: (Func<Func<TArg0, TArg1, TArg2, TArg3, TResult>, TArg0, TArg1, TArg2, TArg3, TResult>)Patch);

            TResult Patch(
                Func<TArg0, TArg1, TArg2, TArg3, TResult> orig,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3)
            {
                (arg0, arg1, arg2, arg3) = prefix(arg0, arg1, arg2, arg3);
                return orig(arg0, arg1, arg2, arg3);
            }
        }

        public static Hook CreateStaticPrefixHook<TArg0, TArg1, TArg2, TArg3, TArg4, TResult>(
            Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TResult>> original,
            Func<TArg0, TArg1, TArg2, TArg3, TArg4, (TArg0, TArg1, TArg2, TArg3, TArg4)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(
                actualMethodBody,
                target: (Func<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TResult>, TArg0, TArg1, TArg2, TArg3, TArg4,
                    TResult>)Patch);

            TResult Patch(
                Func<TArg0, TArg1, TArg2, TArg3, TArg4, TResult> orig,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4)
            {
                (arg0, arg1, arg2, arg3, arg4) = prefix(arg0, arg1, arg2, arg3, arg4);
                return orig(arg0, arg1, arg2, arg3, arg4);
            }
        }

        public static Hook CreateStaticPrefixHook<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(
            Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> original,
            Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, (TArg0, TArg1, TArg2, TArg3, TArg4, TArg5)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(
                actualMethodBody,
                target: (Func<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>, TArg0, TArg1, TArg2, TArg3, TArg4
                    , TArg5, TResult>)Patch);

            TResult Patch(
                Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult> orig,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5)
            {
                (arg0, arg1, arg2, arg3, arg4, arg5) = prefix(arg0, arg1, arg2, arg3, arg4, arg5);
                return orig(arg0, arg1, arg2, arg3, arg4, arg5);
            }
        }

        public static Hook CreateStaticPrefixHook<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(
            Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> original,
            Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, (TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6)>
                prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(
                actualMethodBody,
                target: (Func<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>, TArg0, TArg1, TArg2, TArg3
                    , TArg4, TArg5, TArg6, TResult>)Patch);

            TResult Patch(
                Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult> orig,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5,
                TArg6 arg6)
            {
                (arg0, arg1, arg2, arg3, arg4, arg5, arg6) = prefix(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
                return orig(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            }
        }

        #endregion

        #region Instance postfix with no return

        public static Hook CreatePostfixHook<TObject>(Expression<Action<TObject>> original, Action<TObject> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, target: (Action<Action<TObject>, TObject>)Patch);

            void Patch(Action<TObject> orig, TObject self)
            {
                orig(self);
                postfix(self);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0>(
            Expression<Action<TObject, TArg0>> original,
            Action<TObject, TArg0> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, target: (Action<Action<TObject, TArg0>, TObject, TArg0>)Patch);

            void Patch(Action<TObject, TArg0> orig, TObject self, TArg0 arg0)
            {
                orig(self, arg0);
                postfix(self, arg0);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1>(
            Expression<Action<TObject, TArg0, TArg1>> original,
            Action<TObject, TArg0, TArg1> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TObject, TArg0, TArg1>, TObject, TArg0, TArg1>)Patch);

            void Patch(Action<TObject, TArg0, TArg1> orig, TObject self, TArg0 arg0, TArg1 arg1)
            {
                orig(self, arg0, arg1);
                postfix(self, arg0, arg1);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1, TArg2>(
            Expression<Action<TObject, TArg0, TArg1, TArg2>> original,
            Action<TObject, TArg0, TArg1, TArg2> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TObject, TArg0, TArg1, TArg2>, TObject, TArg0, TArg1, TArg2>)Patch);

            void Patch(Action<TObject, TArg0, TArg1, TArg2> orig, TObject self, TArg0 arg0, TArg1 arg1, TArg2 arg2)
            {
                orig(self, arg0, arg1, arg2);
                postfix(self, arg0, arg1, arg2);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1, TArg2, TArg3>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3>> original,
            Action<TObject, TArg0, TArg1, TArg2, TArg3> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TObject, TArg0, TArg1, TArg2, TArg3>, TObject, TArg0, TArg1, TArg2, TArg3>)
                Patch);

            void Patch(
                Action<TObject, TArg0, TArg1, TArg2, TArg3> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3)
            {
                orig(self, arg0, arg1, arg2, arg3);
                postfix(self, arg0, arg1, arg2, arg3);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1, TArg2, TArg3, TArg4>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4>> original,
            Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4>, TObject, TArg0, TArg1, TArg2, TArg3,
                    TArg4>)Patch);

            void Patch(
                Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4)
            {
                orig(self, arg0, arg1, arg2, arg3, arg4);
                postfix(self, arg0, arg1, arg2, arg3, arg4);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>> original,
            Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>, TObject, TArg0, TArg1, TArg2,
                    TArg3, TArg4, TArg5>)Patch);

            void Patch(
                Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5)
            {
                orig(self, arg0, arg1, arg2, arg3, arg4, arg5);
                postfix(self, arg0, arg1, arg2, arg3, arg4, arg5);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> original,
            Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>, TObject, TArg0, TArg1,
                    TArg2, TArg3, TArg4, TArg5, TArg6>)Patch);

            void Patch(
                Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5,
                TArg6 arg6)
            {
                orig(self, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
                postfix(self, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            }
        }

        #endregion

        #region Instance postfix with return

        public static Hook CreatePostfixHook<TObject, TResult>(
            Expression<Func<TObject, TResult>> original,
            Func<TObject, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, target: (Func<Func<TObject, TResult>, TObject, TResult>)Patch);

            TResult Patch(Func<TObject, TResult> orig, TObject self)
            {
                TResult value = orig(self);
                return postfix(self, value);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TResult>(
            Expression<Func<TObject, TArg0, TResult>> original,
            Func<TObject, TArg0, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Func<Func<TObject, TArg0, TResult>, TObject, TArg0, TResult>)Patch);

            TResult Patch(Func<TObject, TArg0, TResult> orig, TObject self, TArg0 arg0)
            {
                TResult value = orig(self, arg0);
                return postfix(self, arg0, value);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1, TResult>(
            Expression<Func<TObject, TArg0, TArg1, TResult>> original,
            Func<TObject, TArg0, TArg1, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Func<Func<TObject, TArg0, TArg1, TResult>, TObject, TArg0, TArg1, TResult>)Patch);

            TResult Patch(Func<TObject, TArg0, TArg1, TResult> orig, TObject self, TArg0 arg0, TArg1 arg1)
            {
                TResult value = orig(self, arg0, arg1);
                return postfix(self, arg0, arg1, value);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1, TArg2, TResult>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TResult>> original,
            Func<TObject, TArg0, TArg1, TArg2, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Func<Func<TObject, TArg0, TArg1, TArg2, TResult>, TObject, TArg0, TArg1, TArg2, TResult>)
                Patch);

            TResult Patch(
                Func<TObject, TArg0, TArg1, TArg2, TResult> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2)
            {
                TResult value = orig(self, arg0, arg1, arg2);
                return postfix(self, arg0, arg1, arg2, value);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1, TArg2, TArg3, TResult>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TArg3, TResult>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Func<Func<TObject, TArg0, TArg1, TArg2, TArg3, TResult>, TObject, TArg0, TArg1, TArg2, TArg3,
                    TResult>)Patch);

            TResult Patch(
                Func<TObject, TArg0, TArg1, TArg2, TArg3, TResult> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3)
            {
                TResult value = orig(self, arg0, arg1, arg2, arg3);
                return postfix(self, arg0, arg1, arg2, arg3, value);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TResult>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TResult>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Func<Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TResult>, TObject, TArg0, TArg1, TArg2,
                    TArg3, TArg4, TResult>)Patch);

            TResult Patch(
                Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TResult> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4)
            {
                TResult value = orig(self, arg0, arg1, arg2, arg3, arg4);
                return postfix(self, arg0, arg1, arg2, arg3, arg4, value);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Func<Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>, TObject, TArg0, TArg1,
                    TArg2, TArg3, TArg4, TArg5, TResult>)Patch);

            TResult Patch(
                Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5)
            {
                TResult value = orig(self, arg0, arg1, arg2, arg3, arg4, arg5);
                return postfix(self, arg0, arg1, arg2, arg3, arg4, arg5, value);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Func<Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>, TObject, TArg0,
                    TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>)Patch);

            TResult Patch(
                Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5,
                TArg6 arg6)
            {
                TResult value = orig(self, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
                return postfix(self, arg0, arg1, arg2, arg3, arg4, arg5, arg6, value);
            }
        }

        #endregion

        #region Static Postfix with no return

        public static Hook CreateStaticPostfixHook<TObject>(Expression<Action<TObject>> original, Action postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, target: (Action<Action>)Patch);

            void Patch(Action orig)
            {
                orig();
                postfix();
            }
        }

        public static Hook CreateStaticPostfixHook<TArg0>(Expression<Action<TArg0>> original, Action<TArg0> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, target: (Action<Action<TArg0>, TArg0>)Patch);

            void Patch(Action<TArg0> orig, TArg0 arg0)
            {
                orig(arg0);
                postfix(arg0);
            }
        }

        public static Hook CreateStaticPostfixHook<TArg0, TArg1>(
            Expression<Action<TArg0, TArg1>> original,
            Action<TArg0, TArg1> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, target: (Action<Action<TArg0, TArg1>, TArg0, TArg1>)Patch);

            void Patch(Action<TArg0, TArg1> orig, TArg0 arg0, TArg1 arg1)
            {
                orig(arg0, arg1);
                postfix(arg0, arg1);
            }
        }

        public static Hook CreateStaticPostfixHook<TArg0, TArg1, TArg2>(
            Expression<Action<TArg0, TArg1, TArg2>> original,
            Action<TArg0, TArg1, TArg2> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, target: (Action<Action<TArg0, TArg1, TArg2>, TArg0, TArg1, TArg2>)Patch);

            void Patch(Action<TArg0, TArg1, TArg2> orig, TArg0 arg0, TArg1 arg1, TArg2 arg2)
            {
                orig(arg0, arg1, arg2);
                postfix(arg0, arg1, arg2);
            }
        }

        public static Hook CreateStaticPostfixHook<TArg0, TArg1, TArg2, TArg3>(
            Expression<Action<TArg0, TArg1, TArg2, TArg3>> original,
            Action<TArg0, TArg1, TArg2, TArg3> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TArg0, TArg1, TArg2, TArg3>, TArg0, TArg1, TArg2, TArg3>)Patch);

            void Patch(Action<TArg0, TArg1, TArg2, TArg3> orig, TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3)
            {
                orig(arg0, arg1, arg2, arg3);
                postfix(arg0, arg1, arg2, arg3);
            }
        }

        public static Hook CreateStaticPostfixHook<TArg0, TArg1, TArg2, TArg3, TArg4>(
            Expression<Action<TArg0, TArg1, TArg2, TArg3, TArg4>> original,
            Action<TArg0, TArg1, TArg2, TArg3, TArg4> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TArg0, TArg1, TArg2, TArg3, TArg4>, TArg0, TArg1, TArg2, TArg3, TArg4>)Patch);

            void Patch(
                Action<TArg0, TArg1, TArg2, TArg3, TArg4> orig,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4)
            {
                orig(arg0, arg1, arg2, arg3, arg4);
                postfix(arg0, arg1, arg2, arg3, arg4);
            }
        }

        public static Hook CreateStaticPostfixHook<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>(
            Expression<Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>> original,
            Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>, TArg0, TArg1, TArg2, TArg3, TArg4,
                    TArg5>)Patch);

            void Patch(
                Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5> orig,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5)
            {
                orig(arg0, arg1, arg2, arg3, arg4, arg5);
                postfix(arg0, arg1, arg2, arg3, arg4, arg5);
            }
        }

        public static Hook CreateStaticPostfixHook<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(
            Expression<Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> original,
            Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>, TArg0, TArg1, TArg2, TArg3,
                    TArg4, TArg5, TArg6>)Patch);

            void Patch(
                Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> orig,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5,
                TArg6 arg6)
            {
                orig(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
                postfix(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            }
        }

        #endregion

        #region Static Postfix with return

        public static Hook CreateStaticPostfixHook<TResult>(
            Expression<Func<TResult>> original,
            Func<TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, target: (Func<Func<TResult>, TResult>)Patch);

            TResult Patch(Func<TResult> orig)
            {
                TResult value = orig();
                return postfix(value);
            }
        }

        public static Hook CreateStaticPostfixHook<TArg0, TResult>(
            Expression<Func<TArg0, TResult>> original,
            Func<TArg0, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, target: (Func<Func<TArg0, TResult>, TArg0, TResult>)Patch);

            TResult Patch(Func<TArg0, TResult> orig, TArg0 arg0)
            {
                TResult value = orig(arg0);
                return postfix(arg0, value);
            }
        }

        public static Hook CreateStaticPostfixHook<TArg0, TArg1, TResult>(
            Expression<Func<TArg0, TArg1, TResult>> original,
            Func<TArg0, TArg1, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, target: (Func<Func<TArg0, TArg1, TResult>, TArg0, TArg1, TResult>)Patch);

            TResult Patch(Func<TArg0, TArg1, TResult> orig, TArg0 arg0, TArg1 arg1)
            {
                TResult value = orig(arg0, arg1);
                return postfix(arg0, arg1, value);
            }
        }

        public static Hook CreateStaticPostfixHook<TArg0, TArg1, TArg2, TResult>(
            Expression<Func<TArg0, TArg1, TArg2, TResult>> original,
            Func<TArg0, TArg1, TArg2, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(
                actualMethodBody,
                target: (Func<Func<TArg0, TArg1, TArg2, TResult>, TArg0, TArg1, TArg2, TResult>)Patch);

            TResult Patch(Func<TArg0, TArg1, TArg2, TResult> orig, TArg0 arg0, TArg1 arg1, TArg2 arg2)
            {
                TResult value = orig(arg0, arg1, arg2);
                return postfix(arg0, arg1, arg2, value);
            }
        }

        public static Hook CreateStaticPostfixHook<TArg0, TArg1, TArg2, TArg3, TResult>(
            Expression<Func<TArg0, TArg1, TArg2, TArg3, TResult>> original,
            Func<TArg0, TArg1, TArg2, TArg3, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(
                actualMethodBody,
                target: (Func<Func<TArg0, TArg1, TArg2, TArg3, TResult>, TArg0, TArg1, TArg2, TArg3, TResult>)Patch);

            TResult Patch(
                Func<TArg0, TArg1, TArg2, TArg3, TResult> orig,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3)
            {
                TResult value = orig(arg0, arg1, arg2, arg3);
                return postfix(arg0, arg1, arg2, arg3, value);
            }
        }

        public static Hook CreateStaticPostfixHook<TArg0, TArg1, TArg2, TArg3, TArg4, TResult>(
            Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TResult>> original,
            Func<TArg0, TArg1, TArg2, TArg3, TArg4, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(
                actualMethodBody,
                target: (Func<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TResult>, TArg0, TArg1, TArg2, TArg3, TArg4,
                    TResult>)Patch);

            TResult Patch(
                Func<TArg0, TArg1, TArg2, TArg3, TArg4, TResult> orig,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4)
            {
                TResult value = orig(arg0, arg1, arg2, arg3, arg4);
                return postfix(arg0, arg1, arg2, arg3, arg4, value);
            }
        }

        public static Hook CreateStaticPostfixHook<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(
            Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> original,
            Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(
                actualMethodBody,
                target: (Func<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>, TArg0, TArg1, TArg2, TArg3, TArg4
                    , TArg5, TResult>)Patch);

            TResult Patch(
                Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult> orig,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5)
            {
                TResult value = orig(arg0, arg1, arg2, arg3, arg4, arg5);
                return postfix(arg0, arg1, arg2, arg3, arg4, arg5, value);
            }
        }

        public static Hook CreateStaticPostfixHook<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(
            Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> original,
            Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(
                actualMethodBody,
                target: (Func<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>, TArg0, TArg1, TArg2, TArg3
                    , TArg4, TArg5, TArg6, TResult>)Patch);

            TResult Patch(
                Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult> orig,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5,
                TArg6 arg6)
            {
                TResult value = orig(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
                return postfix(arg0, arg1, arg2, arg3, arg4, arg5, arg6, value);
            }
        }

        #endregion

        #region Skip with no return

        public static Hook Skip<TObject>(Expression<Action<TObject>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, target: (Action<Action<TObject>, TObject>)Patch);

            static void Patch(Action<TObject> orig, TObject self) { }
        }

        public static Hook Skip<TObject, TArg0>(Expression<Action<TObject, TArg0>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, target: (Action<Action<TObject, TArg0>, TObject, TArg0>)Patch);

            static void Patch(Action<TObject, TArg0> orig, TObject self, TArg0 arg0) { }
        }

        public static Hook Skip<TObject, TArg0, TArg1>(Expression<Action<TObject, TArg0, TArg1>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TObject, TArg0, TArg1>, TObject, TArg0, TArg1>)Patch);

            static void Patch(Action<TObject, TArg0, TArg1> orig, TObject self, TArg0 arg0, TArg1 arg1) { }
        }

        public static Hook Skip<TObject, TArg0, TArg1, TArg2>(Expression<Action<TObject, TArg0, TArg1, TArg2>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TObject, TArg0, TArg1, TArg2>, TObject, TArg0, TArg1, TArg2>)Patch);

            static void Patch(
                Action<TObject, TArg0, TArg1, TArg2> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2) { }
        }

        public static Hook Skip<TObject, TArg0, TArg1, TArg2, TArg3>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TObject, TArg0, TArg1, TArg2, TArg3>, TObject, TArg0, TArg1, TArg2, TArg3>)
                Patch);

            static void Patch(
                Action<TObject, TArg0, TArg1, TArg2, TArg3> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3) { }
        }

        public static Hook Skip<TObject, TArg0, TArg1, TArg2, TArg3, TArg4>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4>, TObject, TArg0, TArg1, TArg2, TArg3,
                    TArg4>)Patch);

            static void Patch(
                Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4) { }
        }

        public static Hook Skip<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>, TObject, TArg0, TArg1, TArg2,
                    TArg3, TArg4, TArg5>)Patch);

            static void Patch(
                Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5) { }
        }

        public static Hook Skip<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>, TObject, TArg0, TArg1,
                    TArg2, TArg3, TArg4, TArg5, TArg6>)Patch);

            static void Patch(
                Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> orig,
                TObject self,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5,
                TArg6 arg6) { }
        }

        #endregion

        #region Static skip with no return

        public static Hook StaticSkip(Expression<Action> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, target: (Action<Action>)Patch);

            static void Patch(Action orig) { }
        }

        public static Hook StaticSkip<TArg0>(Expression<Action<TArg0>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, target: (Action<Action<TArg0>, TArg0>)Patch);

            static void Patch(Action<TArg0> orig, TArg0 arg0) { }
        }

        public static Hook StaticSkip<TArg0, TArg1>(Expression<Action<TArg0, TArg1>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, target: (Action<Action<TArg0, TArg1>, TArg0, TArg1>)Patch);

            static void Patch(Action<TArg0, TArg1> orig, TArg0 arg0, TArg1 arg1) { }
        }

        public static Hook StaticSkip<TArg0, TArg1, TArg2>(Expression<Action<TArg0, TArg1, TArg2>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, target: (Action<Action<TArg0, TArg1, TArg2>, TArg0, TArg1, TArg2>)Patch);

            static void Patch(Action<TArg0, TArg1, TArg2> orig, TArg0 arg0, TArg1 arg1, TArg2 arg2) { }
        }

        public static Hook StaticSkip<TArg0, TArg1, TArg2, TArg3>(
            Expression<Action<TArg0, TArg1, TArg2, TArg3>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TArg0, TArg1, TArg2, TArg3>, TArg0, TArg1, TArg2, TArg3>)Patch);

            static void Patch(
                Action<TArg0, TArg1, TArg2, TArg3> orig,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3) { }
        }

        public static Hook StaticSkip<TArg0, TArg1, TArg2, TArg3, TArg4>(
            Expression<Action<TArg0, TArg1, TArg2, TArg3, TArg4>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TArg0, TArg1, TArg2, TArg3, TArg4>, TArg0, TArg1, TArg2, TArg3, TArg4>)Patch);

            static void Patch(
                Action<TArg0, TArg1, TArg2, TArg3, TArg4> orig,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4) { }
        }

        public static Hook StaticSkip<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>(
            Expression<Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>, TArg0, TArg1, TArg2, TArg3, TArg4,
                    TArg5>)Patch);

            static void Patch(
                Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5> orig,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5) { }
        }

        public static Hook StaticSkip<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(
            Expression<Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(
                actualMethodBody,
                target: (Action<Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>, TArg0, TArg1, TArg2, TArg3,
                    TArg4, TArg5, TArg6>)Patch);

            static void Patch(
                Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> orig,
                TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3,
                TArg4 arg4,
                TArg5 arg5,
                TArg6 arg6) { }
        }

        #endregion

        #region Replace with no return

        public static Hook Replace<TObject>(Expression<Action<TObject>> original, Action<TObject> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook Replace<TObject, TArg0>(
            Expression<Action<TObject, TArg0>> original,
            Action<TObject, TArg0> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook Replace<TObject, TArg0, TArg1>(
            Expression<Action<TObject, TArg0, TArg1>> original,
            Action<TObject, TArg0, TArg1> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook Replace<TObject, TArg0, TArg1, TArg2>(
            Expression<Action<TObject, TArg0, TArg1, TArg2>> original,
            Action<TObject, TArg0, TArg1, TArg2> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook Replace<TObject, TArg0, TArg1, TArg2, TArg3>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3>> original,
            Action<TObject, TArg0, TArg1, TArg2, TArg3> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook Replace<TObject, TArg0, TArg1, TArg2, TArg3, TArg4>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4>> original,
            Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook Replace<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>> original,
            Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook Replace<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> original,
            Action<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        #endregion

        #region Replace with return

        public static Hook Replace<TObject, TResult>(
            Expression<Func<TObject, TResult>> original,
            Func<TObject, TResult> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook Replace<TObject, TArg0, TResult>(
            Expression<Func<TObject, TArg0, TResult>> original,
            Func<TObject, TArg0, TResult> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook Replace<TObject, TArg0, TArg1, TResult>(
            Expression<Func<TObject, TArg0, TArg1, TResult>> original,
            Func<TObject, TArg0, TArg1, TResult> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook Replace<TObject, TArg0, TArg1, TArg2, TResult>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TResult>> original,
            Func<TObject, TArg0, TArg1, TArg2, TResult> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook Replace<TObject, TArg0, TArg1, TArg2, TArg3, TResult>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TArg3, TResult>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, TResult> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook Replace<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TResult>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TResult>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TResult> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook Replace<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook Replace<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        #endregion

        #region Static replace with no return

        public static Hook StaticReplace<TObject>(Expression<Action<TObject>> original, Action replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook StaticReplace<TArg0>(Expression<Action<TArg0>> original, Action<TArg0> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook StaticReplace<TArg0, TArg1>(
            Expression<Action<TArg0, TArg1>> original,
            Action<TArg0, TArg1> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook StaticReplace<TArg0, TArg1, TArg2>(
            Expression<Action<TArg0, TArg1, TArg2>> original,
            Action<TArg0, TArg1, TArg2> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook StaticReplace<TArg0, TArg1, TArg2, TArg3>(
            Expression<Action<TArg0, TArg1, TArg2, TArg3>> original,
            Action<TArg0, TArg1, TArg2, TArg3> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook StaticReplace<TArg0, TArg1, TArg2, TArg3, TArg4>(
            Expression<Action<TArg0, TArg1, TArg2, TArg3, TArg4>> original,
            Action<TArg0, TArg1, TArg2, TArg3, TArg4> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook StaticReplace<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>(
            Expression<Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5>> original,
            Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook StaticReplace<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(
            Expression<Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> original,
            Action<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, replacement);
        }

        #endregion

        #region Static replace with return

        public static Hook StaticReplace<TResult>(Expression<Func<TResult>> original, Func<TResult> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook StaticReplace<TArg0, TResult>(
            Expression<Func<TArg0, TResult>> original,
            Func<TArg0, TResult> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook StaticReplace<TArg0, TArg1, TResult>(
            Expression<Func<TArg0, TArg1, TResult>> original,
            Func<TArg0, TArg1, TResult> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook StaticReplace<TArg0, TArg1, TArg2, TResult>(
            Expression<Func<TArg0, TArg1, TArg2, TResult>> original,
            Func<TArg0, TArg1, TArg2, TResult> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook StaticReplace<TArg0, TArg1, TArg2, TArg3, TResult>(
            Expression<Func<TArg0, TArg1, TArg2, TArg3, TResult>> original,
            Func<TArg0, TArg1, TArg2, TArg3, TResult> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook StaticReplace<TArg0, TArg1, TArg2, TArg3, TArg4, TResult>(
            Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TResult>> original,
            Func<TArg0, TArg1, TArg2, TArg3, TArg4, TResult> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook StaticReplace<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(
            Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> original,
            Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook StaticReplace<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(
            Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> original,
            Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(original);
            return new Hook(actualMethodBody, replacement);
        }

        #endregion

        internal static MethodInfo GetRuntimeMethod<TObject>(LambdaExpression original)
        {
            return GetRuntimeMethod(type: typeof(TObject), original);
        }

        private static MethodInfo GetRuntimeMethod(LambdaExpression original)
        {
            Type type = ((MethodCallExpression)original.Body).Method.DeclaringType;

            return GetRuntimeMethod(type, original);
        }

        private static MethodInfo GetRuntimeMethod(Type type, LambdaExpression original)
        {
            MethodInfo method = ((MethodCallExpression)original.Body).Method;
            string methodName = method.Name;
            try
            {
                MethodInfo actualMethodBody = type.GetMethod(
                    methodName,
                    bindingAttr: BindingFlags.Public
                                 | BindingFlags.NonPublic
                                 | BindingFlags.Instance
                                 | BindingFlags.Static);
                if (actualMethodBody == null)
                {
                    throw new Exception($"Could not find method {methodName} in type {type} during runtime");
                }
                return actualMethodBody;
            }
            catch (AmbiguousMatchException)
            {
                var methods = type.GetMethods(
                                       bindingAttr: BindingFlags.Public
                                                    | BindingFlags.NonPublic
                                                    | BindingFlags.Instance
                                                    | BindingFlags.Static)
                                  .Where(x => x.Name == methodName);

                MethodInfo a = ((MethodCallExpression)original.Body).Method;

                foreach (MethodInfo potentialMethod in methods)
                {
                    if (ReflectedMethodArgumentsMatchExpression(method, potentialMethod))
                    {
                        return potentialMethod;
                    }
                }
                throw new Exception($"Could not find method {methodName} in type {type} during runtime");
            }
        }

        private static bool ReflectedMethodArgumentsMatchExpression(MethodInfo a, MethodInfo b)
        {
            Debugging.Logger.Info?.Log($"Comparing {a} to {b}");
            var aParams = a.GetParameters();
            var bParams = b.GetParameters();

            for (int i = 0; i < aParams.Length; i++)
            {
                ParameterInfo methodInfoParam = aParams[i];
                Debugging.Logger.Info?.Log($"Reflected param #{i}: {methodInfoParam.ParameterType}");
            }
            for (int i = 0; i < bParams.Length; i++)
            {
                ParameterInfo methodInfoParam = bParams[i];
                Debugging.Logger.Info?.Log($"Reflected param #{i}: {methodInfoParam.ParameterType}");
            }
            Debugging.Logger.Info?.Log($"{aParams.Length} vs {bParams.Length}");
            if (aParams.Length != bParams.Length)
            {
                return false;
            }

            for (int i = 0; i < aParams.Length; i++)
            {
                ParameterInfo expressionParam = aParams[i];
                ParameterInfo methodInfoParam = bParams[i];
                Debugging.Logger.Info?.Log(
                    $"Comparing {expressionParam.ParameterType} to {methodInfoParam.ParameterType}");
                if (expressionParam.ParameterType != methodInfoParam.ParameterType)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
