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

    [TestFixture]
    public class AReferenceTests
    {
        #region Concrete AReference

        // AReference is abstract.  We need a class to instantiate.

        class ConcreteAReference : AReference
        {
            public ConcreteAReference() : base() { }
            public ConcreteAReference(IPersistentMap meta) : base(meta) { }
        }

        #endregion

        #region C-tor tests

        [Test]
        public void Default_ctor_creates_with_null_metadata()
        {
            ConcreteAReference c = new ConcreteAReference();
            Expect(c.meta()).To.Be.Null();
        }

        [Test]
        public void Map_ctor_creates_with_given_metadata()
        {
            IPersistentMap meta = new DummyMeta();

            ConcreteAReference c = new ConcreteAReference(meta);
            Expect(object.ReferenceEquals(c.meta(), meta));
        }

        #endregion

        #region IReference tests

        [Test]
        public void AlterMeta_changes_meta()
        {
            IPersistentMap meta = new DummyMeta();
            IFn fn = DummyFn.CreateForMetaAlter(meta);

            ConcreteAReference c = new ConcreteAReference();
            c.alterMeta(fn, null);

            Expect(object.ReferenceEquals(c.meta(), meta));
        }

        [Test]
        public void ResetMeta_sets_meta()
        {
            IPersistentMap meta = new DummyMeta();

            ConcreteAReference c = new ConcreteAReference();
            c.resetMeta(meta);

            Expect(c.meta()).To.Equal(meta);
        }


        #endregion
    }
}
