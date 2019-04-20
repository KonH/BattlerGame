namespace GameLogics.Shared.Command.Base {
	public interface ICommandBuffer {
		void Add(ICommand command);
	}
}