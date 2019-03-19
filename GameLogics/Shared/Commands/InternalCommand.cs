namespace GameLogics.Shared.Commands {
	/// <summary>
	/// Internal command can be called only as sub-command, server rejects such direct commands
	/// It's required for code re-usage and called inside other commands
	/// </summary>
	public abstract class InternalCommand : BaseCommand {}
}