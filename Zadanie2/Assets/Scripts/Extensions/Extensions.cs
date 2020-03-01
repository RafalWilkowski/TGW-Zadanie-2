using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
	public static Color GetColor(this ObjectColor color)
	{
		switch (color)
		{
			case ObjectColor.RED:
				return Color.red;
			case ObjectColor.YELLOW:
				return Color.yellow;
			case ObjectColor.GREEN:
				return Color.green;
			case ObjectColor.BLUE:
				return Color.blue;
			default:
				return Color.clear;
		}
	}
}
