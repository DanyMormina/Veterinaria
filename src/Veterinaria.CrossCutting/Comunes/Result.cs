namespace Veterinaria.CrossCutting.Comunes;

public class Result
{
    public bool EsExitoso { get; }
    public string Mensaje { get; }
    protected Result(bool esExitoso, string mensaje)
    {
        EsExitoso = esExitoso;
        Mensaje = mensaje;
    }
    public static Result Ok(string mensaje = "Operación realizada con éxito.") => new(true, mensaje);
    public static Result Falla(string mensaje) => new(false, mensaje);
}
public class Result<T> : Result
{
    public T? Valor { get; }
    private Result(bool esExitoso, T? valor, string mensaje) : base(esExitoso, mensaje)
    {
        Valor = valor;
    }
    public static Result<T> Ok(T valor, string mensaje = "Operación realizada con éxito.") => new(true, valor, mensaje);
    public new static Result<T> Falla(string mensaje) => new(false, default, mensaje);
}