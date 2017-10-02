/*
 * Created by SharpDevelop.
 * User: usr4
 * Date: 23.06.2015
 * Time: 18:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace AI1_The_West_World_Project
{
	/// <summary>
	/// Description of MacWorkerStates.
	/// </summary>
	
	public class MacWorker_Hanging : State<MacWorker>
	{
		private static State<MacWorker> instance;		
		
		public static State<MacWorker> Instance()
		{
			if (instance == null)
				instance = new MacWorker_Hanging();
			
			return instance;
		}
		
		public override void Enter(MacWorker pMacWorker)
		{	
		}
		
		public override void Execute(MacWorker pMacWorker)
		{			
		}
		
		public override void Exit(MacWorker pMacWorker)
		{			
		}
		
		public override bool OnMessage(MacWorker pMacWorker, Telegram msg)
		{
			switch (msg.Msg)
			{
				case (int)message_type_mac.Msg_NewOrder:
					{
						Debugger.Instance().WriteLine(String.Format("\nMessage handled by {0} at time: {1}",
						                                            EntityManager.Instance().GetNameOfEntity(pMacWorker.ID), Clock.Instance().GetCurrentTime()));
						
						Console.WriteLine("\n{0}: Добро пожаловать в МакДоналдс! Пара-па-па-пам",
						                  EntityManager.Instance().GetNameOfEntity(pMacWorker.ID));
						pMacWorker.GetFSM().ChangeState(MacWorker_ReceiveOrder.Instance());
						return true;
					}
			}
			
			return false;
		}
		
	}
	
	public class MacWorker_ReceiveOrder : State<MacWorker>
	{
		private static State<MacWorker> instance;
		private static Order order;
		
		public static State<MacWorker> Instance()
		{
			if (instance == null)
				instance = new MacWorker_ReceiveOrder();
			
			return instance;
		}
		
		public override void Enter(MacWorker pMacWorker)
		{	
			Console.WriteLine("\n{0}: Что вы будете кушать сегодня?",
						                  EntityManager.Instance().GetNameOfEntity(pMacWorker.ID));
		}
		
		public override void Execute(MacWorker pMacWorker)
		{			
		}
		
		public override void Exit(MacWorker pMacWorker)
		{			
		}
		
		private void addItemToOrder(food_type item)
		{
			if (order == null)
				order = new Order();
			
			order.AddItem(new OrderItem(item));
		}
		
		public void SendDebugMessage(int ID)
		{
			Debugger.Instance().WriteLine(String.Format("\nMessage handled by {0} at time: {1}",
			                                            EntityManager.Instance().GetNameOfEntity(ID), Clock.Instance().GetCurrentTime()));
		}
		
		public override bool OnMessage(MacWorker pMacWorker, Telegram msg)
		{
			switch (msg.Msg)
			{
				case (int)message_type_mac.Msg_BigTasty:
				{
					addItemToOrder(food_type.BigTasty);
					SendDebugMessage(pMacWorker.ID);
					return true;
				}
					
				case (int)message_type_mac.Msg_CocaCola:
				{
					addItemToOrder(food_type.CocaCola);
					SendDebugMessage(pMacWorker.ID);
					return true;
				}
					
				case (int)message_type_mac.Msg_Free:
				{
					addItemToOrder(food_type.Free);	
					SendDebugMessage(pMacWorker.ID);	               
					return true;
				}
					
				case (int)message_type_mac.Msg_IceCream:
				{
					addItemToOrder(food_type.IceCream);	
					SendDebugMessage(pMacWorker.ID);                 
					return true;
				}
					
				case (int)message_type_mac.Msg_EndOrder:
				{
					//sending a complete order to pMiner
					MessageDispatcher.Instance().DispatchMessage(SEND_MSG_IMMEDIATELY, 
						                                           pMacWorker.ID,
						                                           pMacWorker.MinerID,
						                                           (int)message_type_mac.Msg_EndOrder,
						                                           new AdditionalInfo(order.Clone()));
						
					//start cooking
					pMacWorker.GetFSM().ChangeState(MacWorker_Cooking.Instance());
					
					//sending order to the cooking state to know what to cook
					MessageDispatcher.Instance().DispatchMessage(SEND_MSG_IMMEDIATELY, 
						                                           pMacWorker.ID,
						                                           pMacWorker.ID,
						                                           (int)message_type_mac.Msg_EndOrder,
						                                           new AdditionalInfo(order));
					order = null;
					return true;
				}
			}
			return false;
		}
		
	}
		
		
	public class MacWorker_Cooking : State<MacWorker>
	{
		private static State<MacWorker> instance;
		private static Order order;
		
		public static State<MacWorker> Instance()
		{
			if (instance == null)
				instance = new MacWorker_Cooking();
			
			return instance;
		}
		
		public override void Enter(MacWorker pMacWorker)
		{	
			Console.WriteLine("\n{0}: Спасибо за заказ! Приступаю к приготовлениюё",
						                  EntityManager.Instance().GetNameOfEntity(pMacWorker.ID));
		}
		
		public override void Execute(MacWorker pMacWorker)
		{			
			OrderItem CurrentDish;
			
			if (order != null)
			{
				CurrentDish = order.GetNextDish();
				
				if (CurrentDish != null)
				{
					switch (CurrentDish.Type)
					{
						case food_type.BigTasty:
						Console.WriteLine("\n{0}: Обжариваю ароматное мясо для Биг тейсти, кладу его в булочку и заправляю " + 
							                  "специальным фирменным соусом 'Биг тейсти'",
						                  EntityManager.Instance().GetNameOfEntity(pMacWorker.ID));
						break;
						
						case food_type.CocaCola:
						Console.WriteLine("\n{0}: Наливаю кока-колу",
						                  EntityManager.Instance().GetNameOfEntity(pMacWorker.ID));
						break;
						
						case food_type.Free:
						Console.WriteLine("\n{0}: Готовлю ароматную картошку 'фри'",
						                  EntityManager.Instance().GetNameOfEntity(pMacWorker.ID));
						break;
						
						case food_type.IceCream:
						Console.WriteLine("\n{0}: Наполняю вафельный стаканчик мороженым",
						                  EntityManager.Instance().GetNameOfEntity(pMacWorker.ID));
						break;
					}
				}
				else
				{
					//food is ready
					MessageDispatcher.Instance().DispatchMessage(SEND_MSG_IMMEDIATELY,
					                                             pMacWorker.ID,
					                                             pMacWorker.MinerID,
					                                             (int)message_type_mac.Msg_FoodIsReady,
					                                             null);			
					//back to Hanging state
					pMacWorker.GetFSM().ChangeState(MacWorker_Hanging.Instance());
				}
			}
		}
		
		public override void Exit(MacWorker pMacWorker)
		{			
			order = null;
			Console.WriteLine("\n{0}: Ваш заказ готов, приятного аппетита!",
						                  EntityManager.Instance().GetNameOfEntity(pMacWorker.ID));
		}
		
		public override bool OnMessage(MacWorker pMacWorker, Telegram msg)
		{
			switch (msg.Msg)
			{
				//receiving the order
				case (int)message_type_mac.Msg_EndOrder : 
				{
					Debugger.Instance().WriteLine(String.Format("\nMessage handled by {0} at time: {1}",
					                                            EntityManager.Instance().GetNameOfEntity(pMacWorker.ID), Clock.Instance().GetCurrentTime()));
					
					order = msg.TheAdditionalInfo.GetOrder;
					//cooking = true ?
					return true;
				}
			}
			return false;
		}
		
	}	
	
}
