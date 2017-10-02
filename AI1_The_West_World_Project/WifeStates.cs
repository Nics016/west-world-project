/*
 * Created by SharpDevelop.
 * User: usr4
 * Date: 15.06.2015
 * Time: 12:45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace AI1_The_West_World_Project
{
	/// <summary>
	/// Description of WifeStates.
	/// </summary>
	public class GoBath : State<Wife>
	{
		private static GoBath instance;
		
		public static State<Wife> Instance()
		{
			if (instance == null)
				instance = new GoBath();
			
			return instance;
		}
		
		public override void Enter(Wife pWife)
		{
			if (pWife.Location != wife_location_type.bath)
			{
				Console.WriteLine(String.Format("\n{0}: Иду в ванную. Нужно напудрить мой маленький красивенький носик", 
			                                EntityManager.Instance().GetNameOfEntity(pWife.ID)));
				pWife.ChangeLocation(wife_location_type.bath);
			}
		}
		
		public override void Execute(Wife pWife)
		{
			Console.WriteLine(String.Format("\n{0}: Ахххх! Какая красота!", 
			                                EntityManager.Instance().GetNameOfEntity(pWife.ID)));
			pWife.ChangeState(DoHousework.Instance());
		}
		
		public override void Exit(Wife pWife)
		{
			Console.WriteLine(String.Format("\n{0}: Я выхожу из ванной", 
			                                EntityManager.Instance().GetNameOfEntity(pWife.ID)));
		}
		
		public override bool OnMessage(Wife entity, Telegram msg)
		{
			return false;
		}
	}
	
	public class DoHousework : State<Wife>
	{
		private static DoHousework instance;
		
		public static State<Wife> Instance()
		{
			if (instance == null)
				instance = new DoHousework();
			
			return instance;
		}
		
		public override void Enter(Wife pWife)
		{
			if (pWife.Location != wife_location_type.house)
			{
				Console.WriteLine(String.Format("\n{0}: Самое время навести порядок в доме", 
			                                EntityManager.Instance().GetNameOfEntity(pWife.ID)));
				pWife.ChangeLocation(wife_location_type.house);
			}
		}
		
		public override void Execute(Wife pWife)
		{
			int iWorkType = pWife.rand.Next(7);
			string sWorkType = "the work was not set";
			
			switch (iWorkType)
			{
				case 0:
					sWorkType = "Расстилаю свежее постельное белье";
					break;
					
				case 1:
					sWorkType = "Мою пол в гостинной";
					break;	
					
				case 2:
					sWorkType = "Поливаю цветы";
					break;
				
				case 3:
					sWorkType = "Мою пол в коридоре";
					break;
				
				case 4:
					sWorkType = "Мою окна";
					break;
					
				case 5:
					sWorkType = "Готовлю вкусный обед";
					break;
					
				case 6:
					sWorkType = "Стираю вещи";
					break;
			}
			
			Console.WriteLine(String.Format("\n{0}: " + sWorkType, 
			                                EntityManager.Instance().GetNameOfEntity(pWife.ID)));
			
			if (pWife.rand.Next(10) == 0)
			{
				pWife.ChangeState(GoBath.Instance());
			}
			else
			{
				//go to shop for food
				if (pWife.HasMoney() && pWife.rand.Next(20) == 0)
				{
					pWife.ChangeState(VisitFoodShop.Instance());
				}
				//or for cloth, if rich
				else if (pWife.IsRich() && pWife.rand.Next(30) == 0)
				{
					pWife.ChangeState(VisitClothShop.Instance());
				}
			}
		}
		
		public override void Exit(Wife pWife)
		{
			//not writing anything when leaving this state
		}
		
		public override bool OnMessage(Wife entity, Telegram msg)
		{
			return false;
		}
	}
	
	public class VisitFoodShop : State<Wife>
	{
		private static VisitFoodShop instance;
		
		public static State<Wife> Instance()
		{
			if (instance == null)
				instance = new VisitFoodShop();
			
			return instance;
		}
		
		public override void Enter(Wife pWife)
		{
			if (pWife.Location != wife_location_type.foodshop)
			{
				Console.WriteLine(String.Format("\n{0}: Выхожу из дома. Пора купить что-нибудь из еды к обеду!", 
			                                EntityManager.Instance().GetNameOfEntity(pWife.ID)));
				pWife.ChangeLocation(wife_location_type.foodshop);
			}
		}
		
		public override void Execute(Wife pWife)
		{
			int maxSpend = pWife.GoldInBank;
			if (maxSpend > 5)
				maxSpend = 5;
			
			int iGoldSpent = 1 + pWife.rand.Next(maxSpend);
			
			Console.WriteLine(String.Format("\n{0}: Накупила кучу всяких вкусностей на {1} золота", 
			                                EntityManager.Instance().GetNameOfEntity(pWife.ID), iGoldSpent));
			
			pWife.SpendGold(iGoldSpent);
			pWife.ChangeState(DoHousework.Instance());
		}
		
		public override void Exit(Wife pWife)
		{
			Console.WriteLine(String.Format("\n{0}: Возвращаюсь из магазина домой", 
			                                EntityManager.Instance().GetNameOfEntity(pWife.ID)));
		}
		
		public override bool OnMessage(Wife entity, Telegram msg)
		{
			return false;
		}
	}
	
	public class VisitClothShop : State<Wife>
	{
		private static VisitClothShop instance;
		
		public static State<Wife> Instance()
		{
			if (instance == null)
				instance = new VisitClothShop();
			
			return instance;
		}
		
		public override void Enter(Wife pWife)
		{
			if (pWife.Location != wife_location_type.clothshop)
			{
				Console.WriteLine(String.Format("\n{0}: Выхожу из дома. У нас так много денег, можно купить новое платье!", 
			                                EntityManager.Instance().GetNameOfEntity(pWife.ID)));
				pWife.ChangeLocation(wife_location_type.clothshop);
			}
		}
		
		public override void Execute(Wife pWife)
		{
			int maxSpend = pWife.GoldInBank;
			if (maxSpend > 10)
				maxSpend = 10;
			
			int iGoldSpent = 1 + pWife.rand.Next(maxSpend);
			
			Console.WriteLine(String.Format("\n{0}: Какое миленькое платьице! И всего за {1} золота! Я беру его", 
			                                EntityManager.Instance().GetNameOfEntity(pWife.ID), iGoldSpent));
			pWife.SpendGold(iGoldSpent);
			pWife.ChangeState(DoHousework.Instance());
		}
		
		public override void Exit(Wife pWife)
		{
			Console.WriteLine(String.Format("\n{0}: Я выхожу из магазина. Возвращаюсь домой. ", 
			                                EntityManager.Instance().GetNameOfEntity(pWife.ID)));
		}
		
		public override bool OnMessage(Wife entity, Telegram msg)
		{
			return false;
		}
	}
	
	public class SleepWithHusband : State<Wife>
	{
		private static State<Wife> instance;
		
		public static State<Wife> Instance()
		{
			if (instance == null)
				instance = new SleepWithHusband();
			
			return instance;
		}
		
		public override void Enter(Wife pWife)
		{
			if (pWife.Location != wife_location_type.house)
			{
				pWife.ChangeLocation(wife_location_type.house);
			}
			
				Console.WriteLine(String.Format("\n{0}: Муж пришел с работы! Пора расстилать кровать и ложиться спать", 
			                                EntityManager.Instance().GetNameOfEntity(pWife.ID)));
		}
		
		public override void Execute(Wife pWife)
		{
			Console.WriteLine(String.Format("\n{0}: zzzz...", 
			                                EntityManager.Instance().GetNameOfEntity(pWife.ID)));
			if (pWife.HusbandIsNotSleeping())
				pWife.ChangeState(DoHousework.Instance());
		}
		
		public override void Exit(Wife pWife)
		{
			Console.WriteLine(String.Format("\n{0}: Какое чудесное утро!", 
			                                EntityManager.Instance().GetNameOfEntity(pWife.ID)));
		}
		
		public override bool OnMessage(Wife entity, Telegram msg)
		{
			return false;
		}
	}
	
	public class CookStew : State<Wife>
	{
		private static State<Wife> instance;
		
		public static State<Wife> Instance()
		{
			if (instance == null)
				instance = new CookStew();
			
			return instance;
		}
		
		public override void Enter(Wife pWife)
		{
			//if not already cooking put the stew in the oven
			if (!pWife.Cooking)
			{
				Console.WriteLine(String.Format("\n{0}: Кладу стейк на сковородку", 
			                                EntityManager.Instance().GetNameOfEntity(pWife.ID)));
				
				//send a delayed message to myself so that I know when to take the stew
				//out of the oven
				MessageDispatcher.Instance().DispatchMessage(0.99,
				                                             pWife.ID,
				                                             pWife.ID,
				                                             (int)message_type.Msg_StewReady,
				                                            	null);
				
				pWife.SetCooking(true);				                                             
			}
			
		}
		
		public override void Execute(Wife pWife)
		{
			Console.WriteLine(String.Format("\n{0}: Обжариваю этот аппетитный стейк...", 
			                                EntityManager.Instance().GetNameOfEntity(pWife.ID)));
		}
		public override void Exit(Wife pWife)
		{
			Console.WriteLine(String.Format("\n{0}: Возвращаюсь к домашним делам", 
			                                EntityManager.Instance().GetNameOfEntity(pWife.ID)));
		}
		
		public override bool OnMessage(Wife pWife, Telegram msg)
		{
			switch (msg.Msg)
			{
				case (int)message_type.Msg_StewReady:
					{
						Debugger.Instance().WriteLine(String.Format("\nMessage received by {0} at time: {1}", EntityManager.Instance().GetNameOfEntity(pWife.ID), 
						                                            Clock.Instance().GetCurrentTime()));
						Console.WriteLine("\n{0}: Стейк готов! Пойдем кушать!", EntityManager.Instance().GetNameOfEntity(pWife.ID));
						
						//let hubby know the stew is ready
						MessageDispatcher.Instance().DispatchMessage(SEND_MSG_IMMEDIATELY, 
						                                             pWife.ID,
						                                             pWife.HusbandsId,
						                                             (int)message_type.Msg_StewReady,
						                                             null);
						pWife.ChangeState(DoHousework.Instance());
						pWife.SetCooking(false);
						return true;
					}
			}
			
			return false;
		}
		
	}
		
	public class WifesGlobalState : State<Wife>
	{		
		private static State<Wife> instance;
		
		public static State<Wife> Instance()
		{
			if (instance == null)
				instance = new WifesGlobalState();
			
			return instance;
		}
		
		public override void Enter(Wife pWife)
		{

		}
		
		public override void Execute(Wife pWife)
		{
			
		}
		public override void Exit(Wife pWife)
		{
			
		}
		
		public override bool OnMessage(Wife pWife, Telegram msg)
		{
			switch (msg.Msg)
			{
				case (int)message_type.Msg_HiHoneyImHome:
				{
						Debugger.Instance().WriteLine(String.Format("\nMessage handled by {0} at time: {1}", EntityManager.Instance().GetNameOfEntity(pWife.ID), 
						                                            Clock.Instance().GetCurrentTime()));
						
						Console.WriteLine("\n{0}: Привет, дорогой. Позволь мне приготовить тебе парочку отличных стейков",
						                  EntityManager.Instance().GetNameOfEntity(pWife.ID));
						
						pWife.ChangeState(CookStew.Instance());
						return true;
				}	
				
			}
			return false;
		}
	}
}
