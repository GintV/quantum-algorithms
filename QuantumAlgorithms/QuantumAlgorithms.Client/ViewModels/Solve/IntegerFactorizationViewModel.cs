using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using QuantumAlgorithms.Models.Get;

namespace QuantumAlgorithms.Client.ViewModels.Solve
{
    public class IntegerFactorizationViewModel : IntegerFactorizationGetDto
    {
        public string Url { get; set; }

        public static HtmlString ParseExecutionMessage(string message)
        {
            message = message.Replace("*", "&sdot;");
            message = message.Replace("/", "&frasl;");
            message = message.Replace("=>", "&rarr;");

            int powerIndex;
            while ((powerIndex = message.IndexOf('^')) != -1)
            {
                var subPower = message.Substring(powerIndex, message.IndexOf(')', powerIndex) - powerIndex + 1);
                message = message.Replace(subPower, $"<sup>{subPower.Substring(2, subPower.Length - 3)}</sup>");
            }

            return new HtmlString(message);
        }
    }
}
