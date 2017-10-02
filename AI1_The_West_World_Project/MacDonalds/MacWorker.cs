/*
 * Created by SharpDevelop.
 * User: usr4
 * Date: 23.06.2015
 * Time: 17:59
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace AI1_The_West_World_Project
{
	/// <summary>
	/// Description of MacWorker.
	/// </summary>
	public class MacWorker : BaseGameEntity
	{
		private StateMachine<MacWorker> m_StateMachine;
		private Miner m_miner;
		
		public override void Update()
		{
			m_StateMachine.Update();
		}
		
		public override bool HandleMessage(Telegram msg)
		{
			return m_StateMachine.HandleMessage(msg);
		}
		
		public MacWorker(int id, string name, Miner pMiner) : base (id, name)
		{
			m_StateMachine = new StateMachine<MacWorker>(this);
			m_StateMachine.SetCurrentState(MacWorker_Hanging.Instance());
			
			m_miner = pMiner;
		}
		
		public StateMachine<MacWorker> GetFSM()
		{
			return m_StateMachine;
		}
		
		public int MinerID
		{
			get
			{
				return m_miner.ID;
			}
		}
	}
}
