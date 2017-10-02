/*
 * Created by SharpDevelop.
 * User: usr4
 * Date: 13.06.2015
 * Time: 12:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace AI1_The_West_World_Project
{
	/// <summary>
	/// Description of BaseGameEntity.
	/// </summary>
	public abstract class BaseGameEntity
	{
		//every game entity has a unique identifying number
		protected int m_ID;
		protected string name;
		
		//this is the next valid ID, updating after each instantiation of the BaseGameEntity
		private static int m_iNextValidID;
		private static bool firstTime = true;
		
		//check on the next valid ID
		void SetID(int val)
		{
			if (firstTime)
			{
				//first element
				firstTime = false;
				m_iNextValidID = 1;				
			}
			
			if (val >= m_iNextValidID)
			{
					m_ID = val;
					m_iNextValidID = val + 1;
			}
			else
				throw new Exception(val.ToString() + " can not be used as ID. The next valid ID is " + m_iNextValidID.ToString());
		}
		
		public int ID
		{
			get 
			{
				return m_ID;
			}
			
		}
		
		public string Name
		{
			get
			{
				return name;
			}
		}
		
		public BaseGameEntity(int id)
		{
			SetID(id);
			name = "Noname";
		}
		
		public BaseGameEntity(int id, string Name)
		{
			SetID(id);
			name = Name;
		}
		
		public abstract void Update();
		
		//all subclasses can communicate using messages	
		public abstract bool HandleMessage(Telegram msg);
		
	}
}
