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
		static Player player;
		static List<Enemy> enemies;
		static List<PowerUp> powerups;
		static Texture2D goldCoinSprite;
		static Texture2D heartSprite;
		static Texture2D upgradeSprite;
		static Texture2D mineSprite;
		static Texture2D tripodSprite;
		static PrintText printText;
		static Menu menu;
		static Background background;

		//Properties
		public enum State { Menu, Run, Killscreen, Highscore, Quit };
		public static State currentState;


		public static void Initialize() {
			powerups = new List<PowerUp>();
		}

		public static void UnloadContent() {
			player.SaveToFile("highscore.txt");
		}

		public static void LoadContent(ContentManager content, GameWindow window) {
			//Load menu
			menu = new Menu((int)State.Menu);
			menu.AddItem(content.Load<Texture2D>("Sprites/menu/start"), (int)State.Run);
			menu.AddItem(content.Load<Texture2D>("Sprites/menu/exit"), (int)State.Quit);
			//Load player
			player = new Player(content.Load<Texture2D>("Sprites/ship"), 380, 400, 2.5f, 4.5f, content.Load<Texture2D>("Sprites/bullet"));
			player.LoadFromFile("highscore.txt");
			//Load background
			background = new Background(content.Load<Texture2D>("Sprites/background"), window);

			//Load in enemies
			enemies = new List<Enemy>();
			mineSprite = content.Load<Texture2D>("Sprites/mine");
			tripodSprite = content.Load<Texture2D>("Sprites/Tripod");

			//Load in powerups and points HUD
			goldCoinSprite = content.Load<Texture2D>("Sprites/coin");
			heartSprite = content.Load<Texture2D>("Sprites/heart");
			upgradeSprite = content.Load<Texture2D>("Sprites/upgrade");
			printText = new PrintText(content.Load<SpriteFont>("myFont"));
		}

		public static State MenuUpdate(GameTime gameTime) {
			return (State)menu.Update(gameTime);
		}

		public static void MenuDraw(SpriteBatch spriteBatch) {
			background.Draw(spriteBatch);
			printText.Print("Highscore: " + player.HighScore, spriteBatch, 290, 100);
			menu.Draw(spriteBatch);
		}

		public static State RunUpdate(ContentManager content, GameWindow window, GameTime gameTime) {
			//Update player position
			player.Update(window, gameTime);
			//Update background
			background.Update(window);

			//Generate enemies randomly
			Random random = new Random();
			int newEnemy = random.Next(1, 100);
			if (newEnemy == 1) {
				int rndX = random.Next(0, window.ClientBounds.Width - mineSprite.Width);
				int Y = 0 - mineSprite.Height * 2;
				enemies.Add(new Mine(mineSprite, rndX, Y));
			}
			if (newEnemy == 2) {
				int rndX = random.Next(0, window.ClientBounds.Width - tripodSprite.Width);
				int Y = 0 - mineSprite.Height * 2;
				enemies.Add(new Tripod(tripodSprite, rndX, Y));
			}

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
					if (e.CheckCollision(player)) {
						player.Health--; // If so, damage player
						enemies.Remove(e);
					}
					e.Update(window);
				} else
					enemies.Remove(e);
			}
			if (player.Health == 0) // If the players health is zero then kill player
				player.IsAlive = false;

			// TODO: Fix powerup spawnrate
			// Generate powerups randomly
			int newPowerUp = random.Next(1, 20000);
			if (newPowerUp < 60) {
				int rndx = random.Next(0, window.ClientBounds.Width - 25);
				int rndY = random.Next(0, window.ClientBounds.Height - 25);
				if (newPowerUp < 20) {
					powerups.Add(new Heart(heartSprite, rndx, rndY, gameTime, 7500));
				} else if (newPowerUp < 30) {
					powerups.Add(new Upgrade(upgradeSprite, rndx, rndY, gameTime, 7500));
				} else {
					powerups.Add(new GoldCoin(goldCoinSprite, rndx, rndY, gameTime, 5000));
				}
			}

			// Check if powerup is collected by player
			foreach (PowerUp p in powerups.ToList()) {
				if (p.IsAlive) {
					p.Update(gameTime);
					if (p.CheckCollision(player)) {
						powerups.Remove(p);
						//Set powerup changes
						if (p is Heart)
							player.Health++;
						if (p is GoldCoin)
							player.Points++;
						if (p is Upgrade)
							player.RateOfFire -= 50;
					}
				} else
					powerups.Remove(p);
			}

			if (!player.IsAlive) { //If player is dead, go to menu
				Reset(window, content);
				return State.Menu;
			}
			return State.Run; //Keep playing by default

		}

		public static void RunDraw(SpriteBatch spriteBatch) {
			//Draw player, enemies, goldcoins and points HUD to screen
			background.Draw(spriteBatch);
			player.Draw(spriteBatch);
			foreach (Enemy e in enemies)
				e.Draw(spriteBatch);
			foreach (PowerUp p in powerups)
				p.Draw(spriteBatch);
			printText.Print("Health: " + player.Health, spriteBatch, 0, 0);
			printText.Print("Points: " + player.Points, spriteBatch, 0, 30);
		}

		public static void Reset(GameWindow window, ContentManager content) {
			player.Reset(380, 400, 2.5f, 4.5f);
			enemies.Clear();
			powerups.Clear();
		}

	}
}
