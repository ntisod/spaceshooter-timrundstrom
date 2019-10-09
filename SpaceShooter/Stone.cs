using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter {
	class Stone : Enemy {
		public Stone(Texture2D texture, float positionX, float positionY) : base(texture, positionX, positionY, 0f, 3f) {
		}

		public override void Update(GameWindow window) {
			//Move enemy
			position.Y += speed.Y;
			//Kill enemy if out of bounds
			if (position.Y > window.ClientBounds.Height)
				isAlive = false;
		}
	}
}
