using System;
using System.Collections.Generic;
using System.Data.Entity;
using IgiCore.Inventory.Server.Models;
using IgiCore.Inventory.Server.Storage;
using IgiCore.Inventory.Shared.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFive.SDK.Core.Helpers;

namespace IgiCore.Inventory.Server.Tests
{
	[TestClass]
	public class InventoryManagerTests
	{
		private readonly Mock<InventoryManager> mockInventoryManager = new Mock<InventoryManager>();
		private readonly Mock<StorageContext> mockStorageContext = new Mock<StorageContext>();

		[TestInitialize]
		public void Initialize()
		{
			this.mockInventoryManager.Setup(x => x.GetContext()).Returns(this.mockStorageContext.Object);
		}
		
		[TestMethod]
		public void CanItemWeightFitInContainer_ItemExceedsWeight_ThrowsMaxWeightExceededException()
		{
			var item = new Item
			{
				Weight = 10
			};
			var container = new Container
			{
				MaxWeight = 5
			};

			Assert.ThrowsException<MaxWeightExceededException>(() =>
				this.mockInventoryManager.Object.CanItemWeightFitInContainer(item, container));
		}

		[TestMethod]
		public void CanItemWeightFitInContainer_ItemDoesNotExceedWeight_NoExceptionThrown()
		{
			var item = new Item
			{
				Weight = 5
			};
			var container = new Container
			{
				MaxWeight = 10
			};

			try
			{
				this.mockInventoryManager.Object.CanItemWeightFitInContainer(item, container);
			}
			catch (Exception ex)
			{
				Assert.Fail($"Expected no exception, but got {ex.GetType().Name}: {ex.Message}");
			}
		}

		[TestMethod]
		public void CanItemSizeFitInContainerAt_ItemCollides_ThrowsItemOverlapException()
		{
			var item = new Item
			{
				Width = 2,
				Height = 2,
			};
			var container = new Container
			{
				Width = 10,
				Height = 10,
				Items = new List<Item>
				{
					new Item
					{
						X = 0,
						Y = 0,
						Width = 5,
						Height = 5
					}
				}
			};

			Assert.ThrowsException<ItemOverlapException>(() =>
				this.mockInventoryManager.Object.CanItemSizeFitInContainerAt(4, 3, item, container));
			Assert.ThrowsException<ItemOverlapException>(() =>
				this.mockInventoryManager.Object.CanItemSizeFitInContainerAt(3, 4, item, container));
		}

		[TestMethod]
		public void CanItemSizeFitInContainerAt_ItemOutOfBounds_ThrowsItemOverlapException()
		{
			var item = new Item
			{
				Width = 2,
				Height = 2,
			};
			var container = new Container
			{
				Width = 10,
				Height = 10,
			};

			Assert.ThrowsException<ItemOutOfContainerBoundsException>(() =>
				this.mockInventoryManager.Object.CanItemSizeFitInContainerAt(8, 9, item, container));
			Assert.ThrowsException<ItemOutOfContainerBoundsException>(() =>
				this.mockInventoryManager.Object.CanItemSizeFitInContainerAt(9, 8, item, container));
		}

		[TestMethod]
		public void CanItemSizeFitInContainerAt_ItemCanFit_NoExceptionThrown()
		{
			var item = new Item
			{
				Width = 2,
				Height = 2,
			};
			var container = new Container
			{
				Width = 10,
				Height = 10,
				Items = new List<Item>
				{
					new Item
					{
						X = 0,
						Y = 0,
						Width = 5,
						Height = 5
					}
				}
			};

			try
			{
				this.mockInventoryManager.Object.CanItemSizeFitInContainerAt(5, 5, item, container);
				this.mockInventoryManager.Object.CanItemSizeFitInContainerAt(8, 8, item, container);
			}
			catch (Exception ex)
			{
				Assert.Fail($"Expected no exception, but got {ex.GetType().Name}: {ex.Message}");
			}
		}

		[TestMethod]
		public void CanItemFitInContainerBoundsAt_ItemOutOfBounds_ThrowsItemOutOfContainerBoundsException()
		{
			var item = new Item
			{
				Width = 5,
				Height = 5,
			};

			var container = new Container
			{
				Width = 10,
				Height = 10,
			};

			Assert.ThrowsException<ItemOutOfContainerBoundsException>(() =>
				this.mockInventoryManager.Object.CanItemFitInContainerBoundsAt(6, 6, item, container));
		}

