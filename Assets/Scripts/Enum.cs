using System.Collections.Generic;

public class Enum
{
	public enum ActiveScreen
    {
		MENU = 1,
		DIALOG = 2
    }
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
		{1, "Monte a melhor MECÂNICA possível para seu jogo!" },
		{2, "Monte a melhor NARRATIVA possível para seu jogo!" },
		{3, "Monte a melhor ESTÉTICA possível para seu jogo!" },
		{4, "Monte a melhor TECNOLOGIA possível para seu jogo!" },
	};

	public static IDictionary<int, string> levelHintDict = new Dictionary<int, string>()
	{
		{1, "Neste nível você deve escolher cartas que possuam uma maior afinidade com MECÂNICA" },
		{2, "Neste nível você deve escolher cartas que possuam uma maior afinidade com NARRATIVA" },
		{3, "Neste nível você deve escolher cartas que possuam uma maior afinidade com ESTÉTICA" },
		{4, "Neste nível você deve escolher cartas que possuam uma maior afinidade com TECNOLOGIA" },
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
		{1, "Eita! Parece que você não conseguiu ir bem na etapa de brainstorm." },
		{2, "Você foi bem, mas poderia ter sido melhor. Mais um pouco e você teria pensado em bons elementos para compor o seu jogo." },
		{3, "Muito bem! Você teve boas ideias para o seu jogo!" },
		{4, "Incrível!\r\nVocê mostrou que entende a Tétrade de Schell e foi brilhante nessa etapa de brainstorm. Você está pronto para começar a desenvolver o seu jogo." },
	};

	public static readonly IDictionary<int, string> dialogLines = new Dictionary<int, string>()
	{
		{1, "" },
		{2, "Você chega à Feira de Games da sua região" },
		{3, "Você está ansioso para participar de sua primeira Game Jam e mostrar ao mundo a sua capacidade de criar jogos incríveis" },
		{4, "Logo você encontra um funcionário que te recebe para a competição" },
		{5, "Olá! Que bom que você chegou!\r\nMe diga: como você se chama?" },
		{6, ""  },
		{7, "Olá [PlayerName]! Bem vindo ao game jam. Logo logo você já vai estar criando o seu jogo. Mas antes vamos falar sobre as regras da competição." },
		{8, "Recomendo que, caso nunca tenha participado, leia as regras mais detalhadas neste panfleto de 'Regras'." },
		{9, "Você deve criar seu jogo a partir de certos elementos que se relacionam com 4 categorias distintas. Essas categorias são os pilares da tétrade de Schell." },
		{10, "Para isso você fará um brainstorm a partir de cartas que te apresentarão diferentes elementos de jogos. Como por exemplo: Pulo Duplo, Pixel Art... Então você deve escolher 4 cartas para cada categoria." },
		{11, "Mas fique ligado, as cartas tem uma certa afinidade para cada categoria da tétrade. Você será avaliado de acordo com as cartas que escolher, portanto escolha com sabedoria." },
		{12, "Boa sorte e bom jogo!"},
	};
}