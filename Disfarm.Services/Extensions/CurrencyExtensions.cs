using System;

namespace Disfarm.Services.Extensions
{
    public static class CurrencyExtensions
    {
        public static uint ConvertTokensToChips(this uint tokensAmount)
        {
            return (uint) Math.Ceiling((decimal) (tokensAmount / 100.0));
        }
    }
}