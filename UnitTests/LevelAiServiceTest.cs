using System.Collections.Generic;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {
	public class LevelAiServiceTest : BaseCommandTest<EndPlayerTurnCommand> {
		List<ulong> _playerIds;
		List<ulong> _enemyIds;
		
		public LevelAiServiceTest() {
			_config
				.AddLevel("level_desc", new LevelConfig())
				.AddUnit("player_desc", new UnitConfig(1, 1))
				.AddUnit("enemy_desc", new UnitConfig(1, 1));
			_playerIds = new List<ulong> { _state.NewEntityId(), _state.NewEntityId() };
			_enemyIds = new List<ulong> { _state.NewEntityId() };
			_state.Level = new LevelState(
				"level_desc",
				new List<UnitState> {
					new UnitState("player_desc", 1).WithId(_playerIds[0]),
					new UnitState("player_desc", 1).WithId(_playerIds[1])
				},
				new List<UnitState> {
					new UnitState("enemy_desc", 1).WithId(_enemyIds[0])
				}
			);
			_state.Level.PlayerTurn = true;
		}
		
		[Fact]
		void IsAiCommandsPresent() {
			var commands = Execute(new EndPlayerTurnCommand());
			
			Assert.NotEmpty(commands);
		}

		[Fact]
		void IsAiAttackFirstPlayer() {
			Produces<AttackCommand>(new EndPlayerTurnCommand(), cmd => (cmd.DealerId == _enemyIds[0]) && (cmd.TargetId == _playerIds[0]));
		}

		[Fact]
		void IsAiAttackSecondPlayer() {
			_state.Level.PlayerUnits.RemoveAt(0);
			
			Produces<AttackCommand>(new EndPlayerTurnCommand(), cmd => (cmd.DealerId == _enemyIds[0]) && (cmd.TargetId == _playerIds[1]));
		}

		[Fact]
		void IsAiEndsTurn() {
			Produces<EndEnemyTurnCommand>(new EndPlayerTurnCommand());
		}
		
		[Fact]
		void IsLevelWasFinished() {
			_state.Level.PlayerUnits.RemoveAt(0);

			Produces<FinishLevelCommand>(new EndPlayerTurnCommand());
		}
	}
}