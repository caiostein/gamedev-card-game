using System.Collections.Generic;

public class Enum
{
	public enum Levels
	{
		MECANICA = 1,
		NARRATIVA = 2,
		ESTETICA = 3,
		TECNOLOGIA = 4,
	}
	public enum CardEffects
	{
		METADINHA = 1,
		IDEIA = 2,
		TROCA = 3
	}

	public static IDictionary<int, string> levelDescriptionDict = new Dictionary<int, string>()
	{
		{1, "Monte a melhor mecânica possível para seu jogo!" },
		{2, "Monte a melhor narrativa possível para seu jogo!" },
		{3, "Monte a melhor estética possível para seu jogo!" },
		{4, "Monte a melhor tecnologia possível para seu jogo!" },
	};
}
