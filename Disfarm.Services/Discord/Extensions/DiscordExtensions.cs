using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Discord.Extensions
{
	public static class DiscordExtensions
	{
		public static IApplicationBuilder StartDiscord(this IApplicationBuilder app)
		{
			using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
			var service = serviceScope.ServiceProvider.GetRequiredService<IDiscordClientService>();
			service.Start().Wait();

			return app;
		}

		public static ModalBuilder UpdateTextInput(this ModalBuilder modal, string customId,
			Action<TextInputBuilder> input)
		{
			var components = modal.Components.ActionRows.SelectMany(r => r.Components).OfType<TextInputComponent>();
			var component = components.First(c => c.CustomId == customId);

			var builder = new TextInputBuilder
			{
				CustomId = customId,
				Label = component.Label,
				MaxLength = component.MaxLength,
				MinLength = component.MinLength,
				Placeholder = component.Placeholder,
				Required = component.Required,
				Style = component.Style,
				Value = component.Value
			};

			input(builder);

			foreach (var row in modal.Components.ActionRows.Where(
						 row => row.Components.Any(c => c.CustomId == customId)))
			{
				row.Components.RemoveAll(c => c.CustomId == customId);
				row.AddComponent(builder.Build());
			}

			return modal;
		}

		public static async Task FollowUpResponse(this SocketInteraction interaction, EmbedBuilder embedBuilder,
			MessageComponent components = null, string text = null, bool ephemeral = false)
		{
			try
			{
				await interaction.FollowupAsync(
					text: text,
					embed: embedBuilder.Build(),
					components: components,
					ephemeral: ephemeral);
			}
			catch (HttpException e)
			{
				foreach (var discordJsonError in e.Errors)
				{
					foreach (var discordError in discordJsonError.Errors)
					{
						Console.WriteLine(discordError.Message);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		public static async Task ClearOriginalResponse(this SocketInteraction interaction, Language language)
		{
			await interaction.ModifyOriginalResponseAsync(x =>
			{
				x.Content = Response.OriginalResponseCleared.Parse(language);
				x.Embed = null;
				x.Components = new ComponentBuilder().Build();
			});
		}
	}
}