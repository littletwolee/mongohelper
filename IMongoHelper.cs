using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace pwc_mongo_helper
{
    public interface IMongoHelper<T>
    {
        /// <summary>
        /// Insert Document
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>string objectid</returns>
        string Insert(object entity);
        /// <summary>
        /// Push one into Documentlist
        /// </summary>
        /// <param name="id">string objectid</param>
        /// <param name="filed">lamabad expression</param>
        /// <param name="value">push entity</param>
        /// <returns>true or false</returns>
        bool Push(string id, Expression<Func<T, IEnumerable<object>>> filed, object value);
        /// <summary>
        /// Pull one from Documentlist
        /// </summary>
        /// <param name="id">string objectid</param>
        /// <param name="filed">lamabad expression</param>
        /// <param name="expression">lamabad expression</param>
        /// <returns>true or false</returns>
        bool Pull<C>(string id, Expression<Func<T, IEnumerable<C>>> filed, Expression<Func<C, bool>> expression);
        /// <summary>
        /// Replace expression
        /// </summary>
        /// <param name="id">string objectid</param>
        /// <param name="entity">replace entity</param>
        /// <returns>true or false</returns>
        bool Replace(string id, object entity);
        /// <summary>
        /// Update Document
        /// </summary>
        /// <param name="id">string objectid</param>
        /// <param name="updatedic">update Dictionary</param>
        /// <returns>true or false</returns>
        bool Update(string id, Dictionary<string, object> updatedic);
        /// <summary>
        /// Delete Document
        /// </summary>
        /// <param name="id">string objectid</param>
        /// <returns>true or false</returns>
        bool Delete(string id);
        /// <summary>
        /// FindOne Documents
        /// </summary>
        /// <param name="id">string objectid</param>
        /// <param name="fields">display fields</param>
        /// <returns>T</returns>
        Task<T> FindOne(string id,string[] fields = null);
        /// <summary>
        /// Find Documents
        /// </summary>
        /// <param name="fields">display fields</param>
        /// <param name="sortfields">sort fields(1 is Ascending 0 is Descending)</param>
        /// <param name="expression">lamabad expression</param>
        /// <returns>List<T></returns>
        Task<List<T>> Find(string[] fields, Dictionary<string, int> sortfields = null, System.Linq.Expressions.Expression<Func<T, bool>> expression = null);
    }
}
