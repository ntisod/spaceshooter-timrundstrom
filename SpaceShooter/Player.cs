using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter {
	class Player : PhysicalObject{
		int points = 0;

		public int Points { get { return points; } set { points = value; } }

		//Contructor
		public Player(Texture2D texture, float positionX, float positionY, float speedX, float speedY) : base(texture, positionX, positionY, speedX, speedY){
		}

		//Update(), receives keyboard inputs and tracks movement.
		public void Update(GameWindow window) {

			//Read keyboard inputs
			KeyboardState keyboardState = Keyboard.GetState();

			//Move spaceship & stop at border
			if (position.X <= window.ClientBounds.Width - texture.Width && position.X >= 0){
				if (keyboardState.IsKeyDown(Keys.Right))
					position.X += speed.X;
				if (keyboardState.IsKeyDown(Keys.Left))
					position.X -= speed.X;
			}
			if (position.Y <= window.ClientBounds.Height - texture.Height && position.Y >= 0) {
				if (keyboardState.IsKeyDown(Keys.Down))
					position.Y += speed.Y;
				if (keyboardState.IsKeyDown(Keys.Up))
					position.Y -= speed.Y;
			}

			//Check if spaceship is out of bounds and if so, reset its position
			//Too much left
			if (position.X < 0)
				position.X = 0;
			//Too much right
			if (position.X > window.ClientBounds.Width - texture.Width)
				position.X = window.ClientBounds.Width - texture.Width;
			//Too high up
			if (position.Y < 0)
				position.Y = 0;
			//Too low down
			if (position.Y > window.ClientBounds.Height - texture.Height)
				position.Y = window.ClientBounds.Height - texture.Height;


		}
		
	}
}
