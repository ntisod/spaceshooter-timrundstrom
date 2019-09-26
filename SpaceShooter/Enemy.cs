using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter {
	abstract class Enemy : PhysicalObject {

		public Enemy(Texture2D texture, float positionX, float positionY, float speedX, float speedY) : base(texture, positionX, positionY, speedX, speedY){
		}

		public abstract void Update(GameWindow window);
		


	}

}
