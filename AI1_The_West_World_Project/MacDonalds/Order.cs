/*
 * Created by SharpDevelop.
 * User: usr4
 * Date: 23.06.2015
 * Time: 17:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;

namespace AI1_The_West_World_Project
{
	/// <summary>
	/// Description of Order.
	/// </summary>
	public class Order
	{
		private ArrayList OrderItems; 
		
		public Order()
		{
			OrderItems = new ArrayList();
		}
		
		public void AddItem(OrderItem item)
		{
			OrderItems.Add(item);
		}
		
		public OrderItem GetNextDish()
		{
			OrderItem answ = null; //returning null means that there are no more dishes in the order
			
			if (OrderItems.Count > 0)
			{
				answ = (OrderItem)OrderItems[0];
				OrderItems.Remove(answ);
			}
			
			return answ;
		}
		
		public Order Clone()
		{
			Order answ = new Order();
			
			foreach (OrderItem item in OrderItems)
			{
				answ.AddItem(item);
			}
			
			return answ;
		}
	}
}
