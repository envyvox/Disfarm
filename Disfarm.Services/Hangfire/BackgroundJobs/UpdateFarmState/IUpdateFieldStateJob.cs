using System;
using System.Threading.Tasks;
using Disfarm.Data.Enums;

namespace Disfarm.Services.Hangfire.BackgroundJobs.UpdateFarmState
{
	public interface IUpdateFieldStateJob
	{
		Task Execute(Guid userFarmId, FieldState state);
	}
}