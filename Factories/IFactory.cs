using quoteRedux.Models;
namespace quoteRedux.Factory
{
    //below is used to interface factories, or whatever extends : IFactory
    //i.e. 
    public interface IFactory<T> where T : BaseEntity
    {
        
    }
}