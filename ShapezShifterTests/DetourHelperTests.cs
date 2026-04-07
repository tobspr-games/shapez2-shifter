using System.Linq.Expressions;
using MonoMod.RuntimeDetour;
using ShapezShifter.SharpDetour;

namespace ShapezShifterTests
{
    public class DetourHelperTests
    {
        [SetUp]
        public void SetUp()
        {
            Target.SResult = 0;
        }

        #region Prefix with no return

        [Test]
        public void PrefixNoReturn_0Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePrefixHook<Target>(
                original: t2 => t2.V0(),
                prefix: self => { self.Result = 10; });
            t.V0();
            Assert.That(actual: t.Result, expression: Is.EqualTo(11)); // prefix sets 10, orig adds 1
        }

        [Test]
        public void PrefixNoReturn_1Arg()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePrefixHook<Target, int>(
                original: (t2, a) => t2.V1(a),
                prefix: a => a * 10);
            t.V1(5);
            Assert.That(actual: t.Result, expression: Is.EqualTo(50));
        }

        [Test]
        public void PrefixNoReturn_2Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePrefixHook<Target, int, int>(
                original: (t2, a, b) => t2.V2(a, b),
                prefix: (_, a, b) => (a * 10, b * 10));
            t.V2(a: 1, b: 2);
            Assert.That(actual: t.Result, expression: Is.EqualTo(30));
        }

        [Test]
        public void PrefixNoReturn_3Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePrefixHook<Target, int, int, int>(
                original: (t2, a, b, c) => t2.V3(a, b, c),
                prefix: (_, a, b, c) => (a * 10, b * 10, c * 10));
            t.V3(a: 1, b: 2, c: 3);
            Assert.That(actual: t.Result, expression: Is.EqualTo(60));
        }

        [Test]
        public void PrefixNoReturn_4Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePrefixHook<Target, int, int, int, int>(
                original: (t2, a, b, c, d) => t2.V4(a, b, c, d),
                prefix: (_, a, b, c, d) => (a * 10, b * 10, c * 10, d * 10));
            t.V4(a: 1, b: 2, c: 3, d: 4);
            Assert.That(actual: t.Result, expression: Is.EqualTo(100));
        }

        [Test]
        public void PrefixNoReturn_5Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePrefixHook<Target, int, int, int, int, int>(
                original: (t2, a, b, c, d, e) => t2.V5(a, b, c, d, e),
                prefix: (_, a, b, c, d, e) => (a * 10, b * 10, c * 10, d * 10, e * 10));
            t.V5(a: 1, b: 2, c: 3, d: 4, e: 5);
            Assert.That(actual: t.Result, expression: Is.EqualTo(150));
        }

        [Test]
        public void PrefixNoReturn_6Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePrefixHook<Target, int, int, int, int, int, int>(
                original: (t2, a, b, c, d, e, f) => t2.V6(a, b, c, d, e, f),
                prefix: (_, a, b, c, d, e, f) => (a * 10, b * 10, c * 10, d * 10, e * 10, f * 10));
            t.V6(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6);
            Assert.That(actual: t.Result, expression: Is.EqualTo(210));
        }

        [Test]
        public void PrefixNoReturn_7Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePrefixHook<Target, int, int, int, int, int, int, int>(
                original: (t2, a, b, c, d, e, f, g) => t2.V7(a, b, c, d, e, f, g),
                prefix: (_, a, b, c, d, e, f, g) => (a * 10, b * 10, c * 10, d * 10, e * 10, f * 10, g * 10));
            t.V7(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6, g: 7);
            Assert.That(actual: t.Result, expression: Is.EqualTo(280));
        }

        #endregion

        #region Prefix with return

        [Test]
        public void PrefixWithReturn_0Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePrefixHook<Target, int>(
                original: t2 => t2.R0(),
                prefix: self => { self.Result = 1; });
            int v = t.R0();
            Assert.That(actual: v, expression: Is.EqualTo(100));
            Assert.That(actual: t.Result, expression: Is.EqualTo(1)); // prefix ran
        }

        [Test]
        public void PrefixWithReturn_1Arg()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePrefixHook<Target, int, int>(
                original: (t2, a) => t2.R1(a),
                prefix: (_, a) => a * 10);
            int v = t.R1(5);
            Assert.That(actual: v, expression: Is.EqualTo(50));
        }

        [Test]
        public void PrefixWithReturn_2Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePrefixHook<Target, int, int, int>(
                original: (t2, a, b) => t2.R2(a, b),
                prefix: (_, a, b) => (a * 10, b * 10));
            int v = t.R2(a: 1, b: 2);
            Assert.That(actual: v, expression: Is.EqualTo(30));
        }

        [Test]
        public void PrefixWithReturn_3Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePrefixHook<Target, int, int, int, int>(
                original: (t2, a, b, c) => t2.R3(a, b, c),
                prefix: (_, a, b, c) => (a * 10, b * 10, c * 10));
            int v = t.R3(a: 1, b: 2, c: 3);
            Assert.That(actual: v, expression: Is.EqualTo(60));
        }

        [Test]
        public void PrefixWithReturn_4Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePrefixHook<Target, int, int, int, int, int>(
                original: (t2, a, b, c, d) => t2.R4(a, b, c, d),
                prefix: (_, a, b, c, d) => (a * 10, b * 10, c * 10, d * 10));
            int v = t.R4(a: 1, b: 2, c: 3, d: 4);
            Assert.That(actual: v, expression: Is.EqualTo(100));
        }

        [Test]
        public void PrefixWithReturn_5Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePrefixHook<Target, int, int, int, int, int, int>(
                original: (t2, a, b, c, d, e) => t2.R5(a, b, c, d, e),
                prefix: (_, a, b, c, d, e) => (a * 10, b * 10, c * 10, d * 10, e * 10));
            int v = t.R5(a: 1, b: 2, c: 3, d: 4, e: 5);
            Assert.That(actual: v, expression: Is.EqualTo(150));
        }

        [Test]
        public void PrefixWithReturn_6Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePrefixHook<Target, int, int, int, int, int, int, int>(
                original: (t2, a, b, c, d, e, f) => t2.R6(a, b, c, d, e, f),
                prefix: (_, a, b, c, d, e, f) => (a * 10, b * 10, c * 10, d * 10, e * 10, f * 10));
            int v = t.R6(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6);
            Assert.That(actual: v, expression: Is.EqualTo(210));
        }

        [Test]
        public void PrefixWithReturn_7Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePrefixHook<Target, int, int, int, int, int, int, int, int>(
                original: (t2, a, b, c, d, e, f, g) => t2.R7(a, b, c, d, e, f, g),
                prefix: (_, a, b, c, d, e, f, g) => (a * 10, b * 10, c * 10, d * 10, e * 10, f * 10, g * 10));
            int v = t.R7(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6, g: 7);
            Assert.That(actual: v, expression: Is.EqualTo(280));
        }

        #endregion

        #region Static prefix with no return

        [Test]
        public void StaticPrefixNoReturn_0Args()
        {
            using Hook hook = DetourHelper.CreateStaticPrefixHook<Target>(
                original: t => Target.Sv0(),
                prefix: () => { Target.SResult = 10; });
            Target.Sv0();
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(11)); // prefix sets 10, orig adds 1
        }

        [Test]
        public void StaticPrefixNoReturn_1Arg()
        {
            using Hook hook = DetourHelper.CreateStaticPrefixHook<Target, int>(
                original: a => Target.Sv1(a),
                prefix: a => a * 10);
            Target.Sv1(5);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(50));
        }

        [Test]
        public void StaticPrefixNoReturn_2Args()
        {
            using Hook hook = DetourHelper.CreateStaticPrefixHook<Target, int, int>(
                original: (a, b) => Target.Sv2(a, b),
                prefix: (a, b) => (a * 10, b * 10));
            Target.Sv2(a: 1, b: 2);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(30));
        }

        [Test]
        public void StaticPrefixNoReturn_3Args()
        {
            Expression<Action<Target, int, int, int>> original = (t, a, b, c) => Target.Sv3(a, b, c);
            using Hook hook = DetourHelper.CreateStaticPrefixHook(
                original: original,
                prefix: (a, b, c) => (a * 10, b * 10, c * 10));
            Target.Sv3(a: 1, b: 2, c: 3);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(60));
        }

        [Test]
        public void StaticPrefixNoReturn_4Args()
        {
            using Hook hook = DetourHelper.CreateStaticPrefixHook<Target, int, int, int, int>(
                original: (a, b, c, d) => Target.Sv4(a, b, c, d),
                prefix: (a, b, c, d) => (a * 10, b * 10, c * 10, d * 10));
            Target.Sv4(a: 1, b: 2, c: 3, d: 4);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(100));
        }

        [Test]
        public void StaticPrefixNoReturn_5Args()
        {
            using Hook hook = DetourHelper.CreateStaticPrefixHook<Target, int, int, int, int, int>(
                original: (a, b, c, d, e) => Target.Sv5(a, b, c, d, e),
                prefix: (a, b, c, d, e) => (a * 10, b * 10, c * 10, d * 10, e * 10));
            Target.Sv5(a: 1, b: 2, c: 3, d: 4, e: 5);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(150));
        }

        [Test]
        public void StaticPrefixNoReturn_6Args()
        {
            using Hook hook = DetourHelper.CreateStaticPrefixHook<Target, int, int, int, int, int, int>(
                original: (a, b, c, d, e, f) => Target.Sv6(a, b, c, d, e, f),
                prefix: (a, b, c, d, e, f) => (a * 10, b * 10, c * 10, d * 10, e * 10, f * 10));
            Target.Sv6(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(210));
        }

        [Test]
        public void StaticPrefixNoReturn_7Args()
        {
            using Hook hook = DetourHelper.CreateStaticPrefixHook<Target, int, int, int, int, int, int, int>(
                original: (a, b, c, d, e, f, g) => Target.Sv7(a, b, c, d, e, f, g),
                prefix: (a, b, c, d, e, f, g) => (a * 10, b * 10, c * 10, d * 10, e * 10, f * 10, g * 10));
            Target.Sv7(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6, g: 7);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(280));
        }

        #endregion

        #region Static prefix with return

        [Test]
        public void StaticPrefixWithReturn_0Args()
        {
            using Hook hook = DetourHelper.CreateStaticPrefixHook(
                original: () => Target.Sr0(),
                prefix: () => { Target.SResult = 1; });
            int v = Target.Sr0();
            Assert.That(actual: v, expression: Is.EqualTo(100));
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(1)); // prefix ran
        }

        [Test]
        public void StaticPrefixWithReturn_1Arg()
        {
            using Hook hook = DetourHelper.CreateStaticPrefixHook(
                original: a => Target.Sr1(a),
                prefix: (int a) => a * 10);
            int v = Target.Sr1(5);
            Assert.That(actual: v, expression: Is.EqualTo(50));
        }

        [Test]
        public void StaticPrefixWithReturn_2Args()
        {
            using Hook hook = DetourHelper.CreateStaticPrefixHook(
                original: (a, b) => Target.Sr2(a, b),
                prefix: (int a, int b) => (a * 10, b * 10));
            int v = Target.Sr2(a: 1, b: 2);
            Assert.That(actual: v, expression: Is.EqualTo(30));
        }

        [Test]
        public void StaticPrefixWithReturn_3Args()
        {
            using Hook hook = DetourHelper.CreateStaticPrefixHook(
                original: (a, b, c) => Target.Sr3(a, b, c),
                prefix: (int a, int b, int c) => (a * 10, b * 10, c * 10));
            int v = Target.Sr3(a: 1, b: 2, c: 3);
            Assert.That(actual: v, expression: Is.EqualTo(60));
        }

        [Test]
        public void StaticPrefixWithReturn_4Args()
        {
            using Hook hook = DetourHelper.CreateStaticPrefixHook(
                original: (a, b, c, d) => Target.Sr4(a, b, c, d),
                prefix: (int a, int b, int c, int d) => (a * 10, b * 10, c * 10, d * 10));
            int v = Target.Sr4(a: 1, b: 2, c: 3, d: 4);
            Assert.That(actual: v, expression: Is.EqualTo(100));
        }

        [Test]
        public void StaticPrefixWithReturn_5Args()
        {
            using Hook hook = DetourHelper.CreateStaticPrefixHook(
                original: (a, b, c, d, e) => Target.Sr5(a, b, c, d, e),
                prefix: (int a, int b, int c, int d, int e) => (a * 10, b * 10, c * 10, d * 10, e * 10));
            int v = Target.Sr5(a: 1, b: 2, c: 3, d: 4, e: 5);
            Assert.That(actual: v, expression: Is.EqualTo(150));
        }

        [Test]
        public void StaticPrefixWithReturn_6Args()
        {
            using Hook hook = DetourHelper.CreateStaticPrefixHook(
                original: (a, b, c, d, e, f) => Target.Sr6(a, b, c, d, e, f),
                prefix: (int a, int b, int c, int d, int e, int f) => (a * 10, b * 10, c * 10, d * 10, e * 10, f * 10));
            int v = Target.Sr6(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6);
            Assert.That(actual: v, expression: Is.EqualTo(210));
        }

        [Test]
        public void StaticPrefixWithReturn_7Args()
        {
            using Hook hook = DetourHelper.CreateStaticPrefixHook(
                original: (a, b, c, d, e, f, g) => Target.Sr7(a, b, c, d, e, f, g),
                prefix: (int a, int b, int c, int d, int e, int f, int g) =>
                    (a * 10, b * 10, c * 10, d * 10, e * 10, f * 10, g * 10));
            int v = Target.Sr7(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6, g: 7);
            Assert.That(actual: v, expression: Is.EqualTo(280));
        }

        #endregion

        #region Postfix with no return

        [Test]
        public void PostfixNoReturn_0Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePostfixHook<Target>(
                original: t2 => t2.V0(),
                postfix: self => { self.Result += 1000; });
            t.V0();
            Assert.That(actual: t.Result, expression: Is.EqualTo(1001)); // orig adds 1, postfix adds 1000
        }

        [Test]
        public void PostfixNoReturn_1Arg()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePostfixHook<Target, int>(
                original: (t2, a) => t2.V1(a),
                postfix: (self, _) => { self.Result += 1000; });
            t.V1(3);
            Assert.That(actual: t.Result, expression: Is.EqualTo(1003));
        }

        [Test]
        public void PostfixNoReturn_2Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePostfixHook<Target, int, int>(
                original: (t2, a, b) => t2.V2(a, b),
                postfix: (self, _, _) => { self.Result += 1000; });
            t.V2(a: 1, b: 2);
            Assert.That(actual: t.Result, expression: Is.EqualTo(1003));
        }

        [Test]
        public void PostfixNoReturn_3Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePostfixHook<Target, int, int, int>(
                original: (t2, a, b, c) => t2.V3(a, b, c),
                postfix: (self, _, _, _) => { self.Result += 1000; });
            t.V3(a: 1, b: 2, c: 3);
            Assert.That(actual: t.Result, expression: Is.EqualTo(1006));
        }

        [Test]
        public void PostfixNoReturn_4Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePostfixHook<Target, int, int, int, int>(
                original: (t2, a, b, c, d) => t2.V4(a, b, c, d),
                postfix: (self, _, _, _, _) => { self.Result += 1000; });
            t.V4(a: 1, b: 2, c: 3, d: 4);
            Assert.That(actual: t.Result, expression: Is.EqualTo(1010));
        }

        [Test]
        public void PostfixNoReturn_5Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePostfixHook<Target, int, int, int, int, int>(
                original: (t2, a, b, c, d, e) => t2.V5(a, b, c, d, e),
                postfix: (self, _, _, _, _, _) => { self.Result += 1000; });
            t.V5(a: 1, b: 2, c: 3, d: 4, e: 5);
            Assert.That(actual: t.Result, expression: Is.EqualTo(1015));
        }

        [Test]
        public void PostfixNoReturn_6Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePostfixHook<Target, int, int, int, int, int, int>(
                original: (t2, a, b, c, d, e, f) => t2.V6(a, b, c, d, e, f),
                postfix: (self, _, _, _, _, _, _) => { self.Result += 1000; });
            t.V6(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6);
            Assert.That(actual: t.Result, expression: Is.EqualTo(1021));
        }

        [Test]
        public void PostfixNoReturn_7Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePostfixHook<Target, int, int, int, int, int, int, int>(
                original: (t2, a, b, c, d, e, f, g) => t2.V7(a, b, c, d, e, f, g),
                postfix: (self, _, _, _, _, _, _, _) => { self.Result += 1000; });
            t.V7(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6, g: 7);
            Assert.That(actual: t.Result, expression: Is.EqualTo(1028));
        }

        #endregion

        #region Postfix with return

        [Test]
        public void PostfixWithReturn_0Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePostfixHook<Target, int>(
                original: t2 => t2.R0(),
                postfix: (_, value) => value * 2);
            Assert.That(actual: t.R0(), expression: Is.EqualTo(200));
        }

        [Test]
        public void PostfixWithReturn_1Arg()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePostfixHook<Target, int, int>(
                original: (t2, a) => t2.R1(a),
                postfix: (_, _, value) => value * 2);
            Assert.That(actual: t.R1(7), expression: Is.EqualTo(14));
        }

        [Test]
        public void PostfixWithReturn_2Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePostfixHook<Target, int, int, int>(
                original: (t2, a, b) => t2.R2(a, b),
                postfix: (_, _, _, value) => value * 2);
            Assert.That(actual: t.R2(a: 3, b: 4), expression: Is.EqualTo(14));
        }

        [Test]
        public void PostfixWithReturn_3Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePostfixHook<Target, int, int, int, int>(
                original: (t2, a, b, c) => t2.R3(a, b, c),
                postfix: (_, _, _, _, value) => value * 2);
            Assert.That(actual: t.R3(a: 1, b: 2, c: 3), expression: Is.EqualTo(12));
        }

        [Test]
        public void PostfixWithReturn_4Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePostfixHook<Target, int, int, int, int, int>(
                original: (t2, a, b, c, d) => t2.R4(a, b, c, d),
                postfix: (_, _, _, _, _, value) => value * 2);
            Assert.That(actual: t.R4(a: 1, b: 2, c: 3, d: 4), expression: Is.EqualTo(20));
        }

        [Test]
        public void PostfixWithReturn_5Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePostfixHook<Target, int, int, int, int, int, int>(
                original: (t2, a, b, c, d, e) => t2.R5(a, b, c, d, e),
                postfix: (_, _, _, _, _, _, value) => value * 2);
            Assert.That(actual: t.R5(a: 1, b: 2, c: 3, d: 4, e: 5), expression: Is.EqualTo(30));
        }

        [Test]
        public void PostfixWithReturn_6Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePostfixHook<Target, int, int, int, int, int, int, int>(
                original: (t2, a, b, c, d, e, f) => t2.R6(a, b, c, d, e, f),
                postfix: (_, _, _, _, _, _, _, value) => value * 2);
            Assert.That(actual: t.R6(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6), expression: Is.EqualTo(42));
        }

        [Test]
        public void PostfixWithReturn_7Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.CreatePostfixHook<Target, int, int, int, int, int, int, int, int>(
                original: (t2, a, b, c, d, e, f, g) => t2.R7(a, b, c, d, e, f, g),
                postfix: (_, _, _, _, _, _, _, _, value) => value * 2);
            Assert.That(actual: t.R7(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6, g: 7), expression: Is.EqualTo(56));
        }

        #endregion

        #region Static Postfix with no return

        [Test]
        public void StaticPostfixNoReturn_0Args()
        {
            using Hook hook = DetourHelper.CreateStaticPostfixHook<Target>(
                original: t => Target.Sv0(),
                postfix: () => { Target.SResult += 1000; });
            Target.Sv0();
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(1001));
        }

        [Test]
        public void StaticPostfixNoReturn_1Arg()
        {
            using Hook hook = DetourHelper.CreateStaticPostfixHook(
                original: (int a) => Target.Sv1(a),
                postfix: (int _) => { Target.SResult += 1000; });
            Target.Sv1(5);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(1005));
        }

        [Test]
        public void StaticPostfixNoReturn_2Args()
        {
            using Hook hook = DetourHelper.CreateStaticPostfixHook(
                original: (int a, int b) => Target.Sv2(a, b),
                postfix: (int _, int _) => { Target.SResult += 1000; });
            Target.Sv2(a: 1, b: 2);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(1003));
        }

        [Test]
        public void StaticPostfixNoReturn_3Args()
        {
            using Hook hook = DetourHelper.CreateStaticPostfixHook(
                original: (int a, int b, int c) => Target.Sv3(a, b, c),
                postfix: (int _, int _, int _) => { Target.SResult += 1000; });
            Target.Sv3(a: 1, b: 2, c: 3);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(1006));
        }

        [Test]
        public void StaticPostfixNoReturn_4Args()
        {
            using Hook hook = DetourHelper.CreateStaticPostfixHook(
                original: (int a, int b, int c, int d) => Target.Sv4(a, b, c, d),
                postfix: (int _, int _, int _, int _) => { Target.SResult += 1000; });
            Target.Sv4(a: 1, b: 2, c: 3, d: 4);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(1010));
        }

        [Test]
        public void StaticPostfixNoReturn_5Args()
        {
            using Hook hook = DetourHelper.CreateStaticPostfixHook(
                original: (int a, int b, int c, int d, int e) => Target.Sv5(a, b, c, d, e),
                postfix: (int _, int _, int _, int _, int _) => { Target.SResult += 1000; });
            Target.Sv5(a: 1, b: 2, c: 3, d: 4, e: 5);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(1015));
        }

        [Test]
        public void StaticPostfixNoReturn_6Args()
        {
            using Hook hook = DetourHelper.CreateStaticPostfixHook(
                original: (int a, int b, int c, int d, int e, int f) => Target.Sv6(a, b, c, d, e, f),
                postfix: (int _, int _, int _, int _, int _, int _) => { Target.SResult += 1000; });
            Target.Sv6(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(1021));
        }

        [Test]
        public void StaticPostfixNoReturn_7Args()
        {
            using Hook hook = DetourHelper.CreateStaticPostfixHook(
                original: (int a, int b, int c, int d, int e, int f, int g) => Target.Sv7(a, b, c, d, e, f, g),
                postfix: (int _, int _, int _, int _, int _, int _, int _) => { Target.SResult += 1000; });
            Target.Sv7(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6, g: 7);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(1028));
        }

        #endregion

        #region Static Postfix with return

        [Test]
        public void StaticPostfixWithReturn_0Args()
        {
            using Hook hook = DetourHelper.CreateStaticPostfixHook(
                original: () => Target.Sr0(),
                postfix: value => value * 3);
            Assert.That(actual: Target.Sr0(), expression: Is.EqualTo(300));
        }

        [Test]
        public void StaticPostfixWithReturn_1Arg()
        {
            using Hook hook = DetourHelper.CreateStaticPostfixHook(
                original: a => Target.Sr1(a),
                postfix: (int _, int value) => value * 3);
            Assert.That(actual: Target.Sr1(5), expression: Is.EqualTo(15));
        }

        [Test]
        public void StaticPostfixWithReturn_2Args()
        {
            using Hook hook = DetourHelper.CreateStaticPostfixHook(
                original: (a, b) => Target.Sr2(a, b),
                postfix: (int _, int _, int value) => value * 3);
            Assert.That(actual: Target.Sr2(a: 3, b: 4), expression: Is.EqualTo(21));
        }

        [Test]
        public void StaticPostfixWithReturn_3Args()
        {
            using Hook hook = DetourHelper.CreateStaticPostfixHook(
                original: (a, b, c) => Target.Sr3(a, b, c),
                postfix: (int _, int _, int _, int value) => value * 3);
            Assert.That(actual: Target.Sr3(a: 1, b: 2, c: 3), expression: Is.EqualTo(18));
        }

        [Test]
        public void StaticPostfixWithReturn_4Args()
        {
            using Hook hook = DetourHelper.CreateStaticPostfixHook(
                original: (a, b, c, d) => Target.Sr4(a, b, c, d),
                postfix: (int _, int _, int _, int _, int value) => value * 3);
            Assert.That(actual: Target.Sr4(a: 1, b: 2, c: 3, d: 4), expression: Is.EqualTo(30));
        }

        [Test]
        public void StaticPostfixWithReturn_5Args()
        {
            using Hook hook = DetourHelper.CreateStaticPostfixHook(
                original: (a, b, c, d, e) => Target.Sr5(a, b, c, d, e),
                postfix: (int _, int _, int _, int _, int _, int value) => value * 3);
            Assert.That(actual: Target.Sr5(a: 1, b: 2, c: 3, d: 4, e: 5), expression: Is.EqualTo(45));
        }

        [Test]
        public void StaticPostfixWithReturn_6Args()
        {
            using Hook hook = DetourHelper.CreateStaticPostfixHook(
                original: (a, b, c, d, e, f) => Target.Sr6(a, b, c, d, e, f),
                postfix: (int _, int _, int _, int _, int _, int _, int value) => value * 3);
            Assert.That(actual: Target.Sr6(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6), expression: Is.EqualTo(63));
        }

        [Test]
        public void StaticPostfixWithReturn_7Args()
        {
            using Hook hook = DetourHelper.CreateStaticPostfixHook(
                original: (a, b, c, d, e, f, g) => Target.Sr7(a, b, c, d, e, f, g),
                postfix: (int _, int _, int _, int _, int _, int _, int _, int value) => value * 3);
            Assert.That(actual: Target.Sr7(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6, g: 7), expression: Is.EqualTo(84));
        }

        #endregion

        #region Skip with no return

        [Test]
        public void Skip_0Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Skip<Target>(t2 => t2.V0());
            t.V0();
            Assert.That(actual: t.Result, expression: Is.Zero); // orig not called
        }

        [Test]
        public void Skip_1Arg()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Skip<Target, int>((t2, a) => t2.V1(a));
            t.V1(99);
            Assert.That(actual: t.Result, expression: Is.Zero);
        }

        [Test]
        public void Skip_2Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Skip<Target, int, int>((t2, a, b) => t2.V2(a, b));
            t.V2(a: 1, b: 2);
            Assert.That(actual: t.Result, expression: Is.Zero);
        }

        [Test]
        public void Skip_3Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Skip<Target, int, int, int>((t2, a, b, c) => t2.V3(a, b, c));
            t.V3(a: 1, b: 2, c: 3);
            Assert.That(actual: t.Result, expression: Is.Zero);
        }

        [Test]
        public void Skip_4Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Skip<Target, int, int, int, int>((t2, a, b, c, d) => t2.V4(a, b, c, d));
            t.V4(a: 1, b: 2, c: 3, d: 4);
            Assert.That(actual: t.Result, expression: Is.Zero);
        }

        [Test]
        public void Skip_5Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Skip<Target, int, int, int, int, int>(
                (t2, a, b, c, d, e) => t2.V5(a, b, c, d, e));
            t.V5(a: 1, b: 2, c: 3, d: 4, e: 5);
            Assert.That(actual: t.Result, expression: Is.Zero);
        }

        [Test]
        public void Skip_6Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Skip<Target, int, int, int, int, int, int>(
                (t2, a, b, c, d, e, f) => t2.V6(a, b, c, d, e, f));
            t.V6(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6);
            Assert.That(actual: t.Result, expression: Is.Zero);
        }

        [Test]
        public void Skip_7Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Skip<Target, int, int, int, int, int, int, int>(
                (t2, a, b, c, d, e, f, g) => t2.V7(a, b, c, d, e, f, g));
            t.V7(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6, g: 7);
            Assert.That(actual: t.Result, expression: Is.Zero);
        }

        #endregion

        #region Replace with no return

        [Test]
        public void Replace_0Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Replace<Target>(
                original: t2 => t2.V0(),
                replacement: self => { self.Result = 999; });
            t.V0();
            Assert.That(actual: t.Result, expression: Is.EqualTo(999));
        }

        [Test]
        public void Replace_1Arg()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Replace<Target, int>(
                original: (t2, a) => t2.V1(a),
                replacement: (self, a) => { self.Result = a * 100; });
            t.V1(5);
            Assert.That(actual: t.Result, expression: Is.EqualTo(500));
        }

        [Test]
        public void Replace_2Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Replace<Target, int, int>(
                original: (t2, a, b) => t2.V2(a, b),
                replacement: (self, a, b) => { self.Result = (a + b) * 100; });
            t.V2(a: 1, b: 2);
            Assert.That(actual: t.Result, expression: Is.EqualTo(300));
        }

        [Test]
        public void Replace_3Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Replace<Target, int, int, int>(
                original: (t2, a, b, c) => t2.V3(a, b, c),
                replacement: (self, a, b, c) => { self.Result = (a + b + c) * 100; });
            t.V3(a: 1, b: 2, c: 3);
            Assert.That(actual: t.Result, expression: Is.EqualTo(600));
        }

        [Test]
        public void Replace_4Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Replace<Target, int, int, int, int>(
                original: (t2, a, b, c, d) => t2.V4(a, b, c, d),
                replacement: (self, a, b, c, d) => { self.Result = (a + b + c + d) * 100; });
            t.V4(a: 1, b: 2, c: 3, d: 4);
            Assert.That(actual: t.Result, expression: Is.EqualTo(1000));
        }

        [Test]
        public void Replace_5Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Replace<Target, int, int, int, int, int>(
                original: (t2, a, b, c, d, e) => t2.V5(a, b, c, d, e),
                replacement: (self, a, b, c, d, e) => { self.Result = (a + b + c + d + e) * 100; });
            t.V5(a: 1, b: 2, c: 3, d: 4, e: 5);
            Assert.That(actual: t.Result, expression: Is.EqualTo(1500));
        }

        [Test]
        public void Replace_6Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Replace<Target, int, int, int, int, int, int>(
                original: (t2, a, b, c, d, e, f) => t2.V6(a, b, c, d, e, f),
                replacement: (self, a, b, c, d, e, f) => { self.Result = (a + b + c + d + e + f) * 100; });
            t.V6(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6);
            Assert.That(actual: t.Result, expression: Is.EqualTo(2100));
        }

        [Test]
        public void Replace_7Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Replace<Target, int, int, int, int, int, int, int>(
                original: (t2, a, b, c, d, e, f, g) => t2.V7(a, b, c, d, e, f, g),
                replacement: (self, a, b, c, d, e, f, g) => { self.Result = (a + b + c + d + e + f + g) * 100; });
            t.V7(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6, g: 7);
            Assert.That(actual: t.Result, expression: Is.EqualTo(2800));
        }

        #endregion

        #region Static skip with no return

        [Test]
        public void StaticSkip_0Args()
        {
            using Hook hook = DetourHelper.StaticSkip(() => Target.Sv0());
            Target.Sv0();
            Assert.That(actual: Target.SResult, expression: Is.Zero);
        }

        [Test]
        public void StaticSkip_1Arg()
        {
            using Hook hook = DetourHelper.StaticSkip<int>(a => Target.Sv1(a));
            Target.Sv1(99);
            Assert.That(actual: Target.SResult, expression: Is.Zero);
        }

        [Test]
        public void StaticSkip_2Args()
        {
            using Hook hook = DetourHelper.StaticSkip<int, int>((a, b) => Target.Sv2(a, b));
            Target.Sv2(a: 1, b: 2);
            Assert.That(actual: Target.SResult, expression: Is.Zero);
        }

        [Test]
        public void StaticSkip_3Args()
        {
            using Hook hook = DetourHelper.StaticSkip<int, int, int>((a, b, c) => Target.Sv3(a, b, c));
            Target.Sv3(a: 1, b: 2, c: 3);
            Assert.That(actual: Target.SResult, expression: Is.Zero);
        }

        [Test]
        public void StaticSkip_4Args()
        {
            using Hook hook = DetourHelper.StaticSkip<int, int, int, int>((a, b, c, d) => Target.Sv4(a, b, c, d));
            Target.Sv4(a: 1, b: 2, c: 3, d: 4);
            Assert.That(actual: Target.SResult, expression: Is.Zero);
        }

        [Test]
        public void StaticSkip_5Args()
        {
            using Hook hook = DetourHelper.StaticSkip<int, int, int, int, int>(
                (a, b, c, d, e) => Target.Sv5(a, b, c, d, e));
            Target.Sv5(a: 1, b: 2, c: 3, d: 4, e: 5);
            Assert.That(actual: Target.SResult, expression: Is.Zero);
        }

        [Test]
        public void StaticSkip_6Args()
        {
            using Hook hook = DetourHelper.StaticSkip<int, int, int, int, int, int>(
                (a, b, c, d, e, f) => Target.Sv6(a, b, c, d, e, f));
            Target.Sv6(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6);
            Assert.That(actual: Target.SResult, expression: Is.Zero);
        }

        [Test]
        public void StaticSkip_7Args()
        {
            using Hook hook = DetourHelper.StaticSkip<int, int, int, int, int, int, int>(
                (a, b, c, d, e, f, g) => Target.Sv7(a, b, c, d, e, f, g));
            Target.Sv7(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6, g: 7);
            Assert.That(actual: Target.SResult, expression: Is.Zero);
        }

        #endregion

        #region Replace with return

        [Test]
        public void ReplaceWithReturn_0Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Replace<Target, int>(original: t2 => t2.R0(), replacement: self => 999);
            Assert.That(actual: t.R0(), expression: Is.EqualTo(999));
        }

        [Test]
        public void ReplaceWithReturn_1Arg()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Replace<Target, int, int>(
                original: (t2, a) => t2.R1(a),
                replacement: (self, a) => a * 100);
            Assert.That(actual: t.R1(5), expression: Is.EqualTo(500));
        }

        [Test]
        public void ReplaceWithReturn_2Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Replace<Target, int, int, int>(
                original: (t2, a, b) => t2.R2(a, b),
                replacement: (self, a, b) => (a + b) * 100);
            Assert.That(actual: t.R2(a: 1, b: 2), expression: Is.EqualTo(300));
        }

        [Test]
        public void ReplaceWithReturn_3Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Replace<Target, int, int, int, int>(
                original: (t2, a, b, c) => t2.R3(a, b, c),
                replacement: (self, a, b, c) => (a + b + c) * 100);
            Assert.That(actual: t.R3(a: 1, b: 2, c: 3), expression: Is.EqualTo(600));
        }

        [Test]
        public void ReplaceWithReturn_4Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Replace<Target, int, int, int, int, int>(
                original: (t2, a, b, c, d) => t2.R4(a, b, c, d),
                replacement: (self, a, b, c, d) => (a + b + c + d) * 100);
            Assert.That(actual: t.R4(a: 1, b: 2, c: 3, d: 4), expression: Is.EqualTo(1000));
        }

        [Test]
        public void ReplaceWithReturn_5Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Replace<Target, int, int, int, int, int, int>(
                original: (t2, a, b, c, d, e) => t2.R5(a, b, c, d, e),
                replacement: (self, a, b, c, d, e) => (a + b + c + d + e) * 100);
            Assert.That(actual: t.R5(a: 1, b: 2, c: 3, d: 4, e: 5), expression: Is.EqualTo(1500));
        }

        [Test]
        public void ReplaceWithReturn_6Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Replace<Target, int, int, int, int, int, int, int>(
                original: (t2, a, b, c, d, e, f) => t2.R6(a, b, c, d, e, f),
                replacement: (self, a, b, c, d, e, f) => (a + b + c + d + e + f) * 100);
            Assert.That(actual: t.R6(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6), expression: Is.EqualTo(2100));
        }

        [Test]
        public void ReplaceWithReturn_7Args()
        {
            var t = new Target();
            using Hook hook = DetourHelper.Replace<Target, int, int, int, int, int, int, int, int>(
                original: (t2, a, b, c, d, e, f, g) => t2.R7(a, b, c, d, e, f, g),
                replacement: (_, a, b, c, d, e, f, g) => (a + b + c + d + e + f + g) * 100);
            Assert.That(actual: t.R7(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6, g: 7), expression: Is.EqualTo(2800));
        }

        #endregion

        #region Static replace with no return

        [Test]
        public void StaticReplace_0Args()
        {
            using Hook hook = DetourHelper.StaticReplace<Target>(
                original: t => Target.Sv0(),
                replacement: () => { Target.SResult = 999; });
            Target.Sv0();
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(999));
        }

        [Test]
        public void StaticReplace_1Arg()
        {
            using Hook hook = DetourHelper.StaticReplace(
                original: a => Target.Sv1(a),
                replacement: (int a) => { Target.SResult = a * 100; });
            Target.Sv1(5);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(500));
        }

        [Test]
        public void StaticReplace_2Args()
        {
            using Hook hook = DetourHelper.StaticReplace(
                original: (a, b) => Target.Sv2(a, b),
                replacement: (int a, int b) => { Target.SResult = (a + b) * 100; });
            Target.Sv2(a: 1, b: 2);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(300));
        }

        [Test]
        public void StaticReplace_3Args()
        {
            using Hook hook = DetourHelper.StaticReplace(
                original: (a, b, c) => Target.Sv3(a, b, c),
                replacement: (int a, int b, int c) => { Target.SResult = (a + b + c) * 100; });
            Target.Sv3(a: 1, b: 2, c: 3);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(600));
        }

        [Test]
        public void StaticReplace_4Args()
        {
            using Hook hook = DetourHelper.StaticReplace(
                original: (a, b, c, d) => Target.Sv4(a, b, c, d),
                replacement: (int a, int b, int c, int d) => { Target.SResult = (a + b + c + d) * 100; });
            Target.Sv4(a: 1, b: 2, c: 3, d: 4);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(1000));
        }

        [Test]
        public void StaticReplace_5Args()
        {
            using Hook hook = DetourHelper.StaticReplace(
                original: (a, b, c, d, e) => Target.Sv5(a, b, c, d, e),
                replacement: (int a, int b, int c, int d, int e) => { Target.SResult = (a + b + c + d + e) * 100; });
            Target.Sv5(a: 1, b: 2, c: 3, d: 4, e: 5);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(1500));
        }

        [Test]
        public void StaticReplace_6Args()
        {
            using Hook hook = DetourHelper.StaticReplace(
                original: (a, b, c, d, e, f) => Target.Sv6(a, b, c, d, e, f),
                replacement: (int a, int b, int c, int d, int e, int f) =>
                {
                    Target.SResult = (a + b + c + d + e + f) * 100;
                });
            Target.Sv6(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(2100));
        }

        [Test]
        public void StaticReplace_7Args()
        {
            using Hook hook = DetourHelper.StaticReplace(
                original: (a, b, c, d, e, f, g) => Target.Sv7(a, b, c, d, e, f, g),
                replacement: (int a, int b, int c, int d, int e, int f, int g) =>
                {
                    Target.SResult = (a + b + c + d + e + f + g) * 100;
                });
            Target.Sv7(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6, g: 7);
            Assert.That(actual: Target.SResult, expression: Is.EqualTo(2800));
        }

        #endregion

        #region Static replace with return

        [Test]
        public void StaticReplaceWithReturn_0Args()
        {
            using Hook hook = DetourHelper.StaticReplace(original: () => Target.Sr0(), replacement: () => 999);
            Assert.That(actual: Target.Sr0(), expression: Is.EqualTo(999));
        }

        [Test]
        public void StaticReplaceWithReturn_1Arg()
        {
            using Hook hook = DetourHelper.StaticReplace(original: a => Target.Sr1(a), replacement: (int a) => a * 100);
            Assert.That(actual: Target.Sr1(5), expression: Is.EqualTo(500));
        }

        [Test]
        public void StaticReplaceWithReturn_2Args()
        {
            using Hook hook = DetourHelper.StaticReplace(
                original: (a, b) => Target.Sr2(a, b),
                replacement: (int a, int b) => (a + b) * 100);
            Assert.That(actual: Target.Sr2(a: 1, b: 2), expression: Is.EqualTo(300));
        }

        [Test]
        public void StaticReplaceWithReturn_3Args()
        {
            using Hook hook = DetourHelper.StaticReplace(
                original: (a, b, c) => Target.Sr3(a, b, c),
                replacement: (int a, int b, int c) => (a + b + c) * 100);
            Assert.That(actual: Target.Sr3(a: 1, b: 2, c: 3), expression: Is.EqualTo(600));
        }

        [Test]
        public void StaticReplaceWithReturn_4Args()
        {
            using Hook hook = DetourHelper.StaticReplace(
                original: (a, b, c, d) => Target.Sr4(a, b, c, d),
                replacement: (int a, int b, int c, int d) => (a + b + c + d) * 100);
            Assert.That(actual: Target.Sr4(a: 1, b: 2, c: 3, d: 4), expression: Is.EqualTo(1000));
        }

        [Test]
        public void StaticReplaceWithReturn_5Args()
        {
            using Hook hook = DetourHelper.StaticReplace(
                original: (a, b, c, d, e) => Target.Sr5(a, b, c, d, e),
                replacement: (int a, int b, int c, int d, int e) => (a + b + c + d + e) * 100);
            Assert.That(actual: Target.Sr5(a: 1, b: 2, c: 3, d: 4, e: 5), expression: Is.EqualTo(1500));
        }

        [Test]
        public void StaticReplaceWithReturn_6Args()
        {
            using Hook hook = DetourHelper.StaticReplace(
                original: (a, b, c, d, e, f) => Target.Sr6(a, b, c, d, e, f),
                replacement: (int a, int b, int c, int d, int e, int f) => (a + b + c + d + e + f) * 100);
            Assert.That(actual: Target.Sr6(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6), expression: Is.EqualTo(2100));
        }

        [Test]
        public void StaticReplaceWithReturn_7Args()
        {
            using Hook hook = DetourHelper.StaticReplace(
                original: (a, b, c, d, e, f, g) => Target.Sr7(a, b, c, d, e, f, g),
                replacement: (int a, int b, int c, int d, int e, int f, int g) => (a + b + c + d + e + f + g) * 100);
            Assert.That(actual: Target.Sr7(a: 1, b: 2, c: 3, d: 4, e: 5, f: 6, g: 7), expression: Is.EqualTo(2800));
        }

        #endregion

        #region Mock class

        private class Target
        {
            public int Result;

            // Void instance methods (0-7 args)
            public void V0()
            {
                Result += 1;
            }

            public void V1(int a)
            {
                Result = a;
            }

            public void V2(int a, int b)
            {
                Result = a + b;
            }

            public void V3(int a, int b, int c)
            {
                Result = a + b + c;
            }

            public void V4(int a, int b, int c, int d)
            {
                Result = a + b + c + d;
            }

            public void V5(int a, int b, int c, int d, int e)
            {
                Result = a + b + c + d + e;
            }

            public void V6(int a, int b, int c, int d, int e, int f)
            {
                Result = a + b + c + d + e + f;
            }

            public void V7(int a, int b, int c, int d, int e, int f, int g)
            {
                Result = a + b + c + d + e + f + g;
            }

            // Returning instance methods (0-7 args)
            public int R0()
            {
                return 100;
            }

            public int R1(int a)
            {
                return a;
            }

            public int R2(int a, int b)
            {
                return a + b;
            }

            public int R3(int a, int b, int c)
            {
                return a + b + c;
            }

            public int R4(int a, int b, int c, int d)
            {
                return a + b + c + d;
            }

            public int R5(int a, int b, int c, int d, int e)
            {
                return a + b + c + d + e;
            }

            public int R6(int a, int b, int c, int d, int e, int f)
            {
                return a + b + c + d + e + f;
            }

            public int R7(int a, int b, int c, int d, int e, int f, int g)
            {
                return a + b + c + d + e + f + g;
            }

            // Static state
            public static int SResult;

            // Static void methods (0-7 args)
            public static void Sv0()
            {
                SResult += 1;
            }

            public static void Sv1(int a)
            {
                SResult = a;
            }

            public static void Sv2(int a, int b)
            {
                SResult = a + b;
            }

            public static void Sv3(int a, int b, int c)
            {
                SResult = a + b + c;
            }

            public static void Sv4(int a, int b, int c, int d)
            {
                SResult = a + b + c + d;
            }

            public static void Sv5(int a, int b, int c, int d, int e)
            {
                SResult = a + b + c + d + e;
            }

            public static void Sv6(int a, int b, int c, int d, int e, int f)
            {
                SResult = a + b + c + d + e + f;
            }

            public static void Sv7(int a, int b, int c, int d, int e, int f, int g)
            {
                SResult = a + b + c + d + e + f + g;
            }

            // Static returning methods (0-7 args)
            public static int Sr0()
            {
                return 100;
            }

            public static int Sr1(int a)
            {
                return a;
            }

            public static int Sr2(int a, int b)
            {
                return a + b;
            }

            public static int Sr3(int a, int b, int c)
            {
                return a + b + c;
            }

            public static int Sr4(int a, int b, int c, int d)
            {
                return a + b + c + d;
            }

            public static int Sr5(int a, int b, int c, int d, int e)
            {
                return a + b + c + d + e;
            }

            public static int Sr6(int a, int b, int c, int d, int e, int f)
            {
                return a + b + c + d + e + f;
            }

            public static int Sr7(int a, int b, int c, int d, int e, int f, int g)
            {
                return a + b + c + d + e + f + g;
            }
        }

        #endregion
    }
}
