using System;
using System.Collections.Generic;
using System.IO;

namespace SpaceShooter {

	class HSItem {
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

		int maxInList;
		List<HSItem> hsItems;

		public HighScore(int maxInList) {
			this.maxInList = maxInList;
			hsItems = new List<HSItem>();
		}

		public void Add(string name, int points) {
			hsItems.Add(new HSItem(name, points));
			Sort();
		}

		public void Print() {
			int i = 1;
			Console.WriteLine("*** HIGHSCORE ***");
			foreach (HSItem hs in hsItems) {
				Console.WriteLine($"{i}. {hs.Name} : {hs.Points}");
				i++;
			}
			Console.WriteLine("\n");
		}

		void Sort() {
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
			if (hsItems.Count > maxInList) {
				for (int i = 0; i < hsItems.Count; i++) {
					if (i >= maxInList)
						hsItems.Remove(hsItems[i]);
				}
			}

		}

		public void LoadFromFile(string fileName) {
			StreamReader sr = new StreamReader(fileName);

			for (int i = 0; i < maxInList; i++) {
				string newLine = sr.ReadLine();
				if (newLine != null) {
					string[] info = newLine.Split(' ');
					if (info.Length == 2)
						hsItems.Add(new HSItem(info[0], Convert.ToInt32(info[1])));
				}
			}

			sr.Close();
		}

		public void SaveToFile(string fileName) {
			StreamWriter sw = new StreamWriter(fileName);

			for (int i = 0; i < hsItems.Count; i++) {
				sw.WriteLine($"{hsItems[i].Name} {hsItems[i].Points}");
			}

			sw.Close();
		}

	}
}
