/*
 * Created by SharpDevelop.
 * User: usr4
 * Date: 24.06.2015
 * Time: 12:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace AI1_The_West_World_Project
{
	/// <summary>
	/// Description of AdditionalInfo.
	/// </summary>
	public class AdditionalInfo
	{
		private Order order;
		
		public AdditionalInfo(Order pOrder)
		{
			order = pOrder;
		}
		
		public Order GetOrder
		{
			get 
			{
				return order;
			}
		}
	}
}
