namespace Restaurante.Model
{
    /// <summary>
    /// Estados posibles de una mesa según el diseño de la base de datos.
    /// Los valores numéricos coinciden con la columna Estado de la tabla Mesas
    /// y NO deben modificarse.
    /// </summary>
    public enum EstadoMesa
    {
        Disponible = 1,
        Reservada = 2
    }
}
