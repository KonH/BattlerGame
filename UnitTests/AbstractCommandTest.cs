using System;
using System.Collections.Generic;
using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models.State;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {
	public class AbstractCommandTest {
		class JustCommand : ICommand {
			public readonly string        Name;
			public readonly JustCommand[] Childs;

			public JustCommand(string name, JustCommand[] childs) {
				Name   = name;
				Childs = childs;
			}
			
			public override string ToString() {
				return Name;
			}

			public bool IsValid(GameState state, Config config) => true;

			public void Execute(GameState state, Config config, ICommandBuffer buffer) {
				foreach ( var cmd in Childs ) {
					buffer.Add(cmd);
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

			ICommand cmd = new JustCommand(
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
			
			var real = new List<string>();
			
			var runner = new CommandRunner(cmd, null, null);
			
			var iter = runner.GetEnumerator();
			iter.MoveNext();
			// Execute is required to proceed to next command
			// (if command doesn't executed, state will be in unexpected state)
			Assert.Throws<InvalidOperationException>(() => iter.MoveNext());
			iter.Dispose();
			
			foreach ( var item in runner ) {
				if ( item.IsValid() ) {
					item.Execute();
					real.Add(item.Command.ToString());
				}
			}
			
			Assert.Equal(expected, real.ToArray());
		}
	}
}