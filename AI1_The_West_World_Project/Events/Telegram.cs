/*
 * Created by SharpDevelop.
 * User: usr4
 * Date: 17.06.2015
 * Time: 14:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;

namespace AI1_The_West_World_Project
{
	/// <summary>
	/// Description of Telegram.
	/// </summary>
	
	public enum message_type
	{
		Msg_HiHoneyImHome,
		Msg_StewReady
	}
	
	public enum message_type_mac
	{
		Msg_NewOrder = 0,
		Msg_EndOrder,
		Msg_FoodIsReady,
		
		Msg_BigTasty,
		Msg_CocaCola,
		Msg_Free,
		Msg_IceCream
	}
	
	public struct Telegram
	{
		//the entity that sent this telegram
		public int								Sender;
		
		//the entity that is to receive this telegram
		public int 								Receiver;
		
		//the message itself - enum
		public int 								Msg;
		
		//for delayed dispatching
		public double 						DispatchTime;
		
		public AdditionalInfo 		TheAdditionalInfo;
		
		//any additional information
		//void 							ExtraInfo;
		
		
		public Telegram(double delay, int sender, int receiver, int msg)
		{
			Sender = sender;
			Receiver = receiver;
			Msg = msg;
			DispatchTime = delay;
			TheAdditionalInfo = new AdditionalInfo(new Order());
		}
		
		public Telegram(double delay, int sender, int receiver, int msg, AdditionalInfo additionalInfo)
		{
			Sender = sender;
			Receiver = receiver;
			Msg = msg;
			DispatchTime = delay;
			TheAdditionalInfo = additionalInfo;
		}
	}
	
	public class MessageDispatcher
	{
		private PriorityClass PriorityQ;
		
		private void Discharge(BaseGameEntity pReceiver, Telegram msg)
		{
			pReceiver.HandleMessage(msg);
		}
		
		private static MessageDispatcher instance;
		
		MessageDispatcher() 
		{
			PriorityQ = new PriorityClass();
		}
		
		public static MessageDispatcher Instance()
		{
			if (instance == null)
				instance = new MessageDispatcher();
			
			return instance;
		}
		
		//the procedure isbeing used for adding new messages in queque
		public void DispatchMessage(double delay, int sender, int receiver, int msg, AdditionalInfo additionalInfo)
		{
			Telegram telegram;
			
			//receiver
			BaseGameEntity pReceiver = EntityManager.Instance().GetEntityFromID(receiver);
			
			//create telegram
			if (additionalInfo == null)
				telegram = new Telegram(0, sender, receiver, msg);
			else 
				telegram = new Telegram(0, sender, receiver, msg, additionalInfo);
			
			if (delay <= 0)
			{
				if ((EntityManager.Instance().GetEntityFromID(sender).GetType().ToString() == "AI1_The_West_World_Project.MacWorker") ||
					(EntityManager.Instance().GetEntityFromID(receiver).GetType().ToString() == "AI1_The_West_World_Project.MacWorker"))
				{
					Debugger.Instance().WriteLine(String.Format("\nThe message {0} from {1} to {2} was sent immediately", (message_type_mac)msg,
					                                            EntityManager.Instance().GetNameOfEntity(sender), EntityManager.Instance().GetNameOfEntity(receiver)));
				}
				else
					Debugger.Instance().WriteLine(String.Format("\nThe message {0} from {1} to {2} was sent immediately", (message_type)msg, 
					                                            EntityManager.Instance().GetNameOfEntity(sender), EntityManager.Instance().GetNameOfEntity(receiver)));
				
				Discharge(pReceiver, telegram);
			}
			else
			{
				double CurrentTime = Clock.Instance().GetCurrentTime();
				
				telegram.DispatchTime = CurrentTime + delay;
				
				PriorityQ.Add(telegram);
				
				Debugger.Instance().WriteLine(String.Format("\nThe delayed message {0} from {1} to {2} was sent. It will be discharged in {3} s", (message_type)msg, 
				                  EntityManager.Instance().GetNameOfEntity(sender), EntityManager.Instance().GetNameOfEntity(receiver), 
				                  delay));
			}			
		}
		
		public void DispatchDelayedMessages()
		{
			//this procedure is being called each update
			
			//first get current time
			double CurrentTime = Clock.Instance().GetCurrentTime();
			
			//check whether we need to dispatch messages or not
			bool notNull =  !(PriorityQ.begin().Msg == -1 && PriorityQ.begin().Receiver == -1 
			                  && PriorityQ.begin().Sender == -1);
			
			while ( (PriorityQ.begin().DispatchTime < CurrentTime) &&
			       (PriorityQ.begin().DispatchTime > 0) && notNull)
			{
				Telegram telegram = PriorityQ.begin();
				
				//find the recipient
				BaseGameEntity pReceiver = EntityManager.Instance().GetEntityFromID(telegram.Receiver);
				
				//send the telegram to recipient
				Discharge(pReceiver, telegram);
				
				//and remove it from the queque
				PriorityQ.Remove(PriorityQ.begin());
			}
			
		}
	}
	
		public class PriorityClass : ArrayList
	{
		public Telegram begin()
		{
			Telegram answ;
			if (this[0] == null)
				answ = new Telegram(0, -1, -1, -1);
			else
			{
				answ = (Telegram)this[0];
				foreach (Telegram telegram in this)
				{
					if (answ.DispatchTime > telegram.DispatchTime)
						answ = telegram;
				}
			}
			return answ;
		}
	}
}
