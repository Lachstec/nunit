﻿// Copyright (c) Charlie Poole, Rob Prouse and Contributors. MIT License - see LICENSE.txt

namespace NUnit.Framework.Constraints
{
    [TestFixture]
    public class AssignableFromConstraintTests : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            TheConstraint = new AssignableFromConstraint(typeof(D1));
            ExpectedDescription = $"assignable from <{typeof(D1)}>";
            StringRepresentation = $"<assignablefrom {typeof(D1)}>";
        }

        private static object[] SuccessData = new object[] { new D1(), new B() };
        private static object[] FailureData = new object[] { 
            new TestCaseData( new D2(), "<" + typeof(D2).FullName + ">" ) };

        private class B { }

        private class D1 : B { }

        private class D2 : D1 { }
    }
}