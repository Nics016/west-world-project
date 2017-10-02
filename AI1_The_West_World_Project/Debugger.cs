/*
 * Created by SharpDevelop.
 * User: usr4
 * Date: 24.06.2015
 * Time: 18:52
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace AI1_The_West_World_Project
{
	/// <summary>
	/// Description of Debugger.
	/// </summary>
	public class Debugger
	{
		private static Debugger instance;
		private static bool showDebug;
		
		public static Debugger Instance()
		{
			if (instance == null)
			{
				instance = new Debugger();
				showDebug = false;
			}
			
			return instance;
		}
		
		public void Enable()
		{
			showDebug = true;
		}
		
		public void Disable()
		{
			showDebug = false;
		}
		
		public void WriteLine(string s)
		{
			if (showDebug)
				Console.WriteLine(s);
		}
	}
}
