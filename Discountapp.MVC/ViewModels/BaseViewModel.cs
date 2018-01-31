namespace Discountapp.MVC.ViewModels
{
    public abstract class BaseViewModel
    {
        public virtual string Description { get; set; }
        public virtual string Information { get; set; }
        public virtual string ShortInformation => this.MakeShort(this.Information);
        public virtual string ShortDescription => this.MakeShort(this.Description);

        private string MakeShort(string text, int length = 100)
        {
            var result = text ?? string.Empty;
            if(result.Length > length)
            {
                result = result.Substring(0, length);
                result += "...";
            }
            return result;
        }
    }
}