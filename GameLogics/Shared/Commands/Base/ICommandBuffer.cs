namespace GameLogics.Shared.Commands.Base {
	/// <summary>
	/// Allow to add sub-commands only
	/// </summary>
	public interface ICommandBuffer {
		void AddCommand(BaseCommand cmd);
	}
}