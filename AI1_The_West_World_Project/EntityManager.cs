/*
 * Created by SharpDevelop.
 * User: usr4
 * Date: 06.08.2015
 * Time: 23:05
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;

namespace AI1_The_West_World_Project
{
	/// <summary>
	/// Description of EntityManager.
	/// </summary>
		public class EntityManager
	{
		private ArrayList m_EntityMap;
		
		private EntityManager() 
		{
			m_EntityMap = new ArrayList();
		}
		
		private static EntityManager instance;
		
		public static EntityManager Instance()
		{
			if (instance == null)
				instance = new EntityManager();
			
			return instance;
		}
		
		public void RegisterEntity(BaseGameEntity NewEntity)
		{
			m_EntityMap.Add(NewEntity);
		}
		
		public BaseGameEntity GetEntityFromID(int id)
		{
			BaseGameEntity answer = null;
			
			foreach (BaseGameEntity entity in m_EntityMap)
			{
				if (entity.ID == id)
				{
					answer = entity;
					break;
				}
			}
			
			return answer;
		}
		
		public string GetNameOfEntity(int ID)
		{
			string answ = "Entity was not found";
			
			foreach (BaseGameEntity entity in m_EntityMap)
			{
				if (entity.ID == ID)
				{
					answ = entity.Name;
					break;
				}
			}
			
			return (answ);
		}
		
		public void RemoveEntity(BaseGameEntity pEntity)
		{
			m_EntityMap.Remove(pEntity);
		}
	}
	

}
