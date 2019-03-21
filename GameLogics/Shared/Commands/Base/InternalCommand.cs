namespace GameLogics.Shared.Commands.Base {
	/// <summary>
	/// Internal command can be called only as sub-command, server rejects if it's first command
	/// It's required for code re-usage and called inside other commands
	/// </summary>
	public abstract class InternalCommand : BaseCommand {}
}