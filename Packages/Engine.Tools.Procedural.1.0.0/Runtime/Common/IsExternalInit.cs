namespace Engine.Procedural {
	/// <summary>
	/// This is currently a limitation of Unity when using Span
	/// Unity throws a compile error stating IsExternalInit is missing
	/// The simple solution to this is to create this interface
	/// </summary>
	public class IsExternalInit {
	}
	
	public interface IIsExternalInit{}
}