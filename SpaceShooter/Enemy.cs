using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter {
	class Enemy : PhysicalObject {

		public Enemy(Texture2D texture, float positionX, float positionY) : base(texture, positionX, positionY, 6f, 0.3f) {
		}

		public void Update(GameWindow window) {
			//Move the enemy
			position.X += speed.X;
			//Check if whithin bounds
			if (position.X > window.ClientBounds.Width - texture.Width || position.X < 0)
				speed.X *= -1; //Change direction
			position.Y += speed.Y;
			//Kill enemy if it reaches all the way down
			if (position.Y > window.ClientBounds.Height - texture.Height)
				isAlive = false;
		}
		


	}

}
