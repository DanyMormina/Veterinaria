namespace Veterinaria.Domain.Comunes;

public class Resultado
{
    public bool EsExitoso { get; }
    public string Mensaje { get; }

    public bool IsSuccess => EsExitoso;
    public string Message => Mensaje;

    protected Resultado(bool esExitoso, string mensaje)
    {
        EsExitoso = esExitoso;
        Mensaje = mensaje;
    }

    public static Resultado Exito(string mensaje = "Operación realizada con éxito.") => new(true, mensaje);
    public static Resultado Ok(string mensaje = "Operación realizada con éxito.") => Exito(mensaje);
    public static Resultado Falla(string mensaje) => new(false, mensaje);
}

public class Resultado<T> : Resultado
{
    public T? Valor { get; }

    public T? Value => Valor;
    public T? Data => Valor;

    protected Resultado(bool esExitoso, T? valor, string mensaje) : base(esExitoso, mensaje)
    {
        Valor = valor;
    }

    public static Resultado<T> Exito(T valor, string mensaje = "Operación realizada con éxito.") => new(true, valor, mensaje);
    public static Resultado<T> Ok(T valor, string mensaje = "Operación realizada con éxito.") => Exito(valor, mensaje);
    public new static Resultado<T> Falla(string mensaje) => new(false, default, mensaje);
}

public class Result : Resultado
{
    protected Result(bool esExitoso, string mensaje) : base(esExitoso, mensaje) { }

    public static new Result Ok(string mensaje = "Operación realizada con éxito.") => new(true, mensaje);
    public static new Result Falla(string mensaje) => new(false, mensaje);
}

public class Result<T> : Resultado<T>
{
    protected Result(bool esExitoso, T? valor, string mensaje) : base(esExitoso, valor, mensaje) { }

    public static new Result<T> Ok(T valor, string mensaje = "Operación realizada con éxito.") => new(true, valor, mensaje);
    public static new Result<T> Falla(string mensaje) => new(false, default, mensaje);
}
