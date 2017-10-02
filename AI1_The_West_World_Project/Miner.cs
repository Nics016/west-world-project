/*
 * Created by SharpDevelop.
 * User: usr4
 * Date: 13.06.2015
 * Time: 12:28
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace AI1_The_West_World_Project
{
	/// <summary>
	/// Description of Miner.
	/// </summary>

	public enum location_type
	{
		goldmine,
		bank,
		saloon,
		home,
		mac
	}
	
	public class Miner : BaseGameEntity
	{	
		//where miner is currently situated
		private location_type m_Location;	
		
		//how many nuggets the miner has in his pockets
		private int 				m_iGoldCarried;
		
		private int 				m_iGoldCarriedMax;
		
		private int 				m_iGoldMax;
		
		//how much money the miner has deposited in the bank
		private int 					m_iMoneyInBank;
		
		//thirst
		private int 					m_iThirst;
		
		private int 					m_iThirstMax;
		
		//fatigue
		private int 					m_iFatigue;
		
		//hunger
		private int						m_iHunger;
		
		//hungerMax
		private int						m_iHungerMax;
		
		//state Machine
		private StateMachine<Miner> 	m_pStateMachine;	
		
		//wife
		private Wife m_Wife;
		
		//MacWorker
		private MacWorker m_MacWorker;
		
		//random
		private Random rand;
		
		//current order
		private Order order;
		
		private void SetBaseValues()
		{			
			if (rand == null)
				rand = new Random();
			
			m_iGoldCarried = 0;
			m_iMoneyInBank = 0;
			m_iThirst = 0;
			m_iHunger = 0;
			m_iFatigue = 0;
			m_Location = location_type.home;
			
			m_iGoldCarriedMax = 2 + rand.Next(4);			
			m_iGoldMax = 4 + rand.Next(10);			
			m_iThirstMax = 7 + rand.Next(5);
			m_iHungerMax = 9 + rand.Next(4);
			
			//setting up state machine
			m_pStateMachine = new StateMachine<Miner>(this);
			
			m_pStateMachine.SetCurrentState(GoHomeAndSleepTilRested.Instance());
		}
		
		public Miner(int id) : base(id)
		{
			SetBaseValues();
		}
		
		public Miner(int id, string name) : base (id, name)
		{
			SetBaseValues();
		}
		
		public Miner(int id, string name, Random Rand) : base (id, name)
		{
			rand = Rand;
			SetBaseValues();			
		}
		
		public override void Update()
		{
			m_iThirst++;
			
			//random bar coming TODO: BUGGED!
			/*if (rand.Next(200) == 0 && Location != location_type.saloon)
			{
				Console.WriteLine(String.Format("\n{0}: Мне срочно нужно выпить чего-нибудь покрепче! ", 
			                                EntityManager.Instance().GetNameOfEntity(this.ID)));
				//m_pStateMachine.ChangeState(QuenchThirst.Instance());
				m_pStateMachine.SetGlobalState(QuenchThirst.Instance());
			}*/
			
			m_pStateMachine.Update();
			
			//wife update
			if (m_Wife != null)
					m_Wife.Update();
			
			//macWorker
			if (m_MacWorker != null)
				m_MacWorker.Update();
		}
		
		public void ChangeState(State<Miner> pNewState)
		{
			m_pStateMachine.ChangeState(pNewState);
		}
		
		public void RevertToPreviousState()
		{
			m_pStateMachine.RevertToPreviousState();
		}
		
		public location_type Location
		{
			get
			{
				return m_Location;
			}
		}
		
		public void ChangeLocation (location_type newLocation)
		{
			m_Location = newLocation;
		}
		
		public void AddToGoldCarried(int iGold)
		{
			m_iGoldCarried += iGold;
		}
		
		public void IncreaseFatigue()
		{
			m_iFatigue += 1;
		}
		
		public void Sleep()
		{
			m_iFatigue -= 3;
			if (m_iFatigue < 0)
				m_iFatigue = 0;
		}
		
		public int Fatigue
		{
			get
			{
				return m_iFatigue;
			}
		}
		
		public bool PocketsFull()
		{
			bool answ = false;
			
			if (m_iGoldCarried >= m_iGoldCarriedMax)
				answ = true;
			
			return answ;
		}
		
		public bool Thirsty()
		{
			bool answ = false;
			
			if (m_iThirst >= m_iThirstMax)
				answ = true;
			
			return answ;
		}
		
		public int GoldInPockets
		{
			get
			{
				return m_iGoldCarried;
			}
		}
		
		public int GoldInBank
		{
			get
			{
				return m_iMoneyInBank;
			}
		}
		
		public void IncreaseHunger()
		{
			m_iHunger++;
		}
			
		public bool Hungry()
		{
			bool answ = (m_iHunger > m_iHungerMax);
			
			return answ;
		}
		
		public void ResetHunger()
		{
			m_iHunger = 0;
		}
		
		public void EmptyPockets()
		{
			m_iGoldCarried = 0;
		}
		
		public void DepositIntoTheBank(int iGold)
		{
			m_iMoneyInBank += iGold;
		}
		
		public bool Wealthy()
		{
			bool answ = false;
			
			answ = (m_iMoneyInBank >= m_iGoldMax);
			
			return answ;
		}
	
		public void SpendNugget()
		{
			m_iGoldCarried--;
		}
		
		public void SpendGoldFromBank(int iGoldSpend)
		{
			if (iGoldSpend <= GoldInBank)
			{
				m_iMoneyInBank -= iGoldSpend;
			}
			else
			{
				throw new Exception("Was trying to spend more gold, than has in the bank.");
			}
		}
		
		public void CleanThirst()
		{
			m_iThirst = 0;
		}
		
		public void MarryOn(Wife wife)
		{
			m_Wife = wife;
		}
		
		public State<Miner> CurrentState
		{
			get
			{
				return m_pStateMachine.CurrentState;
			}
		}
		
		public void WifeChangeState(State<Wife> NewState)
		{
			m_Wife.ChangeState(NewState);
		}
		
		public override bool HandleMessage(Telegram msg)
		{
			return m_pStateMachine.HandleMessage(msg);
		}
		
		public int WifeID
		{
			get
			{
				return m_Wife.ID;
			}
		}
		
		public StateMachine<Miner> GetFSM()
		{
			return m_pStateMachine;
		}
		
		public Random GetRand
		{
			get
			{
				return rand;
			}
		}
		
		public void SetMacWorker(MacWorker pMacWorker)
		{
			m_MacWorker = pMacWorker;
		}
		
		public int GetMcWorkerID
		{
			get 
			{
				return m_MacWorker.ID;
			}
		}
		
		public void SetOrder(Order NewOrder)
		{
			order = NewOrder;
		}
		
		public OrderItem GetNextDish()
		{
			return order.GetNextDish();
		}
		
		public void ClearOrder()
		{
			order = null;
		}
	}
}
