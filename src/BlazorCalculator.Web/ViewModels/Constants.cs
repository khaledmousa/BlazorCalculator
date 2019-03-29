using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCalculator.Web.ViewModels
{
    internal static class Constants
    {
        internal static class Text
        {
            internal const string AddStatementCaption = "Add Expression";
            internal const string StatementPlaceholder = "e.g. x = (y + 2) * z^2";
            internal const string InvalidStatement = "Invalid expression";
            internal const string CyclicStatement = "This expression will cause a cycle in variable evaluations";
        }
    }
}
