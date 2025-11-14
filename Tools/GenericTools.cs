namespace Tools
{
	public class GenericTools
	{
		public static int GenerateNumber(int min, int max)
		{
			Random r = new Random();
			return r.Next(min, max);
		}
	}
}
