using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace SpaceShooter {
	class Player : PhysicalObject{
		int points = 0;
		int oldPoints = 0;
		int highScore = 0;
		List<Bullet> bullets; // Ammunition
		Texture2D bulletTexture; // 2D Sprite of bullet textures
		double timeSinceLastBullet = 0;

		public int OldPoins { get { return oldPoints; } }
		public int HighScore { get { return highScore; } }
		public int Points { get { return points; } set { points = value; } }
		public List<Bullet> Bullets { get { return bullets; } }

		//Contructor
		public Player(Texture2D texture, float positionX, float positionY, float speedX, float speedY, Texture2D bulletTexture) : base(texture, positionX, positionY, speedX, speedY){
			bullets = new List<Bullet>();
			this.bulletTexture = bulletTexture;
		}

		//Update(), receives keyboard inputs and tracks movement.
		public void Update(GameWindow window, GameTime gameTime) {

			//Read keyboard inputs
			KeyboardState keyboardState = Keyboard.GetState();

			//Press esc to quit game to menu
			if (keyboardState.IsKeyDown(Keys.Escape))
				isAlive = false;

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

			// Player bullet controls
			if (keyboardState.IsKeyDown(Keys.Space)) {
				//Check if the player can shoot
				if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + 200) {
					bullets.Add(new Bullet(bulletTexture, position.X + texture.Width / 2, position.Y));
					//Reset timeSinceLastBullet
					timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
				}
			}

			// Move bullets
			foreach(Bullet b in bullets.ToList()) {
				b.Update();
				//Remove bullet if it's dead
				if (!b.IsAlive)
					bullets.Remove(b);
			}

		}

		public override void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(texture, position, Color.White);
			foreach (Bullet b in bullets)
				b.Draw(spriteBatch);
		}

		public void Reset(float positionX, float positionY, float speedX, float speedY) {
			//Reset players position and speed
			position.X = positionX;
			position.Y = positionY;
			speed.X = speedX;
			speed.Y = speedY;

			//Reset all bullets
			bullets.Clear();
			timeSinceLastBullet = 0;
			//Reset player points
			oldPoints = points;
			if (points > highScore)
				highScore = points;
			points = 0;
			//Respawn player
			isAlive = true;

		}

		public void SaveToFile(string fileName) {
			StreamWriter sw = new StreamWriter(fileName);
			sw.WriteLine(highScore);
			sw.Close();
		}

		public void LoadFromFile(string fileName) {
			StreamReader sr = new StreamReader(fileName);
			string score = sr.ReadLine();
			if (score == null || score == "")
				highScore = 0;
			else
				highScore = Convert.ToInt32(score);
			sr.Close();
		}
		
	}

	class Bullet : PhysicalObject {

		public Bullet(Texture2D texture, float positionX, float positionY) : base(texture, positionX, positionY, 0, 5f) {
		}

		public void Update() {
			position.Y -= speed.Y;
			if (position.Y < 0)
				isAlive = false;
		}
	}
}
