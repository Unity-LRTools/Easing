using System;
using UnityEngine;

namespace LRT.Easing
{
	public static class Converter
	{
		public enum RoundingMode
		{
			Round,
			Floor,
			Ceil,
		}

		public static int FloatToInt(float value, RoundingMode mode)
		{
			return mode switch
			{
				RoundingMode.Ceil => Mathf.CeilToInt(value),
				RoundingMode.Floor => Mathf.FloorToInt(value),
				_ => Mathf.RoundToInt(value)
			};
		}
	}

	/// <summary>
	/// Represents a mathematical function that describes the rate at which a numerical
	/// value changes.
	/// <br/>
	/// Easing curves present on: easings.net
	/// </summary>
	/// Note that the order of the easing matter when we retrieve it in <see cref="EasingExtension.Ease(Ease, int)"/>
	public enum Ease
	{
		Linear,         
		InSine,         
		OutSine,        
		InOutSine,      
		InCubic,        
		OutCubic,       
		InOutCubic,     
		InQuart,        
		OutQuart,       
		InOutQuart,     
		InQuint,        
		OutQuint,       
		InOutQuint,     
		InExpo,         
		OutExpo,        
		InOutExpo,      
		InCirc,         
		OutCirc,        
		InOutCirc,      
		InBack,         
		OutBack,        
		InOutBack,      
		InElastic,      
		OutElastic,     
		InOutElastic,   
		InBounce,       
		OutBounce,      
		InOutBounce,    
	}

	public static class EasingExtension
	{
		private static Func<float, float>[] easings = new Func<float, float>[]
		{
				(x) => x,																			// Linear
				(x) => 1 - Mathf.Cos((x * Mathf.PI) / 2),											// In	 Sine
				(x) => Mathf.Sin((x * Mathf.PI) / 2),												// Out	 
				(x) => -(Mathf.Cos(Mathf.PI * x) - 1) / 2,											// InOut 
				(x) =>  x * x * x,																	// In	 Cubic
				(x) => 1 - Mathf.Pow(1 - x, 3),														// Out	 
				(x) => x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2,					// InOut 
				(x) => x * x * x * x,																// In	 Quart
				(x) => 1 - Mathf.Pow(1 - x, 4),														// Out	 
				(x) => x < 0.5 ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2,				// InOut 
				(x) => x * x * x * x * x,															// In	 Quint
				(x) => 1 - Mathf.Pow(1 - x, 5),														// Out	 
				(x) => x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2,			// InOut
				(x) => x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10),										// In	 Expo
				(x) => x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x),										// Out
				(x) => InOutExpo(x),																// InOut
				(x) => 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2)),											// In	 Circ
				(x) => Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2)),											// Out
				(x) => InOutCirc(x),																// InOut
				(x) => InBack(x),																	// In	 Back
				(x) => OutBack(x),																	// Out
				(x) => InOutBack(x),																// InOut
				(x) => InElastic(x),																// In	 Elastic
				(x) => OutElastic(x),																// Out
				(x) => InOutElastic(x),																// InOut
				(x) => 1 - OutBounce(1 - x),														// In	 Bounce
				(x) => OutBounce(x),																// Out	 
				(x) => x < 0.5 ? (1 - OutBounce(1 - 2 * x)) / 2 : (1 + OutBounce(2 * x - 1)) / 2,	// InOut 
		};

		/// <summary>
		/// The value will be eased following the selected easing curve.
		/// </summary>
		/// <param name="ease">The easing curve</param>
		/// <param name="value">The normalized value, between [0..1]</param>
		/// <param name="mode">Optionnal rounding mode</param>
		/// <returns>The eased value</returns>
		public static int Evaluate(this Ease ease, int value, Converter.RoundingMode mode = Converter.RoundingMode.Round)
		{
			float evaluatedValue = easings[(int)ease](value);

			return Converter.FloatToInt(evaluatedValue, mode);
		}

		/// <summary>
		/// The value will be eased following the selected easing curve.
		/// </summary>
		/// <param name="ease">The easing curve</param>
		/// <param name="value">The normalized value, between [0..1]</param>
		/// <returns>The eased value</returns>
		public static float Evaluate(this Ease ease, float value)
		{
			return easings[(int)ease](value);
		}

		#region Easing methods
		private static float InOutExpo(float x)
		{
			return x == 0 ?
				0 : x == 1 ?
				1 : x < 0.5 ? Mathf.Pow(2, 20 * x - 10) / 2 : (2 - Mathf.Pow(2, -20 * x + 10)) / 2;
		}

		private static float InOutCirc(float x)
		{
			return x < 0.5 ?
				(1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2
			  : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2;
		}

		private static float InBack(float x)
		{
			const float c1 = 1.70158f;
			const float c3 = c1 + 1;

			return c3 * Mathf.Pow(x, 3) - c1 * Mathf.Pow(x, 2);
		}

		private static float OutBack(float x)
		{
			const float c1 = 1.70158f;
			const float c3 = c1 + 1;

			return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
		}

		private static float InOutBack(float x)
		{
			const float c1 = 1.70158f;
			const float c2 = c1 * 1.525f;

			return x < 0.5
			  ? (Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
			  : (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
		}

		private static float InElastic(float x)
		{
			const float c4 = (2 * Mathf.PI) / 3;

			return x == 0 ?
				0 : x == 1 ?
				1 : -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10 - 10.75f) * c4);
		}

		private static float OutElastic(float x)
		{
			const float c4 = (2 * Mathf.PI) / 3;

			return x == 0 ?
				0 : x == 1 ?
				1 : Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * c4) + 1;
		}

		private static float InOutElastic(float x)
		{
			const float c5 = (2 * Mathf.PI) / 4.5f;

			return x == 0
			  ? 0
			  : x == 1
			  ? 1
			  : x < 0.5
			  ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2
			  : (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2 + 1;
		}

		private static float OutBounce(float x)
		{
			float n1 = 7.5625f;
			float d1 = 2.75f;

			if (x < 1 / d1)
				return n1 * x * x;
			else if (x < 2 / d1)
				return n1 * (x -= 1.5f / d1) * x + 0.75f;
			else if (x < 2.5 / d1)
				return n1 * (x -= 2.25f / d1) * x + 0.9375f;
			else
				return n1 * (x -= 2.625f / d1) * x + 0.984375f;
		}
		#endregion
	}
}