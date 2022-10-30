using System;

namespace Disfarm.Data.Util
{
	public interface ICreatedEntity
	{
		DateTimeOffset CreatedAt { get; set; }
	}
}
