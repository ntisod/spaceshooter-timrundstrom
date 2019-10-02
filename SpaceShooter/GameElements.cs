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
		static List<GoldCoin> goldCoins;
		static Texture2D goldCoinSprite;
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

			//Load in enemies
			enemies = new List<Enemy>();
			mineSprite = content.Load<Texture2D>("Sprites/mine");
			tripodSprite = content.Load<Texture2D>("Sprites/Tripod");

			//Load in coins and points HUD
			goldCoinSprite = content.Load<Texture2D>("Sprites/coin");
			printText = new PrintText(content.Load<SpriteFont>("myFont"));
		}

		public static State MenuUpdate(GameTime gameTime) {
			return (State)menu.Update(gameTime);
		}

		public static void MenuDraw(SpriteBatch spriteBatch) {
			background.Draw(spriteBatch);
			printText.Print("Highscore: " + player.Points, spriteBatch, 290, 100);
			printText.Print("Score: " + player.OldPoins, spriteBatch, 290, 130);
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
						player.IsAlive = false; //If so, kill player
					e.Update(window);
				} else
					enemies.Remove(e);
			}

			//Generate coins randomly
			int newCoin = random.Next(1, 300);
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
			background.Draw(spriteBatch);
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
			background.Draw(spriteBatch);
		}

		public static void Reset(GameWindow window, ContentManager content) {
			player.Reset(380, 400, 2.5f, 4.5f);
			enemies.Clear();
			goldCoins.Clear();
		}

	}
}
