using DisCatSharp;
using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.Enums;
using DisCatSharp.Entities;

namespace EirBot_New.Events;
[SlashCommandGroup("Random", "Random commands.", true, false), EventHandler]
public class RandomCommands : ApplicationCommandsModule {
	private static string GetRollResult(int sides, int number) {
		string resultString = "";
		int total = 0;
		for (int i = 0; i < number; i++) {
			int num = Random.Shared.Next(1, sides + 1);
			total += num;
			resultString += "D" + sides + " :game_die: **" + num + "**\n";
		}
		if (number > 1)
			resultString += "Total: **" + total + "**";
		return resultString;
	}
	private static async Task PostResult(InteractionContext context, string resultString) {
		await context.EditResponseAsync(new DiscordWebhookBuilder()
			.WithContent(resultString)
		);
	}

	[SlashCommand("Coin", "Flip a coin.", true, false)]
	public static async Task Coin(InteractionContext context) {
		await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
		bool flip = Random.Shared.NextSingle() < 0.5f;
		await PostResult(context, ":coin: **" + (flip ? "Heads" : "Tails") + "**");
	}

	[SlashCommand("D4", "Roll one or more D4s.", true, false)]
	public static async Task D4(InteractionContext context, [Option("number", "Amount of dice to roll.", false), MinimumValue(1), MaximumValue(20)] int number = 1) {
		await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
		await PostResult(context, GetRollResult(4, number));
	}
	[SlashCommand("D6", "Roll one or more D6s.", true, false)]
	public static async Task D6(InteractionContext context, [Option("number", "Amount of dice to roll.", false), MinimumValue(1), MaximumValue(20)] int number = 1) {
		await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
		await PostResult(context, GetRollResult(6, number));
	}
	[SlashCommand("Dice", "Roll one or more dice (D6s).", true, false)]
	public static async Task Dice(InteractionContext context, [Option("number", "Amount of dice to roll.", false), MinimumValue(1), MaximumValue(20)] int number = 1) {
		await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
		await PostResult(context, GetRollResult(6, number));
	}
	[SlashCommand("D8", "Roll one or more D8s.", true, false)]
	public static async Task D8(InteractionContext context, [Option("number", "Amount of dice to roll.", false), MinimumValue(1), MaximumValue(20)] int number = 1) {
		await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
		await PostResult(context, GetRollResult(8, number));
	}
	[SlashCommand("D10", "Roll one or more D10s.", true, false)]
	public static async Task D10(InteractionContext context, [Option("number", "Amount of dice to roll.", false), MinimumValue(1), MaximumValue(20)] int number = 1) {
		await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
		await PostResult(context, GetRollResult(10, number));
	}
	[SlashCommand("D20", "Roll one or more D20s.", true, false)]
	public static async Task D20(InteractionContext context, [Option("number", "Amount of dice to roll.", false), MinimumValue(1), MaximumValue(20)] int number = 1) {
		await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
		await PostResult(context, GetRollResult(20, number));
	}
	[SlashCommand("D100", "Roll one or more D100s.", true, false)]
	public static async Task D100(InteractionContext context, [Option("number", "Amount of dice to roll.", false), MinimumValue(1), MaximumValue(20)] int number = 1) {
		await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
		await PostResult(context, GetRollResult(100, number));
	}

	public enum FateLadder {
		Horrifying = -4,
		Catastrophic = -3,
		Terrible = -2,
		Poor = -1,
		Mediocre = 0,
		Average = 1,
		Fair = 2,
		Good = 3,
		Great = 4,
		Superb = 5,
		Fantastic = 6,
		Epic = 7,
		Legendary = 8
	}

	[SlashCommand("Fate", "Roll Fate RPG dice (4).", true, false)]
	public static async Task Fate(InteractionContext context, [Option("modifier", "Modifier to add to roll result."), MinimumValue(-4), MaximumValue(4)] int modifier = 0) {
		await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
		string result = "Fate :game_die: **__";

		int total = 0;
		int[] rolls = new int[4];
		for (int i = 0; i < 4; i++) {
			rolls[i] = Random.Shared.Next(-1, 2);
			if (rolls[i] == -1)
				result += "-";
			else if (rolls[i] == 0)
				result += "\\_";
			else if (rolls[i] == 1)
				result += "+";
			total += rolls[i];
		}
		total += modifier;

		result += "__ " + (modifier >= 0 ? "+" : "") + modifier + " = " + (total >= 0 ? "+" : "") + total + " (" + (FateLadder)Math.Clamp(total, -4, 8) + ")**";
		await PostResult(context, result);
	}
}
