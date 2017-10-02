/*
 * Created by SharpDevelop.
 * User: usr4
 * Date: 17.06.2015
 * Time: 15:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace AI1_The_West_World_Project
{
	/// <summary>
	/// Description of Clock.
	/// </summary>
	public class Clock
	{
		private static Clock instance;
		private double currentTime;
		
		public Clock()
		{
			currentTime = 0;
		}
		
		public static Clock Instance()
		{
			if (instance == null)
				instance = new Clock();
			
			return instance;
		}
		
		public double GetCurrentTime()
		{
			return currentTime;
		}
		
		public void TimePassed(double PassedTimeValue)
		{
			currentTime += PassedTimeValue;
			Debugger.Instance().WriteLine(String.Format("\nClocks: Current time is {0} s", currentTime));
		}
	}
}
