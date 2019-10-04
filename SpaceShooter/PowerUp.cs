using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter {

	abstract class PowerUp : PhysicalObject {
		protected double timeToDie;

		public PowerUp(Texture2D texture, float positionX, float positionY, GameTime gameTime, double timeToDie) : base(texture, positionX, positionY, 0, 0) {
			this.timeToDie = gameTime.TotalGameTime.TotalMilliseconds + timeToDie;
		}

		public virtual void Update(GameTime gameTime) {
		}

	}

	class GoldCoin : PowerUp {

		public GoldCoin(Texture2D texture, float positionX, float positionY, GameTime gameTime, double timeToDie) : base(texture, positionX, positionY, gameTime, timeToDie) {
		}

		public override void Update(GameTime gameTime) {
			if (timeToDie < gameTime.TotalGameTime.TotalMilliseconds)
				isAlive = false;
		}

	}

	class Heart : PowerUp {

		public Heart(Texture2D texture, float positionX, float positionY, GameTime gameTime, double timeToDie) : base(texture, positionX, positionY, gameTime, timeToDie) {
		}

		public override void Update(GameTime gameTime) {
			if (timeToDie < gameTime.TotalGameTime.TotalMilliseconds)
				isAlive = false;
		}
	}

	class Upgrade : PowerUp {

		public Upgrade(Texture2D texture, float positionX, float positionY, GameTime gameTime, double timeToDie) : base(texture, positionX, positionY, gameTime, timeToDie) {
		}
		public override void Update(GameTime gameTime) {
			if (timeToDie < gameTime.TotalGameTime.TotalMilliseconds)
				isAlive = false;
		}
	}
}