		[TestMethod]
		public void CanItemFitInContainerBoundsAt_ItemInBounds_NoExceptionThrown()
		{
			var item = new Item
			{
				Width = 5,
				Height = 5,
			};

			var container = new Container
			{
				Width = 10,
				Height = 10,
			};

			try
			{
				this.mockInventoryManager.Object.CanItemFitInContainerBoundsAt(0, 0, item, container);
			}
			catch (Exception ex)
			{
				Assert.Fail($"Expected no exception, but got {ex.GetType().Name}: {ex.Message}");
			}
		}

		[TestMethod]
		public void DoesItemCollideInContainerAt_ItemCollides_ThrowsItemOverlapException()
		{
			var item = new Item
			{
				Width = 2,
				Height = 2,
			};
			var container = new Container
			{
				Width = 10,
				Height = 10,
				Items = new List<Item>
				{
					new Item
					{
						X = 0,
						Y = 0,
						Width = 5,
						Height = 5
					}
				}
			};

			Assert.ThrowsException<ItemOverlapException>(() =>
				this.mockInventoryManager.Object.DoesItemCollideInContainerAt(4, 3, item, container));
			Assert.ThrowsException<ItemOverlapException>(() =>
				this.mockInventoryManager.Object.DoesItemCollideInContainerAt(3, 4, item, container));
			Assert.ThrowsException<ItemOverlapException>(() =>
				this.mockInventoryManager.Object.DoesItemCollideInContainerAt(4, 4, item, container));
		}

		[TestMethod]
		public void DoesItemCollideInContainerAt_ItemDoesNotCollide_NoExceptionThrown()
		{
			var item = new Item
			{
				Width = 2,
				Height = 2,
			};
			var container = new Container
			{
				Width = 10,
				Height = 10,
				Items = new List<Item>
				{
					new Item
					{
						X = 0,
						Y = 0,
						Width = 5,
						Height = 5
					}
				}
			};
			try
			{
				this.mockInventoryManager.Object.DoesItemCollideInContainerAt(4, 5, item, container);
				this.mockInventoryManager.Object.DoesItemCollideInContainerAt(5, 4, item, container);
				this.mockInventoryManager.Object.DoesItemCollideInContainerAt(5, 5, item, container);
				this.mockInventoryManager.Object.DoesItemCollideInContainerAt(5, 0, item, container);
			}
			catch (Exception ex)
			{
				Assert.Fail($"Expected no exception, but got {ex.GetType().Name}: {ex.Message}");
			}
		}

		[TestMethod]
		public void CanItemFitInContainerAt_ItemExceedsWeight_ThrowsMaxWeightExceededException()
		{
			var item = new Item
			{
				Id = GuidGenerator.GenerateTimeBasedGuid(),
				Weight = 10
			};
			var container = new Container
			{
				Id = GuidGenerator.GenerateTimeBasedGuid(),
				MaxWeight = 5
			};

			var set = new Mock<DbSet<Container>>().SetupData(new[] { container });
			this.mockStorageContext.Setup(c => c.Containers).Returns(set.Object);

			Assert.ThrowsException<MaxWeightExceededException>(() =>
				this.mockInventoryManager.Object.CanItemFitInContainerAt(0, 0, item, container));
		}

		[TestMethod]
		public void CanItemFitInContainerAt_ItemOutOfBounds_ThrowsItemOutOfContainerBoundsException()
		{
			var item = new Item
			{
				Id = GuidGenerator.GenerateTimeBasedGuid(),
				Width = 5,
				Height = 5,
			};
			var container = new Container
			{
				Id = GuidGenerator.GenerateTimeBasedGuid(),
				Width = 10,
				Height = 10,
			};

			var set = new Mock<DbSet<Container>>().SetupData(new[] { container });
			this.mockStorageContext.Setup(c => c.Containers).Returns(set.Object);

			Assert.ThrowsException<ItemOutOfContainerBoundsException>(() =>
				this.mockInventoryManager.Object.CanItemFitInContainerAt(6, 6, item, container));
		}

