using System.Collections.Generic;
using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {
	public class AbstractCommandTest {
		class JustCommand : BaseCommand {
			public readonly string        Name;
			public readonly JustCommand[] Childs;

			public JustCommand(string name, JustCommand[] childs) {
				Name   = name;
				Childs = childs;
			}
			
			public override string ToString() {
				return Name;
			}

			protected override bool IsValid(GameState state, Config config) => true;

			protected override void Execute(GameState state, Config config, ICommandBuffer buffer) {
				foreach ( var cmd in Childs ) {
					buffer.AddCommand(cmd);
				}
			}
		}
		
		[Fact]
		void IsCommandsExecutedInTreeOrder() {
			// cmd1.1
			// -> cmd2.1
			//    -> cmd3.1
			//    -> cmd3.2
			//       -> cmd4.1
			// -> cmd2.2
			//    -> cmd3.3
			// =>
			// cmd1.1, cmd2.1, cmd2.2, cmd3.1, cmd3.2, cmd3.3, cmd4.4

			ICompositeCommand cmd = new JustCommand(
				"cmd1.1",
				new[] {
					new JustCommand(
						"cmd2.1",
						new[] {
							new JustCommand(
								"cmd3.1",
								new JustCommand[0]
							),
							new JustCommand(
								"cmd3.2",
								new[] {
									new JustCommand(
										"cmd4.1",
										new JustCommand[0]
									)
								}
							),
						}
					),
					new JustCommand(
						"cmd2.2",
						new[] {
							new JustCommand(
								"cmd3.3",
								new JustCommand[0]
							)
						}
					),
				}
			);
				
			var expected = new[] {
				"cmd1.1", "cmd2.1", "cmd2.2", "cmd3.1", "cmd3.2", "cmd3.3", "cmd4.1"
			};

			// No direct access, it can breaks logic
			// We can't check single command to validate, it requires to check sub-commands, but they can depend on new state
			// cmd.IsValid();
			// We can't execute single command, it can contains required dependencies
			// cmd.Execute();
			
			var real = new List<string>();
			
			foreach ( var c in cmd.AsEnumerable() ) {
				if ( c.IsCommandValid(null, null) ) {
					// ExecuteCommand is required to proceed to next command
					// (if command doesn't executed, state will be in unexpected state)
					c.ExecuteCommand(null, null);
					real.Add(c.ToString());
				}
			}
			
			Assert.Equal(expected, real.ToArray());
		}
	}
}