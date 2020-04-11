﻿using CaptchaSharp.Enums;
using CaptchaSharp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CaptchaSharp
{
    public abstract class CaptchaService
    {
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(3);

        public CaptchaServiceCapabilities Capabilities => CaptchaServiceCapabilities.None;

        public virtual Task<decimal> GetBalanceAsync
            (CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public virtual Task<CaptchaResponse> SolveTextCaptchaAsync
            (string text, TextCaptchaOptions options = null, Proxy proxy = null, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public virtual Task<CaptchaResponse> SolveImageCaptchaAsync
            (string base64, ImageCaptchaOptions options = null, Proxy proxy = null, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public virtual Task<CaptchaResponse> SolveImageCaptchaAsync
            (Bitmap image, ImageFormat format = null, ImageCaptchaOptions options = null,
            Proxy proxy = null, CancellationToken cancellationToken = default)
        {
            return SolveImageCaptchaAsync(image.ToBase64(format ?? ImageFormat.Jpeg), options, proxy, cancellationToken);
        }

        public virtual Task<CaptchaResponse> SolveRecaptchaV2Async
            (string siteKey, string siteUrl, bool invisible = false, Proxy proxy = null, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public virtual Task<CaptchaResponse> SolveRecaptchaV3Async
            (string siteKey, string siteUrl, string action, float minScore, Proxy proxy = null, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }
        
        public virtual Task<CaptchaResponse> SolveFuncaptchaAsync
            (string publicKey, string serviceUrl, string siteUrl, bool noJS = false, Proxy proxy = null,
            CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public virtual Task<CaptchaResponse> SolveHCaptchaAsync
            (string siteKey, string siteUrl, Proxy proxy = null, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public virtual Task<CaptchaResponse> SolveKeyCaptchaAsync
            (string userId, string sessionId, string webServerSign1, string webServerSign2, string siteUrl,
            Proxy proxy = null, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public virtual Task<CaptchaResponse> SolveGeeTestAsync
            (string gt, string challenge, string apiServer, string siteUrl, Proxy proxy = null,
            CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public virtual Task ReportSolution(string taskId, bool correct = false, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        protected async Task<CaptchaResponse> TryGetResult
            (CaptchaTask task, CancellationToken cancellationToken = default)
        {
            var start = DateTime.Now;
            CaptchaResponse result;

            // Initial 5s delay
            await Delay(5000);

            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                result = await CheckResult(task, cancellationToken);
                await Delay(5000);
            }
            while (!task.Completed && DateTime.Now - start < Timeout);

            if (!task.Completed)
                throw new TimeoutException();

            return result;
        }

        protected virtual Task<CaptchaResponse> CheckResult
            (CaptchaTask task, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        protected virtual IEnumerable<(string, string)> ConvertCapabilities(TextCaptchaOptions options)
        {
            throw new NotImplementedException();
        }

        protected virtual IEnumerable<(string, string)> ConvertCapabilities(ImageCaptchaOptions options)
        {
            throw new NotImplementedException();
        }

        private async Task Delay(int milliseconds)
        {
            await Task.Run(() => Thread.Sleep(milliseconds));
        }
    }
}
