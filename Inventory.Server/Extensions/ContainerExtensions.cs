using System;
using IgiCore.Inventory.Server.Models;

namespace IgiCore.Inventory.Server.Extensions
{
	public static class ContainerExtensions
	{
		public static Guid[,] GetItemMatrix(this Container container)
		{
			var matrix = new Guid[container.Width, container.Height];

			for (var w = 0; w < container.Width; w++)
			{
				for (var h = 0; h < container.Height; h++)
				{
					matrix[w, h] = Guid.Empty;
				}
			}

			foreach (var item in container.Items)
			{
				var xStart = item.X ?? 0;
				var yStart = item.Y ?? 0;
				var xEnd = item.X + item.Width;
				var yEnd = item.Y + item.Height;

				for (var x = xStart; x < xEnd; x++)
				{
					for (var y = yStart; y < yEnd; y++)
					{
						matrix[x, y] = item.Id;
					}
				}
			}

			return matrix;
		}
	}
}
