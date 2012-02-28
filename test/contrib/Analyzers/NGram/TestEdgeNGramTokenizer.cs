﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.NGram;
using NUnit.Framework;

namespace Lucene.Net.Analyzers.Miscellaneous
{
    /**
     * Tests {@link EdgeNGramTokenizer} for correctness.
     */
    [TestFixture]
    public class EdgeNGramTokenizerTest : BaseTokenStreamTestCase
    {
        private StringReader input;

        public override void SetUp()
        {
            input = new StringReader("abcde");
        }
        [Test]
        public void TestInvalidInput()
        {
            bool gotException = false;
            try
            {
                new EdgeNGramTokenizer(input, Side.FRONT, 0, 0);
            }
            catch (ArgumentException e)
            {
                gotException = true;
            }
            Assert.True(gotException);
        }

        [Test]
        public void TestInvalidInput2()
        {
            bool gotException = false;
            try
            {
                new EdgeNGramTokenizer(input, Side.FRONT, 2, 1);
            }
            catch (ArgumentException e)
            {
                gotException = true;
            }
            Assert.True(gotException);
        }

        [Test]
        public void TestInvalidInput3()
        {
            bool gotException = false;
            try
            {
                new EdgeNGramTokenizer(input, Side.FRONT, -1, 2);
            }
            catch (ArgumentException e)
            {
                gotException = true;
            }
            Assert.True(gotException);
        }

        [Test]
        public void TestFrontUnigram()
        {
            EdgeNGramTokenizer tokenizer = new EdgeNGramTokenizer(input, Side.FRONT, 1, 1);
            AssertTokenStreamContents(tokenizer, new String[] { "a" }, new int[] { 0 }, new int[] { 1 }, 5 /* abcde */);
        }

        [Test]
        public void TestBackUnigram()
        {
            EdgeNGramTokenizer tokenizer = new EdgeNGramTokenizer(input, Side.BACK, 1, 1);
            AssertTokenStreamContents(tokenizer, new String[] { "e" }, new int[] { 4 }, new int[] { 5 }, 5 /* abcde */);
        }

        [Test]
        public void TestOversizedNgrams()
        {
            EdgeNGramTokenizer tokenizer = new EdgeNGramTokenizer(input, Side.FRONT, 6, 6);
            AssertTokenStreamContents(tokenizer, new String[0], new int[0], new int[0], 5 /* abcde */);
        }

        [Test]
        public void TestFrontRangeOfNgrams()
        {
            EdgeNGramTokenizer tokenizer = new EdgeNGramTokenizer(input, Side.FRONT, 1, 3);
            AssertTokenStreamContents(tokenizer, new String[] { "a", "ab", "abc" }, new int[] { 0, 0, 0 }, new int[] { 1, 2, 3 }, 5 /* abcde */);
        }

        [Test]
        public void TestBackRangeOfNgrams()
        {
            EdgeNGramTokenizer tokenizer = new EdgeNGramTokenizer(input, Side.BACK, 1, 3);
            AssertTokenStreamContents(tokenizer, new String[] { "e", "de", "cde" }, new int[] { 4, 3, 2 }, new int[] { 5, 5, 5 }, 5 /* abcde */);
        }

        [Test]
        public void TestReset()
        {
            EdgeNGramTokenizer tokenizer = new EdgeNGramTokenizer(input, Side.FRONT, 1, 3);
            AssertTokenStreamContents(tokenizer, new String[] { "a", "ab", "abc" }, new int[] { 0, 0, 0 }, new int[] { 1, 2, 3 }, 5 /* abcde */);
            tokenizer.Reset(new StringReader("abcde"));
            AssertTokenStreamContents(tokenizer, new String[] { "a", "ab", "abc" }, new int[] { 0, 0, 0 }, new int[] { 1, 2, 3 }, 5 /* abcde */);
        }
    }
}