public interface IRange<TValue> where TValue : struct
{
	TValue MinValue
	{
		get;
		set;
	}

	TValue MaxValue
	{
		get;
		set;
	}

	TValue MidValue
	{
		get;
	}

	TValue Length
	{
		get;
	}

	TValue RandomValue
	{
		get;
	}
}