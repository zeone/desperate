using UnityEngine;
using System.Collections;

//клавный класс контрлер, от него дочерний игрок и виртуальный интелект
public class GameController : Actor
{
	[HideInInspector]
	public Vector3 facingDirection;
	[HideInInspector]
	public Vector3 movementDirection;
	[HideInInspector]
	public Vector3 aim;

}
