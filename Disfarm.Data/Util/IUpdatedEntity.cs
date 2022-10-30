using System;

namespace Disfarm.Data.Util
{
	public interface IUpdatedEntity
	{
		DateTimeOffset UpdatedAt { get; set; }
	}
}
