namespace Veterinaria.Domain.Comunes;

public class Resultado
{
    public bool EsExitoso { get; }
    public string Mensaje { get; }

    protected Resultado(bool esExitoso, string mensaje)
    {
        EsExitoso = esExitoso;
        Mensaje = mensaje;
    }

    public static Resultado Exito(string mensaje = "Operación realizada con éxito.") => new(true, mensaje);
    public static Resultado Falla(string mensaje) => new(false, mensaje);
}

public class Resultado<T> : Resultado
{
    public T? Valor { get; }

    private Resultado(bool esExitoso, T? valor, string mensaje) : base(esExitoso, mensaje)
    {
        Valor = valor;
    }

    public static Resultado<T> Exito(T valor, string mensaje = "Operación realizada con éxito.") => new(true, valor, mensaje);
    public new static Resultado<T> Falla(string mensaje) => new(false, default, mensaje);
}
