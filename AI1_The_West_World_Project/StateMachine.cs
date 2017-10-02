/*
 * Created by SharpDevelop.
 * User: usr4
 * Date: 13.06.2015
 * Time: 18:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace AI1_The_West_World_Project
{
	/// <summary>
	/// Description of StateMachine.
	/// </summary>
	public class StateMachine<entity_type>
	{
		private entity_type m_pOwner;
		
		private State<entity_type> m_pCurrentState;		
		private State<entity_type> m_pPreviousState;		
		private State<entity_type> m_pGlobalState;		
		
		public StateMachine(entity_type owner)
		{
			m_pOwner = owner;
		}
		
		public void SetCurrentState(State<entity_type> s)
		{
			m_pCurrentState = s;
		}
		
		public void SetPreviousState(State<entity_type> s)
		{
			m_pPreviousState = s;
		}
		
		public void SetGlobalState(State<entity_type> s)
		{
			m_pGlobalState = s;
		}
		
		//updating
		public void Update()
		{
			if (m_pGlobalState != null) 
			{	
				m_pGlobalState.Execute(m_pOwner);
			}
			
			if (m_pCurrentState != null) 
				m_pCurrentState.Execute(m_pOwner);
		}
		
		//change to a new state
		public void ChangeState(State<entity_type> pNewState)
		{
			if (pNewState == null)
				throw new Exception("Trying to change to a null state - " + pNewState);
			
			m_pPreviousState = m_pCurrentState;
			
			m_pCurrentState.Exit(m_pOwner);			
			m_pCurrentState = pNewState;			
			m_pCurrentState.Enter(m_pOwner);
		}
		
		//change state back to the previous state
		public void RevertToPreviousState()
		{
			ChangeState(m_pPreviousState);
		}
		
		//accessors
		public State<entity_type> CurrentState
		{
			get 
			{ 
				return m_pCurrentState; 
			}
		}
		
		public State<entity_type> GlobalState
		{
			get 
			{ 
				return m_pGlobalState; 
			}
		}
		
		public State<entity_type> PreviousState
		{
			get 
			{ 
				return m_pPreviousState; 
			}
		}
		
		//check on the type of the current state
		public bool IsInState(State<entity_type> s)
		{
			bool answ = (s.GetType() == CurrentState.GetType());
			
			return answ;
		}
		
		public bool HandleMessage(Telegram msg)
		{
			//first see if the current state is valid and that it can handle
			//the message
			if (m_pCurrentState != null && m_pCurrentState.OnMessage(m_pOwner, msg))
			{
				return true;
			}
			
			//if not, and there's a global state, 
			//send it to the global state
			if (m_pGlobalState != null && m_pGlobalState.OnMessage(m_pOwner, msg))
			{
				return true;
			}
			
			return false;
		}
	}
}
