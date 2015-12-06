using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLog {

	public static string kGameName = "Restaurante na Esquina do Universo";

	public static string kProgressLoaded = "O jogo foi carregado.";
	public static string kProgressSaved = "O jogo foi salvo.";
	public static string kTDayIsOver = "O dia terminou.";
	public static string kTGameOver = "Você perdeu!";
	public static string kTNoCashBuyIngredient = "Não há dinheiro suficiênte para comprar o ingrediente.";
	public static string kTNoCashBuyEquipment = "Não há dinheiro suficiênte para comprar o equipamento.";
	public static string kTNoCashHireAdvertisement = "Não há dinheiro suficiênte para fazer propaganda.";
	public static string kTNoCashHireEmployee = "Não há dinheiro suficiênte para contratar o empregado.";
	public static string kTNoCashDismissEmployee = "Não há dinheiro suficiênte para demitir o empregado.";
	public static string kTNoCashTrainEmployee = "Não há dinheiro suficiênte para treinar o empregado.";
	public static string kTNoCashDoCleaning = "Não há dinheiro suficiênte para fazer a limpeza.";	

	public static string kTNoPointsBuyIngredient = "Não há pontos de ação suficiêntes para comprar o ingrediente.";
	public static string kTNoPointsHireEquipment = "Não há pontos de ação suficiêntes para comprar o equipament.";
	public static string kTNoPointsHireAdvertisement = "Não há pontos de ação suficiêntes para fazer propaganda.";
	public static string kTNoPointsHireEmployee = "Não há pontos de ação suficiêntes para contratar o empregado.";
	public static string kTNoPointsDismissEmployee = "Não há pontos de ação suficiêntes para demitir o empregado.";
	public static string kTNoPointsTrainEmployee = "Não há pontos de ação suficiêntes para treinar o empregado.";
	public static string kTNoPointsDoCleaning = "Não há pontos de ação suficiêntes para fazer limpeza.";

	public static string kTEmployeeAlreadyTrained = "O empregado já foi treinado";

	public static string kTIngredientAquired = "Ingrediente adquirido.";
	public static string kTEquipmentAquired = "Equipamento adquirido.";
	public static string kTEmployeeHired = "Empregado contratado.";
	public static string kTEmployeeTrained = "Empregado realizou treinamento.";
	public static string kTEmployeeDismissed = "Empregado foi demitido.";
	public static string kTCleaningDone = "Limpeza realizada.";

	public static string kTClientOrder = "Cliente pediu um ";
	public static string kTClientOrderAttended = "Cliente foi atendido.";
	public static string kTClientOrderNotAttended = "Cliente não foi atendido.";
	public static string kTDishCannotBePrepared = "Nenhum Chef pode preparar o prato.";
	public static string kTClientsLeft = " clientes não conseguiram fazer o pedido.";
	public static string kTOutOfIngredients = "Não há ingredientes suficiênte para preparar o prato.";
	public static string kTClientWaitTooLong = "O cliente esperou de mais e foi embora.";

	public static string kTDirtnessIncreased = "O nível de sujeira subiu: ";
	public static string kTDirtnessDecreased = "O nível de sujeira caiu: ";
	public static string kTSatisfactionIncreased = "A satisfação dos clientes subiu: ";
	public static string kTSatisfactionDecreased = "A satisfação dos clientes caiu ";
	public static string kTEmployeesHappinessIncreased = "A felicidade dos empregados subiu: ";
	public static string kTEmployeesHappinessDecreased = "A felicidade dos empregados caiu: ";
	public static string kTEmployeesSkillIncreasedForType = "O nível de skill subiu: ";
	
	public static string kTRottenIngredientsDiscarted = "Os ingredientes ficaram podres e foram descartados ";
	public static string kTAdvertisementsOver = "Os anúncios contratados acabaram.";
	public static string kTSalariesPayment = "O pagamento do salário dos empregados foi realizado.";
	public static string kTIngredientSpend = "Ingrediente gasto: ";
	public static string kTPricesAlreadyChanged = "Preços só podem ser alterados uma vez por rodada.";

	public static string kTNotEnoughPoints = "Não há pontos suficiêntes para completar a ação.";
	public static string kTNotEnoughtMoney = "Não há dinheiro suficiênte para completar a ação.";
	public static string kTInventoryOutOfSpace = "Não há espaço na dispensa. Limite de itens: ";
	public static string kTStorageTimeDecreased = "Tempo de armazenamento diminuiu para: ";
	public static string kTStorageTimeIncreased = "Tempo de armazenamento subiu para: ";

	public static void Log(string token){
		logs.Add (token);
	}
	public static void Log(string token, string value){
		logs.Add (token + value);
	}

	public static List<string> logs = new List<string>(); 
}
