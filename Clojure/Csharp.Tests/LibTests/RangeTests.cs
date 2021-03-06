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


using NUnit.Framework;
using static NExpect.Expectations;
using clojure.lang;
using NExpect;

namespace Clojure.Tests.LibTests
{
    public abstract class RangeTestHelper
    {
        #region Data

        protected delegate object RangeCreateFn(object start, object end);
        protected RangeCreateFn _createFn;

        #endregion

        #region C-tor tests

        [Test]
        public void Basic_ctor_has_no_meta()
        {
            IObj r = (IObj)_createFn(2L, 5L);
            Expect(r.meta()).To.Be.Null();
        }

        [Test]
        public void Meta_ctor_has_meta()
        {
            IPersistentMap meta = new DummyMeta();
            IObj r = ((IObj)_createFn(2L, 5L)).withMeta(meta);
            Expect(r.meta()).To.Equal(meta);
        }

        #endregion

        #region IPersistentCollection tests

        [Test]
        public void Range_has_correct_count()
        {
            ISeq r = (ISeq)_createFn(2L, 20L);
            Expect(r.count()).To.Equal(18);
        }

        #endregion

        #region IReduce tests

        [Test]
        public void ReduceWithNoStartIterates()
        {
            IFn fn = DummyFn.CreateForReduce();
            IReduce r = (IReduce)_createFn(2L, 5L);
            object ret = r.reduce(fn);
            Expect(ret).To.Be.An.Instance.Of<long>();
            Expect((long)ret).To.Equal(9);
        }

        [Test]
        public void ReduceWithStartIterates()
        {
            IFn fn = DummyFn.CreateForReduce();

            IReduce r = (IReduce)_createFn(2L, 5L);
            object ret = r.reduce(fn, 20);
            Expect(ret).To.Be.An.Instance.Of<long>();
            Expect((long)ret).To.Equal(29);
        }

        #endregion
    }



    [TestFixture]
    public class Range_Tests : RangeTestHelper
    {

        [SetUp]
        public void Setup()
        {
            _createFn = (start, end) => (object)Range.create(start, end);
        }
    }

    [TestFixture]
    public class LongRange_Tests : RangeTestHelper
    {

        [SetUp]
        public void Setup()
        {
            _createFn = (start, end) => (object)LongRange.create((long)start, (long)end);
        }
    }



    [TestFixture]
    public class Range_ISeq_Tests : ISeqTestHelper
    {
        #region Setup

        Range _r;
        Range _rWithMeta;
        object[] _values;

        [SetUp]
        public void Setup()
        {
            IPersistentMap meta = PersistentHashMap.create("a", 1, "b", 2);

            _r = (Range)Range.create(2L, 5L);
            _rWithMeta = (Range)_r.withMeta(meta);
            _values = new object[] { 2L, 3L, 4L };
        }

        #endregion

        #region tests

        [Test]
        public void Range_has_correct_values()
        {
            VerifyISeqContents(_r, _values);
        }

        [Test]
        public void Range_with_meta_has_correct_values()
        {
            VerifyISeqContents(_rWithMeta, _values);
        }

        // NB: in Range prior to CLJ-1515 (commit 07d6129 2015-04-10), next preserved meta.
        // in Range after, it does not.
        //[Test]
        //public void Rest_preserves_meta()
        //{
        //    VerifyISeqRestMaintainsMeta(_rWithMeta);
        //}

        [Test]
        public void Rest_preserves_type()
        {
            VerifyISeqRestTypes(_r, typeof(Range));
        }

        [Test]
        public void Cons_works()
        {
            VerifyISeqCons(_r, 12, _values);
        }

        #endregion
    }

    [TestFixture]
    public class Range_IObj_Tests : IObjTests
    {
        [SetUp]
        public void Setup()
        {
            IPersistentMap meta = new DummyMeta();

            Range r = (Range)Range.create(2L, 5L);

            _objWithNullMeta = (IObj)r;
            _obj = _objWithNullMeta.withMeta(meta);
            _expectedType = typeof(Range);
        }
    }

    [TestFixture]
    public class LongRange_IObj_Tests : IObjTests
    {
        [SetUp]
        public void Setup()
        {
            IPersistentMap meta = new DummyMeta();

            LongRange r = (LongRange)LongRange.create(2L, 5L);

            _objWithNullMeta = (IObj)r;
            _obj = _objWithNullMeta.withMeta(meta);
            _expectedType = typeof(LongRange);
        }
    }
}
