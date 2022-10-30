using System;

namespace Disfarm.Services.Extensions
{
	public class ExceptionExtensions
	{
		public class GameUserExpectedException : Exception
		{
			public GameUserExpectedException()
			{
			}

			public GameUserExpectedException(string message)
				: base(message)
			{
			}

			public GameUserExpectedException(string message, Exception inner)
				: base(message, inner)
			{
			}
		}
	}
}