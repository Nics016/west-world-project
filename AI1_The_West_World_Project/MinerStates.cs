/*
 * Created by SharpDevelop.
 * User: usr4
 * Date: 14.06.2015
 * Time: 13:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace AI1_The_West_World_Project
{
	/// <summary>
	/// Description of MinerStates.
	/// </summary>
	
	public class EnterMineAndDigForNugget : State<Miner>
	{		
		private static EnterMineAndDigForNugget instance;
		
		public override void Enter(Miner pMiner)
		{
			//if miner is not already located at the gold mine,
			//he must go there
			
			if (pMiner.Location != location_type.goldmine)
			{
				Console.WriteLine(String.Format("\n{0}: Иду к золотой шахте", EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
				
				pMiner.ChangeLocation(location_type.goldmine);
			}
		}
		
		public override void Execute(Miner pMiner)
		{
			//digging gold until miner is carrying MaxNuggets number of nuggets
			//although if he gets thirsty during his digging, he stops work and
			//changes states to go to saloon for a whiskey
			pMiner.AddToGoldCarried(1);
			
			//diggin' is hard work
			pMiner.IncreaseFatigue();
			pMiner.IncreaseHunger();
			
			Console.WriteLine(String.Format("\n{0}: Заработал монетку", EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
			
			//if enough gold mined, go and put it in the bank
			if (pMiner.PocketsFull())
			{
				pMiner.ChangeState(VisitBankAndDepositeGold.Instance());
			}
			
			//if thirsty go and get whiskey
			else if (pMiner.Thirsty())
			{
				pMiner.ChangeState(QuenchThirst.Instance());
			}
			
			//if hungry go and eat in Mac
			else if (pMiner.Hungry())
			{
				pMiner.ChangeState(Mac_MakeOrder.Instance());
			}
		}
		
		public override void Exit(Miner pMiner)
		{
			Console.WriteLine(String.Format("\n{0}: Я выхожу из шахты с полными карманами блестящего золота, ухуу", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
		}
		
		public static State<Miner> Instance()
		{
			if (instance == null)
				instance = new EnterMineAndDigForNugget();
			
			return instance;
		}
		
		public override bool OnMessage(Miner entity, Telegram msg)
		{
			return false;
		}
	}
	
	public class QuenchThirst : State<Miner>
	{
		private static QuenchThirst instance;
		
		public static State<Miner> Instance()
		{
			if (instance == null)
				instance = new QuenchThirst();
			
			return instance;
		}
		
		public override void Enter(Miner pMiner)
		{
			if (pMiner.Location != location_type.saloon)
			{
				Console.WriteLine(String.Format("\n{0}: Мне нужно выпить. Иду в барр", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
			
				pMiner.ChangeLocation(location_type.saloon);
			}
		}
		
		public override void Execute(Miner pMiner)
		{
			pMiner.IncreaseFatigue();
			pMiner.IncreaseHunger();
			
			Console.WriteLine(String.Format("\n{0}: О, этот могучий восхитительный ликер", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
			pMiner.SpendNugget();
			pMiner.CleanThirst();
			pMiner.ChangeState(EnterMineAndDigForNugget.Instance());
		}
		
		public override void Exit(Miner pMiner)
		{
			Console.WriteLine(String.Format("\n{0}: Выхожу из бара. Ооо, я слегка пьян", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
		}
		
		public override bool OnMessage(Miner entity, Telegram msg)
		{
			return false;
		}
	}
	
	public class VisitBankAndDepositeGold : State<Miner>
	{
		private static VisitBankAndDepositeGold instance;
		
		public static State<Miner> Instance()
		{
			if (instance == null)
				instance = new VisitBankAndDepositeGold();
			
			return instance;
		}
		
		public override void Enter(Miner pMiner)
		{
			if (pMiner.Location != location_type.bank)
			{
				Console.WriteLine(String.Format("\n{0}: Я иду в банк, ее", EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
			
				pMiner.ChangeLocation(location_type.bank);
			}
		}
		
		public override void Execute(Miner pMiner)
		{
			pMiner.IncreaseFatigue();
			pMiner.IncreaseHunger();
			
			if (pMiner.GoldInPockets > 0)
			{
				pMiner.DepositIntoTheBank(pMiner.GoldInPockets);
				Console.WriteLine(String.Format("\n{0}: Я положил {1} золота в банк. Теперь у меня всего в банке {2} золота", 
				                                EntityManager.Instance().GetNameOfEntity(pMiner.ID), pMiner.GoldInPockets,
				                               pMiner.GoldInBank));
				pMiner.EmptyPockets();
				
				if (pMiner.Wealthy())
				{
					pMiner.ChangeState(GoHomeAndSleepTilRested.Instance());
					//pMiner.GetFSM().SetGlobalState(GoHomeAndSleepTilRested.Instance());
				}
				else
				{
					pMiner.ChangeState(EnterMineAndDigForNugget.Instance());
				}
			}
			else
			{
				Console.WriteLine(String.Format("\n{0}: У меня нет золота с собой. Зачем я пришел в банк?", 
				                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
				pMiner.ChangeState(EnterMineAndDigForNugget.Instance());
			}
		}
		
		public override void Exit(Miner pMiner)
		{
			Console.WriteLine(String.Format("\n{0}: Я выхожу из банка", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
		}
		
		public override bool OnMessage(Miner entity, Telegram msg)
		{
			return false;
		}
	}
	
	public class GoHomeAndSleepTilRested : State<Miner>
	{
		private static GoHomeAndSleepTilRested instance;
		
		public static State<Miner> Instance()
		{
			if (instance == null)
				instance = new GoHomeAndSleepTilRested();
			
			return instance;
		}
		
		public override void Enter(Miner pMiner)
		{
			if (pMiner.Location != location_type.home)
			{
				Console.WriteLine(String.Format("\n{0}: Вухуу! Я уже достаточно богат. Возвращаюсь домой к моей женушке.", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
			
				pMiner.ChangeLocation(location_type.home);
				
				//pMiner.WifeChangeState(SleepWithHusband.Instance());
				
				//let the wife know I'm home
				MessageDispatcher.Instance().DispatchMessage(SEND_MSG_IMMEDIATELY, //0 delay
				                                             pMiner.ID,
				                                             pMiner.WifeID,
				                                             (int)message_type.Msg_HiHoneyImHome,
				                                            	null);
			}
		}
		
		public override void Execute(Miner pMiner)
		{
			pMiner.IncreaseFatigue();
			
			Console.WriteLine(String.Format("\n{0}: ZZZZ...", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
			pMiner.Sleep();
			if (pMiner.Fatigue == 0)
			{
				pMiner.ChangeState(EnterMineAndDigForNugget.Instance());
				//pMiner.GetFSM().SetGlobalState(null);
			}
			
		}
		
		public override void Exit(Miner pMiner)
		{
			Console.WriteLine(String.Format("\n{0}: Какой фантастический был сон!", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
		}
		
		public override bool OnMessage(Miner pMiner, Telegram msg)
		{
			switch (msg.Msg)
			{
				case (int)message_type.Msg_StewReady:
					{
						Debugger.Instance().WriteLine(String.Format("\nMessage handled by {0} at time: {1}",
						                                            EntityManager.Instance().GetNameOfEntity(pMiner.ID), Clock.Instance().GetCurrentTime()));
						
						Console.WriteLine("\n{0}: Хорошо, дорогая, я иду!", EntityManager.Instance().GetNameOfEntity(pMiner.ID));
						
						pMiner.ChangeState(EatStew.Instance());
						
						return true;
					}
			}
			
			return false;
		}
	}
	
	public class EatStew : State<Miner>
	{
		private static State<Miner> instance;
		
		public static State<Miner> Instance()
		{
			if (instance == null)
				instance = new EatStew();
			
			return instance;
		}
		
		public override void Enter(Miner pMiner)
		{
			Console.WriteLine(String.Format("\n{0}: Пахнет ооочень вкусно, милая!", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
		}
		
		public override void Execute(Miner pMiner)
		{
			Console.WriteLine(String.Format("\n{0}: Мммм, какая вкуснятина!", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
			pMiner.GetFSM().RevertToPreviousState();
		}
		
		public override void Exit(Miner pMiner)
		{
			Console.WriteLine(String.Format("\n{0}: Спасибо, дорогая!", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
		}
		
		public override bool OnMessage(Miner pMiner, Telegram msg)
		{
			return false;
		}
	}
}
