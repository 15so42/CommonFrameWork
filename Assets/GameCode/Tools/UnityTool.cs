using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;

//
//次工具类主要对Gamobject进行操作，包括添加子物体，在指定位置添加子物体，寻找物体和子物体。
//
public static class UnityTool
{
	// 附加GameObject
	public static void Attach( GameObject ParentObj, GameObject ChildObj, Vector3 Pos )
	{
		ChildObj.transform.parent = ParentObj.transform;
		ChildObj.transform.localPosition = Pos;
	}

	// 附加GameObject
	public static void AttachToRefPos( GameObject ParentObj, GameObject ChildObj,string RefPointName,Vector3 Pos )
	{
		// Search 
		bool bFinded=false;
		Transform[] allChildren = ParentObj.transform.GetComponentsInChildren<Transform>();
		Transform RefTransform = null;
		foreach (Transform child in allChildren)
		{            
			if (child.name == RefPointName)
			{                
				if (bFinded == true)
				{
					Debug.LogWarning("物件["+ParentObj.transform.name+"]內有兩個以上的參考點["+RefPointName+"]");
					continue;
				}
				bFinded = true;
				RefTransform = child;
			}
		}
		
		//  是否找到
		if (bFinded == false)
		{
			Debug.LogWarning("物件["+ParentObj.transform.name+"]內沒有參考點["+RefPointName+"]");
			Attach( ParentObj,ChildObj,Pos);
			return ;
		}

		ChildObj.transform.parent = RefTransform;
		ChildObj.transform.localPosition = Pos;
		ChildObj.transform.localScale = Vector3.one;
		ChildObj.transform.localRotation = Quaternion.Euler( Vector3.zero);				
	}
	
	// 找到場景上的物件
	public static GameObject FindGameObject(string GameObjectName)
	{
		// 找出對應的GameObject
		GameObject pTmpGameObj = GameObject.Find(GameObjectName);
		if(pTmpGameObj==null)
		{
			Debug.LogWarning("场景中找不到GameObject["+GameObjectName+"]物件");
			return null;
		}
		return pTmpGameObj;
	}

	// 取得子物件
	public static GameObject FindChildGameObject(GameObject Container, string gameobjectName)
	{
		if (Container == null)
		{
			Debug.LogError("查找名为"+gameobjectName+"的子物体时发现父物体为空，无法查找");
			return null;
		}
		
		Transform pGameObjectTF=null; //= Container.transform.FindChild(gameobjectName);											

				
		// 是不是Container本身
		if(Container.name == gameobjectName)			
			pGameObjectTF=Container.transform;
		else
		{	
			// 找出所有子元件						
			Transform[] allChildren = Container.transform.GetComponentsInChildren<Transform>();
			foreach (Transform child in allChildren)			
			{            
				if (child.name == gameobjectName)
				{
					if(pGameObjectTF==null)					
						pGameObjectTF=child;
					else
						Debug.LogWarning("Container["+Container.name+"]下找出重覆的物体名稱["+gameobjectName+"]");
				}
			}
		}
		
		// 都沒有找到
		if(pGameObjectTF==null)			
		{
			Debug.LogError("容器["+Container.name+"]找不到子物体["+gameobjectName+"]");		
			return null;
		}
		
		return pGameObjectTF.gameObject;			
	}

	public static int RandomIndexFrom<T>(T t) where T:System.Collections.ICollection
	{
		var index = UnityEngine.Random.Range(0, t.Count);
		return index;
	}
	
	public static T RandomElement<T>(this ICollection<T> collection)
	{
		if (collection == null || collection.Count == 0)
		{
			throw new ArgumentException("Collection cannot be null or empty.", nameof(collection));
		}

		int index = UnityEngine.Random.Range(0, collection.Count);

		if (collection is IList<T> list)
		{
			return list[index];
		}
		else
		{
			foreach (var item in collection)
			{
				if (index-- == 0)
					return item;
			}
			throw new InvalidOperationException("Index calculation error.");
		}
	}
	
	/// <summary>
	/// 从列表中寻找最近的元素
	/// </summary>
	/// <param name="gameObjects"></param>
	/// <param name="currentTransform"></param>
	/// <returns></returns>
	public static GameObject FindClosestGameObject(GameObject[] gameObjects, Transform currentTransform) 
	{
		int closestIndex = 0;
		float closestDistance = Mathf.Infinity;
		Vector3 currentPosition = currentTransform.position;

		// 遍历数组中的所有GameObject，并找到最近的一个
		for (int i = 0; i < gameObjects.Length; i++) 
		{
			float distance = Vector3.Distance(currentPosition, gameObjects[i].transform.position);
			if (distance < closestDistance) 
			{
				closestIndex = i;
				closestDistance = distance;
			}
		}

		// 返回最近的GameObject
		return gameObjects[closestIndex];
	}


	

	
	// 递归查找所有子物体以及所有层级的子物体
	public static Transform FindDeepChild(this Transform parent, string name)
	{
		if (parent.name == name)
			return parent;
		
		// 检查当前层级的子物体
		Transform result = parent.Find(name);
		if (result != null)
			return result;

		// 递归查找下一层级的子物体
		foreach (Transform child in parent)
		{
			result = child.FindDeepChild(name);
			if (result != null)
				return result;
		}

		// 如果在所有子物体中都没有找到，返回null
		return null;
	}
	
	
	/// <summary>
	/// 获取时间戳（精确到秒）
	/// TimeTool.ConvertDateTimep(DateTime.Now)
	/// </summary>
	/// <param name="time">时间</param>
	public static long ConvertDateTimeToUnix(DateTime time)
	{
		return ((time.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
		//等价于：
		//return ((time.ToUniversalTime().Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks) / 10000000) * 1000;
	}
	/// <summary>
	/// 时间戳转为C#格式时间
	/// TimeTool.GetTime(TimeTool.ConvertDateTiemp(DateTime.Now).ToString())
	/// </summary>
	/// <param name="timeStamp">时间戳</param>
	/// <returns></returns>
	public static DateTime GetTime(string timeStamp)
	{
		if (timeStamp.Length > 10)
		{
			timeStamp = timeStamp.Substring(0, 10);
		}
		DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
		long lTime = long.Parse(timeStamp + "0000000");
		TimeSpan toNow = new TimeSpan(lTime);
		return dateTimeStart.Add(toNow);
	}


	/// <summary>
	/// 获取IpV4地址
	/// </summary>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	public static string GetLocalIPAddress()
	{
		IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (IPAddress ip in host.AddressList)
		{
			// 使用IPv4地址，忽略IPv6地址
			if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
			{
				return ip.ToString();
			}
		}
		throw new System.Exception("No network adapters with an IPv4 address in the system!");
	}
}
