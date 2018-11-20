public static class SoundName
{
	public const string BGM_Main = "Main";
	
	public const string SE_EnemyDamage = "EnemyDamage";
	public const string SE_EnemyDead = "EnemyDead";
	public const string SE_Puyo = "Puyo";
	public const string SE_TextDisplay = "TextDisplay";
}
	
public enum SoundNameBGM
{
	None,
	Main = 1,
}
	
public enum SoundNameSE
{
	None,
	EnemyDamage = 2,
	EnemyDead = 3,
	Puyo = 1,
	TextDisplay = 4,
}
