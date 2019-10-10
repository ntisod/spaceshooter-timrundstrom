﻿using System;
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
		static Texture2D stoneSprite;

		static PrintText printText;
		static Menu menu;
		static Background background;
		static HighScore hs;

		//Properties
		public enum State { Menu, Run, Highscore, Quit };
		public static State currentState;


		public static void Initialize() {
			powerups = new List<PowerUp>();
		}

		public static void UnloadContent() {
			hs.SaveToFile("highscore.txt");
		}

		public static void LoadContent(ContentManager content, GameWindow window) {
			//Load menu
			menu = new Menu((int)State.Menu);
			menu.AddItem(content.Load<Texture2D>("Sprites/menu/start"), (int)State.Run);
			menu.AddItem(content.Load<Texture2D>("Sprites/menu/highscore"), (int)State.Highscore);
			menu.AddItem(content.Load<Texture2D>("Sprites/menu/exit"), (int)State.Quit);
			//Load player
			player = new Player(content.Load<Texture2D>("Sprites/ship"), 380, 400, 2.5f, 4.5f, content.Load<Texture2D>("Sprites/bullet"));
			//Load background
			background = new Background(content.Load<Texture2D>("Sprites/background"), window);
			//Load Highscore
			hs = new HighScore(5);
			hs.LoadFromFile("highscore.txt");

			//Load in enemies
			enemies = new List<Enemy>();
			stoneSprite = content.Load<Texture2D>("Sprites/stone");
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
			menu.Draw(spriteBatch);
		}

		public static State RunUpdate(ContentManager content, GameWindow window, GameTime gameTime) {
			//Update player position
			player.Update(window, gameTime);
			//Update background
			background.Update(window);

			//Generate enemies randomly
			Random random = new Random();
			int newEnemy = random.Next(1, 150);
			if (newEnemy == 1) {
				int rndx = random.Next(0, window.ClientBounds.Width - mineSprite.Width);
				int rndy = 0 - mineSprite.Height * 2;
				enemies.Add(new Mine(mineSprite, rndx, rndy));
			}
			if (newEnemy == 2) {
				int rndx = random.Next(0, window.ClientBounds.Width - tripodSprite.Width);
				int rndy = 0 - mineSprite.Height * 2;
				enemies.Add(new Tripod(tripodSprite, rndx, rndy));
			}
			if (newEnemy == 3) {
				int rndx = random.Next(0, window.ClientBounds.Width - tripodSprite.Width);
				int rndy = 0 - stoneSprite.Height * 2;
				enemies.Add(new Stone(stoneSprite, rndx, rndy));
			}

			//Go through enemies
			foreach (Enemy e in enemies.ToList()) {
				//Check if player shoots an enemy
				foreach (Bullet b in player.Bullets) {
					if (e.CheckCollision(b)) {
						e.IsAlive = false;
						b.IsAlive = false;
						if (!(e is Stone)) {
							player.Points++;
							Drop(e.X, e.Y, gameTime);
						}
					}
				}
				if (e.IsAlive) {
					//Check if enemy collides with player
					if (e.CheckCollision(player)) {
						player.Hurt(gameTime); // If so, damage player
						enemies.Remove(e);
					}
					e.Update(window);
				} else
					enemies.Remove(e);
			}
			if (player.Health == 0) // If the players health is zero then kill player
				player.IsAlive = false;

			// TODO: Make enemies drop hearts / upgrades
			// Generate powerups randomly
			int newPowerUp = random.Next(1, 1000);
			if (newPowerUp < 3) {
				int rndx = random.Next(0, window.ClientBounds.Width - 25);
				int rndY = random.Next(0, window.ClientBounds.Height - 25);
				powerups.Add(new GoldCoin(goldCoinSprite, rndx, rndY, gameTime, 5000));
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

		static void Drop(float positionX, float positionY, GameTime gameTime) {
			Random random = new Random();
			int dropChance = random.Next(1, 100);
			if (dropChance < 20 && player.Health < 5)
				powerups.Add(new Heart(heartSprite, positionX, positionY, gameTime, 7500));
			if (dropChance >= 20 && dropChance < 30 && player.RateOfFire != 200)
				powerups.Add(new Upgrade(upgradeSprite, positionX, positionY, gameTime, 7500));
		}
		

	}
}
