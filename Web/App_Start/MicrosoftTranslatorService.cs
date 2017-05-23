//using CafeT.Text;
//using Microsoft.AspNet.Identity;
//using Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Translator.API;
using System.Net.Http;
using Web.TranslatorService;

namespace Microsoft.Translator.API
{
    public class MicrosoftTranslatorService
    {
        //TAIPM: The first key in Coginitve Services (translate)
        private const string SubscriptionKey = "3c45b3b12bda4d568939ab4fc7245944";   //Enter here the Key from your Microsoft Translator Text subscription on http://portal.azure.com

        /// Demonstrates getting an access token and using the token to translate.
        public async Task<string> TranslateAsync(string content)
        {
            try
            {
                content = "Hello";
                var translatorService = new LanguageServiceClient();
                var authTokenSource = new AzureAuthToken(SubscriptionKey);
                var token = string.Empty;

                try
                {
                    token = await authTokenSource.GetAccessTokenAsync();
                }
                catch (HttpRequestException)
                {
                    switch (authTokenSource.RequestStatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            Console.WriteLine("Request to token service is not authorized (401). Check that the Azure subscription key is valid.");
                            break;
                        case HttpStatusCode.Forbidden:
                            Console.WriteLine("Request to token service is not authorized (403). For accounts in the free-tier, check that the account quota is not exceeded.");
                            break;
                    }
                    throw;
                }

                return translatorService.Translate(token, content, "en", "vi", "text/plain", "general", string.Empty);
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
    }
}