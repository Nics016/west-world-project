/*
 * Created by SharpDevelop.
 * User: usr4
 * Date: 23.06.2015
 * Time: 17:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace AI1_The_West_World_Project
{
	/// <summary>
	/// Description of OrderItem.
	/// </summary>
	
	public enum food_type
	{
		BigTasty,
		CocaCola,
		Free,
		IceCream
	}
	
	public class OrderItem
	{
		private string itemName;
		private food_type item_type;
		
		public OrderItem(food_type item)
		{
			item_type = item;
			
			switch (item)
			{
				case food_type.BigTasty:
					itemName = "Биг тейсти";
					break;
					
				case food_type.CocaCola:
					itemName = "Кока-кола";
					break;
					
				case food_type.Free:
					itemName = "Картошка 'фри'";
					break;
					
				case food_type.IceCream:
					itemName = "Мороженое";
					break;
			}
		}
		
		public string ItemName
		{
			get
			{
				return itemName;
			}
		}
		
		public food_type Type
		{
			get
			{
				return item_type;
			}
		}
	}
}
