﻿// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.IO;
using Ghpr.Core;
using Ghpr.Core.Common;
using Ghpr.Core.Helpers;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;
using GhprSpecFlow.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GhprMSTest.SpecFlowPlugin
{
    public class GhprMSTestSpecFlowScreenHelper : IGhprSpecFlowScreenHelper
    {
        private readonly TestContext _testContext;

        public GhprMSTestSpecFlowScreenHelper()
        {
        }

        public GhprMSTestSpecFlowScreenHelper(TestContext testContext)
        {
            _testContext = testContext;
        }

        public void SaveScreenshot(byte[] screenBytes)
        {
            var guid = _testContext.Properties["TestGuid"]?.ToString();
            var fullName = GhprMSTestSpecFlowHelper.GetFullNameForGuid(_testContext);
            var testGuid = guid != null ? Guid.Parse(guid) : GuidConverter.ToMd5HashGuid(fullName);
            var fullPath = Path.Combine(ReporterManager.OutputPath, Paths.Folders.Tests, testGuid.ToString(), Paths.Folders.Img);
            var screenshotName = ScreenshotHelper.SaveScreenshot(fullPath, screenBytes, DateTime.Now);
            var count = 0;
            var screenKey = ScreenshotHelper.GetScreenKey(count);
            while (_testContext.Properties[screenKey] != null)
            {
                count++;
                screenKey = ScreenshotHelper.GetScreenKey(count);
            }

            _testContext.Properties.Add(screenKey, screenshotName);
        }

        public List<ITestScreenshot> GetScreenshots()
        {
            var screenshots = new List<ITestScreenshot>();
            var count = 0;
            var screenKey = ScreenshotHelper.GetScreenKey(count);
            while (_testContext.Properties[screenKey] != null)
            {
                var screenshotName = _testContext.Properties[screenKey].ToString();
                screenshots.Add(new TestScreenshot(screenshotName));

                count++;
                screenKey = ScreenshotHelper.GetScreenKey(count);
            }

            return screenshots;
        }
    }
}