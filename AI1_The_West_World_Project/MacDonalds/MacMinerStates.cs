/*
 * Created by SharpDevelop.
 * User: usr4
 * Date: 24.06.2015
 * Time: 16:05
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace AI1_The_West_World_Project
{
	/// <summary>
	/// Description of MacMinerStates.
	/// </summary>
	public class Mac_MakeOrder : State<Miner>
	{
		private static State<Miner> instance;
		
		public static State<Miner> Instance()
		{
			if (instance == null)
				instance = new Mac_MakeOrder();
			
			return instance;
		}
		
		public override void Enter(Miner pMiner)
		{
			if (pMiner.Location != location_type.mac)
			{
				Console.WriteLine(String.Format("\n{0}: Я иду в МакДоналдс, еееу!", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
				pMiner.ChangeLocation(location_type.mac);
				
			}
			//creating order
			int iMaxOrderItems = 1 + pMiner.GetRand.Next(5);
			int iCurrentItem;
			
			//warning MacWorker about creating new order
			MessageDispatcher.Instance().DispatchMessage(SEND_MSG_IMMEDIATELY, 
						                                           pMiner.ID,
						                                           pMiner.GetMcWorkerID,
						                                           (int)message_type_mac.Msg_NewOrder,
						                                           null);
			for (int i = 0; i < iMaxOrderItems; i++)
			{
				iCurrentItem = 3 + pMiner.GetRand.Next(4);
				
				MessageDispatcher.Instance().DispatchMessage(SEND_MSG_IMMEDIATELY, 
						                                           pMiner.ID,
						                                           pMiner.GetMcWorkerID,
						                                           iCurrentItem,
						                                           null);
				
				//saying what is the current item to add in the order
				switch ((message_type_mac)iCurrentItem)
				{
					case (message_type_mac.Msg_BigTasty) :
						{
							Console.WriteLine(String.Format("\n{0}: - Биг тейсти, пожалуйста", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
							break;
						}
						
					case (message_type_mac.Msg_CocaCola) :
						{
							Console.WriteLine(String.Format("\n{0}: - Кока-колу", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
							break;
						}
						
					case (message_type_mac.Msg_Free) :
						{
							Console.WriteLine(String.Format("\n{0}: - Картошку 'фри'", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
							break;
						}
						
					case (message_type_mac.Msg_IceCream) :
						{
							Console.WriteLine(String.Format("\n{0}: - Мороженое в стаканчике", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
							break;
						}
				}
			}
			
			MessageDispatcher.Instance().DispatchMessage(SEND_MSG_IMMEDIATELY, 
						                                           pMiner.ID,
						                                           pMiner.GetMcWorkerID,
						                                           (int)message_type_mac.Msg_EndOrder,
						                                           null);
		}
		
		public override void Execute(Miner pMiner)
		{
			
		}
		
		public override void Exit(Miner pMiner)
		{
			
		}
		
		public override bool OnMessage(Miner pMiner, Telegram msg)
		{
			switch (msg.Msg)
			{
				//receiving a complete order
				case (int)message_type_mac.Msg_EndOrder :
				{
					pMiner.SetOrder(msg.TheAdditionalInfo.GetOrder);
					return true;
				}
				
				//MacWorker has finished cooking food
				case (int)message_type_mac.Msg_FoodIsReady : 
				{
					pMiner.ChangeState(Mac_Eat.Instance());
					return true;
				}
			}
			
			return false;
		}
		
	}
	
	public class Mac_Eat : State<Miner>
	{
		private static State<Miner> instance;
		
		public static State<Miner> Instance()
		{
			if (instance == null)
				instance = new Mac_Eat();
			
			return instance;
		}
		
		public override void Enter(Miner pMiner)
		{
		}
		
		public override void Execute(Miner pMiner)
		{
			OrderItem CurrentDish = pMiner.GetNextDish();
			if (CurrentDish != null)
			{
				switch (CurrentDish.Type)
				{
					case food_type.BigTasty :
					{
						Console.WriteLine(String.Format("\n{0}: Мммм, этот биг тейсти просто восхитителен!", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
						break;
					}
					
					case food_type.CocaCola :
					{
						Console.WriteLine(String.Format("\n{0}: Пью кока-колу", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
						pMiner.CleanThirst();
						break;
					}
					
					case food_type.Free :
					{
						Console.WriteLine(String.Format("\n{0}: 'Фри' с сырным соусом. Ммм...", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
						break;
					}
					
					case food_type.IceCream :
					{
						Console.WriteLine(String.Format("\n{0}: Мороженое как нельзя лучше охлаждает после изнурительной работы в шахте", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
						break;
					}
					
				}
			}
			else
			{
				//all dishes are over, change state
				Console.WriteLine(String.Format("\n{0}: Спасибо!", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
				pMiner.ClearOrder();
				pMiner.ResetHunger();
				pMiner.ChangeState(EnterMineAndDigForNugget.Instance());
			}
		}
		
		public override void Exit(Miner pMiner)
		{
			Console.WriteLine(String.Format("\n{0}: Выхожу из Мак-Доналдса", 
			                                EntityManager.Instance().GetNameOfEntity(pMiner.ID)));
		}
		
		public override bool OnMessage(Miner pMiner, Telegram msg)
		{
			return false;
		}
		
	}
}
