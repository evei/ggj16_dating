using System;

public static class RandomHelper
{
	public static Random r;

	static RandomHelper()
	{
		r = new Random();
	}

	public static int Next(int excludedMaxValue)
	{
		return r.Next(excludedMaxValue);
	}	

	public static int Next(int includedMinValue, int excludedMaxValue)
	{
		return r.Next(includedMinValue, excludedMaxValue);
	}	

	public static bool TrueFalse ()
	{
		return r.Next(2) == 0;
	}
}