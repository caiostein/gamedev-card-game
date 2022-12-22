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

	public static IDictionary<int, string> levelHintDict = new Dictionary<int, string>()
	{
		{1, "Descrever Mecânica" },
		{2, "Descrever Narrativa" },
		{3, "Descrever Estética" },
		{4, "Descrever Tecnologia" },
	};


	public static IDictionary<int, string> MecanicaFeedbackDict = new Dictionary<int, string>()
	{
		{1, "Parece que suas ideias não estavam muito claras e você não conseguiu pensar em bons elementos para sua mecânica." },
		{2, "Bateu na trave! Você ficou perto de pensar em bons elementos para sua mecânica." },
		{3, "Boa! Você pensou em bons elementos para sua mecânica. Mas ainda poderia ter sido um pouco melhor." },
		{4, "Parabéns! Você teve ideias brilhantes para a mecânica do seu jogo. Você é um verdadeiro mestre das mecânicas" },
	};

	public static IDictionary<int, string> NarrativaFeedbackDict = new Dictionary<int, string>()
	{
		{1, "Parece que suas ideias não estavam muito claras e você não conseguiu pensar em bons elementos para sua narrativa." },
		{2, "Bateu na trave! Você ficou perto de pensar em bons elementos para sua narrativa." },
		{3, "Boa! Você pensou em bons elementos para sua narrativa. Mas ainda poderia ter sido um pouco melhor." },
		{4, "Parabéns! Você teve ideias brilhantes para a narrativa do seu jogo. Você é um excelente contador de histórias" },
	};

	public static IDictionary<int, string> EsteticaFeedbackDict = new Dictionary<int, string>()
	{
		{1, "Parece que suas ideias não estavam muito claras e você não conseguiu pensar em bons elementos para sua estética." },
		{2, "Bateu na trave! Você ficou perto de pensar em bons elementos para sua estética." },
		{3, "Boa! Você pensou em bons elementos para sua estética. Mas ainda poderia ter sido um pouco melhor." },
		{4, "Parabéns! Você teve ideias brilhantes para a estética do seu jogo. Você é um artista nato" },
	};

	public static IDictionary<int, string> TecnologiaFeedbackDict = new Dictionary<int, string>()
	{
		{1, "Parece que suas ideias não estavam muito claras e você não conseguiu pensar em bons elementos para sua tecnologia." },
		{2, "Bateu na trave! Você ficou perto de pensar em bons elementos para sua tecnologia." },
		{3, "Boa! Você pensou em bons elementos para sua tecnologia. Mas ainda poderia ter sido um pouco melhor." },
		{4, "Parabéns! Você teve ideias brilhantes para a tecnologia do seu jogo. Você é o verdadeiro gênio da tecnologia" },
	};

	public static IDictionary<int, string> FinalFeedbackDict = new Dictionary<int, string>()
	{
		{1, "LIXO SAFADO SE MATA BABACA." },
		{2, "Médio Gamer." },
		{3, "Bom Gamer." },
		{4, "Deus Gamer." },
	};
}
