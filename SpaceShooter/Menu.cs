using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter {
	class MenuItem {

		Texture2D texture;
		Vector2 position;
		int currentState;

		public MenuItem(Texture2D texture, Vector2 position, int currentState) {
			this.texture = texture;
			this.position = position;
			this.currentState = currentState;
		}

		public Texture2D Texture { get { return texture; } }
		public Vector2 Position { get { return position; } }
		public int State { get { return currentState; } }




	}

	class Menu {
		List<MenuItem> menu;
		int selected = 0; //Fist option in the list is selected

		float currentHeight = 0; //Draw menu at different heights
		double lastChange = 0; //Stop selection spamming, by pausing the keyboard inputs
		int defaultMenuState;

		public Menu(int defaultMenuState) {
			menu = new List<MenuItem>();
			this.defaultMenuState = defaultMenuState;
		}

		public void AddItem(Texture2D itemTexture, int state) {
			//Height of item
			float X = 380 - itemTexture.Width / 2;
			float Y = 100 + currentHeight;
			//Set new currentHeight for next item
			currentHeight += itemTexture.Height + 20;
			
			menu.Add(new MenuItem(itemTexture, new Vector2(X, Y), state));
		}

		public int Update(GameTime gameTime) {
			KeyboardState keyboardState = Keyboard.GetState();

			//Pause the ability to change between menuoptions for 130ms
			if (lastChange + 130 < gameTime.TotalGameTime.TotalMilliseconds) {

				//Go down the menu
				if (keyboardState.IsKeyDown(Keys.Down)) {
					selected++;
					if (selected > menu.Count - 1) //If it goes over the limit, loop back around
						selected = 0;
				}
				//Go up the menu
				if (keyboardState.IsKeyDown(Keys.Up)) {
					selected--;
					if (selected < 0) //If it goes over the limit, loop back around
						selected = menu.Count - 1;
				}
				//Reset lastChange
				lastChange = gameTime.TotalGameTime.TotalMilliseconds;

			}
			//Choose the selected menuitem
			if (keyboardState.IsKeyDown(Keys.Enter))
				return menu[selected].State;
			//If none was chosen, then stay in menu
			return defaultMenuState;
		}

		public void Draw(SpriteBatch spriteBatch) {
			for (int i = 0; i < menu.Count; i++) {
				//Draw the selected menuitem with a unique color tone
				if (i == selected)
					spriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.Red);
				else //Otherwise draw it normally
					spriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.White);
			}
		}


	}
}
