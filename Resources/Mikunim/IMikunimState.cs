using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Mikunimの状態を表すためのインターフェース
/// </summary>
public interface IAnimationState
{
	string Condition();
}

public abstract class MikunimState : MonoBehaviour, IAnimationState
{
	public abstract string Condition();
}