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
		TROCA = 3,
		INSPIRACAO = 4
	}

	public static IDictionary<int, string> levelDescriptionDict = new Dictionary<int, string>()
	{
		{1, "Monte a melhor mecânica possível para seu jogo!" },
		{2, "Monte a melhor narrativa possível para seu jogo!" },
		{3, "Monte a melhor estética possível para seu jogo!" },
		{4, "Monte a melhor tecnologia possível para seu jogo!" },
	};

	public static IDictionary<int, string> MecanicaFeedbackDict = new Dictionary<int, string>()
	{
		{1, "LIXO filho" },
		{2, "Médio filho" },
		{3, "OK filho" },
		{4, "Brabo PICA FODA" },
	};

	public static IDictionary<int, string> NarrativaFeedbackDict = new Dictionary<int, string>()
	{
		{1, "Ruim narra" },
		{2, "Médio narra" },
		{3, "OK narra" },
		{4, "Brabo narra" },
	};

	public static IDictionary<int, string> EsteticaFeedbackDict = new Dictionary<int, string>()
	{
		{1, "Ruim estet" },
		{2, "Médio estet" },
		{3, "OK estet" },
		{4, "Brabo estet" },
	};

	public static IDictionary<int, string> TecnologiaFeedbackDict = new Dictionary<int, string>()
	{
		{1, "Ruim tec" },
		{2, "Médio tec" },
		{3, "OK tec" },
		{4, "Brabo tec" },
	};
}
