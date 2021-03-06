﻿using FeralTic.DX11;
using FeralTic.DX11.Resources;
using KGP.Direct3D11.Textures;
using KGP.Frames;
using KGP.Providers;
using KGP.Providers.Sensor;
using Microsoft.Kinect;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LongExposureInfraredTextureSample
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            RenderForm form = new RenderForm("Kinect long exposure sample");

            RenderDevice device = new RenderDevice(SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport);
            RenderContext context = new RenderContext(device);
            DX11SwapChain swapChain = DX11SwapChain.FromHandle(device, form.Handle);

            KinectSensor sensor = KinectSensor.GetDefault();
            sensor.Open();



            bool doQuit = false;
            bool doUpload = false;
            LongExposureInfraredFrameData currentData = null;
            DynamicLongExposureInfraredTexture irTexture = new DynamicLongExposureInfraredTexture(device);
            KinectSensorLongExposureInfraredFrameProvider provider = new KinectSensorLongExposureInfraredFrameProvider(sensor);
            provider.FrameReceived += (sender, args) => { currentData = args.FrameData; doUpload = true; };

            form.KeyDown += (sender, args) => { if (args.KeyCode == Keys.Escape) { doQuit = true; } };

            RenderLoop.Run(form, () =>
            {
                if (doQuit)
                {
                    form.Dispose();
                    return;
                }

                if (doUpload)
                {
                    irTexture.Copy(context, currentData);
                }

                context.RenderTargetStack.Push(swapChain);

                device.Primitives.ApplyFullTri(context, irTexture.ShaderView);

                device.Primitives.FullScreenTriangle.Draw(context);
                context.RenderTargetStack.Pop();
                swapChain.Present(0, SharpDX.DXGI.PresentFlags.None);
            });

            swapChain.Dispose();
            context.Dispose();
            device.Dispose();

            irTexture.Dispose();
            provider.Dispose();

            sensor.Close();
        }
    }
}
