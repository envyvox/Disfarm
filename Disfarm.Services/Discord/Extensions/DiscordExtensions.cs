using System;
using System.Linq;
using Discord;
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
    }
}