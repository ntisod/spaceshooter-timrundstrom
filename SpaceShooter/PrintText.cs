using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter {
	class PrintText {
		SpriteFont font;

		public PrintText(SpriteFont font) {
			this.font = font;
		}
		
		public void Print(string text, SpriteBatch spriteBatch, int positionX, int positionY) {
			spriteBatch.DrawString(font, text, new Vector2(positionX, positionY), Color.White);
		}
	}
}