		[TestMethod]
		public void CanItemFitInContainerAt_ItemCollides_ThrowsItemOverlapException()
		{
			var item = new Item
			{
				Id = GuidGenerator.GenerateTimeBasedGuid(),
				Width = 2,
				Height = 2,
			};
			var container = new Container
			{
				Id = GuidGenerator.GenerateTimeBasedGuid(),
				Width = 10,
				Height = 10,
				Items = new List<Item>
				{
					new Item
					{
						Id = GuidGenerator.GenerateTimeBasedGuid(),
						X = 0,
						Y = 0,
						Width = 5,
						Height = 5
					}
				}
			};

			var set = new Mock<DbSet<Container>>().SetupData(new[] { container });
			this.mockStorageContext.Setup(c => c.Containers).Returns(set.Object);

			Assert.ThrowsException<ItemOverlapException>(() =>
				this.mockInventoryManager.Object.CanItemFitInContainerAt(4, 3, item, container));
			Assert.ThrowsException<ItemOverlapException>(() =>
				this.mockInventoryManager.Object.CanItemFitInContainerAt(3, 4, item, container));
		}

		[TestMethod]
		[DataRow(1, 1, 2)]
		[DataRow(2, 1, 2)]
		[DataRow(3, 3, 2)]
		[DataRow(7, 7, 2)]
		public void MoveItemWithinContainer_ItemFits_ItemIsMoved(int moveToX, int moveToY, int itemSize)
		{
			var container = new Container
			{
				Id = GuidGenerator.GenerateTimeBasedGuid(),
				Width = 10,
				Height = 10,
				Items = new List<Item>(),
			};
			var item = new Item
			{
				Id = GuidGenerator.GenerateTimeBasedGuid(),
                ContainerId = container.Id,
				X = 0,
				Y = 0,
				Width = itemSize,
				Height = itemSize
			};
            container.Items.Add(item);
            container.Items.Add(new Item
            {
	            Id = GuidGenerator.GenerateTimeBasedGuid(),
	            ContainerId = container.Id,
	            X = 5,
	            Y = 5,
	            Width = 2,
	            Height = 2
            });

			this.mockStorageContext.Setup(c => c.Containers).Returns(new Mock<MockableDbSetWithExtensions<Container>>().SetupData(new[] { container }).Object);
			var itemSet = new Mock<MockableDbSetWithExtensions<Item>>().SetupData(new[] {item});
			this.mockStorageContext.Setup(c => c.Items).Returns(itemSet.Object);

			try
			{
				this.mockInventoryManager.Object.MoveItemWithinContainer(item.Id, container.Id, moveToX, moveToY);
			}
			catch (Exception ex)
			{
				Assert.Fail($"Expected no exception, but got {ex.GetType().Name}: {ex.Message}");
			}
            
			itemSet.Verify(s => s.AddOrUpdate(item), Times.Once());
            Assert.AreEqual(item.X, moveToX);
            Assert.AreEqual(item.Y, moveToY);
		}

		[TestMethod]
        [DataRow(5, 5)]
		[DataRow(5, 6)]
		[DataRow(6, 5)]
		[DataRow(6, 6)]
		public void MoveItemWithinContainer_ItemCollides_ThrowsItemOverlapException(int moveToX, int moveToY)
		{
			var container = new Container
			{
				Id = GuidGenerator.GenerateTimeBasedGuid(),
				Width = 10,
				Height = 10,
				Items = new List<Item>(),
			};
			var itemToMove = new Item
			{
				Id = GuidGenerator.GenerateTimeBasedGuid(),
				ContainerId = container.Id,
				X = 0,
				Y = 0,
				Width = 2,
				Height = 2
			};
			var itemToCollide = new Item
			{
				Id = GuidGenerator.GenerateTimeBasedGuid(),
				ContainerId = container.Id,
				X = 5,
				Y = 5,
				Width = 2,
				Height = 2
			};
			var items = new List<Item>
			{
				itemToMove,
				itemToCollide,
			};
			container.Items = items;

			this.mockStorageContext.Setup(c => c.Containers).Returns(new Mock<MockableDbSetWithExtensions<Container>>().SetupData(new[] { container }).Object);
			var itemSet = new Mock<MockableDbSetWithExtensions<Item>>().SetupData();
			this.mockStorageContext.Setup(c => c.Items).Returns(itemSet.Object);

			Assert.ThrowsException<ItemOverlapException>(() =>
				this.mockInventoryManager.Object.MoveItemWithinContainer(itemToMove.Id, container.Id, moveToX, moveToY));
		}
	}
}
