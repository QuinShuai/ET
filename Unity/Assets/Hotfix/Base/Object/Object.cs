namespace ETHotfix
{
	public interface IDisposable
	{
		void Dispose();
	}

	public interface ISupportInitialize
	{
		void BeginInitAsync();
		void EndInit();
	}

	public abstract class Object: ISupportInitialize
	{
		public virtual void BeginInitAsync()
		{
		}

		public virtual void EndInit()
		{
		}

		public override string ToString()
		{
			return JsonHelper.ToJson(this);
		}
	}
}