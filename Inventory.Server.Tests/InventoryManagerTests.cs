using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
		public void CanItemFitInContainerAt_ItemCollides_ThrowsMaxWeightExceededException()
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

	}
}
