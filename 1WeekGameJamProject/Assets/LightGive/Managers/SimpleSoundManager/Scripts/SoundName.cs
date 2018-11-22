public static class SoundName
{
	public const string BGM_Endroll = "Endroll";
	public const string BGM_Main = "Main";
	public const string BGM_Main2 = "Main2";
	public const string BGM_Story1 = "Story1";
	public const string BGM_Story2 = "Story2";
	
	public const string SE_EnemyDamage = "EnemyDamage";
	public const string SE_EnemyDead = "EnemyDead";
	public const string SE_GameStartJingle = "GameStartJingle";
	public const string SE_Puyo = "Puyo";
	public const string SE_TextDisplay = "TextDisplay";
	public const string SE_Thunder = "Thunder";
}
	
public enum SoundNameBGM
{
	None,
	Endroll = 5,
	Main = 1,
	Main2 = 2,
	Story1 = 3,
	Story2 = 4,
}
	
public enum SoundNameSE
{
	None,
	EnemyDamage = 2,
	EnemyDead = 3,
	GameStartJingle = 5,
	Puyo = 1,
	TextDisplay = 4,
	Thunder = 6,
}
