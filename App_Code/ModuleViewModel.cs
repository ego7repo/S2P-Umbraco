using Umbraco.Core.Models;

public class ModuleViewModel
{
     public IPublishedContent Content { get; set; }
     public IPublishedContent CurrentPage { get; set; }

     public ModuleViewModel(IPublishedContent content, IPublishedContent currentPage) 
     {
         Content = content;
         CurrentPage = currentPage;
     }
}