namespace BloggieWeb.Models.ViewModels
{
    public class EditTagRequest
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Displayname { get; set; }
    }
}
