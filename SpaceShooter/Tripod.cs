using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter {
	class Tripod : Enemy{

		public Tripod(Texture2D texture, float positionX, float positionY) : base(texture, positionX, positionY, 3f, 1f) {
		}

		public override void Update(GameWindow window) {
			//Move enemy
			position.X += speed.X;
			//Bounce on the sides
			if (position.X > window.ClientBounds.Width - texture.Width || position.X < 0)
				speed.X *= -1;
			position.Y += speed.Y;
			//Kill enemy if out of bounds
			if (position.Y > window.ClientBounds.Height)
				isAlive = false;
		}


	}
}
