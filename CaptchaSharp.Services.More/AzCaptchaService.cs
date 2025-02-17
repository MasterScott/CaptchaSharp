﻿using CaptchaSharp.Enums;
using System;
using System.Net.Http;

namespace CaptchaSharp.Services.More
{
    public class AzCaptchaService : CustomTwoCaptchaService
    {
        public AzCaptchaService(string apiKey, HttpClient httpClient = null)
            : base(apiKey, new Uri("http://azcaptcha.com"), httpClient, false)
        {
            SupportedCaptchaTypes =
                CaptchaType.ImageCaptcha |
                CaptchaType.ReCaptchaV2 |
                CaptchaType.ReCaptchaV3;
        }
    }
}
