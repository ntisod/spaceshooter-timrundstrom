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
		static List<GoldCoin> goldCoins;
		static Texture2D goldCoinSprite;
		static List<Heart> hearts;
		static Texture2D heartSprite;
		static List<Upgrade> upgrades;
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
			goldCoins = new List<GoldCoin>();
			hearts = new List<Heart>();
			upgrades = new List<Upgrade>();
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
			printText.Print("Highscore: " + player.HighScore, spriteBatch, 250, 100);
			printText.Print("Last Game Score: " + player.OldPoins, spriteBatch, 250, 130);
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
					if (e.CheckCollision(player))
						player.Health--; // If so, damage player
					e.Update(window);
				} else
					enemies.Remove(e);
			}
			if (player.Health == 0) // If the players health is zero then kill player
				player.IsAlive = false;

			//Generate coins randomly
			int newCoin = random.Next(1, 300);
			if (newCoin == 1) {
				int rndx = random.Next(0, window.ClientBounds.Width - goldCoinSprite.Width);
				int rndY = random.Next(0, window.ClientBounds.Height - goldCoinSprite.Height);
				goldCoins.Add(new GoldCoin(goldCoinSprite, rndx, rndY, gameTime));
			}

			//Generate hearts randomly
			int newHeart = random.Next(1, 10000);
			if (newHeart == 1) {
				int rndx = random.Next(0, window.ClientBounds.Width - goldCoinSprite.Width);
				int rndY = random.Next(0, window.ClientBounds.Height - goldCoinSprite.Height);
				hearts.Add(new Heart(heartSprite, rndx, rndY, gameTime));
			}

			//Generate upgrades randomly
			int newUpgrade = random.Next(1, 5000);
			if (newUpgrade == 1) {
				int rndx = random.Next(0, window.ClientBounds.Width - goldCoinSprite.Width);
				int rndY = random.Next(0, window.ClientBounds.Height - goldCoinSprite.Height);
				upgrades.Add(new Upgrade(upgradeSprite, rndx, rndY, gameTime));
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

			//Check if heart is collected by player
			foreach (Heart h in hearts.ToList()) {
				if (h.IsAlive) {
					h.Update(gameTime);
					if (h.CheckCollision(player)) {
						hearts.Remove(h);
						player.Health++;
					}
				} else
					hearts.Remove(h);
			}

			//Check if upgrade is collected by player
			foreach (Upgrade u in upgrades.ToList()) {
				if (u.IsAlive) {
					u.Update(gameTime);
					if (u.CheckCollision(player)) {
						upgrades.Remove(u);
						player.RateOfFire -= 50;
					}
				} else
					upgrades.Remove(u);
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
			foreach (GoldCoin gc in goldCoins)
				gc.Draw(spriteBatch);
			foreach (Heart h in hearts)
				h.Draw(spriteBatch);
			foreach (Upgrade u in upgrades)
				u.Draw(spriteBatch);
			printText.Print("Health: " + player.Health, spriteBatch, 0, 0);
			printText.Print("Points: " + player.Points, spriteBatch, 0, 30);
		}

		public static void Reset(GameWindow window, ContentManager content) {
			player.Reset(380, 400, 2.5f, 4.5f);
			enemies.Clear();
			goldCoins.Clear();
		}

	}
}
