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

			GameElements.currentState = GameElements.State.Menu;
			GameElements.Initialize();
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			GameElements.LoadContent(Content, Window);
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
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				Exit();
			
			switch (GameElements.currentState) {
				case GameElements.State.Run:
					GameElements.currentState = GameElements.RunUpdate(Content, Window, gameTime);
					break;
				case GameElements.State.Highscore:
					GameElements.currentState = GameElements.HighScoreUpdate();
					break;
				case GameElements.State.Quit:
					this.Exit();
					break;
				default:
					GameElements.currentState = GameElements.MenuUpdate();
					break;
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
			
			switch (GameElements.currentState) {
				case GameElements.State.Run:
					GameElements.RunDraw(spriteBatch);
					break;
				case GameElements.State.Highscore:
					GameElements.HighScoreDraw(spriteBatch);
					break;
				case GameElements.State.Quit:
					this.Exit();
					break;
				default:
					GameElements.MenuDraw(spriteBatch);
					break;
			}

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
