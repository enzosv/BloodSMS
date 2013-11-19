using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blood_SMS
{
	public class City
	{
		string name;
		int distance;
		public string Name {get; set;}
		public int Distance {get; set;}
		public City(string NAME, int DISTANCE)
		{
			name = NAME;
			distance = DISTANCE;
		}
	}
}

