/**
 *   Copyright (c) Rich Hickey. All rights reserved.
 *   The use and distribution terms for this software are covered by the
 *   Eclipse Public License 1.0 (http://opensource.org/licenses/eclipse-1.0.php)
 *   which can be found in the file epl-v10.html at the root of this distribution.
 *   By using this software in any fashion, you are agreeing to be bound by
 * 	 the terms of this license.
 *   You must not remove this notice, or any other, from this software.
 **/

/**
 *   Author: David Miller
 **/

using System;

using NUnit.Framework;
using static NExpect.Expectations;
using clojure.lang;
using NExpect;

namespace Clojure.Tests.LibTests
{
    [TestFixture]
    public class LazilyPersistentVectorTests
    {
        #region C-tor tests

        [Test] 
        public void CreateOwningOnNoParamsReturnsEmptyVector()
        {
            IPersistentVector v = LazilyPersistentVector.createOwning();
            Expect(v.count()).To.Equal(0);
        }

        [Test]
        public void CreatingOwningOnParamsReturnsVector()
        {
            IPersistentVector v = LazilyPersistentVector.createOwning(1, 2, 3);
            Expect(v.count()).To.Equal(3);
            Expect(v.nth(0)).To.Equal(1);
            Expect(v.nth(1)).To.Equal(2);
            Expect(v.nth(2)).To.Equal(3);
        }

        [Test]
        public void CreateOnEmptySeqReturnsEmptyVector()
        {
            IPersistentVector v = LazilyPersistentVector.create(new object[] {});
            Expect(v.count()).To.Equal(0);
        }

        [Test]
        public void CreateOnNonEmptyCollectionReturnsVector()
        {
            IPersistentVector v = LazilyPersistentVector.createOwning(new object[] {1, 2, 3});
            Expect(v.count()).To.Equal(3);
            Expect(v.nth(0)).To.Equal(1);
            Expect(v.nth(1)).To.Equal(2);
            Expect(v.nth(2)).To.Equal(3);
        }


        #endregion

        #region IPersistentVector tests

        [Test]
        public void NthInRangeWorks()
        {
            IPersistentVector v = LazilyPersistentVector.createOwning(1, 2, 3);
 
            Expect(v.count()).To.Equal(3);
            Expect(v.nth(0)).To.Equal(1);
            Expect(v.nth(1)).To.Equal(2);
            Expect(v.nth(2)).To.Equal(3);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NthOutOfRangeLowFails()
        {
            IPersistentVector v = LazilyPersistentVector.createOwning(1, 2, 3);
            
            Expect(v.nth(-4)).To.Equal(1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NthOutOfRangeHighFails()
        {
            IPersistentVector v = LazilyPersistentVector.createOwning(1, 2, 3);

            Expect(v.nth(4)).To.Equal(1);
        }

        [Test]
        public void AssocnInRangeModifies()
        {
            IPersistentVector v = LazilyPersistentVector.createOwning(1, 2, 3);
            IPersistentVector v2 = v.assocN(1, 4);

            Expect(v.count()).To.Equal(3);
            Expect(v.nth(0)).To.Equal(1);
            Expect(v.nth(1)).To.Equal(2);
            Expect(v.nth(2)).To.Equal(3);

            Expect(v2.count()).To.Equal(3);
            Expect(v2.nth(0)).To.Equal(1);
            Expect(v2.nth(1)).To.Equal(4);
            Expect(v2.nth(2)).To.Equal(3);
        }

        [Test]
        public void AssocnAtEndModifies()
        {
            IPersistentVector v = LazilyPersistentVector.createOwning(1, 2, 3);
            IPersistentVector v2 = v.assocN(3, 4);

            Expect(v.count()).To.Equal(3);
            Expect(v.nth(0)).To.Equal(1);
            Expect(v.nth(1)).To.Equal(2);
            Expect(v.nth(2)).To.Equal(3);

            Expect(v2.count()).To.Equal(4);
            Expect(v2.nth(0)).To.Equal(1);
            Expect(v2.nth(1)).To.Equal(2);
            Expect(v2.nth(2)).To.Equal(3);
            Expect(v2.nth(3)).To.Equal(4);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AssocNOutOfRangeLowFails()
        {
            IPersistentVector v = LazilyPersistentVector.createOwning(1, 2, 3);
            v.assocN(-4, 4);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AssocNOutOfRangeHighFails()
        {
            IPersistentVector v = LazilyPersistentVector.createOwning(1, 2, 3);
            v.assocN(4, 4);
        }


        [Test]
        public void ConsAddsAtEnd()
        {
            IPersistentVector v = LazilyPersistentVector.createOwning(1, 2, 3);
            IPersistentVector v2 = v.cons(4);

            Expect(v.count()).To.Equal(3);
            Expect(v.nth(0)).To.Equal(1);
            Expect(v.nth(1)).To.Equal(2);
            Expect(v.nth(2)).To.Equal(3);

            Expect(v2.count()).To.Equal(4);
            Expect(v2.nth(0)).To.Equal(1);
            Expect(v2.nth(1)).To.Equal(2);
            Expect(v2.nth(2)).To.Equal(3);
            Expect(v2.nth(3)).To.Equal(4);
        }

        [Test]
        public void LengthWorks()
        {
            IPersistentVector v = LazilyPersistentVector.createOwning(1, 2, 3);

            Expect(v.length()).To.Equal(3);
        }

        #endregion

        #region IPersistentStack tests

        [Test]
        public void PopOnNonEmptyWorks()
        {
            IPersistentVector v = LazilyPersistentVector.createOwning(1, 2, 3);
            IPersistentVector v2 = (IPersistentVector)((IPersistentStack)v).pop();

            Expect(v.count()).To.Equal(3);
            Expect(v.nth(0)).To.Equal(1);
            Expect(v.nth(1)).To.Equal(2);
            Expect(v.nth(2)).To.Equal(3);

            Expect(v2.count()).To.Equal(2);
            Expect(v2.nth(0)).To.Equal(1);
            Expect(v2.nth(1)).To.Equal(2);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PopOnEmptyFails()
        {
            IPersistentVector v = LazilyPersistentVector.createOwning();
            ((IPersistentStack)v).pop();
        }

        #endregion
    }

    [TestFixture]
    public class LazilyPersistentVector_IObj_Tests : IObjTests
    {
        [SetUp]
        public void Setup()
        {
            IPersistentVector v = LazilyPersistentVector.createOwning(1, 2, 3);

            _testNoChange = false;
            _obj = _objWithNullMeta = (IObj)v;
            _expectedType = typeof(PersistentVector);
        }
    }
}
