/*
 * Created by SharpDevelop.
 * User: usr4
 * Date: 15.06.2015
 * Time: 12:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace AI1_The_West_World_Project
{
	/// <summary>
	/// Description of Wife.
	/// </summary>
	
	public enum wife_location_type
	{
		bath,
		house,
		foodshop,
		clothshop
	}
	
	public class Wife : BaseGameEntity
	{
		//VAR
		private wife_location_type w_Location;		
		private StateMachine<Wife> w_StateMachine;
		private Miner w_husband;
		private int w_iRich;
		private bool cooking = false;
		
		//random
		public Random rand;
		
		public Wife(int id, string name, Random Rand, Miner miner) : base(id, name)
		{
			rand = Rand;
			w_husband = miner;
			
			w_iRich = 4 + rand.Next(6);
			w_Location = wife_location_type.bath;
			w_StateMachine = new StateMachine<Wife>(this);
			w_StateMachine.SetCurrentState(GoBath.Instance());
			w_StateMachine.SetGlobalState(WifesGlobalState.Instance());
		}
		
		public override void Update()
		{
			w_StateMachine.Update();
		}
		
		public wife_location_type Location
		{
			get
			{
				return w_Location;
			}
		}
		
		public void ChangeState(State<Wife> NewState)
		{
			w_StateMachine.ChangeState(NewState);
		}
		
		public void ChangeLocation(wife_location_type NewLocation)
		{
			w_Location = NewLocation;
		}
		
		public int GoldInBank
		{
			get
			{
				return w_husband.GoldInBank;
			}
		}
		
		public bool HasMoney()
		{
			bool answ = (GoldInBank > 1);
			
			return answ;
		}
		
		public bool IsRich()
		{
			bool answ = (GoldInBank >= w_iRich);
			
			return answ;
		}
		
		public void SpendGold(int iGoldSpend)
		{
			if (iGoldSpend <= GoldInBank)
			{
				w_husband.SpendGoldFromBank(iGoldSpend);
			}
			else
			{
				throw new Exception("Failed, trying to spend more gold than having in bank");
			}
		}
		
		public bool HusbandIsNotSleeping()
		{
			bool answ = (w_husband.CurrentState.GetType() != GoHomeAndSleepTilRested.Instance().GetType());
			
			return answ;
		}
		
		public override bool HandleMessage(Telegram msg)
		{
			return w_StateMachine.HandleMessage(msg);
		}
		
		public void SetCooking(bool val)
		{
			cooking = val;
		}
		
		public bool Cooking
		{
			get
			{
				return cooking;
			}
		}
		
		public int HusbandsId
		{
			get
			{
				return w_husband.ID;
			}
		}
	}
}
