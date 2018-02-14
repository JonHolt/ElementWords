using System;
using System.Collections.Generic;
using System.IO;

namespace ElementWords {
	class Program {
		static ElementList elements;

		static void Main(string[] args) {
			elements = new ElementList();

			if (args.Length == 0) {
				Dictionary<int, int> occurances = new Dictionary<int, int>();
				int numWords = 0;
				using (StreamReader read = new StreamReader("words_alpha.txt")) {
					while (!read.EndOfStream) {
						string word = read.ReadLine();
						int spellings = FindSpellings(word)?.Count ?? 0;
						if (spellings > 30) {
							Console.WriteLine($"{word}:{spellings}");
						}
						if (occurances.ContainsKey(spellings)) {
							occurances[spellings]++;
						}
						else {
							occurances[spellings] = 1;
						}
						numWords++;
					}
				}

				float percentNotSpelled = (float)occurances[0] / numWords;
				Console.WriteLine($"Percent not spelled: {((float)occurances[0] / numWords) * 100}");
				foreach (KeyValuePair<int, int> pair in occurances) {
					Console.WriteLine($"Percent spelled {pair.Key} ways: {((float)pair.Value / numWords) * 100}");
				}
			}
			else {
				foreach (string word in args) {
					Console.WriteLine($"{word}:");
					List<List<Element>> spellings = FindSpellings(word);
					if (spellings != null && spellings.Count > 0) {
						foreach (List<Element> spelling in spellings) {
							foreach (Element character in spelling) {
								Console.Write($"{character.symbol}({character.name}:{character.number}),");
							}
							Console.WriteLine();
						}
					}
				}
			}
			Console.ReadLine();
		}

		static List<List<Element>> FindSpellings(string word) {
			return FindSpellings(word, new List<Element>());
		}

		static List<List<Element>> FindSpellings(string word, List<Element> found) {
			word = word.Trim();
			List<List<Element>> done = new List<List<Element>>();
			if (word.Length == 0) {
				done.Add(found);
				return done;
			}
			// Check for element whose symbol is the first letter of this word
			// if so call FindSpellings with word - first letter and found + new element
			List<List<Element>> oneLetter = null;
			Element first;
			if (elements.TryFindElement(word.Substring(0, 1), out first)) {
				List<Element> copy = new List<Element>();
				foreach (Element el in found) {
					copy.Add(new Element() {
						name = el.name,
						number = el.number,
						symbol = el.symbol
					});
				}
				copy.Add(first);
				oneLetter = FindSpellings(word.Substring(1), copy);
			}

			// Check for an element whose symbol is the first two letters of this word
			// if so call FindSpellings with word - first two letters and found + new element
			List<List<Element>> twoLetter = null;
			if (word.Length >= 2) {
				Element firstTwo;
				if (elements.TryFindElement(word.Substring(0, 2), out firstTwo)) {
					List<Element> copy = new List<Element>();
					foreach (Element el in found) {
						copy.Add(new Element() {
							name = el.name,
							number = el.number,
							symbol = el.symbol
						});
					}
					copy.Add(firstTwo);
					twoLetter = FindSpellings(word.Substring(2), copy);
				}
			}

			// If they both did combine them and return the result
			if (oneLetter != null && twoLetter != null) {
				oneLetter.AddRange(twoLetter);
				return oneLetter;
			}
			// If one of the above returned a valid result return that
			else if (oneLetter != null) {
				return oneLetter;
			}
			else if (twoLetter != null) {
				return twoLetter;
			}
			// If neither did return null
			else {
				return null;
			}
		}
	}
}
