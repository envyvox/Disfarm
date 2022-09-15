using System;

namespace Disfarm.Data.Util
{
    public interface IExpirationEntity
    {
        DateTimeOffset Expiration { get; set; }
    }
}