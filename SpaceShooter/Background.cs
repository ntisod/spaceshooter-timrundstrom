using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter {

	class BackgroundSprite : GameObject {

		public BackgroundSprite(Texture2D texture, float positionX, float positionY) : base (texture, positionX, positionY) {
		}

		public void Update(GameWindow window, int nrBackgroundsY) {
			position.Y += 2f; //Move background
			//Move position if background is below window
			if (position.Y > window.ClientBounds.Height)
				position.Y = position.Y - nrBackgroundsY * texture.Height; //Place it on top
		}
	}

	class Background {

		BackgroundSprite[,] backgroundSprites;
		int nrBackgroundsX, nrBackgroundsY;

		public Background(Texture2D texture, GameWindow window) {
			//Set amount of sprites on the width
			double tmpX = (double)window.ClientBounds.Width / texture.Width;
			nrBackgroundsX = (int)Math.Ceiling(tmpX);
			//Set amount of sprites on the height
			double tmpY = (double)window.ClientBounds.Height / texture.Height;
			nrBackgroundsY = (int)Math.Ceiling(tmpY) + 1;

			//Set array size
			backgroundSprites = new BackgroundSprite[nrBackgroundsX, nrBackgroundsY];
  
			//Fill array
			for (int i = 0; i < nrBackgroundsX; i++) {
				for (int j = 0; j < nrBackgroundsY; j++) {
					int posX = i * texture.Width;
					//Set the top row above the window
					int posY = j * texture.Height - texture.Height;
					backgroundSprites[i, j] = new BackgroundSprite(texture, posX, posY);
				}
			}

		}

		public void Update(GameWindow window) {
			for (int i = 0; i < nrBackgroundsX; i++) {
				for (int j = 0; j < nrBackgroundsY; j++) {
					backgroundSprites[i, j].Update(window, nrBackgroundsY);
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch) {
			for (int i = 0; i < nrBackgroundsX; i++) {
				for (int j = 0; j < nrBackgroundsY; j++) {
					backgroundSprites[i, j].Draw(spriteBatch);
				}
			}
		}

	}
}
