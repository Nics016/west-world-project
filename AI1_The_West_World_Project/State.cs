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
	/// Description of State.
	/// </summary>
	
	public abstract class State<entity_type>
	{			
		//public State<entity_type> Instance();
		protected const int SEND_MSG_IMMEDIATELY = 0;
		
		public abstract void Enter(entity_type entity);
		
		public abstract void Execute(entity_type entity);
		
		public abstract void Exit(entity_type entity);	
		
		//this executes if the agent receives a message from 
		//the message dispatcher
		public abstract bool OnMessage(entity_type entity, Telegram msg);
		
	}
	
	public interface
}
