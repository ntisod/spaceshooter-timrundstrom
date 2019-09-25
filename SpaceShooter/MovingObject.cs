using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter {
	class MovingObject : GameObject{
		protected Vector2 speed; //Object movement speed

		//MovingObject contructor
		public MovingObject(Texture2D texture, float positionX, float positionY, float speedX, float speedY) : base(texture, positionX, positionY) {
			this.speed.X = speedX;
			this.speed.Y = speedY;
		}
	}
}
