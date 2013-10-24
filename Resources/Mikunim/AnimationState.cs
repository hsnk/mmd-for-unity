using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class AnimationState
{
	public string name;

	public MikunimState invoker;

	public Rect position;

	public AnimationState[] nexts;

	public AnimationState parent;
}