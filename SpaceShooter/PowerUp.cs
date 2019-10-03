using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter {

	class GoldCoin : PhysicalObject {
		double timeToDie;

		public GoldCoin(Texture2D texture, float positionX, float positionY, GameTime gameTime) : base(texture, positionX, positionY, 0, 0) {
			timeToDie = gameTime.TotalGameTime.TotalMilliseconds + 5000;
		}

		public void Update(GameTime gameTime) {
			if (timeToDie < gameTime.TotalGameTime.TotalMilliseconds)
				isAlive = false;
		}

	}

	class Heart : PhysicalObject {
		double timeToDie;

		public Heart(Texture2D texture, float positionX, float positionY, GameTime gameTime) : base(texture, positionX, positionY, 0, 0) {
			timeToDie = gameTime.TotalGameTime.TotalMilliseconds + 7500;
		}
		public void Update(GameTime gameTime) {
			if (timeToDie < gameTime.TotalGameTime.TotalMilliseconds)
				isAlive = false;
		}
	}

	class Upgrade : PhysicalObject {
		double timeToDie;

		public Upgrade(Texture2D texture, float positionX, float positionY, GameTime gameTime) : base(texture, positionX, positionY, 0, 0) {
			timeToDie = gameTime.TotalGameTime.TotalMilliseconds + 7500;
		}
		public void Update(GameTime gameTime) {
			if (timeToDie < gameTime.TotalGameTime.TotalMilliseconds)
				isAlive = false;
		}
	}
}
