using Microsoft.Xna.Framework.Graphics;

namespace RecoilRumble
{
	public interface Drawable
	{
		int X { get; set; }
		int Y { get; set; }
		Texture2D Sprite { get; set; }
		bool Visible { get; set; }
	}
}
