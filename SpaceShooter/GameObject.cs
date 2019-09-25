using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter {
	class GameObject {
		protected Texture2D texture; //Object textures
		protected Vector2 position; //Object position

		//GameObject constructor
		public GameObject(Texture2D texture, float positionX, float positionY) {
			this.texture = texture;
			this.position.X = positionX;
			this.position.Y = positionY;
		}

		//Draw texture on screen
		public void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(texture, position, Color.White);
		}

		//Properties
		public float X { get { return position.X; } }
		public float Y { get { return position.Y; } }
		public float Width { get { return texture.Width; } }
		public float Height { get { return texture.Height; } }

	}
}
