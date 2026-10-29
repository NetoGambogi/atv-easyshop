namespace EasyShop.Services;

/// <summary>Erro de regra de negócio, com mensagem pronta para exibir ao usuário.</summary>
public class ValidationException(string message) : Exception(message);
