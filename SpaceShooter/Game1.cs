using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SpaceShooter {
	/// <summary>
	/// This is the main type for your game.
	/// 
	/// Tim Rundström, te17 för kursen Programmering 2 (2019-2020)
	/// </summary>
	public class Game1 : Game {
		GraphicsDeviceManager graphics; //Graphics
		SpriteBatch spriteBatch; //Draw sprites
		Player player;
		PrintText printText;
		List<Enemy> enemies;
		List<GoldCoin> goldCoins;
		Texture2D goldCoinSprite;


		public Game1() {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize() {
			// TODO: Add your initialization logic here

			goldCoins = new List<GoldCoin>();

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
			player = new Player(Content.Load<Texture2D>("Sprites/ship"), 380, 400, 2.5f, 4.5f, Content.Load<Texture2D>("Sprites/bullet"));
			goldCoinSprite = Content.Load<Texture2D>("Sprites/coin");

			//Create enemies
			enemies = new List<Enemy>();
			Random random = new Random();
			Texture2D tmpSprite = Content.Load<Texture2D>("Sprites/mine");
			for (int i = 0; i < 5; i++) {
				int rndX = random.Next(0, Window.ClientBounds.Width - tmpSprite.Width);
				int rndY = random.Next(0, Window.ClientBounds.Height/2);
				enemies.Add(new Mine(tmpSprite, rndX, rndY));
			}
			tmpSprite = Content.Load<Texture2D>("Sprites/tripod");
			for (int i = 0; i < 5; i++) {
				int rndX = random.Next(0, Window.ClientBounds.Width - tmpSprite.Width);
				int rndY = random.Next(0, Window.ClientBounds.Height / 2);
				enemies.Add(new Tripod(tmpSprite, rndX, rndY));
			}

			printText = new PrintText(Content.Load<SpriteFont>("myFont"));
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent() {
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime) {
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// TODO: Add your update logic here

			player.Update(Window, gameTime);
			foreach (Enemy e in enemies.ToList()) {
				foreach (Bullet b in player.Bullets) {
					if (e.CheckCollision(b)) {
						e.IsAlive = false;
						b.IsAlive = false;
						player.Points++;
					}
				}
				if (e.IsAlive) {
					if (e.CheckCollision(player))
						this.Exit();
					e.Update(Window);
				} else //Remove if dead
					enemies.Remove(e);
			}

			//Generate coins
			Random random = new Random();
			int newCoin = random.Next(1, 200);
			if (newCoin == 1) {
				int randX = random.Next(0, Window.ClientBounds.Width - goldCoinSprite.Width);
				int randY = random.Next(0, Window.ClientBounds.Height - goldCoinSprite.Height);
				goldCoins.Add(new GoldCoin(goldCoinSprite, randX, randY, gameTime));
			}

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

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);

			// TODO: Add your drawing code here
			spriteBatch.Begin();
			
			player.Draw(spriteBatch);
			foreach (Enemy e in enemies)
				e.Draw(spriteBatch);

			foreach (GoldCoin gc in goldCoins)
				gc.Draw(spriteBatch);
			printText.Print("Points:" + player.Points, spriteBatch, 0, 0);

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
