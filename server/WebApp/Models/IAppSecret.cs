namespace WebApp.Models
{
  public interface IAppSecret
  {
    string Title { get; set; }
    string Description { get; set; }
    string Value { get; set; }
    bool IsHighlyClassified { get; set; }
    int Level { get; set; }

  }
}