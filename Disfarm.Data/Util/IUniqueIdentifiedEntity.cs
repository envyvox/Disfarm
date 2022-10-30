using System;

namespace Disfarm.Data.Util
{
	public interface IUniqueIdentifiedEntity
	{
		Guid Id { get; set; }
	}
}
