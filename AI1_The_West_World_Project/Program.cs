/*
 * Created by SharpDevelop.
 * User: usr4
 * Date: 13.06.2015
 * Time: 12:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
 
 
 /// <summary>
 /// The program was finished at 24-th of the June, 2015
 /// 
 /// Hello from Army, boy!
 /// I hope, everything is just fine. Keep going further in programming! 
 /// Goodluck
 /// 
 /// *Garraty, 22:17 24.06.15
 /// </summary>

 
using System;
using System.Timers;

namespace AI1_The_West_World_Project
{	
	class Program
	{
		private static Timer basicTimer;
		private static Miner minerBob;
		private static Wife wifeLea;
		private static MacWorker macWorker;
		private const int maxUpdates = 10;
		private static int currentUpdates;
		public static Random rand;		
		
		public static void Main(string[] args)
		{
			Debugger.Instance().Disable();
			basicTimer = new Timer();
			basicTimer.Interval = 100;
			basicTimer.Elapsed += new ElapsedEventHandler(basicTimer_Elapsed);
			basicTimer.Start();
			
			currentUpdates = 0;
			
			rand = new Random();
			
			minerBob = new Miner(1, "Miner Bob", rand);
			EntityManager.Instance().RegisterEntity(minerBob);
			
			wifeLea = new Wife(2, "Wife Lea", rand, minerBob);
			EntityManager.Instance().RegisterEntity(wifeLea);
			
			macWorker = new MacWorker(3, "McDonalds Worker", minerBob);
			EntityManager.Instance().RegisterEntity(macWorker);
			
			minerBob.MarryOn(wifeLea);
			minerBob.SetMacWorker(macWorker);
			
			do
			{				
				Console.ReadKey(true);	
				currentUpdates = 0;
				basicTimer.Start();
			} while (true);			
		}	

		static void basicTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (currentUpdates < maxUpdates)
			{
				currentUpdates++;	
				Clock.Instance().TimePassed(0.5);
				minerBob.Update();				
				MessageDispatcher.Instance().DispatchDelayedMessages();						
			}
			else
			{				
				basicTimer.Stop();			
			}
		}
		
	}
}