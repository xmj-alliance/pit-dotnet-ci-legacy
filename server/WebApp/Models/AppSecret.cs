namespace WebApp.Models
{
  public class AppSecret : IAppSecret {
    public string Title { get; set; }
    public string Description { get; set; }
    public string Value { get; set; }
    public bool IsHighlyClassified { get; set; }
    public int Level { get; set; }
  }
}