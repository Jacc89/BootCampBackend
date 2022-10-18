namespace Core.Especificaciones
{
    public class MetaData
    {
        public int CurrentPage { get; set; } // pagina actual
        public int TotalPages { get; set; } //total paginas
        public int PageSize { get; set; } //tamaño de pagina
        public int TotalCount  { get; set; } //Total de registro

        public bool HasPrevius => CurrentPage > 1; //paginas previas
        public bool HasNext => CurrentPage < TotalPages;  //paginas siguientes
    }
}