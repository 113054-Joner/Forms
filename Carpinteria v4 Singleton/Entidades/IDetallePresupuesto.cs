namespace Carpinteria.Entidades
{
    public interface IDetallePresupuesto
    {
        int cantidad { get; set; }
        Producto producto { get; set; }

        float CalcularSubtotal();
    }
}