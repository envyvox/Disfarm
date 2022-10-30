using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Game.Localization.Queries;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Discord.Extensions
{
	public class VendorAutocompleteHandler : AutocompleteHandler
	{
		public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context,
			IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
		{
			var mediator = services.GetRequiredService<IMediator>();
			var user = await mediator.Send(new GetUserQuery((long)context.User.Id));
			var category = autocompleteInteraction.Data.Options.First(x => x.Name == "category").Value.ToString();
			var userInput = autocompleteInteraction.Data.Current.Value.ToString() ?? "";

			var localizationCategory = category switch
			{
				"fish" => LocalizationCategory.Fish,
				"crops" => LocalizationCategory.Crop,
				_ => throw new ArgumentOutOfRangeException()
			};

			var localizations = await mediator.Send(new GetLocalizationsByCategoryQuery(
				localizationCategory, user.Language));

			return AutocompletionResult.FromSuccess(localizations
				.Where(x => x.Single.StartsWith(userInput))
				.Take(25)
				.Select(x => new AutocompleteResult(x.Single, x.Name)));
		}
	}
}