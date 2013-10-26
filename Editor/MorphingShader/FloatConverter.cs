using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FloatConverter
{
	/*
	 * 以下を参考
	 * http://www.platinumgames.co.jp/programmer_blog/?p=484
	 * http://www.platinumgames.co.jp/programmer_blog/?p=594
	 */
	public static byte[] Convert16bit(float value)
	{
		Half half = new Half(value);
		return Half.GetBytes(half);
	}
}
