using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Logic.UI.UIAtlas
{
	public class UTexturePacker
	{
		public UTexturePacker(int width, int height, bool rotations)
		{
			this.Init(width, height, rotations);
		}

		public void Init(int width, int height, bool rotations)
		{
			this.binWidth = width;
			this.binHeight = height;
			this.allowRotations = rotations;
			Rect rect = default(Rect);
			rect.x = 0f;
			rect.y = 0f;
			rect.width = (float)width;
			rect.height = (float)height;
			this.usedRectangles.Clear();
			this.freeRectangles.Clear();
			this.freeRectangles.Add(rect);
		}

		public static Rect[] PackTextures(Texture2D texture, Texture2D[] textures, int width, int height, int padding, int maxSize, bool forceSquareAtlas = true)
		{
			if (width > maxSize && height > maxSize)
			{
				return null;
			}
			if (width > maxSize || height > maxSize)
			{
				int num = width;
				width = height;
				height = num;
			}
			if (forceSquareAtlas)
			{
				if (width > height)
				{
					height = width;
				}
				else if (height > width)
				{
					width = height;
				}
			}
			UTexturePacker utexturePacker = new UTexturePacker(width, height, false);
			UTexturePacker.Storage[] array = new UTexturePacker.Storage[textures.Length];
			for (int i = 0; i < textures.Length; i++)
			{
				Texture2D texture2D = textures[i];
				if (texture2D)
				{
					Rect rect = default(Rect);
					int j = 1;
					int k;
					for (k = 1; k >= 0; k--)
					{
						for (j = 1; j >= 0; j--)
						{
							rect = utexturePacker.Insert(texture2D.width + k * padding, texture2D.height + j * padding, UTexturePacker.FreeRectChoiceHeuristic.RectBestAreaFit);
							if (rect.width != 0f && rect.height != 0f)
							{
								break;
							}
							if (k == 0 && j == 0)
							{
								return UTexturePacker.PackTextures(texture, textures, width * ((width <= height) ? 2 : 1), height * ((height < width) ? 2 : 1), padding, maxSize, true);
							}
						}
						if (rect.width != 0f && rect.height != 0f)
						{
							break;
						}
					}
					array[i] = default(UTexturePacker.Storage);
					array[i].rect = rect;
					array[i].paddingX = k != 0;
					array[i].paddingY = j != 0;
				}
			}
			texture.Reinitialize(width, height);
			texture.SetPixels(new Color[width * height]);
			Rect[] array2 = new Rect[textures.Length];
			for (int l = 0; l < textures.Length; l++)
			{
				Texture2D texture2D2 = textures[l];
				if (texture2D2)
				{
					Rect rect2 = array[l].rect;
					int num2 = (array[l].paddingX ? padding : 0);
					int num3 = (array[l].paddingY ? padding : 0);
					Color[] array3 = texture2D2.GetPixels();
					if (rect2.width != (float)(texture2D2.width + num2))
					{
						Color[] pixels = texture2D2.GetPixels();
						int num4 = 0;
						while ((float)num4 < rect2.width)
						{
							int num5 = 0;
							while ((float)num5 < rect2.height)
							{
								int num6 = (int)rect2.height - (num5 + 1) + num4 * texture2D2.width;
								pixels[num4 + num5 * (int)rect2.width] = array3[num6];
								num5++;
							}
							num4++;
						}
						array3 = pixels;
					}
					texture.SetPixels((int)rect2.x, (int)rect2.y, (int)rect2.width - num2, (int)rect2.height - num3, array3);
					rect2.x /= (float)width;
					rect2.y /= (float)height;
					rect2.width = (rect2.width - (float)num2) / (float)width;
					rect2.height = (rect2.height - (float)num3) / (float)height;
					array2[l] = rect2;
				}
			}
			texture.Apply();
			return array2;
		}

		public Rect Insert(int width, int height, UTexturePacker.FreeRectChoiceHeuristic method)
		{
			Rect rect = default(Rect);
			int num = 0;
			int num2 = 0;
			switch (method)
			{
			case UTexturePacker.FreeRectChoiceHeuristic.RectBestShortSideFit:
				rect = this.FindPositionForNewNodeBestShortSideFit(width, height, ref num, ref num2);
				break;
			case UTexturePacker.FreeRectChoiceHeuristic.RectBestLongSideFit:
				rect = this.FindPositionForNewNodeBestLongSideFit(width, height, ref num2, ref num);
				break;
			case UTexturePacker.FreeRectChoiceHeuristic.RectBestAreaFit:
				rect = this.FindPositionForNewNodeBestAreaFit(width, height, ref num, ref num2);
				break;
			case UTexturePacker.FreeRectChoiceHeuristic.RectBottomLeftRule:
				rect = this.FindPositionForNewNodeBottomLeft(width, height, ref num, ref num2);
				break;
			case UTexturePacker.FreeRectChoiceHeuristic.RectContactPointRule:
				rect = this.FindPositionForNewNodeContactPoint(width, height, ref num);
				break;
			}
			if (rect.height == 0f)
			{
				return rect;
			}
			int num3 = this.freeRectangles.Count;
			for (int i = 0; i < num3; i++)
			{
				if (this.SplitFreeNode(this.freeRectangles[i], ref rect))
				{
					this.freeRectangles.RemoveAt(i);
					i--;
					num3--;
				}
			}
			this.PruneFreeList();
			this.usedRectangles.Add(rect);
			return rect;
		}

		public void Insert(List<Rect> rects, List<Rect> dst, UTexturePacker.FreeRectChoiceHeuristic method)
		{
			dst.Clear();
			while (rects.Count > 0)
			{
				int num = int.MaxValue;
				int num2 = int.MaxValue;
				int num3 = -1;
				Rect rect = default(Rect);
				for (int i = 0; i < rects.Count; i++)
				{
					int num4 = 0;
					int num5 = 0;
					Rect rect2 = this.ScoreRect((int)rects[i].width, (int)rects[i].height, method, ref num4, ref num5);
					if (num4 < num || (num4 == num && num5 < num2))
					{
						num = num4;
						num2 = num5;
						rect = rect2;
						num3 = i;
					}
				}
				if (num3 == -1)
				{
					return;
				}
				this.PlaceRect(rect);
				rects.RemoveAt(num3);
			}
		}

		private void PlaceRect(Rect node)
		{
			int num = this.freeRectangles.Count;
			for (int i = 0; i < num; i++)
			{
				if (this.SplitFreeNode(this.freeRectangles[i], ref node))
				{
					this.freeRectangles.RemoveAt(i);
					i--;
					num--;
				}
			}
			this.PruneFreeList();
			this.usedRectangles.Add(node);
		}

		private Rect ScoreRect(int width, int height, UTexturePacker.FreeRectChoiceHeuristic method, ref int score1, ref int score2)
		{
			Rect rect = default(Rect);
			score1 = int.MaxValue;
			score2 = int.MaxValue;
			switch (method)
			{
			case UTexturePacker.FreeRectChoiceHeuristic.RectBestShortSideFit:
				rect = this.FindPositionForNewNodeBestShortSideFit(width, height, ref score1, ref score2);
				break;
			case UTexturePacker.FreeRectChoiceHeuristic.RectBestLongSideFit:
				rect = this.FindPositionForNewNodeBestLongSideFit(width, height, ref score2, ref score1);
				break;
			case UTexturePacker.FreeRectChoiceHeuristic.RectBestAreaFit:
				rect = this.FindPositionForNewNodeBestAreaFit(width, height, ref score1, ref score2);
				break;
			case UTexturePacker.FreeRectChoiceHeuristic.RectBottomLeftRule:
				rect = this.FindPositionForNewNodeBottomLeft(width, height, ref score1, ref score2);
				break;
			case UTexturePacker.FreeRectChoiceHeuristic.RectContactPointRule:
				rect = this.FindPositionForNewNodeContactPoint(width, height, ref score1);
				score1 = -score1;
				break;
			}
			if (rect.height == 0f)
			{
				score1 = int.MaxValue;
				score2 = int.MaxValue;
			}
			return rect;
		}

		public float Occupancy()
		{
			ulong num = 0UL;
			for (int i = 0; i < this.usedRectangles.Count; i++)
			{
				num += (ulong)((uint)this.usedRectangles[i].width * (uint)this.usedRectangles[i].height);
			}
			return num / (float)(this.binWidth * this.binHeight);
		}

		private Rect FindPositionForNewNodeBottomLeft(int width, int height, ref int bestY, ref int bestX)
		{
			Rect rect = default(Rect);
			bestY = int.MaxValue;
			for (int i = 0; i < this.freeRectangles.Count; i++)
			{
				if (this.freeRectangles[i].width >= (float)width && this.freeRectangles[i].height >= (float)height)
				{
					int num = (int)this.freeRectangles[i].y + height;
					if (num < bestY || (num == bestY && this.freeRectangles[i].x < (float)bestX))
					{
						rect.x = this.freeRectangles[i].x;
						rect.y = this.freeRectangles[i].y;
						rect.width = (float)width;
						rect.height = (float)height;
						bestY = num;
						bestX = (int)this.freeRectangles[i].x;
					}
				}
				if (this.allowRotations && this.freeRectangles[i].width >= (float)height && this.freeRectangles[i].height >= (float)width)
				{
					int num2 = (int)this.freeRectangles[i].y + width;
					if (num2 < bestY || (num2 == bestY && this.freeRectangles[i].x < (float)bestX))
					{
						rect.x = this.freeRectangles[i].x;
						rect.y = this.freeRectangles[i].y;
						rect.width = (float)height;
						rect.height = (float)width;
						bestY = num2;
						bestX = (int)this.freeRectangles[i].x;
					}
				}
			}
			return rect;
		}

		private Rect FindPositionForNewNodeBestShortSideFit(int width, int height, ref int bestShortSideFit, ref int bestLongSideFit)
		{
			Rect rect = default(Rect);
			bestShortSideFit = int.MaxValue;
			for (int i = 0; i < this.freeRectangles.Count; i++)
			{
				if (this.freeRectangles[i].width >= (float)width && this.freeRectangles[i].height >= (float)height)
				{
					int num = Mathf.Abs((int)this.freeRectangles[i].width - width);
					int num2 = Mathf.Abs((int)this.freeRectangles[i].height - height);
					int num3 = Mathf.Min(num, num2);
					int num4 = Mathf.Max(num, num2);
					if (num3 < bestShortSideFit || (num3 == bestShortSideFit && num4 < bestLongSideFit))
					{
						rect.x = this.freeRectangles[i].x;
						rect.y = this.freeRectangles[i].y;
						rect.width = (float)width;
						rect.height = (float)height;
						bestShortSideFit = num3;
						bestLongSideFit = num4;
					}
				}
				if (this.allowRotations && this.freeRectangles[i].width >= (float)height && this.freeRectangles[i].height >= (float)width)
				{
					int num5 = Mathf.Abs((int)this.freeRectangles[i].width - height);
					int num6 = Mathf.Abs((int)this.freeRectangles[i].height - width);
					int num7 = Mathf.Min(num5, num6);
					int num8 = Mathf.Max(num5, num6);
					if (num7 < bestShortSideFit || (num7 == bestShortSideFit && num8 < bestLongSideFit))
					{
						rect.x = this.freeRectangles[i].x;
						rect.y = this.freeRectangles[i].y;
						rect.width = (float)height;
						rect.height = (float)width;
						bestShortSideFit = num7;
						bestLongSideFit = num8;
					}
				}
			}
			return rect;
		}

		private Rect FindPositionForNewNodeBestLongSideFit(int width, int height, ref int bestShortSideFit, ref int bestLongSideFit)
		{
			Rect rect = default(Rect);
			bestLongSideFit = int.MaxValue;
			for (int i = 0; i < this.freeRectangles.Count; i++)
			{
				if (this.freeRectangles[i].width >= (float)width && this.freeRectangles[i].height >= (float)height)
				{
					int num = Mathf.Abs((int)this.freeRectangles[i].width - width);
					int num2 = Mathf.Abs((int)this.freeRectangles[i].height - height);
					int num3 = Mathf.Min(num, num2);
					int num4 = Mathf.Max(num, num2);
					if (num4 < bestLongSideFit || (num4 == bestLongSideFit && num3 < bestShortSideFit))
					{
						rect.x = this.freeRectangles[i].x;
						rect.y = this.freeRectangles[i].y;
						rect.width = (float)width;
						rect.height = (float)height;
						bestShortSideFit = num3;
						bestLongSideFit = num4;
					}
				}
				if (this.allowRotations && this.freeRectangles[i].width >= (float)height && this.freeRectangles[i].height >= (float)width)
				{
					int num5 = Mathf.Abs((int)this.freeRectangles[i].width - height);
					int num6 = Mathf.Abs((int)this.freeRectangles[i].height - width);
					int num7 = Mathf.Min(num5, num6);
					int num8 = Mathf.Max(num5, num6);
					if (num8 < bestLongSideFit || (num8 == bestLongSideFit && num7 < bestShortSideFit))
					{
						rect.x = this.freeRectangles[i].x;
						rect.y = this.freeRectangles[i].y;
						rect.width = (float)height;
						rect.height = (float)width;
						bestShortSideFit = num7;
						bestLongSideFit = num8;
					}
				}
			}
			return rect;
		}

		private Rect FindPositionForNewNodeBestAreaFit(int width, int height, ref int bestAreaFit, ref int bestShortSideFit)
		{
			Rect rect = default(Rect);
			bestAreaFit = int.MaxValue;
			for (int i = 0; i < this.freeRectangles.Count; i++)
			{
				int num = (int)this.freeRectangles[i].width * (int)this.freeRectangles[i].height - width * height;
				if (this.freeRectangles[i].width >= (float)width && this.freeRectangles[i].height >= (float)height)
				{
					int num2 = Mathf.Abs((int)this.freeRectangles[i].width - width);
					int num3 = Mathf.Abs((int)this.freeRectangles[i].height - height);
					int num4 = Mathf.Min(num2, num3);
					if (num < bestAreaFit || (num == bestAreaFit && num4 < bestShortSideFit))
					{
						rect.x = this.freeRectangles[i].x;
						rect.y = this.freeRectangles[i].y;
						rect.width = (float)width;
						rect.height = (float)height;
						bestShortSideFit = num4;
						bestAreaFit = num;
					}
				}
				if (this.allowRotations && this.freeRectangles[i].width >= (float)height && this.freeRectangles[i].height >= (float)width)
				{
					int num5 = Mathf.Abs((int)this.freeRectangles[i].width - height);
					int num6 = Mathf.Abs((int)this.freeRectangles[i].height - width);
					int num7 = Mathf.Min(num5, num6);
					if (num < bestAreaFit || (num == bestAreaFit && num7 < bestShortSideFit))
					{
						rect.x = this.freeRectangles[i].x;
						rect.y = this.freeRectangles[i].y;
						rect.width = (float)height;
						rect.height = (float)width;
						bestShortSideFit = num7;
						bestAreaFit = num;
					}
				}
			}
			return rect;
		}

		private int CommonIntervalLength(int i1start, int i1end, int i2start, int i2end)
		{
			if (i1end < i2start || i2end < i1start)
			{
				return 0;
			}
			return Mathf.Min(i1end, i2end) - Mathf.Max(i1start, i2start);
		}

		private int ContactPointScoreNode(int x, int y, int width, int height)
		{
			int num = 0;
			if (x == 0 || x + width == this.binWidth)
			{
				num += height;
			}
			if (y == 0 || y + height == this.binHeight)
			{
				num += width;
			}
			for (int i = 0; i < this.usedRectangles.Count; i++)
			{
				if (this.usedRectangles[i].x == (float)(x + width) || this.usedRectangles[i].x + this.usedRectangles[i].width == (float)x)
				{
					num += this.CommonIntervalLength((int)this.usedRectangles[i].y, (int)this.usedRectangles[i].y + (int)this.usedRectangles[i].height, y, y + height);
				}
				if (this.usedRectangles[i].y == (float)(y + height) || this.usedRectangles[i].y + this.usedRectangles[i].height == (float)y)
				{
					num += this.CommonIntervalLength((int)this.usedRectangles[i].x, (int)this.usedRectangles[i].x + (int)this.usedRectangles[i].width, x, x + width);
				}
			}
			return num;
		}

		private Rect FindPositionForNewNodeContactPoint(int width, int height, ref int bestContactScore)
		{
			Rect rect = default(Rect);
			bestContactScore = -1;
			for (int i = 0; i < this.freeRectangles.Count; i++)
			{
				if (this.freeRectangles[i].width >= (float)width && this.freeRectangles[i].height >= (float)height)
				{
					int num = this.ContactPointScoreNode((int)this.freeRectangles[i].x, (int)this.freeRectangles[i].y, width, height);
					if (num > bestContactScore)
					{
						rect.x = (float)((int)this.freeRectangles[i].x);
						rect.y = (float)((int)this.freeRectangles[i].y);
						rect.width = (float)width;
						rect.height = (float)height;
						bestContactScore = num;
					}
				}
				if (this.allowRotations && this.freeRectangles[i].width >= (float)height && this.freeRectangles[i].height >= (float)width)
				{
					int num2 = this.ContactPointScoreNode((int)this.freeRectangles[i].x, (int)this.freeRectangles[i].y, height, width);
					if (num2 > bestContactScore)
					{
						rect.x = (float)((int)this.freeRectangles[i].x);
						rect.y = (float)((int)this.freeRectangles[i].y);
						rect.width = (float)height;
						rect.height = (float)width;
						bestContactScore = num2;
					}
				}
			}
			return rect;
		}

		private bool SplitFreeNode(Rect freeNode, ref Rect usedNode)
		{
			if (usedNode.x >= freeNode.x + freeNode.width || usedNode.x + usedNode.width <= freeNode.x || usedNode.y >= freeNode.y + freeNode.height || usedNode.y + usedNode.height <= freeNode.y)
			{
				return false;
			}
			if (usedNode.x < freeNode.x + freeNode.width && usedNode.x + usedNode.width > freeNode.x)
			{
				if (usedNode.y > freeNode.y && usedNode.y < freeNode.y + freeNode.height)
				{
					Rect rect = freeNode;
					rect.height = usedNode.y - rect.y;
					this.freeRectangles.Add(rect);
				}
				if (usedNode.y + usedNode.height < freeNode.y + freeNode.height)
				{
					Rect rect2 = freeNode;
					rect2.y = usedNode.y + usedNode.height;
					rect2.height = freeNode.y + freeNode.height - (usedNode.y + usedNode.height);
					this.freeRectangles.Add(rect2);
				}
			}
			if (usedNode.y < freeNode.y + freeNode.height && usedNode.y + usedNode.height > freeNode.y)
			{
				if (usedNode.x > freeNode.x && usedNode.x < freeNode.x + freeNode.width)
				{
					Rect rect3 = freeNode;
					rect3.width = usedNode.x - rect3.x;
					this.freeRectangles.Add(rect3);
				}
				if (usedNode.x + usedNode.width < freeNode.x + freeNode.width)
				{
					Rect rect4 = freeNode;
					rect4.x = usedNode.x + usedNode.width;
					rect4.width = freeNode.x + freeNode.width - (usedNode.x + usedNode.width);
					this.freeRectangles.Add(rect4);
				}
			}
			return true;
		}

		private void PruneFreeList()
		{
			for (int i = 0; i < this.freeRectangles.Count; i++)
			{
				for (int j = i + 1; j < this.freeRectangles.Count; j++)
				{
					if (this.IsContainedIn(this.freeRectangles[i], this.freeRectangles[j]))
					{
						this.freeRectangles.RemoveAt(i);
						i--;
						break;
					}
					if (this.IsContainedIn(this.freeRectangles[j], this.freeRectangles[i]))
					{
						this.freeRectangles.RemoveAt(j);
						j--;
					}
				}
			}
		}

		private bool IsContainedIn(Rect a, Rect b)
		{
			return a.x >= b.x && a.y >= b.y && a.x + a.width <= b.x + b.width && a.y + a.height <= b.y + b.height;
		}

		public int binWidth;

		public int binHeight;

		public bool allowRotations;

		public List<Rect> usedRectangles = new List<Rect>();

		public List<Rect> freeRectangles = new List<Rect>();

		public enum FreeRectChoiceHeuristic
		{
			RectBestShortSideFit,
			RectBestLongSideFit,
			RectBestAreaFit,
			RectBottomLeftRule,
			RectContactPointRule
		}

		private struct Storage
		{
			public Rect rect;

			public bool paddingX;

			public bool paddingY;
		}
	}
}
