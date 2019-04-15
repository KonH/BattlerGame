using GameLogics.Shared.Command;
using GameLogics.Shared.Model;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Model.State;
using System.Collections.Generic;
using Xunit;

namespace UnitTests {
	public sealed class UpgradeItemCommandTest : BaseCommandTest<UpgradeItemCommand> {
		readonly ulong     _id;
		readonly ItemState _item;

		public UpgradeItemCommandTest() {
			_config.AddItem("item", new WeaponConfig {
				Damage = new int[] { 1, 2, 3 },
				UpgradePrice = new Dictionary<Resource, int>[] {
					new Dictionary<Resource, int> { { Resource.Coins, 100 } },
					new Dictionary<Resource, int> { { Resource.Coins, 200 } },
				}
			});
			_id = _state.NewEntityId();
			_item = new ItemState("item").WithId(_id);
			_state.AddItem(_item);
			_state.Resources.Add(Resource.Coins, 300);
		}

		[Fact]
		void CantUpgradeUnknownItem() {
			IsInvalid(new UpgradeItemCommand(ulong.MaxValue));
		}

		[Fact]
		void CantUpgradeIfFullyUpgraded() {
			_item.Level = 2;

			IsInvalid(new UpgradeItemCommand(_id));
		}

		[Fact]
		void CantUpgradeIfNoEnoughResource() {
			_state.Resources[Resource.Coins] = 0;

			IsInvalid(new UpgradeItemCommand(_id));
		}

		[Fact]
		void IsLevelIncreased() {
			Execute(new UpgradeItemCommand(_id));

			Assert.Equal(1, _item.Level);
		}

		[Fact]
		void IsResourceSpent() {
			Execute(new UpgradeItemCommand(_id));

			Assert.Equal(200, _state.Resources[Resource.Coins]);

			Execute(new UpgradeItemCommand(_id));

			Assert.Equal(0, _state.Resources[Resource.Coins]);
		}
	}
}
