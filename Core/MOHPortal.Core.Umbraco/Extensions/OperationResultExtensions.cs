//using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace MOHPortal.Core.Umbraco.Extensions
{
    //TODO[AS]: Check usage with MA and MM to validate if needed or not
    public static class OperationResultExtensions
    {
        public static string FormatOperationResultLog(this OperationResult operationResult , string fallbackMessage = "Operation Has Failed")
        {
            if(operationResult.EventMessages is null)
            {
                return fallbackMessage;
            }
            IEnumerable<string> messages = operationResult.EventMessages.GetAll().Select(x => $"[{x.MessageType.GetDisplayName()}] {x.Category}, {x.Message}");
           
            return $"Operation Failed With The Following Message {fallbackMessage} \n {string.Join("\n ", messages)}";
        }

        public static string FormatPublishResultLog(this PublishResult operationResult, string fallbackMessage = "Operation Has Failed")
        {
            if (operationResult.EventMessages is null)
            {
                return fallbackMessage;
            }
            IEnumerable<string> messages = operationResult.EventMessages.GetAll().Select(x => $"[{x.MessageType.GetDisplayName()}] {x.Category}, {x.Message}");

            return $"Operation Failed With The Following Message {fallbackMessage} \n {string.Join("\n ", messages)}";
        }
    }
}
