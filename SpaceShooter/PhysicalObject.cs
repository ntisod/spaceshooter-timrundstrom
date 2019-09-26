using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter {
	abstract class PhysicalObject : MovingObject {
		protected bool isAlive = true;

		public PhysicalObject(Texture2D texture, float positionX, float positionY, float speedX, float speedY) : base(texture, positionX, positionY, speedX, speedY) {
		}

		public bool CheckCollision(PhysicalObject other) {
			Rectangle myRect = new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), Convert.ToInt32(Width), Convert.ToInt32(Height));
			Rectangle otherRect = new Rectangle(Convert.ToInt32(other.position.X), Convert.ToInt32(other.position.Y), Convert.ToInt32(other.Width), Convert.ToInt32(other.Width));
			return myRect.Intersects(otherRect);
		}

		public bool IsAlive {
			get { return isAlive; }
			set { isAlive = value; }
		}

	}
}
