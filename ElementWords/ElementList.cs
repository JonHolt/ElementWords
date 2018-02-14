using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ElementWords
{
    class ElementList
    {
		List<Element> elements;

		public ElementList() {
			using (StreamReader r = new StreamReader("PeriodicTableJSON.json")) {
				string json = r.ReadToEnd();
				elements = JsonConvert.DeserializeObject<List<Element>>(json);
			}
		}

		public bool TryFindElement(string symbol, out Element result) {
			result = elements.Find(elem => elem.symbol.ToLower() == symbol.ToLower());
			return (result != null);
		}
	}

	public class Element {
		public string name;
		public string symbol;
		public int number;
	}
}
