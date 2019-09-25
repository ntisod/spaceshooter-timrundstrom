using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter {
	class Player{
		Texture2D texture; // Spaceship textures
		Vector2 position; // Spaceship coordinates
		Vector2 speed; // Spaceship movement speeds

		//Contructor
		public Player(Texture2D texture, float positionX, float positionY, float speedX, float speedY) {
			this.texture = texture;
			this.position.X = positionX;
			this.position.Y = positionY;
			this.speed.X = speedX;
			this.speed.Y = speedY;
		}

		//Update(), receives keyboard inputs and tracks movement.
		public void Update(GameWindow window) {
			//Controls
			KeyboardState keyboardState = Keyboard.GetState();

			//Move the ship if it's within bounds on X axis.
			if (position.X <= window.ClientBounds.Width - texture.Width && position.X >= 0) {
				if (keyboardState.IsKeyDown(Keys.Right))
					position.X += speed.X;
				if (keyboardState.IsKeyDown(Keys.Left))
					position.X -= speed.X;
			}
			//Move the ship if it's within bounds on Y axis.
			if (position.Y <= window.ClientBounds.Height - texture.Height && position.Y >= 0) {
				if (keyboardState.IsKeyDown(Keys.Down))
					position.Y += speed.Y;
				if (keyboardState.IsKeyDown(Keys.Up))
					position.Y -= speed.Y;
			}

			//Reset ship position if it's out of bounds.
			//Too much to the left:
			if (position.X < 0)
				position.X = 0;
			//Too much to the right:
			if (position.X > window.ClientBounds.Width - texture.Width)
				position.X = window.ClientBounds.Width - texture.Width;
			//Too high up:
			if (position.Y < 0)
				position.Y = 0;
			//Too low down:
			if (position.Y > window.ClientBounds.Height - texture.Height)
				position.Y = window.ClientBounds.Height - texture.Height;
		}

		public void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(texture, position, Color.White);
		}
	}
}
