// Copyright (c) Charlie Poole, Rob Prouse and Contributors. MIT License - see LICENSE.txt

using System;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Extensions;

namespace NUnit.Framework.Internal
{
    [TestFixture]
    public class PropertyBagTests
    {
        private PropertyBag bag;

        [SetUp]
        public void SetUp()
        {
            bag = new PropertyBag();
            bag.Add("Answer", 42);
            bag.Add("Tag", "bug");
            bag.Add("Tag", "easy");
        }

        [Test]
        public void IndexGetsListOfValues()
        {
            Assert.That(bag["Answer"], Has.Count.EqualTo(1));
            Assert.That(bag["Answer"], Contains.Item(42));

            Assert.That(bag["Tag"], Has.Count.EqualTo(2));
            Assert.That(bag["Tag"], Contains.Item("bug"));
            Assert.That(bag["Tag"], Contains.Item("easy"));
        }

        [Test]
        public void IndexGetsEmptyListIfNameIsNotPresent()
        {
            Assert.That(bag["Level"], Is.Empty);
        }

        [Test]
        public void IndexSetsListOfValues()
        {
            bag["Zip"] = new[] {"junk", "more junk"};
            Assert.That(bag["Zip"], Has.Count.EqualTo(2));
            Assert.That(bag["Zip"], Contains.Item("junk"));
            Assert.That(bag["Zip"], Contains.Item("more junk"));
        }

        [Test]
        public void AllKeysAreListed()
        {
            Assert.That(bag.Keys, Has.Count.EqualTo(2));
            Assert.That(bag.Keys, Has.Member("Answer"));
            Assert.That(bag.Keys, Has.Member("Tag"));
        }

        [Test]
        public void ContainsKey()
        {
            Assert.That(bag.ContainsKey("Answer"));
            Assert.That(bag.ContainsKey("Tag"));
            Assert.That(bag.ContainsKey("Target"), Is.False);
        }

        [Test]
        public void GetReturnsSingleValue()
        {
            Assert.That(bag.Get("Answer"), Is.EqualTo(42));
            Assert.That(bag.Get("Tag"), Is.EqualTo("bug"));
        }

        [Test]
        public void SetAddsNewSingleValue()
        {
            bag.Set("Zip", "ZAP");
            Assert.That(bag["Zip"], Has.Count.EqualTo(1));
            Assert.That(bag["Zip"], Has.Member("ZAP"));
            Assert.That(bag.Get("Zip"), Is.EqualTo("ZAP"));
        }

        [Test]
        public void SetReplacesOldValues()
        {
            bag.Set("Tag", "ZAPPED");
            Assert.That(bag["Tag"], Has.Count.EqualTo(1));
            Assert.That(bag.Get("Tag"), Is.EqualTo("ZAPPED"));
        }

        [Test]
        public void XmlIsProducedCorrectly()
        {
            TNode topNode = bag.ToXml(true);
            Assert.That(topNode.Name, Is.EqualTo("properties"));

            string[] props = new string[topNode.ChildNodes.Count];
            for (int i = 0; i < topNode.ChildNodes.Count; i++)
            {
                TNode node = topNode.ChildNodes[i];

                Assert.That(node.Name, Is.EqualTo("property"));

                props[i] = $"{node.Attributes["name"]}={node.Attributes["value"]}";
            }

            Assert.That(props,
                Is.EquivalentTo(new[] { "Answer=42", "Tag=bug", "Tag=easy" }));
        }

        [Test]
        public void TestNullPropertyValueIsntAdded()
        {
            Assert.Throws<ArgumentNullException>(() => bag.Add("dontAddMe", null));
            Assert.That(bag.ContainsKey("dontAddMe"), Is.False);
        }

        [Test]
        public void TestTryGet()
        {
            Assert.That(bag.TryGet("Answer", 37), Is.EqualTo(42));
            Assert.That(bag.TryGet("WrongAnswer", 37), Is.EqualTo(37));

            Assert.That(bag.TryGet("Tag", "none"), Is.EqualTo("bug"));
            Assert.That(bag.TryGet("Target", "netstandard"), Is.EqualTo("netstandard"));
        }
    }
}
