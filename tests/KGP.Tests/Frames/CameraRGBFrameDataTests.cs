﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KGP.Frames;

namespace KGP.Tests
{
    [TestClass]
    public class CameraRGBFrameDataTests
    {
        [TestMethod]
        public void TestConstrutor()
        {
            CameraRGBFrameData data = new CameraRGBFrameData();
            bool pass = data.DataPointer != IntPtr.Zero;
            data.Dispose();
            Assert.AreEqual(true, pass);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void TestDispose()
        {
            CameraRGBFrameData data = new CameraRGBFrameData();
            data.Dispose();
            Assert.AreEqual(data.DataPointer, IntPtr.Zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void TestDisposeAccess()
        {
            CameraRGBFrameData data = new CameraRGBFrameData();
            data.Dispose();

            //Should throw exception
            var pointer = data.DataPointer;
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void TestMultipleDispose()
        {
            CameraRGBFrameData data = new CameraRGBFrameData();
            data.Dispose();
            //Second call to dispose should do nothing
            data.Dispose();
            
            Assert.AreEqual(data.DataPointer, IntPtr.Zero);
        }

        [TestMethod]
        public void TestSize()
        {
            CameraRGBFrameData data = new CameraRGBFrameData();
            int expected = 512 * 424 * 12;
            bool pass = data.SizeInBytes == expected;
            data.Dispose();
            Assert.AreEqual(pass, true);
        }

        [TestMethod]
        public void TestDisposedSize()
        {
            CameraRGBFrameData data = new CameraRGBFrameData();
            data.Dispose();
            Assert.AreEqual(data.SizeInBytes, 0);
        }
    }
}
