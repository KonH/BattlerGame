namespace GameLogics.Shared.Commands.Base {
	public interface ICommandBuffer {
		void Add(ICommand command);
	}
}