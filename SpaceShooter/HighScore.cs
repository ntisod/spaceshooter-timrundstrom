using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter {

	public class HSItem {
		// Highscore entry
		string name;
		int points;

		public string Name { get => name; }
		public int Points { get => points; }

		public HSItem(string name, int points) {
			this.name = name;
			this.points = points;
		}
	}

	public class HighScore {

		int maxInList; // Amount of highscores to track
		public List<HSItem> hsItems; // Each highscore entry
		
		//Available letter for highscore name
		string[] availableChar = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
		//Cooldown for changing letters
		double lastChange = 0;
		//New highscore name
		string[] name = { "A", "A", "A"};

		int selectedChar = 0; // Which letter in availableChar is selected
		int nameIndex = 0; // Which letter in name is selected

		public HighScore(int maxInList) {
			this.maxInList = maxInList;
			hsItems = new List<HSItem>();
		}

		public void Add(int points) {
			hsItems.Add(new HSItem(GetName(), points)); // Add new highscore to highscorelist
			Sort(); // Sort the list and remove excess entries

			// Reset new highscore settings
			name[0] = "A";
			name[1] = "A";
			name[2] = "A";
			selectedChar = 0;
			nameIndex = 0;
	}

		public string GetName() {
			return $"{name[0]}{name[1]}{name[2]}"; // Convert name array to a string
		}

		void Sort() {
			// Sort highscores from highest to lowest
			int max = hsItems.Count - 1;
			for (int i = 0; i < max; i++) {

				int nrLeft = max - i;
				for (int j = 0; j < nrLeft; j++) {

					if (hsItems[j].Points < hsItems[j + 1].Points) {
						HSItem temp = hsItems[j + 1];
						hsItems[j + 1] = hsItems[j];
						hsItems[j] = temp;
					}
				}
			}

			// Remove excess entries 
			if (hsItems.Count > maxInList) {
				for (int i = 0; i < hsItems.Count; i++) {
					if (i >= maxInList)
						hsItems.Remove(hsItems[i]);
				}
			}

		}

		public void Update(GameTime gameTime) {
			// The selectedChar has to be updated first, then set the name[nameindex] as the chosen character
			// Then update nameIndex, otherwise the wrong letters are changed

			KeyboardState keyboardState = Keyboard.GetState();

			// If cooldown is over
			if (lastChange + 130 < gameTime.TotalGameTime.TotalMilliseconds) {
				// If down arrow is pressed
				if (keyboardState.IsKeyDown(Keys.Down)) {
					// Increase selectedChar by one
					if (selectedChar + 1 < availableChar.Length)
						selectedChar++;
					else // If it reaches the end of the list, go back to index zero
						selectedChar = 0;
				} else if (keyboardState.IsKeyDown(Keys.Up)) { // If up arrow is pressed
					// Decrease selectedChar by one
					if (selectedChar - 1 >= 0)
						selectedChar--;
					else // If it reaches the start of the list, go back to the end
						selectedChar = availableChar.Length - 1;
				}

				name[nameIndex] = availableChar[selectedChar]; // Update selected letter in name

				// If right arrow is pressed
				if (keyboardState.IsKeyDown(Keys.Right)) {
					// Increase nameIndex by one
					if (nameIndex + 1 <= 2) {
						// Set selectedChar to the new nameIndex
						selectedChar = Array.IndexOf(availableChar, name[nameIndex + 1]);
						nameIndex++;
					} else {// If it reaches the end, set it to 0 (start)
						// Set selectedChar to the new nameIndex
						selectedChar = Array.IndexOf(availableChar, name[0]);
						nameIndex = 0;
					}
				} else if (keyboardState.IsKeyDown(Keys.Left)) {// If left arrow is pressed
					// Decrease nameIndex by one
					if (nameIndex - 1 >= 0) {
						// Set selectedChar to the new nameIndex
						selectedChar = Array.IndexOf(availableChar, name[nameIndex - 1]);
						nameIndex--;
					} else { // If it reaches to start, set it to 3 (end)
						// Set selectedChar to the new nameIndex
						selectedChar = Array.IndexOf(availableChar, name[2]);
						nameIndex = 2;
					}
				}

				lastChange = gameTime.TotalGameTime.TotalMilliseconds; // Reset cooldown
			}

		}


		public void LoadFromFile(string fileName) {
			StreamReader sr = new StreamReader(fileName); // Open streamreader

			// Loop through the savefile and fill the hsItems list with highscores.
			for (int i = 0; i < maxInList; i++) {
				string newLine = sr.ReadLine();
				if (newLine != null) {
					string[] info = newLine.Split(' ');
					if (info.Length == 2)
						hsItems.Add(new HSItem(info[0], Convert.ToInt32(info[1])));
				}
			}
			
			sr.Close(); // Close streamreader
		}

		public void SaveToFile(string fileName) {
			StreamWriter sw = new StreamWriter(fileName); // Open streamwriter

			for (int i = 0; i < hsItems.Count; i++) {
				sw.WriteLine($"{hsItems[i].Name} {hsItems[i].Points}"); //Save each highscore
			}

			sw.Close(); // Close streamwriter
		}

	}
}
