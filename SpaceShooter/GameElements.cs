using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter {
	static class GameElements {

		//Variables
		static Texture2D menuSprite;
		static Vector2 menuPos;
		static Player player;
		static List<Enemy> enemies;
		static List<GoldCoin> goldCoins;
		static Texture2D goldCoinSprite;
		static PrintText printText;

		//Properties
		public enum State { Menu, Run, Highscore, Quit };
		public static State currentState;


		public static void Initialize() {
			goldCoins = new List<GoldCoin>();
		}

		public static void LoadContent(ContentManager content, GameWindow window) {
			//Load menu
			menuSprite = content.Load<Texture2D>("Sprites/menu");
			menuPos.X = window.ClientBounds.Width / 2 - menuSprite.Width / 2;
			menuPos.Y = window.ClientBounds.Height / 2 - menuSprite.Height / 2;
			//Load player
			player = new Player(content.Load<Texture2D>("Sprites/ship"), 380, 400, 2.5f, 4.5f, content.Load<Texture2D>("Sprites/bullet"));

			//Generate enemies
			enemies = new List<Enemy>();
			Random random = new Random();
			Texture2D tmpSprite = content.Load<Texture2D>("Sprites/mine");
			for (int i = 0; i < 5; i++) {
				int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
				int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
				enemies.Add(new Mine(tmpSprite, rndX, rndY));
			}
			tmpSprite = content.Load<Texture2D>("Sprites/Tripod");
			for (int i = 0; i < 5; i++) {
				int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
				int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
				enemies.Add(new Tripod(tmpSprite, rndX, rndY));
			}

			//Load in coins and points HUD
			goldCoinSprite = content.Load<Texture2D>("Sprites/coin");
			printText = new PrintText(content.Load<SpriteFont>("myFont"));
		}

		public static State MenuUpdate() {
			KeyboardState keyboardState = Keyboard.GetState();
			if (keyboardState.IsKeyDown(Keys.S)) //Start game
				return State.Run;
			if (keyboardState.IsKeyDown(Keys.H)) //Show Highscore
				return State.Highscore;
			if (keyboardState.IsKeyDown(Keys.A)) //Quit game
				return State.Quit;
			return State.Menu; //Default, stay in menu
		}

		public static void MenuDraw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(menuSprite, menuPos, Color.White);
		}

		public static State RunUpdate(ContentManager content, GameWindow window, GameTime gameTime) {
			//Update player position
			player.Update(window, gameTime);
			//Go through enemies
			foreach (Enemy e in enemies.ToList()) {
				//Check if player shoots an enemy
				foreach (Bullet b in player.Bullets) {
					if (e.CheckCollision(b)) {
						e.IsAlive = false;
						b.IsAlive = false;
						player.Points++;
					}
				}
				if (e.IsAlive) {
					//Check if enemy collides with player
					if (e.CheckCollision(player))
						player.IsAlive = false; //If so, kill player
					e.Update(window);
				} else
					enemies.Remove(e);
			}

			//Generate coins randomly
			Random random = new Random();
			int newCoin = random.Next(1, 200);
			if (newCoin == 1) {
				int rndx = random.Next(0, window.ClientBounds.Width - goldCoinSprite.Width);
				int rndY = random.Next(0, window.ClientBounds.Height - goldCoinSprite.Height);
				goldCoins.Add(new GoldCoin(goldCoinSprite, rndx, rndY, gameTime));
			}

			//Check if coin is collected by player
			foreach (GoldCoin gc in goldCoins.ToList()) {
				if (gc.IsAlive) {
					gc.Update(gameTime);
					if (gc.CheckCollision(player)) {
						goldCoins.Remove(gc);
						player.Points++;
					}
				} else
					goldCoins.Remove(gc);
			}

			if (!player.IsAlive) { //If player is dead, go to menu
				Reset(window, content);
				return State.Menu;
			}
			return State.Run; //Keep playing by default

		}

		public static void RunDraw(SpriteBatch spriteBatch) {
			//Draw player, enemies, goldcoins and points HUD to screen
			player.Draw(spriteBatch);
			foreach (Enemy e in enemies)
				e.Draw(spriteBatch);
			foreach (GoldCoin gc in goldCoins)
				gc.Draw(spriteBatch);
			printText.Print("Points: " + player.Points, spriteBatch, 0, 0);
		}

		public static State HighScoreUpdate() {
			KeyboardState keyboardState = Keyboard.GetState();
			// Go to menu if ESC is pressed
			if (keyboardState.IsKeyDown(Keys.Escape))
				return State.Menu;
			return State.Highscore;
		}

		public static void HighScoreDraw(SpriteBatch spriteBatch) {
			//Draw highscore-list
		}

		public static void GenerateEnemies(GameWindow window, ContentManager content) {
			//Generate new enemies
			enemies.Clear();
			Random random = new Random();
			Texture2D tmpSprite = content.Load<Texture2D>("Sprites/mine");
			for (int i = 0; i < 5; i++) {
				int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
				int rndY = random.Next(0, window.ClientBounds.Height / 2);
				enemies.Add(new Mine(tmpSprite, rndX, rndY));
			}
			tmpSprite = content.Load<Texture2D>("Sprites/Tripod");
			for (int i = 0; i < 5; i++) {
				int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
				int rndY = random.Next(0, window.ClientBounds.Height / 2);
				enemies.Add(new Tripod(tmpSprite, rndX, rndY));
			}
		}

		public static void Reset(GameWindow window, ContentManager content) {
			player.Reset(380, 400, 2.5f, 4.5f);
			GenerateEnemies(window, content);
		}

	}
}
